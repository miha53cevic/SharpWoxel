using OpenTK.Mathematics;
using SharpWoxel.Util;

namespace SharpWoxel.World.Blocks;

class WoodBlock : IBlock
{
    public Vector2i GetFaceTextureAtlasCoordinates(Cube.Face face)
    {
        var coords = new Vector2i();

        switch (face)
        {
            case Cube.Face.TOP:
            case Cube.Face.BOTTOM:
                coords = (4, 0);
                break;
            default:
                coords = (5, 0);
                break;
        }

        return coords;
    }

    public string GetID()
    {
        return "wood_block";
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
