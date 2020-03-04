using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace GitBranchChecker
{
    public class ConfigInfo
    {
        public string winMergePath = "WinMerge\\WinMergeU.exe"; // defaults to WinMerge in relative Folder
        public List<string> branchNameFilter = new List<string>(); // defaults to no filter (empty list)
    }

    public static class ConfigReader
    {
        public const string configFilePath = "config.xml";
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
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t";
            XmlWriter xmlWriter = XmlWriter.Create(configFilePath, settings);
            xmlWriter.WriteStartElement("Config");

            xmlWriter.WriteStartElement("WinMergePath");
            xmlWriter.WriteString(new ConfigInfo().winMergePath);
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

                configInfo.winMergePath = xmlDocument.SelectSingleNode("//WinMergePath").InnerText;

                foreach (XmlNode node in xmlDocument.SelectNodes("//Branch"))
                {
                    var filter = node.InnerText;
                    configInfo.branchNameFilter.Add(filter);
                }

                return configInfo;
            }
            catch
            {
            }

            return new ConfigInfo();
        }
    }
}
