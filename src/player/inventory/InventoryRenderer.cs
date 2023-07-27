using glObjects;
using OpenTK.Mathematics;
using SharpWoxel.entities;
using SharpWoxel.gui;

namespace SharpWoxel.player.inventory
{
    // Better to seperate rendering and inventoryModel to adhere more to the single resposibility principle
    class InventoryRenderer
    {
        private Inventory _inventory;
        private Vector2i _position;
        private Vector2i _itemSize;
        private Rect _uiBox = GUI.CreateRect();
        private Rect _uiItem = GUI.CreateRect();

        public InventoryRenderer(Inventory inventory)
        {
            _inventory = inventory;
            _position = Vector2i.Zero;
            _itemSize = (96, 96);
        }

        public void SetRenderPosition(Vector2i position)
        {
            _position = position;
        }

        public void SetRenderItemSize(Vector2i size)
        {
            _itemSize = size;
        }

        public void Render(Shader shader)
        {
            var items = _inventory.GetInventoryItems();
            var pos = _position;
            foreach (var item in items)
            {
                // Set inventory frame
                if (item.Selected)
                {
                    _uiBox.SetTextureCoordinates(ChunkEntity.TexAtlas.GetTextureCoords(5, 7));
                }
                else _uiBox.SetTextureCoordinates(ChunkEntity.TexAtlas.GetTextureCoords(6, 7));

                // Set block representation in inventoryItem
                if (item.Item != null)
                {
                    var coords = item.Item.GetFaceTextureAtlasCoordinates(util.Cube.Face.FRONT);
                    _uiItem.SetTextureCoordinates(ChunkEntity.TexAtlas.GetTextureCoords(coords.X, coords.Y));
                }

                // Set positions and sizes
                _uiBox.Position = pos;
                _uiBox.Size = _itemSize;
                _uiItem.Position = pos + (_itemSize / 4);
                _uiItem.Size = (_itemSize / 2);
                pos += (_itemSize.X, 0);

                // Render
                _uiBox.Render(shader);
                if (item.Item != null)
                    _uiItem.Render(shader);
            }
        }

        public void CenterOnPosition()
        {
            _position = (_position.X - (_itemSize.X * _inventory.GetInventoryItems().Length / 2), _position.Y - (_itemSize.Y / 2));
        }
    }
}
