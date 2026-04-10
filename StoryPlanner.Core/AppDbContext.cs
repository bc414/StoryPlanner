using Microsoft.EntityFrameworkCore;
using StoryPlanner.Core.Models;

namespace StoryPlanner.Core;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // --- Tables ---
    public DbSet<Chapter> Chapters { get; set; }
    public DbSet<StoryThread> Threads { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<PlotPoint> PlotPoints { get; set; }
    
    // The "Concept Containers"
    public DbSet<Character> Characters { get; set; }
    public DbSet<Theme> Themes { get; set; }
    public DbSet<CodexEntry> CodexEntries { get; set; }
    
    // The Atomic Unit of Truth
    public DbSet<Note> Notes { get; set; }
    
    public DbSet<SourceMaterial> SourceMaterials { get; set; }
    
    public DbSet<GeminiEntry> GeminiEntries { get; set; }
    public DbSet<Idea> Ideas { get; set; }
    
    // --- PAYLOAD CONNECTIONS (The Edges of the Graph) ---
    // Adding these allows you to do: _context.PlotPointCharacters.Remove(link);
    
    public DbSet<PlotPointCharacter> PlotPointCharacters { get; set; }
    public DbSet<PlotPointTheme> PlotPointThemes { get; set; }
    public DbSet<PlotPointThread> PlotPointThreads { get; set; }
    
    // NEW: The replacement for PlotPointNotes
    public DbSet<PlotPointCodexEntry> PlotPointCodexEntries { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // =========================================================
        // 1. COMPOSITE KEYS (Many-to-Many Payloads)
        // =========================================================

        // PlotPoint <-> Thread
        modelBuilder.Entity<PlotPointThread>()
            .HasKey(pt => new { pt.PlotPointId, pt.ThreadId });

        // PlotPoint <-> Theme (Thematic Argument)
        modelBuilder.Entity<PlotPointTheme>()
            .HasKey(pt => new { pt.PlotPointId, pt.ThemeId });

        // PlotPoint <-> Character (Development)
        modelBuilder.Entity<PlotPointCharacter>()
            .HasKey(pc => new { pc.PlotPointId, pc.CharacterId });

        modelBuilder.Entity<PlotPointCodexEntry>()
            .HasKey(pc => new { pc.PlotPointId, pc.CodexEntryId });

        modelBuilder.Entity<Note>()
            .HasOne(n => n.SourceMaterial)
            .WithMany(s => s.Notes)
            .HasForeignKey(n => n.SourceMaterialId)
            .OnDelete(DeleteBehavior.SetNull); // If you delete a Source, keep the note but clear the tag

        // =========================================================
        // 3. OPTIONAL CONSTRAINTS
        // =========================================================
        
        // Ensure Note belongs to only one parent container (optional enforcement)
        // EF Core handles the nullable FKs automatically, but explicit config helps clarity.
        modelBuilder.Entity<Note>()
            .HasOne(n => n.Character)
            .WithMany(c => c.Notes)
            .HasForeignKey(n => n.CharacterId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Note>()
            .HasOne(n => n.Theme)
            .WithMany(t => t.Notes)
            .HasForeignKey(n => n.ThemeId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Note>()
            .HasOne(n => n.CodexEntry)
            .WithMany(c => c.Notes)
            .HasForeignKey(n => n.CodexEntryId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Note>()
            .HasOne(n => n.Chapter)
            .WithMany(c => c.Notes)
            .HasForeignKey(n => n.ChapterId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Note>()
            .HasOne(n => n.StoryThread)
            .WithMany(c => c.Notes)
            .HasForeignKey(n => n.StoryThreadId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Note>()
            .HasOne(n => n.Location)
            .WithMany(c => c.Notes)
            .HasForeignKey(n => n.LocationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
    
    public override int SaveChanges()
    {
        UpdateAuditFields();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditFields();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateAuditFields()
    {
        // Get all tracked entities that implement IAuditableText
        var entries = ChangeTracker.Entries<IAuditableText>();

        foreach (var entry in entries)
        {
            // If the entity was just added or modified, update the timestamp
            if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
            {
                entry.Entity.LastModified = DateTime.UtcNow; 
                // Note: Use DateTime.Now if your app relies on local time, 
                // but UtcNow is generally preferred for databases.
            }
        }
    }
}