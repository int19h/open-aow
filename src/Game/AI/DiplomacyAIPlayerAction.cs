#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.World.Map;
using Aow.Rtl;

using static Aow.Rtl.DelphiRandom;

namespace Aow.Game.AI;

public class DiplomacyAIPlayerAction(CustomNode owner) : AIPlayerAction(owner), IAIPlayerActionClass
{
    private PlayerId? _nextPlayer = PlayerId.Independent;

    // Diplomacy.pas:1549
    public override bool Activate()
    {
        bool result = base.Activate();
        _nextPlayer = PlayerId.Independent;
        return result;
    }

    // Diplomacy.pas:1699
    public override bool Process()
    {
        _ = base.Process();
        if (Control.Player.PlayerIndex != PlayerId.Independent)
        {
            while (true)
            {
                StrategicMap strategicMap = GetEngine().StrategicMap;
                int count = strategicMap.Players.Count;
                if (!(PlayerIdExtensions.ToOriginal(_nextPlayer) < count && !Control.Player.GetBusy()))
                {
                    break;
                }

                _nextPlayer = PlayerId.FromOriginal((sbyte)(PlayerIdExtensions.ToOriginal(_nextPlayer) + 1));
                while (true)
                {
                    int fPlayersCount = strategicMap.Players.Count;
                    if (!(PlayerIdExtensions.ToOriginal(_nextPlayer) < fPlayersCount && (Control.Player.GetFirstDaySeen(PlayerId.FromOriginal((sbyte)(byte)PlayerIdExtensions.ToOriginal(_nextPlayer))) == -1 || Control.Player.PlayerIndex == _nextPlayer)))
                    {
                        break;
                    }

                    _nextPlayer = PlayerId.FromOriginal((sbyte)(PlayerIdExtensions.ToOriginal(_nextPlayer) + 1));
                }

                int count2 = strategicMap.Players.Count;
                if (PlayerIdExtensions.ToOriginal(_nextPlayer) < count2)
                {
                    _ = ExecuteDiplomacy(_nextPlayer);
                }
            }

            int count3 = GetEngine().StrategicMap.Players.Count;
            return PlayerIdExtensions.ToOriginal(_nextPlayer) < count3;
        }
        else
        {
            return false;
        }
    }

    // Diplomacy.pas:1552
    public DiplomaticAction? CreateDA(DiplomaticProposal action, PlayerId? target, int other, int minDays)
    {
        sbyte player = (sbyte)PlayerIdExtensions.ToOriginal(Control.Player.PlayerIndex);
        DiplomaticAction last = GetEngine().StrategicMap.Players.GetPlayers(player).DiplomaticActions.FindLastAction(action.ToFields().Kind, reaction: DiplomaticActionReaction.None, source: PlayerId.FromOriginal(player), target, other);
        return last is null || GetEngine().StrategicMap.Day - last.Day >= minDays
            ? GetEngine().StrategicMap.Players.GetPlayers(player).CreateDiplomaticAction(action, target, other: PlayerId.FromOriginal(other))
            : null;
    }

    // Diplomacy.pas:1563
    public bool ExecuteDiplomacy(PlayerId? player)
    {
        // { Original bug: the result remains False even when an action is sent. }
        DiplomaticAction? action = null;
        int source = PlayerIdExtensions.ToOriginal(Control.Player.PlayerIndex);
        ByteList idealPeace = [];
        ByteList idealWar = [];
        ByteList peace = [];
        ByteList war = [];
        ByteList unknown = [];
        Control.ListPeaceWarUnknownPlayers(peace, war, unknown);
        Control.ListIdealPeaceWarPlayers(idealPeace, war: idealWar);
        int index;
        switch (GetEngine().StrategicMap.Players.GetDiplomaticRelation(PlayerId.FromOriginal((sbyte)(byte)source), otherPlayer: PlayerId.FromOriginal((sbyte)(byte)PlayerIdExtensions.ToOriginal(player))))
        {
            case DiplomaticRelation.Unknown:
                if (idealPeace.IndexOf((byte)PlayerIdExtensions.ToOriginal(player)) != -1)
                {
                    action = Control.CanOfferPeace(PlayerId.FromOriginal((byte)PlayerIdExtensions.ToOriginal(player)))
                        ? CreateDA(new OfferPeace(), target: player, other: -1, minDays: 10)
                        : CreateDA(new DeclareWar(), target: player, other: -1, minDays: -1);
                }

                if (idealWar.IndexOf((byte)PlayerIdExtensions.ToOriginal(player)) != -1)
                {
                    action = Control.CanDeclareWar(PlayerId.FromOriginal((byte)PlayerIdExtensions.ToOriginal(player)))
                        ? CreateDA(new DeclareWar(), target: player, other: -1, minDays: -1)
                        : CreateDA(new OfferPeace(), target: player, other: -1, minDays: 10);
                }

                break;
            case DiplomaticRelation.Peace:
                if (!Control.CanOfferPeace(PlayerId.FromOriginal((byte)PlayerIdExtensions.ToOriginal(player))) && idealWar.IndexOf((byte)PlayerIdExtensions.ToOriginal(player)) != -1)
                {
                    StrategicMap strategicMap = GetEngine().StrategicMap;
                    RandSeed = strategicMap.InitialRandomSeed + strategicMap.Day + source;
                    index = Random(idealWar.Count);
                    if (idealWar[index] == PlayerIdExtensions.ToOriginal(player))
                    {
                        action = CreateDA(new DeclareWar(), target: player, other: -1, minDays: -1);
                    }
                }
                else
                {
                    if (Control.CanProposeAlliance(PlayerId.FromOriginal((byte)PlayerIdExtensions.ToOriginal(player))))
                    {
                        StrategicMap engineStrategicMap = GetEngine().StrategicMap;
                        RandSeed = (engineStrategicMap.InitialRandomSeed + engineStrategicMap.Day + source) >>> 2;
                        if (Random(100) < 30)
                        {
                            action = CreateDA(new ProposeAlliance(), target: player, other: -1, minDays: 10);
                        }
                    }
                }

                break;
            case DiplomaticRelation.Alliance:
                if (!Control.CanOfferPeace(PlayerId.FromOriginal((byte)PlayerIdExtensions.ToOriginal(player))) && idealWar.IndexOf((byte)PlayerIdExtensions.ToOriginal(player)) != -1)
                {
                    StrategicMap initialRandomSeedStrategicMap = GetEngine().StrategicMap;
                    RandSeed = initialRandomSeedStrategicMap.InitialRandomSeed + initialRandomSeedStrategicMap.Day + source + 1;
                    index = Random(idealWar.Count);
                    if (idealWar[index] == PlayerIdExtensions.ToOriginal(player))
                    {
                        action = CreateDA(new BreakAlliance(), target: player, other: -1, minDays: -1);
                    }
                }

                break;
            case DiplomaticRelation.War:
                if (!Control.CanDeclareWar(PlayerId.FromOriginal((byte)PlayerIdExtensions.ToOriginal(player))))
                {
                    DoubleItemList candidates = [];
                    int score;
                    int candidate;
                    for (int i = 0, last = idealPeace.Count - 1; i <= last; i++)
                    {
                        candidate = idealPeace[i].TruncateToSByte();
                        if (war.IndexOf((byte)candidate) != -1 && Control.CanOfferPeace(PlayerId.FromOriginal((byte)candidate)))
                        {
                            action = GetEngine().StrategicMap.Players.GetPlayers(source).DiplomaticActions.FindLastAction(OfferPeace.Kind, reaction: DiplomaticActionReaction.None, source: PlayerId.FromOriginal(source), target: PlayerId.FromOriginal(candidate), other: -1);
                            if (action is null || GetEngine().StrategicMap.Day - action.Day > 12)
                            {
                                score = -i;
                                StrategicMap getDiplomaticRelationCountStrategicMap = GetEngine().StrategicMap;
                                score -= getDiplomaticRelationCountStrategicMap.Players.GetDiplomaticRelationCount(PlayerId.FromOriginal((sbyte)(byte)candidate), relations: DiplomaticRelations.Peace) * 5;
                                score -= getDiplomaticRelationCountStrategicMap.Players.GetDiplomaticRelationCount(PlayerId.FromOriginal((sbyte)(byte)candidate), relations: DiplomaticRelations.Alliance) * 10;
                                _ = candidates.AddAbility(candidate, score);
                            }

                            action = null;
                        }
                    }

                    if (candidates.Count == 0 && war.Count - peace.Count > 2)
                    {
                        for (int j = 0, last = idealWar.Count - 1; j <= last; j++)
                        {
                            candidate = idealWar[j].TruncateToSByte();
                            if (war.IndexOf((byte)candidate) != -1)
                            {
                                action = GetEngine().StrategicMap.Players.GetPlayers(source).DiplomaticActions.FindLastAction(OfferPeace.Kind, reaction: DiplomaticActionReaction.None, source: PlayerId.FromOriginal(source), target: PlayerId.FromOriginal(candidate), other: -1);
                                if (action is null || GetEngine().StrategicMap.Day - action.Day > 12)
                                {
                                    score = j;
                                    StrategicMap playersStrategicMap = GetEngine().StrategicMap;
                                    score -= playersStrategicMap.Players.GetDiplomaticRelationCount(PlayerId.FromOriginal((sbyte)(byte)candidate), relations: DiplomaticRelations.Peace) * 5;
                                    score -= playersStrategicMap.Players.GetDiplomaticRelationCount(PlayerId.FromOriginal((sbyte)(byte)candidate), relations: DiplomaticRelations.Alliance) * 10;
                                    _ = candidates.AddAbility(candidate, score);
                                }

                                action = null;
                            }
                        }
                    }

                    if (candidates.Count != 0)
                    {
                        candidates.SortDescendingItemB();
                        if (candidates[0].First == PlayerIdExtensions.ToOriginal(player))
                        {
                            action = CreateDA(new OfferPeace(), target: PlayerId.FromOriginal(candidates[0].First.TruncateToSByte()), other: -1, minDays: -1);
                        }
                    }
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
        return false;
    }

    // Diplomacy.pas:1670
    public void ListDebugInfo()
    {
        ByteList idealPeace = [];
        ByteList idealWar = [];
        Control.ListIdealPeaceWarPlayers(idealPeace, war: idealWar);
        int player = PlayerIdExtensions.ToOriginal(Control.Player.PlayerIndex);
        Player? players = GetEngine().StrategicMap.Players.GetPlayers(player);
        string s = AbilityRules.TranslateResourceString(GetEngine(), S: GameTables.RaceNamesPluralTexts[players.PlayerRace]);
        string name = GetEngine().StrategicMap.Players.GetPlayers(player).GetName();
        s = $"{s}/{name}";
        GetEngine().OutputDebugMessage(DebugChannel.AI, S: $"DIPLOMACY ({s})");
        GetEngine().OutputDebugMessage(DebugChannel.AI, S: "IDEAL PEACE PLAYERS");
        for (int i = 0, last = idealPeace.Count - 1; i <= last; i++)
        {
            int item = idealPeace[i];
            Player? fPlayersPlayers = GetEngine().StrategicMap.Players.GetPlayers(item);
            s = AbilityRules.TranslateResourceString(GetEngine(), S: GameTables.RaceNamesPluralTexts[fPlayersPlayers.PlayerRace]);
            int idealPeaceItem = idealPeace[i];
            string name2 = GetEngine().StrategicMap.Players.GetPlayers(idealPeaceItem).GetName();
            s = $"{s}/{name2}";
            GetEngine().OutputDebugMessage(DebugChannel.AI, s);
        }

        GetEngine().OutputDebugMessage(DebugChannel.AI, S: "IDEAL WAR PLAYERS");
        for (int i = 0, last = idealWar.Count - 1; i <= last; i++)
        {
            int item = idealWar[i];
            string name2 = GetEngine().StrategicMap.Players.GetPlayers(item).GetName();
            GetEngine().OutputDebugMessage(DebugChannel.AI, S: name2);
        }
    }

    // Diplomacy.pas:1546
    public static new int Priority => 14;

}
