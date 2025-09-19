using BepInEx;
using HarmonyLib;

namespace KillMeForMyPower
{
    [BepInPlugin(GUID, NAME, VERSION)]
    public class KillMeForMyPower : BaseUnityPlugin
    {
        public const string GUID = "Turbero.KillMeForMyPower";
        public const string NAME = "Kill Me For My Power";
        public const string VERSION = "1.3.2";

        private readonly Harmony harmony = new Harmony(GUID);

        void Awake()
        {
            ConfigurationFile.LoadConfig(this);

            harmony.PatchAll();
        }

        void onDestroy()
        {
            harmony.UnpatchSelf();
        }
    }
}

