using System.Diagnostics.CodeAnalysis;

namespace AssetsTools
{
    public class AssetPPtr
    {
        public int FileID;
        public long PathID;

        public void Read(EndianReader reader)
        {
            FileID = reader.ReadInt32();
            reader.Align();
            PathID = reader.ReadInt64();
            reader.Align();
        }

        public void Write(EndianWriter writer)
        {
            writer.Write(FileID);
            writer.Align();
            writer.Write(PathID);
            writer.Align();
        }

        public override bool Equals([NotNullWhen(true)] object obj)
        {
            if (obj is AssetPPtr pptr)
            {
                return Equals(pptr);
            }
            return false;
        }

        public bool Equals(AssetPPtr other)
        {
            return FileID == other.FileID && PathID == other.PathID;
        }

        public static bool operator ==(AssetPPtr left, AssetPPtr right) => left.Equals(right);

        public static bool operator !=(AssetPPtr left, AssetPPtr right) => !left.Equals(right);

        public override int GetHashCode()
        {
            return HashCode.Combine(FileID, PathID);
        }

        public override string ToString()
        {
            return string.Format("File ID: {0}, Path ID: {1}", FileID, PathID);
        }
    }
}
