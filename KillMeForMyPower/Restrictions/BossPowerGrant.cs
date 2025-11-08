using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using KillMeForMyPower.Restrictions.BossNameManagement;
using UnityEngine;

namespace KillMeForMyPower.Restrictions
{
    [HarmonyPatch(typeof(Character), "OnDeath")]
    public class RegisterBossDefeatPatch
    {

        // This will be executed in the player that hits last when the boss dies
        public static void Postfix(Character __instance)
        {
            if (__instance != null && __instance.IsBoss())
            {
                string bossName = __instance.name.Replace("(Clone)", "");
                Player mLocalPlayer = Player.m_localPlayer;
                
                List<string> playersToGrant = new List<string>();
                playersToGrant.Add(mLocalPlayer.GetPlayerName());
                
                //Detect players around
                if (ConfigurationFile.grantKillToNearbyPlayers.Value)
                {
                    Logger.LogInfo("Finding nearby players to grant the kill");
                    Character boss = __instance;
                    BaseAI ai = boss.GetComponent<BaseAI>();
                    float aggroRange = ai ? ai.m_viewRange : 0f;
                    Logger.LogInfo($"Detection boss range for {boss.name.Replace("(Clone)", "")} is {aggroRange} meters");

                    Vector3 bossPosition = __instance.transform.position;
                    List<Player> nearbyPlayers = Player.GetAllPlayers();
                    foreach (Player player in nearbyPlayers)
                    {
                        if (player == null) continue;
                        if (player.GetPlayerName() == mLocalPlayer.GetPlayerName()) continue;
                        
                        if (Vector3.Distance(player.transform.position, bossPosition) <= aggroRange)
                            playersToGrant.Add(player.GetPlayerName());
                    }
                }
                
                //Send RPC to host with all fighters names
                ZRoutedRpc.instance.InvokeRoutedRPC(0L, "RPC_BossPowerGrantServer", bossName, string.Join(",", playersToGrant));
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

    public class RPC_BossPowerGrantCalls
    {
        public static void RPC_BossPowerGrantServer(long sender, string bossEnumStr, string playersToGrant)
        {
            //Message to the host to sync powers list
            Logger.Log($"[RPC_BossPowerGrantServer] RPC sent from sender {sender} with {bossEnumStr} and {playersToGrant}");
            BossNameEnum bossNameEnum = KillMeForMyPowerUtils.findBossNameByPrefabName(bossEnumStr);
            string[] players = playersToGrant.Split(',');
            foreach (string player in players)
            {
                BossNameUtils.GrantBossPowerToPlayer(bossNameEnum, player, true);
                //TODO RPC para avisar al player
            }
        }
        
        public static void RPC_BossPowerRemoveGrantServer(long sender, string bossEnumStr, string playersToRemoveGrant)
        {
            //Message to the host to sync powers list
            Logger.Log($"[RPC_BossPowerRemoveGrantServer] RPC sent from sender {sender} with {bossEnumStr} and {playersToRemoveGrant}");
            BossNameEnum bossNameEnum = KillMeForMyPowerUtils.findBossNameByPrefabName(bossEnumStr);
            string[] players = playersToRemoveGrant.Split(',');
            foreach (string player in players)
            {
                BossNameUtils.GrantBossPowerToPlayer(bossNameEnum, player, false);
                //TODO RPC para avisar al player
            }
        }
    }
}