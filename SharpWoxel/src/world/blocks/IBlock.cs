using OpenTK.Mathematics;
using SharpWoxel.util;

namespace SharpWoxel.world.blocks;

internal interface IBlock
{
    public string GetId();
    public bool IsAir();
    public bool IsTransparent();
    public Vector2i GetFaceTextureAtlasCoordinates(Cube.Face face);
}