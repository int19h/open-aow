// Translated from GlobalSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

namespace Aow.Game.Magic.Spells;

// inherited name, spell ID, mana, sphere, level, spell type
public partial class BirdsView : Spell
{
    // GlobalSpells.pas:489
    public BirdsView(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(BirdsViewRStr);
        FSpellID = 2;
        FSpellType = SpellType.Global;
        FBaseManaCost = 5;
        FSpellLevel = 1;
        FMagicSphere = MagicSphere.Air;
    }

    // GlobalSpells.pas:505
    public override bool ExecuteTE(SpellTokenEvent Event, Troop Source, int ExecuteCount)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte Layer = default;
        base.ExecuteTE(Event, Source, ExecuteCount);
        CastingDone(Source, Event.FEventMana);
        Source.GetLocation(ref X, ref Y, ref Layer);
        if (AoWHSMap.Current.FSeatedPlayer == Source.FPlayer)
        {
            var birdsViewHS = new BirdsViewHexSprite(null);
            birdsViewHS.StartAnimation(X, Y, Layer, 14);
        }

        AoWHSMap.Current.ExploreArea(X, Y, Layer, Source.FPlayer, 14);
        return false;
    }

    // GlobalSpells.pas:500
    public override bool GetEnabled() => AoWHSMap.Current.FExplorationEnabled;

    // GlobalSpells.pas:518
    public override bool CanActivate(Troop Source, ref string MessageText)
    {
        sbyte X = default;
        sbyte Y = default;
        sbyte Layer = default;
        bool Result = base.CanActivate(Source, ref MessageText);
        if (Result)
        {
            Source.GetLocation(ref X, ref Y, ref Layer);
            if (Layer != 0)
            {
                MessageText = AbilityRules.TranslateRStr(GameText.CannotCastSpellInUndergoundRStr);
                Result = false;
            }
        }

        return Result;
    }

    // GlobalSpells.pas:534
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

    private static readonly string BirdsViewRStr = "Bird's View";
}
