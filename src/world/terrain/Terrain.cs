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

        public Terrain(Vector3i size, Vector3i chunkSize)
        {
            _terrainSize = size;
            _chunkSize = chunkSize;
            _chunks = new List<Chunk>();

            // Create chunks
            for (int z = 0; z < size.Z; z++)
            {
                for (int y = 0; y < size.Y; y++)
                {
                    for (int x = 0; x < size.X; x++)
                    {
                        Vector3 position = chunkSize * new Vector3i(x, y, z);
                        _chunks.Add(new Chunk(position, chunkSize));
                    }
                }
            }
        }

        public abstract void GenerateTerrain();

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

        public void SetBlockGlobal(int x, int y, int z, int block)
        {
            // Convert to local block position
            int lx = x % _chunkSize.X;
            int ly = y % _chunkSize.Y;
            int lz = z % _chunkSize.Z;

            GetChunkFromGlobal(x, y, z).GetBlockLocal(lx, ly, lz);
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
