using KIKICourier.Domain.Services.Domain.Services;

namespace KIKICourier.Domain.Services
{
    public class DeliveryCost : IDeliveryCost
    {
        public double Calculate(double distanceKm, double weightKg)
        {
            double baseRate = 5;
            double distanceRate = 2; 

            return (weightKg * baseRate) + (distanceKm * distanceRate);
        }
    }
}
