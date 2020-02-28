using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitBranchChecker
{
    public class BranchChecker
    {
        public string filePath = "";
        public string gitPath = "";
        
        public void SetFilePath(string filePath)
        {
            this.filePath = filePath;
            gitPath = GitPathFinder.FindFromFilePath(filePath);
        }
    }
}
