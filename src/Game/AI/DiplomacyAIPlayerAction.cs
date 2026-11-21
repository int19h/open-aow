#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.World.Map;

using static Aow.Rtl.DelphiRandom;

namespace Aow.Game.AI;

public class DiplomacyAIPlayerAction(CustomNode owner) : AIPlayerAction(owner), IAIPlayerActionClass
{
    private sbyte _nextPlayer;

    // Diplomacy.pas:1549
    public override bool Activate()
    {
        bool result = base.Activate();
        _nextPlayer = 0;
        return result;
    }

    // Diplomacy.pas:1699
    public override bool Process()
    {
        _ = base.Process();
        if (Control.Player.PlayerIndex != 0)
        {
            while (true)
            {
                int count = StrategicMap.Current.Players.Count;
                if (!(_nextPlayer < count && !Control.Player.GetBusy()))
                {
                    break;
                }

                _nextPlayer = (sbyte)(_nextPlayer + 1);
                while (true)
                {
                    int fPlayersCount = StrategicMap.Current.Players.Count;
                    if (!(_nextPlayer < fPlayersCount && (Control.Player.GetFirstDaySeen((sbyte)(byte)_nextPlayer) == -1 || Control.Player.PlayerIndex == _nextPlayer)))
                    {
                        break;
                    }

                    _nextPlayer = (sbyte)(_nextPlayer + 1);
                }

                int count2 = StrategicMap.Current.Players.Count;
                if (_nextPlayer < count2)
                {
                    _ = ExecuteDiplomacy(_nextPlayer);
                }
            }

            int count3 = StrategicMap.Current.Players.Count;
            return _nextPlayer < count3;
        }
        else
        {
            return false;
        }
    }

    // Diplomacy.pas:1552
    public DiplomaticAction? CreateDA(DiplomaticProposal action, sbyte target, sbyte other, int minDays)
    {
        sbyte player = Control.Player.PlayerIndex;
        DiplomaticAction last = StrategicMap.Current.Players.GetPlayers(player).DiplomaticActions.FindLastAction(action.ToFields().Kind, reaction: DiplomaticActionReaction.None, source: player, target, other);
        return last is null || StrategicMap.Current.Day - last.Day >= minDays
            ? StrategicMap.Current.Players.GetPlayers(player).CreateDiplomaticAction(action, target, other)
            : null;
    }

    // Diplomacy.pas:1563
    public bool ExecuteDiplomacy(sbyte player)
    {
        // { Original bug: the result remains False even when an action is sent. }
        DiplomaticAction? action = null;
        sbyte source = Control.Player.PlayerIndex;
        ByteList idealPeace = new();
        ByteList idealWar = new();
        ByteList peace = new();
        ByteList war = new();
        ByteList unknown = new();
        Control.ListPeaceWarUnknownPlayers(peace, war, unknown);
        Control.ListIdealPeaceWarPlayers(idealPeace, war: idealWar);
        int index;
        switch (StrategicMap.Current.Players.GetDiplomaticRelation((sbyte)(byte)source, otherPlayer: (sbyte)(byte)player))
        {
            case DiplomaticRelation.Unknown:
                if (idealPeace.IndexOf((byte)player) != -1)
                {
                    action = Control.CanOfferPeace((byte)player)
                        ? CreateDA(new OfferPeace(), target: player, other: -1, minDays: 10)
                        : CreateDA(new DeclareWar(), target: player, other: -1, minDays: -1);
                }

                if (idealWar.IndexOf((byte)player) != -1)
                {
                    action = Control.CanDeclareWar((byte)player)
                        ? CreateDA(new DeclareWar(), target: player, other: -1, minDays: -1)
                        : CreateDA(new OfferPeace(), target: player, other: -1, minDays: 10);
                }

                break;
            case DiplomaticRelation.Peace:
                if (!Control.CanOfferPeace((byte)player) && idealWar.IndexOf((byte)player) != -1)
                {
                    RandSeed = StrategicMap.Current.InitialRandomSeed + StrategicMap.Current.Day + source;
                    index = Random(idealWar.Count);
                    if (idealWar[index] == player)
                    {
                        action = CreateDA(new DeclareWar(), target: player, other: -1, minDays: -1);
                    }
                }
                else
                {
                    if (Control.CanProposeAlliance((byte)player))
                    {
                        RandSeed = (StrategicMap.Current.InitialRandomSeed + StrategicMap.Current.Day + source) >>> 2;
                        if (Random(100) < 30)
                        {
                            action = CreateDA(new ProposeAlliance(), target: player, other: -1, minDays: 10);
                        }
                    }
                }

                break;
            case DiplomaticRelation.Alliance:
                if (!Control.CanOfferPeace((byte)player) && idealWar.IndexOf((byte)player) != -1)
                {
                    RandSeed = StrategicMap.Current.InitialRandomSeed + StrategicMap.Current.Day + source + 1;
                    index = Random(idealWar.Count);
                    if (idealWar[index] == player)
                    {
                        action = CreateDA(new BreakAlliance(), target: player, other: -1, minDays: -1);
                    }
                }

                break;
            case DiplomaticRelation.War:
                if (!Control.CanDeclareWar((byte)player))
                {
                    DoubleItemList candidates = new();
                    int score;
                    sbyte candidate;
                    for (int i = 0, last = idealPeace.Count - 1; i <= last; i++)
                    {
                        candidate = (sbyte)idealPeace[i];
                        if (war.IndexOf((byte)candidate) != -1 && Control.CanOfferPeace((byte)candidate))
                        {
                            action = StrategicMap.Current.Players.GetPlayers(source).DiplomaticActions.FindLastAction(OfferPeace.Kind, reaction: DiplomaticActionReaction.None, source, target: candidate, other: -1);
                            if (action is null || StrategicMap.Current.Day - action.Day > 12)
                            {
                                score = -i;
                                score -= StrategicMap.Current.Players.GetDiplomaticRelationCount((sbyte)(byte)candidate, relations: DiplomaticRelations.Peace) * 5;
                                score -= StrategicMap.Current.Players.GetDiplomaticRelationCount((sbyte)(byte)candidate, relations: DiplomaticRelations.Alliance) * 10;
                                _ = candidates.AddAbility(candidate, score);
                            }

                            action = null;
                        }
                    }

                    if (candidates.Count == 0 && war.Count - peace.Count > 2)
                    {
                        for (int j = 0, last = idealWar.Count - 1; j <= last; j++)
                        {
                            candidate = (sbyte)idealWar[j];
                            if (war.IndexOf((byte)candidate) != -1)
                            {
                                action = StrategicMap.Current.Players.GetPlayers(source).DiplomaticActions.FindLastAction(OfferPeace.Kind, reaction: DiplomaticActionReaction.None, source, target: candidate, other: -1);
                                if (action is null || StrategicMap.Current.Day - action.Day > 12)
                                {
                                    score = j;
                                    score -= StrategicMap.Current.Players.GetDiplomaticRelationCount((sbyte)(byte)candidate, relations: DiplomaticRelations.Peace) * 5;
                                    score -= StrategicMap.Current.Players.GetDiplomaticRelationCount((sbyte)(byte)candidate, relations: DiplomaticRelations.Alliance) * 10;
                                    _ = candidates.AddAbility(candidate, score);
                                }

                                action = null;
                            }
                        }
                    }

                    if (candidates.Count != 0)
                    {
                        candidates.SortDescendingItemB();
                        if (candidates[0].First == player)
                        {
                            action = CreateDA(new OfferPeace(), target: (sbyte)candidates[0].First, other: -1, minDays: -1);
                        }
                    }

                    candidates?.Dispose();
                }

                break;
            default:
                break;
        }

        if (action is not null)
        {
            string errorText = "";
            _ = action.Send(ref errorText);
            _ = action.Release();
        }

        peace?.Dispose();
        war?.Dispose();
        unknown?.Dispose();
        idealPeace?.Dispose();
        idealWar?.Dispose();
        return false;
    }

    // Diplomacy.pas:1670
    public void ListDebugInfo()
    {
        ByteList idealPeace = new();
        ByteList idealWar = new();
        Control.ListIdealPeaceWarPlayers(idealPeace, war: idealWar);
        int player = Control.Player.PlayerIndex;
        Player? players = StrategicMap.Current.Players.GetPlayers(player);
        string s = AbilityRules.TranslateResourceString(GetEngine(), S: GameTables.RaceNamesPluralTexts[players.PlayerRace]);
        string name = StrategicMap.Current.Players.GetPlayers(player).GetName();
        s = $"{s}/{name}";
        GetEngine().OutputDebugMessage(DebugChannel.AI, S: $"DIPLOMACY ({s})");
        GetEngine().OutputDebugMessage(DebugChannel.AI, S: "IDEAL PEACE PLAYERS");
        for (int i = 0, last = idealPeace.Count - 1; i <= last; i++)
        {
            byte item = idealPeace[i];
            Player? fPlayersPlayers = StrategicMap.Current.Players.GetPlayers(item);
            s = AbilityRules.TranslateResourceString(GetEngine(), S: GameTables.RaceNamesPluralTexts[fPlayersPlayers.PlayerRace]);
            byte idealPeaceItem = idealPeace[i];
            string name2 = StrategicMap.Current.Players.GetPlayers(idealPeaceItem).GetName();
            s = $"{s}/{name2}";
            GetEngine().OutputDebugMessage(DebugChannel.AI, s);
        }

        GetEngine().OutputDebugMessage(DebugChannel.AI, S: "IDEAL WAR PLAYERS");
        for (int i = 0, last = idealWar.Count - 1; i <= last; i++)
        {
            byte item = idealWar[i];
            string name2 = StrategicMap.Current.Players.GetPlayers(item).GetName();
            GetEngine().OutputDebugMessage(DebugChannel.AI, S: name2);
        }

        idealPeace?.Dispose();
        idealWar?.Dispose();
    }

    // Diplomacy.pas:1546
    public static new int Priority => 14;

}
