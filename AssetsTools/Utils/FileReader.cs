namespace AssetsTools.Utils
{
    public class FileReader : EndianReader
    {
        public string FullPath;
        public string FileName;
        public FileType FileType;

        public FileReader(string path, bool bigEndian = true) : this(path, File.OpenRead(path), bigEndian)
        {
        }

        public FileReader(string path, Stream stream, bool bigEndian = true) : base(stream, bigEndian)
        {
            FullPath = Path.GetFullPath(path);
            FileName = Path.GetFileName(path);
            FileType = DetectFileType();
        }

        private FileType DetectFileType()
        {
            // todo
            return default;
        }
    }
}
