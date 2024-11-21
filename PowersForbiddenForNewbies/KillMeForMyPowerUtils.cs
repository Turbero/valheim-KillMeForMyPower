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

        public static bool isElderForPlayer()
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
            else if (bossName == "TheElder")
                return isElderForPlayer();
            else if (bossName == "Bonemass")
                return isBonemassDefeatedForPlayer();
            else if (bossName == "Moder")
                return isModerDefeatedForPlayer();
            else if (bossName == "Yagluth")
                return isYagluthDefeatedForPlayer();
            else if (bossName == "Queen")
                return isQueenDefeatedForPlayer();
            else if (bossName == "Fader")
                return isFaderDefeatedForPlayer();
            else
                return false;
        }
    }
}
