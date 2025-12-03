using KIKICourier.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace KIKICourier.Application
{
    public interface IDeliveryService
    {
        double ComputeDeliveryCost(double distanceKm, double weightKg, bool applyOffer);
    }


}
