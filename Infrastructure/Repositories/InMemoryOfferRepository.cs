using KIKICourier.Domain.Entities;
using KIKICourier.Domain.Repositories;

namespace KIKICourier.Infrastructure.Repositories;

public class InMemoryOfferRepository : IOfferRepository
{
    private readonly Dictionary<string, Offer> _offers;

    public InMemoryOfferRepository()
    {
        _offers = new Dictionary<string, Offer>(StringComparer.OrdinalIgnoreCase)
        {
            {
                "OFR001",
                new Offer(
                    code: "OFR001",
                    discountPercentage: 10,
                    minDistanceKm: 0,
                    maxDistanceKm: 200,
                    minWeightKg: 70,
                    maxWeightKg: 200
                )
            },
            {
                "OFR002",
                new Offer(
                    code: "OFR002",
                    discountPercentage: 7,
                    minDistanceKm: 50,
                    maxDistanceKm: 150,
                    minWeightKg: 100,
                    maxWeightKg: 250
                )
            },
            {
                "OFR003",
                new Offer(
                    code: "OFR003",
                    discountPercentage: 5,
                    minDistanceKm: 50,
                    maxDistanceKm: 250,
                    minWeightKg: 10,
                    maxWeightKg: 150
                )
            }
        };
    }

    public Offer? GetByCode(string code)
    {
        _offers.TryGetValue(code, out var offer);
        return offer;
    }

    public IEnumerable<Offer> GetAll()
    {
        return _offers.Values;
    }
}
