#nullable disable warnings
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
public class ConcealArea : Spell
{
    // GlobalSpells.pas:286
    public ConcealArea(CustomNode? owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: s_concealAreaText);
        SpellId = 1;
        SpellType = SpellType.Global;
        BaseManaCost = 5;
        SpellLevel = 1;
        MagicSphere = MagicSphere.Earth;
    }

    // GlobalSpells.pas:302
    public override bool ExecuteTokenEvent(SpellTokenEvent eventValue, Troop source, int executeCount)
    {
        _ = base.ExecuteTokenEvent(eventValue, source, executeCount);
        CastingDone(source, mana: eventValue.EventMana);
        MapCoordinate coordinate60 = source.GetLocation();
        sbyte x = coordinate60.X;
        sbyte y = coordinate60.Y;
        sbyte layer = coordinate60.MapLevel;
        if (source.Player == StrategicMap.Current.SeatedPlayer)
        {
            ConcealAreaHexSprite conceal = new(null) { Holder = CreationHolder() };
            conceal.StartAnimation(x, y, layer, radius: 14);
        }

        for (int player = 1, last = StrategicMap.Current.Players.Count - 1; player <= last; player++)
        {
            MapLevel level = StrategicMap.Current.MapContainer.MapLevels[layer];
            LocationTextEventLog? log;
            if (StrategicMap.Current.Players.GetDiplomaticRelation(source.Player, otherPlayer: (sbyte)player) == DiplomaticRelation.War && player != source.Player)
            {
                log = null;
                HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, x, y, radius: 0);
                while (walk.Radius <= 14)
                {
                    if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
                    {
                        MapField mapField = level.GetField(walk.X, walk.Y);
                        if (mapField.GetExplored(player) && !StrategicMap.Current.VisibleForPlayer(walk.X, walk.Y, layer, player: (sbyte)player))
                        {
                            mapField.SetExplored(player, value: false);
                            if (log is null)
                            {
                                log = new LocationTextEventLog(null)
                                {
                                    Holder = CreationHolder(),
                                    Location = new MapCoordinate((sbyte)walk.X, (sbyte)walk.Y, layer),
                                    Player = source.Player
                                };
                                string concealArea = AbilityRules.TranslateResourceString(GetEngine(), S: s_concealAreaText);
                                log.Text = DelphiFormat.Format(AbilityRules.TranslateResourceString(GetEngine(), S: GameText.CastFormat), args: [concealArea]);
                                StrategicMap.Current.Players.GetPlayers(player).EventLogbook.AddEvent(log);
                                _ = log.Release();
                            }
                        }
                    }

                    HexNumberWalkData.WalkToNextHexNumber(ref walk);
                }
            }
            else
            {
                if (StrategicMap.Current.VisibleForPlayer(x, y, layer, player: (sbyte)player))
                {
                    log = new LocationTextEventLog(null)
                    {
                        Holder = CreationHolder(),
                        Location = new MapCoordinate(x, y, layer),
                        Player = source.Player
                    };
                    string concealArea = AbilityRules.TranslateResourceString(GetEngine(), S: s_concealAreaText);
                    log.Text = DelphiFormat.Format(AbilityRules.TranslateResourceString(GetEngine(), S: GameText.CastFormat), args: [concealArea]);
                    StrategicMap.Current.Players.GetPlayers(player).EventLogbook.AddEvent(log);
                    _ = log.Release();
                }
            }
        }

        return false;
    }

    // GlobalSpells.pas:297
    public override bool Enabled => StrategicMap.Current.ExplorationEnabled;

    // GlobalSpells.pas:368
    public override bool CanActivate(Troop source, ref string messageText)
    {
        bool result = base.CanActivate(source, ref messageText);
        if (result)
        {
            bool war = false;
            for (int player = 1, last = StrategicMap.Current.Players.Count - 1; player <= last; player++)
            {
                war = war || StrategicMap.Current.Players.GetPlayers(source.Player).DiplomaticRelations.GetDiplomaticRelation((sbyte)player) == DiplomaticRelation.War;
            }

            if (!war)
            {
                string concealArea = AbilityRules.TranslateResourceString(GetEngine(), S: s_concealAreaText);
                messageText = DelphiFormat.Format(AbilityRules.TranslateResourceString(GetEngine(), S: s_warRelationIsRequiredFormat), args: [concealArea]);
                return false;
            }
        }

        return result;
    }

    // GlobalSpells.pas:388
    public override bool Activate(Troop source)
    {
        string reason = "";
        _ = base.Activate(source);
        if (CanActivate(source, messageText: ref reason) && !StrategicMap.Current.Players.GetPlayers(source.Player).GetBusy())
        {
            SpellTokenEvent eventValue = new(null) { Holder = CreationHolder() };
            eventValue.Initialize(SpellId, playerIndex: (byte)source.Player, sourceId: source.UnitId, mana: BaseManaCost);
            StrategicMap.Current.TokenManager.GetTokenControl(source.Player).AddEvent(eventValue);
            _ = eventValue.Release();
            return true;
        }
        else
        {
            return false;
        }
    }

    private static readonly string s_warRelationIsRequiredFormat = "A state of WAR is required for %s";
    private static readonly string s_concealAreaText = "Conceal Area";
}
