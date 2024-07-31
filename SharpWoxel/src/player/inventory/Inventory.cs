using SharpWoxel.world.blocks;

namespace SharpWoxel.player.inventory;

internal class PlayerInventory
{
    private int _currentlySelected;
    private readonly InventoryItem[] _items;

    public PlayerInventory(int inventorySize)
    {
        _items = new InventoryItem[inventorySize];

        for (var i = 0; i < _items.Length; i++) _items[i] = new InventoryItem(null);
        _currentlySelected = 0;
        _items[_currentlySelected].Selected = true;
    }

    private void UpdateSelection(int newSelected)
    {
        _items[_currentlySelected].Selected = false;
        _items[newSelected].Selected = true;
        _currentlySelected = newSelected;
    }

    public InventoryItem GetInventoryItem(int index)
    {
        if (index >= _items.Count())
            throw new ArgumentException($"Index {index} out of bound in array");
        return _items[index];
    }

    public void ChangeInventoryItem(int index, IBlock item)
    {
        if (index >= _items.Length)
            throw new ArgumentException($"Index {index} out of bound in array");
        _items[index].Item = item;
    }

    public InventoryItem[] GetInventoryItems()
    {
        return _items;
    }

    public void SelectNext()
    {
        if (_currentlySelected == _items.Length - 1)
            UpdateSelection(0);
        else
            UpdateSelection(_currentlySelected + 1);
    }

    public void SelectPrevious()
    {
        if (_currentlySelected == 0)
            UpdateSelection(_items.Length - 1);
        else
            UpdateSelection(_currentlySelected - 1);
    }
}