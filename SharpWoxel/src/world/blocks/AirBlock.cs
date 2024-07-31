using OpenTK.Mathematics;
using SharpWoxel.mesh;

namespace SharpWoxel.world.blocks;

internal class AirBlock : IBlock
{
    public Vector2i GetFaceTextureAtlasCoordinates(CubeFaceMesh.Face face)
    {
        throw new NotImplementedException();
    }

    public string GetId()
    {
        return "air_block";
    }

    public bool IsAir()
    {
        return true;
    }

    public bool IsTransparent()
    {
        throw new NotImplementedException();
    }
}