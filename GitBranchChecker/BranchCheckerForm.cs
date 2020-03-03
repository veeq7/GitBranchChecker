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
        public static ConfigInfo configInfo = ConfigReader.LoadConfig();

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
                CommitDataModel commitModel = GetCommitFromGrid(col, row);
                if (commitModel == null) return;
                selectedCommits.Add(commitModel);
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
            } else
            {
                //MessageBox.Show(this, "Could not open the file!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            catch
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

        private CommitDataModel GetCommitFromGrid(int col, int row)
        {
            if (!branchChecker.repo.branchesByColumn.ContainsKey(col)) return null;
            var branch = branchChecker.repo.branchesByColumn[col];
            if (!branch.commitsByRow.ContainsKey(row)) return null;
            return branch.commitsByRow[row];
        }
        
        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Right) return;

            CommitDataModel commitModel = GetCommitFromGrid(e.ColumnIndex, e.RowIndex);
            if (commitModel == null) return;
            Clipboard.SetText(commitModel.blob.Sha);
            MessageBox.Show("Sha copied to clipboard!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
