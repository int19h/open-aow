// Translated from AoWE.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.AI;

public partial class DiplomacyAIPlayerAction : AIPlayerAction, IAIPlayerActionClass
{
    // Diplomacy.pas:1540
    public DiplomacyAIPlayerAction(ECustomNode owner) : base(owner)
    {
    }

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
        bool result;
        base.Process();
        if (Control.Player.PlayerIndex != 0)
        {
            while (true)
            {
                var count = StrategicMap.Current.Players.Count;
                if (!(_nextPlayer < count && !Control.Player.GetBusy()))
                    break;
                _nextPlayer = (sbyte)(_nextPlayer + 1);
                while (true)
                {
                    var fPlayersCount = StrategicMap.Current.Players.Count;
                    if (!(_nextPlayer < fPlayersCount && (Control.Player.GetFirstDaySeen((sbyte)(byte)_nextPlayer) == -1 || Control.Player.PlayerIndex == _nextPlayer)))
                        break;
                    _nextPlayer = (sbyte)(_nextPlayer + 1);
                }

                var count2 = StrategicMap.Current.Players.Count;
                if (_nextPlayer < count2)
                {
                    ExecuteDiplomacy(_nextPlayer);
                }
            }

            var count3 = StrategicMap.Current.Players.Count;
            result = _nextPlayer < count3;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Diplomacy.pas:1552
    public DiplomaticAction CreateDA(DiplomaticProposal action, sbyte target, sbyte other, int minDays)
    {
        DiplomaticAction result;
        sbyte player;
        DiplomaticAction last;
        player = Control.Player.PlayerIndex;
        last = StrategicMap.Current.Players.GetPlayers(player).DiplomaticActions.FindLastAction(action.ToFields().Kind, DiplomaticActionReaction.None, player, target, other);
        if (last == null || StrategicMap.Current.Day - last.Day >= minDays)
        {
            result = StrategicMap.Current.Players.GetPlayers(player).CreateDiplomaticAction(action, target, other);
        }
        else
        {
            result = null;
        }

        return result;
    }

    // Diplomacy.pas:1563
    public bool ExecuteDiplomacy(sbyte player)
    {
        DiplomaticAction action;
        ByteList peace;
        ByteList war;
        ByteList unknown;
        ByteList idealPeace;
        ByteList idealWar;
        DoubleItemList candidates;
        int index;
        int score;
        sbyte source;
        sbyte candidate;
        string errorText = "";
        // { Original bug: the result remains False even when an action is sent. }
        bool result = false;
        action = null;
        source = Control.Player.PlayerIndex;
        idealPeace = new ByteList();
        idealWar = new ByteList();
        peace = new ByteList();
        war = new ByteList();
        unknown = new ByteList();
        Control.ListPeaceWarUnknownPlayers(peace, war, unknown);
        Control.ListIdealPeaceWarPlayers(idealPeace, idealWar);
        switch (StrategicMap.Current.Players.GetDiplomaticRelation((sbyte)(byte)source, (sbyte)(byte)player))
        {
            case DiplomaticRelation.Unknown:
                if (idealPeace.IndexOf((byte)player) != -1)
                {
                    if (Control.CanOfferPeace((byte)player))
                    {
                        action = CreateDA(new OfferPeace(), player, -1, 10);
                    }
                    else
                    {
                        action = CreateDA(new DeclareWar(), player, -1, -1);
                    }
                }

                if (idealWar.IndexOf((byte)player) != -1)
                {
                    if (Control.CanDeclareWar((byte)player))
                    {
                        action = CreateDA(new DeclareWar(), player, -1, -1);
                    }
                    else
                    {
                        action = CreateDA(new OfferPeace(), player, -1, 10);
                    }
                }

                break;
            case DiplomaticRelation.Peace:
                if (!Control.CanOfferPeace((byte)player) && idealWar.IndexOf((byte)player) != -1)
                {
                    RandSeed = StrategicMap.Current.InitialRandomSeed + StrategicMap.Current.Day + source;
                    index = Random(idealWar.Count);
                    if (idealWar[index] == player)
                    {
                        action = CreateDA(new DeclareWar(), player, -1, -1);
                    }
                }
                else
                {
                    if (Control.CanProposeAlliance((byte)player))
                    {
                        RandSeed = (int)(((uint)(StrategicMap.Current.InitialRandomSeed + StrategicMap.Current.Day + source)) >> 2);
                        if (Random(100) < 30)
                        {
                            action = CreateDA(new ProposeAlliance(), player, -1, 10);
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
                        action = CreateDA(new BreakAlliance(), player, -1, -1);
                    }
                }

                break;
            case DiplomaticRelation.War:
                if (!Control.CanDeclareWar((byte)player))
                {
                    candidates = new DoubleItemList();
                    for (int I = 0, last = idealPeace.Count - 1; I <= last; I++)
                    {
                        candidate = (sbyte)idealPeace[I];
                        if (war.IndexOf((byte)candidate) != -1 && Control.CanOfferPeace((byte)candidate))
                        {
                            action = StrategicMap.Current.Players.GetPlayers(source).DiplomaticActions.FindLastAction(OfferPeace.Kind, DiplomaticActionReaction.None, source, candidate, -1);
                            if (action == null || StrategicMap.Current.Day - action.Day > 12)
                            {
                                score = -I;
                                score -= StrategicMap.Current.Players.GetDiplomaticRelationCount((sbyte)(byte)candidate, Set<DiplomaticRelation>.Of(DiplomaticRelation.Peace)) * 5;
                                score -= StrategicMap.Current.Players.GetDiplomaticRelationCount((sbyte)(byte)candidate, Set<DiplomaticRelation>.Of(DiplomaticRelation.Alliance)) * 10;
                                candidates.AddAbility(candidate, score);
                            }

                            action = null;
                        }
                    }

                    if (candidates.Count == 0 && war.Count - peace.Count > 2)
                    {
                        for (int J = 0, last = idealWar.Count - 1; J <= last; J++)
                        {
                            candidate = (sbyte)idealWar[J];
                            if (war.IndexOf((byte)candidate) != -1)
                            {
                                action = StrategicMap.Current.Players.GetPlayers(source).DiplomaticActions.FindLastAction(OfferPeace.Kind, DiplomaticActionReaction.None, source, candidate, -1);
                                if (action == null || StrategicMap.Current.Day - action.Day > 12)
                                {
                                    score = J;
                                    score -= StrategicMap.Current.Players.GetDiplomaticRelationCount((sbyte)(byte)candidate, Set<DiplomaticRelation>.Of(DiplomaticRelation.Peace)) * 5;
                                    score -= StrategicMap.Current.Players.GetDiplomaticRelationCount((sbyte)(byte)candidate, Set<DiplomaticRelation>.Of(DiplomaticRelation.Alliance)) * 10;
                                    candidates.AddAbility(candidate, score);
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
                            action = CreateDA(new OfferPeace(), (sbyte)candidates[0].First, -1, -1);
                        }
                    }

                    candidates?.Dispose();
                }

                break;
        }

        if (action != null)
        {
            action.Send(ref errorText);
            action.Release();
        }

        peace?.Dispose();
        war?.Dispose();
        unknown?.Dispose();
        idealPeace?.Dispose();
        idealWar?.Dispose();
        return result;
    }

    // Diplomacy.pas:1670
    public void ListDebugInfo()
    {
        ByteList idealPeace;
        ByteList idealWar;
        int player;
        string S;
        idealPeace = new ByteList();
        idealWar = new ByteList();
        Control.ListIdealPeaceWarPlayers(idealPeace, idealWar);
        player = Control.Player.PlayerIndex;
        var players = StrategicMap.Current.Players.GetPlayers(player);
        S = AbilityRules.TranslateResourceString(GameTables.RaceNamesPluralTexts[players.PlayerRace]);
        var name = StrategicMap.Current.Players.GetPlayers(player).GetName();
        S = S + "/" + name;
        GameTables.GameEngine.OutputDebugMessage(AbilityRules.AiDebugChannel, "DIPLOMACY (" + S + ")");
        GameTables.GameEngine.OutputDebugMessage(AbilityRules.AiDebugChannel, "IDEAL PEACE PLAYERS");
        for (int I = 0, last = idealPeace.Count - 1; I <= last; I++)
        {
            var item = idealPeace[I];
            var fPlayersPlayers = StrategicMap.Current.Players.GetPlayers(item);
            S = AbilityRules.TranslateResourceString(GameTables.RaceNamesPluralTexts[fPlayersPlayers.PlayerRace]);
            var idealPeaceItem = idealPeace[I];
            var name2 = StrategicMap.Current.Players.GetPlayers(idealPeaceItem).GetName();
            S = S + "/" + name2;
            GameTables.GameEngine.OutputDebugMessage(AbilityRules.AiDebugChannel, S);
        }

        GameTables.GameEngine.OutputDebugMessage(AbilityRules.AiDebugChannel, "IDEAL WAR PLAYERS");
        for (int I = 0, last = idealWar.Count - 1; I <= last; I++)
        {
            var item = idealWar[I];
            var name2 = StrategicMap.Current.Players.GetPlayers(item).GetName();
            GameTables.GameEngine.OutputDebugMessage(AbilityRules.AiDebugChannel, name2);
        }

        idealPeace?.Dispose();
        idealWar?.Dispose();
    }

    // intf_P5_08.inc:209

    // Diplomacy.pas:1546
    public static new int Priority => 14;

}
