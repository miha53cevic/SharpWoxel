using OpenTK.Mathematics;
using SharpWoxel.mesh;

namespace SharpWoxel.world.blocks;

internal class WoodBlock : IBlock
{
    public Vector2i GetFaceTextureAtlasCoordinates(CubeFaceMesh.Face face)
    {
        var coords = face switch
        {
            CubeFaceMesh.Face.Top or CubeFaceMesh.Face.Bottom => (4, 0),
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