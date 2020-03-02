using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GitBranchChecker.DataModels;
using System.Data;
using System.Diagnostics;
using System.IO;
using LibGit2Sharp;
using System.Windows.Forms;

namespace GitBranchChecker
{
    public class BranchChecker
    {
        public string filePath = "";
        public string rootFolder = "";
        public string gitPath = "";
        public string relativeFilePath = "";
        public RepoDataModel repo;
        public RepoParser parser = new RepoParser();
        
        public void SetFilePath(string filePath)
        {
            this.filePath = filePath;
            gitPath = GitPathFinder.FindFromFilePath(filePath, ref rootFolder);
            relativeFilePath = filePath.Substring(rootFolder.Length+1);
        }

        public DataTable Parse()
        {
            repo = parser.GetRepo(gitPath, relativeFilePath);
            return parser.Parse(repo);
        }

        public void Compare(CommitDataModel commit1, CommitDataModel commit2)
        {
            string commitFilePathLeft = GetCommitFile(commit1, 0);
            string commitFilePathRight = GetCommitFile(commit2, 1);
            string args = "\"" + commitFilePathLeft + "\" \"" + commitFilePathRight + "\"";
            Process.Start(BranchCheckerForm.winMergePath, args);
        }
        
        public string GetCommitFile(CommitDataModel commitModel, int i)
        {
            using (var repo = new Repository(gitPath))
            {
                Branch branch = repo.Branches[commitModel.parent.name];
                Commit commit = GetCommitByID(branch, commitModel.id);
                var blob = commit[relativeFilePath].Target as Blob;
                var dirInfo = Directory.CreateDirectory("temp");
                string tempPath = dirInfo.FullName + "\\commit" + i;
                File.WriteAllText(tempPath, blob.GetContentText());
                return tempPath;
            }
        }

        public Commit GetCommitByID(Branch branch, string id)
        {
            foreach (var commit in branch.Commits)
            {
                if (commit.Id.ToString() == id)
                    return commit;
            }
            return null;
        }
    }
}
