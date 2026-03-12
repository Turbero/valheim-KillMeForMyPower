using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace KillMeForMyPower.Restrictions
{
    [HarmonyPatch(typeof(CharacterDrop), "GenerateDropList")]
    public class BossDropsRules
    {
        public static void Postfix(ref Character ___m_character, ref List<KeyValuePair<GameObject, int>> __result)
        {
            if (!ConfigurationFile.dropsBossTrophies.Value && !ConfigurationFile.dropsBossItems.Value) return;
            if (!___m_character.IsBoss()) return;
            int countPlayersNearby = GetCountPlayersNearby(___m_character);
            
            Logger.Log("result distinct drops count: "+__result.Count);
            Logger.Log("result total items count: "+__result.Select(elem => elem.Value).Sum());
            Logger.Log("countPlayersNearby: "+countPlayersNearby);
            
            List<KeyValuePair<GameObject, int>> newResult = new List<KeyValuePair<GameObject, int>>();
            foreach (KeyValuePair<GameObject, int> current in __result)
            {
                GameObject dropGo = current.Key;
                int amount = current.Value;
                
                string name = dropGo.name.ToLowerInvariant();
                Logger.Log($"Previous amount for {name}: {amount}");
                // Items
                if (ConfigurationFile.dropsBossItems.Value)
                {
                    if (name.ToLowerInvariant().Contains("hardantler") || name.ToLowerInvariant().Contains("yagluthdrop"))
                        amount = 3 * countPlayersNearby;
                    else if (name.ToLowerInvariant().Contains("cryptkey") || name.ToLowerInvariant().Contains("wishbone"))
                        amount = countPlayersNearby;
                    else if (name.ToLowerInvariant().Contains("dragontear")) 
                        amount = 10 * countPlayersNearby;
                    else if (name.ToLowerInvariant().Contains("queendrop") || name.ToLowerInvariant().Contains("faderdrop"))
                        amount = 5 * countPlayersNearby;
                }
                //Trophies
                if (ConfigurationFile.dropsBossTrophies.Value && name.ToLowerInvariant().Contains("trophy"))
                    amount = countPlayersNearby;
                
                Logger.Log($"New amount for {name}: {amount}");
                newResult.Add(new KeyValuePair<GameObject, int>(dropGo, amount));
            }
            __result = newResult;
            Logger.Log("__result new total items count: "+__result.Select(elem => elem.Value).Sum());
        }
        
        private static int GetCountPlayersNearby(Character boss)
        {
            BaseAI ai = boss.GetComponent<BaseAI>();
            float aggroRange = ai ? ai.m_viewRange : 0f;
            Logger.Log($"Detection boss range for {boss.name.Replace("(Clone)", "")} is {aggroRange} meters");
            
            List<Player> playersNearby = new List<Player>();
            Vector3 playerPosition = Player.m_localPlayer.transform.position;
            Player.GetPlayersInRange(playerPosition, aggroRange, playersNearby);
            return playersNearby.Count;
        }
    }
}