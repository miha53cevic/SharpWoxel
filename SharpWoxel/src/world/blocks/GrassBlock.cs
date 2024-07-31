using OpenTK.Mathematics;
using SharpWoxel.util;

namespace SharpWoxel.world.blocks;

internal class GrassBlock : IBlock
{
    public Vector2i GetFaceTextureAtlasCoordinates(Cube.Face face)
    {
        Vector2i coords = face switch
        {
            Cube.Face.Top => (0, 0),
            Cube.Face.Bottom => (2, 0),
            _ => (1, 0)
        };

        return coords;
    }

    public string GetId()
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