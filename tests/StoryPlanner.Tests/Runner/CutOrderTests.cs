using StoryPlanner.BatchFiles;
using Xunit;

namespace StoryPlanner.Tests;

/// <summary>
/// The cut's order (d-2026-09-27-5), shared by the runner and DocIntegrity: ascending SHA-256
/// of each item's slug over its UTF-8 bytes, independent of the index's order, and the items a
/// batch answered out of it. Tier: pure.
/// </summary>
public class CutOrderTests
{
    [Fact]
    public void The_key_is_the_sha256_of_the_slugs_utf8_bytes()
        => Assert.Equal("ba7816bf8f01cfea414140de5dae2223b00361a396177a9cb410ff61f20015ad", CutOrder.KeyOf("abc"));

    [Fact]
    public void The_order_ascends_by_key_and_ignores_the_order_given()
    {
        string[] items = ["item-01", "item-02", "item-03", "item-04"];
        var order = CutOrder.Of(items);
        Assert.Equal(["item-04", "item-02", "item-01", "item-03"], order);
        Assert.Equal(order, CutOrder.Of(items.Reverse()));
        var keys = order.Select(CutOrder.KeyOf).ToList();
        Assert.Equal(keys.OrderBy(k => k, StringComparer.Ordinal), keys);
    }

    [Fact]
    public void Items_dropped_from_a_cut_leave_the_rest_in_their_places()
    {
        var full = CutOrder.Of(["item-01", "item-02", "item-03", "item-04"]);
        Assert.Equal(full.Where(i => i != "item-02"), CutOrder.Of(["item-01", "item-03", "item-04"]));
    }

    [Fact]
    public void Answered_items_after_the_first_gap_are_out_of_the_cuts_order()
    {
        string[] items = ["item-01", "item-02", "item-03", "item-04"];   // cut's order: 04, 02, 01, 03
        Assert.Empty(CutOrder.AnsweredOutOfOrder(items, new HashSet<string> { "item-04", "item-02" }));          // a prefix
        Assert.Empty(CutOrder.AnsweredOutOfOrder(items, items.ToHashSet()));                                      // complete
        Assert.Equal(["item-01", "item-03"], CutOrder.AnsweredOutOfOrder(items, new HashSet<string> { "item-04", "item-01", "item-03" })); // item-02 waits, failed or skipped
        Assert.Equal(["item-03"], CutOrder.AnsweredOutOfOrder(items, new HashSet<string> { "item-03" }));        // a queue jump
    }
}
