using OpenTK.Mathematics;
using SharpWoxel.mesh;

namespace SharpWoxel.world.blocks;

internal interface IBlock
{
    public string GetId();
    public bool IsAir();
    public bool IsTransparent();
    public Vector2i GetFaceTextureAtlasCoordinates(CubeFaceMesh.Face face);
}