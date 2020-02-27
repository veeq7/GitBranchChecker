using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            label1.Text = "";
        }

       
        private void button2_Click(object sender, EventArgs e)
        {
            compareGit("podkowa");
        }

        private void compareGit(string authorEmail)
        {
            using (var repository = new Repository(@"D:\Projects\Vendo\Master\.git"))
            {
                //
                compareVersionAndSaveToFile(repository, "master", "v20190703", authorEmail);
                compareVersionAndSaveToFile(repository, "master", "v20180524", authorEmail);
                compareVersionAndSaveToFile(repository, "master", "v20170926", authorEmail);
                compareVersionAndSaveToFile(repository, "master", "v20160630", authorEmail);
                //
                compareVersionAndSaveToFile(repository, "v20190703", "master", authorEmail);
                compareVersionAndSaveToFile(repository, "v20180524", "master", authorEmail);
                compareVersionAndSaveToFile(repository, "v20180524", "v20190703", authorEmail);
                compareVersionAndSaveToFile(repository, "v20190703", "v20180524", authorEmail);
            }
        }

        private void compareVersionAndSaveToFile(Repository repository, string patternBranch, string comparedBranch, string authorEmail)
        {
            var ok_commits = 0;

            var pattern = getCommitList(repository, patternBranch, authorEmail);
            var compared = getCommitList(repository, comparedBranch, authorEmail);

            foreach (var item in pattern)
            {
                if (tryRemove(compared, item)) ok_commits++;
            }

            saveToFile(compared, ok_commits, $"from_{comparedBranch}_to_{patternBranch}");
        }

        private void saveToFile(List<Commit> list, int ok, string name)
        {
            string path = @"d:\temp\git\" + DateTime.Now.ToString("yyyyMMddHHmm") + "_" + name + ".txt";

            // This text is added only once to the file.
            if (File.Exists(path))
                return;

            string txt = $"{name}: {ok} {list.Count}";
            txt += "\r\n" + getText(list);


            File.WriteAllText(path, txt);
        }

        private string getText(List<Commit> list)
        {
            var txt = new List<string>();
            foreach (var item in list)
            {
                var ln = $"{item.Sha} {item.Committer.When} {item.Author.When}: ({item.MessageShort})";
                txt.Add(ln);
            }

            return string.Join("\r\n", txt.ToArray());
        }

        private bool tryRemove(List<Commit> list, Commit elem)
        {
            var l = new List<string>();

            for (int i = 0; i < list.Count; i++)
            {
                if (elem.Message == list[i].Message)
                {
                    list.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        private List<Commit> getCommitList(Repository rep, string branch, string email="")
        {
            var ret = new List<Commit>();
            var commitLog = rep.Branches["refs/remotes/origin/" + branch].Commits;
            foreach (var commit in commitLog)
            {
                bool addToList = true;
                if (!string.IsNullOrEmpty(email)) addToList = commit.Author.Email.Contains(email) || commit.Committer.Email.Contains(email);
                if (addToList) addToList = commit.Author.When > new DateTimeOffset(new DateTime(2019, 10, 20));
                if (addToList) ret.Add(commit);
            }

            return ret;
        }
    }
}
