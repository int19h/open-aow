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
        LocalControlledPlayers = new ByteList();
        LocalHumanPlayers = new ByteList();
        ActivePlayers = new ByteList();
        LocalPlayers = new ByteList();
    }

    public ByteList ActivePlayers;
    public ByteList LocalControlledPlayers;
    public ByteList LocalPlayers;
    public ByteList LocalHumanPlayers;

    // Player.PAS:1543
    public override void Assign(EngineObject source, GameEngine engine)
    {
        SetNumberOfPlayers(((PlayerList)source).GetNumberOfPlayers());
        for (int I = 0, last = _children.Count - 1; I <= last; I++)
        {
            var players = ((PlayerList)source).GetPlayers(I);
            ((EngineObject)_children.List[I]).Assign(players, GameTables.GameEngine);
        }
    }

    // Player.PAS:1434
    public int GetDiplomaticRelationCount(sbyte player, Set<DiplomaticRelation> relations)
    {
        int result = 0;
        for (int I = 1, last = AoWHSMap.Current.Players.Count - 1; I <= last; I++)
        {
            if (I != player)
            {
                if (GetPlayers(I).Status == PlayerStatus.Playing)
                {
                    if (relations.Contains(GetDiplomaticRelation(player, (sbyte)I)))
                    {
                        result += 1;
                    }
                }
            }
        }

        return result;
    }

    // Player.PAS:1445
    public DiplomaticRelation GetDiplomaticRelation(sbyte player, sbyte otherPlayer) => ((Player)_children.List[player]).DiplomaticRelations.GetDiplomaticRelation(otherPlayer);

    // Player.PAS:1450
    public void SetDiplomaticRelation(sbyte player, sbyte otherPlayer, DiplomaticRelation relation)
    {
        if (((Player)_children.List[player]).DiplomaticRelations.GetDiplomaticRelation(otherPlayer) != relation || ((Player)_children.List[otherPlayer]).DiplomaticRelations.GetDiplomaticRelation(player) != relation)
        {
            if (relation == DiplomaticRelation.Alliance)
            {
                for (int I = 1, last = Count - 1; I <= last; I++)
                {
                    if (GetPlayers(player).GetFirstDaySeen((sbyte)I) == -1 && GetPlayers(otherPlayer).GetFirstDaySeen((sbyte)I) != -1)
                    {
                        GetPlayers(player).PlayerSeen((sbyte)I);
                    }

                    if (GetPlayers(otherPlayer).GetFirstDaySeen((sbyte)I) == -1 && GetPlayers(player).GetFirstDaySeen((sbyte)I) != -1)
                    {
                        GetPlayers(otherPlayer).PlayerSeen((sbyte)I);
                    }
                }
            }

            ((Player)_children.List[player]).DiplomaticRelations.SetDiplomaticRelation(otherPlayer, relation);
            ((Player)_children.List[otherPlayer]).DiplomaticRelations.SetDiplomaticRelation(player, relation);
            if (relation == DiplomaticRelation.War)
            {
                ((Player)_children.List[player]).AIValidAttackPlayers[otherPlayer] = true;
                ((Player)_children.List[otherPlayer]).AIValidAttackPlayers[player] = true;
            }

            if (Set<DiplomaticRelation>.Of(DiplomaticRelation.Peace, DiplomaticRelation.Alliance).Contains(relation))
            {
                ((Player)_children.List[player]).AIValidAttackPlayers[otherPlayer] = false;
                ((Player)_children.List[otherPlayer]).AIValidAttackPlayers[player] = false;
            }

            AoWHSMap.Current._diplomaticRelationsChangedEventsStore.Raise(this);
            for (int J = Count - 1; J >= 1; J--)
            {
                ((Player)_children.List[J]).MagicControl.UpdateEnemyEnchantments();
            }

            AoWHSMap.Current.UpdateVisibility();
        }
    }

    // Player.PAS:1484
    public int GetActivePlayers()
    {
        int I;
        I = 1;
        int result = 0;
        // { Original tests the active count, not the index, and can run past the list. }
        while (result != _children.Count)
        {
            if (((Player)_children.List[I]).GameOverStatus != GameOverStatus.Lost)
            {
                result += 1;
            }

            I += 1;
        }

        return result;
    }

    // Player.PAS:1526
    public void BeginUpdate()
    {
        for (int I = 0, last = _children.Count - 1; I <= last; I++)
        {
            ((Player)_children.List[I]).BeginUpdate();
        }
    }

    // Player.PAS:1534
    public void EndUpdate()
    {
        for (int I = 0, last = _children.Count - 1; I <= last; I++)
        {
            ((Player)_children.List[I]).EndUpdate();
        }

        Update();
    }

    // Player.PAS:1552
    public Player GetPlayers(int index)
    {
        Player result;
        if (index >= 0 && index < _children.Count)
        {
            result = (Player)_children.List[index];
        }
        else
        {
            result = null;
        }

        return result;
    }

    // Player.PAS:1559
    public bool GetActivePlayersAllied()
    {
        int I;
        int J;
        bool result = true;
        I = ActivePlayers.Count;
        while (result && I != 0)
        {
            I -= 1;
            J = ActivePlayers.Count;
            while (result && J != 0)
            {
                J -= 1;
                if (J != I)
                {
                    var item = ActivePlayers[I];
                    result = GetDiplomaticRelation((sbyte)ActivePlayers[J], (sbyte)item) == DiplomaticRelation.Alliance;
                }
            }
        }

        return result;
    }

    // Player.PAS:1577
    public int GetAverageHeroLevel()
    {
        int count;
        Player player;
        count = 0;
        int result = 0;
        for (int I = 1, last = _children.Count - 1; I <= last; I++)
        {
            player = (Player)_children.EObjects[I];
            for (int J = player.Heroes.Count - 1; J >= 0; J--)
            {
                result += player.Heroes.GetHero(J).GetLevel();
                count += 1;
            }
        }

        if (count != 0)
        {
            result = result / count;
        }
        else
        {
            result = 0;
        }

        return result;
    }

    // Player.PAS:1597
    public int GetAverageHeroCount()
    {
        int result = 0;
        for (int I = 1, last = _children.Count - 1; I <= last; I++)
        {
            result += ((Player)_children.EObjects[I]).Heroes.Count;
        }

        return result / (_children.Count - 1);
    }

    // Player.PAS:1607
    public int GetMaxHeroCount()
    {
        int result = 0;
        for (int I = 1, last = _children.Count - 1; I <= last; I++)
        {
            if (((Player)_children.EObjects[I]).Heroes.Count > result)
            {
                result = ((Player)_children.EObjects[I]).Heroes.Count;
            }
        }

        return result;
    }

    // Player.PAS:1617
    public byte GetNumberOfPlayers() => (byte)(_children.Count - 1);

    // Player.PAS:1622
    public int RaceToPlayer(sbyte race)
    {
        int I;
        int result = -1;
        I = 1;
        // { Original uses <> Count and assumes at least the independent player exists. }
        while (result == -1 && I != _children.Count)
        {
            if (((Player)_children.List[I]).PlayerRace == race)
            {
                result = I;
            }

            I += 1;
        }

        return result;
    }

    // Player.PAS:1636
    public void SetNumberOfPlayers(byte value)
    {
        Player player;
        if (value < 2)
        {
            value = 2;
        }

        if (value > 12)
        {
            value = 12;
        }

        while (Count <= value)
        {
            player = new Player(null);
            player.SetPlayerIndex((sbyte)Count);
            if (Count == 0)
            {
                player.PlayerType = PlayerType.Independent;
            }

            player.PlayerRace = (sbyte)(Count - 1);
            player.SetArrayOwner(this, Count);
        }

        while (Count > value + 1)
        {
            GetPlayers(Count - 1).Dispose();
            var count = Count;
            _children.Delete(count - 1);
        }
    }

    // Player.PAS:1657
    public void ValidateRaces(RaceList races)
    {
        Int32List available;
        Int32List invalid;
        int I;
        int index;
        int defaultRace;
        available = new Int32List();
        invalid = new Int32List();
        I = 0;
        int last = races.Count - 1;
        for (; I <= last; I += 1)
        {
            if (races.GetRace(I).Enabled)
            {
                available.Add(I);
            }
        }

        I = 1;
        int limit = _children.Count - 1;
        for (; I <= limit; I += 1)
        {
            index = available.IndexOf(((Player)_children.List[I]).PlayerRace);
            if (index == -1)
            {
                invalid.Add(I);
            }
            else
            {
                available.Delete(index);
            }
        }

        I = 0;
        defaultRace = -1;
        while (I != AoWHSSet.Current.RaceResources.Count && defaultRace == -1)
        {
            if (AoWHSSet.Current.RaceResources.GetRaceResource(I) != null)
            {
                if (races == null || races.GetRace(I).Enabled)
                {
                    defaultRace = I;
                }
            }

            I += 1;
        }

        I = 0;
        int last2 = invalid.Count - 1;
        for (; I <= last2; I += 1)
        {
            if (available.Count == 0)
            {
                var item = invalid[I];
                ((Player)_children.List[item]).SetRace((sbyte)defaultRace);
            }
            else
            {
                var item = available[0];
                var invalidItem = invalid[I];
                ((Player)_children.List[invalidItem]).SetRace((sbyte)item);
                available.Delete(0);
            }
        }

        invalid?.Dispose();
        available?.Dispose();
    }

    // Player.PAS:1691
    public int VerifyRaces(RaceList races)
    {
        int result;
        bool valid;
        int I;
        int J;
        Player player;
        valid = true;
        I = 1;
        while (valid && I < _children.Count)
        {
            player = (Player)_children.List[I];
            valid = player.PlayerRace != -1 && races.GetRace(player.PlayerRace).Enabled;
            if (valid)
            {
                J = 1;
                while (valid && J != _children.Count)
                {
                    valid = J == I || GetPlayers(J).PlayerRace != player.PlayerRace;
                    J += 1;
                }
            }

            if (valid)
            {
                I += 1;
            }
        }

        if (valid)
        {
            result = 0;
        }
        else
        {
            result = I;
        }

        return result;
    }

    // Player.PAS:1718
    public sbyte GetSuperiorPlayer()
    {
        int maximum;
        int bestPlayer;
        int value;
        maximum = 0;
        bestPlayer = -1;
        for (int I = 1, last = Count - 1; I <= last; I++)
        {
            if (GetPlayers(I).Status == PlayerStatus.Playing)
            {
                value = GetPlayers(I).GetSuperiority();
                if (value > maximum)
                {
                    maximum = value;
                    bestPlayer = I;
                }
            }
        }

        return (sbyte)bestPlayer;
    }

    // Player.PAS:1737
    public int IndexOfPlayerType(PlayerType playerType)
    {
        int result = Count;
        while (result != 0)
        {
            result -= 1;
            if (GetPlayers(result).PlayerType == playerType)
            {
                // { The search index is the function result. }
                return result;
            }
        }

        return -1;
    }

    // intf_P1_03.inc:319
    public void Update()
    {
        if (AoWHSMap.Current.PlayerCommunicator.PlayerCommunicationControl is null) return;
        LocalControlledPlayers.Clear();
        LocalHumanPlayers.Clear();
        LocalPlayers.Clear();
        ActivePlayers.Clear();
        for (int i = 0; i < _children.Count; i++)
        {
            var player = (Player)_children.List[i];
            if (player.Status != PlayerStatus.Playing) continue;
            if (player.GameOverStatus != GameOverStatus.Lost && i != 0) ActivePlayers.Add((byte)i);
            if (player.GetLocal())
            {
                LocalControlledPlayers.Add((byte)i);
                if (player.PlayerType == PlayerType.Human) LocalHumanPlayers.Add((byte)i);
            }
            if (player.PlayerType == PlayerType.Human) LocalPlayers.Add((byte)i);
        }
    }
}
