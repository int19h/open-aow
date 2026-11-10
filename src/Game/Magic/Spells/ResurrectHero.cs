// Translated from GlobalSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animations;

using Aow.Game.Events;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;
using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

// inherited name, spell ID, mana, sphere, level, type, sounds, images
public partial class ResurrectHero : Spell
{
    // GlobalSpells.pas:980
    public ResurrectHero(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(ResurrectHeroRStr);
        FSpellID = 66;
        FSpellType = SpellType.Global;
        FBaseManaCost = 0;
        FSpellLevel = 2;
        FMagicSphere = MagicSphere.Cosmos;
    }

    // GlobalSpells.pas:991
    public override bool ExecuteTE(SpellTE Event, AbstractUnit Source, int ExecuteCount)
    {
        bool Result = default;
        sbyte X = default;
        sbyte Y = default;
        sbyte Layer = default;
        TwinkleAnimation Animation = default;
        HeroList Heroes;
        Hero Hero;
        ArmyHS Army;
        LocationTextEventLog Log;
        HeroEmergeEventLog EmergeLog = default;
        ArmyInfoEventLog InfoLog = default;
        if (ExecuteCount == 0)
        {
            Source.GetLocation(ref X, ref Y, ref Layer);
            AoWHSMap.Current.ValidateObservation(Source.FPlayer, X, Y, Layer, 0);
            Animation = new TwinkleAnimation(null);
            Animation.Setup(FCastImages.GetImageSequence(0), Set<TwinkleModeElement>.Of(TwinkleModeElement.Rotate), 50, 64, 50, 30);
            AoWHSMap.Current.PlaceHX(Animation, X, Y, Layer);
            if (Source.FPlayer == AoWHSMap.Current.FSeatedPlayer || AoWHSMap.Current.Watching(X, Y, Layer, 0))
            {
                FCastSounds.Play(0);
            }
        }

        if (ExecuteCount == 20)
        {
            Heroes = new HeroList(null);
            AoWHSMap.Current.FHeroControl.ListDeadHeroes(Heroes);
            for (int I = Heroes.Count - 1; I >= 0; I--)
            {
                if (Heroes.GetHero(I) is Leader || Heroes.GetHero(I).GetLevel() > AoWHSMap.Current.FPlayers.GetPlayers(Source.FPlayer).FBaseHeroJoinLevel + 2)
                {
                    Heroes.RemoveChild(Heroes.GetHero(I));
                }
            }

            if (Heroes.Count != 0)
            {
                Source.GetLocation(ref X, ref Y, ref Layer);
                var count = Heroes.Count;
                Hero = Heroes.GetHero(AoWHSMap.Current.Random(count));
                Hero.Resurrect();
                Army = Hero.Place(X, Y, Layer, 0, Set<UnitPlaceModeElement>.Of(UnitPlaceModeElement.ScanArea, UnitPlaceModeElement.CreateNewArmy));
                Log = new LocationTextEventLog(null);
                Log.FPlayer = Source.FPlayer;
                Log.FText = RtlUnit.Format(AbilityRules.TranslateRStr(XResurrectedRStr), [Hero.FHeroName]);
                var lhx = Army.GetLhx();
                var yhx = Army.GetYhx();
                Log.SetLocation(Army.GetXhx(), yhx, lhx);
                var armyYhx = Army.GetYhx();
                var armyLhx = Army.GetLhx();
                var xhx = Army.GetXhx();
                EventLog.DistributeLocationEventLog(Log, Source.FPlayer, xhx, armyYhx, armyLhx, false);
                Log.Release();
                if (Hero.OfferToJoin(Source.FPlayer))
                {
                    EmergeLog = new HeroEmergeEventLog(null);
                    EmergeLog.Setup(Hero, Source.FPlayer, HeroEmergeEventLog.HeroJoinMode.Resurrected);
                    EmergeLog.Execute(true, ExecuteTrigger.Event, null);
                    EmergeLog.Release();
                }
                else
                {
                    InfoLog = new ArmyInfoEventLog(null);
                    var indexOf = Army.FArmy.IndexOf(Hero);
                    InfoLog.FArmyView.Setup(Army.FArmy, (byte)(1 << indexOf));
                    InfoLog.FSelection = 255;
                    InfoLog.FPlayer = Source.FPlayer;
                    var noJoinResurrection = AbilityRules.TranslateRStr(NoJoinResurrectionRStr);
                    InfoLog.FMessages.Text = noJoinResurrection;
                    InfoLog.FTriggers = Set<ExecuteTrigger>.Of(ExecuteTrigger.Event);
                    InfoLog.Execute(true, ExecuteTrigger.Event, null);
                    InfoLog.Release();
                }
            }
            else
            {
                var noHeroAnswersYourCall = AbilityRules.TranslateRStr(GameText.NoHeroAnswersYourCallRStr);
                AoWHSMap.Current.ShowMessage(noHeroAnswersYourCall);
            }

            CastingDone(Source, Event.FEventMana);
            Heroes?.Dispose();
            Result = false;
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // GlobalSpells.pas:1062
    public override bool Activate(AbstractUnit Source)
    {
        bool Result;
        string Reason = "";
        SpellTE Event;
        base.Activate(Source);
        if (CanActivate(Source, ref Reason) && !AoWHSMap.Current.FPlayers.GetPlayers(Source.FPlayer).GetBusy())
        {
            Event = new SpellTE(null);
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

    private static readonly string NoJoinResurrectionRStr = "I needed that... Goodbye.";
    private static readonly string ResurrectHeroRStr = "Resurrect Hero";
    private static readonly string XResurrectedRStr = "%s resurrected";
}
