
using DotnetNoise;
using OpenTK.Mathematics;
using SharpWoxel.util.noise;
using SharpWoxel.world.blocks;

namespace SharpWoxel.world.terrain
{
    class TestTerrain : Terrain
    {
        public TestTerrain(Vector3i size, Vector3i chunkSize) 
            : base(size, chunkSize)
        {
        }

        protected override void Generate()
        {
            NoiseOptions noiseOptions1 = new NoiseOptions
            {
                Octaves = 6,
                Frequency = 0.9f,
                Roughness = 0.5f,
                Redistribution = 1.0f
            };

            NoiseOptions noiseOptions2 = new NoiseOptions
            {
                Octaves = 4,
                Frequency = 0.8f,
                Roughness = 0.5f,
                Redistribution = 1.0f
            };

            int seed = 1337;
            Noise noise = new SimplexNoise(seed);

            int minAmp = 1;
            int maxAmp = _chunkSize.Y * _terrainSize.Y;

            void createTerrain(Chunk chunk)
            {
                // Go through every block in the chunk
                for (int x = 0; x < _chunkSize.X; x++)
                {
                    for (int z = 0; z < _chunkSize.Z; z++)
                    {
                        // Calculate the peaks
                        float posX = chunk.Entity.Position.X + x;
                        float posZ = chunk.Entity.Position.Z + z;

                        // Combine 2 noise height maps for hilly and flat terrain combos
                        float noise1 = noise.GenerateNoise2(posX, posZ, noiseOptions1);
                        float noise2 = noise.GenerateNoise2(posX, posZ, noiseOptions2);
                        float result = noise1 * noise2;

                        int height = (int)(result * (float)maxAmp + (float)minAmp);

                        // Check if the height is valid
                        if (height > (_chunkSize.Y * _terrainSize.Y))
                            height = (_chunkSize.Y * _terrainSize.Y);

                        // For every y block in the chunk set its layers
                        for (int y = 0; y < _chunkSize.Y; y++)
                        {
                            // Get the voxels global Y position
                            int voxelY = (int)chunk.Entity.Position.Y + y;
                            
                            if (voxelY == height)
                            {
                                chunk.SetBlockLocal(x, y, z, new GrassBlock());
                            }
                            if (voxelY < height)
                            {
                                if (voxelY >= height - 3)
                                {
                                    chunk.SetBlockLocal(x, y, z, new DirtBlock());
                                }
                                else
                                {
                                    chunk.SetBlockLocal(x, y, z, new StoneBlock());
                                }
                            }
                        }
                    }
                } 
            }

            foreach (var chunk in  _chunks)
            {
                createTerrain(chunk);
            }
        }
    }
}
