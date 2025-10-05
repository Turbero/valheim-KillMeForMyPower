using System;
using System.Reflection;

namespace KillMeForMyPower.Restrictions
{
    public enum BossNameEnum
    {
        [BossNameAttr("Eikthyr",             "GP_Eikthyr",          "$enemy_eikthyr")]     Eikthyr,
        [BossNameAttr("gd_king",             "GP_TheElder",         "$enemy_gdking")]      TheElder,
        [BossNameAttr("Bonemass",            "GP_Bonemass",         "$enemy_bonemass")]    Bonemass,
        [BossNameAttr("Dragon",              "GP_Moder",            "$enemy_dragon")]      Moder,
        [BossNameAttr("GoblinKing",          "GP_Yagluth",          "$enemy_goblinking")]  Yagluth,
        [BossNameAttr("SeekerQueen",         "GP_Queen",            "$enemy_seekerqueen")] Queen,
        [BossNameAttr("Fader",               "GP_Fader",            "$enemy_fader")]       Fader,
        [BossNameAttr(null,                  null,                  null)]                 None,
        [BossNameAttr("BossGorr_TW",         "SE_Boss_Gorr",        "Gorr")]               SE_Boss_Gorr,
        [BossNameAttr("BossBrutalis_TW",     "SE_Boss_Brutalis",    "Brutalis")]           SE_Boss_Brutalis,
        [BossNameAttr("BossStormHerald_TW",  "SE_Boss_StormHerald", "StormHerald")]        SE_Boss_StormHerald,
        [BossNameAttr("BossSythrak_TW",      "SE_Boss_Sythrak",     "Sythrak")]            SE_Boss_Sythrak
    }
    
    class BossNameAttr: Attribute
    {
        public string oldKey;
        public string uniqueKey;
        internal BossNameAttr(string prefabBossName, string powerKey, string translationKey)
        {
            this.prefabBossName = prefabBossName;
            oldKey = prefabBossName + "_Defeated";
            uniqueKey = prefabBossName + "_Defeated_KMFMP";
            this.powerKey = powerKey;
            this.translationKey = translationKey;
        }
        
        public string prefabBossName { get; private set; }
        public string translationKey { get; private set; }
        public string powerKey { get; private set; }
    }

    public static class BossNameUtils
    {
        public static string GetFightBossname(this BossNameEnum p)
        {
            BossNameAttr attr = GetAttr(p);
            return attr.prefabBossName;
        }

        public static string GetOldKey(this BossNameEnum p)
        {
            BossNameAttr attr = GetAttr(p);
            return attr.oldKey;
        }

        public static string GetUniqueKey(this BossNameEnum p)
        {
            BossNameAttr attr = GetAttr(p);
            return attr.uniqueKey;
        }
        public static string GetTranslationKey(this BossNameEnum p)
        {
            BossNameAttr attr = GetAttr(p);
            return attr.translationKey;
        }

        public static string GetPowerKey(this BossNameEnum p)
        {
            BossNameAttr attr = GetAttr(p);
            return attr.powerKey;
        }

        private static BossNameAttr GetAttr(BossNameEnum p)
        {
            return (BossNameAttr)Attribute.GetCustomAttribute(ForValue(p), typeof(BossNameAttr));
        }

        private static MemberInfo ForValue(BossNameEnum p)
        {
            return typeof(BossNameEnum).GetField(Enum.GetName(typeof(BossNameEnum), p));
        }
    }
}