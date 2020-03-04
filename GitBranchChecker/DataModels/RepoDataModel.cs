using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibGit2Sharp;

namespace GitBranchChecker.DataModels
{
    public class RepoDataModel
    {
        public Dictionary<string, BranchDataModel> branches = new Dictionary<string, BranchDataModel>();
        public Dictionary<int, BranchDataModel> branchesByColumn = new Dictionary<int, BranchDataModel>();
        public Repository repo;
    }
}
