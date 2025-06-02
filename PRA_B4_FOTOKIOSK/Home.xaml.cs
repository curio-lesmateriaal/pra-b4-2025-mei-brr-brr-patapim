using PRA_B4_FOTOKIOSK.controller;
using PRA_B4_FOTOKIOSK.magie;
using PRA_B4_FOTOKIOSK.models;
using System;
using System.Collections.Generic;
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
            ShopManager.SaveReceiptToFile();
        }

        private void btnShopTotal_Click(object sender, RoutedEventArgs e)
        {
            ShopController.Total();
        }

        private void cbProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

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

    }
}
