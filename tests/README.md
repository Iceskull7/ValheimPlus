# Focused regressions

Run with .NET SDK 8:

```sh
dotnet run --project tests/RegressionTests/RegressionTests.csproj -c Release
```

This dependency-free console runner links the production animal selector and chest
distribution implementation. A failed assertion exits nonzero. It covers all original
adult/offspring pairs, moose/calves, disabled/None/All/combined/numeric selections,
exclusion of summoned moose, existing-stack preference, full/missing chests, partial
acceptance, synchronization callbacks, and conservation of quantities across 1,000
deterministic capacity scenarios.

These tests use in-memory inventories with the game's partial-acceptance contract.
They do **not** run Unity, Harmony patches, prefab loading, breeding, windmill recipes,
or multiplayer. Build the complete plugin against the supported game references as
well. See `docs/moose-oats-verification.md` for source evidence and game checks.
