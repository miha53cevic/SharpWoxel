using OpenTK.Mathematics;
using SharpWoxel.util;

namespace SharpWoxel.world.blocks;

internal class WoodBlock : IBlock
{
    public Vector2i GetFaceTextureAtlasCoordinates(Cube.Face face)
    {
        var coords = face switch
        {
            Cube.Face.Top or Cube.Face.Bottom => (4, 0),
            _ => (5, 0)
        };

        return coords;
    }

    public string GetId()
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