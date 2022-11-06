namespace AssetsTools
{
    public partial class AssetsManager
    {
        public bool useTemplateFieldCache = false;

        public ClassDatabaseFile ClassFile;
        public ClassDatabasePackage ClassPackage;
        public string activeDirectory;

        public Dictionary<string, AssetsFile> LoadedFiles;
        public Dictionary<string, AssetBundleFile> LoadedBundles;
        public Dictionary<AssetsFile, AssetBundleFile> bundleMap;
        public Dictionary<AssetBundleFile, List<AssetsFile>> bundleLoadedFilesMap;

        private Dictionary<AssetClassID, AssetTypeTemplateField> templateFieldCache { get; }
        private Dictionary<string, AssetTypeTemplateField> monoTemplateFieldCache { get; }

        public AssetsManager()
        {
            LoadedFiles = new Dictionary<string, AssetsFile>();
            LoadedBundles = new Dictionary<string, AssetBundleFile>();
            bundleMap = new Dictionary<AssetsFile, AssetBundleFile>();
            bundleLoadedFilesMap = new Dictionary<AssetBundleFile, List<AssetsFile>>();
            templateFieldCache = new Dictionary<AssetClassID, AssetTypeTemplateField>();
            monoTemplateFieldCache = new Dictionary<string, AssetTypeTemplateField>();
        }

        public void SetActiveDirectory(string directory)
        {
            activeDirectory = directory;
            //if (monoTempGenerator != null)
            //{
            //    monoTempGenerator.SetActiveDirectory(directory);
            //}
        }

        //public void LoadFiles(params string[] files)
        //{
        //    var path = Path.GetDirectoryName(Path.GetFullPath(files[0]));
        //    ImportHelper.MergeSplitAssets(path);
        //    var processedFiles = ImportHelper.ProcessSplitFiles(files);
        //    Load(processedFiles);
        //}

        //public void LoadFolder(string path)
        //{
        //    ImportHelper.MergeSplitAssets(path);
        //    var files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
        //    var processedFiles = ImportHelper.ProcessSplitFiles(files);
        //    Load(processedFiles);
        //}

        //private void Load(string[] files)
        //{
        //    //Progress.Reset(); 
        //    for (var i = 0; i < files.Length; i++)
        //    {
        //        LoadFile(files[i]);
        //    }
        //}

        //private void LoadFile(string fullName)
        //{
        //    var reader = new FileReader(fullName);
        //    LoadFile(reader);
        //}

        //private void LoadFile(FileReader reader)
        //{
        //    switch (reader.FileType)
        //    {
        //        case FileType.AssetsFile:
        //            LoadAssetsFile(reader.BaseStream, reader.FullPath, true);
        //            break;
        //        case FileType.BundleFile:
        //            LoadBundleFile(reader.BaseStream, reader.FullPath, false);
        //            break;
        //        case FileType.WebFile:
        //            LoadWebFile(reader);
        //            break;
        //        case FileType.GZipFile:
        //            LoadFile(DecompressGZip(reader));
        //            break;
        //        case FileType.BrotliFile:
        //            LoadFile(DecompressBrotli(reader));
        //            break;
        //        case FileType.ZipFile:
        //            LoadZipFile(reader);
        //            break;
        //    }
        //}
    }
}