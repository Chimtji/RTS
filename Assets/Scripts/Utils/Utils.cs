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

    }
}