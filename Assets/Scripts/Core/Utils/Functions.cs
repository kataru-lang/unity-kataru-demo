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
                default:
                    return Color.black;
            }
        }

        public static bool OutOfBounds(Vector2 pointerPos)
        {
            if (pointerPos.x < 0 || pointerPos.y < 0 || pointerPos.x > Screen.width || pointerPos.y > Screen.height) return true;
            return false;
        }
        public static bool Approximately(float a, float b, float threshold = 0.01f)
        {
            return ((a - b) < 0 ? ((a - b) * -1) : (a - b)) <= threshold;
        }

        public static bool Approximately(Vector2 a, Vector2 b, float threshold = 0.01f)
        {
            return Approximately(a.x, b.x, threshold) && Approximately(a.y, b.y, threshold);
        }

        public static Color ChangeColorA(Color c, float a)
        {
            c.a = a;
            return c;
        }

        public static Vector3 TopColliderPosition(Collider2D collider) => new Vector3(
            collider.gameObject.transform.position.x + collider.offset.x,
            collider.gameObject.transform.position.y + collider.offset.y + collider.bounds.extents.y
             + 0.5f,
            0);

        public static Vector3 BottomColliderPosition(Collider2D collider) => new Vector3(
        collider.gameObject.transform.position.x + collider.offset.x,
        collider.gameObject.transform.position.y + collider.offset.y - collider.bounds.extents.y
         + 1f,
        0);

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

        public static bool IsPlayerHurtbox(Collider2D other)
        {
            return other.transform.parent != null &&
                                        other.transform.parent.gameObject.CompareTag(Constants.PLAYER_TAG);
        }

        public static bool IsHurtbox(Collider2D other, LayerMask layerMask)
        {
            return other.transform.parent != null &&
                                        ((layerMask.value & (1 << other.transform.parent.gameObject.layer)) > 0);
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