// Translated from GlobalTargetSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class Vortex : GlobalTargetSpell
{
    // GlobalTargetSpells.pas:2069
    public Vortex(ECustomNode owner) : base(owner)
    {
        this.FSpellName = AbilityRules.TranslateRStr(GameText.VortexRStr);
        FSpellID = 70;
        FAboveGroundOnly = false;
    }

    // GlobalTargetSpells.pas:2120
    public override void SetupTargetSelection(AbstractUnit Source, TargetSelectionControl TargetSelection)
    {
        // { Use the incoming target. }
        base.SetupTargetSelection(Source, TargetSelection);
        TargetSelection.FHNs.Clear();
        TargetSelection.FOnValidateTarget = ValidateTargetEvent;
    }

    // GlobalTargetSpells.pas:2100
    public override bool TargetSelected(AbstractUnit Source, int X, int Y, int L)
    {
        bool Result;
        VortexTE Event;
        ArmyHS Army;
        if (!AoWHSMap.Current.FPlayers.GetPlayers(Source.FPlayer).GetBusy())
        {
            Event = new VortexTE(null);
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

    // GlobalTargetSpells.pas:2077
    public override bool ValidTargetSelection(TargetSelectionControl TargetSelection)
    {
        bool Result;
        MapField MapF;
        if (TargetSelection.FTargetX != -1)
        {
            MapF = AoWHSMap.Current.MapContainer.MapLevels[TargetSelection.FTargetL].GetField(TargetSelection.FTargetX, TargetSelection.FTargetY);
            Result = Set<byte>.WithBounds(0, 255).With(0).With(10).Contains((byte)MapF.TerrainType);
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // GlobalTargetSpells.pas:2088
    public void ValidateTargetEvent(TargetSelectionControl Target, sbyte X, sbyte Y, sbyte Layer, ref bool Valid)
    {
        MapLevel MapLevel;
        if (X != -1)
        {
            MapLevel = AoWHSMap.Current.MapContainer.MapLevels[Layer];
            var fieldValue = MapLevel.GetField(X, Y);
            Valid = Set<byte>.WithBounds(0, 255).With(0).With(10).Contains((byte)fieldValue.TerrainType);
        }
        else
        {
            Valid = false;
        }
    }
}
