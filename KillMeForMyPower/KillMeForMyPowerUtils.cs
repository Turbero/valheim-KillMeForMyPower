using System;
using KillMeForMyPower.Restrictions;

namespace KillMeForMyPower
{
    public class KillMeForMyPowerUtils
    {
        public static BossNameEnum parseBossName(string value) {
            Logger.Log("Parsing value: " + value);
            return (BossNameEnum) Enum.Parse(typeof(BossNameEnum), value, true);
        }
        
        public static BossNameEnum parseFightBossName(string value) {
            Logger.Log($"Parsing fighting bossName {value} into Enum");
            
            foreach (BossNameEnum bossNameEnum in Enum.GetValues(typeof(BossNameEnum)))
            {
                if (bossNameEnum.GetFightBossname() == value)
                    return bossNameEnum;
            }

            Logger.LogWarning($"{value} not found as fightBossName.");
            return BossNameEnum.None;
        }

        public static string getBossNameTranslation(BossNameEnum bossName)
        {
            return bossName.GetTranslationKey();
        }
        
        public static bool HasDefeatedBossNameStr(string bossName)
        {
            try
            {
                var parsedEnum = parseBossName(bossName);
                return HasDefeatedBossName(parsedEnum);
            }
            catch (Exception e)
            {
                Logger.LogError("Error in HasDefeatedBossName with "+bossName+", possibly parseBossName. "+e);
                return false;
            }
        }
        
        public static bool HasDefeatedBossName(BossNameEnum bossNameEnum)
        {
            bool hasDefeated = Player.m_localPlayer.HaveUniqueKey(bossNameEnum.GetUniqueKey());
            if (!hasDefeated)
            {
                hasDefeated = ConfigurationFile.activateMidPlayDetection.Value && Player.m_localPlayer.HaveUniqueKey(bossNameEnum.GetPowerKey());
                if (hasDefeated)
                {
                    //Take the chance to update key
                    Player player = Player.m_localPlayer;
                    GameManager.updateKeyToKMFMPKey(bossNameEnum, player);
                }
            }

            return hasDefeated;
        }

        public static int GetCurrentDay()
        {
            return (int) GameManager.GetPrivateMethod(EnvMan.instance, "GetCurrentDay");
        }

        public static int GetBossMinimumDayForPower(string bossName)
        {
            var parsedEnum = parseBossName(bossName);
            if (parsedEnum == BossNameEnum.Eikthyr)
                return ConfigurationFile.daysBossEikthyr.Value;
            if (parsedEnum == BossNameEnum.TheElder)
                return ConfigurationFile.daysBossElder.Value;
            if (parsedEnum == BossNameEnum.Bonemass)
                return ConfigurationFile.daysBossBonemass.Value;
            if (parsedEnum == BossNameEnum.Moder)
                return ConfigurationFile.daysBossModer.Value;
            if (parsedEnum == BossNameEnum.Yagluth)
                return ConfigurationFile.daysBossYagluth.Value;
            if (parsedEnum == BossNameEnum.Queen)
                return ConfigurationFile.daysBossQueen.Value;
            if (parsedEnum == BossNameEnum.Fader)
                return ConfigurationFile.daysBossFader.Value;
            return 0;
        }

        public static bool bossIsKilled(string bossToCheck)
        {
            return bossIsKilled(parseBossName(bossToCheck));
        }

        public static bool bossIsKilled(BossNameEnum bossToCheck)
        {
            return HasDefeatedBossName(bossToCheck);
        }
    }
}
