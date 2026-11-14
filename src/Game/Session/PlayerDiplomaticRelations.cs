// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Formats.Tree;

using Aow.Game.World.Map;

namespace Aow.Game.Session;

// player, relations, history
public partial class PlayerDiplomaticRelations : EngineObject
{
    // Player.PAS:78
    public PlayerDiplomaticRelations(ECustomNode owner) : base(owner)
    {
        _relations = new ByteList();
        _history = new ENode(null) { Holder = CreationHolder() };
    }

    public Player Player { get; set; }

    private readonly ByteList _relations;
    private readonly ENode _history;

    // Player.PAS:85
    public override void Dispose()
    {
        _relations?.Dispose();
        _history?.Dispose();
        base.Dispose();
    }

    // Player.PAS:92
    // A load resets the default to Unknown after the list, as the original ReadWrite does.
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        PersistSupport.ByteList(persister, "Relations", _relations);
        persister.Object("History", _history);
        // The translate stream's mode holds read, so the original ReadWrite resets it there too.
        if (persister.Loading || PersistSupport.Translating(persister))
        {
            _relations.Default = (byte)DiplomaticRelation.Unknown;
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
                _relations.Assign(Player.LegacyNewRelations);
                Player.LegacyNewRelations.Clear();
            }

            if (StrategicMap.Current.Day == 0 && !StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
            {
                for (int I = 1, last = StrategicMap.Current.Players.Count - 1; I <= last; I++)
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
                    for (int I = 1, last = StrategicMap.Current.Players.Count - 1; I <= last; I++)
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
        if (player < _relations.Count)
        {
            result = (DiplomaticRelation)_relations.List[player];
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
        _relations[player] = (byte)value;
        if (!StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting) && StrategicMap.Current.Day > 0)
        {
            GetHistory(player).Add(value, StrategicMap.Current.Day);
        }
    }

    // Player.PAS:114
    public DiplomaticRelationHistoryList GetHistory(sbyte player) => (DiplomaticRelationHistoryList)_history.GetChild(player - 1);

    // Player.PAS:119
    public void ValidateHistory()
    {
        while (_history.Count + 1 < StrategicMap.Current.Players.Count)
        {
            new DiplomaticRelationHistoryList(null) { Holder = CreationHolder() }.Owner = _history;
        }

        while (_history.Count + 1 > StrategicMap.Current.Players.Count)
        {
            var count = _history.Count;
            _history.Children[count - 1]?.Dispose();
        }
    }

    // intf_P1_03.inc:79

}
