using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;

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
                Player mLocalPlayer = Player.m_localPlayer;
                GameManager.updateKeyToKMFMPKey(KillMeForMyPowerUtils.findBossNameByPrefabName(bossName), mLocalPlayer);
                
                //Detect players around
                if (ConfigurationFile.grantKillToNearbyPlayers.Value)
                {
                    Logger.Log("Finding nearby players to grant the kill");
                    Character boss = __instance;
                    BaseAI ai = boss.GetComponent<BaseAI>();
                    float aggroRange = ai ? ai.m_viewRange : 0f;
                    Logger.Log($"Detection boss range for {boss.name} is {aggroRange} meters");

                    Vector3 bossPosition = __instance.transform.position;
                    List<Player> nearbyPlayers = Player.GetAllPlayers();
                    foreach (Player player in nearbyPlayers)
                    {
                        if (player == null) continue;
                        if (Vector3.Distance(player.transform.position, bossPosition) <= aggroRange)
                        {
                            //Add also to the player nearby
                            Logger.Log($"Also adding key of {boss.name} to {player.GetPlayerName()}...");
                            GameManager.updateKeyToKMFMPKey(KillMeForMyPowerUtils.findBossNameByPrefabName(bossName), player);
                        }
                    }
                }
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