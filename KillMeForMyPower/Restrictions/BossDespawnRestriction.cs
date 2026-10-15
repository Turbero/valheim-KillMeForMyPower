using System;
using System.Collections.Generic;
using HarmonyLib;
using KillMeForMyPower.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KillMeForMyPower.Restrictions
{
    internal static class BossDespawnRestriction
    {
        private static readonly List<Vector3> bossesOkayToDestroyAndWaiting_spawnPositions = new();
        private static readonly List<Vector3> bossesReadyToDestroy_spawnPositions = new();
        private static readonly List<BaseAI> bossesPendingDestruction = new();

        // Cooldown until the boss will disappear
        private static readonly Dictionary<Vector3, float> despawnTimers = new();

        [HarmonyPatch(typeof(BaseAI), "UpdateAI")]
        public static class BaseAI_UpdateAI_Patch
        {
            [HarmonyPostfix]
            public static void Postfix(BaseAI __instance)
            {
                if (__instance == null) return;
                if (!ConfigurationFile.despawnEnabled.Value) return;
                if (SceneManager.GetActiveScene().name != "main" || ZNetScene.instance == null || ZNet.instance == null || !__instance) return;

                // Destroy in the server only
                if (!ZNet.instance.IsServer()) return;

                Vector3 spawnPoint = (Vector3)GameManager.GetPrivateValue(__instance, "m_spawnPoint");

                if (!IsBoss(__instance)) return;

                if (!HasZeroPlayersNearby(__instance))
                {
                    CancelDespawnTimer(spawnPoint);
                    return;
                }

                // If boss must disappear
                if (bossesReadyToDestroy_spawnPositions.Contains(spawnPoint))
                {
                    string str = Localization.instance.Localize(__instance.name);

                    Logger.Log($"Destroying {str}...");
                    DestroyBoss(__instance);

                    bossesReadyToDestroy_spawnPositions.Remove(spawnPoint);
                    bossesOkayToDestroyAndWaiting_spawnPositions.Remove(spawnPoint);
                    despawnTimers.Remove(spawnPoint);

                    return;
                }

                // Already counting down
                if (bossesOkayToDestroyAndWaiting_spawnPositions.Contains(spawnPoint))
                {
                    CheckDespawnTimer(spawnPoint);
                    return;
                }

                // New countdown
                bossesOkayToDestroyAndWaiting_spawnPositions.Add(spawnPoint);
                float delay = ConfigurationFile.despawnDelay.Value;
                Logger.Log($"Starting timer for {TimeSpan.FromSeconds(delay).TotalMilliseconds} milliseconds");
                if (delay <= 0f)
                    OnTimerElapsed(spawnPoint);
                else
                    despawnTimers[spawnPoint] = Time.time;
            }

            private static void CheckDespawnTimer(Vector3 spawnPoint)
            {
                if (!despawnTimers.TryGetValue(spawnPoint, out float startTime))
                    return;

                float elapsed = Time.time - startTime;
                Logger.Log($"Despawn timer: {elapsed:F1}/{ConfigurationFile.despawnDelay.Value:F1}s");
                if (elapsed >= ConfigurationFile.despawnDelay.Value)
                    OnTimerElapsed(spawnPoint);
            }

            private static void OnTimerElapsed(Vector3 spawnPoint)
            {
                Logger.Log("Timer elapsed");

                despawnTimers.Remove(spawnPoint);

                if (!bossesReadyToDestroy_spawnPositions.Contains(spawnPoint))
                    bossesReadyToDestroy_spawnPositions.Add(spawnPoint);
            }

            private static void CancelDespawnTimer(Vector3 spawnPoint)
            {
                bool removedWaiting = bossesOkayToDestroyAndWaiting_spawnPositions.Remove(spawnPoint);
                bool removedReady = bossesReadyToDestroy_spawnPositions.Remove(spawnPoint);
                bool removedTimer = despawnTimers.Remove(spawnPoint);

                if (removedWaiting || removedReady || removedTimer)
                    Logger.Log($"Cancelled despawn timer for boss at {spawnPoint}");
            }

            private static bool IsBoss(BaseAI monster)
            {
                if (!monster)
                    return false;

                Character character = (Character)GameManager.GetPrivateValue(monster, "m_character");
                return character != null && character.IsBoss();
            }

            private static bool HasZeroPlayersNearby(BaseAI monster)
            {
                if (!monster)
                    return false;

                var prefabName = monster.gameObject.name.Replace("(Clone)", "");
                if (ConfigurationFile.exceptionPrefabIdList.Value.Contains(prefabName)) return false;
                
                return !IsPlayerInRange(monster.transform.position, ConfigurationFile.radius.Value);
            }

            private static bool IsPlayerInRange(Vector3 position, float radius)
            {
                float radiusSqr = radius * radius;
                foreach (Player player in Player.GetAllPlayers())
                {
                    if (player == null)
                        continue;

                    Vector3 delta = player.transform.position - position;
                    if (delta.sqrMagnitude <= radiusSqr)
                        return true;
                }

                return false;
            }

            private static void DestroyBoss(BaseAI monster)
            {
                if (monster == null)
                    return;

                if (!bossesPendingDestruction.Contains(monster))
                {
                    bossesPendingDestruction.Add(monster);
                    Logger.Log($"Boss queued for destruction: {monster.name}");
                }
            }
        }
        
        [HarmonyPatch(typeof(MonoUpdaters), "FixedUpdate")]
        public static class MonoUpdaters_FixedUpdate_Patch
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                if (ZNet.instance == null || !ZNet.instance.IsServer()) return;
                if (ZNetScene.instance == null) return;
                if (bossesPendingDestruction.Count == 0) return;

                foreach (BaseAI monster in bossesPendingDestruction)
                {
                    if (monster == null) continue;
                    if (ZNetScene.instance == null)
                        break;

                    Logger.Log($"Destroying boss after AI update: {monster.name}");
                    ZNetScene.instance.Destroy(monster.gameObject);
                }
                bossesPendingDestruction.Clear();
            }
        }
    }
}