using OpenTK.Mathematics;
using SharpWoxel.mesh;

namespace SharpWoxel.world.blocks;

internal class GrassBlock : IBlock
{
    public Vector2i GetFaceTextureAtlasCoordinates(CubeFaceMesh.Face face)
    {
        Vector2i coords = face switch
        {
            CubeFaceMesh.Face.Top => (0, 0),
            CubeFaceMesh.Face.Bottom => (2, 0),
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