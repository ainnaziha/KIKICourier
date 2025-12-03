using System;
using System.Collections.Generic;
using System.Text;

namespace KIKICourier.Domain.Entities
{
    public class DeliveryRequest
    {
        public double BaseRate { get; }
        public int NumberOfPackages { get; }
        public int PckgId { get; }
        public double WeightKg { get; }
        public double DistanceKm { get; }
        public bool ApplyOfferCode { get; }

        public DeliveryRequest(
        double baseRate,
        int numberOfPackages,
        int pckgId,
        double distanceKm,
        double weightKg,
        bool applyOfferCode)
        {
            if (baseRate <= 0) throw new ArgumentException("Base rate must be > 0");
            if (numberOfPackages <= 0) throw new ArgumentException("Number of packages must be > 0");
            if (pckgId <= 0) throw new ArgumentException("Package ID must be > 0");
            if (distanceKm <= 0) throw new ArgumentException("Distance must be > 0");
            if (weightKg <= 0) throw new ArgumentException("Weight must be > 0");

            BaseRate = baseRate;
            NumberOfPackages = numberOfPackages;
            PckgId = pckgId;
            DistanceKm = distanceKm;
            WeightKg = weightKg;
            ApplyOfferCode = applyOfferCode;
        }
    }
}
