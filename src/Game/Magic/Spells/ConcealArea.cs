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
        int x = coordinate60.X;
        int y = coordinate60.Y;
        int layer = coordinate60.MapLevel;
        if (source.Player == GetEngine().StrategicMap.SeatedPlayer)
        {
            ConcealAreaHexSprite conceal = new(null) { Holder = CreationHolder() };
            conceal.StartAnimation(x, y, layer, radius: 14);
        }

        for (int player = 1, last = GetEngine().StrategicMap.Players.Count - 1; player <= last; player++)
        {
            StrategicMap strategicMap = GetEngine().StrategicMap;
            MapLevel level = strategicMap.MapContainer.MapLevels[layer];
            LocationTextEventLog? log;
            if (strategicMap.Players.GetDiplomaticRelation(source.Player, otherPlayer: PlayerId.FromOriginal((sbyte)player)) == DiplomaticRelation.War && player != PlayerIdExtensions.ToOriginal(source.Player))
            {
                log = null;
                HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, x, y, radius: 0);
                while (walk.Radius <= 14)
                {
                    if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
                    {
                        MapField mapField = level.GetField(walk.X, walk.Y);
                        if (mapField.GetExplored(PlayerId.FromOriginal(player)) && !GetEngine().StrategicMap.VisibleForPlayer(walk.X, walk.Y, layer, player: PlayerId.FromOriginal((sbyte)player)))
                        {
                            mapField.SetExplored(PlayerId.FromOriginal(player), value: false);
                            if (log is null)
                            {
                                log = new LocationTextEventLog(null)
                                {
                                    Holder = CreationHolder(),
                                    Location = new MapCoordinate((sbyte)walk.X, (sbyte)walk.Y, (sbyte)layer),
                                    Player = source.Player
                                };
                                string concealArea = AbilityRules.TranslateResourceString(GetEngine(), S: s_concealAreaText);
                                log.Text = DelphiFormat.Format(AbilityRules.TranslateResourceString(GetEngine(), S: GameText.CastFormat), args: [concealArea]);
                                GetEngine().StrategicMap.Players.GetPlayers(player).EventLogbook.AddEvent(log);
                                _ = log.Release();
                            }
                        }
                    }

                    HexNumberWalkData.WalkToNextHexNumber(ref walk);
                }
            }
            else
            {
                if (GetEngine().StrategicMap.VisibleForPlayer(x, y, layer, player: PlayerId.FromOriginal((sbyte)player)))
                {
                    log = new LocationTextEventLog(null)
                    {
                        Holder = CreationHolder(),
                        Location = new MapCoordinate((sbyte)x, (sbyte)y, (sbyte)layer),
                        Player = source.Player
                    };
                    string concealArea = AbilityRules.TranslateResourceString(GetEngine(), S: s_concealAreaText);
                    log.Text = DelphiFormat.Format(AbilityRules.TranslateResourceString(GetEngine(), S: GameText.CastFormat), args: [concealArea]);
                    GetEngine().StrategicMap.Players.GetPlayers(player).EventLogbook.AddEvent(log);
                    _ = log.Release();
                }
            }
        }

        return false;
    }

    // GlobalSpells.pas:297
    public override bool Enabled => GetEngine().StrategicMap.ExplorationEnabled;

    // GlobalSpells.pas:368
    public override bool CanActivate(Troop source, ref string messageText)
    {
        bool result = base.CanActivate(source, ref messageText);
        if (result)
        {
            bool war = false;
            StrategicMap strategicMap = GetEngine().StrategicMap;
            for (int player = 1, last = strategicMap.Players.Count - 1; player <= last; player++)
            {
                war = war || strategicMap.Players.GetPlayers(source.Player).DiplomaticRelations.GetDiplomaticRelation(PlayerId.FromOriginal((sbyte)player)) == DiplomaticRelation.War;
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
        if (CanActivate(source, messageText: ref reason) && !GetEngine().StrategicMap.Players.GetPlayers(source.Player).GetBusy())
        {
            SpellTokenEvent eventValue = new(null) { Holder = CreationHolder() };
            eventValue.Initialize(SpellId, playerIndex: PlayerId.FromOriginal((byte)PlayerIdExtensions.ToOriginal(source.Player)), sourceId: source.UnitId, mana: BaseManaCost);
            GetEngine().StrategicMap.TokenManager.GetTokenControl(source.Player).AddEvent(eventValue);
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
