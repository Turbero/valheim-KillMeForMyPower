using BepInEx;
using HarmonyLib;
using KillMeForMyPower.Commands;

namespace KillMeForMyPower
{
    [BepInPlugin(GUID, NAME, VERSION)]
    public class KillMeForMyPower : BaseUnityPlugin
    {
        public const string GUID = "Turbero.KillMeForMyPower";
        public const string NAME = "Kill Me For My Power";
        public const string VERSION = "1.3.8";

        private readonly Harmony harmony = new Harmony(GUID);

        void Awake()
        {
            ConfigurationFile.LoadConfig(this);
            CheckBossesCommand.RegisterConsoleCommand();

            harmony.PatchAll();
        }

        void onDestroy()
        {
            harmony.UnpatchSelf();
        }
    }
}

