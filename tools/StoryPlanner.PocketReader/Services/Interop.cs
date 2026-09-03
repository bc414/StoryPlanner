using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Versioning;

namespace StoryPlanner.PocketReader;

/// <summary>
/// The only JavaScript the reader uses: IndexedDB for the picked file bytes (so a phone keeps
/// its plans across visits) and localStorage for per-device preferences. Bytes cross the
/// boundary through [JSImport] typed arrays, not base64 strings, because a plan is tens of
/// megabytes. Loading a stored plan is two steps (prepare, then take) because a Task cannot
/// carry a byte[] across JSImport.
/// </summary>
[SupportedOSPlatform("browser")]
internal static partial class Interop
{
    private const string Module = "pocketStorage";

    public static Task InitializeAsync() => JSHost.ImportAsync(Module, "../storage.js");

    /// <summary>Returns JSON {name, pickedAt} when a plan is stored for the slug, else null, and stages its bytes.</summary>
    [JSImport("prepare", Module)]
    public static partial Task<string?> PrepareStoredPlan(string slug);

    /// <summary>The bytes staged by PrepareStoredPlan; the staging slot is cleared.</summary>
    [JSImport("take", Module)]
    public static partial byte[]? TakeStoredPlan(string slug);

    [JSImport("save", Module)]
    public static partial Task SavePlan(string slug, string name, string pickedAtIso, byte[] bytes);

    [JSImport("remove", Module)]
    public static partial Task RemovePlan(string slug);

    [JSImport("getPref", Module)]
    public static partial string? GetPref(string key);

    [JSImport("setPref", Module)]
    public static partial void SetPref(string key, string value);
}
