using ImGuiNET;
using OpenTK.Mathematics;
using SharpWoxel.imgui;
using SharpWoxel.util.noise;
using SharpWoxel.world.blocks;

namespace SharpWoxel.world.terrain
{
    class TestTerrain : Terrain
    {
        private NoiseOptions _noiseOptions1;
        private NoiseOptions _noiseOptions2;
        private Random _random;

        public TestTerrain(Vector3i size, Vector3i chunkSize) 
            : base(size, chunkSize)
        {
            _random = new Random();

            _noiseOptions1 = new NoiseOptions
            {
                Octaves = 6,
                Frequency = 0.108f,
                Amplitude = 0.5f,
            };

            _noiseOptions2 = new NoiseOptions
            {
                Octaves = 4,
                Frequency = 0.4f,
                Amplitude = 0.612f,
            };

            ImGuiSingleton.GetInstance().AddRenderFunction((controller) =>
            {
                if (ImGui.Begin("NoiseOptions"))
                {
                    ImGui.SeparatorText("NoiseOptions1");
                    {
                        int octaves = _noiseOptions1.Octaves;
                        ImGui.SliderInt("n1/octaves", ref octaves, 0, 10);
                        _noiseOptions1.Octaves = octaves;

                        float freq = _noiseOptions1.Frequency;
                        ImGui.SliderFloat("n1/frequency", ref freq, 0f, 1f);
                        _noiseOptions1.Frequency = freq;

                        float amplitude = _noiseOptions1.Amplitude;
                        ImGui.SliderFloat("n1/amplitude", ref amplitude, 0f, 1f);
                        _noiseOptions1.Amplitude = amplitude;
                    }
                    ImGui.SeparatorText("NoiseOptions2");
                    {
                        int octaves = _noiseOptions2.Octaves;
                        ImGui.SliderInt("n2/octaves", ref octaves, 0, 10);
                        _noiseOptions2.Octaves = octaves;

                        float freq = _noiseOptions2.Frequency;
                        ImGui.SliderFloat("n2/frequency", ref freq, 0f, 1f);
                        _noiseOptions2.Frequency = freq;

                        float amplitude = _noiseOptions2.Amplitude;
                        ImGui.SliderFloat("n2/amplitude", ref amplitude, 0f, 1f);
                        _noiseOptions2.Amplitude = amplitude;
                    }
                    ImGui.Spacing(); // like <br>
                    if (ImGui.Button("Rebuild terrain"))
                    {
                        base.GenerateTerrain();
                    }
                }
                ImGui.End();
            });
        }

        protected override void Generate()
        {
            int seed = 1337;
            Noise noise = new SimplexNoise(seed);

            int minAmp = 1;
            int maxAmp = _chunkSize.Y * _terrainSize.Y;

            void createTree(Vector3i location)
            {
                int treeHeight = _random.Next(4, 7);
                for (int i = 0; i < treeHeight; i++)
                    SetBlockGlobal(location.X, location.Y + i, location.Z, new WoodBlock());

                // TODO: REDO THIS
                for (int x = -2; x <= 2; x++)
                    for (int y = -2; y < 1; y++)
                        for (int z = -2; z <= 2; z++)
                        {
                            if (x == 0 && y < 0 && z == 0)
                                continue;

                            if (y == -1 && (Math.Abs(x) == 2 || Math.Abs(z) == 2))
                                continue;

                            if (y == 0 && (Math.Abs(x) >= 1 || Math.Abs(z) >= 1))
                                continue;

                            SetBlockGlobal(location.X + x, location.Y + y + treeHeight, location.Z + z, new LeafBlock());
                        }

                SetBlockGlobal(location.X + 1, location.Y + treeHeight, location.Z + 0, new LeafBlock());
                SetBlockGlobal(location.X - 1, location.Y + treeHeight, location.Z + 0, new LeafBlock());
                SetBlockGlobal(location.X + 0, location.Y + treeHeight, location.Z + 1, new LeafBlock());
                SetBlockGlobal(location.X + 0, location.Y + treeHeight, location.Z - 1, new LeafBlock());
            }

            void createTerrain(Chunk chunk)
            {
                // Set every block to air
                chunk.AirOutChunk();

                // Go through every block in the chunk
                for (int x = 0; x < _chunkSize.X; x++)
                {
                    for (int z = 0; z < _chunkSize.Z; z++)
                    {
                        // Calculate the peaks
                        float posX = chunk.Entity.Position.X + x;
                        float posZ = chunk.Entity.Position.Z + z;

                        // Combine 2 noise height maps for hilly and flat terrain combos
                        float noise1 = noise.GenerateNoise2(posX, posZ, _noiseOptions1);
                        float noise2 = noise.GenerateNoise2(posX, posZ, _noiseOptions2);
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

                                // TODO: Move this into createTree and keep only location
                                if (_random.NextSingle() >= 0.99f)
                                {
                                    if (x > 1 && x + 2 < _chunkSize.X && y > 1 && y + 7 < _chunkSize.Y && z > 1 && z + 2 < _chunkSize.Z)
                                    {
                                        var location = Vector3i.Zero;
                                        location.X = (int)chunk.Entity.Position.X + x;
                                        location.Y = voxelY + 1;
                                        location.Z = (int)chunk.Entity.Position.Z + z;
                                        createTree(location);
                                    }
                                }
                            }
                            else if (voxelY < height)
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
