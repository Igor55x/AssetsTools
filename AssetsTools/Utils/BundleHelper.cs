using AssetsTools.Utils;
using AssetsTools.Compression;

namespace AssetsTools
{
    public static class BundleHelper
    {
        public static byte[] LoadAssetDataFromBundle(AssetBundleFile bundle, int index)
        {
            bundle.GetFileRange(index, out var offset, out var length);
            var reader = bundle.Reader;
            reader.Position = offset;
            return reader.ReadBytes((int)length);
        }

        public static byte[] LoadAssetDataFromBundle(AssetBundleFile bundle, string name)
        {
            var index = bundle.GetFileIndex(name);
            if (index != -1)
                return LoadAssetDataFromBundle(bundle, index);

            return null;
        }

        public static AssetsFile LoadAssetFromBundle(AssetBundleFile bundle, int index)
        {
            bundle.GetFileRange(index, out var offset, out var length);
            var ss = new SegmentStream(bundle.Reader.BaseStream, offset, length);
            var reader = new EndianReader(ss, true);
            return new AssetsFile(reader);
        }

        public static AssetsFile LoadAssetFromBundle(AssetBundleFile bundle, string name)
        {
            var index = bundle.GetFileIndex(name);
            if (index != -1)
                return LoadAssetFromBundle(bundle, index);

            return null;
        }

        public static List<byte[]> LoadAllAssetsDataFromBundle(AssetBundleFile bundle)
        {
            var files = new List<byte[]>();
            var fileCount = bundle.FileCount;
            for (var i = 0; i < fileCount; i++)
            {
                if (bundle.IsAssetsFile(i))
                {
                    files.Add(LoadAssetDataFromBundle(bundle, i));
                }
            }
            return files;
        }

        public static List<AssetsFile> LoadAllAssetsFromBundle(AssetBundleFile bundle)
        {
            var files = new List<AssetsFile>();
            var fileCount = bundle.FileCount;
            for (var i = 0; i < fileCount; i++)
            {
                if (bundle.IsAssetsFile(i))
                {
                    files.Add(LoadAssetFromBundle(bundle, i));
                }
            }
            return files;
        }

        public static AssetBundleFile UnpackBundle(AssetBundleFile file, bool freeOriginalStream = true)
        {
            var ms = new MemoryStream();
            file.Unpack(file.Reader, new EndianWriter(ms, true));
            ms.Position = 0;

            var newFile = new AssetBundleFile();
            newFile.Read(new EndianReader(ms, true));

            if (freeOriginalStream)
            {
                file.Reader.Close();
            }
            return newFile;
        }

        public static AssetBundleFile UnpackBundleToStream(AssetBundleFile file, Stream stream, bool freeOriginalStream = true)
        {
            file.Unpack(file.Reader, new EndianWriter(stream, true));
            stream.Position = 0;

            var newFile = new AssetBundleFile();
            newFile.Read(new EndianReader(stream, true));

            if (freeOriginalStream)
            {
                file.Reader.Close();
            }
            return newFile;
        }

        public static AssetBundleDirectoryInfo GetDirInfo(AssetBundleFile bundle, int index)
        {
            return bundle.Metadata.DirectoryInfo[index];
        }

        public static AssetBundleDirectoryInfo GetDirInfo(AssetBundleFile bundle, string name)
        {
            foreach (var info in bundle.Metadata.DirectoryInfo)
            {
                if (info.Name == name)
                {
                    return info;
                }
            }
            return null;
        }
    }
}
