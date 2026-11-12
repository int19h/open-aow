// Translated from AoWE.pas.
using System.Runtime.InteropServices;
using Aow.Engine;
using Aow.Engine.Buffers;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Formats.Tree;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Units;

// UnitConcealment. ConcealedForPlayers. MoraleValues
public partial class ArmyView : Army
{
    public new const int ClassId = 131609;

    public ArmyView(ECustomNode owner) : base(owner)
    { }

    public UInt16Array8 UnitConcealment;
    public ushort ConcealedForPlayers;
    public Int32Array8 MoraleValues;

    // Army.PAS:122
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwData(MemoryMarshal.AsBytes((Span<ushort>)UnitConcealment).Slice(0, 16), 16, 40);
        stream.rwWord(ref ConcealedForPlayers, 41);
        stream.rwData(MemoryMarshal.AsBytes((Span<int>)MoraleValues).Slice(0, 32), 32, 42);
    }

    // Army.PAS:130
    public override void MainReadWrite(EStorageStream stream)
    {
        base.MainReadWrite(stream);
        if (stream.StorageMode().Contains(StorageModeElement.Read))
        {
            for (int I = 0, last = GetCount() - 1; I <= last; I++)
            {
                GetUnit(I).MoraleValue = (sbyte)MoraleValues[I];
            }
        }
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        PersistSupport.UInt16Array(persister, "UnitConcealment", UnitConcealment);
        persister.UInt16("ConcealedForPlayers", ref ConcealedForPlayers);
        PersistSupport.Int32Array(persister, "MoraleValues", MoraleValues);
    }

    public override void MainPersist(IPersister persister)
    {
        base.MainPersist(persister);
        if (!persister.Loading) return;
        for (int i = 0; i < GetCount(); i++)
            GetUnit(i).MoraleValue = (sbyte)MoraleValues[i];
    }

    // Army.PAS:138
    public override void GetLocation(ref sbyte hexX, ref sbyte hexY, ref sbyte hexLevel)
    {
        hexX = X;
        hexY = Y;
        hexLevel = MapLevel;
    }

    // Army.PAS:89
    public override bool GetUnitConcealedForPlayer(int index, sbyte player) => (((int)(((uint)UnitConcealment[index]) >> (player - 1))) & 1) != 0;

    // Army.PAS:78
    public override bool GetConcealedForPlayer(sbyte player) => (((int)(((uint)ConcealedForPlayers) >> (player - 1))) & 1) != 0;

    // Army.PAS:100
    public virtual void Setup(Army army, byte selection)
    {
        Troop troop;
        AssignSettings(army);
        X = army.X;
        Y = army.Y;
        MapLevel = army.MapLevel;
        for (int I = 0, last = army.GetCount() - 1; I <= last; I++)
        {
            if ((selection & 1) != 0)
            {
                troop = (Troop)army.GetUnit(I).Copy(GameTables.GameEngine);
                troop.SetOwner(this);
                troop.Release();
                var unit = army.GetUnit(I);
                var count = GetCount();
                MoraleValues[count - 1] = unit.MoraleValue;
                for (int player = 1, lastPlayer = AoWHSMap.Current.Players.GetCount() - 1; player <= lastPlayer; player++)
                {
                    var unitConcealedForPlayer = army.GetUnitConcealedForPlayer(I, (sbyte)player);
                    SetUnitConcealedForPlayer(GetCount() - 1, (sbyte)player, unitConcealedForPlayer);
                }
            }

            selection = (byte)(int)(((uint)selection) >> 1);
        }
    }

    // Army.PAS:81
    public void SetConcealedForPlayer(sbyte player, bool value)
    {
        int mask;
        mask = 1 << (player - 1);
        if (value)
        {
            ConcealedForPlayers = (ushort)(ConcealedForPlayers | (ushort)mask);
        }
        else
        {
            ConcealedForPlayers = (ushort)(ConcealedForPlayers & ~(ushort)mask);
        }
    }

    // Army.PAS:92
    public void SetUnitConcealedForPlayer(int index, sbyte player, bool value)
    {
        int mask;
        mask = 1 << (player - 1);
        if (value)
        {
            UnitConcealment[index] = (ushort)(UnitConcealment[index] | (ushort)mask);
        }
        else
        {
            UnitConcealment[index] = (ushort)(UnitConcealment[index] & ~(ushort)mask);
        }
    }

    // intf_P2_07.inc:445

}
