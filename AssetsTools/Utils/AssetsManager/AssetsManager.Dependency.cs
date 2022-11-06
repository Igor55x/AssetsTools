namespace AssetsTools
{
    public partial class AssetsManager
    {
        /*
        public void UpdateDependencies(AssetsFile ofFile)
        {
            var depList = ofFile.file.dependencies;
            for (var i = 0; i < depList.dependencyCount; i++)
            {
                var dep = depList.dependencies[i];
                var index = LoadedFiles.FindIndex(f => Path.GetFileName(dep.assetPath.ToLower()) == Path.GetFileName(f.path.ToLower()));
                if (index != -1)
                {
                    ofFile.dependencies[i] = LoadedFiles[index];
                }
            }
        }

        public void UpdateDependencies()
        {
            for (var i = 0; i < LoadedFiles.Count; i++)
            {
                UpdateDependencies(LoadedFiles[i]);
            }
        }*/

        public void LoadDependencies(AssetsFile depFile)
        {
            for (var i = 0; i < depFile.dependencies.dependencies.Count; i++)
            {
                var depPath = depFile.dependencies.dependencies[i].assetPath;

                if (depPath == string.Empty)
                {
                    continue;
                }

                var depPathLower = depPath.ToLower();
                if (!LoadedFiles.ContainsKey(depPathLower))
                {
                    var absPath = Path.Combine(activeDirectory, depPath);
                    if (File.Exists(absPath))
                    {
                        LoadAssetsFile(File.OpenRead(absPath), true);
                    }
                }
            }
        }

        public void LoadBundleDependencies(AssetsFile depFile, AssetBundleFile depBundle)
        {
            for (var i = 0; i < depFile.dependencies.dependencies.Count; i++)
            {
                var depPath = depFile.dependencies.dependencies[i].assetPath;
                var depPathLower = depPath.ToLower();
                if (!LoadedFiles.ContainsKey(depPathLower))
                {
                    var bunPath = Path.GetFileName(depPath);
                    var bunIndex = depBundle.GetFileIndex(bunPath);
                    var absPath = Path.Combine(activeDirectory, depPath);

                    if (bunIndex != -1)
                    {
                        LoadAssetsFileFromBundle(depBundle, bunIndex, true);
                    }
                    else if (File.Exists(absPath))
                    {
                        LoadAssetsFile(File.OpenRead(absPath), true);
                    }
                }
            }
        }
    }
}