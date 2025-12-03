using System;
using System.Collections.Generic;
using System.Text;

namespace KIKICourier.Domain.Services
{
    namespace Domain.Services
    {
        public interface IDeliveryCost
        {
            double Calculate(double distanceKm, double weightKg);
        }
    }
}
