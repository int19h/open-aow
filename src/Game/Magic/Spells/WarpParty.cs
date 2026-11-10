// Translated from GlobalSpells.pas.
using Aow.Engine.Buffers;

using Aow.Engine.Nodes;

using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Spells;

// inherited name, ID, mana, sphere, level, spell type
public partial class WarpParty : Spell
{
    // GlobalSpells.pas:632
    public WarpParty(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(WarpPartyRStr);
        FSpellID = 34;
        FSpellType = SpellType.Global;
        FBaseManaCost = 1;
        FSpellLevel = 2;
        FMagicSphere = MagicSphere.Fire;
    }

    // GlobalSpells.pas:719
    public override bool Activate(AbstractUnit Source)
    {
        bool Result;
        string Reason = "";
        WarpPartyTE Event;
        base.Activate(Source);
        if (CanActivate(Source, ref Reason) && !AoWHSMap.Current.FPlayers.GetPlayers(Source.FPlayer).GetBusy())
        {
            if (Source.Owner is HeroList)
            {
                return false;
            }

            Event = new WarpPartyTE(null);
            Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
            Event.FArmyID = Source.ArmyOwner.CreateID(255);
            var warpLocation = CalculateWarpLocation(Source.ArmyOwner.GetArmyHS());
            Event.FWarpLocation.Packed = warpLocation;
            AoWHSMap.Current.FSelectedArmy.Unselect();
            if (Event.FWarpLocation.Packed != 16777215)
            {
                AoWHSMap.Current.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
                Result = true;
            }
            else
            {
                Result = false;
            }

            Event.Release();
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // GlobalSpells.pas:643
    public bool ValidTarget(ArmyHS Army, sbyte X, sbyte Y, sbyte Layer)
    {
        AoWMapField Field;
        MapFieldMsg Msg = default;
        Field = (AoWMapField)AoWHSMap.Current.MapContainer.GetField(X, Y, Layer);
        bool Result = Field.GetArmyHS() == null && Field.GetPlayer() == -1;
        if (Result)
        {
            Msg.MsgID = 4384;
            Msg.Param[0] = Army;
            Msg.Result = 0;
            Field.SendMapFieldMsg(ref Msg);
            Result = Msg.Result.Int == 0 && Army.FArmy.CanPlace(X, Y, Layer);
        }

        return Result;
    }

    // GlobalSpells.pas:660
    public int CalculateWarpLocation(ArmyHS Army)
    {
        DelphiList Candidates;
        FixedArray_Aow_Units_FixedArray_sbyte_16_16 Table = default;
        MapFieldMsg Msg = default;
        HNWalkData Walk = default;
        MapLevel Level;
        AoWMapField Field;
        int I;
        bool Found;
        Candidates = new DelphiList();
        Army.FArmy.CreateMovePointTable(255, Bytes.Of(ref Table));
        var yhx = Army.Yhx;
        var xhx = Army.Xhx;
        HNWalkData.InitHNWalk(ref Walk, xhx, yhx, 10);
        Level = AoWHSMap.Current.MapContainer.MapLevels[Army.Lhx];
        while (Walk.Radius < 20)
        {
            if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
            {
                Field = (AoWMapField)Level.GetField(Walk.X, Walk.Y);
                if (Table[Field.TerrainType][Field.OverlayType - -1] != -1)
                {
                    Candidates.Add(Field);
                }
            }

            HNWalkData.WalkToNextHN(ref Walk);
        }

        for (int L = 0, last = AoWHSMap.Current.MapContainer.LevelCount - 1; L <= last; L++)
        {
            if (Math.Abs(L - Army.Lhx) == 1)
            {
                Level = AoWHSMap.Current.MapContainer.MapLevels[L];
                var armyYhx = Army.Yhx;
                var armyXhx = Army.Xhx;
                HNWalkData.InitHNWalk(ref Walk, armyXhx, armyYhx, 0);
                while (Walk.Radius < 10)
                {
                    if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
                    {
                        Field = (AoWMapField)Level.GetField(Walk.X, Walk.Y);
                        if (Table[Field.TerrainType][Field.OverlayType - -1] != -1)
                        {
                            Candidates.Add(Field);
                        }
                    }

                    HNWalkData.WalkToNextHN(ref Walk);
                }
            }
        }

        Found = false;
        int Result = 16777215;
        while (Candidates.Count != 0 && !Found)
        {
            I = Random(Candidates.Count);
            Field = (AoWMapField)Candidates[I];
            if (Field.GetArmyHS() == null && Field.GetPlayer() == -1)
            {
                Msg.MsgID = 4384;
                Msg.Param[0] = Army;
                Msg.Result = 0;
                Field.SendMapFieldMsg(ref Msg);
                Found = Msg.Result.Int == 0 && Army.FArmy.CanPlace(Field.Xhx, Field.Yhx, Field.Lhx);
            }

            if (Found)
            {
                Result = Field.GetXYL();
            }
            else
            {
                Candidates.Delete(I);
            }
        }

        Candidates?.Dispose();
        return Result;
    }

    private static readonly string WarpPartyRStr = "Warp Party";
}
