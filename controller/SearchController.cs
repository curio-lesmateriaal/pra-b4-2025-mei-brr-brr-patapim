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
    public class SearchController
    {
        // De window die we laten zien op het scherm
        public static Home Window { get; set; }
        

        // Start methode die wordt aangeroepen wanneer de zoek pagina opent.
        public void Start()
        {

        }

        // Wordt uitgevoerd wanneer er op de Zoeken knop is geklikt
        public void SearchButtonClick()
        {
            // Stap 1: Verkrijg de zoekinput (bijv. dag en tijd of alleen dag)
            string input = SearchManager.GetSearchInput();

            // Stap 2: Loop door de beschikbare foto's zoals bij A1
            foreach (string path in Directory.GetFiles(@"../../../fotos/0_Zondag")) // vervang "foto_folder" met jouw daadwerkelijke map
            {
                // Stap 3: Filter op datum/tijd via bestandsnaam (zoals bij A1 en B3)
                if (path.Contains(input)) // of gebruik DateTime.Parse voor nauwkeuriger zoeken
                {
                    // Stap 4: Toon de foto zodra er een match is
                    SearchManager.SetPicture(path);
                }
            }
        }


    }
}
