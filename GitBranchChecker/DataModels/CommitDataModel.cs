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
        public BranchDataModel parent;
        public Commit commit;
        public Blob blob;

        public CommitDataModel() { }

        public CommitDataModel(Commit commit, BranchDataModel parent, Blob blob)
        {
            this.id = commit.Id.ToString();
            this.name = commit.Message;
            this.commit = commit;
            this.parent = parent;
            this.blob = blob;
        }

        public string FormatDate()
        {
            var date = commit.Committer.When;
            return $"{AddTrailingZeroIfNeeded(date.Day)}.{AddTrailingZeroIfNeeded(date.Month)}.{date.Year} {AddTrailingZeroIfNeeded(date.Hour)}:{AddTrailingZeroIfNeeded(date.Minute)}";
        }

        public string AddTrailingZeroIfNeeded(int i)
        {
            if (i.ToString().Length == 1) return "0" + i;
            return i.ToString();
        }
    }
}
