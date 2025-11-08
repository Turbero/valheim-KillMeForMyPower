using System.Collections.Generic;
using System.Reflection;

namespace KillMeForMyPower.Managers
{
    public class GameManager
    {
        public static object GetPrivateValue(object obj, string name, BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.NonPublic)
        {
            return obj.GetType().GetField(name, bindingAttr)?.GetValue(obj);
        }
        
        public static object GetPrivateMethod(object obj, string name, BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.NonPublic)
        {
            return obj.GetType().GetMethod(name, bindingAttr)?.Invoke(obj, null);
        }
        
        public static bool isDetailedLevelsInstalled()
        {
            return BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("Turbero.DetailedLevels");
        }
        
        public static bool IsAdmin(Player player)
        {
            Logger.Log("[IsAdmin] Checking powerCommandsAdminPlayersList...");
            var playerName = player.GetPlayerName();
            bool fileAdmin = ConfigurationFile.powerCommandsAdminPlayersList.Value.Contains(playerName);
            if (fileAdmin) return true;

            Logger.Log("[IsAdmin] Finding All PlayerInfo...");
            List<ZNet.PlayerInfo> result = ZNet.instance.GetPlayerList().FindAll(p => p.m_name == playerName);
            if (result.Count == 0) return false;
            
            string steamID = result[0].m_userInfo.m_id.m_userID;
            Logger.Log($"[IsAdmin] Matching steamID {steamID} in adminList...");
            bool serverAdmin = 
                ZNet.instance != null &&
                ZNet.instance.GetAdminList() != null &&
                ZNet.instance.GetAdminList().Contains(steamID);
            return serverAdmin;
        }
    }
}