namespace SharpWoxel.util.noise
{
    abstract class Noise
    {
        protected abstract float NoiseImplementation2(float x, float y);
        protected abstract float NoiseImplementation3(float x, float y, float z);

        // Okvirna metoda
        public virtual float GenerateNoise2(float x, float y, NoiseOptions options)
        {
            float total = 0.0f;
            float max = 0.0f;

            float freq = options.Frequency;
            float amp = 1.0f;
            for (int i = 0; i < options.Octaves; i++)
            {
                float sx = x * freq;
                float sy = y * freq;

                total += NoiseImplementation2(sx, sy) * amp;
                max += amp; // used for converting to [0,1]

                freq *= 2.0f;
                amp *= options.Roughness;
            }
            // Map from [-1, 1] to [0, 1]
            return (float)Math.Pow((1 + (total / max)) / 2, options.Redistribution);
        }

        public virtual float GenerateNoise3(float x, float y, float z, NoiseOptions options)
        {
            float total = 0.0f;
            float max = 0.0f;

            float freq = options.Frequency;
            float amp = 1.0f;
            for (int i = 0; i < options.Octaves; i++)
            {
                float sx = x * freq;
                float sy = y * freq;
                float sz = z * freq;

                total += NoiseImplementation3(sx, sy, sz) * amp;
                max += amp; // used for converting to [0,1]

                freq *= 2.0f;
                amp *= options.Roughness;
            }
            // Map from [-1, 1] to [0, 1]
            return MathF.Pow((1 + (total / max)) / 2, options.Redistribution);
        }
    }
}
