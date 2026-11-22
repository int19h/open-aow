#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from AoWE.pas.
using System.Collections.Immutable;
using Aow.Engine;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Magic;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Session;

// { Interface declaration run owned by P1. }
// race, defaults, player relations, enabled
public sealed class Race(CustomNode owner) : EngineObject(owner), IOwned<RaceList>, IOwnedSink<RaceList>
{

    // The race list the race belongs to. AcceptsOwner admits only a RaceList, so the cast never fails.
    public new RaceList Owner => GetOwner<RaceList>();

    protected sealed override bool AcceptsOwner(CustomNode owner) => owner is RaceList;

    void IOwnedSink<RaceList>.SetOwner(RaceList owner) => SetOwner(owner);

    public new const int ClassId = 131_586;

    public int RaceIndex { get; set; } = -1;

    public ByteList DefaultRelations { get; } = [];

    // TStatus values
    private readonly Int32List _playerRelations = [];

    public bool Enabled { get; set; }

    // Ability.PAS:221
    public override void Dispose()
    {
        SetEnabled(false);
        _playerRelations?.Dispose();
        DefaultRelations?.Dispose();
        base.Dispose();
    }

    // Ability.PAS:343
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        RaceIndex = persister.UnsignedByte(name: "RaceIndex", value: RaceIndex.TruncateToSByte());
        persister.Int32List(name: "PlayerRelations", list: _playerRelations);
        if (persister.Loading)
        {
            // { Original leaves this local uninitialized if property 4 is absent. }
            bool enabled = default;
            enabled = persister.Boolean("Enabled", value: enabled);
            SetEnabled(enabled);
        }
        else
        {
            Enabled = persister.Boolean("Enabled", value: Enabled);
        }

        persister.ByteList(name: "DefaultRelations", list: DefaultRelations);
        // The translate stream's mode holds read, so the original ReadWrite resets the default
        // relations there too.
        if ((persister.Loading || persister.Translating()) && DefaultRelations.Count == 0)
        {
            ResetDefaultRaceRelations();
        }
    }

    // Ability.PAS:361
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 537001985)
        {
            NewDay();
        }
    }

    // Ability.PAS:229
    public int GetRelationValue(int race)
    {
        int player = StrategicMap.Current.Players.RaceToPlayer(race);
        int result = player == -1 ? StatusToStatusValue((Status)DefaultRelations[race]) : _playerRelations[player - 1];

        return result + Owner.RelationOffset;
    }

    // Ability.PAS:239
    public Status GetRelation(int race)
    {
        int player = StrategicMap.Current.Players.RaceToPlayer(race);
        return player == -1
            ? StatusValueToStatus(StatusToStatusValue((Status)DefaultRelations[race]) + StrategicMap.Current.Races.RelationOffset)
            : StatusValueToStatus(_playerRelations[player - 1] + StrategicMap.Current.Races.RelationOffset);
    }

    // Ability.PAS:249
    public void SetPlayerRelationValue(sbyte player, int value)
    {
        if (player > 0)
        {
            // { Original dereferences AoWHSMap before the nil check below. }
            _playerRelations[player - 1] = value - StrategicMap.Current.Races.RelationOffset;
            StrategicMap.Current?.TriggerRelationsChanged();
        }
    }

    // Ability.PAS:259
    public int GetPlayerRelationValue(sbyte player) => player > 0 ? _playerRelations[player - 1] + StrategicMap.Current.Races.RelationOffset : 50;

    // Ability.PAS:265
    public Status GetPlayerRelation(sbyte player) => player > 0 ? StatusValueToStatus(_playerRelations[player - 1] + StrategicMap.Current.Races.RelationOffset) : Status.Neutral;

    // Ability.PAS:271
    public void NewDay()
    {
        if (StrategicMap.Current.Day != 1)
        {
            if ((StrategicMap.Current.GlobalMagicControl.ActiveEnchantments & (GlobalEnchantmentTypes.Hatred | GlobalEnchantmentTypes.Harmony)) == GlobalEnchantmentTypes.None)
            {
                for (int i = 0, last = _playerRelations.Count - 1; i <= last; i++)
                {
                    Player players = StrategicMap.Current.Players.GetPlayers(i + 1);
                    int defaultValue = StatusToStatusValue((Status)DefaultRelations[players.PlayerRace]);
                    if (_playerRelations[i] > defaultValue)
                    {
                        int item = _playerRelations[i];
                        _playerRelations[i] = item - 1;
                        if (_playerRelations[i] < defaultValue)
                        {
                            _playerRelations[i] = defaultValue;
                        }
                    }

                    if (_playerRelations[i] < defaultValue)
                    {
                        int item = _playerRelations[i];
                        _playerRelations[i] = item + 1;
                        if (_playerRelations[i] > defaultValue)
                        {
                            _playerRelations[i] = defaultValue;
                        }
                    }
                }
            }
        }
        else
        {
            ResetPlayerRelations();
        }
    }

    // Ability.PAS:298
    public Status GetDefaultRaceRelation(int race) => race >= 0 && race < DefaultRelations.Count ? (Status)DefaultRelations[race] : Status.Neutral;

    // Ability.PAS:308
    public void ResetDefaultRaceRelations()
    {
        DefaultRelations.Clear();
        if (RaceIndex is >= 0 and <= 11)
        {
            for (int i = 0; i <= 11; i++)
            {
                _ = DefaultRelations.Add((byte)(int)s_defaultRaceRelations[RaceIndex][i]);
            }
        }
    }

    // Ability.PAS:317
    public void ResetPlayerRelations()
    {
        _playerRelations.Clear();
        for (int i = 1, last = StrategicMap.Current.Players.GetCount() - 1; i <= last; i++)
        {
            Player players = StrategicMap.Current.Players.GetPlayers(i);
            int statusToStatusValue = StatusToStatusValue((Status)DefaultRelations[players.PlayerRace]);
            _ = _playerRelations.Add(statusToStatusValue);
        }
    }

    // Ability.PAS:326
    public void SetRace(int value)
    {
        RaceIndex = value;
        ResetDefaultRaceRelations();
    }

    // Ability.PAS:332
    public void SetEnabled(bool value)
    {
        if (value != Enabled)
        {
            if (GamePack.Current.RaceResources.GetRaceResource(RaceIndex) is not null)
            {
                _ = value
                    ? GamePack.Current.RaceResources.GetRaceResource(RaceIndex).AddRef()
                    : GamePack.Current.RaceResources.GetRaceResource(RaceIndex).Release();

                Enabled = value;
            }
        }
    }

    private static readonly ImmutableArray<ImmutableArray<Status>> s_defaultRaceRelations = [[Status.Friendly, Status.Polite, Status.Polite, Status.Polite, Status.Neutral, Status.Neutral, Status.Neutral, Status.Polite, Status.Neutral, Status.Neutral, Status.Neutral, Status.Wary], [Status.Polite, Status.Friendly, Status.Polite, Status.Neutral, Status.Neutral, Status.Neutral, Status.Neutral, Status.Wary, Status.Neutral, Status.Neutral, Status.Neutral, Status.Wary], [Status.Polite, Status.Polite, Status.Friendly, Status.Polite, Status.Neutral, Status.Neutral, Status.Neutral, Status.Wary, Status.Neutral, Status.Neutral, Status.Neutral, Status.Wary], [Status.Polite, Status.Neutral, Status.Polite, Status.Friendly, Status.Neutral, Status.Neutral, Status.Neutral, Status.Wary, Status.Neutral, Status.Neutral, Status.Neutral, Status.Wary], [Status.Neutral, Status.Neutral, Status.Neutral, Status.Neutral, Status.Friendly, Status.Friendly, Status.Polite, Status.Friendly, Status.Hostile, Status.Hostile, Status.Hostile, Status.Hostile], [Status.Neutral, Status.Neutral, Status.Neutral, Status.Neutral, Status.Friendly, Status.Friendly, Status.Friendly, Status.Polite, Status.Hostile, Status.Hostile, Status.Hostile, Status.Hostile], [Status.Neutral, Status.Neutral, Status.Neutral, Status.Neutral, Status.Polite, Status.Friendly, Status.Friendly, Status.Friendly, Status.Hostile, Status.Hostile, Status.Hostile, Status.Hostile], [Status.Polite, Status.Wary, Status.Wary, Status.Wary, Status.Friendly, Status.Polite, Status.Friendly, Status.Friendly, Status.Hostile, Status.Hostile, Status.Hostile, Status.Hostile], [Status.Neutral, Status.Neutral, Status.Neutral, Status.Neutral, Status.Hostile, Status.Hostile, Status.Hostile, Status.Hostile, Status.Friendly, Status.Friendly, Status.Polite, Status.Polite], [Status.Neutral, Status.Neutral, Status.Neutral, Status.Neutral, Status.Hostile, Status.Hostile, Status.Hostile, Status.Hostile, Status.Friendly, Status.Friendly, Status.Friendly, Status.Polite], [Status.Neutral, Status.Neutral, Status.Neutral, Status.Neutral, Status.Hostile, Status.Hostile, Status.Hostile, Status.Hostile, Status.Polite, Status.Friendly, Status.Friendly, Status.Polite], [Status.Wary, Status.Wary, Status.Wary, Status.Wary, Status.Hostile, Status.Hostile, Status.Hostile, Status.Hostile, Status.Polite, Status.Polite, Status.Polite, Status.Friendly]];

    private static int StatusToStatusValue(Status status)
    {
        switch ((int)status)
        {
            case 0:
                return 10;
            case 1:
                return 30;
            case 2:
                return 50;
            case 3:
                return 70;
            case 4:
                return 90;
            default:
                _ = new ArgumentException("Invalid TStatus Value");
                break;
        }
        return 50;
    }

    internal static Status StatusValueToStatus(int value)
    {
        if (value <= 20)
        {
            return Status.Hostile;
        }
        else if (value > 80)
        {
            return Status.Friendly;
        }
        else
        {
            switch (value)
            {
                case 21:
                case 22:
                case 23:
                case 24:
                case 25:
                case 26:
                case 27:
                case 28:
                case 29:
                case 30:
                case 31:
                case 32:
                case 33:
                case 34:
                case 35:
                case 36:
                case 37:
                case 38:
                case 39:
                case 40:
                    return Status.Wary;
                case 41:
                case 42:
                case 43:
                case 44:
                case 45:
                case 46:
                case 47:
                case 48:
                case 49:
                case 50:
                case 51:
                case 52:
                case 53:
                case 54:
                case 55:
                case 56:
                case 57:
                case 58:
                case 59:
                case 60:
                    return Status.Neutral;
                case 61:
                case 62:
                case 63:
                case 64:
                case 65:
                case 66:
                case 67:
                case 68:
                case 69:
                case 70:
                case 71:
                case 72:
                case 73:
                case 74:
                case 75:
                case 76:
                case 77:
                case 78:
                case 79:
                case 80:
                    return Status.Polite;
                default:
                    _ = new ArgumentException("Invalid StatusValue");
                    break;
            }
        }
        return Status.Neutral;
    }
}
