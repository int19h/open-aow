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
        SpellName = AbilityRules.TranslateResourceString(s_concealAreaText);
        SpellId = 1;
        SpellType = SpellType.Global;
        BaseManaCost = 5;
        SpellLevel = 1;
        MagicSphere = MagicSphere.Earth;
    }

    // GlobalSpells.pas:302
    public override bool ExecuteTokenEvent(SpellTokenEvent eventValue, Troop source, int executeCount)
    {
        AoWMapField mapField;
        sbyte X = default;
        sbyte Y = default;
        sbyte layer = default;
        HexNumberWalkData walk = default;
        MapLevel level;
        LocationTextEventLog log;
        ConcealAreaHexSprite conceal;
        base.ExecuteTokenEvent(eventValue, source, executeCount);
        CastingDone(source, eventValue.EventMana);
        source.GetLocation(ref X, ref Y, ref layer);
        if (source.Player == AoWHSMap.Current.SeatedPlayer)
        {
            conceal = new ConcealAreaHexSprite(null);
            conceal.StartAnimation(X, Y, layer, 14);
        }

        for (int player = 1, last = AoWHSMap.Current.Players.Count - 1; player <= last; player++)
        {
            level = AoWHSMap.Current.MapContainer.MapLevels[layer];
            if (AoWHSMap.Current.Players.GetDiplomaticRelation(source.Player, (sbyte)player) == DiplomaticRelation.War && player != source.Player)
            {
                log = null;
                HexNumberWalkData.InitializeHexNumberWalk(ref walk, X, Y, 0);
                while (walk.Radius <= 14)
                {
                    if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
                    {
                        mapField = (AoWMapField)level.GetField(walk.X, walk.Y);
                        if (mapField.GetExplored(player) && !AoWHSMap.Current.VisibleForPlayer(walk.X, walk.Y, layer, (sbyte)player))
                        {
                            mapField.SetExplored(player, false);
                            if (log == null)
                            {
                                log = new LocationTextEventLog(null);
                                log.HexX = (sbyte)walk.X;
                                log.HexY = (sbyte)walk.Y;
                                log.HexLevel = layer;
                                log._player = source.Player;
                                var concealArea = AbilityRules.TranslateResourceString(s_concealAreaText);
                                log.Text = RtlUnit.Format(AbilityRules.TranslateResourceString(GameText.s_castFormat), [concealArea]);
                                AoWHSMap.Current.Players.GetPlayers(player).EventLogbook.AddEvent(log);
                                log.Release();
                            }
                        }
                    }

                    HexNumberWalkData.WalkToNextHexNumber(ref walk);
                }
            }
            else
            {
                if (AoWHSMap.Current.VisibleForPlayer(X, Y, layer, (sbyte)player))
                {
                    log = new LocationTextEventLog(null);
                    log.HexX = X;
                    log.HexY = Y;
                    log.HexLevel = layer;
                    log._player = source.Player;
                    var concealArea = AbilityRules.TranslateResourceString(s_concealAreaText);
                    log.Text = RtlUnit.Format(AbilityRules.TranslateResourceString(GameText.s_castFormat), [concealArea]);
                    AoWHSMap.Current.Players.GetPlayers(player).EventLogbook.AddEvent(log);
                    log.Release();
                }
            }
        }

        return false;
    }

    // GlobalSpells.pas:297
    public override bool GetEnabled() => AoWHSMap.Current.ExplorationEnabled;

    // GlobalSpells.pas:368
    public override bool CanActivate(Troop source, ref string messageText)
    {
        bool war;
        bool result = base.CanActivate(source, ref messageText);
        if (result)
        {
            war = false;
            for (int player = 1, last = AoWHSMap.Current.Players.Count - 1; player <= last; player++)
            {
                war = war || AoWHSMap.Current.Players.GetPlayers(source.Player).DiplomaticRelations.GetDiplomaticRelation((sbyte)player) == DiplomaticRelation.War;
            }

            if (!war)
            {
                var concealArea = AbilityRules.TranslateResourceString(s_concealAreaText);
                messageText = RtlUnit.Format(AbilityRules.TranslateResourceString(s_warRelationIsRequiredFormat), [concealArea]);
                result = false;
            }
        }

        return result;
    }

    // GlobalSpells.pas:388
    public override bool Activate(Troop source)
    {
        bool result;
        string reason = "";
        SpellTokenEvent eventValue;
        base.Activate(source);
        if (CanActivate(source, ref reason) && !AoWHSMap.Current.Players.GetPlayers(source.Player).GetBusy())
        {
            eventValue = new SpellTokenEvent(null);
            eventValue.Initialize(SpellId, (byte)source.Player, source.UnitId, BaseManaCost);
            AoWHSMap.Current.TokenManager.GetTokenControl(source.Player).AddEvent(eventValue);
            eventValue.Release();
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    private static readonly string s_warRelationIsRequiredFormat = "A state of WAR is required for %s";
    private static readonly string s_concealAreaText = "Conceal Area";
}
