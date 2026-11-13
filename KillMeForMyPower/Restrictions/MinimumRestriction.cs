using System.Collections.Generic;
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
                return EpicMMOSystem_API.HasEnoughLevelToSpawn(__instance.m_bossPrefab.name);
            }

            return true;
        }
    }
}