#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Formats.Tree;

using Aow.Game.World.Map;

namespace Aow.Game.Session;

// player, relations, history
public sealed class PlayerDiplomaticRelations : EngineObject
{
    // Player.PAS:78
    public PlayerDiplomaticRelations(CustomNode? owner) : base(owner)
    {
        _relations = [];
        _history = new OwningNode(null) { Holder = CreationHolder() };
    }

    public Player Player { get; init; }

    private readonly ByteList _relations;
    private readonly OwningNode _history;

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
        persister.ByteList(name: "Relations", list: _relations);
        persister.Object("History", _history);
        // The translate stream's mode holds read, so the original ReadWrite resets it there too.
        if (persister.Loading || persister.Translating())
        {
            _relations.Default = (byte)DiplomaticRelation.Unknown;
        }
    }

    // Player.PAS:127
    public override void MessageProc(ref EngineMessage message, EngineObject? sender)
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

            if (StrategicMap.Current.Day == 0 && !StrategicMap.Current.Status.Contains(HexSpriteMapStatuses.Editting))
            {
                for (int i = 1, last = StrategicMap.Current.Players.Count - 1; i <= last; i++)
                {
                    // { The loop index supplies the player. }
                    DiplomaticRelation diplomaticRelation = GetDiplomaticRelation((sbyte)i);
                    GetHistory((sbyte)i).Add(diplomaticRelation, day: 0);
                }
            }
            else
            {
                if (Player.LegacyRelations.Count != 0)
                {
                    for (int i = 1, last = StrategicMap.Current.Players.Count - 1; i <= last; i++)
                    {
                        DiplomaticRelation diplomaticRelation = Player.LegacyRelations.GetDiplomaticRelation(i);
                        GetHistory((sbyte)i).Add(diplomaticRelation, day: 0);
                    }

                    Player.LegacyRelations.Clear();
                }
            }
        }
    }

    // Player.PAS:100
    public DiplomaticRelation GetDiplomaticRelation(sbyte player) => player < _relations.Count ? (DiplomaticRelation)_relations.List[player] : DiplomaticRelation.Unknown;

    // Player.PAS:107
    public void SetDiplomaticRelation(sbyte player, DiplomaticRelation value)
    {
        _relations[player] = (byte)value;
        if (!StrategicMap.Current.Status.Contains(HexSpriteMapStatuses.Editting) && StrategicMap.Current.Day > 0)
        {
            GetHistory(player).Add(value, StrategicMap.Current.Day);
        }
    }

    // Player.PAS:114
    public DiplomaticRelationHistoryList GetHistory(sbyte player) => _history.GetChild<DiplomaticRelationHistoryList>(player - 1);

    // Player.PAS:119
    public void ValidateHistory()
    {
        while (_history.Count + 1 < StrategicMap.Current.Players.Count)
        {
            new DiplomaticRelationHistoryList(null) { Holder = CreationHolder() }.Owner = _history;
        }

        while (_history.Count + 1 > StrategicMap.Current.Players.Count)
        {
            int count = _history.Count;
            _history.Children[count - 1]?.Dispose();
        }
    }
}
