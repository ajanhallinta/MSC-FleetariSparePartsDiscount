using MSCLoader;
using UnityEngine;

namespace FleetariPartDiscount
{
    public class FleetariPartDiscount : Mod
    {
        public override string ID => "FleetariPartDiscount"; //Your mod ID (unique)
        public override string Name => "Fleetari Spare Parts Discount"; //You mod name
        public override string Author => "ajanhallinta"; //Your Username
        public override string Version => "1.1"; //Version
        public override string Description => "Apply Fleetari's discount to spare parts."; //Short description of your mod

        public override void ModSetup()
        {
            SetupFunction(Setup.OnLoad, Mod_OnLoad);
        }

        public override void ModSettings()
        {
            // All settings should be created here. 
            // DO NOT put anything else here that settings or keybinds
            Settings.AddHeader(this, "Support");
            Settings.AddText(this, "Please... I'm begging you...");
            Settings.AddButton(this, "support", "PayPal", () =>
            {
                try
                {
                    Application.OpenURL("https://paypal.me/ajanhallinta");
                }
                catch
                {
                }
            });
        }

        private void Mod_OnLoad()
        {
            // Called once, when mod is loading after game is fully loaded
            GameObject go = new GameObject();
            go.name = "Spare Parts Discount";

            try
            {
                go.transform.parent = GameObject.Find("REPAIRSHOP").transform.GetChild(14).GetChild(9).GetChild(3); // REPAIRSHOP/LOD/Store/Parts
            }
            catch
            {
                PartsDiscount.DebugPrint("Error: Can't find REPAIRSHOP/LOD/Store/Parts!");
                return;
            }

            go.AddComponent<PartsDiscount>();
        }
    }
}