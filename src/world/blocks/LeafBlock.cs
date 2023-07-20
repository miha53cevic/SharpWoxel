using OpenTK.Mathematics;
using SharpWoxel.util;

namespace SharpWoxel.world.blocks
{
    class LeafBlock : IBlock
    {
        public Vector2i GetFaceTextureAtlasCoordinates(Cube.Face face)
        {
            var coords = new Vector2i();

            switch(face)
            {
                default:
                    coords = (6, 0);
                    break;
            }

            return coords;
        }

        public string GetID()
        {
            return "leaf_block";
        }

        public bool IsAir()
        {
            return false;
        }

        public bool IsTransparent()
        {
            return false;
        }
    }
}
