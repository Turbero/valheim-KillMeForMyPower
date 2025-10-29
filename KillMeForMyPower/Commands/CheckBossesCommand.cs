using System;
using KillMeForMyPower.Restrictions.BossNameManagement;

namespace KillMeForMyPower.Commands
{
    public class CheckBossesCommand
    {
        public static void RegisterConsoleCommand()
        {
            new Terminal.ConsoleCommand("bosses_permissions", "", args =>
            {
                bool someKilled = false;
                string playerName = Player.m_localPlayer.GetPlayerName();
                args.Context.AddString("=== Bosses killed and allowed to use their powers ===");
                foreach (BossNameEnum enumValue in Enum.GetValues(typeof(BossNameEnum)))
                {
                    if (string.IsNullOrEmpty(enumValue.GetConfigurationListName()))
                        continue;
                    
                    string playersList = enumValue.GetGrantedPlayerNamesList();
                    if (playersList != null && playersList.Contains(playerName))
                    {
                        args.Context.AddString($"- {Localization.instance.Localize(enumValue.GetTranslationKey())}");
                        someKilled = true;
                    }
                }
                if (!someKilled)
                    args.Context.AddString("NONE");
            });
        }
    }
}