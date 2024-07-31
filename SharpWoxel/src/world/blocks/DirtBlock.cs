using OpenTK.Mathematics;
using SharpWoxel.mesh;

namespace SharpWoxel.world.blocks;

internal class DirtBlock : IBlock
{
    public Vector2i GetFaceTextureAtlasCoordinates(CubeFaceMesh.Face face)
    {
        var coords = face switch
        {
            _ => (2, 0)
        };

        return coords;
    }

    public string GetId()
    {
        return "dirt_block";
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