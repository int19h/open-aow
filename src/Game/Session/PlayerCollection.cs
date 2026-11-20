#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Session;

// active, local, human, local human lists
public partial class PlayerCollection : ControlStyleNode, IChildList<Player>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only a Player, so the cast never fails.
    public Player this[int index] => GetChild<Player>(index);

    protected sealed override bool AcceptsChild(EngineObject child) => child is Player;

    int IChildSink<Player>.AddChild(Player child) => AddChild(child);

    // Player.PAS:1414
    public PlayerCollection(CustomNode owner) : base(owner)
    {
        SetControlStyle(Set<ControlStyleElement>.Of(ControlStyleElement.SaveLoad, ControlStyleElement.FixedChild, ControlStyleElement.ArrayMode));
        SetNumberOfPlayers(2);
        _localControlledPlayers = new ByteList();
        LocalHumanPlayers = new ByteList();
        ActivePlayers = new ByteList();
        LocalPlayers = new ByteList();
    }

    public ByteList ActivePlayers { get; }

    private readonly ByteList _localControlledPlayers;

    public ByteList LocalPlayers { get; }
    public ByteList LocalHumanPlayers { get; init; }

    // Player.PAS:1543
    public override void Assign(EngineObject source, GameEngine engine)
    {
        // Assign's source is EngineObject. This override copies a player collection, and the cast stays.
        PlayerCollection other = (PlayerCollection)source;
        SetNumberOfPlayers(other.GetNumberOfPlayers());
        for (int i = 0, last = Count - 1; i <= last; i++)
        {
            Player players = other.GetPlayers(i);
            this[i].Assign(players, engine);
        }
    }

    // Player.PAS:1434
    public int GetDiplomaticRelationCount(sbyte player, Set<DiplomaticRelation> relations)
    {
        int result = 0;
        for (int i = 1, last = StrategicMap.Current.Players.Count - 1; i <= last; i++)
        {
            if (i != player)
            {
                if (GetPlayers(i).Status == PlayerStatus.Playing)
                {
                    if (relations.Contains(GetDiplomaticRelation(player, (sbyte)i)))
                    {
                        result += 1;
                    }
                }
            }
        }

        return result;
    }

    // Player.PAS:1445
    public DiplomaticRelation GetDiplomaticRelation(sbyte player, sbyte otherPlayer) => this[player].DiplomaticRelations.GetDiplomaticRelation(otherPlayer);

    // Player.PAS:1450
    public void SetDiplomaticRelation(sbyte player, sbyte otherPlayer, DiplomaticRelation relation)
    {
        if (this[player].DiplomaticRelations.GetDiplomaticRelation(otherPlayer) != relation || this[otherPlayer].DiplomaticRelations.GetDiplomaticRelation(player) != relation)
        {
            if (relation == DiplomaticRelation.Alliance)
            {
                for (int i = 1, last = Count - 1; i <= last; i++)
                {
                    if (GetPlayers(player).GetFirstDaySeen((sbyte)i) == -1 && GetPlayers(otherPlayer).GetFirstDaySeen((sbyte)i) != -1)
                    {
                        GetPlayers(player).PlayerSeen((sbyte)i);
                    }

                    if (GetPlayers(otherPlayer).GetFirstDaySeen((sbyte)i) == -1 && GetPlayers(player).GetFirstDaySeen((sbyte)i) != -1)
                    {
                        GetPlayers(otherPlayer).PlayerSeen((sbyte)i);
                    }
                }
            }

            this[player].DiplomaticRelations.SetDiplomaticRelation(otherPlayer, relation);
            this[otherPlayer].DiplomaticRelations.SetDiplomaticRelation(player, relation);
            if (relation == DiplomaticRelation.War)
            {
                this[player].AIValidAttackPlayers[otherPlayer] = true;
                this[otherPlayer].AIValidAttackPlayers[player] = true;
            }

            if (Set<DiplomaticRelation>.Of(DiplomaticRelation.Peace, DiplomaticRelation.Alliance).Contains(relation))
            {
                this[player].AIValidAttackPlayers[otherPlayer] = false;
                this[otherPlayer].AIValidAttackPlayers[player] = false;
            }

            StrategicMap.Current.DiplomaticRelationsChangedEventsStore.Raise(this);
            for (int j = Count - 1; j >= 1; j--)
            {
                this[j].MagicControl.UpdateEnemyEnchantments();
            }

            StrategicMap.Current.UpdateVisibility();
        }
    }

    // Player.PAS:1484
    public int GetActivePlayers()
    {
        int i = 1;
        int result = 0;
        // { Original tests the active count, not the index, and can run past the list. }
        while (result != Count)
        {
            if (this[i].GameOverStatus != GameOverStatus.Lost)
            {
                result += 1;
            }

            i += 1;
        }

        return result;
    }

    // Player.PAS:1526
    public void BeginUpdate()
    {
        for (int i = 0, last = Count - 1; i <= last; i++)
        {
            this[i].BeginUpdate();
        }
    }

    // Player.PAS:1534
    public void EndUpdate()
    {
        for (int i = 0, last = Count - 1; i <= last; i++)
        {
            this[i].EndUpdate();
        }

        Update();
    }

    // Player.PAS:1552
    public Player GetPlayers(int index) => index >= 0 && index < Count ? this[index] : null;

    // Player.PAS:1559
    public bool GetActivePlayersAllied()
    {
        bool result = true;
        int i = ActivePlayers.Count;
        while (result && i != 0)
        {
            i -= 1;
            int j = ActivePlayers.Count;
            while (result && j != 0)
            {
                j -= 1;
                if (j != i)
                {
                    byte item = ActivePlayers[i];
                    result = GetDiplomaticRelation((sbyte)ActivePlayers[j], (sbyte)item) == DiplomaticRelation.Alliance;
                }
            }
        }

        return result;
    }

    // Player.PAS:1577
    public int GetAverageHeroLevel()
    {
        int count = 0;
        int result = 0;
        for (int i = 1, last = Count - 1; i <= last; i++)
        {
            Player player = GetChildInRange<Player>(i);
            for (int j = player.Heroes.Count - 1; j >= 0; j--)
            {
                result += player.Heroes.GetHero(j).GetLevel();
                count += 1;
            }
        }

        return count != 0 ? result / count : 0;
    }

    // Player.PAS:1597
    public int GetAverageHeroCount()
    {
        int result = 0;
        for (int i = 1, last = Count - 1; i <= last; i++)
        {
            result += GetChildInRange<Player>(i).Heroes.Count;
        }

        return result / (Count - 1);
    }

    // Player.PAS:1607
    public int GetMaxHeroCount()
    {
        int result = 0;
        for (int i = 1, last = Count - 1; i <= last; i++)
        {
            if (GetChildInRange<Player>(i).Heroes.Count > result)
            {
                result = GetChildInRange<Player>(i).Heroes.Count;
            }
        }

        return result;
    }

    // Player.PAS:1617
    public byte GetNumberOfPlayers() => (byte)(Count - 1);

    // Player.PAS:1622
    public int RaceToPlayer(sbyte race)
    {
        int result = -1;
        int i = 1;
        // { Original uses <> Count and assumes at least the independent player exists. }
        while (result == -1 && i != Count)
        {
            if (this[i].PlayerRace == race)
            {
                result = i;
            }

            i += 1;
        }

        return result;
    }

    // Player.PAS:1636
    public void SetNumberOfPlayers(byte value)
    {
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
            Player player = new(null) { Holder = CreationHolder() };
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
            int count = Count;
            DeleteChildAt(count - 1);
        }
    }

    // Player.PAS:1657
    public void ValidateRaces(RaceList races)
    {
        Int32List available = new();
        Int32List invalid = new();
        int i = 0;
        int last = races.Count - 1;
        for (; i <= last; i += 1)
        {
            if (races.GetRace(i).Enabled)
            {
                _ = available.Add(i);
            }
        }

        i = 1;
        int limit = Count - 1;
        for (; i <= limit; i += 1)
        {
            int index = available.IndexOf(this[i].PlayerRace);
            if (index == -1)
            {
                _ = invalid.Add(i);
            }
            else
            {
                available.Delete(index);
            }
        }

        i = 0;
        int defaultRace = -1;
        while (i != GamePack.Current.RaceResources.Count && defaultRace == -1)
        {
            if (GamePack.Current.RaceResources.GetRaceResource(i) is not null)
            {
                if (races is null || races.GetRace(i).Enabled)
                {
                    defaultRace = i;
                }
            }

            i += 1;
        }

        i = 0;
        int last2 = invalid.Count - 1;
        for (; i <= last2; i += 1)
        {
            if (available.Count == 0)
            {
                this[invalid[i]].SetRace((sbyte)defaultRace);
            }
            else
            {
                int item = available[0];
                this[invalid[i]].SetRace((sbyte)item);
                available.Delete(0);
            }
        }

        invalid?.Dispose();
        available?.Dispose();
    }

    // Player.PAS:1691
    public int VerifyRaces(RaceList races)
    {
        bool valid = true;
        int i = 1;
        while (valid && i < Count)
        {
            Player player = this[i];
            valid = player.PlayerRace != -1 && races.GetRace(player.PlayerRace).Enabled;
            if (valid)
            {
                int j = 1;
                while (valid && j != Count)
                {
                    valid = j == i || GetPlayers(j).PlayerRace != player.PlayerRace;
                    j += 1;
                }
            }

            if (valid)
            {
                i += 1;
            }
        }

        return valid ? 0 : i;
    }

    // Player.PAS:1718
    public sbyte GetSuperiorPlayer()
    {
        int maximum = 0;
        int bestPlayer = -1;
        for (int i = 1, last = Count - 1; i <= last; i++)
        {
            if (GetPlayers(i).Status == PlayerStatus.Playing)
            {
                int value = GetPlayers(i).GetSuperiority();
                if (value > maximum)
                {
                    maximum = value;
                    bestPlayer = i;
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
        if (StrategicMap.Current.PlayerCommunicator.PlayerCommunicationControl is null)
        {
            return;
        }

        _localControlledPlayers.Clear();
        LocalHumanPlayers.Clear();
        LocalPlayers.Clear();
        ActivePlayers.Clear();
        for (int i = 0; i < Count; i++)
        {
            Player player = this[i];
            if (player.Status != PlayerStatus.Playing)
            {
                continue;
            }

            if (player.GameOverStatus != GameOverStatus.Lost && i != 0)
            {
                _ = ActivePlayers.Add((byte)i);
            }

            if (player.GetLocal())
            {
                _ = _localControlledPlayers.Add((byte)i);
                if (player.PlayerType == PlayerType.Human)
                {
                    _ = LocalHumanPlayers.Add((byte)i);
                }
            }
            if (player.PlayerType == PlayerType.Human)
            {
                _ = LocalPlayers.Add((byte)i);
            }
        }
    }
}
