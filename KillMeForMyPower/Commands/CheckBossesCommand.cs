using KillMeForMyPower.Restrictions;

namespace KillMeForMyPower.Commands
{
    public class CheckBossesCommand
    {
        public static void RegisterConsoleCommand()
        {
            new Terminal.ConsoleCommand("bosses_permissions", "", args =>
            {
                bool someKilled = false;
                args.Context.AddString("=== Bosses killed and allowed to use their powers ===");
                foreach (var uniqueKey in Player.m_localPlayer.GetUniqueKeys().FindAll(key => key.EndsWith("_Defeated_KMFMP")))
                {
                    args.Context.AddString($"- {Localization.instance.Localize(BossNameUtils.findBossNameByPrefabName(uniqueKey.Replace("_Defeated_KMFMP", "")).GetTranslationKey()) }");
                    someKilled = true;
                }
                if (!someKilled)
                    args.Context.AddString("NONE");
            });
        }
    }
}