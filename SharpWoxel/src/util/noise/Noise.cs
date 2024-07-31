using OpenTK.Mathematics;

namespace SharpWoxel.util.noise;

internal abstract class Noise
{
    protected abstract float NoiseImplementation2(float x, float y);
    protected abstract float NoiseImplementation3(float x, float y, float z);

    // Okvirna metoda
    public virtual float GenerateNoise2(float x, float y, NoiseOptions options)
    {
        // Commonly used values (changing these doesn't really change much)
        const float lacunarity = 2.0f;
        const float persistance = 1.0f / lacunarity; // 0.5f, isto se zove ponekad i Gain

        var elevation = options.Amplitude;
        var freq = options.Frequency;
        var amp = options.Amplitude;
        for (var i = 0; i < options.Octaves; i++)
        {
            var sx = x * freq;
            var sy = y * freq;

            elevation += NoiseImplementation2(sx, sy) * amp;

            freq *= lacunarity;
            amp *= persistance;
        }

        // Safeguard to ensure the return value is [0, 1]
        return MathHelper.Clamp(elevation, 0.0f, 1.0f);
    }

    public virtual float GenerateNoise3(float x, float y, float z, NoiseOptions options)
    {
        // Commonly used values (changing these doesn't really change much)
        const float lacunarity = 2.0f;
        const float persistance = 1.0f / lacunarity; // 0.5f, isto se zove ponekad i Gain

        var elevation = options.Amplitude;
        var freq = options.Frequency;
        var amp = options.Amplitude;
        for (var i = 0; i < options.Octaves; i++)
        {
            var sx = x * freq;
            var sy = y * freq;
            var sz = z * freq;

            elevation += NoiseImplementation3(sx, sy, sz) * amp;

            freq *= lacunarity;
            amp *= persistance;
        }

        // Safeguard to ensure the return value is [0, 1]
        return MathHelper.Clamp(elevation, 0.0f, 1.0f);
    }
}