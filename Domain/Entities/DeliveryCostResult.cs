namespace KIKICourier.Domain.Entities;

public class DeliveryCostResult
{
    public string PackageId { get; }
    public double Discount { get; }
    public double TotalCost { get; }
    public double? EstimatedDeliveryTimeHours { get; set; }

    public DeliveryCostResult(string packageId, double discount, double totalCost)
    {
        PackageId = packageId;
        Discount = discount;
        TotalCost = totalCost;
    }
}
