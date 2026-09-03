using StoryPlanner.Core;

namespace StoryPlanner.PocketReader;

/// <summary>
/// The random view's state: which unit, which corpora, the trail of items drawn so far (so
/// "back" retraces it), and the no-repeat ring. Unit and scope persist per device in
/// localStorage; the trail is per visit.
/// </summary>
public sealed class ReaderState
{
    private const string UnitKey = "pocket.unit";
    private const string ScopeKey = "pocket.scope";

    private readonly PlanStore _plans;
    private readonly List<ItemRef> _trail = new();
    private readonly Queue<ItemRef> _recent = new();
    private int _index = -1;

    public ReaderState(PlanStore plans)
    {
        _plans = plans;
    }

    public RandomUnit Unit { get; private set; } = RandomUnit.Note;
    public CorpusScope Scope { get; private set; } = CorpusScope.Plan;

    public ItemRef? Current => _index >= 0 && _index < _trail.Count ? _trail[_index] : null;
    public bool CanBack => _index > 0;
    public bool CanForward => _index >= 0 && _index < _trail.Count - 1;

    public event Action? Changed;

    public Task InitializeAsync()
    {
        if (Enum.TryParse<RandomUnit>(Interop.GetPref(UnitKey), out var u)) Unit = u;
        if (Enum.TryParse<CorpusScope>(Interop.GetPref(ScopeKey), out var s)) Scope = s;
        return Task.CompletedTask;
    }

    public void SetUnit(RandomUnit unit)
    {
        Unit = unit;
        Interop.SetPref(UnitKey, unit.ToString());
        Changed?.Invoke();
    }

    public void SetScope(CorpusScope scope)
    {
        Scope = scope;
        Interop.SetPref(ScopeKey, scope.ToString());
        Changed?.Invoke();
    }

    public int PoolSize => RandomDraw.Pool(_plans.Working.Cache, _plans.Archive.Cache, Scope, Unit).Count;

    /// <summary>Draws the next item; false when the pool is empty.</summary>
    public bool Next()
    {
        var pool = RandomDraw.Pool(_plans.Working.Cache, _plans.Archive.Cache, Scope, Unit);
        var drawn = RandomDraw.Draw(pool, _recent, Random.Shared);
        if (drawn is null) return false;

        if (_index < _trail.Count - 1) _trail.RemoveRange(_index + 1, _trail.Count - _index - 1);
        _trail.Add(drawn.Value);
        _index = _trail.Count - 1;

        _recent.Enqueue(drawn.Value);
        while (_recent.Count > RandomDraw.RecentRingSize) _recent.Dequeue();

        Changed?.Invoke();
        return true;
    }

    public void Back()
    {
        if (!CanBack) return;
        _index--;
        Changed?.Invoke();
    }

    public void Forward()
    {
        if (!CanForward) return;
        _index++;
        Changed?.Invoke();
    }
}
