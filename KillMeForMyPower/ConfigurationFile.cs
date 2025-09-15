using BepInEx.Configuration;
using BepInEx;
using KillMeForMyPower.Restrictions;
using ServerSync;

namespace KillMeForMyPower
{
    internal class ConfigurationFile
    {
        private static ConfigEntry<bool> _serverConfigLocked = null;
        public static ConfigEntry<bool> debug;
        public static ConfigEntry<string> forbiddenMessage;
        public static ConfigEntry<bool> vendorLocalRestrictions;
        public static ConfigEntry<BossNameEnum> vendorHaldorBossToKill;
        public static ConfigEntry<string> vendorHaldorRestrictions;
        public static ConfigEntry<BossNameEnum> vendorHildirBossToKill;
        public static ConfigEntry<string> vendorHildirRestrictions;
        public static ConfigEntry<BossNameEnum> vendorBogWitchBossToKill;
        public static ConfigEntry<string> vendorBogWitchRestrictions;
        public static ConfigEntry<string> forbiddenVendorMessage;
        public static ConfigEntry<bool> restrictUsingKeyItems;
        public static ConfigEntry<string> restrictUsingKeyItemsMessage;
        public static ConfigEntry<string> itemRestrictionAvailableTooltipMessage;
        public static ConfigEntry<string> itemRestrictionAvailableTooltipYes;
        public static ConfigEntry<string> itemRestrictionAvailableTooltipNo;
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
                vendorLocalRestrictions = config("2 - Config", "VendorLocalRestrictions", true, "Vendors allow buying items based on personal progress, not global (default = true)");
                vendorHaldorBossToKill = config("2 - Config", "VendorHaldorBossToKill", BossNameEnum.Eikthyr, "Boss to be killed before being able to talk to Haldor (default = Eikthyr). Set to 'None' to remove this restriction. Possible values: Eikthyr,TheElder,Bonemass,Moder,Yagluth,Queen,Fader,None");
                vendorHaldorRestrictions = config("2 - Config", "VendorHaldorRestrictions", "BeltStrength,Eikthyr;YmirRemains,TheElder;Thunderstone,TheElder;ChickenEgg,Yagluth", "Restricted items for Haldor split by comma and semicolon. Ex: BeltStrength,Eikthyr;YmirRemains,TheElder;Thunderstone,TheElder;ChickenEgg,Yagluth. Available boss names: Eikthyr,TheElder,Bonemass,Moder,Yagluth,Queen,Fader (empty = nothing to restrict)");
                vendorHildirBossToKill = config("2 - Config", "VendorHildirBossToKill", BossNameEnum.Eikthyr, "Boss to be killed before being able to talk to Haldor (default = Eikthyr). Set to 'None' to remove this restriction. Possible values: Eikthyr,TheElder,Bonemass,Moder,Yagluth,Queen,Fader,None");
                vendorHildirRestrictions = config("2 - Config", "VendorHildirRestrictions", "", "Restricted items for Hildir split by comma and semicolon. Ex: Ironpit,Moder. Available boss names: Eikthyr,TheElder,Bonemass,Moder,Yagluth,Queen,Fader (empty = nothing to restrict)");
                vendorBogWitchBossToKill = config("2 - Config", "VendorBogWitchBossToKill", BossNameEnum.TheElder, "Boss to be killed before being able to talk to Haldor (default = TheElder). Set to 'None' to remove this restriction. Possible values: Eikthyr,TheElder,Bonemass,Moder,Yagluth,Queen,Fader,None");
                vendorBogWitchRestrictions = config("2 - Config", "VendorBogWitchRestrictions", "", "Restricted items for BogWitch split by comma. Ex: MeadTrollPheromones,TheElder;SpicePlains,Moder;SpiceAshlands,Queen. Available boss names: Eikthyr,TheElder,Bonemass,Moder,Yagluth,Queen,Fader (empty = nothing to restrict)");
                forbiddenVendorMessage = config("2 - Config", "ForbiddenVendorMessage", "You have not killed {0} yet to buy my stuff!", "Message to show when you cannot buy from a NPC");
                restrictUsingKeyItems = config("2 - Config", "RestrictUsingKeyItems", true, "Restricts using crypt key, wishbone and wisplight until you kill the previous boss even if you get them from someone else. (default = true)");
                restrictUsingKeyItemsMessage = config("2 - Config", "RestrictUsingKeyItemsMessage", "You must kill {0} before doing that action!", "Message to show when you cannot equip or do an action with an important key item");
                itemRestrictionAvailableTooltipMessage = config("2 - Config", "ItemRestrictionAvailableTooltipMessage", "Available to use", "Message to show in item descriptions to know when you can start using them");
                itemRestrictionAvailableTooltipYes = config("2 - Config", "ItemRestrictionAvailableTooltipMessageYes", "YES", "Message to show in item descriptions confirming when you can use them");
                itemRestrictionAvailableTooltipNo = config("2 - Config", "ItemRestrictionAvailableTooltipMessageNo", "NO", "Message to show in item descriptions confirming when you cannot use them");
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
