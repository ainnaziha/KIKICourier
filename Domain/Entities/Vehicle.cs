namespace KIKICourier.Domain.Entities;

public class Vehicle
{
    public int Id { get; }
    public double MaxSpeedKmPerHour { get; }
    public double MaxCarriableWeightKg { get; }
    public double AvailableAtHours { get; set; }

    public Vehicle(int id, double maxSpeedKmPerHour, double maxCarriableWeightKg)
    {
        if (id <= 0)
            throw new ArgumentException("Vehicle ID must be positive", nameof(id));
        if (maxSpeedKmPerHour <= 0)
            throw new ArgumentException("Max speed must be positive", nameof(maxSpeedKmPerHour));
        if (maxCarriableWeightKg <= 0)
            throw new ArgumentException("Max carriable weight must be positive", nameof(maxCarriableWeightKg));

        Id = id;
        MaxSpeedKmPerHour = maxSpeedKmPerHour;
        MaxCarriableWeightKg = maxCarriableWeightKg;
        AvailableAtHours = 0;
    }
}
