namespace AssetsTools
{
    public partial class AssetsManager
    {
        public AssetTypeTemplateField GetTemplateBaseField(AssetsFile file, AssetFileInfoEx info, bool forceFromCldb = false)
        {
            var scriptIndex = AssetHelper.GetScriptIndex(file, info);
            var fixedId = AssetHelper.FixAudioID(info.curFileType);

            var hasTypeTree = file.typeTree.hasTypeTree;
            AssetTypeTemplateField baseField;
            if (useTemplateFieldCache && templateFieldCache.ContainsKey(fixedId))
            {
                baseField = templateFieldCache[fixedId];
            }
            else
            {
                baseField = new AssetTypeTemplateField();
                if (hasTypeTree && !forceFromCldb)
                {
                    baseField.From0D(AssetHelper.FindTypeTreeTypeByID(file.typeTree, fixedId, scriptIndex), 0);
                }
                else
                {
                    baseField.FromClassDatabase(ClassFile, AssetHelper.FindAssetClassByID(ClassFile, fixedId), 0);
                }

                if (useTemplateFieldCache)
                {
                    templateFieldCache[fixedId] = baseField;
                }
            }

            return baseField;
        }

        public AssetTypeValueField GetBaseField(AssetsFile file, AssetFileInfoEx info)
        {
            var tempField = GetTemplateBaseField(file, info);
            file.reader.Position = info.absoluteFilePos;
            var valueField = tempField.MakeValue(file.reader);
            return valueField;
        }

        //public AssetTypeValueField GetMonoBaseFieldCached(AssetsFile inst, AssetFileInfoEx info, string managedPath)
        //{
        //    var file = inst.file;
        //    var scriptIndex = AssetHelper.GetScriptIndex(file, info);
        //    if (scriptIndex == 0xFFFF)
        //        return null;

        //    string scriptName;
        //    if (!inst.monoIdToName.ContainsKey(scriptIndex))
        //    {
        //        var scriptAti = GetExtAsset(inst, GetTypeInstance(inst.file, info).GetBaseField().Get("m_Script")).instance;

        //        // Couldn't find asset
        //        if (scriptAti == null)
        //            return null;

        //        scriptName = scriptAti.GetBaseField().Get("m_Name").GetValue().AsString();
        //        var scriptNamespace = scriptAti.GetBaseField().Get("m_Namespace").GetValue().AsString();
        //        var assemblyName = scriptAti.GetBaseField().Get("m_AssemblyName").GetValue().AsString();

        //        if (scriptNamespace == string.Empty)
        //        {
        //            scriptNamespace = "-";
        //        }

        //        scriptName = $"{assemblyName}.{scriptNamespace}.{scriptName}";
        //        inst.monoIdToName[scriptIndex] = scriptName;
        //    }
        //    else
        //    {
        //        scriptName = inst.monoIdToName[scriptIndex];
        //    }

        //    if (monoTemplateFieldCache.ContainsKey(scriptName))
        //    {
        //        var baseTemplateField = monoTemplateFieldCache[scriptName];
        //        var baseAti = new AssetTypeInstance(baseTemplateField, file.reader, info.absoluteFilePos);
        //        return baseAti.GetBaseField();
        //    }

        //    var baseValueField = MonoDeserializer.GetMonoBaseField(this, inst, info, managedPath);
        //    monoTemplateFieldCache[scriptName] = baseValueField.TemplateField;
        //    return baseValueField;
        //}
    }
}
