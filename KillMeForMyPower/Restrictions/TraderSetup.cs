using System.Collections.Generic;
using HarmonyLib;

namespace KillMeForMyPower.Restrictions
{
    public class TraderSetup
    {
        [HarmonyPatch(typeof(Trader), "Interact")]
        public class TraderInteractPatch
        {
            [HarmonyPrefix]
            public static bool InteractPrefix(Trader __instance, Humanoid character, bool hold, bool alt)
            {
                Logger.Log("**Trader.Interact called for "+__instance.gameObject.name);
                if (!ConfigurationFile.vendorLocalRestrictions.Value) return true;
                
                if (!grantedVendor(__instance))
                {
                    character.Message(MessageHud.MessageType.Center, ConfigurationFile.forbiddenVendorMessage.Value.Replace("{0}", getMandatoryBossToKill(__instance)));
                    return false;
                }

                return true;
            }

            private static string getMandatoryBossToKill(Trader trader)
            {
                if (trader.gameObject.name.StartsWith("Haldor"))
                    return Localization.instance.Localize(KillMeForMyPowerUtils.getBossNameTranslation(ConfigurationFile.vendorHaldorBossToKill.Value));
                if (trader.gameObject.name.StartsWith("Hildir"))
                    return Localization.instance.Localize(KillMeForMyPowerUtils.getBossNameTranslation(ConfigurationFile.vendorHildirBossToKill.Value));
                if (trader.gameObject.name.StartsWith("BogWitch"))
                    return Localization.instance.Localize(KillMeForMyPowerUtils.getBossNameTranslation(ConfigurationFile.vendorBogWitchBossToKill.Value));
                return "";
            }

            private static bool grantedVendor(Trader trader)
            {
                bool haldorOk = trader.gameObject.name.StartsWith("Haldor") && KillMeForMyPowerUtils.bossIsKilled(ConfigurationFile.vendorHaldorBossToKill.Value);
                bool hildirOk = trader.gameObject.name.StartsWith("Hildir") && KillMeForMyPowerUtils.bossIsKilled(ConfigurationFile.vendorHildirBossToKill.Value);
                bool bogWitchOk = trader.gameObject.name.StartsWith("BogWitch") && KillMeForMyPowerUtils.bossIsKilled(ConfigurationFile.vendorBogWitchBossToKill.Value);
                
                return haldorOk || hildirOk || bogWitchOk;
            }
        }


        [HarmonyPatch(typeof(Trader), "GetAvailableItems")]
        public class TraderGetAvailableItemsPatch
        {
            [HarmonyPostfix]
            public static void GetAvailableItemsPostfix(Trader __instance, ref List<Trader.TradeItem> __result)
            {
                Logger.Log("**Trader.GetAvailableItems called for "+__instance.gameObject.name);
                if (!ConfigurationFile.vendorLocalRestrictions.Value) return;

                //Custom items to remove
                List<Trader.TradeItem> customItemsToRemove = new List<Trader.TradeItem>();
                if (__instance.name.Contains("Haldor"))
                    addCustomItemsToRemove(__result, customItemsToRemove, ConfigurationFile.vendorHaldorRestrictions.Value);
                else if (__instance.name.Contains("Hildir"))
                    addCustomItemsToRemove(__result, customItemsToRemove, ConfigurationFile.vendorHildirRestrictions.Value);
                else if (__instance.name.Contains("BogWitch"))
                    addCustomItemsToRemove(__result, customItemsToRemove,
                        ConfigurationFile.vendorBogWitchRestrictions.Value);

                foreach (Trader.TradeItem tradeItem in customItemsToRemove)
                    __result.Remove(tradeItem);
            }

            private static void addCustomItemsToRemove(
                List<Trader.TradeItem> __result,
                List<Trader.TradeItem> customItemsToRemove,
                string vendorRestrictions)
            {
                string[] items = vendorRestrictions.Split(';');
                foreach (string item in items)
                {
                    string[] itemParts = item.Split(',');
                    foreach (Trader.TradeItem tradeItem in __result)
                    {
                        if (tradeItem.m_prefab.gameObject.name == itemParts[0] && !KillMeForMyPowerUtils.bossIsKilled(itemParts[1]))
                        {
                            customItemsToRemove.Add(tradeItem);
                        }
                    }
                }
            }
        }
    }
}