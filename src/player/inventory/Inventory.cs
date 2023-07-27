using glObjects;
using OpenTK.Mathematics;
using SharpWoxel.world.blocks;

namespace SharpWoxel.player.inventory
{
    class Inventory
    {
        private InventoryItem[] _items;
        private Vector2i _position;
        private Vector2i _itemSize;
        private int _currentlySelected;

        public Inventory(int inventorySize, Vector2i position, Vector2i itemSize)
        {
            _items = new InventoryItem[inventorySize];
            _position = position;
            _itemSize = itemSize;

            for (int i = 0; i < _items.Length; i++)
            {
                _items[i] = new InventoryItem(null);
            }
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
                throw new ArgumentException(string.Format("Index {0} out of bound in array", index));
            return _items[index];
        }

        public void ChangeInventoryItem(int index, IBlock item)
        {
            if (index >= _items.Count())
                throw new ArgumentException(string.Format("Index {0} out of bound in array", index));
            _items[index].Item = item;
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

        public void Render(Shader shader)
        {
            var pos = new Vector2i(_position.X, _position.Y);
            foreach (var item in _items)
            {
                item.Render(shader, pos, _itemSize);
                pos += (_itemSize.X, 0);
            }
        }
    }
}
