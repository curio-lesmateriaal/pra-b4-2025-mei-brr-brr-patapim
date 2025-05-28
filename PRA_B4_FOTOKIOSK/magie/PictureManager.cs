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
using System.Windows.Shapes; 

namespace PRA_B4_FOTOKIOSK.magie
{
    public class PictureManager
    {

        public static Home Instance { get; set; }


        public static void UpdatePictures(List<KioskPhoto> PicturesToDisplay)
        {
            Instance.spPictures.Children.Clear();
            foreach (KioskPhoto picture in PicturesToDisplay)
            {
                Grid photoContainer = new Grid();

                // Foto
                Image image = new Image();
                var bitmap = pathToImage(picture.Source);
                image.Source = bitmap;
                image.Width = 1920 / 3.5;
                image.Height = 1080 / 3.5;
                photoContainer.Children.Add(image);

                // Haal de dag en tijd uit het pad en bestandsnaam
                string dayText = GetDayFromPath(picture.Source);
                string timeText = GetTimeFromFileName(picture.Source);
                string idText = GetIdFromFileName(picture.Source);

                // Zwarte rechthoek om oude tekst te bedekken
                Rectangle coverRect = new Rectangle();
                coverRect.Fill = System.Windows.Media.Brushes.Black;
                coverRect.Width = 200;
                coverRect.Height = 60;
                coverRect.VerticalAlignment = VerticalAlignment.Bottom;
                coverRect.HorizontalAlignment = HorizontalAlignment.Left;
                coverRect.Margin = new Thickness(5, 0, 0, 5);
                photoContainer.Children.Add(coverRect);

                // Nieuwe gele tekst met juiste dag, tijd en ID
                TextBlock overlay = new TextBlock();
                overlay.Text = $"{dayText} {timeText}\n{idText}";
                overlay.FontSize = 24;
                overlay.Foreground = System.Windows.Media.Brushes.Yellow;
                overlay.VerticalAlignment = VerticalAlignment.Bottom;
                overlay.HorizontalAlignment = HorizontalAlignment.Left;
                overlay.Margin = new Thickness(10);
                photoContainer.Children.Add(overlay);

                Instance.spPictures.Children.Add(photoContainer);
            }
        }

        // Methode om de dag uit het pad te halen
        private static string GetDayFromPath(string filePath)
        {
            string directoryName = System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(filePath));

            if (directoryName.Contains("_"))
            {
                return directoryName.Split('_')[1]; // Haalt "Zondag" uit "0_Zondag"
            }

            return "Onbekend";
        }

        // Nieuwe methode om tijd uit bestandsnaam te halen
        private static string GetTimeFromFileName(string filePath)
        {
            string fileName = System.IO.Path.GetFileNameWithoutExtension(filePath);
            // Bestandsnaam format: 10_05_30_id8824
            string[] parts = fileName.Split('_');

            if (parts.Length >= 3)
            {
                return $"{parts[0]}:{parts[1]}:{parts[2]}"; // "10:05:30"
            }

            return "00:00:00";
        }

        // Nieuwe methode om ID uit bestandsnaam te halen
        private static string GetIdFromFileName(string filePath)
        {
            string fileName = System.IO.Path.GetFileNameWithoutExtension(filePath);
            // Bestandsnaam format: 10_05_30_id8824
            string[] parts = fileName.Split('_');

            if (parts.Length >= 4 && parts[3].StartsWith("id"))
            {
                return parts[3].Substring(2); // Haalt "8824" uit "id8824"
            }

            return "0000";
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

    }
}
