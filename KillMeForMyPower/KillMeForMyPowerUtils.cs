using System;

namespace KillMeForMyPower
{
    public enum BossNameEnum
    {
        Eikthyr,
        TheElder,
        Bonemass,
        Moder,
        Yagluth,
        Queen,
        Fader
    }
    public class KillMeForMyPowerUtils
    {
        private static BossNameEnum parseBossName(string value) {
            return (BossNameEnum) Enum.Parse(typeof(BossNameEnum), value, true);
        }

        public static string getBossNameTranslation(BossNameEnum bossName)
        {
            if (bossName == BossNameEnum.Eikthyr)
                return "$enemy_eikthyr";
            if (bossName == BossNameEnum.TheElder)
                return "$enemy_gdking";
            if (bossName == BossNameEnum.Bonemass)
                return "$enemy_bonemass";
            if (bossName == BossNameEnum.Moder)
                return "$enemy_dragon";
            if (bossName == BossNameEnum.Yagluth)
                return "$enemy_goblinking";
            if (bossName == BossNameEnum.Queen)
                return "$enemy_seekerqueen";
            if (bossName == BossNameEnum.Fader)
                return "$enemy_fader";
            throw new ArgumentException("Unknown boss name");
        }
        
        private static bool hasUniqueKey(string key)
        {
            return Player.m_localPlayer.HaveUniqueKey(key);
        }

        public static bool isEikthyrDefeatedForPlayer()
        {
            return hasUniqueKey("Eikthyr_Defeated");
        }

        public static bool isElderDefeatedForPlayer()
        {
            return hasUniqueKey("gd_king_Defeated");
        }

        public static bool isBonemassDefeatedForPlayer()
        {
            return hasUniqueKey("Bonemass_Defeated");
        }

        public static bool isModerDefeatedForPlayer()
        {
            return hasUniqueKey("Dragon_Defeated");
        }

        public static bool isYagluthDefeatedForPlayer()
        {
            return hasUniqueKey("GoblinKing_Defeated");
        }

        public static bool isQueenDefeatedForPlayer()
        {
            return hasUniqueKey("SeekerQueen_Defeated");
        }

        public static bool isFaderDefeatedForPlayer()
        {
            return hasUniqueKey("Fader_Defeated");
        }

        public static bool HasDefeatedBoss(string bossName)
        { 
            var parsedEnum = parseBossName(bossName);
            if (parsedEnum == BossNameEnum.Eikthyr)
                return isEikthyrDefeatedForPlayer();
            if (parsedEnum == BossNameEnum.TheElder)
                return isElderDefeatedForPlayer();
            if (parsedEnum == BossNameEnum.Bonemass)
                return isBonemassDefeatedForPlayer();
            if (parsedEnum == BossNameEnum.Moder)
                return isModerDefeatedForPlayer();
            if (parsedEnum == BossNameEnum.Yagluth)
                return isYagluthDefeatedForPlayer();
            if (parsedEnum == BossNameEnum.Queen)
                return isQueenDefeatedForPlayer();
            if (parsedEnum == BossNameEnum.Fader)
                return isFaderDefeatedForPlayer();
            return false;
        }

        public static int GetCurrentDay()
        {
            var method = typeof(EnvMan).GetMethod("GetCurrentDay", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            return (int)method.Invoke(EnvMan.instance, null);
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
            if (bossToCheck == BossNameEnum.Eikthyr)
                return isEikthyrDefeatedForPlayer();
            if (bossToCheck == BossNameEnum.TheElder)
                return isElderDefeatedForPlayer();
            if (bossToCheck == BossNameEnum.Bonemass)
                return isBonemassDefeatedForPlayer();
            if (bossToCheck == BossNameEnum.Moder)
                return isModerDefeatedForPlayer();
            if (bossToCheck == BossNameEnum.Yagluth)
                return isYagluthDefeatedForPlayer();
            if (bossToCheck == BossNameEnum.Queen)
                return isQueenDefeatedForPlayer();
            if (bossToCheck == BossNameEnum.Fader)
                return isFaderDefeatedForPlayer();
            return false;
        }
    }
}
