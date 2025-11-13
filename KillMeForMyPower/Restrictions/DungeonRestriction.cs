using System;
using HarmonyLib;
using KillMeForMyPower.Managers;
using KillMeForMyPower.Restrictions.BossNameManagement;
using UnityEngine;

namespace KillMeForMyPower.Restrictions
{
    [HarmonyPatch(typeof(Teleport), "OnTriggerEnter")]
    [HarmonyPriority(Priority.VeryHigh)]
    public class Teleport_OnTriggerEnter_Patch
    {
        [HarmonyPrefix]
        public static bool Prefix(Teleport __instance, Collider collider)
        {
            if (!ConfigurationFile.restrictEnteringDungeonsBeforeKillingBosses.Value) return true;
            
            Logger.Log("[Teleport - OnTriggerEnter] entered");
            string targetName = __instance.name.Replace("(Clone)", "").ToLower();
            Logger.Log($"[Teleport - OnTriggerEnter] Trying to teleport the player through {targetName}");
            var player = collider.GetComponentInParent<Player>();
            if (player == null) return true;

            if (!ConfigurationFile.restrictEnteringDungeonsBeforeKillingBossesAdmins.Value && GameManager.IsAdmin(player))
            {
                Logger.Log("Player "+player.GetPlayerName()+" is admin. Skipping restriction to enter...");
                return true;
            }
            
            if (targetName == "gateway")
            {
                if (player.GetCurrentBiome() == Heightmap.Biome.BlackForest && !KillMeForMyPowerUtils.HasDefeatedBossName(BossNameEnum.Eikthyr))
                    return negateAccess(player, BossNameEnum.Eikthyr);
                if (player.GetCurrentBiome() == Heightmap.Biome.Swamp && !KillMeForMyPowerUtils.HasDefeatedBossName(BossNameEnum.TheElder))
                    return negateAccess(player, BossNameEnum.TheElder);
                if (player.GetCurrentBiome() == Heightmap.Biome.Mountain && !KillMeForMyPowerUtils.HasDefeatedBossName(BossNameEnum.Bonemass))
                    return negateAccess(player, BossNameEnum.Bonemass);
                if (player.GetCurrentBiome() == Heightmap.Biome.Mistlands && !KillMeForMyPowerUtils.HasDefeatedBossName(BossNameEnum.Yagluth))
                    return negateAccess(player, BossNameEnum.Yagluth);
                if (player.GetCurrentBiome() == Heightmap.Biome.AshLands && !KillMeForMyPowerUtils.HasDefeatedBossName(BossNameEnum.Queen))
                    return negateAccess(player, BossNameEnum.Queen);
            }

            if (ConfigurationFile.restrictExitingQueenDungeonIfAlive.Value && targetName == "exteriorgateway" && IsQueenNearbyAndAlert(player))
            {
                player.Message(MessageHud.MessageType.Center, ConfigurationFile.restrictExitingQueenDungeonIfAliveMessage.Value);
                Effects.scareEffect();
                return false;
            }

            return true;
        }

        private static bool negateAccess(Player player, BossNameEnum bossName)
        {
            player.Message(MessageHud.MessageType.Center, ConfigurationFile.restrictEnteringDungeonsBeforeKillingBossesMessage.Value
                .Replace("{0}", Localization.instance.Localize(bossName.GetTranslationKey())));
            Effects.scareEffect();
            return false;
        }
        
        public static bool IsQueenNearbyAndAlert(Player player)
        {
            Vector3 playerPos = player.transform.position;

            foreach (Character c in Character.GetAllCharacters())
            {
                if (c != null && !c.IsDead() && c.name.StartsWith("SeekerQueen", StringComparison.OrdinalIgnoreCase))
                {
                    if (!c.GetComponent<BaseAI>().IsAlerted())
                    {
                        Logger.LogInfo("SeekerQueen is close but not alerted. Lucky!");
                        return false;
                    } 
                    
                    float distance = Vector3.Distance(playerPos, c.transform.position);
                    Logger.Log("SeekerQueen is " + distance + " meters away!");
                    if (distance <= 60f) //60 meters is the height of the infested citadel (approx.)
                        return true;
                }
            }

            return false;
        }
    }
}