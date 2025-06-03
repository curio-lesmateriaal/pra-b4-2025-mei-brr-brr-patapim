using PRA_B4_FOTOKIOSK.controller;
using PRA_B4_FOTOKIOSK.magie;
using PRA_B4_FOTOKIOSK.models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PRA_B4_FOTOKIOSK
{
    public partial class Home : Window
    {
        private SearchManager zoekManager;
        public ShopController ShopController { get; set; }
        public PictureController PictureController { get; set; }
        public SearchController SearchController { get; set; }

        public Home()
        {
            // Bouw de UI
            InitializeComponent();

            ShopManager.Instance = this;
            ShopManager.InitializeReceipt();

            // Stel de manager in
            PictureManager.Instance = this;
            ShopManager.Instance = this;
            ShopController.Window = this;
            PictureController.Window = this;
            SearchController.Window = this;

            // Maak de controllers
            ShopController = new ShopController();
            PictureController = new PictureController();
            SearchController = new SearchController();

            // Start de paginas
            PictureController.Start();
            ShopController.Start();
            SearchController.Start();

            string fotosPath = @"../../../fotos"; // <-- pas dit aan
            zoekManager = new SearchManager(fotosPath);
        }

        private void btnShopAdd_Click(object sender, RoutedEventArgs e)
        {
            ShopController.AddButtonClick();
        }

        private void btnShopReset_Click(object sender, RoutedEventArgs e)
        {
            ShopController.ResetButtonClick();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            PictureController.RefreshButtonClick();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            ShopController.SaveButtonClick();
        }

        private void btnShopTotal_Click(object sender, RoutedEventArgs e)
        {
            ShopController.Total();
        }

        private void cbProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnZoekMijnFotos_Click(object sender, RoutedEventArgs e)
        {
            if (cbMijnFotosDag.SelectedItem is ComboBoxItem selectedDag)
            {
                string dagMap = selectedDag.Tag.ToString(); // bijv. "2_Dinsdag"
                string fotoId = tbMijnFotoId.Text.Trim();

                if (string.IsNullOrWhiteSpace(fotoId))
                {
                    lbMijnFotosStatus.Content = "Voer een Foto ID in.";
                    return;
                }

                string folderToSearch = System.IO.Path.Combine(@"../../../fotos", dagMap);

                if (!Directory.Exists(folderToSearch))
                {
                    lbMijnFotosStatus.Content = $"De map '{folderToSearch}' bestaat niet.";
                    return;
                }

                var allFiles = Directory.GetFiles(folderToSearch, "*.jpg").ToList();

                // Zoek eerst de eerste foto met de juiste ID
                var eersteFoto = allFiles
                    .FirstOrDefault(f => PictureManager.GetIdFromFileName(f).Equals(fotoId, StringComparison.OrdinalIgnoreCase));

                if (eersteFoto == null)
                {
                    lbMijnFotosStatus.Content = $"Geen foto gevonden met ID {fotoId}.";
                    return;
                }

                // Toon eerste foto
                imgMijnFoto1.Source = PictureManager.pathToImage(eersteFoto);
                lbMijnFoto1Info.Content = $"{dagMap.Replace("_", " ")} - {PictureManager.GetTimeFromFileName(eersteFoto)} - ID {fotoId}";

                var tijdEersteFoto = ParseTime(PictureManager.GetTimeFromFileName(eersteFoto));

                // Zoek in ALLE foto's (ongeacht ID) een foto die precies 60 seconden later is gemaakt
                var tweedeFoto = allFiles
                    .Where(f => f != eersteFoto) // Niet dezelfde foto als eerste
                    .FirstOrDefault(f =>
                    {
                        var tijd = ParseTime(PictureManager.GetTimeFromFileName(f));
                        return Math.Abs((tijd - tijdEersteFoto).TotalSeconds) == 60;
                    });

                if (tweedeFoto != null)
                {
                    imgMijnFoto2.Source = PictureManager.pathToImage(tweedeFoto);
                    lbMijnFoto2Info.Content = $"{dagMap.Replace("_", " ")} - {PictureManager.GetTimeFromFileName(tweedeFoto)} - ID {PictureManager.GetIdFromFileName(tweedeFoto)}";
                    lbMijnFotosStatus.Content = "Twee foto's gevonden met exact 60 seconden verschil.";
                }
                else
                {
                    imgMijnFoto2.Source = null;
                    lbMijnFoto2Info.Content = "";
                    lbMijnFotosStatus.Content = "Alleen eerste foto gevonden, geen tweede foto met 60 seconden verschil.";
                }
            }
            else
            {
                lbMijnFotosStatus.Content = "Selecteer een dag.";
            }
        }

        // Helper method voor tijdparsing blijft hetzelfde
        private TimeSpan ParseTime(string tijd)
        {
            return TimeSpan.TryParse(tijd, out var result) ? result : TimeSpan.Zero;
        }



        private void btnZoeken_Click(object sender, RoutedEventArgs e)
        {
            if (cbDag.SelectedItem == null)
            {
                lbSearchInfo.Content = "Selecteer eerst een dag.";
                return;
            }

            string dagMap = ((ComboBoxItem)cbDag.SelectedItem).Tag.ToString();
            string input = tbZoeken.Text.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                lbSearchInfo.Content = "Voer een tijd of ID in.";
                return;
            }

            var zoekManager = new SearchManager(@"../../../fotos");
            string fotoPad = zoekManager.ZoekFoto(dagMap, input);

            if (fotoPad != null)
            {
                ToonFoto(fotoPad);
                lbSearchInfo.Content = $"Foto gevonden:";
            }
            else
            {
                lbSearchInfo.Content = "Geen foto gevonden.";
                imgBig.Source = null;
            }
        }

        private void ToonFoto(string pad)
        {
            string absoluutPad = System.IO.Path.GetFullPath(pad);

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(absoluutPad, UriKind.Absolute);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();

            imgBig.Source = bitmap;
        }

        private void btnResetMijnFotos_Click(object sender, RoutedEventArgs e)
        {
            tbMijnFotoId.Text = "";
            imgMijnFoto1.Source = null;
            imgMijnFoto2.Source = null;
            lbMijnFoto1Info.Content = "";
            lbMijnFoto2Info.Content = "";
            lbMijnFotosStatus.Content = "";
        }
    }
}
