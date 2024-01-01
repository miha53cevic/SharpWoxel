using OpenTK.Mathematics;
using SharpWoxel.Util;

namespace SharpWoxel.World.Blocks;

interface IBlock
{
    public string GetID();
    public bool IsAir();
    public bool IsTransparent();
    public Vector2i GetFaceTextureAtlasCoordinates(Cube.Face face);
}
