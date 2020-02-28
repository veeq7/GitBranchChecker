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
            foreach (var branchkv in repo.branches)
            {
                var branch = branchkv.Value;
                table.Columns.Add(branch.name);
            }
            var branchList = DictToNameList(repo.branches);
            for (int x = 0; x < branchList.Count; x++)
            {
                var branch = repo.branches[branchList[x]];
             
                var commitList = DictToNameList(branch.commits);

                for (int y = 0; y < commitList.Count; y++)
                {
                    DataRow row;
                    CommitDataModel commitModel = branch.commits[commitList[y]];
                    if (table.Rows.Count <= y)
                    {
                        row = table.NewRow();
                        table.Rows.Add(row);
                    } else
                    {
                        row = table.Rows[y];
                    }

                    branch.commitsByRow.Add(y, commitModel);

                    row[x] = commitModel.name;
                }
                repo.branchesByColumn.Add(x, branch);
            }
            return table;
        }

        public List<string> DictToNameList<T>(Dictionary<string, T> branches)
        {
            List<string> list = new List<string>();
            foreach (var kv in branches)
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
