using System.Text.RegularExpressions;

namespace AssetsTools
{
    public partial class AssetsManager
    {
        public ClassDatabaseFile LoadClassDatabase(Stream stream)
        {
            ClassFile = new ClassDatabaseFile();
            ClassFile.Read(new EndianReader(stream, true));
            return ClassFile;
        }

        public ClassDatabaseFile LoadClassDatabase(string path)
        {
            return LoadClassDatabase(File.OpenRead(path));
        }

        public ClassDatabaseFile LoadClassDatabaseFromPackage(string version, bool specific = false)
        {
            if (ClassPackage == null)
                throw new Exception("No class package loaded!");

            if (specific)
            {
                if (!version.StartsWith("U"))
                    version = "U" + version;
                var index = ClassPackage.header.files.FindIndex(f => f.name == version);
                if (index == -1)
                    return null;

                ClassFile = ClassPackage.files[index];
                return ClassFile;
            }
            else
            {
                var matchingFiles = new List<ClassDatabaseFile>();
                var matchingVersions = new List<UnityVersion>();

                if (version.StartsWith("U"))
                    version = version[1..];

                var versionParsed = new UnityVersion(version);

                for (var i = 0; i < ClassPackage.files.Count; i++)
                {
                    var file = ClassPackage.files[i];
                    for (int j = 0; j < file.header.unityVersions.Length; j++)
                    {
                        string unityVersion = file.header.unityVersions[j];
                        if (version == unityVersion)
                        {
                            ClassFile = file;
                            return ClassFile;
                        }
                        else if (WildcardMatches(version, unityVersion))
                        {
                            string fullUnityVersion = unityVersion;
                            if (fullUnityVersion.EndsWith("*"))
                                fullUnityVersion = file.header.unityVersions[1 - j];

                            matchingFiles.Add(file);
                            matchingVersions.Add(new UnityVersion(fullUnityVersion));
                        }
                    }
                }

                if (matchingFiles.Count == 1)
                {
                    ClassFile = matchingFiles[0];
                    return ClassFile;
                }
                else if (matchingFiles.Count > 0)
                {
                    var selectedIndex = 0;
                    var patchNumToMatch = versionParsed.Build;
                    var highestMatchingPatchNum = matchingVersions[selectedIndex].Build;

                    for (var i = 1; i < selectedIndex; i++)
                    {
                        var thisPatchNum = matchingVersions[selectedIndex].Build;
                        if (thisPatchNum > highestMatchingPatchNum && thisPatchNum <= patchNumToMatch)
                        {
                            selectedIndex = i;
                            highestMatchingPatchNum = thisPatchNum;
                        }
                    }

                    ClassFile = matchingFiles[selectedIndex];
                    return ClassFile;
                }
                return null;
            }
        }

        private static bool WildcardMatches(string test, string pattern)
        {
            return Regex.IsMatch(test, "^" + Regex.Escape(pattern).Replace("\\*", ".*") + "$");
        }

        public ClassDatabasePackage LoadClassPackage(Stream stream)
        {
            ClassPackage = new ClassDatabasePackage();
            ClassPackage.Read(new EndianReader(stream, true));
            return ClassPackage;
        }

        public ClassDatabasePackage LoadClassPackage(string path)
        {
            return LoadClassPackage(File.OpenRead(path));
        }
    }
}