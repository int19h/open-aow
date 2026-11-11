// Translated from AoWE.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.World.Map;

namespace Aow.Game.Session;

// player, relations, history
public partial class PlayerDiplomaticRelations : EngineObject
{
    // Player.PAS:78
    public PlayerDiplomaticRelations(ECustomNode owner) : base(owner)
    {
        FRelations = new ByteList();
        FHistory = new ENode(null);
    }

    public Player FPlayer;
    public ByteList FRelations;
    public ENode FHistory;

    // Player.PAS:85
    public override void Dispose()
    {
        FRelations?.Dispose();
        FHistory?.Dispose();
        base.Dispose();
    }

    // Player.PAS:92
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwStreamProc(20, FRelations.LoadFromStream, FRelations.SaveToStream);
        Stream.rwEObject(FHistory, 21);
        if (Stream.StorageMode().Contains(StorageModeElement.Read))
        {
            FRelations.Default = (int)DiplomaticRelation.Unknown;
        }
    }

    // Player.PAS:127
    public override void MsgProc(ref EngineMessage Msg, EngineObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 536870920)
        {
            ValidateHistory();
            if (FPlayer.FLegacyNewRelations.Count != 0)
            {
                FRelations.Assign(FPlayer.FLegacyNewRelations);
                FPlayer.FLegacyNewRelations.Clear();
            }

            if (AoWHSMap.Current.FDay == 0 && !AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
            {
                for (int I = 1, last = AoWHSMap.Current.FPlayers.Count - 1; I <= last; I++)
                {
                    // { The loop index supplies the player. }
                    var diplomaticRelation = GetDiplomaticRelation((sbyte)I);
                    GetHistory((sbyte)I).Add(diplomaticRelation, 0);
                }
            }
            else
            {
                if (FPlayer.FLegacyRelations.Count != 0)
                {
                    for (int I = 1, last = AoWHSMap.Current.FPlayers.Count - 1; I <= last; I++)
                    {
                        var diplomaticRelation = FPlayer.FLegacyRelations.GetDiplomaticRelation(I);
                        GetHistory((sbyte)I).Add(diplomaticRelation, 0);
                    }

                    FPlayer.FLegacyRelations.Clear();
                }
            }
        }
    }

    // Player.PAS:100
    public DiplomaticRelation GetDiplomaticRelation(sbyte Player)
    {
        DiplomaticRelation Result;
        if (Player < FRelations.Count)
        {
            Result = (DiplomaticRelation)FRelations.List[Player];
        }
        else
        {
            Result = DiplomaticRelation.Unknown;
        }

        return Result;
    }

    // Player.PAS:107
    public void SetDiplomaticRelation(sbyte Player, DiplomaticRelation Value)
    {
        FRelations[Player] = (byte)Value;
        if (!AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting) && AoWHSMap.Current.FDay > 0)
        {
            GetHistory(Player).Add(Value, AoWHSMap.Current.FDay);
        }
    }

    // Player.PAS:114
    public DiplomaticRelationHistoryList GetHistory(sbyte Player) => (DiplomaticRelationHistoryList)FHistory.FChildren.List[Player - 1];

    // Player.PAS:119
    public void ValidateHistory()
    {
        while (FHistory.Count + 1 < AoWHSMap.Current.FPlayers.Count)
        {
            new DiplomaticRelationHistoryList(null).Owner = FHistory;
        }

        while (FHistory.Count + 1 > AoWHSMap.Current.FPlayers.Count)
        {
            var count = FHistory.Count;
            FHistory.Children[count - 1]?.Dispose();
        }
    }

    // intf_P1_03.inc:79

}
