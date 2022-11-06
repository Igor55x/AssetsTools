using AssetsTools.Utils;

namespace AssetsTools
{
    public partial class AssetsManager
    {
        public AssetBundleFile LoadBundleFile(Stream stream, string path, bool unpackIfPacked = true)
        {
            AssetBundleFile bundle;
            var fullPathLower = Path.GetFullPath(path).ToLower();
            if (!LoadedBundles.ContainsKey(fullPathLower))
            {
                bundle = new AssetBundleFile();
                bundle.Read(new EndianReader(stream));
                LoadedBundles[fullPathLower] = bundle;
                bundleLoadedFilesMap[bundle] = new List<AssetsFile>();
            }
            else
            {
                bundle = LoadedBundles[fullPathLower];
            }
            return bundle;
        }

        public AssetBundleFile LoadBundleFile(FileStream stream, bool unpackIfPacked = true)
        {
            return LoadBundleFile(stream, Path.GetFullPath(stream.Name), unpackIfPacked);
        }

        public AssetBundleFile LoadBundleFile(string path, bool unpackIfPacked = true)
        {
            return LoadBundleFile(File.OpenRead(path), unpackIfPacked);
        }

        public bool UnloadBundleFile(string path, bool unloadAssets = true)
        {
            var fullPathLower = Path.GetFullPath(path).ToLower();
            if (LoadedBundles.ContainsKey(fullPathLower))
            {
                var bundle = LoadedBundles[fullPathLower];
                bundle.Close();

                if (unloadAssets)
                {
                    // could be faster...
                    foreach (var assetBunPair in bundleMap)
                    {
                        if (assetBunPair.Value == bundle)
                            assetBunPair.Key.Close();
                    }
                }

                LoadedBundles.Remove(fullPathLower);
                bundleLoadedFilesMap.Remove(bundle);
                return true;
            }
            return false;
        }

        public bool UnloadAllBundleFiles(bool unloadAssets = true)
        {
            if (LoadedBundles.Count != 0)
            {
                foreach (var nameBunPair in LoadedBundles)
                {
                    var bundle = nameBunPair.Value;
                    bundle.Close();

                    if (unloadAssets)
                    {
                        foreach (var assetBunPair in bundleMap)
                        {
                            if (assetBunPair.Value == bundle)
                                assetBunPair.Key.Close();
                        }
                    }
                }
                LoadedBundles.Clear();
                bundleLoadedFilesMap.Clear();
                return true;
            }
            return false;
        }

        public AssetsFile LoadAssetsFileFromBundle(AssetBundleFile bundle, int index, bool loadDeps = false)
        {
            var assetMemPath = Path.Combine(bundle.Path, bundle.GetFileName(index));

            var fullPathLower = Path.GetFullPath(assetMemPath).ToLower();
            if (!LoadedFiles.ContainsKey(fullPathLower))
            {
                if (bundle.IsAssetsFile(index))
                {
                    bundle.GetFileRange(index, out var offset, out var length);
                    var ss = new SegmentStream(bundle.Reader.BaseStream, offset, length);
                    var file = LoadAssetsFile(ss, assetMemPath, loadDeps, bundle);
                    bundleLoadedFilesMap[bundle].Add(file);
                    return file;
                }
            }
            else
            {
                return LoadedFiles[fullPathLower];
            }
            return null;
        }

        public AssetsFile LoadAssetsFileFromBundle(AssetBundleFile bundle, string name, bool loadDeps = false)
        {
            var index = bundle.GetFileIndex(name);
            if (index >= 0 && index < bundle.FileCount)
            {
                return LoadAssetsFileFromBundle(bundle, index, loadDeps);
            }
            return null;
        }
    }
}