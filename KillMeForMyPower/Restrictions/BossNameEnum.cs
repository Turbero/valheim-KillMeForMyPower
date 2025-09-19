using System;
using System.Reflection;

namespace KillMeForMyPower.Restrictions
{
    public enum BossNameEnum
    {
        [BossNameAttr("Eikthyr",     "Eikthyr_Defeated",     "Eikthyr_Defeated_KMFMP",     "GP_Eikthyr",  "$enemy_eikthyr")]     Eikthyr,
        [BossNameAttr("gd_king",     "gd_king_Defeated",     "gd_king_Defeated_KMFMP",     "GP_TheElder", "$enemy_gdking")]      TheElder,
        [BossNameAttr("Bonemass",    "Bonemass_Defeated",    "Bonemass_Defeated_KMFMP",    "GP_Bonemass", "$enemy_bonemass")]    Bonemass,
        [BossNameAttr("Dragon",      "Dragon_Defeated",      "Dragon_Defeated_KMFMP",      "GP_Moder",    "$enemy_dragon")]      Moder,
        [BossNameAttr("GoblinKing",  "GoblinKing_Defeated",  "GoblinKing_Defeated_KMFMP",  "GP_Yagluth",  "$enemy_goblinking")]  Yagluth,
        [BossNameAttr("SeekerQueen", "SeekerQueen_Defeated", "SeekerQueen_Defeated_KMFMP", "GP_Queen",    "$enemy_seekerqueen")] Queen,
        [BossNameAttr("Fader",       "Fader_Defeated",       "Fader_Defeated_KMFMP",       "GP_Fader",    "$enemy_fader")]       Fader,
        [BossNameAttr(null,                   null,                   null,                         null,          null)]                 None
    }
    
    class BossNameAttr: Attribute
    {
        internal BossNameAttr(string fightBossName, string oldKey, string uniqueKey, string powerKey, string translationKey)
        {
            this.fightBossName = fightBossName;
            this.oldKey = oldKey;
            this.uniqueKey = uniqueKey;
            this.powerKey = powerKey;
            this.translationKey = translationKey;
        }
        
        public string fightBossName { get; private set; }
        public string oldKey { get; private set; }
        public string uniqueKey { get; private set; }
        public string translationKey { get; private set; }
        public string powerKey { get; private set; }
    }

    public static class BossNameUtils
    {
        public static string GetFightBossname(this BossNameEnum p)
        {
            BossNameAttr attr = GetAttr(p);
            return attr.fightBossName;
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