namespace AssetsTools.Utils
{
    public static class ImportHelper
    {
        public static void MergeSplitAssets(string path)
        {
            var splitFiles = Directory.GetFiles(path, "*.split0", SearchOption.AllDirectories);
            foreach (var splitFile in splitFiles)
            {
                var destFile = Path.GetFileNameWithoutExtension(splitFile);
                var destPath = Path.GetDirectoryName(splitFile);
                var destFull = Path.Combine(destPath, destFile);
                if (!File.Exists(destFull))
                {
                    var splitParts = Directory.GetFiles(destPath, destFile + ".split*");
                    using var destStream = File.Create(destFull);
                    for (int i = 0; i < splitParts.Length; i++)
                    {
                        var splitPart = string.Format("{0}.split{1}", destFull, i);
                        using var sourceStream = File.OpenRead(splitPart);
                        sourceStream.CopyTo(destStream);
                    }
                }
            }
        }

        public static string[] ProcessSplitFiles(string[] selectedFiles)
        {
            var processedFiles = new HashSet<string>();
            for (var i = 0; i < selectedFiles.Length; i++)
            {
                var selectedFile = selectedFiles[i];
                if (!Path.GetExtension(selectedFile).StartsWith(".split"))
                {
                    processedFiles.Add(selectedFile);
                }
            }
            return processedFiles.ToArray();
        }
    }
}
