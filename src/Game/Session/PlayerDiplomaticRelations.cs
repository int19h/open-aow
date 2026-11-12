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
        Relations = new ByteList();
        History = new ENode(null);
    }

    public Player Player;
    public ByteList Relations;
    public ENode History;

    // Player.PAS:85
    public override void Dispose()
    {
        Relations?.Dispose();
        History?.Dispose();
        base.Dispose();
    }

    // Player.PAS:92
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwStreamProc(20, Relations.LoadFromStream, Relations.SaveToStream);
        stream.rwEObject(History, 21);
        if (stream.StorageMode().Contains(StorageModeElement.Read))
        {
            Relations.Default = (int)DiplomaticRelation.Unknown;
        }
    }

    // Player.PAS:127
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 536870920)
        {
            ValidateHistory();
            if (Player.LegacyNewRelations.Count != 0)
            {
                Relations.Assign(Player.LegacyNewRelations);
                Player.LegacyNewRelations.Clear();
            }

            if (AoWHSMap.Current.Day == 0 && !AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
            {
                for (int I = 1, last = AoWHSMap.Current.Players.Count - 1; I <= last; I++)
                {
                    // { The loop index supplies the player. }
                    var diplomaticRelation = GetDiplomaticRelation((sbyte)I);
                    GetHistory((sbyte)I).Add(diplomaticRelation, 0);
                }
            }
            else
            {
                if (Player.LegacyRelations.Count != 0)
                {
                    for (int I = 1, last = AoWHSMap.Current.Players.Count - 1; I <= last; I++)
                    {
                        var diplomaticRelation = Player.LegacyRelations.GetDiplomaticRelation(I);
                        GetHistory((sbyte)I).Add(diplomaticRelation, 0);
                    }

                    Player.LegacyRelations.Clear();
                }
            }
        }
    }

    // Player.PAS:100
    public DiplomaticRelation GetDiplomaticRelation(sbyte player)
    {
        DiplomaticRelation result;
        if (player < Relations.Count)
        {
            result = (DiplomaticRelation)Relations.List[player];
        }
        else
        {
            result = DiplomaticRelation.Unknown;
        }

        return result;
    }

    // Player.PAS:107
    public void SetDiplomaticRelation(sbyte player, DiplomaticRelation value)
    {
        Relations[player] = (byte)value;
        if (!AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting) && AoWHSMap.Current.Day > 0)
        {
            GetHistory(player).Add(value, AoWHSMap.Current.Day);
        }
    }

    // Player.PAS:114
    public DiplomaticRelationHistoryList GetHistory(sbyte player) => (DiplomaticRelationHistoryList)History._children.List[player - 1];

    // Player.PAS:119
    public void ValidateHistory()
    {
        while (History.Count + 1 < AoWHSMap.Current.Players.Count)
        {
            new DiplomaticRelationHistoryList(null).Owner = History;
        }

        while (History.Count + 1 > AoWHSMap.Current.Players.Count)
        {
            var count = History.Count;
            History.Children[count - 1]?.Dispose();
        }
    }

    // intf_P1_03.inc:79

}
