using System.Reflection;

namespace KillMeForMyPower.Restrictions
{
    public class GameManager
    {
        public static object GetPrivateValue(object obj, string name, BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.NonPublic)
        {
            return obj.GetType().GetField(name, bindingAttr).GetValue(obj);
        }
    }
}