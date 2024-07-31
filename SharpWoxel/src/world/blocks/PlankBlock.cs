using OpenTK.Mathematics;
using SharpWoxel.util;

namespace SharpWoxel.world.blocks;

internal class PlankBlock : IBlock
{
    public Vector2i GetFaceTextureAtlasCoordinates(Cube.Face face)
    {
        var coords = face switch
        {
            _ => (7, 0)
        };

        return coords;
    }

    public string GetId()
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