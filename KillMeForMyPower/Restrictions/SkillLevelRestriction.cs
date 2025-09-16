using HarmonyLib;

namespace KillMeForMyPower.Restrictions
{
    [HarmonyPatch(typeof(Player), nameof(Player.RaiseSkill))]
    public class Player_RaiseSkill_Prefix_Patch
    {
        static bool Prefix(Player __instance, Skills.SkillType skill, float value)
        {
            Logger.Log("Checking skill level availability...");

            if (__instance != null)
            {
                float skillLevelValue = __instance.GetSkillLevel(skill);
                if (skillLevelValue > 0 && skillLevelValue < Skills.c_MaxSkillLevel)
                {
                    float futureSkillLevelValue = __instance.GetSkillLevel(skill) + value;
                    
                    bool decision = !(
                        (!KillMeForMyPowerUtils.HasDefeatedBossName(BossNameEnum.Eikthyr) && futureSkillLevelValue > ConfigurationFile.maxLevelBeforeBoss1Eikthyr.Value) ||
                        (!KillMeForMyPowerUtils.HasDefeatedBossName(BossNameEnum.TheElder) && futureSkillLevelValue > ConfigurationFile.maxLevelBeforeBoss2TheElder.Value) ||
                        (!KillMeForMyPowerUtils.HasDefeatedBossName(BossNameEnum.Bonemass) && futureSkillLevelValue > ConfigurationFile.maxLevelBeforeBoss3Bonemass.Value) ||
                        (!KillMeForMyPowerUtils.HasDefeatedBossName(BossNameEnum.Moder) && futureSkillLevelValue > ConfigurationFile.maxLevelBeforeBoss4Moder.Value) ||
                        (!KillMeForMyPowerUtils.HasDefeatedBossName(BossNameEnum.Yagluth) && futureSkillLevelValue > ConfigurationFile.maxLevelBeforeBoss5Yagluth.Value) ||
                        (!KillMeForMyPowerUtils.HasDefeatedBossName(BossNameEnum.Queen) && futureSkillLevelValue > ConfigurationFile.maxLevelBeforeBoss6Queen.Value) ||
                        (!KillMeForMyPowerUtils.HasDefeatedBossName(BossNameEnum.Fader) && futureSkillLevelValue > ConfigurationFile.maxLevelBeforeBoss7Fader.Value)
                    );
                    Logger.Log($"skill up decision. skill: {skill}, skillLevelValue: {skillLevelValue}, futureSKillLevelValue: {futureSkillLevelValue}. decision: {decision}");
                    return decision;
                }
            }

            return true;
        }
    }
}