using Microsoft.EntityFrameworkCore;
using StoryPlanner.Core.Models;

namespace StoryPlanner.Core;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // --- Tables ---
    public DbSet<PlotPoint> PlotPoints { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<PlotPointSubjectLink> PlotPointSubjectLinks { get; set; }
    public DbSet<Chapter> Chapters { get; set; }
    public DbSet<Note> Notes { get; set; }

    public DbSet<SubjectDefinition> SubjectDefinitions { get; set; }
    public DbSet<NoteTrackDefinition> NoteTrackDefinitions { get; set; }
    public DbSet<NarrativePropertyDefinition> NarrativePropertyDefinitions { get; set; }
    public DbSet<NarrativePropertyValueDefinition> NarrativePropertyValueDefinitions { get; set; }
    public DbSet<NarrativePropertyValue> NarrativePropertyValues { get; set; }
    public DbSet<Theme> Themes { get; set; }
    
    public DbSet<SourceMaterial> SourceMaterials { get; set; }
    
    public DbSet<GeminiEntry> GeminiEntries { get; set; }
    public DbSet<Idea> Ideas { get; set; }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
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
        var entries = ChangeTracker.Entries<Note>();

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