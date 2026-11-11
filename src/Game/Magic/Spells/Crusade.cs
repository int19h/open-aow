// Translated from GlobalSpells.pas.
using Aow.Engine.Buffers;

using Aow.Engine.Nodes;
using Aow.Game.AI.Groups;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;
using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

// inherited name, spell ID, mana, sphere, level, type, sounds, images
public partial class Crusade : Spell
{
    // GlobalSpells.pas:1302
    public Crusade(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(CrusadeRStr);
        FSpellID = 75;
        FSpellType = SpellType.Global;
        FBaseManaCost = 0;
        FSpellLevel = 4;
        FMagicSphere = MagicSphere.Life;
    }

    // GlobalSpells.pas:1340
    public override bool ExecuteTE(SpellTokenEvent Event, Troop Source, int ExecuteCount)
    {
        int Count;
        int UnitIndex;
        int Xhp = default;
        int Yhp = default;
        sbyte X = default;
        sbyte Y = default;
        sbyte Layer = default;
        ArmyHexSprite Party;
        Unit NewUnit;
        if (ExecuteCount == 0)
        {
            Source.GetLocation(ref X, ref Y, ref Layer);
            AoWHSMap.Current.ValidateObservation(Source.FPlayer, X, Y, Layer, 0);
            HexGeometry.HXtoHP(X, Y, ref Xhp, ref Yhp);
            var worldFXHS = AoWHSMap.Current.AddEffect(WorldEffectType.Crusade);
            worldFXHS.FX = Xhp + 16;
            worldFXHS.FY = Yhp + 16;
            worldFXHS.FLayer = Layer;

            if (AoWHSMap.Current.Random(100) < 20)
            {
                Party = new ArmyHexSprite(null);
                Party.FArmy.SetPlayer(0);
                Count = 1;
                UnitIndex = AoWHSMap.Current.Random(1);
                for (int J = 0; J <= Count - 1; J++)
                {
                    NewUnit = new Unit(null);
                    NewUnit.SetUnitResource(AoWHSSet.Current.FUnitResources.GetUnitResource(TCrusade_ExecuteTE__LargeUnits[UnitIndex]));
                    NewUnit.ExpandAbility(101);
                    Party.FArmy.AddUnit(NewUnit);
                    NewUnit.Release();
                }

                if (!PlaceParty(Source, Party))
                {
                    Party?.Dispose();
                }
            }
            else
            {
                for (int I = 0; I <= 1; I++)
                {
                    Party = new ArmyHexSprite(null);
                    Party.FArmy.SetPlayer(0);
                    Count = AoWHSMap.Current.Random(3) + 4;
                    for (int J = 0; J <= Count - 1; J++)
                    {
                        NewUnit = new Unit(null);
                        UnitIndex = AoWHSMap.Current.Random(2);
                        NewUnit.SetUnitResource(AoWHSSet.Current.FUnitResources.GetUnitResource(TCrusade_ExecuteTE__FootUnits[UnitIndex]));
                        NewUnit.ExpandAbility(101);
                        Party.FArmy.AddUnit(NewUnit);
                        NewUnit.Release();
                    }

                    if (!PlaceParty(Source, Party))
                    {
                        Party?.Dispose();
                    }
                }

                Party = new ArmyHexSprite(null);
                Party.FArmy.SetPlayer(0);
                UnitIndex = AoWHSMap.Current.Random(3);
                Count = 2;
                if (UnitIndex == 2)
                {
                    Count += 1;
                }

                for (int J = 0; J <= Count - 1; J++)
                {
                    NewUnit = new Unit(null);
                    NewUnit.SetUnitResource(AoWHSSet.Current.FUnitResources.GetUnitResource(TCrusade_ExecuteTE__MountedUnits[UnitIndex]));
                    NewUnit.ExpandAbility(101);
                    Party.FArmy.AddUnit(NewUnit);
                    NewUnit.Release();
                }

                if (!PlaceParty(Source, Party))
                {
                    Party?.Dispose();
                }
            }

            AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).CastingDone(Source, FBaseManaCost);
            AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastSounds.Play(0);
        }

        return false;
    }

    // GlobalSpells.pas:1417
    public override bool Activate(Troop Source)
    {
        bool Result;
        string Reason = "";
        SpellTokenEvent Event;
        base.Activate(Source);
        if (CanActivate(Source, ref Reason) && !AoWHSMap.Current.FPlayers.GetPlayers(Source.FPlayer).GetBusy())
        {
            Event = new SpellTokenEvent(null);
            Event.Initialize(FSpellID, (byte)Source.FPlayer, Source.FUnitID, FBaseManaCost);
            AoWHSMap.Current.FTokenManager.GetTokenControl(Source.FPlayer).AddEvent(Event);
            Event.Release();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // GlobalSpells.pas:1313
    public bool PlaceParty(Troop Source, ArmyHexSprite Party)
    {
        bool Result;
        int TargetX = default;
        int TargetY = default;
        sbyte X = default;
        sbyte Y = default;
        sbyte Layer = default;
        ImageSequenceAnimation Animation;
        Source.GetLocation(ref X, ref Y, ref Layer);
        var radToHN = HexGeometry.RadToHN(5);
        var random = AoWHSMap.Current.Random(radToHN);
        HexGeometry.CenterHNtoHX(X, Y, random, ref TargetX, ref TargetY);
        if (Party.PlaceEx((sbyte)TargetX, (sbyte)TargetY, Layer, Set<ArmyPlaceModeElement>.Of(ArmyPlaceModeElement.Safe, ArmyPlaceModeElement.ScanArea)))
        {
            Party.Activate();
            Party.FArmy.SetBehavior(255, GroupBehavior.Suicidal);
            var lhx = Party.GetLhx();
            var yhx = Party.GetYhx();
            var xhx = Party.GetXhx();
            if (AoWHSMap.Current.Watching(xhx, yhx, lhx, 0))
            {
                if (AoWHSSet.Current.FSpellControl.GetSpell(34).FCastImages.GetImageSequence(0) != null)
                {
                    // { Original uses Warp Party's images for Crusade parties. }
                    Animation = new ImageSequenceAnimation(null);
                    Animation.SetImageSequence(AoWHSSet.Current.FSpellControl.GetSpell(34).FCastImages.GetImageSequence(0));
                    Animation.FAnimationLevel = 28;
                    var partyLhx = Party.GetLhx();
                    var partyYhx = Party.GetYhx();
                    var partyXhx = Party.GetXhx();
                    AoWHSMap.Current.PlaceHX(Animation, partyXhx, partyYhx, partyLhx);
                }
            }

            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    private static readonly string CrusadeRStr = "Crusade";
    private static Int32Array1 TCrusade_ExecuteTE__LargeUnits = Int32Array1.From([133]);
    private static Int32Array3 TCrusade_ExecuteTE__MountedUnits = Int32Array3.From([132, 134, 128]);
    private static Int32Array2 TCrusade_ExecuteTE__FootUnits = Int32Array2.From([129, 126]);
}
