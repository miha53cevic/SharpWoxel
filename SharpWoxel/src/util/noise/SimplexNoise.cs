using DotnetNoise;

namespace SharpWoxel.util.noise;

internal class SimplexNoise(int seed = 1337) : Noise
{
    private readonly FastNoise _noise = new(seed);

    protected override float NoiseImplementation2(float x, float y)
    {
        return _noise.GetSimplex(x, y);
    }

    protected override float NoiseImplementation3(float x, float y, float z)
    {
        return _noise.GetSimplex(x, y, z);
    }
}