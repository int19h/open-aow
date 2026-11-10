// Translated from CitySpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class TownGate : CityTargetSpell
{
    // CitySpells.pas:421
    public TownGate(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(TownGateRStr);
        FSpellID = 38;
        FCityTargetMode = Set<CityTargetModeElement>.Of(CityTargetModeElement.Own, CityTargetModeElement.SkipSource);
        FBaseManaCost = 5;
        FSpellLevel = 3;
        FMagicSphere = MagicSphere.Cosmos;
    }

    // CitySpells.pas:486
    public override bool ExecuteTE(SpellTE Event, AbstractUnit Source, int ExecuteCount)
    {
        bool ShowSource;
        bool ShowTarget;
        byte Selection = default;
        ArmyHS Army = default;
        AoWMapField Field;
        ArmyHS NewArmy;
        sbyte X = default;
        sbyte Y = default;
        sbyte L = default;
        ImageSequenceAnimation A;
        Source.GetLocation(ref X, ref Y, ref L);
        ((TownGateTE)Event).FArmyID.RetrieveArmy(ref Army, ref Selection);
        Field = (AoWMapField)AoWHSMap.Current.MapContainer.GetFieldXYL(((TownGateTE)Event).FTeleportLocation.Packed);
        Army.FArmy.ResetDestinationPos();
        if (AoWHSMap.Current.FSelectedArmy.GetArmyHS() == Army)
        {
            AoWHSMap.Current.FSelectedArmy.Select(Army);
        }

        if (Army.FArmy.ValidateSelection(Selection) != Army.FArmy.ValidateSelection(255))
        {
            NewArmy = new ArmyHS(null);
            NewArmy.FArmy.SetPlayer(Army.FArmy.FPlayer);
            NewArmy.FArmy.AddUnitSelection(Army.FArmy, Selection);
            AoWHSMap.Current.PlaceHX(NewArmy, ((TownGateTE)Event).FTeleportLocation.X, ((TownGateTE)Event).FTeleportLocation.Y, ((TownGateTE)Event).FTeleportLocation.L);
        }
        else
        {
            if (Field.GetArmyHS() != null)
            {
                Field.GetArmyHS().FArmy.AddUnitSelection(Army.FArmy, Selection);
            }
            else
            {
                Army.MoveTo(Field, 0);
            }
        }

        ShowSource = AoWHSMap.Current.FSeatedPlayer == Source.FPlayer || AoWHSMap.Current.Visible(X, Y, L, 0);
        ShowTarget = AoWHSMap.Current.FSeatedPlayer == Source.FPlayer || AoWHSMap.Current.Visible(Field.Xhx, Field.Yhx, Field.Lhx, 0);
        if (ShowSource)
        {
            if (AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastImages.ImageSequences[0] != null)
            {
                A = new ImageSequenceAnimation(null);
                A.SetImageSequence(AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastImages.ImageSequences[0]);
                AoWHSMap.Current.PlaceHX(A, X, Y, L);
            }
        }

        if (ShowTarget)
        {
            if (AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastImages.ImageSequences[0] != null)
            {
                A = new ImageSequenceAnimation(null);
                A.SetImageSequence(AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastImages.ImageSequences[0]);
                AoWHSMap.Current.PlaceHX(A, Field.Xhx, Field.Yhx, Field.Lhx);
            }
        }

        if (AoWHSMap.Current.FSeatedPlayer == Source.FPlayer)
        {
            AoWHSMap.Current.CenterViewTo(Field.Xhx, Field.Yhx, Field.Lhx);
        }
        else
        {
            if (!AoWHSMap.Current.ValidateObservation(Source.FPlayer, Field.Xhx, Field.Yhx, Field.Lhx, 0))
            {
                AoWHSMap.Current.ValidateObservation(Source.FPlayer, X, Y, L, 0);
            }
        }

        if (AoWHSMap.Current.FSeatedPlayer == Source.FPlayer || AoWHSMap.Current.Watching(X, Y, L, 0) || AoWHSMap.Current.Watching(Field.Xhx, Field.Yhx, Field.Lhx, 0))
        {
            AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastSounds.Play(0);
        }

        CastingDone(Source, Event.FEventMana);
        return false;
    }

    // CitySpells.pas:540
    public override bool CanActivate(AbstractUnit Source, ref string MessageText) => base.CanActivate(Source, ref MessageText);

    // CitySpells.pas:545
    public override void SetupTargetSelection(AbstractUnit Source, TargetSelectionControl TargetSelection)
    {
        base.SetupTargetSelection(Source, TargetSelection);
        TargetSelection.FTargetMode = TargetSelection.FTargetMode - Set<TargetSelectionModeElement>.Of(TargetSelectionModeElement.FixedLevel, TargetSelectionModeElement.ValidateDistance);
    }

    // CitySpells.pas:445
    public override CityTargetSpellTE CreateTE() => new TownGateTE(null);

    // CitySpells.pas:450
    public override bool SetupTE(CityTargetSpellTE Event, AbstractUnit Source, int X, int Y, int L)
    {
        byte Selection;
        ArmyHS Army;
        bool Found;
        MapLevel Level;
        HNWalkData Walk = default;
        bool Result = base.SetupTE(Event, Source, X, Y, L);
        if (Result)
        {
            if (Source.Owner is HeroList)
            {
                return false;
            }

            Army = Source.ArmyOwner.GetArmyHS();
            if (Army.FArmy.FPlayer == AoWHSMap.Current.FSeatedPlayer && AoWHSMap.Current.FSelectedArmy.GetArmyHS() == Army)
            {
                Selection = AoWHSMap.Current.FSelectedArmy.GetSelection();
            }
            else
            {
                Selection = 255;
            }

            HNWalkData.InitHNWalk(ref Walk, X, Y, 0);
            Found = false;
            Level = AoWHSMap.Current.MapContainer.GetMapLevel(L);
            while (Walk.Radius < 4 && !Found)
            {
                if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
                {
                    Found = ValidTeleportLocation(Army, Selection, (sbyte)Walk.X, (sbyte)Walk.Y, (sbyte)L);
                }

                if (!Found)
                {
                    HNWalkData.WalkToNextHN(ref Walk);
                }
            }

            if (Found)
            {
                var id = Army.FArmy.CreateID(Selection);
                ((TownGateTE)Event).FArmyID = id;
                var packXYL = HexGeometry.PackXYL((sbyte)Walk.X, (sbyte)Walk.Y, (sbyte)L);
                ((TownGateTE)Event).FTeleportLocation = packXYL;
                Result = true;
            }
            else
            {
                Result = false;
            }
        }

        return Result;
    }

    // CitySpells.pas:432
    public bool ValidTeleportLocation(ArmyHS Army, byte Selection, sbyte X, sbyte Y, sbyte L)
    {
        bool Result;
        AoWMapField Field;
        Field = (AoWMapField)AoWHSMap.Current.MapContainer.GetField(X, Y, L);
        if (Field.GetPlayer() != -1 && Field.GetPlayer() != Army.FArmy.FPlayer)
        {
            Result = false;
        }
        else
        {
            Result = true;
        }

        if (Result)
        {
            Result = Army.FArmy.ValidTerrainEx(Selection, Field.TerrainType, Field.OverlayType);
            // { CanAddUnitSelection returns arOK (zero) when the destination accepts the selection. }
            Result = Result && (Field.GetArmyHS() == null || Field.GetArmyHS().FArmy.CanAddUnitSelection(Army.FArmy, Selection) == AddUnitsResult.OK);
        }

        return Result;
    }

    private static readonly string TownGateRStr = "Town Gate";
}
