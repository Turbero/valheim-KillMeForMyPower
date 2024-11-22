﻿using BepInEx.Configuration;
using BepInEx;
using ServerSync;

namespace KillMeForMyPower
{
    internal class ConfigurationFile
    {
        private static ConfigEntry<bool> _serverConfigLocked = null;
        public static ConfigEntry<bool> debug;
        public static ConfigEntry<string> forbiddenMessage;
        public static ConfigEntry<int> daysBossEikthyr;
        public static ConfigEntry<int> daysBossElder;
        public static ConfigEntry<int> daysBossBonemass;
        public static ConfigEntry<int> daysBossModer;
        public static ConfigEntry<int> daysBossYagluth;
        public static ConfigEntry<int> daysBossQueen;
        public static ConfigEntry<int> daysBossFader;

        private static ConfigFile configFile;

        private static readonly ConfigSync ConfigSync = new ConfigSync(KillMeForMyPower.GUID)
        {
            DisplayName = KillMeForMyPower.NAME,
            CurrentVersion = KillMeForMyPower.VERSION,
            MinimumRequiredVersion = KillMeForMyPower.VERSION
        };

        internal static void LoadConfig(BaseUnityPlugin plugin)
        {
            {
                configFile = plugin.Config;

                _serverConfigLocked = config("1 - General", "Lock Configuration", true,
                "If on, the configuration is locked and can be changed by server admins only.");
                _ = ConfigSync.AddLockingConfigEntry(_serverConfigLocked);

                debug = config("1 - General", "DebugMode", false, "Enabling/Disabling the debugging in the console (default = false)", false);
                forbiddenMessage = config("2 - Config", "ForbiddenMessage", "Kill the forsaken first!", "Message to show when you cannot obtain the forsaken power");

                daysBossEikthyr  = config("3 - Days", "DaysBossEikthyr", 10000, "Minimum number of days until the Eikthyr power cannot be obtained without killing him (default = 10000)");
                daysBossElder    = config("3 - Days", "DaysBossElder", 10000, "Minimum number of days until the Elder power cannot be obtained without killing him (default = 10000)");
                daysBossBonemass = config("3 - Days", "DaysBossBonemass", 10000, "Minimum number of days until the Bonemass power cannot be obtained without killing him (default = 10000)");
                daysBossModer    = config("3 - Days", "DaysBossModer", 10000, "Minimum number of days until the Moder power cannot be obtained without killing him (default = 10000)");
                daysBossYagluth  = config("3 - Days", "DaysBossYagluth", 10000, "Minimum number of days until the Yagluth power cannot be obtained without killing him (default = 10000)");
                daysBossQueen    = config("3 - Days", "DaysBossQueen", 10000, "Minimum number of days until the Queen power cannot be obtained without killing him (default = 10000)");
                daysBossFader    = config("3 - Days", "DaysBossFader", 10000, "Minimum number of days until the Fader power cannot be obtained without killing him (default = 10000)");
            }
        }

        private static ConfigEntry<T> config<T>(string group, string name, T value, string description,
            bool synchronizedSetting = true)
        {
            return config(group, name, value, new ConfigDescription(description), synchronizedSetting);
        }

        private static ConfigEntry<T> config<T>(string group, string name, T value, ConfigDescription description,
            bool synchronizedSetting = true)
        {
            ConfigDescription extendedDescription =
                new ConfigDescription(
                    description.Description +
                    (synchronizedSetting ? " [Synced with Server]" : " [Not Synced with Server]"),
                    description.AcceptableValues, description.Tags);
            ConfigEntry<T> configEntry = configFile.Bind(group, name, value, extendedDescription);
            //var configEntry = Config.Bind(group, name, value, description);

            SyncedConfigEntry<T> syncedConfigEntry = ConfigSync.AddConfigEntry(configEntry);
            syncedConfigEntry.SynchronizedConfig = synchronizedSetting;

            return configEntry;
        }
    }
}
