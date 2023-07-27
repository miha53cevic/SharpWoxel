using OpenTK.Mathematics;
using SharpWoxel.world.blocks;
using SharpWoxel.util;
using glObjects;

namespace SharpWoxel.world.terrain
{
    abstract class Terrain
    {
        protected Vector3i _terrainSize; // the size of the world
        protected Vector3i _chunkSize;
        protected List<Chunk> _chunks;

        public Vector3i TerrainSize { get => _terrainSize; }
        public Vector3i ChunkSize { get => _chunkSize; }

        public Terrain(Vector3i size, Vector3i chunkSize)
        {
            _terrainSize = size; // number of chunks
            _chunkSize = chunkSize;
            _chunks = new List<Chunk>();

            // Create chunks
            // Bitan redosljed za IndexFrom3D, mora biti x,y,z for loop order
            for (int x = 0; x < size.X; x++)
            {
                for (int y = 0; y < size.Y; y++)
                {
                    for (int z = 0; z < size.Z; z++)
                    {
                        Vector3 position = new Vector3i(chunkSize.X * x, chunkSize.Y * y, chunkSize.Z * z);
                        _chunks.Add(new Chunk(position, chunkSize));
                    }
                }
            }

            // Set each chunks neighbours
            for (int x = 0; x < size.X; x++)
            {
                for (int y = 0; y < size.Y; y++)
                {
                    for (int z = 0; z < size.Z; z++)
                    {
                        bool left = false, right = false, front = false, back = false, top = false, bottom = false; // hasNeighbouring chunk
                        if (x != 0) left = true;
                        if (y != 0) bottom = true;
                        if (z != 0) back = true;
                        if (x != size.X - 1) right = true;
                        if (y != size.Y - 1) top = true;
                        if (z != size.Z - 1) front = true;

                        // boolean values tell us if a given chunk can have a neighbour on that side and if it can
                        // we add a reference to that neighbour to the given chunk
                        if (left)
                            GetChunkFromLocal(x, y, z).SetNeighbour(NeighbourDirection.WEST, GetChunkFromLocal(x - 1, y, z));
                        if (right)
                            GetChunkFromLocal(x, y, z).SetNeighbour(NeighbourDirection.EAST, GetChunkFromLocal(x + 1, y, z));
                        if (back)
                            GetChunkFromLocal(x, y, z).SetNeighbour(NeighbourDirection.NORTH, GetChunkFromLocal(x, y, z - 1));
                        if (front)
                            GetChunkFromLocal(x, y, z).SetNeighbour(NeighbourDirection.SOUTH, GetChunkFromLocal(x, y, z + 1));
                        if (top)
                            GetChunkFromLocal(x, y, z).SetNeighbour(NeighbourDirection.ABOVE, GetChunkFromLocal(x, y + 1, z));
                        if (bottom)
                            GetChunkFromLocal(x, y, z).SetNeighbour(NeighbourDirection.BELOW, GetChunkFromLocal(x, y - 1, z));
                    }
                }
            }
        }

        // Fill out chunk blocks, chunk.BuildMesh() is called after this function in GenerateTerrain()
        protected abstract void Generate();

        // okvirna metoda
        public virtual void GenerateTerrain()
        {
            Generate();
            foreach (var chunk in _chunks)
            {
                chunk.BuildMesh();
            }
        }

        public void Render(Shader shader, Camera camera)
        {
            foreach (var chunk in _chunks)
            {
                chunk.Entity.Render(shader, camera);
            }
        }

        public IBlock GetBlockGlobal(int x, int y, int z)
        {
            // Convert to local block position
            int lx = x % _chunkSize.X;
            int ly = y % _chunkSize.Y;
            int lz = z % _chunkSize.Z;

            return GetChunkFromGlobal(x, y, z).GetBlockLocal(lx, ly, lz);
        }

        public void SetBlockGlobal(int x, int y, int z, IBlock block)
        {
            // Convert to local block position
            int lx = x % _chunkSize.X;
            int ly = y % _chunkSize.Y;
            int lz = z % _chunkSize.Z;

            GetChunkFromGlobal(x, y, z).SetBlockLocal(lx, ly, lz, block);
        }

        public Chunk GetChunkFromGlobal(int x, int y, int z)
        {
            // Get the chunk from global block position
            int cx = x / _chunkSize.X;
            int cy = y / _chunkSize.Y;
            int cz = z / _chunkSize.Z;

            if (IsChunkOutOfBounds(cx, cy, cz))
                throw new Exception(string.Format("[Terrain::GetChunkFromGlobal]: Chunk ({0},{1},{2}) is out of bounds", cx, cy, cz));

            return _chunks[Maths.IndexFrom3D(cx, cy, cz, _terrainSize.Y, _terrainSize.Z)];
        }

        public Chunk GetChunkFromLocal(int x, int y, int z)
        {
            if (IsChunkOutOfBounds(x, y, z))
                throw new Exception(string.Format("[Terrain::GetChunkFromLocal]: Chunk ({0},{1},{2}) is out of bounds", x, y, z));

            return _chunks[Maths.IndexFrom3D(x, y, z, _terrainSize.Y, _terrainSize.Z)];
        }

        private bool IsChunkOutOfBounds(int x, int y, int z)
        {
            if (x < 0 || x >= _terrainSize.X || y < 0 || y >= _terrainSize.Y || z < 0 || z >= _terrainSize.Z)
                return true;
            else return false;
        }
    }
}
