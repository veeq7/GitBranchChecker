using System.IO;

namespace GitBranchChecker
{
    class GitPathFinder
    {
        public static string FindFromFilePath(string filePath)
        {
            return FindFromFolderPath(Path.GetDirectoryName(filePath));
        }

        public static string FindFromFolderPath(string folderPath)
        {
            if (Directory.Exists(folderPath + "\\.git"))
            {
                return folderPath + "\\.git";
            }
            if (Directory.GetParent(folderPath).FullName == "") return "";
            return FindFromFolderPath(Directory.GetParent(folderPath).FullName);
        }
    }
}