using System;
using System.Collections.Generic;
using UnityEngine;

namespace Trout.Utils
{
    public class Utils
    {
        public static void ClearChildren(Transform container)
        {
            int children = container.transform.childCount;
            for (int i = children - 1; i >= 0; i--)
            {
                GameObject.DestroyImmediate(container.transform.GetChild(i).gameObject);
            }
        }

        public static void RemoveObject(string name)
        {
            GameObject.DestroyImmediate(GameObject.Find("/MapGenerator/" + name));
        }

        public static void Log<T>(string text, T value)
        {
            Debug.Log(text + ": " + value);
        }

        public static float GetAvegerage(float[] values)
        {
            float avg = 0;
            foreach (float val in values)
            {
                avg += val;
            };

            return avg /= values.Length;
        }

    }
}