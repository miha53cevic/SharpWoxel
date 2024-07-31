using ImGuiNET;
using OpenTK.Mathematics;
using SharpWoxel.imgui;
using SharpWoxel.util.noise;
using SharpWoxel.world.blocks;

namespace SharpWoxel.world.terrain;

internal class TestTerrain : BaseTerrain
{
    private readonly NoiseOptions _noiseOptions1;
    private readonly NoiseOptions _noiseOptions2;
    private readonly Random _random;

    public TestTerrain(Vector3i numberOfChunks, Vector3i chunkSize)
        : base(numberOfChunks, chunkSize)
    {
        _random = new Random();

        _noiseOptions1 = new NoiseOptions
        {
            Octaves = 6,
            Frequency = 0.108f,
            Amplitude = 0.5f
        };

        _noiseOptions2 = new NoiseOptions
        {
            Octaves = 4,
            Frequency = 0.4f,
            Amplitude = 0.612f
        };

        ImGuiSingleton.GetInstance().AddRenderFunction(_ =>
        {
            if (ImGui.Begin("NoiseOptions"))
            {
                ImGui.SeparatorText("NoiseOptions1");
                {
                    var octaves = _noiseOptions1.Octaves;
                    ImGui.SliderInt("n1/octaves", ref octaves, 0, 10);
                    _noiseOptions1.Octaves = octaves;

                    var freq = _noiseOptions1.Frequency;
                    ImGui.SliderFloat("n1/frequency", ref freq, 0f, 1f);
                    _noiseOptions1.Frequency = freq;

                    var amplitude = _noiseOptions1.Amplitude;
                    ImGui.SliderFloat("n1/amplitude", ref amplitude, 0f, 1f);
                    _noiseOptions1.Amplitude = amplitude;
                }
                ImGui.SeparatorText("NoiseOptions2");
                {
                    var octaves = _noiseOptions2.Octaves;
                    ImGui.SliderInt("n2/octaves", ref octaves, 0, 10);
                    _noiseOptions2.Octaves = octaves;

                    var freq = _noiseOptions2.Frequency;
                    ImGui.SliderFloat("n2/frequency", ref freq, 0f, 1f);
                    _noiseOptions2.Frequency = freq;

                    var amplitude = _noiseOptions2.Amplitude;
                    ImGui.SliderFloat("n2/amplitude", ref amplitude, 0f, 1f);
                    _noiseOptions2.Amplitude = amplitude;
                }
                ImGui.Spacing(); // like <br>
                if (ImGui.Button("Rebuild terrain")) base.GenerateTerrain();
            }

            ImGui.End();
        });
    }

    private void CreateTree(float chance, int minHeight, int maxHeight, Vector3i globalLocation)
    {
        // Chance to createTree
        if (!(_random.NextSingle() <= chance)) return;

        var treeHeight = _random.Next(minHeight, maxHeight + 1);
        for (var i = 0; i < treeHeight; i++)
            SetBlockGlobal(globalLocation.X, globalLocation.Y + i, globalLocation.Z, new WoodBlock());

        // Create minecraft like tree
        for (var x = -2; x <= 2; x++)
        for (var y = -2; y < 1; y++)
        for (var z = -2; z <= 2; z++)
        {
            // Keep the WoodBlocks
            if (x == 0 && y < 0 && z == 0)
                continue;

            if (y == -1 && (Math.Abs(x) == 2 || Math.Abs(z) == 2))
                continue;

            if (y == 0 && (Math.Abs(x) >= 1 || Math.Abs(z) >= 1))
                continue;

            var globalX = globalLocation.X + x;
            var globalY = globalLocation.Y + y + treeHeight;
            var globalZ = globalLocation.Z + z;

            // If the tree is going outside of the world border don't render those blocks
            if (globalX < 0 || globalX >= ChunkSize.X * NumberOfChunks.X ||
                globalY < 0 || globalY >= ChunkSize.Y * NumberOfChunks.Y ||
                globalZ < 0 || globalZ >= ChunkSize.Z * NumberOfChunks.Z)
                continue;

            SetBlockGlobal(globalX, globalY, globalZ, new LeafBlock());
        }
    }

    protected override void Generate()
    {
        const int seed = 1337;
        Noise noise = new SimplexNoise(seed);

        const int minAmp = 1;
        var maxAmp = ChunkSize.Y * NumberOfChunks.Y;

        // Fill each chunk with air first, before setting it
        foreach (var chunk in ChunksList)
            chunk.AirOutChunk();

        foreach (var chunk in ChunksList) CreateTerrain(chunk);
        return;

        void CreateTerrain(Chunk chunk)
        {
            // Go through every block in the chunk
            for (var x = 0; x < ChunkSize.X; x++)
            for (var z = 0; z < ChunkSize.Z; z++)
            {
                // Calculate the peaks
                var posX = chunk.Entity.Position.X + x;
                var posZ = chunk.Entity.Position.Z + z;

                // Combine 2 noise height maps for hilly and flat terrain combos
                var noise1 = noise.GenerateNoise2(posX, posZ, _noiseOptions1);
                var noise2 = noise.GenerateNoise2(posX, posZ, _noiseOptions2);
                var result = noise1 * noise2;

                var height = (int)(result * maxAmp + minAmp);

                // Check if the height is valid
                if (height > ChunkSize.Y * NumberOfChunks.Y)
                    height = ChunkSize.Y * NumberOfChunks.Y;

                // For every y block in the chunk set its layers
                for (var y = 0; y < ChunkSize.Y; y++)
                {
                    // Get the voxels global Y position
                    var voxelY = (int)chunk.Entity.Position.Y + y;

                    if (voxelY == height)
                    {
                        chunk.SetBlockLocal(x, y, z, new GrassBlock());

                        // Chance to create trees on the top block
                        var globalLocation = Vector3i.Zero;
                        globalLocation.X = (int)chunk.Entity.Position.X + x;
                        globalLocation.Y = voxelY + 1;
                        globalLocation.Z = (int)chunk.Entity.Position.Z + z;
                        CreateTree(0.01f, 4, 6, globalLocation);
                    }
                    else if (voxelY < height)
                    {
                        if (voxelY >= height - 3)
                            chunk.SetBlockLocal(x, y, z, new DirtBlock());
                        else
                            chunk.SetBlockLocal(x, y, z, new StoneBlock());
                    }
                }
            }
        }
    }
}