# Genealogical Algorithmist

The core idea is this: instead of writing a single algorithm to solve a problem, you "breed" algorithms. You start with a few simple, often flawed, "progenitor" algorithms. You then define a fitness function (how well they solve a given task) and the program "mates" the most successful algorithms, combining their logical steps to create new "offspring" algorithms. Over many generations, the system evolves a highly effective, and potentially very unconventional, algorithm for the task.

This has had very little thought put into it for general-purpose problem-solving, as it borrows concepts from genetic programming but applies them in a more abstract, "logical-gene" way that can be visualized and understood by the programmer.

Here is an advanced C# program that implements the core engine for this concept.

I will provide a complete, runnable console application that demonstrates the concept by evolving an algorithm to sort a list of numbers.