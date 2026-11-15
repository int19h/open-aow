#nullable disable
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
    public ConcealArea(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), s_concealAreaText);
        SpellId = 1;
        SpellType = SpellType.Global;
        BaseManaCost = 5;
        SpellLevel = 1;
        MagicSphere = MagicSphere.Earth;
    }

    // GlobalSpells.pas:302
    public override bool ExecuteTokenEvent(SpellTokenEvent eventValue, Troop source, int executeCount)
    {
        MapField mapField;
        sbyte x = default;
        sbyte y = default;
        sbyte layer = default;
        HexNumberWalkData walk = default;
        MapLevel level;
        LocationTextEventLog log;
        ConcealAreaHexSprite conceal;
        base.ExecuteTokenEvent(eventValue, source, executeCount);
        CastingDone(source, eventValue.EventMana);
        MapCoordinate coordinate60 = source.GetLocation();
        x = coordinate60.X;
        y = coordinate60.Y;
        layer = coordinate60.MapLevel;
        if (source.Player == StrategicMap.Current.SeatedPlayer)
        {
            conceal = new ConcealAreaHexSprite(null) { Holder = CreationHolder() };
            conceal.StartAnimation(x, y, layer, 14);
        }

        for (int player = 1, last = StrategicMap.Current.Players.Count - 1; player <= last; player++)
        {
            level = StrategicMap.Current.MapContainer.MapLevels[layer];
            if (StrategicMap.Current.Players.GetDiplomaticRelation(source.Player, (sbyte)player) == DiplomaticRelation.War && player != source.Player)
            {
                log = null;
                HexNumberWalkData.InitializeHexNumberWalk(out walk, x, y, 0);
                while (walk.Radius <= 14)
                {
                    if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
                    {
                        mapField = level.GetField(walk.X, walk.Y);
                        if (mapField.GetExplored(player) && !StrategicMap.Current.VisibleForPlayer(walk.X, walk.Y, layer, (sbyte)player))
                        {
                            mapField.SetExplored(player, false);
                            if (log == null)
                            {
                                log = new LocationTextEventLog(null) { Holder = CreationHolder() };
                                log.Location = new MapCoordinate((sbyte)walk.X, (sbyte)walk.Y, layer);
                                log.Player = source.Player;
                                string concealArea = AbilityRules.TranslateResourceString(GetEngine(), s_concealAreaText);
                                log.Text = RtlUnit.Format(AbilityRules.TranslateResourceString(GetEngine(), GameText.CastFormat), [concealArea]);
                                StrategicMap.Current.Players.GetPlayers(player).EventLogbook.AddEvent(log);
                                log.Release();
                            }
                        }
                    }

                    HexNumberWalkData.WalkToNextHexNumber(ref walk);
                }
            }
            else
            {
                if (StrategicMap.Current.VisibleForPlayer(x, y, layer, (sbyte)player))
                {
                    log = new LocationTextEventLog(null) { Holder = CreationHolder() };
                    log.Location = new MapCoordinate(x, y, layer);
                    log.Player = source.Player;
                    string concealArea = AbilityRules.TranslateResourceString(GetEngine(), s_concealAreaText);
                    log.Text = RtlUnit.Format(AbilityRules.TranslateResourceString(GetEngine(), GameText.CastFormat), [concealArea]);
                    StrategicMap.Current.Players.GetPlayers(player).EventLogbook.AddEvent(log);
                    log.Release();
                }
            }
        }

        return false;
    }

    // GlobalSpells.pas:297
    public override bool GetEnabled() => StrategicMap.Current.ExplorationEnabled;

    // GlobalSpells.pas:368
    public override bool CanActivate(Troop source, ref string messageText)
    {
        bool war;
        bool result = base.CanActivate(source, ref messageText);
        if (result)
        {
            war = false;
            for (int player = 1, last = StrategicMap.Current.Players.Count - 1; player <= last; player++)
            {
                war = war || StrategicMap.Current.Players.GetPlayers(source.Player).DiplomaticRelations.GetDiplomaticRelation((sbyte)player) == DiplomaticRelation.War;
            }

            if (!war)
            {
                string concealArea = AbilityRules.TranslateResourceString(GetEngine(), s_concealAreaText);
                messageText = RtlUnit.Format(AbilityRules.TranslateResourceString(GetEngine(), s_warRelationIsRequiredFormat), [concealArea]);
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
        if (CanActivate(source, ref reason) && !StrategicMap.Current.Players.GetPlayers(source.Player).GetBusy())
        {
            eventValue = new SpellTokenEvent(null) { Holder = CreationHolder() };
            eventValue.Initialize(SpellId, (byte)source.Player, source.UnitId, BaseManaCost);
            StrategicMap.Current.TokenManager.GetTokenControl(source.Player).AddEvent(eventValue);
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
