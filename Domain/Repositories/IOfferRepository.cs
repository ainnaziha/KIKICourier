using KIKICourier.Domain.Entities;

namespace KIKICourier.Domain.Repositories;

public interface IOfferRepository
{
    Offer? GetByCode(string code);
    IEnumerable<Offer> GetAll();
}
