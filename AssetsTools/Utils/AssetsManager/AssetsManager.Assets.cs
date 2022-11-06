namespace AssetsTools
{
    public partial class AssetsManager
    {
        public AssetsFile LoadAssetsFile(Stream stream, string path, bool loadDeps, AssetBundleFile parentBundle = null)
        {
            AssetsFile file;
            var pathLower = path.ToLower();
            if (!LoadedFiles.ContainsKey(pathLower))
            {
                file = new AssetsFile(stream, path);
                if (parentBundle != null)
                {
                    bundleMap[file] = parentBundle;
                }
                LoadedFiles[pathLower] = file;
            }
            else
            {
                file = LoadedFiles[pathLower];
            }

            if (loadDeps)
            {
                if (parentBundle == null)
                {
                    LoadDependencies(file);
                }
                else
                {
                    LoadBundleDependencies(file, parentBundle);
                }
            }
            return file;
        }

        public AssetsFile LoadAssetsFile(FileStream stream, bool loadDeps)
        {
            if (string.IsNullOrEmpty(activeDirectory))
            {
                activeDirectory = Path.GetDirectoryName(stream.Name);
            }
            return LoadAssetsFile(stream, stream.Name, loadDeps);
        }

        public AssetsFile LoadAssetsFile(string path, bool loadDeps)
        {
            if (string.IsNullOrEmpty(activeDirectory))
            {
                activeDirectory = Path.GetDirectoryName(path);
            }
            return LoadAssetsFile(File.OpenRead(path), loadDeps);
        }

        public bool UnloadAssetsFile(string path)
        {
            var fullPathLower = Path.GetFullPath(path).ToLower();
            if (LoadedFiles.ContainsKey(fullPathLower))
            {
                var file = LoadedFiles[fullPathLower];
                if (bundleMap.TryGetValue(file, out AssetBundleFile bundle))
                {
                    bundleLoadedFilesMap[bundle].Remove(file);
                    bundleMap.Remove(file);
                }
                LoadedFiles.Remove(fullPathLower);
                file.Close();
                return true;
            }
            return false;
        }

        public bool UnloadAllAssetsFiles(bool clearCache = false)
        {
            if (clearCache)
            {
                templateFieldCache.Clear();
                monoTemplateFieldCache.Clear();
            }

            if (LoadedFiles.Count != 0)
            {
                foreach (var filePair in LoadedFiles)
                {
                    filePair.Value.Close();
                }
                bundleLoadedFilesMap.Clear();
                bundleMap.Clear();
                LoadedFiles.Clear();
                return true;
            }
            return false;
        }

        public void UnloadAll(bool unloadClassData = false)
        {
            UnloadAllAssetsFiles(true);
            UnloadAllBundleFiles();
            if (unloadClassData)
            {
                ClassPackage = null;
                ClassFile = null;
            }
        }
    }
}