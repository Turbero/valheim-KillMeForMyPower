using System.Collections.Generic;
using HarmonyLib;
using KillMeForMyPower.Restrictions.BossNameManagement;

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
                    string mandatoryBossToKill = getMandatoryBossToKill(__instance);
                    if (mandatoryBossToKill != null)
                    {
                        character.Message(MessageHud.MessageType.Center, ConfigurationFile.forbiddenVendorMessage.Value.Replace("{0}", mandatoryBossToKill));
                        return false;
                    }
                }

                return true;
            }

            private static string getMandatoryBossToKill(Trader trader)
            {
                if (trader.gameObject.name.StartsWith("Haldor") && ConfigurationFile.vendorHaldorBossToKill.Value != BossNameEnum.None)
                    return Localization.instance.Localize(KillMeForMyPowerUtils.getBossNameTranslation(ConfigurationFile.vendorHaldorBossToKill.Value));
                if (trader.gameObject.name.StartsWith("Hildir") && ConfigurationFile.vendorHildirBossToKill.Value != BossNameEnum.None)
                    return Localization.instance.Localize(KillMeForMyPowerUtils.getBossNameTranslation(ConfigurationFile.vendorHildirBossToKill.Value));
                if (trader.gameObject.name.StartsWith("BogWitch") && ConfigurationFile.vendorBogWitchBossToKill.Value != BossNameEnum.None)
                    return Localization.instance.Localize(KillMeForMyPowerUtils.getBossNameTranslation(ConfigurationFile.vendorBogWitchBossToKill.Value));
                return null;
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
                    addCustomItemsToRemove(__result, customItemsToRemove, ConfigurationFile.vendorBogWitchRestrictions.Value);

                foreach (Trader.TradeItem tradeItem in customItemsToRemove)
                    __result.Remove(tradeItem);
            }

            private static void addCustomItemsToRemove(
                List<Trader.TradeItem> __result,
                List<Trader.TradeItem> customItemsToRemove,
                string vendorRestrictions)
            {
                if (string.IsNullOrEmpty(vendorRestrictions)) return;
                
                string[] items = vendorRestrictions.Split(';');
                foreach (string item in items)
                {
                    string[] itemParts = item.Split(',');
                    Logger.Log($"Checking vendor item {itemParts[0]} against {itemParts[1]}");
                    foreach (Trader.TradeItem tradeItem in __result)
                    {
                        Logger.Log($"- Checking {tradeItem.m_prefab.gameObject.name} from the vendor list...");
                        if (tradeItem.m_prefab.gameObject.name == itemParts[0])
                        {
                            if (!KillMeForMyPowerUtils.bossIsKilled(itemParts[1]))
                            {
                                customItemsToRemove.Add(tradeItem);
                                Logger.Log($"{itemParts[1]} will be excluded.");
                            }
                            else
                                Logger.Log($"{itemParts[1]} will NOT be excluded.");

                            break;
                        }
                    }
                }
            }
        }

        [HarmonyPatch(typeof(Vegvisir), "Interact")]
        public class HildirMapTablePatch
        {
            public static bool Prefix(Vegvisir __instance, Humanoid character, bool hold, bool alt, ref bool __result)
            {
                if (character is Player &&
                    __instance.m_locations.FindAll(loc => 
                        loc.m_pinType == Minimap.PinType.Hildir1 ||
                        loc.m_pinType == Minimap.PinType.Hildir2 ||
                        loc.m_pinType == Minimap.PinType.Hildir3).Count > 0 &&
                    !KillMeForMyPowerUtils.HasDefeatedBossName(ConfigurationFile.vendorHildirBossToKill.Value))
                {
                    character.Message(MessageHud.MessageType.Center, ConfigurationFile.restrictUsingKeyItemsMessage.Value.Replace("{0}", ConfigurationFile.vendorHildirBossToKill.Value.GetTranslationKey()));
                    Effects.scareEffect();
                    __result = false;
                    return false;
                }

                return true;
            }
        }
    }
}