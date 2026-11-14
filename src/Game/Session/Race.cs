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
public partial class Race : EngineObject, IOwned<RaceList>, IOwnedSink<RaceList>
{
    // The race list the race belongs to. AcceptsOwner admits only a RaceList, so the cast never fails.
    public new RaceList Owner { get => (RaceList)base.Owner; set => SetOwner(value); }

    protected sealed override bool AcceptsOwner(ECustomNode owner) => owner is RaceList;

    void IOwnedSink<RaceList>.SetOwner(RaceList owner) => SetOwner(owner);

    public new const int ClassId = 131586;

    // Ability.PAS:212
    public Race(ECustomNode owner) : base(owner)
    {
        _playerRelations = new Int32List();
        DefaultRelations = new ByteList();
        RaceIndex = -1;
        Enabled = false;
    }

    public sbyte RaceIndex { get; set; }

    public ByteList DefaultRelations { get; }

    // TStatus values
    private readonly Int32List _playerRelations;

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
        bool enabled = default;
        base.PersistFields(persister);
        RaceIndex = PersistSupport.UnsignedByte(persister, name: "RaceIndex", value: RaceIndex);
        PersistSupport.Int32List(persister, name: "PlayerRelations", list: _playerRelations);
        if (persister.Loading)
        {
            // { Original leaves this local uninitialized if property 4 is absent. }
            enabled = persister.Boolean("Enabled", value: enabled);
            SetEnabled(enabled);
        }
        else
        {
            Enabled = persister.Boolean("Enabled", value: Enabled);
        }

        PersistSupport.ByteList(persister, name: "DefaultRelations", list: DefaultRelations);
        // The translate stream's mode holds read, so the original ReadWrite resets the default
        // relations there too.
        if ((persister.Loading || PersistSupport.Translating(persister)) && DefaultRelations.Count == 0)
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
    public int GetRelationValue(sbyte race)
    {
        int result;
        int player;
        player = StrategicMap.Current.Players.RaceToPlayer(race);
        if (player == -1)
        {
            result = StatusToStatusValue((Status)DefaultRelations[race]);
        }
        else
        {
            result = _playerRelations[player - 1];
        }

        return result + Owner.RelationOffset;
    }

    // Ability.PAS:239
    public Status GetRelation(sbyte race)
    {
        Status result;
        int player;
        player = StrategicMap.Current.Players.RaceToPlayer(race);
        if (player == -1)
        {
            result = StatusValueToStatus(StatusToStatusValue((Status)DefaultRelations[race]) + StrategicMap.Current.Races.RelationOffset);
        }
        else
        {
            result = StatusValueToStatus(_playerRelations[player - 1] + StrategicMap.Current.Races.RelationOffset);
        }

        return result;
    }

    // Ability.PAS:249
    public void SetPlayerRelationValue(sbyte player, int value)
    {
        if (player > 0)
        {
            // { Original dereferences AoWHSMap before the nil check below. }
            _playerRelations[player - 1] = value - StrategicMap.Current.Races.RelationOffset;
            if (StrategicMap.Current != null)
            {
                StrategicMap.Current.TriggerRelationsChanged();
            }
        }
    }

    // Ability.PAS:259
    public int GetPlayerRelationValue(sbyte player)
    {
        int result;
        if (player > 0)
        {
            result = _playerRelations[player - 1] + StrategicMap.Current.Races.RelationOffset;
        }
        else
        {
            result = 50;
        }

        return result;
    }

    // Ability.PAS:265
    public Status GetPlayerRelation(sbyte player)
    {
        Status result;
        if (player > 0)
        {
            result = StatusValueToStatus(_playerRelations[player - 1] + StrategicMap.Current.Races.RelationOffset);
        }
        else
        {
            result = Status.Neutral;
        }

        return result;
    }

    // Ability.PAS:271
    public void NewDay()
    {
        int defaultValue;
        if (StrategicMap.Current.Day != 1)
        {
            if ((StrategicMap.Current.GlobalMagicControl.ActiveEnchantments & Set<GlobalEnchantmentType>.Of(GlobalEnchantmentType.Hatred, GlobalEnchantmentType.Harmony)) == Set<GlobalEnchantmentType>.Of())
            {
                for (int I = 0, last = _playerRelations.Count - 1; I <= last; I++)
                {
                    var players = StrategicMap.Current.Players.GetPlayers(I + 1);
                    defaultValue = StatusToStatusValue((Status)DefaultRelations[players.PlayerRace]);
                    if (_playerRelations[I] > defaultValue)
                    {
                        var item = _playerRelations[I];
                        _playerRelations[I] = item - 1;
                        if (_playerRelations[I] < defaultValue)
                        {
                            _playerRelations[I] = defaultValue;
                        }
                    }

                    if (_playerRelations[I] < defaultValue)
                    {
                        var item = _playerRelations[I];
                        _playerRelations[I] = item + 1;
                        if (_playerRelations[I] > defaultValue)
                        {
                            _playerRelations[I] = defaultValue;
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
    public Status GetDefaultRaceRelation(sbyte race)
    {
        Status result;
        if (race >= 0 && race < DefaultRelations.Count)
        {
            result = (Status)DefaultRelations[race];
        }
        else
        {
            result = Status.Neutral;
        }

        return result;
    }

    // Ability.PAS:308
    public void ResetDefaultRaceRelations()
    {
        DefaultRelations.Clear();
        if (RaceIndex >= 0 && RaceIndex <= 11)
        {
            for (int I = 0; I <= 11; I++)
            {
                DefaultRelations.Add((byte)(int)s_defaultRaceRelations[RaceIndex][I]);
            }
        }
    }

    // Ability.PAS:317
    public void ResetPlayerRelations()
    {
        _playerRelations.Clear();
        for (int I = 1, last = StrategicMap.Current.Players.GetCount() - 1; I <= last; I++)
        {
            var players = StrategicMap.Current.Players.GetPlayers(I);
            var statusToStatusValue = StatusToStatusValue((Status)DefaultRelations[players.PlayerRace]);
            _playerRelations.Add(statusToStatusValue);
        }
    }

    // Ability.PAS:326
    public void SetRace(sbyte value)
    {
        RaceIndex = value;
        ResetDefaultRaceRelations();
    }

    // Ability.PAS:332
    public void SetEnabled(bool value)
    {
        if (value != Enabled)
        {
            if (GamePack.Current.RaceResources.GetRaceResource(RaceIndex) != null)
            {
                if (value)
                {
                    GamePack.Current.RaceResources.GetRaceResource(RaceIndex).AddRef();
                }
                else
                {
                    GamePack.Current.RaceResources.GetRaceResource(RaceIndex).Release();
                }

                Enabled = value;
            }
        }
    }

    private static readonly ImmutableArray<ImmutableArray<Status>> s_defaultRaceRelations = [[Status.Friendly, Status.Polite, Status.Polite, Status.Polite, Status.Neutral, Status.Neutral, Status.Neutral, Status.Polite, Status.Neutral, Status.Neutral, Status.Neutral, Status.Wary], [Status.Polite, Status.Friendly, Status.Polite, Status.Neutral, Status.Neutral, Status.Neutral, Status.Neutral, Status.Wary, Status.Neutral, Status.Neutral, Status.Neutral, Status.Wary], [Status.Polite, Status.Polite, Status.Friendly, Status.Polite, Status.Neutral, Status.Neutral, Status.Neutral, Status.Wary, Status.Neutral, Status.Neutral, Status.Neutral, Status.Wary], [Status.Polite, Status.Neutral, Status.Polite, Status.Friendly, Status.Neutral, Status.Neutral, Status.Neutral, Status.Wary, Status.Neutral, Status.Neutral, Status.Neutral, Status.Wary], [Status.Neutral, Status.Neutral, Status.Neutral, Status.Neutral, Status.Friendly, Status.Friendly, Status.Polite, Status.Friendly, Status.Hostile, Status.Hostile, Status.Hostile, Status.Hostile], [Status.Neutral, Status.Neutral, Status.Neutral, Status.Neutral, Status.Friendly, Status.Friendly, Status.Friendly, Status.Polite, Status.Hostile, Status.Hostile, Status.Hostile, Status.Hostile], [Status.Neutral, Status.Neutral, Status.Neutral, Status.Neutral, Status.Polite, Status.Friendly, Status.Friendly, Status.Friendly, Status.Hostile, Status.Hostile, Status.Hostile, Status.Hostile], [Status.Polite, Status.Wary, Status.Wary, Status.Wary, Status.Friendly, Status.Polite, Status.Friendly, Status.Friendly, Status.Hostile, Status.Hostile, Status.Hostile, Status.Hostile], [Status.Neutral, Status.Neutral, Status.Neutral, Status.Neutral, Status.Hostile, Status.Hostile, Status.Hostile, Status.Hostile, Status.Friendly, Status.Friendly, Status.Polite, Status.Polite], [Status.Neutral, Status.Neutral, Status.Neutral, Status.Neutral, Status.Hostile, Status.Hostile, Status.Hostile, Status.Hostile, Status.Friendly, Status.Friendly, Status.Friendly, Status.Polite], [Status.Neutral, Status.Neutral, Status.Neutral, Status.Neutral, Status.Hostile, Status.Hostile, Status.Hostile, Status.Hostile, Status.Polite, Status.Friendly, Status.Friendly, Status.Polite], [Status.Wary, Status.Wary, Status.Wary, Status.Wary, Status.Hostile, Status.Hostile, Status.Hostile, Status.Hostile, Status.Polite, Status.Polite, Status.Polite, Status.Friendly]];

    private static int StatusToStatusValue(Status status)
    {
        int result;
        switch ((int)status)
        {
            case 0:
                result = 10;
                break;
            case 1:
                result = 30;
                break;
            case 2:
                result = 50;
                break;
            case 3:
                result = 70;
                break;
            case 4:
                result = 90;
                break;
            default:
                result = 50;
                new ArgumentException("Invalid TStatus Value");
                break;
        }
        return result;
    }

    internal static Status StatusValueToStatus(int value)
    {
        Status result;
        if (value <= 20)
        {
            result = Status.Hostile;
        }
        else if (value > 80)
        {
            result = Status.Friendly;
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
                    result = Status.Wary;
                    break;
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
                    result = Status.Neutral;
                    break;
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
                    result = Status.Polite;
                    break;
                default:
                    result = Status.Neutral;
                    new ArgumentException("Invalid StatusValue");
                    break;
            }
        }
        return result;
    }

    // intf_P1_03.inc:13

    // intf_P1_03.inc:13

}
