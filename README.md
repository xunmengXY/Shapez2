## Overview
Explore the world of Shapez2 with this third-party, programmable library that offers a customizable and flexible approach to the game's shape and fluid processing challenges. Designed to enhance your understanding and manipulation of Shapez2 mechanics.

## Customization Features
- **Layer Customization**: Set the maximum number of layers for shapes, with common configurations like normal scenario (4 layers), insane scenario (5 layers), it can also be set to other numbers of layers, such as 10 layers.
- **Mode Support**: The library supports both quarter and hexagon modes. The number of parts per layer and the machine's processing based on part count are decoupled, allowing for flexible configuration. While changing the `Shape.CurrentMode` property typically adjusts both, the library also provides `UnsafeXXX` methods for individual modification (see documentation for details) .
## Core Components
This library includes:
1. Shape and Piece Classes
2. Shape and Fluid Processing Functions
3. Shape Code Validity Check Functions

## Overloaded Features
- **Operator Overloading**: `==` and `!=` for content equality comparison rather than reference equality.
- **Implicit Conversions and ToString**: Between `Shape` and `string`, and overloading of `.ToString()` for `Shape`.
- **Extension Methods**: `.CW()` and `.CCW()` for `int` to simplify index rotation (see `Shapez2.cs`), and `.ToShape()` and `.ToPiece()` for `string`.

## Function Versions
Each function in this library typically comes in two versions:
1. **Static Version**: Inputs are passed as parameters, outputs are returned, and the inputs remain unchanged.
2. **Non-static Version**: Often modifies the calling instance or parameters (see documentation for details) , with or without a return value, but offers better performance than the static version.

## Usage
1. **Learning Shapez2 Shapes Processing**: The detailed implementation of Shapez2's shape processing may not be fully captured in the game's wiki, but this library provides an accurate description.
2. **Distinguishing Shape Feasibility and Mechanical Solutions**: Users can implement related algorithms based on this library.

## Notes
1. The current code is more suitable for learning than for high-performance scenarios.
2. Some illegal graphics are handled differently from the original game. For example, `Cu------` and `Cu------:--------` are treated as different in the original game, generating `Cucrcrcr` and `Cucrcrcr:crcrcrcr` respectively after processing by a crystal generator. In this library, they are considered the same shape `Cu------`. However, rest assured that `Cu------:--------` cannot be obtained in the original game without cheating.
