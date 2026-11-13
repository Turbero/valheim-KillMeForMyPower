using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace KillMeForMyPower.Managers
{

    public static class EpicMMOSystem_API
    {
        private static API_State state = API_State.NotReady;
        private static MethodInfo eGetLevel;
        private static MethodInfo eAddExp;
        private static MethodInfo eGetAttribute;
        private static MethodInfo eGetAttributeRusty;
        private static MethodInfo eSetSingleRate;


        private enum API_State
        {
            NotReady,
            NotInstalled,
            Ready,
        }

        public enum Attribut
        {
            Strength = 0,
            Agility = 1,
            Intellect = 2,
            Body = 3,
            Vigour = 4,
            Special = 5,
        }

        public static bool IsLoaded()
        {
            Init();
            return state == API_State.Ready;
        }

        public static int GetLevel()
        {
            int result = 0;
            Init();
            if (eGetLevel != null) result = (int)eGetLevel.Invoke(null, null);
            return result;
        }

        public static int GetAttribute(Attribut attribute)
        {
            int result = 0;
            Init();
            if (eGetAttribute != null) result = (int)eGetAttribute.Invoke(null, new object[] { attribute });
            return result;
        }

        public static int GetAttributeRusty(string attribute)
        {
            int result = 0;
            Init();
            if (eGetAttributeRusty != null) result = (int)eGetAttributeRusty.Invoke(null, new object[] { attribute });
            return result;
        }


        public static void AddExp(int value)
        {
            Init();
            eAddExp?.Invoke(null, new object[] { value });
        }

        public static void SetSingleRate(float rate)
        {
            Init();
            eSetSingleRate?.Invoke(null, new object[] { rate });
        }

        private static void Init()
        {
            if (state is API_State.Ready or API_State.NotInstalled) return;
            if (Type.GetType("EpicMMOSystem.EpicMMOSystem, EpicMMOSystem") == null)
            {
                state = API_State.NotInstalled;
                return;
            }

            state = API_State.Ready;

            Type actionsMO = Type.GetType("API.EMMOS_API, EpicMMOSystem");
            eGetLevel = actionsMO?.GetMethod("GetLevel", BindingFlags.Public | BindingFlags.Static);
            eAddExp = actionsMO?.GetMethod("AddExp", BindingFlags.Public | BindingFlags.Static);
            eGetAttribute = actionsMO?.GetMethod("GetAttribute", BindingFlags.Public | BindingFlags.Static);
            eGetAttributeRusty = actionsMO?.GetMethod("GetAttribute", BindingFlags.Public | BindingFlags.Static);
            eSetSingleRate = actionsMO?.GetMethod("SetSingleRate", BindingFlags.Public | BindingFlags.Static);
        }
        
        public static IDictionary GetMonsters()
        {
            Init();
            var type = Type.GetType("EpicMMOSystem.DataMonsters, EpicMMOSystem");
            if (type == null) return new Dictionary<string, object>();

            var field = type.GetField("dictionary", BindingFlags.Static | BindingFlags.NonPublic);
            return field?.GetValue(null) as IDictionary;
        }
        
        public static int GetMonsterLevel(object monster)
        {
            Init();
            var field = monster.GetType().GetField("level", BindingFlags.Public | BindingFlags.Instance);
            if (field != null)
            {
                return (int)field.GetValue(monster);
            }

            return 0;
        }

        public static bool HasEnoughLevelToSpawn(string bossPrefabName)
        {
            Init();
            try
            {
                object monsterData = GetMonsters()[bossPrefabName + "(Clone)"];
                if (monsterData != null)
                {
                    int monsterLevel = GetMonsterLevel(monsterData);
                    int maxLevelGap = ConfigurationFile.maxLevelGapBetweenBossAndPlayer.Value;
                    int minLevel = monsterLevel - maxLevelGap;
                    if (GetLevel() < minLevel)
                    {
                        Player.m_localPlayer.Message(MessageHud.MessageType.Center, ConfigurationFile.minLevelToSpawnBossNotMet.Value.Replace("{0}", minLevel.ToString()));
                        return false;
                    }

                    Logger.Log("Level enough to spawn.");
                }
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogWarning("Error when calculating monster level. Ignoring restriction. Error: "+ex);
                return true; //in case of exception or bad configuration we let it spawn to avoid stopping the game progression
            }
        }
    }

    /*//Don't Use
    public static class EMMOS_API
    {
        public static int GetLevel()
        {
            return LevelSystem.Instance.getLevel();
        }

        public static void AddExp(int exp)
        {
            LevelSystem.Instance.AddExp(exp);
        }

        public static void SetSingleRate(float rate)
        {
            LevelSystem.Instance.SetSingleRate(rate);
        }

        public static int GetAttribute(EpicMMOSystem_API.Attribut attribute)
        {
            var index = (int)attribute;
            return LevelSystem.Instance.getParameter((Parameter)index);
        }

        public static int GetAttributeRusty(string attribute)
        {
            if (!Enum.TryParse(attribute, true, out EpicMMOSystem_API.Attribut type)) return 0;
            var index = (int)type;
            return LevelSystem.Instance.getParameter((Parameter)index);
        }
    }*/
}