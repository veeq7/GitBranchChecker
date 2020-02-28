using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;
using System.Security.Permissions;
using GitBranchChecker.DataModels;

namespace GitBranchChecker
{
    public partial class BranchCheckerForm : Form
    {
        #region Vars
        BranchChecker branchChecker = new BranchChecker();
        public static string winMergePath = "\"WinMerge\\WinMergeU.exe\"";
        public static string winMergeCommand = winMergePath + " /dr ";

        #endregion

        #region Init
        public BranchCheckerForm()
        {
            InitializeComponent();
        }
        #endregion

        #region Buttons

        private void btnSelectFile_click(object sender, EventArgs e)
        {
            SelectFile();
            dataGridView1.DataSource = branchChecker.Parse();
        }

        private void btnCompare_Click(object sender, EventArgs e)
        {
            List<CommitDataModel> selectedCommits = new List<CommitDataModel>();
            foreach (DataGridViewCell cell in dataGridView1.SelectedCells)
            {
                if (selectedCommits.Count >= 2) break;
                int col = cell.ColumnIndex;
                int row = cell.RowIndex;
                var branch = branchChecker.repo.branchesByColumn[col];
                var commit = branch.commitsByRow[row];
                selectedCommits.Add(commit);
            }
            if (selectedCommits.Count == 2)
                branchChecker.Compare(selectedCommits[0], selectedCommits[1]);
        }

        private void btnRegisterAssosiation_Click(object sender, EventArgs e)
        {
            RegisterApplicationInRegistry();
        }

        #endregion

        #region SelectFile

        void SelectFile()
        {
            string filePath = "";
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Any File (*.*)|*.*";
            dlg.Title = "Select File";

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                filePath = dlg.FileName.ToString();
                branchChecker.SetFilePath(filePath);
                textBox1.Text = Path.GetFileName(filePath);
            }
        }

        #endregion

        #region ContextMenu

        private const string MenuName = "Folder\\shell\\OpenWithBranchChecker";
        private const string Command = MenuName + "\\command";

        [PrincipalPermissionAttribute(SecurityAction.Demand, Role = @"BUILTIN\Administrators")]
        private void RegisterApplicationInRegistry()
        {
            // TODO: Test if it works, can't test because I am not an administrator
            RegistryKey regmenu = null;
            RegistryKey regcmd = null;
            try
            {
                regmenu = Registry.ClassesRoot.CreateSubKey(MenuName);
                if (regmenu != null)
                    regmenu.SetValue("", "Open with Branch Checker");
                regcmd = Registry.ClassesRoot.CreateSubKey(Command);
                if (regcmd != null)
                    regcmd.SetValue("", System.Reflection.Assembly.GetEntryAssembly().Location);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.ToString());
            }
            finally
            {
                if (regmenu != null)
                    regmenu.Close();
                if (regcmd != null)
                    regcmd.Close();
            }
        }

        #endregion
    }
}
