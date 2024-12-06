namespace KillMeForMyPower
{
    public class KillMeForMyPowerUtils
    {
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
            if (bossName == "Eikthyr")
                return isEikthyrDefeatedForPlayer();
            if (bossName == "TheElder")
                return isElderDefeatedForPlayer();
            if (bossName == "Bonemass")
                return isBonemassDefeatedForPlayer();
            if (bossName == "Moder")
                return isModerDefeatedForPlayer();
            if (bossName == "Yagluth")
                return isYagluthDefeatedForPlayer();
            if (bossName == "Queen")
                return isQueenDefeatedForPlayer();
            if (bossName == "Fader")
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
            if (bossName == "Eikthyr")
                return ConfigurationFile.daysBossEikthyr.Value;
            if (bossName == "TheElder")
                return ConfigurationFile.daysBossElder.Value;
            if (bossName == "Bonemass")
                return ConfigurationFile.daysBossBonemass.Value;
            if (bossName == "Moder")
                return ConfigurationFile.daysBossModer.Value;
            if (bossName == "Yagluth")
                return ConfigurationFile.daysBossYagluth.Value;
            if (bossName == "Queen")
                return ConfigurationFile.daysBossQueen.Value;
            if (bossName == "Fader")
                return ConfigurationFile.daysBossFader.Value;
            return 0;
        }
    }
}
