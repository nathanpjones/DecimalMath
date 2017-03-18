# Decimal Math
*Portable math support for Decimal that Microsoft forgot and more.*

The .NET Decimal data type is included in .NET, but is often overlooked for scientific calculations. It's high precision and exact up to 28 decimal places and it's available in any .NET environment.

You might be in a situation where you just need a lot more precision than Double can provide. I used Decimal for calculating locations in space for CNC manufacturing and pick and place control. I've found the increased precision of Decimal reduces overall errors throughout the set of calculations and it also improves the odds of reversing the calculations if necessary for debugging. In that case it ends up being a kind of oversampling.

Unfortunately, a lot of the usual number functionality is not provided for .NET. For example, you can't calculate a square root or even perform [exponentiation](http://stackoverflow.com/questions/6425501/is-there-a-math-api-for-powdecimal-decimal). You can cast to Double for these operations, but you can end up with a significant loss of precision.

**Note** All of this I've used in the "real world", but this is also a hobby. Although the library is performant, I've perferred accuracy and readability to raw performance.

## Install

Install via [NuGet](https://www.nuget.org/packages/DecimalMath.DecimalEx) package manager console:
```
PM> Install-Package DecimalMath.DecimalEx
```

Or of course you can clone the repository and pull in the projects directly.

# Libraries

This project contains two portable libraries.

#### DecimalEx
Bridges the gap in .NET support.
- Decimal versions of the following functions:
  - `Sqrt`
  - `Pow`
  - `Exp`
  - `Log`
  - `Sin`, `Cos`, `Tan`
  - `ASin`, `ACos`, `ATan`, `ATan2`
- General Decimal functionality:
  - `Floor` / `Ceiling` to a given number of decimal places
  - Implementations of `GCF`, `AGMean`, and a fault-tolerant `Average`
  - Function to get decimal places of a number (looking at bits, NOT by converting to string)
  - Other minor helper functions
- Functionality for working with Decimal matrices as 2D arrays and a base class for implementing an NxN matrix for use in affine transformations.

#### Decimal2D
Provides support for high-accuracy geometric calculations. *Note: This is still a work in progress. It's been used in production, but it still needs some more grooming (was originally VB code) and unit tests.*
- Support for 2D geometric objects
  - `Point`, `Line`, and `Vector`
  - `Circle` and `Arc`
- Support for geometric relationships, e.g. tangent to, intersects, etc.
- A 2D tranformation matrix with a number of provided transforms.
- An implementation of an abstract right triangle (i.e., the sides and angles but without a location in space).
- Generic right triangle operations.

## License

This project uses the MIT License. See the license file in the same folder as this readme.
