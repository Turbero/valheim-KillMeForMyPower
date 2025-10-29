using System;
using System.Reflection;
using BepInEx.Configuration;

namespace KillMeForMyPower.Restrictions.BossNameManagement
{
    public enum BossNameEnum
    {
        [BossNameAttr("Eikthyr",             "GP_Eikthyr",          "$enemy_eikthyr",     "playerListForBoss1EikthyrPower")]            Eikthyr,
        [BossNameAttr("gd_king",             "GP_TheElder",         "$enemy_gdking",      "playerListForBoss2TheElderPower")]           TheElder,
        [BossNameAttr("Bonemass",            "GP_Bonemass",         "$enemy_bonemass",    "playerListForBoss3BonemassPower")]           Bonemass,
        [BossNameAttr("Dragon",              "GP_Moder",            "$enemy_dragon",      "playerListForBoss4ModerPower")]              Moder,
        [BossNameAttr("GoblinKing",          "GP_Yagluth",          "$enemy_goblinking",  "playerListForBoss5YagluthPower")]            Yagluth,
        [BossNameAttr("SeekerQueen",         "GP_Queen",            "$enemy_seekerqueen", "playerListForBoss6QueenPower")]              Queen,
        [BossNameAttr("Fader",               "GP_Fader",            "$enemy_fader",       "playerListForBoss7FaderPower")]              Fader,
        [BossNameAttr(null,                  null,                  null,                 null)]                                        None,
        [BossNameAttr("BossGorr_TW",         "SE_Boss_Gorr",        "Gorr",               "playerListForBoss8TherzieGorrPower")]        SE_Boss_Gorr,
        [BossNameAttr("BossBrutalis_TW",     "SE_Boss_Brutalis",    "Brutalis",           "playerListForBoss8TherzieBrutalisPower")]    SE_Boss_Brutalis,
        [BossNameAttr("BossStormHerald_TW",  "SE_Boss_StormHerald", "StormHerald",        "playerListForBoss8TherzieStormHeralsPower")] SE_Boss_StormHerald,
        [BossNameAttr("BossSythrak_TW",      "SE_Boss_Sythrak",     "Sythrak",            "playerListForBoss8TherzieSythrakPower")]     SE_Boss_Sythrak
    }
    
    class BossNameAttr: Attribute
    {
        internal BossNameAttr(string prefabBossName, string powerKey, string translationKey, string configurationListName)
        {
            this.prefabBossName = prefabBossName;
            this.powerKey = powerKey;
            this.translationKey = translationKey;
            this.configurationListName = configurationListName;
        }
        
        public string prefabBossName { get; private set; }
        public string translationKey { get; private set; }
        public string powerKey { get; private set; }
        public string configurationListName { get; private set; }
    }

    public static class BossNameFields
    {
        public static string GetGrantedPlayerNamesList(this BossNameEnum p)
        {
            BossNameAttr attr = GetAttr(p);
            return ((ConfigEntry<string>)typeof(ConfigurationFile).GetField(attr.configurationListName, BindingFlags.Static | BindingFlags.Public)?.GetValue(null))?.Value;
        }
        public static string GetFightBossname(this BossNameEnum p)
        {
            BossNameAttr attr = GetAttr(p);
            return attr.prefabBossName;
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

        public static string GetConfigurationListName(this BossNameEnum p)
        {
            BossNameAttr attr = GetAttr(p);
            return attr.configurationListName;
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