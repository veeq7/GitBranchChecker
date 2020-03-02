using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibGit2Sharp;

namespace GitBranchChecker.DataModels
{
    public class BranchDataModel
    {
        public string name;
        public Dictionary<string, CommitDataModel> commits = new Dictionary<string, CommitDataModel>();
        public Dictionary<int, CommitDataModel> commitsByRow = new Dictionary<int, CommitDataModel>();
    }
}
