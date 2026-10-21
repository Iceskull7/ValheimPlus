# Moose breeding and windmill oats — 0.10.3.1

Target: Valheim **1.0.20 (n-41)**, BepInExPack_Valheim **5.4.2351**
(BepInEx **5.4.23.5**). This fork retains the upstream plugin GUID.

## Evidence and implementation

The [Jotunn generated prefab and item documentation](https://github.com/Valheim-Modding/Jotunn/tree/18a0b624ceaf757abcfeb5cb1386d111a6dec9b2/data)
was inspected at commit `18a0b624ceaf757abcfeb5cb1386d111a6dec9b2` on `gh-pages`.
Its dataset identifies itself as **Valheim 1.0.7**, not 1.0.20. It establishes these
names and component types; it is not a substitute for testing current game assets:

| Prefab | Localization key | Relevant components / meaning |
| --- | --- | --- |
| `Moose` | `$enemy_moose` | Humanoid, MonsterAI, Tameable, Procreation |
| `Moose_calf` | `$enemy_moosecalf` | Humanoid, AnimalAI, Growup |
| `Moose_spiritcaller` | `$spiritcaller_moose` | No Procreation; excluded |
| `Oat` | `$item_oat` | Displayed as Oats; description says it can be ground into flour |
| `OatFlour` | `$item_oatflour` | Oat Flour, the flour product for oats |
| `OatSeeds` | `$item_oatseeds` | Distinct item; description says milling produces oats |

Compilation uses `Digitalroot.Valheim.Common.References` **1.0.20** and
`Digitalroot.References.Unity` **6000.0.75**. These are stripped reference
assemblies: they validate API compatibility, but contain no executable game logic
or prefab conversion values. The 1.0.20 `Smelter.ItemConversion` exposes `m_from`
and `m_to`; it has no per-recipe input/output count fields. The available executable
0.221.4 reference from `CoffeeNova.Valheim.ManagedReferences` **1.2214.6** was used
to inspect the longstanding `Inventory.AddItem` partial-acceptance contract and
the one-input-per-queued-entry smelter implementation. That older code is not an
in-game validation of 1.0.20.

### Breeding

`ProcreationAnimalType` preserves the five existing bit values and adds Moose at
bit 5. The separate enum leaves `Tameable.animalTypes` and taming rules unchanged.
Existing `All` includes moose; explicit lists and numeric `31` retain their existing
animals. Both adult and calf localization keys participate in the selector.

Existing patches then apply love-point requirements, creature limit, partner range,
pregnancy duration/chance, optional hunger/alerted overrides, and hover information.
The existing Growup patch selects the grown animal and applies the maturity modifier;
the calf selector also enables its growth hover information. Egg/Hen precedence is
unchanged. No additional fire, shield, tame-state, or other breeding checks are removed.
The selector returns false when the Procreation section is disabled.

### Windmill

Input automation was already recipe-driven: it walks eligible chests in distance order
and `Smelter.m_conversion` in game order, skips absent inputs, and fills only free queue
slots. One removed item results in one `RPC_AddOre` call, with its cheat flag. The
windmill has no fuel ingredient. This path is retained rather than adding duplicate
recipes or hardcoding a guessed prefab named `Oats`.

Output is resolved through `GetItemConversion(queuedInput).m_to`, so barley, oats,
and any other conversion provided by the loaded windmill use their own actual output.
The native `Spawn` stack quantity is preserved. The exact current 1.0.20 prefab table
and quantities were **not independently dumped** in this environment; no guessed
recipe or quantity is installed by the mod.

Windmill deposit now synchronizes partially changed chests even if `AddItem` reports
failure. It passes only the unaccepted remainder to the original `Smelter.Spawn`.
Existing stacks are tried first, then other chests, in distance order. Missing/full
chests retain the normal world drop. Other smelter deposit algorithms are unchanged.
The temporary item's initialization flag is restored in a `finally` block.

Input and output remain gated by the section and their individual `autoFuel` /
`autoDeposit` switches. Only the machine owner transfers items. Existing public-chest,
ward, loaded-inventory and range filtering, container synchronization, and the
one-second input throttle remain in place. No additional chest scan is introduced.

## Automated verification

`dotnet run --project tests/RegressionTests/RegressionTests.csproj -c Release`
executes production selector/distribution code: **3,143 assertions**, including
1,000 deterministic capacity cases. See `tests/README.md` for boundaries.

The full .NET Framework 4.8 Release build must also succeed with ServerSync and
INIFileParser merged. Release packaging must check the DLL version, merged types,
and byte equality against the committed source's Release build. Game/Unity reference
DLLs and user configuration files must not be included.

## Remaining game checks (not executed here)

Use a disposable world and matching server/client mod versions:

1. Dump the loaded windmill's `m_conversion` to confirm each `m_from` / `m_to` pair
   for `Barley`, `Oat`, and any `OatSeeds` conversion present in 1.0.20. Confirm input
   consumed, output produced, and queue size with automation off and on.
2. With `Procreation.enabled=false`, compare moose and all original animals with
   vanilla. Enable Moose only, then All, and check breeding, offspring timers and
   hover text. Confirm the calf's grown prefab and summoned moose exclusion.
3. Check love/limit/range/time multipliers and hunger/alerted options independently;
   check fire and shield interactions with those overrides disabled.
4. Test Barley-only, Oat-only and mixed chests, a full input queue, absent input,
   both automation switches independently disabled, and chests outside range/wards.
5. Test output to a nearly full stack with no empty slot, several partly available
   chests, completely full chests, and no chest. Count stored plus dropped items.
6. Repeat transfers on a dedicated server with two clients, move ownership between
   peers, and unload/reload the area. Confirm exactly-once transfers and persistence.

No Unity session, live breeding cycle, real windmill operation, or multiplayer test
has been executed in this cloud environment.
