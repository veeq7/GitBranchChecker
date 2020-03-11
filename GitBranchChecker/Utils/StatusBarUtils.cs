using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GitBranchChecker.Utils
{
    public static class StatusBarUtils
    {
        public static void SetProgress(string label, int current, int max)
        {
            BranchCheckerForm.Instance.ProgressBarInfo.Text = label;
            if (max <= 0)
                BranchCheckerForm.Instance.ProgressBar.Value = 100;
            else
                BranchCheckerForm.Instance.ProgressBar.Value = (int) Math.Round(((double)current / max) * 100);
        }

        public static void ClearProgress()
        {
            SetProgress("", 0, 1);
        }
    }
}
