using PRA_B4_FOTOKIOSK.magie;
using PRA_B4_FOTOKIOSK.models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace PRA_B4_FOTOKIOSK.controller
{
    public class PictureController
    {
        // We show the window on the screen
        public static Home Window { get; set; }
        // De lijst met fotos die we laten zien
        public List<KioskPhoto> PicturesToDisplay = new List<KioskPhoto>();

        // Start method called when the photo page opens.
        public void Start()
        {
            // Initialize the list of photos
            foreach (string dir in Directory.GetDirectories(@"../../../fotos"))
            {
                foreach (string file in Directory.GetFiles(dir))
                {
                    // Check if the photo falls within the time period (2-30 minutes ago)
                    if (IsPhotoInTimeRange(file))
                    {
                        PicturesToDisplay.Add(new KioskPhoto() { Id = 0, Source = file });
                    }
                }
            }
            // Update the fotos
            PictureManager.UpdatePictures(PicturesToDisplay);
        }

        // Executed when the Refresh button is clicked
        public void RefreshButtonClick()
        {
            // Clear the current list
            PicturesToDisplay.Clear();

            // Reload with time filter
            foreach (string dir in Directory.GetDirectories(@"../../../fotos"))
            {
                foreach (string file in Directory.GetFiles(dir))
                {
                    if (IsPhotoInTimeRange(file))
                    {
                        PicturesToDisplay.Add(new KioskPhoto() { Id = 0, Source = file });
                    }
                }
            }

            // Update de fotos
            PictureManager.UpdatePictures(PicturesToDisplay);
        }

        // Method to check if photo was taken within 2-30 minutes ago
        private bool IsPhotoInTimeRange(string filePath)
        {
            try
            {
                // Get day from folder name
                string directoryName = Path.GetFileName(Path.GetDirectoryName(filePath));
                if (!directoryName.Contains("_"))
                    return false;

                string dayName = directoryName.Split('_')[1];

                // Remove time from file name
                
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                string[] parts = fileName.Split('_');

                if (parts.Length < 3)
                    return false;

                // Parse time from file name (10_05_30 = 10:05:30)
                if (!int.TryParse(parts[0], out int hour) ||
                    !int.TryParse(parts[1], out int minute) ||
                    !int.TryParse(parts[2], out int second))
                    return false;

                // Create DateTime from photo time (use today as date)
                DateTime photoTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hour, minute, second);

                // If photo time is in the future, then it's probably from yesterday
                if (photoTime > DateTime.Now)
                {
                    photoTime = photoTime.AddDays(-1);
                }

                // Calculate difference in minutes
                TimeSpan timeDifference = DateTime.Now - photoTime;
                double minutesAgo = timeDifference.TotalMinutes;

                // Check if photo was taken between 2 and 30 minutes ago
                return minutesAgo >= 2 && minutesAgo <= 30;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing time from file {filePath}: {ex.Message}");
                return false;
            }
        }
    }
}