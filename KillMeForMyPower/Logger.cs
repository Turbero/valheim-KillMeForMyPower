using BepInEx.Logging;

namespace KillMeForMyPower
{
    public static class Logger
    {
        public static ManualLogSource logger = BepInEx.Logging.Logger.CreateLogSource(KillMeForMyPower.NAME);
        internal static void Log(object s)
        {
            if (!ConfigurationFile.debug.Value)
            {
                return;
            }

            logger.LogInfo(s?.ToString());
        }

        internal static void LogInfo(object s)
        {
            logger.LogInfo(s?.ToString());
        }

        internal static void LogWarning(object s)
        {
            logger.LogWarning(s?.ToString());
        }

        internal static void LogError(object s)
        {
            logger.LogError(s?.ToString());
        }
    }
}
