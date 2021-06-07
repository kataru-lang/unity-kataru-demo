using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace JnA.Utils
{
    public class Functions
    {
        public static Color StringToColor(string color)
        {
            switch (color)
            {
                case "white":
                    return Color.white;
                case "blue":
                    return Color.blue;
                default:
                    return Color.black;
            }
        }

        public static bool Approximately(float a, float b, float threshold = 0.01f)
        {
            return ((a - b) < 0 ? ((a - b) * -1) : (a - b)) <= threshold;
        }

        public static Color ChangeColorA(Color c, float a)
        {
            c.a = a;
            return c;
        }

        public static Camera GetCamera(Cameras type)
        {
            switch (type)
            {
                case Cameras.UI:
                    return GameObject.FindGameObjectWithTag(Constants.UI_CAMERA_TAG).GetComponent<Camera>();
                case Cameras.Main:
                default:
                    return Camera.main;
            }
        }

        public static List<string> PublicStaticStrings(Type type)
        {
            var allStrings = new List<string>();
            var fields = type.GetFields(BindingFlags.Static | BindingFlags.Public);

            foreach (FieldInfo field in fields)
            {
                allStrings.Add(field.GetValue(null).ToString());
            }
            return allStrings;
        }
    }
}