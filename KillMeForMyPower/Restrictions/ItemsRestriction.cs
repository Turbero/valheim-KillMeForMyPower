using HarmonyLib;

namespace KillMeForMyPower.Restrictions
{
    [HarmonyPatch(typeof(Humanoid), "EquipItem")]
    public class PlayerEquipItemPatch
    {
        [HarmonyPrefix]
        public static bool EquipItemPrefix(Humanoid __instance, ItemDrop.ItemData item, bool triggerEquipEffects, ref bool __result)
        {
            if (!ConfigurationFile.restrictUsingKeyItems.Value) return true;
            
            if (__instance is Player)
            {
                if (item.m_shared.m_name == "$item_wishbone" && !KillMeForMyPowerUtils.HasDefeatedBossName(BossNameEnum.Bonemass))
                {
                    __instance.Message(MessageHud.MessageType.Center, ConfigurationFile.restrictUsingKeyItemsMessage.Value.Replace("{0}", BossNameEnum.Bonemass.GetTranslationKey()));
                    __result = false;
                    return false;
                }
                else if (item.m_shared.m_name == "$item_demister" && !KillMeForMyPowerUtils.HasDefeatedBossName(BossNameEnum.Yagluth))
                {
                    __instance.Message(MessageHud.MessageType.Center, ConfigurationFile.restrictUsingKeyItemsMessage.Value.Replace("{0}", BossNameEnum.Yagluth.GetTranslationKey()));
                    __result = false;
                    return false;
                }
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(Door), "Interact")]
    public class SwampKeyRestrictionPatch
    {
        [HarmonyPrefix]
        public static bool InteractPrefix(Door __instance, Humanoid character, bool hold, ref bool __result)
        {
            if (!ConfigurationFile.restrictUsingKeyItems.Value) return true;
            
            if (__instance.name.Contains("sunken_crypt_gate") && character is Player && !KillMeForMyPowerUtils.HasDefeatedBossName(BossNameEnum.TheElder))
            {
                character.Message(MessageHud.MessageType.Center, ConfigurationFile.restrictUsingKeyItemsMessage.Value.Replace("{0}", BossNameEnum.TheElder.GetTranslationKey()));
                __result = false;
                return false;
            }

            return true;
        }
    }
}