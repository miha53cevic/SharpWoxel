using OpenTK.Mathematics;
using SharpWoxel.Util;

namespace SharpWoxel.World.Blocks;

class StoneBlock : IBlock
{
    public Vector2i GetFaceTextureAtlasCoordinates(Cube.Face face)
    {
        var coords = new Vector2i();

        switch (face)
        {
            default:
                coords = (3, 0);
                break;
        }

        return coords;
    }

    public string GetID()
    {
        return "stone_block";
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
