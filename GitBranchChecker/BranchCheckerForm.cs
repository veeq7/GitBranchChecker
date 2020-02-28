using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;
using System.Security.Permissions;
using GitBranchChecker.DataModels;
using System.Reflection;

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
        public BranchCheckerForm(string[] args)
        {
            InitializeComponent();
            if (args.Length >= 1)
            {
                LoadFile(args[0]);
            }
        }
        #endregion

        #region Buttons

        private void btnSelectFile_click(object sender, EventArgs e)
        {
            SelectFile();
        }

        private void btnCompare_Click(object sender, EventArgs e)
        {
            List<CommitDataModel> selectedCommits = new List<CommitDataModel>();
            foreach (DataGridViewCell cell in dataGridView1.SelectedCells)
            {
                if (selectedCommits.Count >= 2) break;
                int col = cell.ColumnIndex;
                int row = cell.RowIndex;
                if (!branchChecker.repo.branchesByColumn.ContainsKey(col)) return;
                if (!branchChecker.repo.branchesByColumn.ContainsKey(row)) return;
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
                LoadFile(filePath);
                ParseFile();
            } else
            {
                MessageBox.Show(this, "Could not open the file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void LoadFile(string filePath)
        {
            branchChecker.SetFilePath(filePath);
            textBox1.Text = Path.GetFileName(filePath);
            ParseFile();
        }

        void ParseFile()
        {
            dataGridView1.DataSource = branchChecker.Parse();
        }

        #endregion

        #region ContextMenu

        private const string MenuName = "*\\shell\\OpenWithBranchChecker";
        private const string Command = MenuName + "\\command";

        private void RegisterApplicationInRegistry()
        {
            // TODO: Test if it works, can't test because I am not an administrator
            RegistryKey regmenu = null;
            RegistryKey regcmd = null;
            try
            {
                string filePath = Assembly.GetEntryAssembly().Location;
                regmenu = Registry.ClassesRoot.CreateSubKey(MenuName);
                if (regmenu != null)
                    regmenu.SetValue("", "Open with Branch Checker");
                    regmenu.SetValue("icon", filePath);
                regcmd = Registry.ClassesRoot.CreateSubKey(Command);
                if (regcmd != null)
                    regcmd.SetValue("",  filePath + " %1");
                MessageBox.Show(this, "File assossiation added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "File assossiation failed, try running\nprogram with administrator rights!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
