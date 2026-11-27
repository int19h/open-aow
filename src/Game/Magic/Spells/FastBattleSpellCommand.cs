#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Game.Combat;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

public sealed record class FastBattleSpellCommand : FastBattleCommand
{
    public FastBattleSpellCommand(int abilityId, EngineObject creator) : base(abilityId) => Creator = creator;

    // The spell that created this command.
    public EngineObject Creator { get; }

    public int SpellId { get; init; }

    // Spells.PAS:1
    public override string GetName(GameEngine engine)
    {
        Spell spell = engine.GamePack.SpellControl[SpellId];
        string result = AbilityRules.Format(AbilityRules.TranslateResourceString(engine, S: s_castFormat), arguments: [spell.SpellName]);
        return Target is not null ? $"{result} on {Target.IdString()}" : result;
    }

    private static readonly string s_castFormat = "Cast %s";
}
