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
    public class ShopManager
    {

        public static List<KioskProduct> Products = new List<KioskProduct>();    
        public static Home Instance { get; set; }

        public static void SetShopPriceList(string text)
        {
            Instance.lbPrices.Content = text;
        }

        public static void AddShopPriceList(string text)
        {
            Instance.lbPrices.Content = Instance.lbPrices.Content + text;
        }

        public static void SetShopReceipt(string text)
        {
            Instance.lbReceipt.Content = text;
        }

        public static string GetShopReceipt()
        {
            return (string)Instance.lbReceipt.Content;
        }

        public static void AddShopReceipt(string text)
        {
            SetShopReceipt(GetShopReceipt() + text);
        }

        public static void UpdateDropDownProducts()
        {
            Instance.cbProducts.Items.Clear();
            foreach (KioskProduct item in Products)
            {
                Instance.cbProducts.Items.Add(item.Name);
            }
        }

        public static KioskProduct GetSelectedProduct()
        {
            if (Instance.cbProducts.SelectedItem == null) return null;
            string selected = Instance.cbProducts.SelectedItem.ToString();
            foreach (KioskProduct product in Products)
            {
                if (product.Name == selected) return product;
            }
            return null;
        }

        public static int? GetFotoId()
        {
            int? id = null;
            int amount = -1;
            if (int.TryParse(Instance.tbFotoId.Text, out amount))
            {
                id = amount;
            }
            return id;
        }

        public static int? GetAmount()
        {
            int? id = null;
            int amount = -1;
            if (int.TryParse(Instance.tbAmount.Text, out amount))
            {
                id = amount;
            }
            return id;
        }
        /// <summary>
        /// Slaat de huidige bon op als tekstbestand op een door de gebruiker gekozen locatie
        /// </summary>
        /// <returns>True als het opslaan is gelukt, anders False</returns>
        public static bool SaveReceiptToFile()
        {
            try
            {
                // Haal de huidige bon op
                string receipt = GetShopReceipt();

                if (string.IsNullOrEmpty(receipt))
                {
                    MessageBox.Show("Er is geen bon om op te slaan.", "Waarschuwing", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }

                // Maak een SaveFileDialog aan om de gebruiker een locatie te laten kiezen
                Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "Tekstbestanden (*.txt)|*.txt",
                    Title = "Bon opslaan",
                    FileName = "Bon_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") // Standaardnaam met datum en tijd
                };

                // Toon de SaveFileDialog
                bool? result = saveFileDialog.ShowDialog();

                // Als de gebruiker op Opslaan heeft geklikt
                if (result == true)
                {
                    // Schrijf de bon naar het gekozen bestand
                    File.WriteAllText(saveFileDialog.FileName, receipt);

                    MessageBox.Show("Bon is succesvol opgeslagen naar: " + saveFileDialog.FileName,
                        "Opgeslagen", MessageBoxButton.OK, MessageBoxImage.Information);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fout bij het opslaan van de bon: " + ex.Message,
                    "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

    }
}
