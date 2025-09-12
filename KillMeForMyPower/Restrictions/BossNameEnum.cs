using System;
using System.Reflection;

namespace KillMeForMyPower.Restrictions
{
    public enum BossNameEnum
    {
        [BossNameAttr("Eikthyr_Defeated",     "$enemy_eikthyr")]     Eikthyr,
        [BossNameAttr("gd_king_Defeated",     "$enemy_gdking")]      TheElder,
        [BossNameAttr("Bonemass_Defeated",    "$enemy_bonemass")]    Bonemass,
        [BossNameAttr("Dragon_Defeated",      "$enemy_dragon")]      Moder,
        [BossNameAttr("GoblinKing_Defeated",  "$enemy_goblinking")]  Yagluth,
        [BossNameAttr("SeekerQueen_Defeated", "$enemy_seekerqueen")] Queen,
        [BossNameAttr("Fader_Defeated",       "$enemy_fader")]       Fader
    }
    
    class BossNameAttr: Attribute
    {
        internal BossNameAttr(string uniqueKey, string translationKey)
        {
            this.uniqueKey = uniqueKey;
            this.translationKey = translationKey;
        }
        public string uniqueKey { get; private set; }
        public string translationKey { get; private set; }
    }

    public static class BossNameUtils
    {
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