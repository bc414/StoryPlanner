using Microsoft.EntityFrameworkCore;
using StoryPlanner.Core.Models;

namespace StoryPlanner.Core;

public static class DataSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (await context.Chapters.AnyAsync()) return;

        // 1. Get the Data
        var data = DesignDataFactory.CreateWorld();

        // 2. Dump it into EF Core
        // EF is smart enough to handle the graph if we add the root lists.
        // Important: We reset IDs to 0 if your DB auto-increments, 
        // OR allow identity insert. For SQLite, adding with IDs usually works fine.
    
        context.SourceMaterials.AddRange(data.Sources);
        context.Locations.AddRange(data.Locations);
        context.Characters.AddRange(data.Characters);
        context.Themes.AddRange(data.Themes);
        context.Threads.AddRange(data.StoryThreads);
        context.Notes.AddRange(data.Notes);
        context.Chapters.AddRange(data.Chapters);
        context.PlotPoints.AddRange(data.PlotPoints);
        context.CodexEntries.AddRange(data.CodexEntries);

        await context.SaveChangesAsync();
    }
}