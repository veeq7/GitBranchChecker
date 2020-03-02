using System.IO;

namespace GitBranchChecker
{
    class GitPathFinder
    {
        public static string FindFromFilePath(string filePath, ref string rootFilePath)
        {
            return FindFromFolderPath(Path.GetDirectoryName(filePath), ref rootFilePath);
        }

        public static string FindFromFolderPath(string folderPath, ref string rootFilePath)
        {
            if (Directory.Exists(folderPath + "\\.git"))
            {
                rootFilePath = folderPath;
                return folderPath + "\\.git";
            }
            if (Directory.GetParent(folderPath).FullName == "") return "";
            return FindFromFolderPath(Directory.GetParent(folderPath).FullName, ref rootFilePath);
        }
    }
}