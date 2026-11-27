#nullable disable warnings
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

    public Player this[PlayerId player] => this[player.Index];

    public IEnumerable<PlayerId> Ids
    {
        get
        {
            for (int i = 0; i < Count; i++)
            {
                yield return PlayerId.FromInt32(i);
            }
        }
    }

    public IEnumerable<PlayerId> GameIds
    {
        get
        {
            for (int i = 1; i < Count; i++)
            {
                yield return PlayerId.FromInt32(i);
            }
        }
    }

    protected sealed override bool AcceptsChild(EngineObject child) => child is Player;

    int IChildSink<Player>.AddChild(Player child) => AddChild(child);

    // Player.PAS:1414
    public PlayerCollection(CustomNode? owner) : base(owner)
    {
        SetControlStyle(ControlStyles.SaveLoad | ControlStyles.FixedChild | ControlStyles.ArrayMode);
        SetNumberOfPlayers(2);
        _localControlledPlayers = [];
        LocalHumanPlayers = [];
        ActivePlayers = [];
        LocalPlayers = [];
    }

    public ByteList ActivePlayers { get; }

    private readonly ByteList _localControlledPlayers;

    public ByteList LocalPlayers { get; }
    public ByteList LocalHumanPlayers { get; init; }

    // Player.PAS:1543
    public override void Assign(EngineObject? source, GameEngine? engine)
    {
        // Assign's source is EngineObject. This override copies a player collection, and the cast stays.
        PlayerCollection? other = (PlayerCollection?)source;
        SetNumberOfPlayers(other.NumberOfPlayers);
        for (int i = 0, last = Count - 1; i <= last; i++)
        {
            Player? players = other.GetPlayers(i);
            this[i].Assign(players, engine);
        }
    }

    // Player.PAS:1434
    public int GetDiplomaticRelationCount(PlayerId? player, DiplomaticRelations relations)
    {
        int result = 0;
        for (int i = 1, last = GetEngine().StrategicMap.Players.Count - 1; i <= last; i++)
        {
            if (i != PlayerIdExtensions.ToOriginal(player))
            {
                if (GetPlayers(i).Status == PlayerStatus.Playing)
                {
                    if (relations.Contains(GetDiplomaticRelation(player, otherPlayer: PlayerId.FromOriginal((sbyte)i)).ToFlag()))
                    {
                        result += 1;
                    }
                }
            }
        }

        return result;
    }

    // Player.PAS:1445
    public DiplomaticRelation GetDiplomaticRelation(PlayerId? player, PlayerId? otherPlayer) => this[PlayerIdExtensions.ToOriginal(player)].DiplomaticRelations.GetDiplomaticRelation(otherPlayer);

    // Player.PAS:1450
    public void SetDiplomaticRelation(PlayerId? player, PlayerId? otherPlayer, DiplomaticRelation relation)
    {
        if (this[PlayerIdExtensions.ToOriginal(player)].DiplomaticRelations.GetDiplomaticRelation(otherPlayer) != relation || this[PlayerIdExtensions.ToOriginal(otherPlayer)].DiplomaticRelations.GetDiplomaticRelation(player) != relation)
        {
            if (relation == DiplomaticRelation.Alliance)
            {
                for (int i = 1, last = Count - 1; i <= last; i++)
                {
                    if (GetPlayers(player).GetFirstDaySeen(PlayerId.FromOriginal((sbyte)i)) == -1 && GetPlayers(otherPlayer).GetFirstDaySeen(PlayerId.FromOriginal((sbyte)i)) != -1)
                    {
                        GetPlayers(player).PlayerSeen(PlayerId.FromOriginal((sbyte)i));
                    }

                    if (GetPlayers(otherPlayer).GetFirstDaySeen(PlayerId.FromOriginal((sbyte)i)) == -1 && GetPlayers(player).GetFirstDaySeen(PlayerId.FromOriginal((sbyte)i)) != -1)
                    {
                        GetPlayers(otherPlayer).PlayerSeen(PlayerId.FromOriginal((sbyte)i));
                    }
                }
            }

            this[PlayerIdExtensions.ToOriginal(player)].DiplomaticRelations.SetDiplomaticRelation(otherPlayer, value: relation);
            this[PlayerIdExtensions.ToOriginal(otherPlayer)].DiplomaticRelations.SetDiplomaticRelation(player, value: relation);
            if (relation == DiplomaticRelation.War)
            {
                this[PlayerIdExtensions.ToOriginal(player)].AIValidAttackPlayers[PlayerIdExtensions.ToOriginal(otherPlayer)] = true;
                this[PlayerIdExtensions.ToOriginal(otherPlayer)].AIValidAttackPlayers[PlayerIdExtensions.ToOriginal(player)] = true;
            }

            if (relation is DiplomaticRelation.Peace or DiplomaticRelation.Alliance)
            {
                this[PlayerIdExtensions.ToOriginal(player)].AIValidAttackPlayers[PlayerIdExtensions.ToOriginal(otherPlayer)] = false;
                this[PlayerIdExtensions.ToOriginal(otherPlayer)].AIValidAttackPlayers[PlayerIdExtensions.ToOriginal(player)] = false;
            }

            GetEngine().StrategicMap.OnDiplomaticRelationsChanged(this);
            for (int j = Count - 1; j >= 1; j--)
            {
                this[j].MagicControl.UpdateEnemyEnchantments();
            }

            GetEngine().StrategicMap.UpdateVisibility();
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
        foreach (Player player in this)
        {
            player.BeginUpdate();
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
    public Player? GetPlayers(int index) => index >= 0 && index < Count ? this[index] : null;

    public Player? GetPlayers(PlayerId player) => GetPlayers(player.Index);

    public Player? GetPlayers(PlayerId? player) => player is PlayerId id ? GetPlayers(id) : null;

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
                    result = GetDiplomaticRelation(PlayerId.FromOriginal((sbyte)ActivePlayers[j]), otherPlayer: PlayerId.FromOriginal((sbyte)item)) == DiplomaticRelation.Alliance;
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
    public byte NumberOfPlayers => (byte)(Count - 1);

    // Player.PAS:1622
    // { Original uses <> Count and assumes at least the independent player exists. }
    public int RaceToPlayer(int race) => this.FindIndex(startIndex: 1, match: player => player.PlayerRace == race);

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
            player.SetPlayerIndex(PlayerId.FromOriginal((sbyte)Count));
            if (Count == 0)
            {
                player.PlayerType = PlayerType.Independent;
            }

            player.PlayerRace = (sbyte)(Count - 1);
            player.SetArrayOwner(this, index: Count);
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
        Int32List available = [];
        Int32List invalid = [];
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
        GamePack gamePack = GetEngine().GamePack;
        while (i != gamePack.RaceResources.Count && defaultRace == -1)
        {
            if (gamePack.RaceResources.GetRaceResource(i) is not null)
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
                this[invalid[i]].SetRace(defaultRace.TruncateToSByte());
            }
            else
            {
                int item = available[0];
                this[invalid[i]].SetRace(item.TruncateToSByte());
                available.Delete(0);
            }
        }
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

    // Player.PAS:1737
    // { The search index is the function result. }
    public int IndexOfPlayerType(PlayerType playerType)
        => this.FindLastIndex(playerType, match: static (sought, player) => player.PlayerType == sought);

    // intf_P1_03.inc:319
    public void Update()
    {
        if (GetEngine().StrategicMap.PlayerCommunicator.PlayerCommunicationControl is null)
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
                _ = ActivePlayers.Add(i.TruncateToByte());
            }

            if (player.GetLocal())
            {
                _ = _localControlledPlayers.Add(i.TruncateToByte());
                if (player.PlayerType == PlayerType.Human)
                {
                    _ = LocalHumanPlayers.Add(i.TruncateToByte());
                }
            }
            if (player.PlayerType == PlayerType.Human)
            {
                _ = LocalPlayers.Add(i.TruncateToByte());
            }
        }
    }
}
