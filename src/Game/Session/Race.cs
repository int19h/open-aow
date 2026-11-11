// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Magic;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Session;

// { Interface declaration run owned by P1. }
// race, defaults, player relations, enabled
public partial class Race : EngineObject
{
    public new const int ClassID = 131586;

    // Ability.PAS:212
    public Race(ECustomNode owner) : base(owner)
    {
        FPlayerRelations = new Int32List();
        FDefaultRelations = new ByteList();
        FRace = -1;
        FEnabled = false;
    }

    public sbyte FRace;
    public ByteList FDefaultRelations;
    // TStatus values
    public Int32List FPlayerRelations;
    public bool FEnabled;

    // Ability.PAS:221
    public override void Dispose()
    {
        SetEnabled(false);
        FPlayerRelations?.Dispose();
        FDefaultRelations?.Dispose();
        base.Dispose();
    }

    // Ability.PAS:343
    public override void ReadWrite(EStorageStream Stream)
    {
        bool Enabled = default;
        base.ReadWrite(Stream);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FRace), 2);
        Stream.rwStreamProc(3, FPlayerRelations.LoadFromStream, FPlayerRelations.SaveToStream);
        if ((Stream.StorageMode() & Set<StorageModeElement>.Of(StorageModeElement.Read, StorageModeElement.TextOnly, StorageModeElement.Compare)) == Set<StorageModeElement>.Of(StorageModeElement.Read))
        {
            // { Original leaves this local uninitialized if property 4 is absent. }
            Stream.rwBool(ref Enabled, 4);
            SetEnabled(Enabled);
        }
        else
        {
            Stream.rwBool(ref FEnabled, 4);
        }

        Stream.rwStreamProc(5, FDefaultRelations.LoadFromStream, FDefaultRelations.SaveToStream);
        if (Stream.StorageMode().Contains(StorageModeElement.Read) && FDefaultRelations.Count == 0)
        {
            ResetDefaultRaceRelations();
        }
    }

    // Ability.PAS:361
    public override void MsgProc(ref EngineMessage Msg, EngineObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 537001985)
        {
            NewDay();
        }
    }

    // Ability.PAS:229
    public int GetRelationValue(sbyte Race)
    {
        int Result;
        int Player;
        Player = AoWHSMap.Current.FPlayers.RaceToPlayer(Race);
        if (Player == -1)
        {
            Result = StatusToStatusValue((Status)FDefaultRelations[Race]);
        }
        else
        {
            Result = FPlayerRelations[Player - 1];
        }

        return Result + ((RaceList)Owner).FRelationOffset;
    }

    // Ability.PAS:239
    public Status GetRelation(sbyte Race)
    {
        Status Result;
        int Player;
        Player = AoWHSMap.Current.FPlayers.RaceToPlayer(Race);
        if (Player == -1)
        {
            Result = StatusValueToStatus(StatusToStatusValue((Status)FDefaultRelations[Race]) + AoWHSMap.Current.FRaces.FRelationOffset);
        }
        else
        {
            Result = StatusValueToStatus(FPlayerRelations[Player - 1] + AoWHSMap.Current.FRaces.FRelationOffset);
        }

        return Result;
    }

    // Ability.PAS:249
    public void SetPlayerRelationValue(sbyte Player, int Value)
    {
        if (Player > 0)
        {
            // { Original dereferences AoWHSMap before the nil check below. }
            FPlayerRelations[Player - 1] = Value - AoWHSMap.Current.FRaces.FRelationOffset;
            if (AoWHSMap.Current != null)
            {
                AoWHSMap.Current.TriggerRelationsChanged();
            }
        }
    }

    // Ability.PAS:259
    public int GetPlayerRelationValue(sbyte Player)
    {
        int Result;
        if (Player > 0)
        {
            Result = FPlayerRelations[Player - 1] + AoWHSMap.Current.FRaces.FRelationOffset;
        }
        else
        {
            Result = 50;
        }

        return Result;
    }

    // Ability.PAS:265
    public Status GetPlayerRelation(sbyte Player)
    {
        Status Result;
        if (Player > 0)
        {
            Result = StatusValueToStatus(FPlayerRelations[Player - 1] + AoWHSMap.Current.FRaces.FRelationOffset);
        }
        else
        {
            Result = Status.Neutral;
        }

        return Result;
    }

    // Ability.PAS:271
    public void NewDay()
    {
        int DefaultValue;
        if (AoWHSMap.Current.FDay != 1)
        {
            if ((AoWHSMap.Current.FGlobalMagicControl.FActiveEnchantments & Set<GlobalEnchantmentType>.Of(GlobalEnchantmentType.Hatred, GlobalEnchantmentType.Harmony)) == Set<GlobalEnchantmentType>.Of())
            {
                for (int I = 0, last = FPlayerRelations.Count - 1; I <= last; I++)
                {
                    var players = AoWHSMap.Current.FPlayers.GetPlayers(I + 1);
                    DefaultValue = StatusToStatusValue((Status)FDefaultRelations[players.FPlayerRace]);
                    if (FPlayerRelations[I] > DefaultValue)
                    {
                        var item = FPlayerRelations[I];
                        FPlayerRelations[I] = item - 1;
                        if (FPlayerRelations[I] < DefaultValue)
                        {
                            FPlayerRelations[I] = DefaultValue;
                        }
                    }

                    if (FPlayerRelations[I] < DefaultValue)
                    {
                        var item = FPlayerRelations[I];
                        FPlayerRelations[I] = item + 1;
                        if (FPlayerRelations[I] > DefaultValue)
                        {
                            FPlayerRelations[I] = DefaultValue;
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
    public Status GetDefaultRaceRelation(sbyte Race)
    {
        Status Result;
        if (Race >= 0 && Race < FDefaultRelations.Count)
        {
            Result = (Status)FDefaultRelations[Race];
        }
        else
        {
            Result = Status.Neutral;
        }

        return Result;
    }

    // Ability.PAS:308
    public void ResetDefaultRaceRelations()
    {
        FDefaultRelations.Clear();
        if (FRace >= 0 && FRace <= 11)
        {
            for (int I = 0; I <= 11; I++)
            {
                FDefaultRelations.Add((byte)(int)DefaultRaceRelations[FRace][I]);
            }
        }
    }

    // Ability.PAS:317
    public void ResetPlayerRelations()
    {
        FPlayerRelations.Clear();
        for (int I = 1, last = AoWHSMap.Current.FPlayers.GetCount() - 1; I <= last; I++)
        {
            var players = AoWHSMap.Current.FPlayers.GetPlayers(I);
            var statusToStatusValue = StatusToStatusValue((Status)FDefaultRelations[players.FPlayerRace]);
            FPlayerRelations.Add(statusToStatusValue);
        }
    }

    // Ability.PAS:326
    public void SetRace(sbyte Value)
    {
        FRace = Value;
        ResetDefaultRaceRelations();
    }

    // Ability.PAS:332
    public void SetEnabled(bool Value)
    {
        if (Value != FEnabled)
        {
            if (AoWHSSet.Current.FRaceResources.GetRaceResource(FRace) != null)
            {
                if (Value)
                {
                    AoWHSSet.Current.FRaceResources.GetRaceResource(FRace).AddRef();
                }
                else
                {
                    AoWHSSet.Current.FRaceResources.GetRaceResource(FRace).Release();
                }

                FEnabled = Value;
            }
        }
    }

    private static StatusArray12By12 DefaultRaceRelations = StatusArray12By12.From([StatusArray12.From([Status.Friendly, Status.Polite, Status.Polite, Status.Polite, Status.Neutral, Status.Neutral, Status.Neutral, Status.Polite, Status.Neutral, Status.Neutral, Status.Neutral, Status.Wary]), StatusArray12.From([Status.Polite, Status.Friendly, Status.Polite, Status.Neutral, Status.Neutral, Status.Neutral, Status.Neutral, Status.Wary, Status.Neutral, Status.Neutral, Status.Neutral, Status.Wary]), StatusArray12.From([Status.Polite, Status.Polite, Status.Friendly, Status.Polite, Status.Neutral, Status.Neutral, Status.Neutral, Status.Wary, Status.Neutral, Status.Neutral, Status.Neutral, Status.Wary]), StatusArray12.From([Status.Polite, Status.Neutral, Status.Polite, Status.Friendly, Status.Neutral, Status.Neutral, Status.Neutral, Status.Wary, Status.Neutral, Status.Neutral, Status.Neutral, Status.Wary]), StatusArray12.From([Status.Neutral, Status.Neutral, Status.Neutral, Status.Neutral, Status.Friendly, Status.Friendly, Status.Polite, Status.Friendly, Status.Hostile, Status.Hostile, Status.Hostile, Status.Hostile]), StatusArray12.From([Status.Neutral, Status.Neutral, Status.Neutral, Status.Neutral, Status.Friendly, Status.Friendly, Status.Friendly, Status.Polite, Status.Hostile, Status.Hostile, Status.Hostile, Status.Hostile]), StatusArray12.From([Status.Neutral, Status.Neutral, Status.Neutral, Status.Neutral, Status.Polite, Status.Friendly, Status.Friendly, Status.Friendly, Status.Hostile, Status.Hostile, Status.Hostile, Status.Hostile]), StatusArray12.From([Status.Polite, Status.Wary, Status.Wary, Status.Wary, Status.Friendly, Status.Polite, Status.Friendly, Status.Friendly, Status.Hostile, Status.Hostile, Status.Hostile, Status.Hostile]), StatusArray12.From([Status.Neutral, Status.Neutral, Status.Neutral, Status.Neutral, Status.Hostile, Status.Hostile, Status.Hostile, Status.Hostile, Status.Friendly, Status.Friendly, Status.Polite, Status.Polite]), StatusArray12.From([Status.Neutral, Status.Neutral, Status.Neutral, Status.Neutral, Status.Hostile, Status.Hostile, Status.Hostile, Status.Hostile, Status.Friendly, Status.Friendly, Status.Friendly, Status.Polite]), StatusArray12.From([Status.Neutral, Status.Neutral, Status.Neutral, Status.Neutral, Status.Hostile, Status.Hostile, Status.Hostile, Status.Hostile, Status.Polite, Status.Friendly, Status.Friendly, Status.Polite]), StatusArray12.From([Status.Wary, Status.Wary, Status.Wary, Status.Wary, Status.Hostile, Status.Hostile, Status.Hostile, Status.Hostile, Status.Polite, Status.Polite, Status.Polite, Status.Friendly])]);

    private static int StatusToStatusValue(Status Status)
    {
        int result;
        switch ((int)Status)
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

    internal static Status StatusValueToStatus(int Value)
    {
        Status result;
        if (Value <= 20)
        {
            result = Status.Hostile;
        }
        else if (Value > 80)
        {
            result = Status.Friendly;
        }
        else
        {
            switch (Value)
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
