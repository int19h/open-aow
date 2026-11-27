#nullable disable warnings
// Translated from AoWE.pas.

namespace Aow.Game.Combat;

// { Interface declaration run owned by P5. }
// ability ID, source DV, target DV, expected DV, move cost, target
public record class FastBattleCommand
{
    public FastBattleCommand(int abilityId) => AbilityId = abilityId;

    public int AbilityId { get; }
    public int SourceDamageValue { get; init; }
    public int TargetDamageValue { get; init; }
    public int ExpectedDamageValue { get; init; }
    public int MoveCost { get; init; }
    public CombatObject? Target { get; init; }

    // AIGroup.pas:6710
    // The name reads the pack of the engine the caller runs for (issue 335).
    public virtual string GetName(GameEngine engine)
    {
        string result = engine.GamePack.AbilityControl[AbilityId].GetName(null);
        return Target is not null ? $"{result} on {Target.IdString()}" : result;
    }
}
