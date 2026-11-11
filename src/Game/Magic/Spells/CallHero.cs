// Translated from GlobalSpells.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;
using Aow.Game.World.Map;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Spells;

// inherited name, spell ID, mana, sphere, level, type, sounds, images
public partial class CallHero : Spell
{
    // GlobalSpells.pas:1184
    public CallHero(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(CallHeroRStr);
        FSpellID = 69;
        FSpellType = SpellType.Global;
        FBaseManaCost = 0;
        FSpellLevel = 2;
        FMagicSphere = MagicSphere.Cosmos;
    }

    // GlobalSpells.pas:1250
    public override bool ExecuteTE(SpellTokenEvent Event, Troop Source, int ExecuteCount)
    {
        bool ShowSource;
        bool ShowTarget;
        sbyte X = default;
        sbyte Y = default;
        sbyte Layer = default;
        Hero Hero;
        ArmyHexSprite Army;
        ImageSequenceAnimation Animation;
        base.ExecuteTE(Event, Source, ExecuteCount);
        Source.GetLocation(ref X, ref Y, ref Layer);
        Hero = FindCallHero(Source);
        if (Hero != null)
        {
            if (Hero.Owner == null)
            {
                Hero.SetHitPoints(Hero.GetHits());
            }

            Army = Hero.Place(X, Y, Layer, 0, Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.ScanArea, UnitPlaceModeElement.CreateNewArmy));
            ShowSource = AoWHSMap.Current.FSeatedPlayer == Source.FPlayer || AoWHSMap.Current.VisibleForPlayer(X, Y, Layer, AoWHSMap.Current.FSeatedPlayer);
            bool condition = AoWHSMap.Current.FSeatedPlayer == Source.FPlayer;
            if (!condition)
            {
                var lhx = Army.GetLhx();
                var yhx = Army.GetYhx();
                var xhx = Army.GetXhx();
                condition = AoWHSMap.Current.VisibleForPlayer(xhx, yhx, lhx, AoWHSMap.Current.FSeatedPlayer);
            }

            ShowTarget = condition;
            if (ShowSource)
            {
                if (AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastImages.GetImageSequence(0) != null)
                {
                    Animation = new ImageSequenceAnimation(null);
                    Animation.SetImageSequence(AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastImages.GetImageSequence(0));
                    AoWHSMap.Current.PlaceHX(Animation, X, Y, Layer);
                }
            }

            if (ShowTarget)
            {
                if (AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastImages.GetImageSequence(0) != null)
                {
                    Animation = new ImageSequenceAnimation(null);
                    Animation.SetImageSequence(AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastImages.GetImageSequence(0));
                    var lhx = Army.GetLhx();
                    var yhx = Army.GetYhx();
                    var xhx = Army.GetXhx();
                    AoWHSMap.Current.PlaceHX(Animation, xhx, yhx, lhx);
                }
            }

            if (AoWHSMap.Current.FSeatedPlayer == Source.FPlayer)
            {
                var lhx = Army.GetLhx();
                var yhx = Army.GetYhx();
                var xhx = Army.GetXhx();
                AoWHSMap.Current.CenterViewTo(xhx, yhx, lhx);
            }
            else
            {
                var lhx = Army.GetLhx();
                var yhx = Army.GetYhx();
                var xhx = Army.GetXhx();
                AoWHSMap.Current.ValidateObservation(Source.FPlayer, xhx, yhx, lhx, 0);
            }

            bool test = AoWHSMap.Current.FSeatedPlayer == Source.FPlayer || AoWHSMap.Current.Watching(X, Y, Layer, 0);
            if (!test)
            {
                var lhx = Army.GetLhx();
                var yhx = Army.GetYhx();
                var xhx = Army.GetXhx();
                test = AoWHSMap.Current.Watching(xhx, yhx, lhx, 0);
            }

            if (test)
            {
                AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastSounds.Play(0);
            }
        }
        else
        {
            if (AoWHSMap.Current.FSeatedPlayer == Source.FPlayer)
            {
                var noHeroAnsweredYourCall = AbilityRules.TranslateRStr(NoHeroAnsweredYourCallRStr);
                AoWHSMap.Current.ShowMessage(noHeroAnsweredYourCall);
                AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).FCastSounds.Play(0);
            }
        }

        var spell = AoWHSSet.Current.FSpellControl.GetSpell(FSpellID);
        AoWHSSet.Current.FSpellControl.GetSpell(FSpellID).CastingDone(Source, spell.FBaseManaCost);
        return false;
    }

    // GlobalSpells.pas:1195
    public override bool Activate(Troop Source)
    {
        bool Result;
        string Reason = "";
        SpellTokenEvent Event;
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

    // GlobalSpells.pas:1212
    public Hero FindCallHero(Troop Source)
    {
        Hero Result;
        Hero Hero;
        DoubleItemList<Hero> Heroes;
        int Index;
        Heroes = new DoubleItemList<Hero>();
        for (int I = AoWHSMap.Current.FHeroControl.GetCount() - 1; I >= 0; I--)
        {
            Hero = AoWHSMap.Current.FHeroControl.GetHero(I);
            if (Hero.FPlayer == 0 && Hero.Owner is Army && Hero.ArmyOwner.FX != -1)
            {
                if (Hero.OfferToJoin(Source.FPlayer))
                {
                    Heroes.AddAB(Hero, 10);
                }
                else
                {
                    Heroes.AddAB(Hero, 1);
                }
            }
        }

        if (Heroes.Count == 0)
        {
            for (int I = AoWHSMap.Current.FHeroControl.GetCount() - 1; I >= 0; I--)
            {
                Hero = AoWHSMap.Current.FHeroControl.GetHero(I);
                if (!Hero.FUnitStatus.Contains(UnitStatusElement.Killed) && Hero.FPlayer <= 0 && Hero.Owner == null)
                {
                    if (Hero.OfferToJoin(Source.FPlayer))
                    {
                        Heroes.AddAB(Hero, 10);
                    }
                    else
                    {
                        Heroes.AddAB(Hero, 1);
                    }
                }
            }
        }

        if (Heroes.Count != 0)
        {
            RandSeed = AoWHSMap.Current.Random(16777215);
            Index = Heroes.RandomChanceItemB(0);
            Result = Heroes[Index].A;
        }
        else
        {
            Result = null;
        }

        Heroes?.Dispose();
        return Result;
    }

    private static readonly string CallHeroRStr = "Call Hero";
    private static readonly string NoHeroAnsweredYourCallRStr = "No hero answered your call";
}
