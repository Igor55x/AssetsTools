namespace AssetsTools
{
    public class AssetID
    {
        public string FileName;
        public AssetPPtr PPtr;

        public AssetID(string fileName, AssetPPtr pptr)
        {
            FileName = fileName;
            PPtr = pptr;
        }

        public override bool Equals(object obj)
        {
            if (obj is AssetID cobj)
            {
                return cobj.FileName == FileName && cobj.PPtr == PPtr;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FileName, PPtr);
        }
    }
}