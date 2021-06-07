// Declare all passages here.
using System.Collections.Generic;
using System.Linq;

namespace Kataru
{
    public static class Namespaces
    {
        public const string Global = "Global";
        public static List<string> All() => new List<string> {
          Global,
          "Room1",
          "Room2",
    };
    }
    public static class Characters
    {
        public const string None = "None",
        Think = "Think";

        public static List<string> AllInNamespace(string n)
        {
            System.Func<string, bool> predicate;
            if (n == Namespaces.Global)
            {
                predicate = r => !r.Contains(":");
            }
            else
            {
                predicate = r => r.StartsWith(n + ":");
            }
            var list = All().Where(predicate).ToList<string>();
            list.Insert(0, None);
            return list;
        }
        public static List<string> All()
        {
            var l = JnA.Utils.Functions.PublicStaticStrings(typeof(Characters));
            var l2 = new List<string>
            {
                "Slime",
              #region Room1
                "Room1:RedSlime",
              #endregion
              #region Room2
                 "Room2:BlueSlime",
                 "Room2:GreenSlime",
              #endregion
            };
            l.AddRange(l2);
            return l;
        }
    }

    public static class Passages
    {
        public const string None = "None";

        public static List<string> All() => new List<string> {
          #region Room1
            "Child:Start",
          #endregion
          #region Room2
            "Teen:BedroomDoor",
          #endregion
        };

        public static List<string> AllInNamespace(string n)
        {
            if (string.IsNullOrEmpty(n)) return new List<string> { None };
            System.Func<string, bool> predicate;
            if (n == Namespaces.Global)
            {
                predicate = r => !r.Contains(":");
            }
            else
            {
                predicate = r => r.StartsWith(n + ":");
            }
            var list = All().Where(predicate).ToList<string>();
            list.Insert(0, None);
            return list;
        }
    }
}
