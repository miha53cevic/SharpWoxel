using OpenTK.Mathematics;
using SharpWoxel.entities;

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
        private readonly Block[,,] _blocks; // int default value is 0
        private readonly Chunk[] _neighbours;
        private const int _neighbourCount = 6;

        public SimpleEntity Entity { get; set; }

        public Chunk(Vector3 position, Vector3i chunkSize) 
        {
            Entity = new SimpleEntity("../../../res/textureAtlas.png");
            Entity.Position = position;

            _chunkSize = chunkSize;
            _blocks = new Block[chunkSize.X, chunkSize.Y, chunkSize.Z];
            _neighbours = new Chunk[_neighbourCount];
        }

        public Block GetBlockLocal(int x, int y, int z)
        {
            if (IsBlockOutOfBounds(x, y, z))
                throw new Exception(string.Format("[Chunk::GetBlockLocal]: Block ({0},{1},{2}) is out of bounds", x, y, z));

            return _blocks[x, y, z];
        }

        public void SetBlockLocal(int x, int y, int z, Block block)
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
