using BepInEx.Configuration;
using BepInEx;
using KillMeForMyPower.Restrictions.BossNameManagement;
using ServerSync;

namespace KillMeForMyPower
{
    internal class ConfigurationFile
    {
        private static ConfigEntry<bool> _serverConfigLocked;
        public static ConfigEntry<bool> debug;
        public static ConfigEntry<bool> activateMidPlayDetection;
        public static ConfigEntry<string> powerCommandsAdminPlayersList;
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
        public static ConfigEntry<bool> grantKillToNearbyPlayers;
        public static ConfigEntry<int> daysBossEikthyr;
        public static ConfigEntry<int> daysBossElder;
        public static ConfigEntry<int> daysBossBonemass;
        public static ConfigEntry<int> daysBossModer;
        public static ConfigEntry<int> daysBossYagluth;
        public static ConfigEntry<int> daysBossQueen;
        public static ConfigEntry<int> daysBossFader;
        public static ConfigEntry<int> daysBossModded;
        public static ConfigEntry<float> maxLevelBeforeBoss1Eikthyr;
        public static ConfigEntry<float> maxLevelBeforeBoss2TheElder;
        public static ConfigEntry<float> maxLevelBeforeBoss3Bonemass;
        public static ConfigEntry<float> maxLevelBeforeBoss4Moder;
        public static ConfigEntry<float> maxLevelBeforeBoss5Yagluth;
        public static ConfigEntry<float> maxLevelBeforeBoss6Queen;
        public static ConfigEntry<float> maxLevelBeforeBoss7Fader;
        public static ConfigEntry<string> playerListForBoss1EikthyrPower;
        public static ConfigEntry<string> playerListForBoss2TheElderPower;
        public static ConfigEntry<string> playerListForBoss3BonemassPower;
        public static ConfigEntry<string> playerListForBoss4ModerPower;
        public static ConfigEntry<string> playerListForBoss5YagluthPower;
        public static ConfigEntry<string> playerListForBoss6QueenPower;
        public static ConfigEntry<string> playerListForBoss7FaderPower;
        public static ConfigEntry<string> playerListForBoss8TherzieGorrPower;
        public static ConfigEntry<string> playerListForBoss8TherzieBrutalisPower;
        public static ConfigEntry<string> playerListForBoss8TherzieStormHeraldPower;
        public static ConfigEntry<string> playerListForBoss8TherzieSythrakPower;

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

                _serverConfigLocked = config("1 - General", "Lock Configuration", true, "If on, the configuration is locked and can be changed by server admins only.");
                _ = ConfigSync.AddLockingConfigEntry(_serverConfigLocked);

                debug = config("1 - General", "DebugMode", false, "Enabling/Disabling the debugging in the console (default = false)", false);
                activateMidPlayDetection = config("1 - General", "ActivateMidPlayDetection", true, "Adds boss power detection to identify if the player had used the power before installing the mod in a mid-play (default = false)");
                powerCommandsAdminPlayersList = config("1 - General", "Power Commands Admin Players List", "", "List of additional player names that can help admin servers to assign powers to other players or themselves using the mod commands.");
                
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
                grantKillToNearbyPlayers = config("2 - Config", "GrantKillToNearbyPlayers", true, "Allows nearby players to grant the boss kill (default = true)");
                
                daysBossEikthyr  = config("3 - Days", "DaysBossEikthyr", 10000, "Minimum number of days until the Eikthyr power cannot be obtained without killing him (default = 10000)");
                daysBossElder    = config("3 - Days", "DaysBossElder", 10000, "Minimum number of days until the Elder power cannot be obtained without killing him (default = 10000)");
                daysBossBonemass = config("3 - Days", "DaysBossBonemass", 10000, "Minimum number of days until the Bonemass power cannot be obtained without killing him (default = 10000)");
                daysBossModer    = config("3 - Days", "DaysBossModer", 10000, "Minimum number of days until the Moder power cannot be obtained without killing him (default = 10000)");
                daysBossYagluth  = config("3 - Days", "DaysBossYagluth", 10000, "Minimum number of days until the Yagluth power cannot be obtained without killing him (default = 10000)");
                daysBossQueen    = config("3 - Days", "DaysBossQueen", 10000, "Minimum number of days until the Queen power cannot be obtained without killing him (default = 10000)");
                daysBossFader    = config("3 - Days", "DaysBossFader", 10000, "Minimum number of days until the Fader power cannot be obtained without killing him (default = 10000)");
                daysBossModded   = config("3 - Days", "DaysBossModded", 10000, "Minimum number of days until any modded boss power cannot be obtained without killing him (default = 10000)");
                
                maxLevelBeforeBoss1Eikthyr  = config("4 - Max levels", "MaxLevelBeforeBoss1Eikthyr", 100f, "Maximum skill level that player can level up skills before killing Eikthyr (default = 100)");
                maxLevelBeforeBoss2TheElder = config("4 - Max levels", "MaxLevelBeforeBoss2TheElder", 100f, "Maximum skill level that player can level up skills before killing The Elder (default = 100)");
                maxLevelBeforeBoss3Bonemass = config("4 - Max levels", "MaxLevelBeforeBoss3Bonemass", 100f, "Maximum skill level that player can level up skills before killing Bonemass (default = 100)");
                maxLevelBeforeBoss4Moder    = config("4 - Max levels", "MaxLevelBeforeBoss4Moder", 100f, "Maximum skill level that player can level up skills before killing Moder (default = 100)");
                maxLevelBeforeBoss5Yagluth  = config("4 - Max levels", "MaxLevelBeforeBoss5Yagluth", 100f, "Maximum skill level that player can level up skills before killing Yagluth (default = 100)");
                maxLevelBeforeBoss6Queen    = config("4 - Max levels", "MaxLevelBeforeBoss6Queen", 100f, "Maximum skill level that player can level up skills before killing The Queen (default = 100)");
                maxLevelBeforeBoss7Fader    = config("4 - Max levels", "MaxLevelBeforeBoss7Fader", 100f, "Maximum skill level that player can level up skills before killing Fader (default = 100)");
                
                playerListForBoss1EikthyrPower = config("5 - Power Granted player lists", "Player List for Boss 1 - Eikthyr power", "", "List of player names that can use Eikthyr power after defeating him");
                playerListForBoss2TheElderPower = config("5 - Power Granted player lists", "Player List for Boss 2 - The Elder power", "", "List of player names that can use The Elder power after defeating him");
                playerListForBoss3BonemassPower = config("5 - Power Granted player lists", "Player List for Boss 3 - Bonemass power", "", "List of player names that can use Bonemass power after defeating him");
                playerListForBoss4ModerPower = config("5 - Power Granted player lists", "Player List for Boss 4 - Moder power", "", "List of player names that can use Moder power after defeating her");
                playerListForBoss5YagluthPower = config("5 - Power Granted player lists", "Player List for Boss 5 - Yagluth power", "", "List of player names that can use Yagluth power after defeating him");
                playerListForBoss6QueenPower = config("5 - Power Granted player lists", "Player List for Boss 6 - Queen power", "", "List of player names that can use Queen power after defeating her");
                playerListForBoss7FaderPower = config("5 - Power Granted player lists", "Player List for Boss 7 - Fader power", "", "List of player names that can use Fader power after defeating him");
                playerListForBoss8TherzieGorrPower = config("5 - Power Granted player lists", "Player List for Therzie Boss - Gorr power", "", "List of player names that can use Gorr power after defeating him");
                playerListForBoss8TherzieBrutalisPower = config("5 - Power Granted player lists", "Player List for Therzie Boss - Brutalis power", "", "List of player names that can use Brutalis power after defeating him");
                playerListForBoss8TherzieStormHeraldPower = config("5 - Power Granted player lists", "Player List for Therzie Boss - StormHerald power", "", "List of player names that can use StormHerald power after defeating him");
                playerListForBoss8TherzieSythrakPower = config("5 - Power Granted player lists", "Player List for Therzie Boss - Sythrak power", "", "List of player names that can use Sythrak power after defeating him");
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
