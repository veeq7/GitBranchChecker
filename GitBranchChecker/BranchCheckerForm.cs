using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;
using System.Security.Permissions;
using GitBranchChecker.DataModels;
using System.Reflection;
using System.Drawing;
using GitBranchChecker.Exceptions;
using System.Threading.Tasks;
using System.Threading;

namespace GitBranchChecker
{
    public partial class BranchCheckerForm : Form
    {
        #region Vars
        public static BranchCheckerForm Instance { get; private set; }
        BranchChecker branchChecker = new BranchChecker();
        public static ConfigInfo configInfo = ConfigReader.LoadConfig();
        private bool dateInitialized = false;

        private bool interuptable = false;
        public bool interupt { get; private set; } = false;

        #endregion

        #region Init
        public BranchCheckerForm(string[] args)
        {
            Instance = this;

            InitializeComponent();
            if (args.Length >= 1)
            {
                LoadFile(args[0]);
            }
            InitFilerDates();
        }
        #endregion

        #region Buttons

        private void btnSelectFile_click(object sender, EventArgs e)
        {
            SelectFile();
        }

        private void btnCompare_Click(object sender, EventArgs e)
        {
            Compare();
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

            new Thread(()=> ParseFile()).Start();
        }

        void ParseFile()
        {
            interuptable = true;
            Invoke(new Action(() => { abortLoading.Enabled = interuptable; }));
            interupt = false;

            try
            {
                var dataTable = branchChecker.Parse();
                if (dataTable != null)
                    Invoke(new Action(() => { dataGridView1.DataSource = dataTable; }));
            }
            catch (InteruptionException e)
            {
                
            }

           interuptable = false;
            Invoke(new Action(() => { abortLoading.Enabled = interuptable; }));
            interupt = false;
        }
        
        void UpdateTable()
        {
            dataGridView1.DataSource = branchChecker.Update();
        }

        #endregion

        #region Compare
        private List<CommitDataModel> GetSelectedCommits()
        {
            List<CommitDataModel> selectedCommits = new List<CommitDataModel>();
            foreach (DataGridViewCell cell in dataGridView1.SelectedCells)
            {
                int col = cell.ColumnIndex;
                int row = cell.RowIndex;
                CommitDataModel commitModel = GetCommitFromGrid(col, row);
                if (commitModel == null) continue;
                selectedCommits.Add(commitModel);
            }
            return selectedCommits;
        }

        private int CountValidSelectedCommits()
        {
            return GetSelectedCommits().Count;
        }

        private void LimitSelection()
        {
            var selectedCells = dataGridView1.SelectedCells;
            while (dataGridView1.SelectedCells.Count > 3)
            {
                selectedCells[selectedCells.Count - 1].Selected = false;
            }
        }

        private void OpenFile()
        {
            var selectedCommits = GetSelectedCommits();
            if (selectedCommits.Count > 0)
                branchChecker.OpenFile(selectedCommits);
        }

        private void Compare()
        {
            var selectedCommits = GetSelectedCommits();
            if (selectedCommits.Count > 0 && selectedCommits.Count <= 3)
                branchChecker.Compare(selectedCommits);
        }
        #endregion

        #region WindowsContextMenu

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

        #region GridView

        private CommitDataModel GetCommitFromGrid(int col, int row)
        {
            if (!branchChecker.repo.branchesByColumn.ContainsKey(col)) return null;
            var branch = branchChecker.repo.branchesByColumn[col];
            if (!branch.commitsByRow.ContainsKey(row)) return null;
            return branch.commitsByRow[row];
        }

        private int _current_col;
        private int _current_row;
        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            LimitSelection();

            if (e.Button != MouseButtons.Right) return;
            int validCount = CountValidSelectedCommits();
            if (validCount == 0) return;

            ContextMenu m = new ContextMenu();
            if (validCount == 1) m.MenuItems.Add(new MenuItem("Copy Commit Sha", new EventHandler(CMCopyCommitSha)));
            if (validCount == 1) m.MenuItems.Add(new MenuItem("Copy File Sha", new EventHandler(CMCopyFileSha)));
            if (validCount > 0) m.MenuItems.Add(new MenuItem("Open in Text Editor", new EventHandler(CMOpenInTextEditor)));
            if (validCount > 1 && validCount <= 3) m.MenuItems.Add(new MenuItem("Compare", new EventHandler(CMCompare)));

            
            _current_col = e.ColumnIndex;
            _current_row = e.RowIndex;
            
            m.Show(dataGridView1, dataGridView1.PointToClient(MousePosition));
        }

        void CMCopyCommitSha(object sender, EventArgs e)
        {
            CommitDataModel commitModel = GetCommitFromGrid(_current_col, _current_row);
            if (commitModel == null) return;
            Clipboard.SetText(commitModel.commit.Sha);
            MessageBox.Show("Commit Sha copied to clipboard!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        void CMCopyFileSha(object sender, EventArgs e)
        {
            CommitDataModel commitModel = GetCommitFromGrid(_current_col, _current_row);
            if (commitModel == null) return;
            Clipboard.SetText(commitModel.blob.Sha);
            MessageBox.Show("File Sha copied to clipboard!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        void CMOpenInTextEditor(object sender, EventArgs e)
        {
            OpenFile();
        }

        void CMCompare(object sender, EventArgs e)
        {
            Compare();
        }
        #endregion

        #region DateFilter

        private void InitFilerDates()
        {
            startFilterDate.Value = DateTime.Now;
            endFilterDate.Value = DateTime.Now;
            dateInitialized = true;
        }

        private void doFilterStartDate_CheckedChanged(object sender, EventArgs e)
        {
            if (!dateInitialized) return;
            if (doFilterStartDate.Checked)
            {
                configInfo.dateFilterStart = startFilterDate.Value;
                endFilterDate.MinDate = startFilterDate.Value;
                startFilterDate.Enabled = true;
            }
            else
            {
                endFilterDate.MinDate = DateTimePicker.MinimumDateTime;
                configInfo.dateFilterStart = null;
                startFilterDate.Enabled = false;
            }
            UpdateTable();
        }

        private void doFilterEndDate_CheckedChanged(object sender, EventArgs e)
        {
            if (!dateInitialized) return;
            if (doFilterEndDate.Checked)
            {
                configInfo.dateFilterEnd = endFilterDate.Value;
                endFilterDate.Enabled = true;
            } else
            {
                configInfo.dateFilterEnd = null;
                endFilterDate.Enabled = false;
            }
            UpdateTable();
        }



        private void startFilterDate_ValueChanged(object sender, EventArgs e)
        {
            if (!dateInitialized) return;
            doFilterEndDate.Checked = true;
            configInfo.dateFilterStart = startFilterDate.Value;
            endFilterDate.MinDate = startFilterDate.Value;
            configInfo.dateFilterEnd = endFilterDate.Value;

            UpdateTable();
        }

        private void endFilterDate_ValueChanged(object sender, EventArgs e)
        {
            if (!dateInitialized) return;
            doFilterEndDate.Checked = true;
            configInfo.dateFilterEnd = endFilterDate.Value;

            UpdateTable();
        }

        #endregion

        private void abortLoading_Click(object sender, EventArgs e)
        {
            if (interuptable)
            {
                interupt = true;
            }
        }
    }
}
