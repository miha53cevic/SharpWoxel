using OpenTK.Mathematics;
using SharpWoxel.Util;

namespace SharpWoxel.World.Blocks;

class PlankBlock : IBlock
{
    public Vector2i GetFaceTextureAtlasCoordinates(Cube.Face face)
    {
        var coords = new Vector2i();

        switch (face)
        {
            default:
                coords = (7, 0);
                break;
        }

        return coords;
    }

    public string GetID()
    {
        return "plank_block";
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
