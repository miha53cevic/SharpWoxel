using DotnetNoise;

namespace SharpWoxel.util.noise
{
    class SimplexNoise : Noise
    {
        private FastNoise _noise;

        public SimplexNoise(int seed = 1337)
        {
            _noise = new FastNoise(seed);
        }

        protected override float NoiseImplementation2(float x, float y)
        {
            return _noise.GetSimplex(x, y);
        }

        protected override float NoiseImplementation3(float x, float y, float z)
        {
            return _noise.GetSimplex(x, y, z);
        }
    }
}
