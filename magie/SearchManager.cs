using PRA_B4_FOTOKIOSK.models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace PRA_B4_FOTOKIOSK.magie
{
    public class SearchManager
    {

        public static Home Instance { get; set; }
        private readonly string rootFolder;
        public string RootFolder => rootFolder;

        public SearchManager(string folder)
        {
            rootFolder = System.IO.Path.GetFullPath(folder);

        }

        public static void SetPicture(string path)
        {
            Instance.imgBig.Source = pathToImage(path);
        }

        public static BitmapImage pathToImage(string path)
        {
            var stream = new MemoryStream(File.ReadAllBytes(path));
            var img = new System.Windows.Media.Imaging.BitmapImage();

            img.BeginInit();
            img.StreamSource = stream;
            img.EndInit();

            return img;
        }


        public static string GetSearchInput()
        {
            return Instance.tbZoeken.Text;
        }

        public static void SetSearchImageInfo(string text)
        {
            Instance.lbSearchInfo.Content = text;
        }

        public static string GetSearchImageInfo()
        {
            return (string)Instance.lbSearchInfo.Content;
        }

        public static void AddSearchImageInfo(string text)
        {
            SetSearchImageInfo(GetSearchImageInfo() + text);
        }

        public string ZoekFoto(string dagMap, string query)
        {
            string folderToSearch = Path.Combine(rootFolder, dagMap);
            if (!Directory.Exists(folderToSearch))
                return null;

            foreach (var file in Directory.GetFiles(folderToSearch, "*.jpg"))
            {
                string naam = Path.GetFileNameWithoutExtension(file);
                if (naam.Contains(query))
                    return file;
            }

            return null;
        }
    }
}
