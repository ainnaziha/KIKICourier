# KIKI Courier Delivery Service

A console application for calculating delivery costs and estimating delivery times.

## Input Format

**Base delivery cost and packages:**
```
100 3
```

**Package details (one per line):**
```
PKG1 5 5 OFR001
PKG2 15 5 OFR002
PKG3 10 100 OFR003
```

**Vehicle details (if calculating delivery time):**
```
2 70 200
```

## Example

```
Enter base delivery cost and number of packages: 100 3

Enter package details:
Package 1: PKG1 5 5 OFR001
Package 2: PKG2 15 5 OFR002
Package 3: PKG3 10 100 OFR003

Results:
PKG1 0 175
PKG2 0 275
PKG3 35 665
```

## Available Offers

| Code | Discount | Distance (km) | Weight (kg) |
|------|----------|---------------|-------------|
| OFR001 | 10% | 0-200 | 70-200 |
| OFR002 | 7% | 50-150 | 100-250 |
| OFR003 | 5% | 50-250 | 10-150 |

## Cost Calculation

```
Total Cost = Base Cost + (Weight × 10) + (Distance × 5) - Discount
```
