namespace CoordinatesServiceWebApi.Domain;

public readonly struct Distance
{
    public const double MetersInMile = 1609.344;

    private readonly double _inMeters = 0.0d;

    public double InMeters => _inMeters;

    public double InMiles => _inMeters / MetersInMile;


    private Distance(double inMeters) => _inMeters = inMeters;

    public static Distance FromMeters(double inMeters) => new(inMeters);


    public static Distance operator +(Distance left, Distance right) =>
        FromMeters(left._inMeters + right._inMeters);
}