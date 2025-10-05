using System.Linq;
using HarmonyLib;

namespace KillMeForMyPower.Restrictions
{
    [HarmonyPatch(typeof(Character), "OnDeath")]
    public static class RegisterBossDefeatPatch
    {
        public static void Postfix(Character __instance)
        {
            if (__instance != null && __instance.IsBoss())
            {
                string bossName = __instance.name.Replace("(Clone)", "");
                Player player = Player.m_localPlayer;
                GameManager.updateKeyToKMFMPKey(KillMeForMyPowerUtils.findBossNameByPrefabName(bossName), player);
            }
        }
    }
    
    [HarmonyPatch(typeof(Player), "Save")]
    public class Player_Save_Null_Key_Clean_Patch
    {
        [HarmonyPrefix]
        public static void Prefix(Player __instance)
        {
            var invalidKeys = __instance.m_customData.Keys
                .Where(k => string.IsNullOrEmpty(k) || k == "null")
                .ToList();

            foreach (var key in invalidKeys)
            {
                Logger.LogWarning($"Removing null key from player {__instance.GetPlayerName()}");
                __instance.RemoveUniqueKey(key);
            }
        }
    }
}