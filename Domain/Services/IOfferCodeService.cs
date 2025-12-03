using System;
using System.Collections.Generic;
using System.Text;

namespace KIKICourier.Domain.Services
{
    public interface IOfferCodeService
    {
        double GetDiscount(double distanceKm, double weightKg);
    }

}
