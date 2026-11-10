// Translated from NetworkE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Net;

// the node that owns the players' settings
public partial class PlayerCommunicationSettingsList : ENode
{
    public PlayerCommunicationSettingsList(ECustomNode owner) : base(owner)
    { }

    // NetworkE.pas:461
    public PlayerCommunicationSettings GetPlayers(int Index) => (PlayerCommunicationSettings)this.FChildren.List[Index];

    public sealed class PlayersAccessor(PlayerCommunicationSettingsList target)
    {
        public PlayerCommunicationSettings this[int Index] { get => target.GetPlayers(Index); }
    }

    public PlayersAccessor Players => new PlayersAccessor(this);

}
