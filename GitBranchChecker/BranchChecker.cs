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
            relativeFilePath = filePath.Substring(rootFolder.Length + 1);
        }

        public DataTable Parse()
        {
            repo = parser.GetRepo(gitPath, relativeFilePath);
            return parser.Parse(repo);
        }

        public DataTable Update()
        {
            return parser.Parse(repo);
        }

        public void OpenFile(List<CommitDataModel> commits)
        {
            int i = 0;
            commits.OrderByDescending(x => x.commit.Committer.When).ToList().ForEach((x) => {
                Process.Start(BranchCheckerForm.configInfo.textEditor, GetCommitFile(x, i++));
            });
        }

        public void Compare(List<CommitDataModel> commits)
        {
            var orderedCommits = commits.OrderByDescending(x => x.commit.Committer.When).ToList();
            string fileArgs = "\"" + String.Join("\" \"", orderedCommits.Select((x, i) => GetCommitFile(x, i))) + "\"";
            string nameArgs = MakeNameArgs(orderedCommits);
            Process.Start(BranchCheckerForm.configInfo.winMergePath, fileArgs + " " + nameArgs);
        }

        private string MakeNameArgs(List<CommitDataModel> commits)
        {
            if (commits.Count == 2)
            {
                return "/dl \"" + commits[0].ToString() + "\" /dr \"" + commits[1].ToString() + "\"";
            }
            else if (commits.Count == 3)
            {
                return "/dl \"" + commits[0].ToString() + "\" /dm \"" + commits[1].ToString() + "\" /dr \"" + commits[2].ToString() + "\"";
            }
            return "";
        }

        public string GetCommitFile(CommitDataModel commitModel, int i)
        {
            using (var repo = new Repository(gitPath))
            {
                Branch branch = repo.Branches[commitModel.parent.name];
                Commit commit = GetCommitByID(branch, commitModel.id);
                var blob = commitModel.blob;
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
