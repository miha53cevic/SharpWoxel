using SharpWoxel.World.Blocks;

namespace SharpWoxel.Player.Inventory;

class InventoryItem
{
    public IBlock? Item { get; set; }
    public bool Selected { get; set; }
    public int Count { get; set; }

    public InventoryItem(IBlock? block)
    {
        Item = block;
        Selected = false;
        Count = 0;
    }
}
