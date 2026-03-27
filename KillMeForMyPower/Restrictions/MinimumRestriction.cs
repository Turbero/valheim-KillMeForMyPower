using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using KillMeForMyPower.Managers;
using UnityEngine;

namespace KillMeForMyPower.Restrictions
{
    [HarmonyPatch(typeof(OfferingBowl), "InitiateSpawnBoss")]
    public static class InitiateSpawnBossPatch
    {
        static bool Prefix(OfferingBowl __instance, Vector3 point, bool removeItemsFromInventory)
        {
            //Detect players around
            Logger.Log("Detecting players around "+ConfigurationFile.minimumPlayersAroundRange.Value + " meters...");

            List<Player> playersNearby = new List<Player>();
            Vector3 playerPosition = Player.m_localPlayer.transform.position;
            Player.GetPlayersInRange(playerPosition, ConfigurationFile.minimumPlayersAroundRange.Value, playersNearby);

            if (playersNearby.Count < ConfigurationFile.minimumPlayersAroundAmount.Value)
            {
                Logger.Log($"{playersNearby} players detected. Minimum required: {ConfigurationFile.minimumPlayersAroundAmount.Value}. Cancelling...");
                Player.m_localPlayer.Message(MessageHud.MessageType.Center, ConfigurationFile.minimumPlayersAroundForbiddenMessage.Value.Replace("{0}", ConfigurationFile.minimumPlayersAroundAmount.Value.ToString()));
                return false;
            }
            Logger.Log("No monsters detected.");
            
            //Minimum Level
            if (ConfigurationFile.minLevelToSpawnBoss.Value && EpicMMOSystem_API.IsLoaded())
            {
                try
                {
                    string bossName = __instance.m_bossPrefab.name;
                    object monsterData = GetMonsters()[bossName + "(Clone)"];
                    if (monsterData != null)
                    {
                        int minLevel = GetMonsterLevel(monsterData);
                        if (EpicMMOSystem_API.GetLevel() < minLevel)
                        {
                            Player.m_localPlayer.Message(MessageHud.MessageType.Center, ConfigurationFile.minLevelToSpawnBossNotMet.Value.Replace("{0}", minLevel.ToString()));
                            return false;
                        }

                        Logger.Log("Level enough to spawn.");
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogWarning("Error when calculating monster level. Ignoring restriction. Error: "+ex);
                }
            }

            return true;
        }
        
        private static IDictionary GetMonsters()
        {
            var type = Type.GetType("EpicMMOSystem.DataMonsters, EpicMMOSystem");
            if (type == null) return new Dictionary<string, object>();

            var field = type.GetField("dictionary", BindingFlags.Static | BindingFlags.NonPublic);
            return field?.GetValue(null) as IDictionary;
        }
        
        private static int GetMonsterLevel(object monster)
        {
            var field = monster.GetType().GetField("level", BindingFlags.Public | BindingFlags.Instance);
            if (field != null)
            {
                return (int)field.GetValue(monster);
            }

            return 0;
        }
    }
}