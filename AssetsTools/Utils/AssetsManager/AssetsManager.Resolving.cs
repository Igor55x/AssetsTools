namespace AssetsTools
{
    public partial class AssetsManager
    {
        public AssetExternal GetExternal(AssetsFile relativeTo, int fileId, long pathId, bool onlyGetInfo = false)
        {
            var ext = new AssetExternal
            {
                info = null,
                baseField = null,
                file = null
            };

            if (fileId == 0 && pathId == 0)
            {
                return ext;
            }
            else if (fileId != 0)
            {
                var dep = GetDependency(relativeTo, fileId - 1);

                if (dep == null)
                    return ext;

                ext.file = dep;
                ext.info = dep.GetAssetInfo(pathId);

                if (ext.info == null)
                    return ext;

                if (!onlyGetInfo)
                    ext.baseField = GetBaseField(dep, ext.info);
                else
                    ext.baseField = null;

                return ext;
            }
            else
            {
                ext.file = relativeTo;
                ext.info = relativeTo.GetAssetInfo(pathId);

                if (ext.info == null)
                    return ext;

                if (!onlyGetInfo)
                    ext.baseField = GetBaseField(relativeTo, ext.info);
                else
                    ext.baseField = null;

                return ext;
            }
        }

        public AssetsFile GetDependency(AssetsFile file, int index, bool loadIfNotLoaded = true)
        {
            if (index < 0 || index >= file.dependencies.dependencies.Count)
            {
                return null;
            }

            var depName = Path.GetFileName(file.dependencies.dependencies[index].assetPath);
            var depNameLower = depName.ToLower();
            if (LoadedFiles.ContainsKey(depNameLower))
            {
                return LoadedFiles[depNameLower];
            }
            else if (loadIfNotLoaded)
            {
                var depPath = Path.Combine(activeDirectory, depName);
                if (File.Exists(depPath))
                {
                    return LoadAssetsFile(depPath, false);
                }
            }

            return null;
        }

        public AssetExternal GetExternal(AssetsFile relativeTo, AssetTypeValueField baseField, bool onlyGetInfo = false)
        {
            var fileId = baseField.Get("m_FileID").GetValue().AsInt();
            var pathId = baseField.Get("m_PathID").GetValue().AsInt64();
            return GetExternal(relativeTo, fileId, pathId, onlyGetInfo);
        }
    }
}
