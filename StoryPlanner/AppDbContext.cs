using Microsoft.EntityFrameworkCore;

namespace StoryPlanner;
using StoryPlanner.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // --- Tables ---
    public DbSet<Chapter> Chapters { get; set; }
    public DbSet<StoryThread> Threads { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<PlotPoint> PlotPoints { get; set; }
    public DbSet<PlotPointNote> PlotPointNotes { get; set; }
    
    // The "Concept Containers"
    public DbSet<Character> Characters { get; set; }
    public DbSet<Theme> Themes { get; set; }
    public DbSet<CodexEntry> CodexEntries { get; set; }
    
    // The Atomic Unit of Truth
    public DbSet<Note> Notes { get; set; }
    
    public DbSet<SourceMaterial> SourceMaterials { get; set; }
    
    public DbSet<GeminiEntry> GeminiEntries { get; set; }

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

        // PlotPoint <-> Note (Consistency Check)
        modelBuilder.Entity<PlotPointNote>()
            .HasKey(pn => new { pn.PlotPointId, pn.NoteId });

        modelBuilder.Entity<PlotPointNote>()
            .HasOne(pn => pn.PlotPoint)
            .WithMany(p => p.NoteReferences) // <--- Explicitly map your new name here
            .HasForeignKey(pn => pn.PlotPointId);

        modelBuilder.Entity<PlotPointNote>()
            .HasOne(pn => pn.Note)
            .WithMany(n => n.PlotPointReferences)
            .HasForeignKey(pn => pn.NoteId);

        // =========================================================
        // 2. SELF-REFERENCING RELATIONSHIPS (The Logic Engine)
        // =========================================================

        // --- PlotPoint Causality (Event A causes Event B) ---
        modelBuilder.Entity<PlotPointDependency>()
            .HasKey(k => new { k.PrerequisiteId, k.DependentId });

        modelBuilder.Entity<PlotPointDependency>()
            .HasOne(pd => pd.Prerequisite)
            .WithMany(p => p.Dependents)
            .HasForeignKey(pd => pd.PrerequisiteId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent Cascade Cycles

        modelBuilder.Entity<PlotPointDependency>()
            .HasOne(pd => pd.Dependent)
            .WithMany(p => p.Prerequisites)
            .HasForeignKey(pd => pd.DependentId)
            .OnDelete(DeleteBehavior.Restrict);

        // --- Note Logic Tree (Fact A implies Fact B) ---
        modelBuilder.Entity<NoteDependency>()
            .HasKey(k => new { k.PrerequisiteId, k.DependentId });

        modelBuilder.Entity<NoteDependency>()
            .HasOne(nd => nd.Prerequisite)
            .WithMany(n => n.Dependents)
            .HasForeignKey(nd => nd.PrerequisiteId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent Cascade Cycles

        modelBuilder.Entity<NoteDependency>()
            .HasOne(nd => nd.Dependent)
            .WithMany(n => n.Prerequisites)
            .HasForeignKey(nd => nd.DependentId)
            .OnDelete(DeleteBehavior.Restrict);
        
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
    }
}