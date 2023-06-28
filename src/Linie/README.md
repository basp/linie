# Linie
A ray tracing bootstrapper library.

## Rationale
This was originally all part of **Pixie** and that was fine. However, when trying to experiment with different ray tracing algorithms it quickly became apparent I was writing the same classes over and over again. Sharing the actual code wasn't the best option (since it was hard to share improvements) so the basic math related classes were moved into **Linie** (this package).

## Goal
The **Linie** package aims to provide a set of robust classes that are related to computer graphics and linear algebra. The main goal is to provide a solid base upon which to implement ray tracing and other gfx related algorithms. 