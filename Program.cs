using KIKICourier.Application;
using KIKICourier.Domain.Services;
using KIKICourier.Infrastructure.CLI;

class Program
{
    static void Main()
    {
        var calculator = new DeliveryCost();

        IOfferCodeService offerCodeService = new OfferCodeService();

        IDeliveryService deliveryService = new DeliveryService(calculator, offerCodeService);

        var consoleApp = new ConsoleIO(deliveryService);

        consoleApp.Run();
    }
}
