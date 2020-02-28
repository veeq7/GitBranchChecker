using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GitBranchChecker.DataModels;
using System.Data;
using System.Diagnostics;

namespace GitBranchChecker
{
    public class BranchChecker
    {
        public string filePath = "";
        public string gitPath = "";
        public RepoDataModel repo;
        public RepoParser parser = new RepoParser();
        
        public void SetFilePath(string filePath)
        {
            this.filePath = filePath;
            gitPath = GitPathFinder.FindFromFilePath(filePath);
        }

        public DataTable Parse()
        {
            repo = parser.GetRepo(gitPath);
            return parser.Parse(repo);
        }

        public void Compare(CommitDataModel commit1, CommitDataModel commit2)
        {
            string[] filePaths = GetFilePath(commit1, commit2);
            Process.Start(BranchCheckerForm.winMergeCommand + filePaths[0] + " " + filePaths[1]);
        }

        public string[] GetFilePath(CommitDataModel commit1, CommitDataModel commit2)
        {
            string[] filePaths = new string[2];
            return filePaths;
        }
    }
}
