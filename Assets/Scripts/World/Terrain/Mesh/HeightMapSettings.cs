using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class HeightMapSettings
{
    public NoiseLayer[] ground;
    public NoiseLayer[] mountains;
    public NoiseLayer[] waters;
    public NoiseLayer[] startLocation;

    // public bool useFalloff = false;

    // public float heightMultiplier = 30;
    // public AnimationCurve heightCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    // public float minHeight
    // {
    //     get
    //     {
    //         return heightMultiplier * heightCurve.Evaluate(0);
    //     }
    // }
    // public float maxHeight
    // {
    //     get
    //     {
    //         return heightMultiplier * heightCurve.Evaluate(1);
    //     }
    // }
}
