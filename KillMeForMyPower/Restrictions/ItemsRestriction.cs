using HarmonyLib;

namespace KillMeForMyPower.Restrictions
{
    public class ItemsRestriction
    {
        [HarmonyPatch(typeof(Humanoid), "EquipItem")]
        public static class PlayerEquipItemPatch
        {
            [HarmonyPrefix]
            public static bool EquipItemPrefix(Humanoid __instance, ItemDrop.ItemData item, bool triggerEquipEffects, ref bool __result)
            {
                if (!ConfigurationFile.restrictUsingKeyItems.Value) return true;
                
                if (__instance is Player)
                {
                    if (item.m_shared.m_name == "$item_wishbone" && !KillMeForMyPowerUtils.isBonemassDefeatedForPlayer())
                    {
                        __instance.Message(MessageHud.MessageType.Center, ConfigurationFile.restrictUsingKeyItemsMessage.Value.Replace("{0}","$enemy_bonemass"));
                        __result = false;
                        return false;
                    }
                    else if (item.m_shared.m_name == "$item_demister" && !KillMeForMyPowerUtils.isYagluthDefeatedForPlayer())
                    {
                        __instance.Message(MessageHud.MessageType.Center, ConfigurationFile.restrictUsingKeyItemsMessage.Value.Replace("{0}","$enemy_goblinking"));
                        __result = false;
                        return false;
                    }
                }

                return true;
            }
        }

        [HarmonyPatch(typeof(Door), "Interact")]
        public static class SwampKeyRestrictionPatch
        {
            [HarmonyPrefix]
            public static bool InteractPrefix(Door __instance, Humanoid character, bool hold, ref bool __result)
            {
                if (!ConfigurationFile.restrictUsingKeyItems.Value) return true;
                
                if (__instance.name.Contains("sunken_crypt_gate") && character is Player && !KillMeForMyPowerUtils.isElderDefeatedForPlayer())
                {
                    character.Message(MessageHud.MessageType.Center, ConfigurationFile.restrictUsingKeyItemsMessage.Value.Replace("{0}","$enemy_gdking"));
                    __result = false;
                    return false;
                }

                return true;
            }
        }

    }
}