namespace CoordinatesServiceWebApi.Helpers;

public static class RandomExtensions
{
    public static double NextDoubleInRange(this Random random, double minValue, double maxValue)
    {
        if (minValue > maxValue)
            throw new ArgumentOutOfRangeException(
                nameof(minValue), $"{nameof(minValue)} должно быть меньше или равно {nameof(maxValue)}.");

        double range = maxValue - minValue;
        double randomValue = minValue + random.NextDouble() * range;
        
        return randomValue;
    }
}