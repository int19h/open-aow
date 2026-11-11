// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Spells;

public partial class Tornado : GlobalTargetSpell
{
    // GlobalTargetSpells.pas:1798
    public Tornado(ECustomNode owner) : base(owner)
    {
        this.FSpellName = AbilityRules.TranslateRStr(GameText.TornadoRStr);
        FSpellID = 68;
        FAboveGroundOnly = true;
    }

    // GlobalTargetSpells.pas:1885
    public override void SetupTargetSelection(Troop Source, TargetSelectionControl TargetSelection)
    {
        TargetSelectionControl Target;
        Target = TargetSelection;
        base.SetupTargetSelection(Source, Target);
        Target.FHNs.Clear();
    }

    // GlobalTargetSpells.pas:1865
    public override bool TargetSelected(Troop Source, int X, int Y, int L)
    {
        bool Result;
        TornadoTokenEvent Event;
        ArmyHexSprite Army;
        if (!AoWHSMap.Current.FPlayers.GetPlayers(Source.FPlayer).GetBusy())
        {
            Event = new TornadoTokenEvent(null);
            Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
            Event.SetLocation((sbyte)X, (sbyte)Y, (sbyte)L);
            Army = ((AoWMapField)AoWHSMap.Current.MapContainer.GetField(X, Y, L)).GetArmyHS();
            if (Army != null)
            {
                Event.FArmyID = Army.FArmy.CreateID(255);
            }

            AoWHSMap.Current.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
            // Original queues through AddEvent
            Event.Release();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // GlobalTargetSpells.pas:1806
    public bool ValidWarpLocation(ArmyHexSprite Army, sbyte X, sbyte Y, sbyte Layer)
    {
        AoWMapField MapF;
        MapFieldMessage Msg = default;
        MapF = (AoWMapField)AoWHSMap.Current.MapContainer.GetField(X, Y, Layer);
        bool Result = MapF.GetArmyHS() == null && MapF.GetPlayer() == -1;
        if (Result)
        {
            Msg.MsgID = 4384;
            Msg.Param[0] = Army;
            Msg.Result = 0;
            MapF.SendMapFieldMsg(ref Msg);
            Result = Msg.Result.Int == 0 && Army.FArmy.CanPlace(X, Y, Layer);
        }

        return Result;
    }

    // GlobalTargetSpells.pas:1823
    public int CalculateWarpLocation(ArmyHexSprite Army, byte Selection)
    {
        SpellMoveTable Table = default;
        HexNumberWalkData Walk = default;
        MapLevel MapLevel;
        MapField MapF;
        DelphiList Locations;
        bool Found;
        int Index;
        int X;
        Locations = new DelphiList();
        Army.FArmy.CreateMovePointTable(Selection, Bytes.Of(ref Table));
        var yhx = Army.Yhx;
        var xhx = Army.Xhx;
        HexNumberWalkData.InitHNWalk(ref Walk, xhx, yhx, 10);
        MapLevel = AoWHSMap.Current.MapContainer.MapLevels[Army.Lhx];
        while (Walk.Radius < 15)
        {
            X = Walk.X;
            if (X >= 0 && Walk.Y >= 0 && X < MapLevel.Width && Walk.Y < MapLevel.Height)
            {
                MapF = MapLevel.GetField(X, Walk.Y);
                if (Table.Cost[MapF.TerrainType][MapF.OverlayType] != -1)
                {
                    Locations.Add(MapF);
                }
            }

            HexNumberWalkData.WalkToNextHN(ref Walk);
        }

        Found = false;
        int Result = 16777215;
        while (Locations.Count != 0 && !Found)
        {
            Index = Random(Locations.Count);
            MapF = (MapField)Locations[Index];
            if (ValidWarpLocation(Army, MapF.Xhx, MapF.Yhx, MapF.Lhx))
            {
                Found = true;
                Result = MapF.XYL;
            }
            else
            {
                Locations.Delete(Index);
            }
        }

        Locations?.Dispose();
        return Result;
    }
}
