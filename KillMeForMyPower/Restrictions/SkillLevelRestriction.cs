using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using HarmonyLib;
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
            float currentLevel = playerSkill.m_level + playerSkill.GetLevelPercentage();
                
            bool decision = canSkillUp(currentLevel);
            Logger.Log($"Decision with skill {skillType}, level {playerSkill.m_level}, percentage {playerSkill.GetLevelPercentage()} and currentLevel {currentLevel}: {decision}");
            if (!decision && updateBuffTextIfDecisionFalse)
            {
                playerSkill.m_accumulator = 0;
                Logger.Log($"{skillType} rounded to max allowed value: {playerSkill.m_level}");

                //Reset buff for DetailedLevels mod
                Logger.Log("Checking DetailedLevels buff");
                List<StatusEffect> statusEffects =
                    (List<StatusEffect>)GameManager.GetPrivateValue(Player.m_localPlayer.GetSEMan(), "m_statusEffects");
                StatusEffect buffEffect = statusEffects.Find(effect => effect.m_name.Contains(buffName));
                if (buffEffect != null)
                {
                    Logger.Log("Fixing value in buff");
                    buffEffect.m_name = $"{buffName}: {playerSkill.m_level}";
                }
            }

            return decision;
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
            Logger.Log("Checking blood magic here since it doesn't seem to go through Player.RaiseSkill...");
            if (__instance != null && __instance.IsMonsterFaction(0f))
            {
                Character attacker = hit.GetAttacker();
                if (attacker != null && attacker.IsTamed())
                {
                    _ = WaitForSecondsAsync(0.1f); // Small delay in async method to wait for updating blood magic skill
                }
            }
        }

        private static async Task WaitForSecondsAsync(float seconds)
        {
            await Task.Delay((int)(Math.Max(0f, seconds) * 1000)); // to milliseconds
            LevelCalculation.reviewAndUpdateSkill(Player.m_localPlayer, Skills.SkillType.BloodMagic, "$skill_bloodmagic");
        }
    }
}