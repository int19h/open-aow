namespace Aow.Game.Magic.Spells;

// A view key for a spell caster. Instances are the keys, the name serves the reports.
public sealed class SpellCasterView(string name)
{
    public string Name => name;

    public override string ToString() => name;

    // No dialog opens for this caster.
    public static SpellCasterView None { get; } = new("None");
    public static SpellCasterView UnitTargets { get; } = new("UnitTargets");
    public static SpellCasterView Disjunction { get; } = new("Disjunction");
    public static SpellCasterView CosmagicSurgery { get; } = new("CosmagicSurgery");
}
