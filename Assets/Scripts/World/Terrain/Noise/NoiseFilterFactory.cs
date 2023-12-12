using System.Drawing;
using UnityEngine;

public static class NoiseFilterFactory
{
    public static INoiseFilter CreateNoiseFilter(int size, NoiseSettings settings, Vector2 position, ChunkPositionName edge)
    {
        switch (settings.filterType)
        {
            case NoiseSettings.FilterType.Simplex:
                return new SimplexNoiseFilter(size, settings, position, SimplexType.Normal);
            case NoiseSettings.FilterType.SimplexRigid:
                return new SimplexNoiseFilter(size, settings, position, SimplexType.Rigid);
            case NoiseSettings.FilterType.Circle:
                return new CircleNoiseFilter(size, settings, position, edge);
        }

        return null;
    }
}