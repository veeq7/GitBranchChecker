using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Reflection;
using GitBranchChecker.Utils;
using System.Windows.Forms;

namespace GitBranchChecker
{
    public class ConfigInfo
    {
        // Loaded from Config File
        public string winMergePath = "WinMerge\\WinMergeU.exe"; // defaults to WinMerge in relative Folder
        public string textEditor = "notepad"; // defaults to notepad
        public List<string> branchNameFilter = new List<string>(); // defaults to no filter (empty list)

        // Loaded from UI
        public DateTime? dateFilterStart = null;
        public DateTime? dateFilterEnd = null;
    }

    public static class ConfigReader
    {
        public static string configFilePath = PathUtils.MakePath("config.xml");
        public static ConfigInfo LoadConfig()
        {
            if (!File.Exists(configFilePath))
            {
                SaveDefaultConfig();
                return new ConfigInfo();
            }
            return ReadConfig();
        }

        private static void SaveDefaultConfig()
        {
            ConfigInfo defaultConfigInfo = new ConfigInfo();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t";
            XmlWriter xmlWriter = XmlWriter.Create(configFilePath, settings);
            xmlWriter.WriteStartElement("Config");

            xmlWriter.WriteStartElement("WinMergePath");
            xmlWriter.WriteString(defaultConfigInfo.winMergePath);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("TextEditorPath");
            xmlWriter.WriteString(defaultConfigInfo.textEditor);
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("BranchNameFilter");
            xmlWriter.WriteStartElement("Branch");
            xmlWriter.WriteString("");
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndElement();
            xmlWriter.Flush();
        }

        private static ConfigInfo ReadConfig()
        {
            ConfigInfo configInfo = new ConfigInfo();
            XmlDocument xmlDocument = new XmlDocument();

            try
            {
                xmlDocument.Load(configFilePath);

                configInfo.winMergePath = PathUtils.MakePath(xmlDocument.SelectSingleNode("//WinMergePath").InnerText);
                configInfo.textEditor = PathUtils.MakePath(xmlDocument.SelectSingleNode("//TextEditorPath").InnerText);

                foreach (XmlNode node in xmlDocument.SelectNodes("//BranchNameFilter/Branch"))
                {
                    var filter = node.InnerText;
                    configInfo.branchNameFilter.Add(filter);
                }

                return configInfo;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return new ConfigInfo();
        }
    }
}
