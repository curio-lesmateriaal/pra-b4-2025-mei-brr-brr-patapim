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
        // Reference to the main Home UI instance (used to access the UI elements)
        public static Home Instance { get; set; }

        /// Updates the picture display panel with a list of KioskPhoto items.
        /// Clears the current images and repopulates with updated image elements,
        /// including a label with the day, time, and ID extracted from the filename.
     
        public static void UpdatePictures(List<KioskPhoto> PicturesToDisplay)
        {
            // Clear previous images
            Instance.spPictures.Children.Clear();

            foreach (KioskPhoto picture in PicturesToDisplay)
            {
                // Container for each photo
                Grid photoContainer = new Grid();

                // Load and display the photo
                Image image = new Image();
                var bitmap = pathToImage(picture.Source);
                image.Source = bitmap;
                image.Width = 1920 / 3.5;
                image.Height = 1080 / 3.5;
                photoContainer.Children.Add(image);

                // Extract metadata from the file path
                string dayText = GetDayFromPath(picture.Source);
                string timeText = GetTimeFromFileName(picture.Source);
                string idText = GetIdFromFileName(picture.Source);

                // Add a black rectangle overlay to cover any old text on the image
                Rectangle coverRect = new Rectangle();
                coverRect.Fill = System.Windows.Media.Brushes.Black;
                coverRect.Width = 200;
                coverRect.Height = 60;
                coverRect.VerticalAlignment = VerticalAlignment.Bottom;
                coverRect.HorizontalAlignment = HorizontalAlignment.Left;
                coverRect.Margin = new Thickness(5, 0, 0, 5);
                photoContainer.Children.Add(coverRect);

                // Add new yellow text displaying day, time, and ID
                TextBlock overlay = new TextBlock();
                overlay.Text = $"{dayText} {timeText}\n{idText}";
                overlay.FontSize = 24;
                overlay.Foreground = System.Windows.Media.Brushes.Yellow;
                overlay.VerticalAlignment = VerticalAlignment.Bottom;
                overlay.HorizontalAlignment = HorizontalAlignment.Left;
                overlay.Margin = new Thickness(10);
                photoContainer.Children.Add(overlay);

                // Add the constructed photo container to the UI
                Instance.spPictures.Children.Add(photoContainer);
            }
        }

        /// <summary>
        /// Extracts the day name from the directory part of the file path.
        /// Expected format: ".../Dag_<Day>/..."
        /// </summary>
        public static string GetDayFromPath(string filePath)
        {
            string directoryName = System.IO.Path.GetFileName(System.IO.Path.GetDirectoryName(filePath));

            if (directoryName.Contains("_"))
            {
                return directoryName.Split('_')[1]; // e.g., from "Dag_Maandag" returns "Maandag"
            }
            // Fallback if format is incorrect
            return "Onbekend"; 
        }

        /// Extracts the time from the file name.
        /// Expected file name format: "HH_MM_SS_idXXXX"
        public static string GetTimeFromFileName(string filePath)
        {
            string fileName = System.IO.Path.GetFileNameWithoutExtension(filePath);
            string[] parts = fileName.Split('_');

            if (parts.Length >= 3)
            {
                return $"{parts[0]}:{parts[1]}:{parts[2]}"; // Return as HH:MM:SS
            }

            return "00:00:00"; // Default fallback time
        }

        /// Extracts the numeric ID from the file name.
        /// Expected file name format: "..._idXXXX"  
        public static string GetIdFromFileName(string filePath)
        {
            string fileName = System.IO.Path.GetFileNameWithoutExtension(filePath);
            string[] parts = fileName.Split('_');

            if (parts.Length >= 4 && parts[3].StartsWith("id"))
            {
                // Extract only the numeric part of "idXXXX"
                return parts[3].Substring(2); 
            }

            return "0000"; 
        }

        /// Converts a file path to a BitmapImage by loading it from memory.
        /// Ensures the stream remains open while the image is initialized.
        public static BitmapImage pathToImage(string path)
        {
            var stream = new MemoryStream(File.ReadAllBytes(path));
            var img = new BitmapImage();

            img.BeginInit();
            img.StreamSource = stream;
            img.EndInit();

            return img;
        }
    }
}
