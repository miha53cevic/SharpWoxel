using OpenTK.Mathematics;
using SharpWoxel.util;

namespace SharpWoxel.world.blocks
{
    interface IBlock
    {
        public string GetID();
        public bool IsAir();
        public bool IsTransparent();
        public Vector2i GetFaceTextureAtlasCoordinates(Cube.Face face);
    }
}
