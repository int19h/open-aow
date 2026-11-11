// Translated from AoWE.pas.
using System.Runtime.InteropServices;

using Aow.Engine.Buffers;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Magic.Abilities;

using Aow.Game.Magic.Spells;

using Aow.Game.Units;

using Aow.Game.World.Map;

namespace Aow.Game.Commands;

// (target IDs, target users, target player, spell data)
public partial class UnitSpellTokenEvent : SpellTokenEvent
{
    public new const int ClassID = 135944;

    public Int32List FTargetUnitIDs;
    public UnitUserList FTargetUnits;
    public byte FTargetPlayer;
    public Int32Array2 FSpellData;

    // Spells.PAS:1126
    public override void Dispose()
    {
        FTargetUnitIDs?.Dispose();
        FTargetUnits?.Dispose();
        base.Dispose();
    }

    // Spells.PAS:1136
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwStreamProc(30, FTargetUnitIDs.LoadFromStream, FTargetUnitIDs.SaveToStream);
        Stream.rwData(MemoryMarshal.AsBytes((Span<int>)FSpellData).Slice(0, 8), 8, 31);
        Stream.rwByte(ref FTargetPlayer, 32);
    }

    // Spells.PAS:1120
    public UnitSpellTokenEvent(ECustomNode owner) : base(owner)
    {
        FTargetUnitIDs = new Int32List();
        FTargetUnits = new UnitUserList(null);
    }

    // Spells.PAS:1159
    public override bool Validate(Hero source)
    {
        int I;
        Troop Target;
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        sbyte TargetX = default;
        sbyte TargetY = default;
        sbyte TargetL = default;
        bool Result = base.Validate(source);
        source.GetLocation(ref X, ref Y, ref L);
        I = FTargetUnitIDs.Count;
        while (Result && I != 0)
        {
            I -= 1;
            var item = FTargetUnitIDs[I];
            Target = AoWHSMap.Current.FUnitControl.FindUnit(item);
            if (Target != null)
            {
                Target.GetLocation(ref TargetX, ref TargetY, ref TargetL);
                Result = Target.GetHitPoints() != 0 && (byte)Target.FPlayer == FTargetPlayer;
                if (Result)
                {
                    if (((UnitSpell)AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID)).FTargetRange != -1)
                    {
                        Result = HexGeometry.dHXtoRad(X, Y, TargetX, TargetY) <= ((UnitSpell)AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID)).FTargetRange;
                    }
                }

                FTargetUnits.AddUnit(Target);
            }
            else
            {
                Result = false;
            }
        }

        return Result;
    }

    // Spells.PAS:1185
    public override bool Process(Troop source, int value)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        Troop Target;
        base.Process(source, value);
        if (FTargetUnits.GetCount() != 0)
        {
            FTargetUnits.GetUnit(0).GetLocation(ref X, ref Y, ref L);
            AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).PlayDefaultCastEffects(source.FPlayer, X, Y, L);
            for (int I = 0, last = FTargetUnits.GetCount() - 1; I <= last; I++)
            {
                Target = FTargetUnits.GetUnit(I);
                Target.AddRef();
                if (((UnitSpell)AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID)).ExecuteSpell(source, Target, FEventMana, ref FSpellData))
                {
                    AoWHSSet.Current.FSpellControl.GetSpell(FEventSpellID).CastingDone(source, FEventMana);
                    Target.Changed();
                }

                Target.Release();
            }
        }

        return false;
    }

    // Spells.PAS:1143
    public void Initialize(int SpellID, Troop Source, UnitList Targets, int Mana)
    {
        FEventSpellID = SpellID;
        FEventPlayerIndex = (byte)Source.FPlayer;
        FEventSourceID = Source.FUnitID;
        FEventMana = Mana;
        if (Targets != null && Targets.GetCount() != 0)
        {
            FTargetPlayer = (byte)Targets.GetUnit(0).FPlayer;
            for (int I = 0, last = Targets.GetCount() - 1; I <= last; I++)
            {
                var unit = Targets.GetUnit(I);
                FTargetUnitIDs.Add(unit.FUnitID);
            }
        }
        else
        {
            FTargetPlayer = 255;
        }
    }

    // intf_P4_02.inc:1059

}
