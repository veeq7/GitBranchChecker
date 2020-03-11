using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GitBranchChecker.Utils
{
    public static class PathUtils
    {
        public static string MakePath(string url)
        {
            try
            {
                if (Path.IsPathRooted(url))
                {
                    return url;
                }
            } catch
            {

            }
            
            var newUrl = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\" + url;
            if (File.Exists(newUrl) || Directory.Exists(newUrl))
            {
                return newUrl;
            }
            return url;
        }
    }
}
