// Translated from NetworkE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Net;

// the node that owns the players' settings
public partial class PlayerCommunicationSettingsList : ENode
{
    public PlayerCommunicationSettingsList(ECustomNode owner) : base(owner)
    { }

    // NetworkE.pas:461
    public PlayerCommunicationSettings GetPlayers(int index) => (PlayerCommunicationSettings)this._children.List[index];

    public sealed class PlayersAccessor(PlayerCommunicationSettingsList target)
    {
        public PlayerCommunicationSettings this[int index] { get => target.GetPlayers(index); }
    }

    public PlayersAccessor Players => new PlayersAccessor(this);

}
