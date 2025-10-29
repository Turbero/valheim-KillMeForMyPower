using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace KillMeForMyPower.Restrictions.BossNameManagement
{
    public static class BossNameUtils
    {
        public static void GrantBossPowerToPlayer(BossNameEnum bossNameEnum, Player player)
        {
            if (bossNameEnum != BossNameEnum.None)
            {
                AddToConfigurationGrantedPlayersList(bossNameEnum, player);
                Logger.LogInfo($"Player {player.GetPlayerName()} defeated {bossNameEnum}.");
            }
        }

        public static bool IsBossPowerGrantedForPlayer(BossNameEnum bossNameEnum, Player player)
        {
            string playersListConfig = bossNameEnum.GetGrantedPlayerNamesList();
            return playersListConfig.Contains(player.GetPlayerName());
        }

        [MethodImpl(MethodImplOptions.Synchronized)] //one by one to update file, not all at once!
        public static void AddToConfigurationGrantedPlayersList(BossNameEnum bossNameEnum, Player player)
        {
            string playersListConfig = bossNameEnum.GetGrantedPlayerNamesList();
            if (playersListConfig.Contains(player.GetPlayerName())) return;

            List<string> list = new List<string>();
            string result;
                
            if (!string.IsNullOrEmpty(playersListConfig))
            {
                string[] playersList = playersListConfig.Split(',');
                list = new List<string>(playersList);
                list.Add(player.GetPlayerName());
                list.Sort();

                result = string.Join(",", list.ToArray());
            } else
                result = player.GetPlayerName();

            list.Add(player.GetPlayerName());
            list.Sort();

            //Save player (can't use reflection here)
            switch (bossNameEnum)
            {
                case BossNameEnum.Eikthyr:
                    ConfigurationFile.playerListForBoss1EikthyrPower.Value = result;
                    break;
                case BossNameEnum.TheElder:
                    ConfigurationFile.playerListForBoss2TheElderPower.Value = result;
                    break;
                case BossNameEnum.Bonemass:
                    ConfigurationFile.playerListForBoss3BonemassPower.Value = result;
                    break;
                case BossNameEnum.Moder:
                    ConfigurationFile.playerListForBoss4ModerPower.Value = result;
                    break;
                case BossNameEnum.Yagluth:
                    ConfigurationFile.playerListForBoss5YagluthPower.Value = result;
                    break;
                case BossNameEnum.Queen:
                    ConfigurationFile.playerListForBoss6QueenPower.Value = result;
                    break;
                case BossNameEnum.Fader:
                    ConfigurationFile.playerListForBoss7FaderPower.Value = result;
                    break;
                case BossNameEnum.SE_Boss_Gorr:
                    ConfigurationFile.playerListForBoss8TherzieGorrPower.Value = result;
                    break;
                case BossNameEnum.SE_Boss_Brutalis:
                    ConfigurationFile.playerListForBoss8TherzieBrutalisPower.Value = result;
                    break;
                case BossNameEnum.SE_Boss_StormHerald:
                    ConfigurationFile.playerListForBoss8StormHeraldPower.Value = result;
                    break;
                case BossNameEnum.SE_Boss_Sythrak:
                    ConfigurationFile.playerListForBoss8TherzieSythrakPower.Value = result;
                    break;
                default:
                    Logger.LogWarning("Not saved, boss not found!");
                    break;
            }
        }
    }
}