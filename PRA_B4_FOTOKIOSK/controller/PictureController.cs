using PRA_B4_FOTOKIOSK.magie;
using PRA_B4_FOTOKIOSK.models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRA_B4_FOTOKIOSK.controller
{
    public class PictureController
    {
        // De window die we laten zien op het scherm
        public static Home Window { get; set; }
        // De lijst met fotos die we laten zien
        public List<KioskPhoto> PicturesToDisplay = new List<KioskPhoto>();

        // Start methode die wordt aangeroepen wanneer de foto pagina opent.
        public void Start()
        {
            // Initializeer de lijst met fotos
            foreach (string dir in Directory.GetDirectories(@"../../../fotos"))
            {
                foreach (string file in Directory.GetFiles(dir))
                {
                    // Controleer of de foto binnen de tijdsperiode valt (2-30 minuten geleden)
                    if (IsPhotoInTimeRange(file))
                    {
                        PicturesToDisplay.Add(new KioskPhoto() { Id = 0, Source = file });
                    }
                }
            }
            // Update de fotos
            PictureManager.UpdatePictures(PicturesToDisplay);
        }

        // Wordt uitgevoerd wanneer er op de Refresh knop is geklikt
        public void RefreshButtonClick()
        {
            // Clear de huidige lijst
            PicturesToDisplay.Clear();

            // Laad opnieuw met tijdfilter
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

        // Methode om te controleren of foto binnen 2-30 minuten geleden is genomen
        private bool IsPhotoInTimeRange(string filePath)
        {
            try
            {
                // Haal dag uit mapnaam
                string directoryName = Path.GetFileName(Path.GetDirectoryName(filePath));
                if (!directoryName.Contains("_"))
                    return false;

                string dayName = directoryName.Split('_')[1];

                // Haal tijd uit bestandsnaam
                string fileName = Path.GetFileNameWithoutExtension(filePath);
                string[] parts = fileName.Split('_');

                if (parts.Length < 3)
                    return false;

                // Parse tijd uit bestandsnaam (10_05_30 = 10:05:30)
                if (!int.TryParse(parts[0], out int hour) ||
                    !int.TryParse(parts[1], out int minute) ||
                    !int.TryParse(parts[2], out int second))
                    return false;

                // Maak DateTime van foto tijd (gebruik vandaag als datum)
                DateTime photoTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hour, minute, second);

                // Als foto tijd in de toekomst ligt, dan is het waarschijnlijk van gisteren
                if (photoTime > DateTime.Now)
                {
                    photoTime = photoTime.AddDays(-1);
                }

                // Bereken verschil in minuten
                TimeSpan timeDifference = DateTime.Now - photoTime;
                double minutesAgo = timeDifference.TotalMinutes;

                // Controleer of foto tussen 2 en 30 minuten geleden is genomen
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