// Translated from NetworkE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Net;

// the node that owns the players' settings
public partial class PlayerCommunicationSettingsList : OwningNode, IChildList<PlayerCommunicationSettings>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only a PlayerCommunicationSettings, so the cast never fails.
    public PlayerCommunicationSettings this[int index] => (PlayerCommunicationSettings)GetChild(index);

    protected sealed override bool AcceptsChild(EngineObject child) => child is PlayerCommunicationSettings;

    int IChildSink<PlayerCommunicationSettings>.AddChild(PlayerCommunicationSettings child) => AddChild(child);

    public PlayerCommunicationSettingsList(CustomNode owner) : base(owner)
    { }

    // NetworkE.pas:461
    public PlayerCommunicationSettings GetPlayers(int index) => this[index];

    public sealed class PlayersAccessor(PlayerCommunicationSettingsList target)
    {
        public PlayerCommunicationSettings this[int index] => target.GetPlayers(index);
    }

    public PlayersAccessor Players => new PlayersAccessor(this);

}
