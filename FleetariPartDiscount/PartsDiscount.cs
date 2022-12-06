using System.Linq;
using UnityEngine;
using HutongGames.PlayMaker;

namespace FleetariPartDiscount
{
    public class PartsDiscount : MonoBehaviour
    {
        public static void DebugPrint(string msg)
        {
            MSCLoader.ModConsole.Print("Discount: " + msg);
        }

        private void OnEnable()
        {
            DebugPrint("Enabled");
            UpdateDiscount();
        }

        public void UpdateDiscount()
        {
            PlayMakerFSM discountFsm = null;
            float discount = -1;

            try
            {
                discountFsm = Resources.FindObjectsOfTypeAll<PlayMakerFSM>().Where(x => x.name == "PriceDiscount").FirstOrDefault();
            }
            catch
            {
                DebugPrint("no discountFsm");
                return;
            }

            try
            {
                discount = MSCLoader.PlayMakerExtensions.GetVariable<FsmFloat>(discountFsm, "DiscountPercent").Value;
            }
            catch
            {
                DebugPrint("error when trying to get discount percent");
            }

            if (discount == -1)
            {
                DebugPrint("no discount price float value");
                return;
            }

            // update part price in fsm variables
            foreach (PlayMakerFSM partFsm in transform.parent.GetComponentsInChildren<PlayMakerFSM>())
            {
                // create custom fsmfloat for original price
                FsmFloat ogPrice = new FsmFloat();
                ogPrice.Name = "OriginalPrice";
                ogPrice.Value = partFsm.FsmVariables.GetFsmFloat("Price").Value;
                MSCLoader.PlayMakerExtensions.AddVariable(partFsm, ogPrice);

                float ogPriceValue = partFsm.FsmVariables.GetFsmFloat("Price").Value;

                // calculate price from og price
                float newPrice = (int)(ogPriceValue - (ogPriceValue * discount));
                if (newPrice == 0)
                    continue;

                partFsm.FsmVariables.GetFsmFloat("Price").Value = newPrice;

                // get and split price string
                FsmString notification = partFsm.FsmVariables.GetFsmString("Notification");
                string[] priceStrings = notification.Value.Split(' ');
                priceStrings[priceStrings.Length - 2] = newPrice.ToString();

                // create new price string
                string newPriceString = "";
                for (int i = 0; i < priceStrings.Length; i++)
                {
                    newPriceString += priceStrings[i];
                    if (i != priceStrings.Length - 1)
                        newPriceString += " ";
                }

                // apply new price string
                notification.Value = newPriceString;
            }

            DebugPrint("Applied Parts Discount, destroying...");
            Destroy(gameObject);
        }
    }
}