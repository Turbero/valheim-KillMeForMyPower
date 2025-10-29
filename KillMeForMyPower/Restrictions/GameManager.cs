using System.Reflection;

namespace KillMeForMyPower.Restrictions
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
    }
}