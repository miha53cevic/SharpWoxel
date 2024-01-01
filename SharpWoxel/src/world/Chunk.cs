using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using SharpWoxel.Entities;
using SharpWoxel.Util;
using SharpWoxel.World.Blocks;

namespace SharpWoxel.World;

enum NeighbourDirection
{
    NORTH,
    SOUTH,
    EAST,
    WEST,
    ABOVE,
    BELOW
};

class Chunk
{
    private Vector3i _chunkSize;
    private readonly IBlock[,,] _blocks;
    private readonly Chunk[] _neighbours;
    private const int _neighbourCount = 6;

    public ChunkEntity Entity { get; set; }

    public Chunk(Vector3 position, Vector3i chunkSize)
    {
        Entity = new ChunkEntity();
        Entity.Position = position;

        _chunkSize = chunkSize;
        _blocks = new IBlock[chunkSize.X, chunkSize.Y, chunkSize.Z]; // default object values is null
        _neighbours = new Chunk[_neighbourCount];

        // Set chunk to be air by default
        AirOutChunk();
    }

    public void AirOutChunk()
    {
        for (int x = 0; x < _chunkSize.X; x++)
        {
            for (int y = 0; y < _chunkSize.Y; y++)
            {
                for (int z = 0; z < _chunkSize.Z; z++)
                {
                    _blocks[x, y, z] = new AirBlock();
                }
            }
        }
    }

    public IBlock GetBlockLocal(int x, int y, int z)
    {
        if (IsBlockOutOfBounds(x, y, z))
            throw new Exception(string.Format("[Chunk::GetBlockLocal]: Block ({0},{1},{2}) is out of bounds", x, y, z));

        return _blocks[x, y, z];
    }

    public void SetBlockLocal(int x, int y, int z, IBlock block)
    {
        if (IsBlockOutOfBounds(x, y, z))
            throw new Exception(string.Format("[Chunk::SetBlockLocal]: Block ({0},{1},{2}) is out of bounds", x, y, z));

        _blocks[x, y, z] = block;
    }

    private bool IsBlockOutOfBounds(int x, int y, int z)
    {
        if (x < 0 || x >= _chunkSize.X || y < 0 || y >= _chunkSize.Y || z < 0 || z >= _chunkSize.Z)
            return true;
        else return false;
    }

    public void BuildMesh()
    {
        var temp_verticies = new List<float>();
        var temp_textureCoords = new List<float>();
        var temp_indicies = new List<uint>();
        uint indicies = 0;

        /*int i = 0;
        foreach (var n in _neighbours)
        {
            Console.WriteLine(string.Format("{0}: {1}", (NeighbourDirection)i, n));
            i++;
        }
        Console.WriteLine(Entity.Position);
        Console.WriteLine(this);*/

        void CreateCubeFace(Cube.Face face, int x, int y, int z)
        {
            var faceVerticies = Cube.CubeFace.GetCubeFace(face);

            for (int i = 0; i < faceVerticies.Length / 3; i++)
            {
                temp_verticies.Add(faceVerticies[0 + i * 3] + x);
                temp_verticies.Add(faceVerticies[1 + i * 3] + y);
                temp_verticies.Add(faceVerticies[2 + i * 3] + z);
            }

            var block = _blocks[x, y, z];
            var coords = block.GetFaceTextureAtlasCoordinates(face);
            temp_textureCoords.AddRange(ChunkEntity.TexAtlas.GetTextureCoords(coords.X, coords.Y));

            temp_indicies.Add(indicies + 0);
            temp_indicies.Add(indicies + 1);
            temp_indicies.Add(indicies + 3);
            temp_indicies.Add(indicies + 3);
            temp_indicies.Add(indicies + 1);
            temp_indicies.Add(indicies + 2);
            indicies += 4;
        }

        for (int x = 0; x < _chunkSize.X; x++)
        {
            for (int y = 0; y < _chunkSize.Y; y++)
            {
                for (int z = 0; z < _chunkSize.Z; z++)
                {
                    // Skip if air block
                    if (_blocks[x, y, z].IsAir())
                        continue;

                    // Check for possible block neighbours
                    bool left = false, right = false, front = false, back = false, top = false, bottom = false;
                    if (x != 0) left = true;
                    if (y != 0) bottom = true;
                    if (z != 0) back = true;
                    if (x != _chunkSize.X - 1) right = true;
                    if (y != _chunkSize.Y - 1) top = true;
                    if (z != _chunkSize.Z - 1) front = true;

                    // Create cube face if the neighbouring block is air
                    if (left)
                        if (_blocks[x - 1, y, z].IsAir()) CreateCubeFace(Cube.Face.LEFT, x, y, z);
                    if (right)
                        if (_blocks[x + 1, y, z].IsAir()) CreateCubeFace(Cube.Face.RIGHT, x, y, z);
                    if (bottom)
                        if (_blocks[x, y - 1, z].IsAir()) CreateCubeFace(Cube.Face.BOTTOM, x, y, z);
                    if (top)
                        if (_blocks[x, y + 1, z].IsAir()) CreateCubeFace(Cube.Face.TOP, x, y, z);
                    if (back)
                        if (_blocks[x, y, z - 1].IsAir()) CreateCubeFace(Cube.Face.BACK, x, y, z);
                    if (front)
                        if (_blocks[x, y, z + 1].IsAir()) CreateCubeFace(Cube.Face.FRONT, x, y, z);

                    // Check 1 block width with neighbouring chunks, for the outer chunk blocks
                    if (!left && _neighbours[(int)NeighbourDirection.WEST] != null)
                        if (_neighbours[(int)NeighbourDirection.WEST].GetBlockLocal(_chunkSize.X - 1, y, z).IsAir()) CreateCubeFace(Cube.Face.LEFT, x, y, z);
                    if (!right && _neighbours[(int)NeighbourDirection.EAST] != null)
                        if (_neighbours[(int)NeighbourDirection.EAST].GetBlockLocal(0, y, z).IsAir()) CreateCubeFace(Cube.Face.RIGHT, x, y, z);
                    if (!bottom && _neighbours[(int)NeighbourDirection.BELOW] != null)
                        if (_neighbours[(int)NeighbourDirection.BELOW].GetBlockLocal(x, _chunkSize.Y - 1, z).IsAir()) CreateCubeFace(Cube.Face.BOTTOM, x, y, z);
                    if (!top && _neighbours[(int)NeighbourDirection.ABOVE] != null)
                        if (_neighbours[(int)NeighbourDirection.ABOVE].GetBlockLocal(x, 0, z).IsAir()) CreateCubeFace(Cube.Face.TOP, x, y, z);
                    if (!back && _neighbours[(int)NeighbourDirection.NORTH] != null)
                        if (_neighbours[(int)NeighbourDirection.NORTH].GetBlockLocal(x, y, _chunkSize.Z - 1).IsAir()) CreateCubeFace(Cube.Face.BACK, x, y, z);
                    if (!front && _neighbours[(int)NeighbourDirection.SOUTH] != null)
                        if (_neighbours[(int)NeighbourDirection.SOUTH].GetBlockLocal(x, y, 0).IsAir()) CreateCubeFace(Cube.Face.FRONT, x, y, z);
                }
            }
        }

        Entity.SetVerticies(temp_verticies.ToArray(), BufferUsageHint.DynamicDraw);
        Entity.SetIndicies(temp_indicies.ToArray(), BufferUsageHint.DynamicDraw);
        Entity.SetTextureCoords(temp_textureCoords.ToArray(), BufferUsageHint.DynamicDraw);

        Console.WriteLine(string.Format("Created {0} with: verticies({1}), indicies({2}), textureCoords({3})", this, temp_verticies.Count, temp_indicies.Count, temp_textureCoords.Count));
    }

    public void RebuildMesh()
    {
        BuildMesh();
        RebuildNeighbours();
    }

    public void SetNeighbour(NeighbourDirection direction, Chunk neighbour)
    {
        _neighbours[(int)direction] = neighbour;
    }

    private void RebuildNeighbours()
    {
        foreach (var neighbour in _neighbours)
        {
            neighbour?.BuildMesh(); // vazno da NIJE rebuild onda je rekurzivno zvanje
        }
    }

    public override string ToString()
    {
        return string.Format("Chunk({0}, {1})", this.GetHashCode(), Entity.Position);
    }
}
