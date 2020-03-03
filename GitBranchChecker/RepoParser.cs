using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GitBranchChecker.DataModels;
using System.Data;
using LibGit2Sharp;

namespace GitBranchChecker
{

    public class RepoParser
    {
        public DataTable Parse(RepoDataModel repo)
        {
            DataTable dataTable = new DataTable();

            foreach (var branchName in repo.branches.Keys)
            {
                dataTable.Columns.Add(branchName);
            }

            var indexCommits = MakeIndexCommitList(repo);
            int x = 0;
            foreach(var branch in repo.branches.Values)
            {
                int y = 0;
                foreach(var indexCommit in indexCommits)
                {
                    MakeRow(dataTable, x, branch, y, indexCommit);
                    y++;
                }
                repo.branchesByColumn.Add(x, branch);
                x++;
            }

            return dataTable;
        }

        private void MakeRow(DataTable dataTable, int x, BranchDataModel branch, int y, Commit indexCommit)
        {
            DataRow row;
            while (dataTable.Rows.Count <= y)
            {
                dataTable.Rows.Add(dataTable.NewRow());
            }
            row = dataTable.Rows[y];

            var commit = GetCommit(branch, indexCommit.Message);
            if (commit != null)
            {
                row[x] = commit.FormatDate() + " " + commit.commit.Sha + " " + commit.name;

                branch.commitsByRow.Add(y, commit);
            }
        }

        CommitDataModel GetCommit(BranchDataModel branch, string name)
        {
            foreach (var commit in branch.commits.Values)
            {
                if (commit.name == name)
                {
                    return commit;
                }
            }
            return null;
        }

        List<Commit> MakeIndexCommitList(RepoDataModel repo)
        {
            List<Commit> commits = new List<Commit>();
            foreach(var branch in repo.branches.Values)
            {
                foreach (var commit in branch.commits.Values)
                {
                    if (!IsCommitAdded(commit.commit, commits))
                    {
                        commits.Add(commit.commit);
                    }
                }
            }
            commits = commits.OrderByDescending(commit => commit.Committer.When).ToList();
            return commits;
        }

        bool IsCommitAdded(Commit commit, List<Commit> commits)
        {
            foreach (var other in commits)
            {
                if (other.Message == commit.Message)
                    return true;
            }
            return false;
        }

        public List<string> DictToNameList<T>(Dictionary<string, T> dict)
        {
            List<string> list = new List<string>();
            foreach (var kv in dict)
            {
                list.Add(kv.Key);
            }
            return list;
        }

        public RepoDataModel GetRepo(string gitPath, string fileRelativePath)
        {
            RepoDataModel repoModel = new RepoDataModel();
            repoModel.repo = new Repository(gitPath);

            foreach (var branch in repoModel.repo.Branches)
            {
                if (!IsBranchInFilter(branch) && !IsFilterEmpty())
                    continue;

                BranchDataModel branchModel = new BranchDataModel();
                branchModel.name = branch.FriendlyName;

                Commit previousCommit = null;
                foreach (var commit in branch.Commits)
                {
                    Blob currentCommitBlob;
                    try
                    {
                        currentCommitBlob = GetBlob(commit, fileRelativePath);
                    }
                    catch
                    {
                        continue;
                    }
                    if (previousCommit != null)
                    {
                        Blob previousCommitBlob = GetBlob(previousCommit, fileRelativePath);
                        if (previousCommitBlob.Sha == currentCommitBlob.Sha)
                            continue;
                    }

                    CommitDataModel commitModel = new CommitDataModel(commit, branchModel, currentCommitBlob);

                    branchModel.commits.Add(commitModel.id, commitModel);
                    previousCommit = commit;
                }

                repoModel.branches.Add(branchModel.name, branchModel);
            }
            return repoModel;
        }

        bool IsBranchInFilter(Branch branch)
        {
            foreach (var branchName in BranchCheckerForm.configInfo.branchNameFilter)
            {
                if (branch.FriendlyName == branchName)
                {
                    return true;
                }
            }
            return false;
        }

        bool IsFilterEmpty()
        {
            var count = BranchCheckerForm.configInfo.branchNameFilter.Count;
            return count == 0 || (count == 1 && String.IsNullOrEmpty(BranchCheckerForm.configInfo.branchNameFilter[0]));
        }

        public static Blob GetBlob(Commit commit, string relativePath)
        {
            string[] fragments = relativePath.Split('\\');

            TreeEntry entry = commit[fragments[0]];
            for (int i = 1; i <= fragments.Length; i++)
            {
                if (entry.TargetType == TreeEntryTargetType.Tree)
                {
                    Tree dir = entry.Target as Tree;
                    entry = dir[fragments[i]];
                } else if (entry.TargetType == TreeEntryTargetType.Blob)
                {
                    return entry.Target as Blob;
                }
            }
            return null;
        }

    }
}
