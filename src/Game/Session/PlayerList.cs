// Translated from AoWE.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Session;

// active, local, human, local human lists
public partial class PlayerList : EControlStyleNode
{

    // Player.PAS:1414
    public PlayerList(ECustomNode owner) : base(owner)
    {
        SetControlStyle(Set<ControlStyleElement>.Of(ControlStyleElement.SaveLoad, ControlStyleElement.FixedChild, ControlStyleElement.ArrayMode));
        SetNumberOfPlayers(2);
        FLocalControlledPlayers = new ByteList();
        FLocalHumanPlayers = new ByteList();
        FActivePlayers = new ByteList();
        FLocalPlayers = new ByteList();
    }

    public ByteList FActivePlayers;
    public ByteList FLocalControlledPlayers;
    public ByteList FLocalPlayers;
    public ByteList FLocalHumanPlayers;

    // Player.PAS:1543
    public override void Assign(EngineObject Source, GameEngine AEngine)
    {
        SetNumberOfPlayers(((PlayerList)Source).GetNumberOfPlayers());
        for (int I = 0, last = FChildren.Count - 1; I <= last; I++)
        {
            var players = ((PlayerList)Source).GetPlayers(I);
            ((EngineObject)FChildren.List[I]).Assign(players, GameTables.AoWEngine);
        }
    }

    // Player.PAS:1434
    public int GetDiplomaticRelationCount(sbyte Player, Set<DiplomaticRelation> Relations)
    {
        int Result = 0;
        for (int I = 1, last = AoWHSMap.Current.FPlayers.Count - 1; I <= last; I++)
        {
            if (I != Player)
            {
                if (GetPlayers(I).FStatus == Session.Player.Status.Playing)
                {
                    if (Relations.Contains(GetDiplomaticRelation(Player, (sbyte)I)))
                    {
                        Result += 1;
                    }
                }
            }
        }

        return Result;
    }

    // Player.PAS:1445
    public DiplomaticRelation GetDiplomaticRelation(sbyte Player, sbyte OtherPlayer) => ((Player)FChildren.List[Player]).FDiplomaticRelations.GetDiplomaticRelation(OtherPlayer);

    // Player.PAS:1450
    public void SetDiplomaticRelation(sbyte Player, sbyte OtherPlayer, DiplomaticRelation Relation)
    {
        if (((Player)FChildren.List[Player]).FDiplomaticRelations.GetDiplomaticRelation(OtherPlayer) != Relation || ((Player)FChildren.List[OtherPlayer]).FDiplomaticRelations.GetDiplomaticRelation(Player) != Relation)
        {
            if (Relation == DiplomaticRelation.Alliance)
            {
                for (int I = 1, last = Count - 1; I <= last; I++)
                {
                    if (GetPlayers(Player).GetFirstDaySeen((sbyte)I) == -1 && GetPlayers(OtherPlayer).GetFirstDaySeen((sbyte)I) != -1)
                    {
                        GetPlayers(Player).PlayerSeen((sbyte)I);
                    }

                    if (GetPlayers(OtherPlayer).GetFirstDaySeen((sbyte)I) == -1 && GetPlayers(Player).GetFirstDaySeen((sbyte)I) != -1)
                    {
                        GetPlayers(OtherPlayer).PlayerSeen((sbyte)I);
                    }
                }
            }

            ((Player)FChildren.List[Player]).FDiplomaticRelations.SetDiplomaticRelation(OtherPlayer, Relation);
            ((Player)FChildren.List[OtherPlayer]).FDiplomaticRelations.SetDiplomaticRelation(Player, Relation);
            if (Relation == DiplomaticRelation.War)
            {
                ((Player)FChildren.List[Player]).FAIValidAttackPlayers[OtherPlayer] = true;
                ((Player)FChildren.List[OtherPlayer]).FAIValidAttackPlayers[Player] = true;
            }

            if (Set<DiplomaticRelation>.Of(DiplomaticRelation.Peace, DiplomaticRelation.Alliance).Contains(Relation))
            {
                ((Player)FChildren.List[Player]).FAIValidAttackPlayers[OtherPlayer] = false;
                ((Player)FChildren.List[OtherPlayer]).FAIValidAttackPlayers[Player] = false;
            }

            AoWHSMap.Current.FDiplomaticRelationsChangedEventsStore.Raise(this);
            for (int J = Count - 1; J >= 1; J--)
            {
                ((Player)FChildren.List[J]).FMagicControl.UpdateEnemyEnchantments();
            }

            AoWHSMap.Current.UpdateVisibility();
        }
    }

    // Player.PAS:1484
    public int GetActivePlayers()
    {
        int I;
        I = 1;
        int Result = 0;
        // { Original tests the active count, not the index, and can run past the list. }
        while (Result != FChildren.Count)
        {
            if (((Player)FChildren.List[I]).FGameOverStatus != GameOverStatus.Lost)
            {
                Result += 1;
            }

            I += 1;
        }

        return Result;
    }

    // Player.PAS:1526
    public void BeginUpdate()
    {
        for (int I = 0, last = FChildren.Count - 1; I <= last; I++)
        {
            ((Player)FChildren.List[I]).BeginUpdate();
        }
    }

    // Player.PAS:1534
    public void EndUpdate()
    {
        for (int I = 0, last = FChildren.Count - 1; I <= last; I++)
        {
            ((Player)FChildren.List[I]).EndUpdate();
        }

        Update();
    }

    // Player.PAS:1552
    public Player GetPlayers(int Index)
    {
        Player Result;
        if (Index >= 0 && Index < FChildren.Count)
        {
            Result = (Player)FChildren.List[Index];
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Player.PAS:1559
    public bool GetActivePlayersAllied()
    {
        int I;
        int J;
        bool Result = true;
        I = FActivePlayers.Count;
        while (Result && I != 0)
        {
            I -= 1;
            J = FActivePlayers.Count;
            while (Result && J != 0)
            {
                J -= 1;
                if (J != I)
                {
                    var item = FActivePlayers[I];
                    Result = GetDiplomaticRelation((sbyte)FActivePlayers[J], (sbyte)item) == DiplomaticRelation.Alliance;
                }
            }
        }

        return Result;
    }

    // Player.PAS:1577
    public int GetAverageHeroLevel()
    {
        int Count;
        Player Player;
        Count = 0;
        int Result = 0;
        for (int I = 1, last = FChildren.Count - 1; I <= last; I++)
        {
            Player = (Player)FChildren.EObjects[I];
            for (int J = Player.FHeroes.Count - 1; J >= 0; J--)
            {
                Result += Player.FHeroes.GetHero(J).GetLevel();
                Count += 1;
            }
        }

        if (Count != 0)
        {
            Result = Result / Count;
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // Player.PAS:1597
    public int GetAverageHeroCount()
    {
        int Result = 0;
        for (int I = 1, last = FChildren.Count - 1; I <= last; I++)
        {
            Result += ((Player)FChildren.EObjects[I]).FHeroes.Count;
        }

        return Result / (FChildren.Count - 1);
    }

    // Player.PAS:1607
    public int GetMaxHeroCount()
    {
        int Result = 0;
        for (int I = 1, last = FChildren.Count - 1; I <= last; I++)
        {
            if (((Player)FChildren.EObjects[I]).FHeroes.Count > Result)
            {
                Result = ((Player)FChildren.EObjects[I]).FHeroes.Count;
            }
        }

        return Result;
    }

    // Player.PAS:1617
    public byte GetNumberOfPlayers() => (byte)(FChildren.Count - 1);

    // Player.PAS:1622
    public int RaceToPlayer(sbyte Race)
    {
        int I;
        int Result = -1;
        I = 1;
        // { Original uses <> Count and assumes at least the independent player exists. }
        while (Result == -1 && I != FChildren.Count)
        {
            if (((Player)FChildren.List[I]).FPlayerRace == Race)
            {
                Result = I;
            }

            I += 1;
        }

        return Result;
    }

    // Player.PAS:1636
    public void SetNumberOfPlayers(byte Value)
    {
        Player Player;
        if (Value < 2)
        {
            Value = 2;
        }

        if (Value > 12)
        {
            Value = 12;
        }

        while (Count <= Value)
        {
            Player = new Player(null);
            Player.SetPlayerIndex((sbyte)Count);
            if (Count == 0)
            {
                Player.FPlayerType = PlayerType.Independent;
            }

            Player.FPlayerRace = (sbyte)(Count - 1);
            Player.SetArrayOwner(this, Count);
        }

        while (Count > Value + 1)
        {
            GetPlayers(Count - 1).Dispose();
            var count = Count;
            FChildren.Delete(count - 1);
        }
    }

    // Player.PAS:1657
    public void ValidateRaces(RaceList Races)
    {
        Int32List Available;
        Int32List Invalid;
        int I;
        int Index;
        int DefaultRace;
        Available = new Int32List();
        Invalid = new Int32List();
        I = 0;
        int last = Races.Count - 1;
        for (; I <= last; I += 1)
        {
            if (Races.GetRace(I).FEnabled)
            {
                Available.Add(I);
            }
        }

        I = 1;
        int limit = FChildren.Count - 1;
        for (; I <= limit; I += 1)
        {
            Index = Available.IndexOf(((Player)FChildren.List[I]).FPlayerRace);
            if (Index == -1)
            {
                Invalid.Add(I);
            }
            else
            {
                Available.Delete(Index);
            }
        }

        I = 0;
        DefaultRace = -1;
        while (I != AoWHSSet.Current.FRaceResources.Count && DefaultRace == -1)
        {
            if (AoWHSSet.Current.FRaceResources.GetRaceResource(I) != null)
            {
                if (Races == null || Races.GetRace(I).FEnabled)
                {
                    DefaultRace = I;
                }
            }

            I += 1;
        }

        I = 0;
        int last2 = Invalid.Count - 1;
        for (; I <= last2; I += 1)
        {
            if (Available.Count == 0)
            {
                var item = Invalid[I];
                ((Player)FChildren.List[item]).SetRace((sbyte)DefaultRace);
            }
            else
            {
                var item = Available[0];
                var invalidItem = Invalid[I];
                ((Player)FChildren.List[invalidItem]).SetRace((sbyte)item);
                Available.Delete(0);
            }
        }

        Invalid?.Dispose();
        Available?.Dispose();
    }

    // Player.PAS:1691
    public int VerifyRaces(RaceList Races)
    {
        int Result;
        bool Valid;
        int I;
        int J;
        Player Player;
        Valid = true;
        I = 1;
        while (Valid && I < FChildren.Count)
        {
            Player = (Player)FChildren.List[I];
            Valid = Player.FPlayerRace != -1 && Races.GetRace(Player.FPlayerRace).FEnabled;
            if (Valid)
            {
                J = 1;
                while (Valid && J != FChildren.Count)
                {
                    Valid = J == I || GetPlayers(J).FPlayerRace != Player.FPlayerRace;
                    J += 1;
                }
            }

            if (Valid)
            {
                I += 1;
            }
        }

        if (Valid)
        {
            Result = 0;
        }
        else
        {
            Result = I;
        }

        return Result;
    }

    // Player.PAS:1718
    public sbyte GetSuperiorPlayer()
    {
        int Maximum;
        int BestPlayer;
        int Value;
        Maximum = 0;
        BestPlayer = -1;
        for (int I = 1, last = Count - 1; I <= last; I++)
        {
            if (GetPlayers(I).FStatus == Player.Status.Playing)
            {
                Value = GetPlayers(I).GetSuperiority();
                if (Value > Maximum)
                {
                    Maximum = Value;
                    BestPlayer = I;
                }
            }
        }

        return (sbyte)BestPlayer;
    }

    // Player.PAS:1737
    public int IndexOfPlayerType(PlayerType PlayerType)
    {
        int Result = Count;
        while (Result != 0)
        {
            Result -= 1;
            if (GetPlayers(Result).FPlayerType == PlayerType)
            {
                // { The search index is the function result. }
                return Result;
            }
        }

        return -1;
    }

    // intf_P1_03.inc:319
    public void Update()
    {
        if (AoWHSMap.Current.FPlayerCommunicator.PCC is null) return;
        FLocalControlledPlayers.Clear();
        FLocalHumanPlayers.Clear();
        FLocalPlayers.Clear();
        FActivePlayers.Clear();
        for (int i = 0; i < FChildren.Count; i++)
        {
            var player = (Player)FChildren.List[i];
            if (player.FStatus != Player.Status.Playing) continue;
            if (player.FGameOverStatus != GameOverStatus.Lost && i != 0) FActivePlayers.Add((byte)i);
            if (player.GetLocal())
            {
                FLocalControlledPlayers.Add((byte)i);
                if (player.FPlayerType == PlayerType.Human) FLocalHumanPlayers.Add((byte)i);
            }
            if (player.FPlayerType == PlayerType.Human) FLocalPlayers.Add((byte)i);
        }
    }
}
