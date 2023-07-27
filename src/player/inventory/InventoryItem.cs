using glObjects;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using SharpWoxel.entities;
using SharpWoxel.gui;
using SharpWoxel.world.blocks;

namespace SharpWoxel.player.inventory
{
    class InventoryItem
    {
        private readonly Rect _uiBox = GUI.CreateRect();
        private readonly Rect _uiItem = GUI.CreateRect();
        private IBlock? _item;
        private bool _selected;

        public bool Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                if (_selected)
                    _uiBox.SetTextureCoordinates(ChunkEntity.TexAtlas.GetTextureCoords(5, 7));
                else
                    _uiBox.SetTextureCoordinates(ChunkEntity.TexAtlas.GetTextureCoords(6, 7));
            }
        }
        public int Count { get; set; } = 0;
        public IBlock? Item
        {
            get => _item;
            set
            {
                _item = value;
                if (_item != null)
                {
                    //Display block front texture as item display
                    var coords = _item.GetFaceTextureAtlasCoordinates(util.Cube.Face.FRONT);
                    _uiItem.SetTextureCoordinates(ChunkEntity.TexAtlas.GetTextureCoords(coords.X, coords.Y));
                }
            }
        }

        public InventoryItem(IBlock? block)
        {
            Item = block;
            Selected = false;
        }

        public void Render(Shader shader, Vector2i position, Vector2i size)
        {
            ChunkEntity.TexAtlas.Use(TextureUnit.Texture0);
            _uiBox.Render(shader, position, size);
            if (Item != null)
                _uiItem.Render(shader, position + (size / 4), size / 2);
        }
    }
}
