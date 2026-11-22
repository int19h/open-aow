// Translated from AoWE.pas.
using System.Diagnostics.CodeAnalysis;
using Aow.Game.Session;
using Aow.Game.Units;

namespace Aow.Game.Combat;

public struct BattleFieldInfo
{
    public sbyte Player { get; private set; }
    public sbyte IndependentRelation { get; }
    public BattleFieldInfoRequests Requests { get; set; }
    public DamageTypes DamageTypes { get; private set; }

    // The troops already counted. A default value has no holder and counts every troop.
    public UnitCollection Units { get; set; }
    public DiplomaticRelations FriendlyRelations { get; set; }
    public DiplomaticRelations EnemyRelations { get; set; }
    public int FriendlyStrength { get; set; }
    public int Strength { get; set; }

    [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "The Delphi signature keeps relation and the original ignores it.")]
    internal static void SetupBattleFieldInfo(out BattleFieldInfo info, sbyte player, int relation, BattleFieldInfoRequests requests, DamageTypes damageTypes)
    {
        info = default;
        info.Player = player;
        // { Original ignores the independent relation parameter. }
        info.Requests = requests;
        info.DamageTypes = damageTypes;
        info.FriendlyRelations = DiplomaticRelations.Alliance;
        info.EnemyRelations = DiplomaticRelations.War | DiplomaticRelations.Unknown;
    }
}
