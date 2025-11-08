using System;
using HarmonyLib;

namespace KillMeForMyPower.Restrictions.BossNameManagement
{
    [HarmonyPatch(typeof(Game), "Start")]
    public class GameStartPatch {
        private static void Prefix() {
            ZRoutedRpc.instance.Register("RPC_BossPowerGrantServer", new Action<long, string, string>(RPC_BossPowerGrantCalls.RPC_BossPowerGrantServer));
            ZRoutedRpc.instance.Register("RPC_BossPowerRemoveGrantServer", new Action<long, string, string>(RPC_BossPowerGrantCalls.RPC_BossPowerRemoveGrantServer));
        }
    }
}