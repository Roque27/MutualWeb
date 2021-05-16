using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MAASoft.HomeBankingWeb.Sitio.Helpers
{
    public class FilesHelper
    {
        public static List<string> ObtenerTramitesSubidos(string UserName)
        {
            string path = System.Web.HttpContext.Current.Server.MapPath("~/DocumentosSubidos/" + UserName);
            bool exists = System.IO.Directory.Exists(path);
            if (!exists)
                System.IO.Directory.CreateDirectory(path);

            var dir = new System.IO.DirectoryInfo(path);
            System.IO.FileInfo[] fileNames = dir.GetFiles("*.*");

            List<string> items = new List<string>();
            foreach (var file in fileNames)
            {
                items.Add(file.Name);
            }

            return items;
        }

        public static void BorrarTramiteSubido(string UserName, string FileName)
        {
            string root = System.Web.HttpContext.Current.Server.MapPath("/");
            string downloadFolder = "DocumentosSubidos\\" + UserName + "\\";
            string authorsFile = FileName;
            string route = Path.Combine(root, downloadFolder, authorsFile);
            if (System.IO.File.Exists(route))
            {
                System.IO.File.Delete(Path.Combine(route));
            }
        }
    }
}