using BepInEx.Configuration;
using BepInEx;

namespace KillMeForMyPower
{
    internal class ConfigurationFile
    {
        public static ConfigEntry<bool> debug;
        public static ConfigEntry<string> forbiddenMessage;
        //public static ConfigEntry<float> damageTick;

        private static ConfigFile config;

        internal static void LoadConfig(BaseUnityPlugin plugin)
        {
            {
                config = plugin.Config;

                debug = config.Bind<bool>("1 - General", "DebugMode", false, "Enabling/Disabling the debugging in the console (default = false)");
                forbiddenMessage = config.Bind<string>("2 - Config", "ForbiddenMessage", "Kill the forsaken first!", "Message to show when you cannot obtain the forsaken power");
                //damageTick = config.Bind<float>("2 - Config", "DamageTick", 1f, "Damage per tick received by forbidden power fire (default = 1/tick)");
            }
        }
    }
}
