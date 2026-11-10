// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Rtl;

namespace Aow.Game.Layout;

public class LayoutGDPlay : GDPlay
{
    public LayoutGDPlay(Component owner) : base(owner)
    {
    }

    public int FilePacketSize { get; set; }
    public bool GauranteeMsgs { get; set; }
    public int MaxPlayers { get; set; }
    public GDChatMessageEvent OnChatMessage { get; set; }
    public GDDestroyPlayerEvent OnDestroyPlayer { get; set; }
    public NotifyEvent<LayoutGDPlay> OnEjected { get; set; }
    public GDFileDoneMessageEvent OnFileDoneMessage { get; set; }
    public GDFileProgressMessageEvent OnFileProgressMessage { get; set; }
    public GDFileStartMessageEvent OnFileStartMessage { get; set; }
    public GDMessageEvent OnMessage { get; set; }
    public GDMessageErrorEvent OnMessageError { get; set; }
    public GDPingMessageEvent OnPingMessage { get; set; }
    public GDSessionLostEvent OnSessionLost { get; set; }
    public bool UseCodes { get; set; }
    public bool UseSetupDialogs { get; set; }

    public delegate void GDChatMessageEvent(ref string ChatMsg, ref int ToID, ref int FromID);
    public delegate void GDDestroyPlayerEvent(int ID, int NumPlayers);
    public delegate void GDFileDoneMessageEvent(int ID, GDFile gdFile);
    public delegate void GDFileProgressMessageEvent(int ID, int Position, int Size);
    public delegate void GDFileStartMessageEvent(int ID, ref string Name, int Size, ref byte Response);
    public delegate void GDMessageErrorEvent(int ID, int Result);
    public delegate void GDMessageEvent(ref object Data, ref int Size, ref int ToID, ref int FromID);
    public delegate void GDPingMessageEvent(int PingMS, int PingPlayer);
    public delegate void GDSessionLostEvent();
}
