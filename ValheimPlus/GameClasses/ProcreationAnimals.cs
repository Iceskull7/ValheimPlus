using System;
using System.Collections.Generic;

namespace ValheimPlus.GameClasses
{
    // Keep the original animalTypes bit values so existing named and numeric settings still work.
    // This selection is separate from Tameable: enabling Moose breeding must not change taming rules.
    [Flags]
    public enum ProcreationAnimalType
    {
        None = 0,
        Boar = 1 << 0,
        Wolf = 1 << 1,
        Lox = 1 << 2,
        Hen = 1 << 3,
        Asksvin = 1 << 4,
        Moose = 1 << 5,
        All = (1 << 6) - 1
    }

    public static class ProcreationAnimals
    {
        private static readonly Dictionary<string, ProcreationAnimalType> NamedTypes = new()
        {
            { "$enemy_boar", ProcreationAnimalType.Boar },
            { "$enemy_boarpiggy", ProcreationAnimalType.Boar },
            { "$enemy_wolf", ProcreationAnimalType.Wolf },
            { "$enemy_wolfcub", ProcreationAnimalType.Wolf },
            { "$enemy_lox", ProcreationAnimalType.Lox },
            { "$enemy_loxcalf", ProcreationAnimalType.Lox },
            { "$enemy_hen", ProcreationAnimalType.Hen },
            { "$enemy_chicken", ProcreationAnimalType.Hen },
            { "$enemy_asksvin", ProcreationAnimalType.Asksvin },
            { "$enemy_asksvin_hatchling", ProcreationAnimalType.Asksvin },
            { "$enemy_moose", ProcreationAnimalType.Moose },
            { "$enemy_moosecalf", ProcreationAnimalType.Moose }
        };

        /// <summary>Matches the game's adult/offspring display keys against the enabled breeding selection.</summary>
        public static bool IsSelected(bool enabled, ProcreationAnimalType selection, string name) =>
            enabled && name != null && NamedTypes.TryGetValue(name, out var type) && selection.HasFlag(type);
    }
}
