using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibGit2Sharp;

namespace GitBranchChecker.DataModels
{
    public class CommitDataModel
    {
        public string id;
        public string name;
        public Commit commit;
    }
}
