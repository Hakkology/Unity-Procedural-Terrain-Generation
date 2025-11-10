using UnityEngine;

static public class Utils
{
    public static float FractalBrownianMotion(float x, float y, int oct, float persistance)
    {
        float total = 0.0f;
        float frequency = 1.0f;
        float amplitude = 1.0f;
        float maxValue = 0.0f;

        for (int i = 0; i < oct; i++)
        {
            total += Mathf.PerlinNoise(x * frequency, y * frequency) * amplitude;
            maxValue += amplitude;
            amplitude *= persistance;
            frequency *= 2;
        }

        return total / maxValue;
    }

    public static float Map(float value, float originalMin, float originalMax, float targetMin, float targetMax) {

        return (value - originalMin) * (targetMax - targetMin) / (originalMax - originalMin) + targetMin;
    }
}