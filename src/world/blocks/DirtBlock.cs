using OpenTK.Mathematics;
using SharpWoxel.util;

namespace SharpWoxel.world.blocks
{
    class DirtBlock : IBlock
    {
        public Vector2i GetFaceTextureAtlasCoordinates(Cube.Face face)
        {
            var coords = new Vector2i();

            switch(face)
            {
                default:
                    coords = (2, 0);
                    break;
            }

            return coords;
        }

        public string GetID()
        {
            return "dirt_block";
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
