#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from AoWE.pas.
using Aow.Engine;

using Aow.Engine.Nodes;

using Aow.Formats.Tree;

using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Units;

// UnitConcealment. ConcealedForPlayers. MoraleValues
public sealed class ArmyView(CustomNode owner) : Army(owner)
{
    public new const int ClassId = 131_609;

    public ushort[] UnitConcealment { get; } = new ushort[8];

    public ushort ConcealedForPlayers { get; set; }

    public int[] MoraleValues { get; } = new int[8];

    // Army.PAS:122
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.UInt16Array(name: "UnitConcealment", values: UnitConcealment);
        ConcealedForPlayers = (ushort)persister.UInt16("ConcealedForPlayers", ConcealedForPlayers);
        persister.Int32Array(name: "MoraleValues", values: MoraleValues);
    }

    // Army.PAS:130
    public override void MainPersist(IPersister persister)
    {
        base.MainPersist(persister);
        // The translate stream's mode holds read, so the original sets the morale there too.
        if (!persister.Loading && !persister.Translating())
        {
            return;
        }

        for (int i = 0; i < Units.Count; i++)
        {
            Units.GetUnit(i).MoraleValue = MoraleValues[i].TruncateToSByte();
        }
    }

    // Army.PAS:138
    public override MapCoordinate GetLocation() => Location;

    // Army.PAS:89
    public override bool GetUnitConcealedForPlayer(int index, PlayerId? player) => ((UnitConcealment[index] >>> (PlayerIdExtensions.ToOriginal(player) - 1)) & 1) != 0;

    // Army.PAS:78
    public override bool GetConcealedForPlayer(PlayerId? player) => ((ConcealedForPlayers >>> (PlayerIdExtensions.ToOriginal(player) - 1)) & 1) != 0;

    // Army.PAS:100
    public void Setup(Army army, int selection)
    {
        AssignSettings(army);
        Location = new MapCoordinate(army.Location.X, army.Location.Y, army.Location.MapLevel);
        for (int i = 0, last = army.Units.Count - 1; i <= last; i++)
        {
            if ((selection & 1) != 0)
            {
                Troop troop = army.Units.GetUnit(i).Copy<Troop>(GetEngine());
                troop.SetOwner(this);
                _ = troop.Release();
                Troop unit = army.Units.GetUnit(i);
                int count = Units.Count;
                MoraleValues[count - 1] = unit.MoraleValue;
                for (int player = 1, lastPlayer = StrategicMap.Current.Players.GetCount() - 1; player <= lastPlayer; player++)
                {
                    SetUnitConcealedForPlayer(value: army.GetUnitConcealedForPlayer(i, player: PlayerId.FromOriginal((sbyte)player)), index: Units.Count - 1, player: PlayerId.FromOriginal((sbyte)player));
                }
            }

            selection >>>= 1;
        }
    }

    // Army.PAS:81
    public void SetConcealedForPlayer(PlayerId player, bool value)
    {
        int mask = 1 << (player.Value - 1);
        ConcealedForPlayers = value ? (ushort)(ConcealedForPlayers | (ushort)mask) : (ushort)(ConcealedForPlayers & ~(ushort)mask);
    }

    // Army.PAS:92
    public void SetUnitConcealedForPlayer(int index, PlayerId? player, bool value)
    {
        int mask = 1 << (PlayerIdExtensions.ToOriginal(player) - 1);
        UnitConcealment[index] = value ? (ushort)(UnitConcealment[index] | (ushort)mask) : (ushort)(UnitConcealment[index] & ~(ushort)mask);
    }
}
