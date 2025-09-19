using HarmonyLib;
using System;

namespace KillMeForMyPower.Restrictions
{
    public class BossPowerRestriction
    {
        [HarmonyPatch(typeof(Player), "ActivateGuardianPower")]
        public static class ActivateGuardianPowerPatch
        {
            public static bool Prefix(Player __instance, ref bool __result)
            {
                string selectedPower = __instance.GetGuardianPowerName().Replace("GP_", "");
                Logger.Log("Guardian power name: " + selectedPower);
                
                if (!KillMeForMyPowerUtils.HasDefeatedBossNameStr(selectedPower) && 
                    KillMeForMyPowerUtils.GetCurrentDay() < KillMeForMyPowerUtils.GetBossMinimumDayForPower(selectedPower))
                {
                    __instance.Message(MessageHud.MessageType.Center, ConfigurationFile.forbiddenMessage.Value);
                    ApplyBlockedEffect(selectedPower);

                    __result = false;
                    return false;
                }

                return true;
            }
        }

        [HarmonyPatch(typeof(ItemStand), "DelayedPowerActivation")]
        public static class DelayedPowerActivationPatch
        {
            public static bool Prefix(ItemStand __instance)
            {
                string guardianPowerName = __instance.m_guardianPower?.name.Replace("GP_", "");
                Logger.Log("guardianPowerName: "+ guardianPowerName);
                if (!KillMeForMyPowerUtils.HasDefeatedBossNameStr(guardianPowerName) &&
                    KillMeForMyPowerUtils.GetCurrentDay() < KillMeForMyPowerUtils.GetBossMinimumDayForPower(guardianPowerName))
                {
                    Player.m_localPlayer.Message(MessageHud.MessageType.Center, ConfigurationFile.forbiddenMessage.Value);
                    ApplyBlockedEffect(guardianPowerName);
                    return false;
                }

                return true;
            }
        }

        [HarmonyPatch(typeof(Character), "OnDeath")]
        public static class RegisterBossDefeatPatch
        {
            public static void Postfix(Character __instance)
            {
                if (__instance != null && __instance.IsBoss())
                {
                    string bossName = __instance.name.Replace("(Clone)", "");
                    Player player = Player.m_localPlayer;
                    GameManager.updateKeyToKMFMPKey(KillMeForMyPowerUtils.parseFightBossName(bossName), player);
                }
            }
        }

        private static void ApplyBlockedEffect(string bossName)
        {
            SEMan seMan = Player.m_localPlayer.GetSEMan();
            BossNameEnum parsedBossName = (BossNameEnum)Enum.Parse(typeof(BossNameEnum), bossName, true);
            if (parsedBossName == BossNameEnum.Eikthyr)
            {
                StatusEffect se = seMan?.AddStatusEffect("Lightning".GetHashCode(), resetTime: false);
                se.m_ttl = 5;
            }
            else if (parsedBossName == BossNameEnum.TheElder)
            {
                SE_Burning se = (SE_Burning)seMan?.AddStatusEffect("Burning".GetHashCode(), resetTime: false);
                se.AddFireDamage(Math.Max(2, Player.m_localPlayer.GetHealth() - 10f));
            }
            else if (parsedBossName == BossNameEnum.Bonemass)
            {
                SE_Poison se = (SE_Poison)seMan?.AddStatusEffect("Poison".GetHashCode(), resetTime: false);
                se.AddDamage(Math.Max(1, Player.m_localPlayer.GetHealth() - 10f));
            }
            else if (parsedBossName == BossNameEnum.Moder)
            {
                SE_Frost se = (SE_Frost)seMan?.AddStatusEffect("Frost".GetHashCode(), resetTime: false);
                se.m_ttl = 5;
            }
            else if (parsedBossName == BossNameEnum.Yagluth)
            {
                SE_Burning se = (SE_Burning)seMan?.AddStatusEffect("Burning".GetHashCode(), resetTime: false);
                se.AddFireDamage(Math.Max(2, Player.m_localPlayer.GetHealth() - 10f));
            }
            else if (parsedBossName == BossNameEnum.Queen)
            {
                SE_Poison se = (SE_Poison)seMan?.AddStatusEffect("Poison".GetHashCode(), resetTime: false);
                se.AddDamage(Math.Max(1, Player.m_localPlayer.GetHealth() - 10f));
            }
            else if (parsedBossName == BossNameEnum.Fader)
            {
                SE_Burning se = (SE_Burning)seMan?.AddStatusEffect("Burning".GetHashCode(), resetTime: false);
                se.AddFireDamage(Math.Max(2, Player.m_localPlayer.GetHealth() - 10f));
            }
        }
    }
}