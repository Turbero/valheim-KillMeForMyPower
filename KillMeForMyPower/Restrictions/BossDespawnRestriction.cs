using System;
using System.Collections.Generic;
using HarmonyLib;
using KillMeForMyPower.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace KillMeForMyPower.Restrictions
{
    [HarmonyPatch]
	internal static class BossDespawnRestrictionPath
	{
		private static readonly List<Vector3> bossesOkayToDestroyAndWaiting_spawnPositions = new();
		private static readonly List<Vector3> bossesReadyToDestroy_spawnPositions = new();

		[HarmonyPatch(typeof(Humanoid), "CustomFixedUpdate")]
		public static void Postfix(Humanoid __instance)
		{
			if (!ConfigurationFile.despawnEnabled.Value) return;
			
			if (SceneManager.GetActiveScene().name != "main" ||
			    ZNetScene.instance == null ||
			    ZNet.instance == null ||
			    !__instance)
				return;
			
			string bossName = Localization.instance.Localize(__instance.m_name);
			
			BaseAI baseAI = __instance.GetBaseAI();
			if (baseAI == null) return;
			
			Vector3 spawnPoint = (Vector3)GameManager.GetPrivateValue(baseAI, "m_spawnPoint");
			
			if (!IsOkayToDestroy(__instance))
			{
				if (bossesOkayToDestroyAndWaiting_spawnPositions.Contains(spawnPoint))
				{
					bossesOkayToDestroyAndWaiting_spawnPositions.Remove(spawnPoint);
				}
			}
			else if (bossesReadyToDestroy_spawnPositions.Contains(spawnPoint))
			{
				Logger.LogInfo("Despawning " + bossName + "...");
				ZNetScene.instance.Destroy(__instance.gameObject);
				Player.m_localPlayer.Message(MessageHud.MessageType.TopLeft, "Despawning " + bossName + "...");
				bossesReadyToDestroy_spawnPositions.Remove(spawnPoint);
				bossesOkayToDestroyAndWaiting_spawnPositions.Remove(spawnPoint);
			}
			else
			{
				if (bossesOkayToDestroyAndWaiting_spawnPositions.Contains(spawnPoint))
					return;
				
				bossesOkayToDestroyAndWaiting_spawnPositions.Add(spawnPoint);
				bossesReadyToDestroy_spawnPositions.Add(spawnPoint); //TODO Remove this line when despawnDelay works again
				/*var totalMilliseconds = TimeSpan.FromMinutes(ConfigurationFile.despawnDelay.Value).TotalMilliseconds;
				
				if (totalMilliseconds > 0.0)
				{
					var timer = new System.Timers.Timer((float)totalMilliseconds);
					timer.Elapsed += delegate
					{
						OnTimerElapsed(timer, spawnPoint);
					};
					Logger.LogInfo($"Starting despawning timer for {bossName} in {ConfigurationFile.despawnDelay.Value} seconds...");
					timer.Start();
				}
				else
				{
					OnTimerElapsed(null, spawnPoint);
					Logger.LogInfo($"Despawning timer stopped for {bossName}");
				}*/
			}
		}

		/*private static void OnTimerElapsed(System.Timers.Timer timer, Vector3 spawnPoint)
		{
			bossesReadyToDestroy_spawnPositions.Add(spawnPoint);
			if (timer == null) return;
			timer.Stop();
			timer.Dispose();
		}*/

		private static bool IsOkayToDestroy(Humanoid humanoid)
		{
			if (humanoid == null)
				return false;
			
			var prefabName = humanoid.name.Replace("(Clone)", "");
			var arePlayerNearby = Player.IsPlayerInRange(humanoid.transform.position, ConfigurationFile.radius.Value);
			var isInExceptionList = ConfigurationFile.exceptionPrefabIdList.Value.Contains(prefabName);
			bool isOk = humanoid.IsBoss() && !arePlayerNearby && !isInExceptionList;
			if (isOk)
				Logger.Log("humanoid.name: " + humanoid.name + " to be destroyed.");
			return humanoid.IsBoss() && !arePlayerNearby && !isInExceptionList;
		}
	}
}