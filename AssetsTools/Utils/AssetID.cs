namespace AssetsTools
{
    public class AssetID
    {
        public string FileName;
        public long PathID;

        public AssetID(string fileName, long pathId)
        {
            FileName = fileName;
            PathID = pathId;
        }

        public override bool Equals(object obj)
        {
            if (obj is AssetID cobj)
            {
                return cobj.FileName == FileName && cobj.PathID == PathID;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FileName, PathID);
        }
    }
}