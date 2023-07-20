using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using SharpWoxel.entities;
using SharpWoxel.util;
using SharpWoxel.world.blocks;

namespace SharpWoxel.world
{
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
            Entity = new ChunkEntity("../../../res/textureAtlas.png", 2048, 256);
            Entity.Position = position;

            _chunkSize = chunkSize;
            _blocks = new IBlock[chunkSize.X, chunkSize.Y, chunkSize.Z]; // default object values is null
            _neighbours = new Chunk[_neighbourCount];

            // Set chunk to be air by default
            for (int z = 0; z < chunkSize.Z; z++)
            {
                for (int y = 0; y < chunkSize.Y; y++)
                {
                    for (int x = 0; x < chunkSize.X; x++)
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
                temp_textureCoords.AddRange(Entity.GetTextureAtlas().GetTextureCoords(coords.X, coords.Y));

                temp_indicies.Add(indicies + 0);
                temp_indicies.Add(indicies + 1);
                temp_indicies.Add(indicies + 3);
                temp_indicies.Add(indicies + 3);
                temp_indicies.Add(indicies + 1);
                temp_indicies.Add(indicies + 2);
                indicies += 4;
            }
            
            for (int z = 0; z < _chunkSize.Z; z++)
            {
                for (int y = 0; y < _chunkSize.Y; y++)
                {
                    for (int x = 0; x < _chunkSize.X; x++)
                    {
                        // Check for possible neighbours
                        bool left = false, right = false, front = false, back = false, top = false, bottom = false;
                        if (x != 0) left = true;
                        if (y != 0) bottom = true;
                        if (z != 0) back = true;
                        if (x != _chunkSize.X - 1) right = true;
                        if (y != _chunkSize.Y - 1) top = true;
                        if (z != _chunkSize.Z - 1) front = true;

                        if (_blocks[x, y, z].IsAir())
                            continue;

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
                    }
                }
            }

            Entity.SetVerticies(temp_verticies.ToArray(), BufferUsageHint.DynamicDraw);
            Entity.SetIndicies(temp_indicies.ToArray(), BufferUsageHint.DynamicDraw);
            Entity.SetTextureCoords(temp_textureCoords.ToArray(), BufferUsageHint.DynamicDraw);

            Console.WriteLine(String.Format("Created chunk with: verticies({0}), indicies({1}), textureCoords({2})", temp_verticies.Count, temp_indicies.Count, temp_textureCoords.Count));
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
    }
}
