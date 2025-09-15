using System.Reflection;
using HarmonyLib;
using TMPro;
using UnityEngine;

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
                if (item.m_shared.m_name == "$item_demister" && !KillMeForMyPowerUtils.HasDefeatedBossName(BossNameEnum.Yagluth))
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

    [HarmonyPatch(typeof(UITooltip), "UpdateTextElements")]
    public class UITooltipPatch
    {
        public static void Postfix(UITooltip __instance)
        {
            GameObject m_tooltip = (GameObject)GameManager.GetPrivateValue(__instance, "m_tooltip", BindingFlags.Static | BindingFlags.NonPublic);
            if (m_tooltip != null)
            {
                Transform transform = Utils.FindChild(m_tooltip.transform, "Text");
                if (transform != null)
                {
                    if (__instance.m_topic == "$item_demister")
                    {
                        bool ready = KillMeForMyPowerUtils.HasDefeatedBossName(BossNameEnum.Yagluth);
                        string text = ready 
                            ? ConfigurationFile.itemRestrictionAvailableTooltipYes.Value 
                            : ConfigurationFile.itemRestrictionAvailableTooltipNo.Value;
                        string color = ready ? "green" : "red";
                        
                        string availabilityText = $"{ConfigurationFile.itemRestrictionAvailableTooltipMessage.Value}: <color={color}>{text}</color>";
                        __instance.m_text = __instance.m_text.Replace("_description", "_description\n" + availabilityText);
                        transform.GetComponent<TMP_Text>().text = Localization.instance.Localize(__instance.m_text);
                        
                        //Remove last repetition of extra text
                        string[] parts = transform.GetComponent<TMP_Text>().text.Split('\n');
                        transform.GetComponent<TMP_Text>().text = string.Join("\n", parts, 0, parts.Length - 1);
                    }
                }
            }
        }
    }
}