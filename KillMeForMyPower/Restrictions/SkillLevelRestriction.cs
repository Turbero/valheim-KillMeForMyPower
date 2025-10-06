using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using HarmonyLib;
using TMPro;
using UnityEngine;

namespace KillMeForMyPower.Restrictions
{
    public class LevelCalculation
    {
        public static bool canSkillUp(float skillLevelValue)
        {
            bool decision = !(
                (!KillMeForMyPowerUtils.HasDefeatedBossName(BossNameEnum.Eikthyr) && skillLevelValue >= ConfigurationFile.maxLevelBeforeBoss1Eikthyr.Value) ||
                (!KillMeForMyPowerUtils.HasDefeatedBossName(BossNameEnum.TheElder) && skillLevelValue >= ConfigurationFile.maxLevelBeforeBoss2TheElder.Value) ||
                (!KillMeForMyPowerUtils.HasDefeatedBossName(BossNameEnum.Bonemass) && skillLevelValue >= ConfigurationFile.maxLevelBeforeBoss3Bonemass.Value) ||
                (!KillMeForMyPowerUtils.HasDefeatedBossName(BossNameEnum.Moder) && skillLevelValue >= ConfigurationFile.maxLevelBeforeBoss4Moder.Value) ||
                (!KillMeForMyPowerUtils.HasDefeatedBossName(BossNameEnum.Yagluth) && skillLevelValue >= ConfigurationFile.maxLevelBeforeBoss5Yagluth.Value) ||
                (!KillMeForMyPowerUtils.HasDefeatedBossName(BossNameEnum.Queen) && skillLevelValue >= ConfigurationFile.maxLevelBeforeBoss6Queen.Value) ||
                (!KillMeForMyPowerUtils.HasDefeatedBossName(BossNameEnum.Fader) && skillLevelValue >= ConfigurationFile.maxLevelBeforeBoss7Fader.Value)
            );
            return decision;
        }

        public static bool reviewAndUpdateSkill(Player player, Skills.SkillType skillType, string buffName, bool updateBuffTextIfDecisionFalse = true)
        {
            Skills.Skill playerSkill = player.GetSkills().GetSkillList().Find(s => s.m_info.m_skill == skillType);
            int skillLevel = (int)playerSkill.m_level; //Remove possible decimals and round down at the same time
            float currentLevel = skillLevel + playerSkill.GetLevelPercentage();
                
            bool skillUpAllowed = canSkillUp(currentLevel);
            Logger.Log($"Decision with skill {skillType}, skillLevel {skillLevel}, percentage {playerSkill.GetLevelPercentage()} and currentLevel {currentLevel}: {skillUpAllowed}");
            if (!skillUpAllowed && updateBuffTextIfDecisionFalse)
            {
                playerSkill.m_level = skillLevel;
                playerSkill.m_accumulator = 0;
                Logger.Log($"{skillType} rounded to max allowed value: {skillLevel}");

                //Reset buff for DetailedLevels mod
                Logger.Log("Checking DetailedLevels buff");
                List<StatusEffect> statusEffects = (List<StatusEffect>)GameManager.GetPrivateValue(Player.m_localPlayer.GetSEMan(), "m_statusEffects");
                StatusEffect buffEffect = statusEffects.Find(effect => effect.m_name.Contains(buffName));
                if (buffEffect != null)
                {
                    Logger.Log("Fixing value in buff");
                    buffEffect.m_name = $"{buffName}: {playerSkill.m_level}";
                }
            }

            return skillUpAllowed;
        }
    }
    
    [HarmonyPatch(typeof(Player), nameof(Player.RaiseSkill))]
    public class Player_RaiseSkill_Prefix_Patch
    {
        static bool Prefix(Player __instance, Skills.SkillType skill, float value)
        {
            if (__instance != null)
            {
                float levelBeforeAddingSkillUp = __instance.GetSkills().GetSkillLevel(skill);
                Logger.Log($"Checking skill level availability {skill} with {levelBeforeAddingSkillUp}...");
                if (levelBeforeAddingSkillUp < Skills.c_MaxSkillLevel)
                {
                    return LevelCalculation.reviewAndUpdateSkill(__instance, skill, $"$skill_{skill.ToString().ToLower()}", false);
                }
            }

            return true;
        }
    }
    
    [HarmonyPatch]
    [HarmonyPriority(Priority.VeryHigh)]
    public class Dodge_ManualUpdate_Patch
    {
        static MethodBase TargetMethod()
        {
            return AccessTools.Method(typeof(Player), "Dodge");
        }

        static void Postfix(ref Player __instance, Vector3 dodgeDir)
        {
            Logger.Log("Checking dodge skill here since it doesn't seem to go through Player.RaiseSkill...");
            _ = WaitForSecondsAsync(__instance, 0.1f); // Small delay in async method to wait for updating dodge skill
        }
        private static async Task WaitForSecondsAsync(Player player, float seconds)
        {
            await Task.Delay((int)(Math.Max(0f, seconds) * 1000)); // to milliseconds
            LevelCalculation.reviewAndUpdateSkill(player, Skills.SkillType.Dodge, "$skill_dodge");
        }
    }
    
    [HarmonyPatch(typeof(Character), "Damage")]
    public class BloodMagic_ManualUpdate_Patch
    {
        static void Postfix(Character __instance, HitData hit)
        {
            if (__instance != null)
            {
                Character attacker = hit.GetAttacker();
                if (attacker != null)
                {
                    // Small delay in async method to wait for updating blood magic skill
                    
                    // if attacker is a pet/invocation and attacked is a monster there is a bloodmagic skillup!
                    if (attacker.IsTamed() && __instance.IsMonsterFaction(0f))
                        _ = WaitForSecondsAsync(null, 0.1f);
                    // attacker is a monster and attacked is a player. If the magic barriers breaks, there is a bloodmagic skillup!
                    else if  (attacker.IsMonsterFaction(0f) && __instance.GetType() == typeof(Player))
                        _ = WaitForSecondsAsync(__instance as Player, 0.1f);
                    // attacker is a Troll_Summoned and attacked is Player or Tamed
                    else if (attacker.name.Contains("Troll_Summoned"))
                        if (__instance.GetType() == typeof(Player))
                            _ = WaitForSecondsAsync(__instance as Player, 0.1f);
                        else if (__instance.IsTamed())
                            _ = WaitForSecondsAsync(null, 0.1f);
                }
            }
        }
        
        private static async Task WaitForSecondsAsync(Player player, float seconds)
        {
            Logger.Log("Checking blood magic here since it doesn't seem to go through Player.RaiseSkill...");
            await Task.Delay((int)(Math.Max(0f, seconds) * 1000)); // to milliseconds
            LevelCalculation.reviewAndUpdateSkill(player == null ? Player.m_localPlayer : player, Skills.SkillType.BloodMagic, "$skill_bloodmagic");
        }
    }
    
    [HarmonyPatch(typeof(SkillsDialog), nameof(SkillsDialog.Setup))]
    [HarmonyPriority(Priority.VeryHigh)]
    public class SkillsDialogAdditions_Patch
    {
        static void Postfix(SkillsDialog __instance, ref Player player, ref List<GameObject> ___m_elements)
        {
            List<Skills.Skill> skillList = player.GetSkills().GetSkillList();
            for (int j = 0; j < skillList.Count; j++)
            {
                GameObject obj = ___m_elements[j];
                Skills.Skill skill = skillList[j];

                Color skillNumberColor = LevelCalculation.canSkillUp((int)skill.m_level) ? Color.white : Color.red;
                Utils.FindChild(obj.transform, "leveltext", (Utils.IterativeSearchType)0).GetComponent<TMP_Text>().color = skillNumberColor;
            }
        }
    }
}