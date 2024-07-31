using SharpWoxel.world.blocks;

namespace SharpWoxel.player.inventory;

internal class InventoryItem
{
    public InventoryItem(IBlock? block)
    {
        Item = block;
        Selected = false;
        Count = 0;
    }

    public IBlock? Item { get; set; }
    public bool Selected { get; set; }
    public int Count { get; set; }
}