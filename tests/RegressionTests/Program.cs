using ValheimPlus.GameClasses;
using ValheimPlus.Utility;

int assertions = 0;
void Check(bool condition, string message)
{
    assertions++;
    if (!condition) throw new Exception(message);
}

var animals = new[]
{
    (ProcreationAnimalType.Boar, "$enemy_boar", "$enemy_boarpiggy", 1),
    (ProcreationAnimalType.Wolf, "$enemy_wolf", "$enemy_wolfcub", 2),
    (ProcreationAnimalType.Lox, "$enemy_lox", "$enemy_loxcalf", 4),
    (ProcreationAnimalType.Hen, "$enemy_hen", "$enemy_chicken", 8),
    (ProcreationAnimalType.Asksvin, "$enemy_asksvin", "$enemy_asksvin_hatchling", 16),
    (ProcreationAnimalType.Moose, "$enemy_moose", "$enemy_moosecalf", 32)
};
foreach (var (type, adult, offspring, numeric) in animals)
{
    Check((int)type == numeric, $"Stable config bit: {type}");
    foreach (var name in new[] { adult, offspring })
    {
        Check(ProcreationAnimals.IsSelected(true, type, name), $"Explicit selection: {name}");
        Check(ProcreationAnimals.IsSelected(true, ProcreationAnimalType.All, name), $"All: {name}");
        Check(!ProcreationAnimals.IsSelected(false, ProcreationAnimalType.All, name), $"Disabled: {name}");
        Check(!ProcreationAnimals.IsSelected(true, ProcreationAnimalType.None, name), $"None: {name}");
        foreach (var other in animals.Where(a => a.Item1 != type))
            Check(!ProcreationAnimals.IsSelected(true, other.Item1, name), $"Excluded by {other.Item1}: {name}");
        Check(ProcreationAnimals.IsSelected(true, (ProcreationAnimalType)31, name) == (type != ProcreationAnimalType.Moose),
            $"Old numeric All remains the original five animals: {name}");
    }
}
var combined = Enum.Parse<ProcreationAnimalType>("wolf, moose", true);
Check(ProcreationAnimals.IsSelected(true, combined, "$enemy_wolfcub"), "Named combination: wolf");
Check(ProcreationAnimals.IsSelected(true, combined, "$enemy_moosecalf"), "Named combination: moose");
Check(!ProcreationAnimals.IsSelected(true, combined, "$enemy_boar"), "Named combination excludes boar");
foreach (var unknown in new[] { "$spiritcaller_moose", "$enemy_deer", "Moose", "", null })
    Check(!ProcreationAnimals.IsSelected(true, ProcreationAnimalType.All, unknown), $"Unknown/non-breeding creature: {unknown}");

// Exercise the actual distribution code with inventories implementing AddItem's partial-acceptance
// contract. These are managed regression tests, not a simulation of Unity or network transport.
int Deposit(Chest[] chests, int amount) => ChestDeposit.Distribute(chests, amount,
    c => c.ContainsItem,
    (c, requested) =>
    {
        c.Attempts++;
        int accepted = Math.Min(c.Space, requested);
        c.Space -= accepted;
        c.Added += accepted;
        c.ContainsItem |= accepted > 0;
        return requested - accepted;
    },
    c => c.Syncs++);

Check(Deposit(Array.Empty<Chest>(), 20) == 20, "Missing chests: drop entire stack");
var full = new[] { new Chest(0, true), new Chest(0, false) };
Check(Deposit(full, 20) == 20, "Full chests: drop entire stack");
Check(full.All(c => c.Added == 0 && c.Syncs == 0 && c.Attempts == 1), "Full chests have no transfers");
var partial = new[] { new Chest(3, true), new Chest(4, true) };
Check(Deposit(partial, 20) == 13, "Partial deposit: only remainder drops");
Check(partial.Sum(c => c.Added) == 7 && partial.All(c => c.Syncs == 1), "Synchronize every partial transfer");
var existingFirst = new[] { new Chest(20, false), new Chest(8, true), new Chest(20, true) };
Check(Deposit(existingFirst, 10) == 0, "Complete deposit: suppress world drop");
Check(existingFirst[0].Added == 0 && existingFirst[1].Added == 8 && existingFirst[2].Added == 2,
    "Prefer existing stacks, preserving chest distance order");
var emptyFallback = new[] { new Chest(20, false), new Chest(3, true) };
Check(Deposit(emptyFallback, 10) == 0 && emptyFallback[0].Added == 7 && emptyFallback[1].Added == 3,
    "Continue into empty chest after partially filling existing stack");
var zero = new[] { new Chest(20, false) };
Check(Deposit(zero, 0) == 0 && zero[0].Attempts == 0, "No transfers for empty output");

var random = new Random(20);
for (int i = 0; i < 1000; ++i)
{
    int amount = random.Next(1, 201);
    var chests = Enumerable.Range(0, random.Next(0, 8))
        .Select(_ => new Chest(random.Next(0, 51), random.Next(2) == 1)).ToArray();
    int space = chests.Sum(c => c.Space);
    int remaining = Deposit(chests, amount);
    Check(remaining == Math.Max(0, amount - space), $"Available capacity used: case {i}");
    Check(chests.Sum(c => c.Added) + remaining == amount, $"Conservation of output: case {i}");
    Check(chests.All(c => c.Syncs == (c.Added > 0 ? 1 : 0) && c.Attempts <= 1), $"Sync once per changed chest: case {i}");
}
Console.WriteLine($"PASS: {assertions} assertions (animal selection and chest output distribution).");

sealed class Chest(int space, bool containsItem)
{
    public int Space = space;
    public bool ContainsItem = containsItem;
    public int Added;
    public int Attempts;
    public int Syncs;
}
