using KIKICourier.Domain.Services;
using KIKICourier.Domain.Services.Domain.Services;

namespace KIKICourier.Application
{
    public class DeliveryService : IDeliveryService
    {
        private readonly IDeliveryCost _deliveryCost;
        private readonly IOfferCodeService _offerService;

        public DeliveryService(
            IDeliveryCost deliveryCost,
            IOfferCodeService offerService)
        {
            _deliveryCost = deliveryCost;
            _offerService = offerService;
        }

        public double ComputeDeliveryCost(double distanceKm, double weightKg, bool applyOffer)
        {
            double baseCost = _deliveryCost.Calculate(distanceKm, weightKg);

            if (!applyOffer)
                return baseCost;

            double discount = _offerService.GetDiscount(distanceKm, weightKg);

            return baseCost - (baseCost * discount);
        }
    }
}
