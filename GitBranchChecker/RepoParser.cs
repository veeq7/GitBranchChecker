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
            DataTable table = new DataTable();
            var branchList = DictToNameList(repo.branches);
            foreach (var branchName in branchList)
            {
                table.Columns.Add(branchName);
            }
            List<string> previousCommitList = null;
            List<string> commitList = null;
            for (int x = 0; x < branchList.Count; x++)
            {
                if (!repo.branches.ContainsKey(branchList[x]))
                {
                    x++;
                    continue;
                }
                var branch = repo.branches[branchList[x]];

                previousCommitList = commitList;
                commitList = GetSortedCommitNameList(branch.commits, previousCommitList);

                for (int y = 0; y < commitList.Count; y++)
                {
                    DataRow row;
                    if (table.Rows.Count <= y)
                    {
                        row = table.NewRow();
                        table.Rows.Add(row);
                    } else
                    {
                        row = table.Rows[y];
                    }
                    if (!branch.commits.ContainsKey(commitList[y])) continue;
                    CommitDataModel commitModel = branch.commits[commitList[y]];

                    branch.commitsByRow.Add(y, commitModel);

                    row[x] = commitModel.name;
                }
                repo.branchesByColumn.Add(x, branch);
            }
            return table;
        }

        public List<string> GetSortedCommitNameList(Dictionary<string, CommitDataModel> dict, List<string> previousDict)
        {
            List<string> list = new List<string>();
            int i = 0;
            int offset = 0;
            foreach (var name in dict.Keys)
            {
                list.Add(name);
                //if (previousDict == null || name == previousDict[i])
                //{
                //    list.Add(name);
                //} else
                //{
                //    list.Add("");
                //}
                i++;
            }
            return list;
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

        public RepoDataModel GetRepo(string gitPath)
        {
            using (var repo = new Repository(gitPath))
            {
                RepoDataModel repoModel = new RepoDataModel();
                foreach(var branch in repo.Branches)
                {
                    BranchDataModel branchModel = new BranchDataModel();
                    branchModel.name = branch.FriendlyName;
                    branchModel.branch = branch;

                    foreach (var commit in branch.Commits)
                    {
                        CommitDataModel commitModel = new CommitDataModel();
                        commitModel.id = commit.Id.ToString();
                        commitModel.name = commit.Message;
                        commitModel.commit = commit;

                        branchModel.commits.Add(commitModel.id, commitModel);
                    }

                    repoModel.branches.Add(branchModel.name, branchModel);
                }
                return repoModel;
            }
        }

    }
}
