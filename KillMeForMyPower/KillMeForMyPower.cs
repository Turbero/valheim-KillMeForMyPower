using BepInEx;
using HarmonyLib;
using KillMeForMyPower.Commands;
using UnityEngine;

namespace KillMeForMyPower
{
    [BepInPlugin(GUID, NAME, VERSION)]
    public class KillMeForMyPower : BaseUnityPlugin
    {
        public const string GUID = "Turbero.KillMeForMyPower";
        public const string NAME = "Kill Me For My Power";
        public const string VERSION = "2.2.1";

        private readonly Harmony harmony = new Harmony(GUID);

        void Awake()
        {
            ConfigurationFile.LoadConfig(this);
            harmony.PatchAll();
        }

        private void Start()
        {
            StartCoroutine(WaitForNetworking());
        }

        private System.Collections.IEnumerator WaitForNetworking()
        {
            // Wait until full networking initialization
            while (ZRoutedRpc.instance == null || ZNet.instance == null)
                yield return new WaitForSeconds(1f);
            
            // Commands registration
            PowerManagementCommands.RegisterConsoleCommand();
            CheckBossesCommand.RegisterConsoleCommand();
            
            Logger.LogInfo($"Console commands registered successfully. IsServer: {ZNet.instance.IsServer().ToString().ToUpperInvariant()}");
        }

        void onDestroy()
        {
            harmony.UnpatchSelf();
        }
    }
}

