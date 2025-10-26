using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace KillMeForMyPower.Restrictions
{
    public class RPCs
    {
        public const string RPC_SET_KILL_TO_PLAYER_CLIENT = "KillMeForMyPowerRPC_SetKillToPlayer_Client";
        public static void RegisterRPC()
        {
            ZRoutedRpc.instance.Register(RPC_SET_KILL_TO_PLAYER_CLIENT, new Action<long, string>(RPC_SetKillToPlayerClient));
        }
        
        public static void RPC_SetKillToPlayerClient(long sender, string bossName)
        {
            Logger.LogInfo("RPC_SetKillToPlayerClient message from "+sender+" with "+bossName);
            //I'm the client here
            if (Player.m_localPlayer == null)
                return;

            BossNameEnum bossNameEnum = KillMeForMyPowerUtils.findBossNameByPrefabName(bossName);
            GameManager.updateKeyToKMFMPKey(bossNameEnum, Player.m_localPlayer);
            Logger.LogInfo(bossNameEnum.GetUniqueKey() + " kill granted!");
            Player.m_localPlayer.Message(MessageHud.MessageType.TopLeft, bossNameEnum.GetUniqueKey() + " kill granted!"); //TODO Improve with sprite of boss in message
        }
    }
    
    [HarmonyPatch(typeof(Character), "OnDeath")]
    public class RegisterBossDefeatPatch
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
                    var peers = ZNet.instance.GetConnectedPeers(); //all remote players
                    foreach (Player player in nearbyPlayers)
                    {
                        if (player == null) continue;
                        if (player.GetPlayerName() == mLocalPlayer.GetPlayerName()) continue;
                        
                        if (Vector3.Distance(player.transform.position, bossPosition) <= aggroRange)
                        {
                            //Add also to the nearby player
                            string playerName = player.GetPlayerName();
                            Logger.LogInfo($"Sending RPC kill for {boss.name} to {playerName}...");
                            
                            //Send RPC message
                            var peer = peers.FirstOrDefault(p => p.m_playerName.Equals(playerName, StringComparison.OrdinalIgnoreCase));
                            if (peer == null)
                            {
                                Logger.LogError($"Player '{playerName}' not found among connected peers.");
                                continue;
                            }
                            ZRoutedRpc.instance.InvokeRoutedRPC(peer.m_uid, RPCs.RPC_SET_KILL_TO_PLAYER_CLIENT, bossName);
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