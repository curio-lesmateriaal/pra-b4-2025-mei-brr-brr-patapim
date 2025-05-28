using PRA_B4_FOTOKIOSK.magie;
using PRA_B4_FOTOKIOSK.models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Documents;

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


            
        }

        // Wordt uitgevoerd wanneer er op de Resetten knop is geklikt
        public void ResetButtonClick()
        {

        }

        // Wordt uitgevoerd wanneer er op de Save knop is geklikt
        public void SaveButtonClick()
        {
        }

    }
}