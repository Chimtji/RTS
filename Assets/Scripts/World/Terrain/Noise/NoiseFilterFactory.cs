using System.Drawing;
using UnityEngine;

public static class NoiseFilterFactory
{
    public static INoiseFilter CreateNoiseFilter(int size, NoiseSettings settings, Vector2 position)
    {
        switch (settings.filterType)
        {
            case NoiseSettings.FilterType.Simple:
                return new NoiseFilterSimple(size, settings, position);
            case NoiseSettings.FilterType.Rigid:
                return new NoiseFilterRigid(size, settings, position);
        }

        return null;
    }
}