using PRA_B4_FOTOKIOSK.magie;
using PRA_B4_FOTOKIOSK.models;
using PRA_B4_FOTOKIOSK.controller;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Documents;
using System.Diagnostics;

namespace PRA_B4_FOTOKIOSK.controller
{
    public class ShopController
    {


        public static Home Window { get; set; }

        public static List<(string productName, double productPrice)> productList = new List<(string, double)>
        {
            ("Foto 10x15", 2.55),
            ("Foto 20x15", 4.55),
            ("Foto 30x15", 6.55),
            ("Foto 40x15", 8.55),
            ("Foto 50x20", 10.55),
        };

        public void Start()
        {
            // Stel de prijslijst in aan de rechter kant.
            ShopManager.SetShopPriceList("Prijzen:\n");

            // Stel de bon in onderaan het scherm
            ShopManager.SetShopReceipt("Eindbedrag\n€");

            // Vul de productlijst met producten
            foreach(var product in productList)
            {
                ShopManager.AddShopPriceList($"\n{product.productName}: {product.productPrice}");
                ShopManager.Products.Add(new KioskProduct()
                {
                    Name = product.productName,
                    Price = product.productPrice
                });
            }
            
            // Update dropdown met producten
            ShopManager.UpdateDropDownProducts();
        }

        // Wordt uitgevoerd wanneer er op de Toevoegen knop is geklikt
        public void AddButtonClick()
        {
            // Get the selected product using the new method
            var selectedProduct = ShopManager.GetSelectedProductFromList();
            var fotoID = ShopManager.GetFotoId().ToString();
            var amount = ShopManager.GetAmount();

            // Check if amount has a value
            if (!amount.HasValue || amount.Value <= 0)
            {
                Trace.WriteLine("Invalid amount");
                return;
            }

            // Debug trace with correct product info
            Trace.WriteLine($"Product: {selectedProduct.productName} (€{selectedProduct.productPrice}) FotoID: {fotoID} Amount: {amount.Value}");

            // Add to receipt using the new methods
            if (!string.IsNullOrEmpty(selectedProduct.productName))
            {
                ShopManager.AddSelectedProductToReceipt(amount.Value); // Use .Value to get the int from int?
            }
            else
            {
                Trace.WriteLine("No product selected");
            }
        }

        // Wordt uitgevoerd wanneer er op de Resetten knop is geklikt
        public void ResetButtonClick()
        {
            ShopManager.ClearReceipt();
            ShopManager.InitializeReceipt(); // Start fresh
        }

        private static double CalculateTotalFromReceipt()
        {
            string receipt = ShopManager.GetShopReceipt();
            double total = 0.0;

            // Parse through receipt lines to calculate total
            string[] lines = receipt.Split('\n');
            foreach (string line in lines)
            {
                if (line.Contains(" - €"))
                {
                    string priceString = line.Substring(line.LastIndexOf("€") + 1);
                    if (double.TryParse(priceString, out double price))
                    {
                        total += price;
                    }
                }
            }
            return total;
        }

        public void Total()
        {
            // Calculate total from all items in receipt
            double total = CalculateTotalFromReceipt(); // You'll need to implement this
            ShopManager.AddReceiptTotal(total);
        }

        // Wordt uitgevoerd wanneer er op de Save knop is geklikt
        public void SaveButtonClick()
        {
            ShopManager.AddShopReceipt("test");
        }

    }
}