// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Magic.Spells;

public record class FastBattleSpellCommand : FastBattleCommand
{
    public FastBattleSpellCommand(int abilityId, EngineObject creator) : base(abilityId) => Creator = creator;

    // The spell that created this command. The command reaches the engine through it.
    public EngineObject Creator { get; }

    public int SpellId { get; init; }

    // Spells.PAS:1
    public override string GetName()
    {
        Spell spell = GamePack.Current.SpellControl[SpellId];
        string result = AbilityRules.Format(AbilityRules.TranslateResourceString(Creator.GetEngine(), s_castFormat), [spell.SpellName]);
        return Target is not null ? $"{result} on {Target.IdString()}" : result;
    }

    private static readonly string s_castFormat = "Cast %s";
}
