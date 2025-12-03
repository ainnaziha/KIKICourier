using KIKICourier.Application;
using KIKICourier.Domain.Repositories;
using KIKICourier.Domain.Services;
using KIKICourier.Infrastructure.CLI;
using KIKICourier.Infrastructure.Repositories;

class Program
{
    static void Main()
    {
        IOfferRepository offerRepository = new InMemoryOfferRepository();

        IDeliveryCostCalculator costCalculator = new DeliveryCostCalculator(offerRepository);

        IShipmentOptimizer shipmentOptimizer = new ShipmentOptimizer();

        IDeliveryTimeEstimator timeEstimator = new DeliveryTimeEstimator(shipmentOptimizer);

        IDeliveryService deliveryService = new DeliveryService(costCalculator, timeEstimator);

        var consoleApp = new ConsoleIO(deliveryService);

        consoleApp.Run();
    }
}
