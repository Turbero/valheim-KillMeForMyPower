using System.Collections.Generic;
using HarmonyLib;

namespace KillMeForMyPower
{
    public class TraderSetup
    {
        [HarmonyPatch(typeof(Trader), "Interact")]
        public static class TraderInteractPatch
        {
            [HarmonyPrefix]
            public static bool InteractPrefix(Trader __instance, Humanoid character, bool hold, bool alt)
            {
                Logger.Log("**Trader.Interact called for "+__instance.gameObject.name);
                if (!ConfigurationFile.vendorLocalRestrictions.Value) return true;
                
                if (!grantedVendor(__instance))
                {
                    character.Message(MessageHud.MessageType.Center, ConfigurationFile.forbiddenVendorMessage.Value);
                    return false;
                }

                return true;
            }

            private static bool grantedVendor(Trader trader)
            {
                bool haldorOk = trader.gameObject.name.StartsWith("Haldor") && KillMeForMyPowerUtils.isEikthyrDefeatedForPlayer();
                bool hildirOk = trader.gameObject.name.StartsWith("Hildir") && KillMeForMyPowerUtils.isEikthyrDefeatedForPlayer();
                bool bogWitchOk = trader.gameObject.name.StartsWith("BogWitch") && KillMeForMyPowerUtils.isElderDefeatedForPlayer();
                
                return haldorOk || hildirOk || bogWitchOk;
            }
        }

        
        [HarmonyPatch(typeof(Trader), "GetAvailableItems")]
        public static class TraderGetAvailableItemsPatch
        {
            [HarmonyPostfix]
            public static void GetAvailableItemsPostfix(Trader __instance, ref List<Trader.TradeItem> __result)
            {
                Logger.Log("**Trader.GetAvailableItems called for "+__instance.gameObject.name);
                if (!ConfigurationFile.vendorLocalRestrictions.Value) return;

                List<Trader.TradeItem> itemsToRemove = new List<Trader.TradeItem>();
                foreach (Trader.TradeItem tradeItem in __result)
                {
                    if ((tradeItem.m_prefab.gameObject.name == "Thunderstone" && !KillMeForMyPowerUtils.isElderDefeatedForPlayer()) ||
                        (tradeItem.m_prefab.gameObject.name == "YmirRemains" && !KillMeForMyPowerUtils.isElderDefeatedForPlayer()) ||
                        (tradeItem.m_prefab.gameObject.name == "ChickenEgg" && !KillMeForMyPowerUtils.isYagluthDefeatedForPlayer())
                       )
                        itemsToRemove.Add(tradeItem);
                }
                foreach (Trader.TradeItem tradeItem in itemsToRemove)
                {
                    __result.Remove(tradeItem);
                }
            }
        }
    }
}