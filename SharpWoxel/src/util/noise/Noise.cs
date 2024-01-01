using OpenTK.Mathematics;

namespace SharpWoxel.Util.Noise;

abstract class Noise
{
    protected abstract float NoiseImplementation2(float x, float y);
    protected abstract float NoiseImplementation3(float x, float y, float z);

    // Okvirna metoda
    public virtual float GenerateNoise2(float x, float y, NoiseOptions options)
    {
        // Commonly used values (changing these doesn't really change much)
        float Lacunarity = 2.0f;
        float Persistance = 1.0f / Lacunarity; // 0.5f, isto se zove ponekad i Gain

        float elevation = options.Amplitude;
        float freq = options.Frequency;
        float amp = options.Amplitude;
        for (int i = 0; i < options.Octaves; i++)
        {
            float sx = x * freq;
            float sy = y * freq;

            elevation += NoiseImplementation2(sx, sy) * amp;

            freq *= Lacunarity;
            amp *= Persistance;
        }

        // Safeguard to ensure the return value is [0, 1]
        return MathHelper.Clamp(elevation, 0.0f, 1.0f);
    }

    public virtual float GenerateNoise3(float x, float y, float z, NoiseOptions options)
    {
        // Commonly used values (changing these doesn't really change much)
        float Lacunarity = 2.0f;
        float Persistance = 1.0f / Lacunarity; // 0.5f, isto se zove ponekad i Gain

        float elevation = options.Amplitude;
        float freq = options.Frequency;
        float amp = options.Amplitude;
        for (int i = 0; i < options.Octaves; i++)
        {
            float sx = x * freq;
            float sy = y * freq;
            float sz = z * freq;

            elevation += NoiseImplementation3(sx, sy, sz) * amp;

            freq *= Lacunarity;
            amp *= Persistance;
        }
        // Safeguard to ensure the return value is [0, 1]
        return MathHelper.Clamp(elevation, 0.0f, 1.0f);
    }
}
