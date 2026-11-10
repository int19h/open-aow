// Translated from AoWE.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ai;

public partial class DiplomacyAIPA : AIPlayerAction, IAIPlayerActionClass
{
    // Diplomacy.pas:1540
    public DiplomacyAIPA(ECustomNode owner) : base(owner)
    {
    }

    public sbyte FNextPlayer;

    // Diplomacy.pas:1549
    public override bool Activate()
    {
        bool Result = base.Activate();
        FNextPlayer = 0;
        return Result;
    }

    // Diplomacy.pas:1699
    public override bool Process()
    {
        bool Result;
        base.Process();
        if (FControl.FPlayer.FPlayerIndex != 0)
        {
            while (true)
            {
                var count = AoWHSMap.Current.FPlayers.Count;
                if (!(FNextPlayer < count && !FControl.FPlayer.GetBusy()))
                    break;
                FNextPlayer = (sbyte)(FNextPlayer + 1);
                while (true)
                {
                    var fPlayersCount = AoWHSMap.Current.FPlayers.Count;
                    if (!(FNextPlayer < fPlayersCount && (FControl.FPlayer.GetFirstDaySeen((sbyte)(byte)FNextPlayer) == -1 || FControl.FPlayer.FPlayerIndex == FNextPlayer)))
                        break;
                    FNextPlayer = (sbyte)(FNextPlayer + 1);
                }

                var count2 = AoWHSMap.Current.FPlayers.Count;
                if (FNextPlayer < count2)
                {
                    ExecuteDiplomacy(FNextPlayer);
                }
            }

            var count3 = AoWHSMap.Current.FPlayers.Count;
            Result = FNextPlayer < count3;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Diplomacy.pas:1552
    public DiplomaticAction CreateDA(DiplomaticProposal Action, sbyte Target, sbyte Other, int MinDays)
    {
        DiplomaticAction Result;
        sbyte Player;
        DiplomaticAction Last;
        Player = FControl.FPlayer.FPlayerIndex;
        Last = AoWHSMap.Current.FPlayers.GetPlayers(Player).FDiplomaticActions.FindLastAction(Action.ToFields().Kind, DiplomaticActionReaction.None, Player, Target, Other);
        if (Last == null || AoWHSMap.Current.FDay - Last.FDay >= MinDays)
        {
            Result = AoWHSMap.Current.FPlayers.GetPlayers(Player).CreateDiplomaticAction(Action, Target, Other);
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Diplomacy.pas:1563
    public bool ExecuteDiplomacy(sbyte Player)
    {
        DiplomaticAction Action;
        ByteList Peace;
        ByteList War;
        ByteList Unknown;
        ByteList IdealPeace;
        ByteList IdealWar;
        DoubleItemList Candidates;
        int Index;
        int Score;
        sbyte Source;
        sbyte Candidate;
        string ErrorText = "";
        // { Original bug: the result remains False even when an action is sent. }
        bool Result = false;
        Action = null;
        Source = FControl.FPlayer.FPlayerIndex;
        IdealPeace = new ByteList();
        IdealWar = new ByteList();
        Peace = new ByteList();
        War = new ByteList();
        Unknown = new ByteList();
        FControl.ListPeaceWarUnknownPlayers(Peace, War, Unknown);
        FControl.ListIdealPeaceWarPlayers(IdealPeace, IdealWar);
        switch (AoWHSMap.Current.FPlayers.GetDiplomaticRelation((sbyte)(byte)Source, (sbyte)(byte)Player))
        {
            case DiplomaticRelation.Unknown:
                if (IdealPeace.IndexOf((byte)Player) != -1)
                {
                    if (FControl.CanOfferPeace((byte)Player))
                    {
                        Action = CreateDA(new OfferPeace(), Player, -1, 10);
                    }
                    else
                    {
                        Action = CreateDA(new DeclareWar(), Player, -1, -1);
                    }
                }

                if (IdealWar.IndexOf((byte)Player) != -1)
                {
                    if (FControl.CanDeclareWar((byte)Player))
                    {
                        Action = CreateDA(new DeclareWar(), Player, -1, -1);
                    }
                    else
                    {
                        Action = CreateDA(new OfferPeace(), Player, -1, 10);
                    }
                }

                break;
            case DiplomaticRelation.Peace:
                if (!FControl.CanOfferPeace((byte)Player) && IdealWar.IndexOf((byte)Player) != -1)
                {
                    RandSeed = AoWHSMap.Current.FInitialRandomSeed + AoWHSMap.Current.FDay + Source;
                    Index = Random(IdealWar.Count);
                    if (IdealWar[Index] == Player)
                    {
                        Action = CreateDA(new DeclareWar(), Player, -1, -1);
                    }
                }
                else
                {
                    if (FControl.CanProposeAlliance((byte)Player))
                    {
                        RandSeed = (int)(((uint)(AoWHSMap.Current.FInitialRandomSeed + AoWHSMap.Current.FDay + Source)) >> 2);
                        if (Random(100) < 30)
                        {
                            Action = CreateDA(new ProposeAlliance(), Player, -1, 10);
                        }
                    }
                }

                break;
            case DiplomaticRelation.Alliance:
                if (!FControl.CanOfferPeace((byte)Player) && IdealWar.IndexOf((byte)Player) != -1)
                {
                    RandSeed = AoWHSMap.Current.FInitialRandomSeed + AoWHSMap.Current.FDay + Source + 1;
                    Index = Random(IdealWar.Count);
                    if (IdealWar[Index] == Player)
                    {
                        Action = CreateDA(new BreakAlliance(), Player, -1, -1);
                    }
                }

                break;
            case DiplomaticRelation.War:
                if (!FControl.CanDeclareWar((byte)Player))
                {
                    Candidates = new DoubleItemList();
                    for (int I = 0, last = IdealPeace.Count - 1; I <= last; I++)
                    {
                        Candidate = (sbyte)IdealPeace[I];
                        if (War.IndexOf((byte)Candidate) != -1 && FControl.CanOfferPeace((byte)Candidate))
                        {
                            Action = AoWHSMap.Current.FPlayers.GetPlayers(Source).FDiplomaticActions.FindLastAction(OfferPeace.Kind, DiplomaticActionReaction.None, Source, Candidate, -1);
                            if (Action == null || AoWHSMap.Current.FDay - Action.FDay > 12)
                            {
                                Score = -I;
                                Score -= AoWHSMap.Current.FPlayers.GetDiplomaticRelationCount((sbyte)(byte)Candidate, Set<DiplomaticRelation>.Of(DiplomaticRelation.Peace)) * 5;
                                Score -= AoWHSMap.Current.FPlayers.GetDiplomaticRelationCount((sbyte)(byte)Candidate, Set<DiplomaticRelation>.Of(DiplomaticRelation.Alliance)) * 10;
                                Candidates.AddAB(Candidate, Score);
                            }

                            Action = null;
                        }
                    }

                    if (Candidates.Count == 0 && War.Count - Peace.Count > 2)
                    {
                        for (int J = 0, last = IdealWar.Count - 1; J <= last; J++)
                        {
                            Candidate = (sbyte)IdealWar[J];
                            if (War.IndexOf((byte)Candidate) != -1)
                            {
                                Action = AoWHSMap.Current.FPlayers.GetPlayers(Source).FDiplomaticActions.FindLastAction(OfferPeace.Kind, DiplomaticActionReaction.None, Source, Candidate, -1);
                                if (Action == null || AoWHSMap.Current.FDay - Action.FDay > 12)
                                {
                                    Score = J;
                                    Score -= AoWHSMap.Current.FPlayers.GetDiplomaticRelationCount((sbyte)(byte)Candidate, Set<DiplomaticRelation>.Of(DiplomaticRelation.Peace)) * 5;
                                    Score -= AoWHSMap.Current.FPlayers.GetDiplomaticRelationCount((sbyte)(byte)Candidate, Set<DiplomaticRelation>.Of(DiplomaticRelation.Alliance)) * 10;
                                    Candidates.AddAB(Candidate, Score);
                                }

                                Action = null;
                            }
                        }
                    }

                    if (Candidates.Count != 0)
                    {
                        Candidates.SortDescendingItemB();
                        if (Candidates[0].A == Player)
                        {
                            Action = CreateDA(new OfferPeace(), (sbyte)Candidates[0].A, -1, -1);
                        }
                    }

                    Candidates?.Dispose();
                }

                break;
        }

        if (Action != null)
        {
            Action.Send(ref ErrorText);
            Action.Release();
        }

        Peace?.Dispose();
        War?.Dispose();
        Unknown?.Dispose();
        IdealPeace?.Dispose();
        IdealWar?.Dispose();
        return Result;
    }

    // Diplomacy.pas:1670
    public void ListDebugInfo()
    {
        ByteList IdealPeace;
        ByteList IdealWar;
        int Player;
        string S;
        IdealPeace = new ByteList();
        IdealWar = new ByteList();
        FControl.ListIdealPeaceWarPlayers(IdealPeace, IdealWar);
        Player = FControl.FPlayer.FPlayerIndex;
        var players = AoWHSMap.Current.FPlayers.GetPlayers(Player);
        S = AbilityRules.TranslateRStr(GameTables.RaceNamesPluralRStr[players.FPlayerRace]);
        var name = AoWHSMap.Current.FPlayers.GetPlayers(Player).GetName();
        S = S + "/" + name;
        GameTables.AoWEngine.OutputDebugMessage(AbilityRules.AIDID, "DIPLOMACY (" + S + ")");
        GameTables.AoWEngine.OutputDebugMessage(AbilityRules.AIDID, "IDEAL PEACE PLAYERS");
        for (int I = 0, last = IdealPeace.Count - 1; I <= last; I++)
        {
            var item = IdealPeace[I];
            var fPlayersPlayers = AoWHSMap.Current.FPlayers.GetPlayers(item);
            S = AbilityRules.TranslateRStr(GameTables.RaceNamesPluralRStr[fPlayersPlayers.FPlayerRace]);
            var idealPeaceItem = IdealPeace[I];
            var name2 = AoWHSMap.Current.FPlayers.GetPlayers(idealPeaceItem).GetName();
            S = S + "/" + name2;
            GameTables.AoWEngine.OutputDebugMessage(AbilityRules.AIDID, S);
        }

        GameTables.AoWEngine.OutputDebugMessage(AbilityRules.AIDID, "IDEAL WAR PLAYERS");
        for (int I = 0, last = IdealWar.Count - 1; I <= last; I++)
        {
            var item = IdealWar[I];
            var name2 = AoWHSMap.Current.FPlayers.GetPlayers(item).GetName();
            GameTables.AoWEngine.OutputDebugMessage(AbilityRules.AIDID, name2);
        }

        IdealPeace?.Dispose();
        IdealWar?.Dispose();
    }

    // intf_P5_08.inc:209

    // Diplomacy.pas:1546
    public static new int Priority => 14;

}
