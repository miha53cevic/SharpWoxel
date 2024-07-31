using OpenTK.Mathematics;
using SharpWoxel.util;

namespace SharpWoxel.world.blocks;

internal class StoneBlock : IBlock
{
    public Vector2i GetFaceTextureAtlasCoordinates(Cube.Face face)
    {
        var coords = face switch
        {
            _ => (3, 0)
        };

        return coords;
    }

    public string GetId()
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