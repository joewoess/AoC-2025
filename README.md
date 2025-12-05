# AoC-2025

My solutions to the coding challenge [adventofcode](https://adventofcode.com/2025) written in whatever coding language my heart desired that day :)

All implemented solutions will be linked in the [Challenges table](##Challenges) with their respective languages.

---

## Challenges

| Day | Challenge                                                  |                    C#                     |   F#   | Typescript | Rust | Python |
| --: | :--------------------------------------------------------- | :---------------------------------------: | :----: | :--------: | :--: | :----: |
|   1 | [Secret Entrance](https://adventofcode.com/2025/day/1)     | [Csharp](src/aoc-csharp/puzzles/Day01.cs) | FSharp | Typescript | Rust | Python |
|   2 | [Gift Shop](https://adventofcode.com/2025/day/2)           | [Csharp](src/aoc-csharp/puzzles/Day02.cs) | FSharp | Typescript | Rust | Python |
|   3 | [Lobby](https://adventofcode.com/2025/day/3)               | [Csharp](src/aoc-csharp/puzzles/Day03.cs) | FSharp | Typescript | Rust | Python |
|   4 | [Printing Department](https://adventofcode.com/2025/day/4) | [Csharp](src/aoc-csharp/puzzles/Day04.cs) | FSharp | Typescript | Rust | Python |
|   5 | [Cafeteria](https://adventofcode.com/2025/day/5)           | [Csharp](src/aoc-csharp/puzzles/Day05.cs) | FSharp | Typescript | Rust | Python |
|   6 | [TBA](https://adventofcode.com/2025/day/6)                 |                  Csharp                   | FSharp | Typescript | Rust | Python |
|   7 | [TBA](https://adventofcode.com/2025/day/7)                 |                  Csharp                   | FSharp | Typescript | Rust | Python |
|   8 | [TBA](https://adventofcode.com/2025/day/8)                 |                  Csharp                   | FSharp | Typescript | Rust | Python |
|   9 | [TBA](https://adventofcode.com/2025/day/9)                 |                  Csharp                   | FSharp | Typescript | Rust | Python |
|  10 | [TBA](https://adventofcode.com/2025/day/10)                |                  Csharp                   | FSharp | Typescript | Rust | Python |
|  11 | [TBA](https://adventofcode.com/2025/day/11)                |                  Csharp                   | FSharp | Typescript | Rust | Python |
|  12 | [TBA](https://adventofcode.com/2025/day/12)                |                  Csharp                   | FSharp | Typescript | Rust | Python |

---

## Usage

Depending on the language version, all that is need is to go into the respective folder and
use their common build tool to run it.

```
# these are the valid optional parameters for all implementations (can be freely combined)
 --demo      ... Use test input instead of puzzle input
 --debug     ... Show debug output
 --last      ... Show last challenge commited
 01 4 20     ... Number list specifying certain days to output
```

```zsh
# using gradle for kotlin
# or for the included gradle wrapper use gradlew (or ./gradlew on windows)
gradle run --args='01'
gradlew run --args='01'

# using dotnet for c# and f#
dotnet run -- --demo

# using cargo for rust
cargo run -- 01

# using ts-node for typescript
ts-node filename.ts

# using python3 for python
python3 main.py --debug
```

## Languages used in this challenge

- C# 14.0 (.NET 10)
- F# 10.0 (.NET 10)

## Folder structure

```
+---data
|   +---demo
|       - day01.txt
|       - ...
|   +---real
|       - day01.txt
|       - ...
+---src
|   +---aoc-csharp
|   +---aoc-fsharp
+---dump
|   - AoC.sln
- README.md
- .gitignore
```

## Sample output

```log
------------------------------------------------------------------------------
AdventOfCode Runner for 2025
Challenge at: https://adventofcode.com/2025/
Author: Johannes Wöß
------------------------------------------------------------------------------
|  Day  |         1st |         2nd |
| Day01 |        1624 |        1653 |
Could not find solution for day Day02
```
