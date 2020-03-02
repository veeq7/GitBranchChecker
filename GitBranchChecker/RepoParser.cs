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

            int x = 0;
            foreach(var branch in repo.branches.Values)
            {
                int y = 0;//= x;
                foreach(var commit in branch.commits.Values)
                {
                    //while (ShouldShift(dataTable, x, y, commit.name))
                    //{
                    //    y--;
                    //}

                    DataRow row;
                    while (dataTable.Rows.Count <= y)
                    {
                        dataTable.Rows.Add(dataTable.NewRow());
                    }
                    row = dataTable.Rows[y];

                    row[x] = commit.name;

                    branch.commitsByRow.Add(y, commit);
                    y++;// y+= repo.branches.Count();
                }
                //repo.branchesByColumn.Add(x, branch);
                x++;
            }

            return dataTable;
        }

        bool ShouldShift(DataTable dataTable, int x, int y, string currentDescription)
        {
            return false;
            //string oldDescription = dataTable.Rows[y][x];
            //for (int i = x; i > 0; i--)
            //if (oldDescription != currentDescription || String.IsNullOrEmpty(currentDescription))
            //return true;
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
                BranchDataModel branchModel = new BranchDataModel();
                branchModel.name = branch.FriendlyName;

                Commit previousCommit = null;
                foreach (var commit in branch.Commits)
                {
                    Blob currentCommitBlob;
                    try
                    {
                        currentCommitBlob = commit[fileRelativePath].Target as Blob;
                    }
                    catch
                    {
                        continue;
                    }
                    if (previousCommit != null)
                    {
                        Blob previousCommitBlob = previousCommit[fileRelativePath].Target as Blob;
                        if (previousCommitBlob.Sha == currentCommitBlob.Sha)
                            continue;
                    }
                    
                    CommitDataModel commitModel = new CommitDataModel();
                    commitModel.id = commit.Id.ToString();
                    commitModel.name = commit.Message;
                    commitModel.parent = branchModel;
                    
                    branchModel.commits.Add(commitModel.id, commitModel);
                    previousCommit = commit;
                }

                repoModel.branches.Add(branchModel.name, branchModel);
            }
            return repoModel;
            
            
        }

    }
}
