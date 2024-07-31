using SharpWoxel.world.blocks;

namespace SharpWoxel.player.inventory;

internal class InventoryItem(IBlock? block)
{
    public IBlock? Item { get; set; } = block;
    public bool Selected { get; set; } = false;
    public int Count { get; set; } = 0;
}