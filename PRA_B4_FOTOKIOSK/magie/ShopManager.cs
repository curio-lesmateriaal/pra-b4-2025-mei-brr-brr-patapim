using PRA_B4_FOTOKIOSK.models;
using PRA_B4_FOTOKIOSK.controller;
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

        // Method to get selected product from your productList
        public static (string productName, double productPrice) GetSelectedProductFromList()
        {
            if (Instance.cbProducts.SelectedItem == null) return ("", 0.0);
            string selected = Instance.cbProducts.SelectedItem.ToString();

            foreach (var product in ShopController.productList)
            {
                if (product.productName == selected) return product;
            }
            return ("", 0.0);
        }

        // Method to add product line to receipt using your original AddShopReceipt
        public static void AddProductToReceipt(string productName, double productPrice, int quantity)
        {
            double totalPrice = productPrice * quantity;
            string productLine = $"{productName} x{quantity} - €{totalPrice:F2}\n";
            AddShopReceipt(productLine);
        }

        // Method to add selected product to receipt using your original methods
        public static void AddSelectedProductToReceipt(int quantity)
        {
            var selectedProduct = GetSelectedProductFromList();
            if (!string.IsNullOrEmpty(selectedProduct.productName))
            {
                double totalPrice = selectedProduct.productPrice * quantity;
                string productLine = $"{selectedProduct.productName} x{quantity} - €{totalPrice:F2}\n";
                AddShopReceipt(productLine);
            }
        }

        // Method to clear receipt using your original SetShopReceipt
        public static void ClearReceipt()
        {
            SetShopReceipt("");
        }

        // Method to add receipt header using your original AddShopReceipt
        public static void InitializeReceipt()
        {
            SetShopReceipt("=== FOTO KIOSK RECEIPT ===\n");
        }

        // Method to add total using your original AddShopReceipt
        public static void AddReceiptTotal(double totalAmount)
        {
            string totalLine = $"\n--- TOTAL: €{totalAmount:F2} ---\n";
            AddShopReceipt(totalLine);
        }

        public static void UpdateDropDownProducts()
        {
            Instance.cbProducts.Items.Clear();
            foreach (KioskProduct item in Products)
            {
                Instance.cbProducts.Items.Add(item.Name);
            }
        }

        public static (string productName, double productPrice) GetSelectedProduct()
        {
            if (Instance.cbProducts.SelectedItem == null) return ("", 0.0);
            string selected = Instance.cbProducts.SelectedItem.ToString();

            foreach (var product in ShopController.productList)
            {
                if (product.productName == selected) return product;
            }
            return ("", 0.0);
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
    }
}
