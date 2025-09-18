using System.Reflection;

namespace KillMeForMyPower.Restrictions
{
    public class GameManager
    {
        public static object GetPrivateValue(object obj, string name, BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.NonPublic)
        {
            return obj.GetType().GetField(name, bindingAttr).GetValue(obj);
        }
        
        public static object GetPrivateMethod(object obj, string name, BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.NonPublic)
        {
            return obj.GetType().GetMethod(name, bindingAttr).Invoke(obj, null);
        }

        public static void updateKeyToKMFMPKey(BossNameEnum bossNameEnum, Player player)
        {
            string oldKey = bossNameEnum.GetOldKey();
            if (player.HaveUniqueKey(oldKey))
            {
                player.RemoveUniqueKey(oldKey);
            }
            player.AddUniqueKey(bossNameEnum.GetUniqueKey());
            Logger.LogInfo($"player {player.GetPlayerName()} defeated {bossNameEnum}. Added personal KMFMP key!");
        }
    }
}