using HarmonyLib;
using System.Reflection;
using TMPro;
using UnityEngine;

namespace KillMeForMyPower.Restrictions
{
    [HarmonyPatch(typeof(InventoryGui), "Show")]
    public static class CraftStationDescriptionsPatch
    {
        public static bool updated = false;
        public static bool Prefix(Container container, int activeGroup)
        {
            if (updated) return true;

            foreach (var recipe in ObjectDB.instance.m_recipes)
            {
                //FIXME
                /*if (RecipeManager.recipesProfessionData.ContainsKey(recipe.name))
                {
                    string playerProfession = PlayerInfoManager.GetPlayerProfession();
                    ProfessionData pData = RecipeManager.recipesProfessionData.GetValueSafe(recipe.name);
                    string profession = pData.professionName;
                    string color = playerProfession == profession ? "green" : "red";

                    //Add profession to tooltip
                    string description = recipe.m_item.m_itemData.m_shared.m_description.Split('\n')[0]; // To remove profession from previous execution
                    recipe.m_item.m_itemData.m_shared.m_description = description + //do not use "+=" !
                        $"\n{ConfigurationFile.profession.Value}: <color={color}>{profession}</color>" +
                        (
                            (pData.SkillType != Skills.SkillType.None && pData.MinSkillLevel > 0)
                                ? $"\n(Skill = $skill_{pData.SkillType.ToString().ToLower()}, $msg_level = {pData.MinSkillLevel})"
                                : ""
                        );
                }*/
            }
            updated = true;

            return true;
        }
    }

    [HarmonyPatch(typeof(UITooltip), "UpdateTextElements")]
    public static class UITooltipPatch
    {
        public static void Postfix(UITooltip __instance)
        {
            GameObject m_tooltip = (GameObject)GameManager.GetPrivateValue(__instance, "m_tooltip", BindingFlags.Static | BindingFlags.NonPublic);
            if (m_tooltip != null)
            {
                Transform transform = Utils.FindChild(m_tooltip.transform, "Text");
                if (transform != null)
                {
                    //FIXME
                    /*ProfessionData pData = RecipeManager.FindRecipeByItemName(__instance.m_topic);
                    if (pData?.ObjectName != null)
                    {
                        string playerProfession = PlayerInfoManager.GetPlayerProfession();
                        string profession = pData.professionName;
                        string color = playerProfession == profession ? "green" : "red";

                        if (!(__instance.m_text.Contains(ConfigurationFile.profession.Value)))
                        {
                            //Add profession
                            var professionText =
                                $"\n{ConfigurationFile.profession.Value}: <color={color}>{profession}</color>" +
                                (
                                    (pData.SkillType != Skills.SkillType.None && pData.MinSkillLevel > 0)
                                        ? $"\n(Skill = $skill_{pData.SkillType.ToString().ToLower()}, $msg_level = {pData.MinSkillLevel})"
                                        : ""
                                );

                            __instance.m_text = __instance.m_text.Replace("_description", "_description " + professionText);
                        }
                        transform.GetComponent<TMP_Text>().text = Localization.instance.Localize(__instance.m_text);
                    }*/
                }

            }
        }
    }
}
