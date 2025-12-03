namespace KIKICourier.Domain.Services
{
    public class OfferCodeService : IOfferCodeService
    {
        public double GetDiscount(double distanceKm, double weightKg)
        {
            if (distanceKm < 50 && weightKg < 10)
                return 0.10;

            if (distanceKm >= 50 && distanceKm <= 200)
                return 0.15;
            if (weightKg > 20)
                return 0.20;

            return 0;
        }
    }
}

