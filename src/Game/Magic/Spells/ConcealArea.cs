// Translated from GlobalSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Events;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

// inherited name, spell ID, mana, sphere, level, type
public partial class ConcealArea : Spell
{
    // GlobalSpells.pas:286
    public ConcealArea(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(ConcealAreaRStr);
        FSpellID = 1;
        FSpellType = SpellType.Global;
        FBaseManaCost = 5;
        FSpellLevel = 1;
        FMagicSphere = MagicSphere.Earth;
    }

    // GlobalSpells.pas:302
    public override bool ExecuteTE(SpellTokenEvent Event, Troop Source, int ExecuteCount)
    {
        AoWMapField MapF;
        sbyte X = default;
        sbyte Y = default;
        sbyte Layer = default;
        HexNumberWalkData Walk = default;
        MapLevel Level;
        LocationTextEventLog Log;
        ConcealAreaHexSprite Conceal;
        base.ExecuteTE(Event, Source, ExecuteCount);
        CastingDone(Source, Event.FEventMana);
        Source.GetLocation(ref X, ref Y, ref Layer);
        if (Source.FPlayer == AoWHSMap.Current.FSeatedPlayer)
        {
            Conceal = new ConcealAreaHexSprite(null);
            Conceal.StartAnimation(X, Y, Layer, 14);
        }

        for (int Player = 1, last = AoWHSMap.Current.FPlayers.Count - 1; Player <= last; Player++)
        {
            Level = AoWHSMap.Current.MapContainer.MapLevels[Layer];
            if (AoWHSMap.Current.FPlayers.GetDiplomaticRelation(Source.FPlayer, (sbyte)Player) == DiplomaticRelation.War && Player != Source.FPlayer)
            {
                Log = null;
                HexNumberWalkData.InitHNWalk(ref Walk, X, Y, 0);
                while (Walk.Radius <= 14)
                {
                    if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
                    {
                        MapF = (AoWMapField)Level.GetField(Walk.X, Walk.Y);
                        if (MapF.GetExplored(Player) && !AoWHSMap.Current.VisibleForPlayer(Walk.X, Walk.Y, Layer, (sbyte)Player))
                        {
                            MapF.SetExplored(Player, false);
                            if (Log == null)
                            {
                                Log = new LocationTextEventLog(null);
                                Log.FXhx = (sbyte)Walk.X;
                                Log.FYhx = (sbyte)Walk.Y;
                                Log.FLhx = Layer;
                                Log.FPlayer = Source.FPlayer;
                                var concealArea = AbilityRules.TranslateRStr(ConcealAreaRStr);
                                Log.FText = RtlUnit.Format(AbilityRules.TranslateRStr(GameText.XCastRStr), [concealArea]);
                                AoWHSMap.Current.FPlayers.GetPlayers(Player).FEventLogbook.AddEvent(Log);
                                Log.Release();
                            }
                        }
                    }

                    HexNumberWalkData.WalkToNextHN(ref Walk);
                }
            }
            else
            {
                if (AoWHSMap.Current.VisibleForPlayer(X, Y, Layer, (sbyte)Player))
                {
                    Log = new LocationTextEventLog(null);
                    Log.FXhx = X;
                    Log.FYhx = Y;
                    Log.FLhx = Layer;
                    Log.FPlayer = Source.FPlayer;
                    var concealArea = AbilityRules.TranslateRStr(ConcealAreaRStr);
                    Log.FText = RtlUnit.Format(AbilityRules.TranslateRStr(GameText.XCastRStr), [concealArea]);
                    AoWHSMap.Current.FPlayers.GetPlayers(Player).FEventLogbook.AddEvent(Log);
                    Log.Release();
                }
            }
        }

        return false;
    }

    // GlobalSpells.pas:297
    public override bool GetEnabled() => AoWHSMap.Current.FExplorationEnabled;

    // GlobalSpells.pas:368
    public override bool CanActivate(Troop Source, ref string MessageText)
    {
        bool War;
        bool Result = base.CanActivate(Source, ref MessageText);
        if (Result)
        {
            War = false;
            for (int Player = 1, last = AoWHSMap.Current.FPlayers.Count - 1; Player <= last; Player++)
            {
                War = War || AoWHSMap.Current.FPlayers.GetPlayers(Source.FPlayer).FDiplomaticRelations.GetDiplomaticRelation((sbyte)Player) == DiplomaticRelation.War;
            }

            if (!War)
            {
                var concealArea = AbilityRules.TranslateRStr(ConcealAreaRStr);
                MessageText = RtlUnit.Format(AbilityRules.TranslateRStr(AWarRelationIsRequiredForXRStr), [concealArea]);
                Result = false;
            }
        }

        return Result;
    }

    // GlobalSpells.pas:388
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

    private static readonly string AWarRelationIsRequiredForXRStr = "A state of WAR is required for %s";
    private static readonly string ConcealAreaRStr = "Conceal Area";
}
