using OpenTK.Mathematics;
using SharpWoxel.Util;

namespace SharpWoxel.World.Blocks;

class GrassBlock : IBlock
{
    public Vector2i GetFaceTextureAtlasCoordinates(Cube.Face face)
    {
        var coords = new Vector2i();

        switch (face)
        {
            case Cube.Face.TOP:
                coords = (0, 0);
                break;
            case Cube.Face.BOTTOM:
                coords = (2, 0);
                break;
            default:
                coords = (1, 0);
                break;
        }

        return coords;
    }

    public string GetID()
    {
        return "grass_block";
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
