# KIKI Courier

Console app for delivery cost and time estimates.

## Quick Start

Run from Visual Studio (F5) or command line:
```bash
dotnet run
```

Choose option 1 for cost only, option 2 for delivery time.

## Input Format

First line: base cost and package count
```
100 3
```

Next lines: package details (id, weight, distance, offer code)
```
PKG1 5 5 OFR001
PKG2 15 5 OFR002
PKG3 10 100 OFR003
```

For delivery time, also provide: vehicles, speed, max weight
```
2 70 200
```

## Offers

Discount applies only if package meets ALL criteria:

- **OFR001**: 10% off (0-200km, 70-200kg)
- **OFR002**: 7% off (50-150km, 100-250kg)
- **OFR003**: 5% off (50-250km, 10-150kg)

Use "NA" if no offer applies.

## How It Works

**Cost Formula:**
```
Base + (Weight × 10) + (Distance × 5) - Discount
```

**Delivery Time:**
- Groups packages to maximize vehicle capacity
- Prioritizes: more packages > heavier weight > shorter distance
- Vehicles return after delivering farthest package
- Time = Distance / Speed

## Development Notes

**Architecture layers:**
- Domain: core business logic (Package, Offer, calculators)
- Application: orchestrates domain services
- Infrastructure: handles data and I/O

**Key decisions:**
- Repository pattern for offers - easy to swap in-memory storage with database later
- Dependency injection through constructors - no tight coupling between services
- Each class has one job (SRP) - DeliveryCostCalculator only calculates costs, ShipmentOptimizer only finds optimal shipments
- Used interfaces so components can be swapped or mocked for testing

**Tricky parts:**
- Shipment optimization needed combinatorial algorithm to find best package groupings
- Delivery time calculation required tracking vehicle availability and using truncated times (not rounded) for vehicle return calculations
- Floating point precision issues - had to use `Math.Floor(value * 100) / 100` for display

## Testing

Run unit tests:
```bash
dotnet test
```

48 tests covering:
- Entity validation (Package, Offer)
- Service logic (calculators, optimizer, estimator)
- Integration scenarios (full delivery flow)

Tests helped catch the truncation vs rounding issue early.
