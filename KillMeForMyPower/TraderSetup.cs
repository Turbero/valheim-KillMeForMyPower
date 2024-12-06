using System.Collections.Generic;
using HarmonyLib;

namespace KillMeForMyPower
{
    public class TraderSetup
    {
        [HarmonyPatch(typeof(Trader), "GetAvailableItems")]
        public static class TraderGetAvailableItemsPatch
        {
            [HarmonyPostfix]
            public static void GetAvailableItemsPostfix(Trader __instance, ref List<Trader.TradeItem> __result)
            {
                Logger.Log("**Trader.GetAvailableItems called");
                Logger.Log(__instance.m_name);

                if (grantedVendor(__instance))
                {
                    List<Trader.TradeItem> itemsToRemove = new List<Trader.TradeItem>();
                    foreach (Trader.TradeItem tradeItem in __result)
                    {
                        if ((tradeItem.m_prefab.gameObject.name == "Thunderstone" && KillMeForMyPowerUtils.isElderDefeatedForPlayer()) ||
                            (tradeItem.m_prefab.gameObject.name == "YmirRemains" && KillMeForMyPowerUtils.isElderDefeatedForPlayer()) ||
                            (tradeItem.m_prefab.gameObject.name == "ChickenEgg" && KillMeForMyPowerUtils.isYagluthDefeatedForPlayer())
                           )
                            itemsToRemove.Add(tradeItem);
                    }

                    foreach (Trader.TradeItem tradeItem in itemsToRemove)
                    {
                        __result.Remove(tradeItem);
                    }
                }
                else
                {
                    //Remove all if vendor not granted
                    __result.Clear();
                }

                if (__result.Count == 0)
                {
                    //dummy to avoid exceptions
                    __result.Add(new Trader.TradeItem()
                    {
                        m_prefab = KillMeForMyPowerUtils.findItemDropByName("Coins"),
                        m_stack = 1,
                        m_price = 1,
                        m_requiredGlobalKey = null
                    });
                }
            }

            private static bool grantedVendor(Trader trader)
            {
                bool haldorOk = trader.gameObject.name.StartsWith("Haldor") && KillMeForMyPowerUtils.isEikthyrDefeatedForPlayer();
                bool hildirOk = trader.gameObject.name.StartsWith("Hildir") && KillMeForMyPowerUtils.isEikthyrDefeatedForPlayer();
                bool bogWitchOk = trader.gameObject.name.StartsWith("BogWitch") && KillMeForMyPowerUtils.isElderDefeatedForPlayer();
                
                return haldorOk || hildirOk || bogWitchOk;
            }
        }
    }
}