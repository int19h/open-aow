// Layout translated from recovered Delphi forms.
#nullable disable
using System.Diagnostics.CodeAnalysis;
using Aow.Rtl;

namespace Aow.Game.Layout;

public class LayoutDirectPlay(Component owner) : GDPlay(owner)
{
    public int FilePacketSize { get; set; }
    public bool GuaranteeMessages { get; set; }
    public int MaxPlayers { get; set; }

    // Nothing in the port raises OnChatMessage. The bound handler is kept.
    [SuppressMessage("Style", "IDE0052:Remove unread private members", Justification = "Public event OnChatMessage keeps this backing field until the port raises it.")]
    private GDChatMessageEvent _onChatMessage;

    public event GDChatMessageEvent OnChatMessage { add => _onChatMessage += value; remove => _onChatMessage -= value; }

    // Nothing in the port raises OnDestroyPlayer. The bound handler is kept.
    [SuppressMessage("Style", "IDE0052:Remove unread private members", Justification = "Public event OnDestroyPlayer keeps this backing field until the port raises it.")]
    private GDDestroyPlayerEvent _onDestroyPlayer;

    public event GDDestroyPlayerEvent OnDestroyPlayer { add => _onDestroyPlayer += value; remove => _onDestroyPlayer -= value; }

    // Nothing in the port raises OnEjected. The bound handler is kept.
    [SuppressMessage("Style", "IDE0052:Remove unread private members", Justification = "Public event OnEjected keeps this backing field until the port raises it.")]
    private NotifyEvent<LayoutDirectPlay> _onEjected;

    public event NotifyEvent<LayoutDirectPlay> OnEjected { add => _onEjected += value; remove => _onEjected -= value; }

    // Nothing in the port raises OnFileDoneMessage. The bound handler is kept.
    [SuppressMessage("Style", "IDE0052:Remove unread private members", Justification = "Public event OnFileDoneMessage keeps this backing field until the port raises it.")]
    private GDFileDoneMessageEvent _onFileDoneMessage;

    public event GDFileDoneMessageEvent OnFileDoneMessage { add => _onFileDoneMessage += value; remove => _onFileDoneMessage -= value; }

    // Nothing in the port raises OnFileProgressMessage. The bound handler is kept.
    [SuppressMessage("Style", "IDE0052:Remove unread private members", Justification = "Public event OnFileProgressMessage keeps this backing field until the port raises it.")]
    private GDFileProgressMessageEvent _onFileProgressMessage;

    public event GDFileProgressMessageEvent OnFileProgressMessage { add => _onFileProgressMessage += value; remove => _onFileProgressMessage -= value; }

    // Nothing in the port raises OnFileStartMessage. The bound handler is kept.
    [SuppressMessage("Style", "IDE0052:Remove unread private members", Justification = "Public event OnFileStartMessage keeps this backing field until the port raises it.")]
    private GDFileStartMessageEvent _onFileStartMessage;

    public event GDFileStartMessageEvent OnFileStartMessage { add => _onFileStartMessage += value; remove => _onFileStartMessage -= value; }

    // Nothing in the port raises OnMessage. The bound handler is kept.
    [SuppressMessage("Style", "IDE0052:Remove unread private members", Justification = "Public event OnMessage keeps this backing field until the port raises it.")]
    private GDMessageEvent _onMessage;

    public event GDMessageEvent OnMessage { add => _onMessage += value; remove => _onMessage -= value; }

    // Nothing in the port raises OnMessageError. The bound handler is kept.
    [SuppressMessage("Style", "IDE0052:Remove unread private members", Justification = "Public event OnMessageError keeps this backing field until the port raises it.")]
    private GDMessageErrorEvent _onMessageError;

    public event GDMessageErrorEvent OnMessageError { add => _onMessageError += value; remove => _onMessageError -= value; }

    // Nothing in the port raises OnPingMessage. The bound handler is kept.
    [SuppressMessage("Style", "IDE0052:Remove unread private members", Justification = "Public event OnPingMessage keeps this backing field until the port raises it.")]
    private GDPingMessageEvent _onPingMessage;

    public event GDPingMessageEvent OnPingMessage { add => _onPingMessage += value; remove => _onPingMessage -= value; }

    // Nothing in the port raises OnSessionLost. The bound handler is kept.
    [SuppressMessage("Style", "IDE0052:Remove unread private members", Justification = "Public event OnSessionLost keeps this backing field until the port raises it.")]
    private GDSessionLostEvent _onSessionLost;

    public event GDSessionLostEvent OnSessionLost { add => _onSessionLost += value; remove => _onSessionLost -= value; }
    public bool UseCodes { get; set; }
    public bool UseSetupDialogs { get; set; }

    public delegate void GDChatMessageEvent(string ChatMsg, in int ToID, in int FromID);
    public delegate void GDDestroyPlayerEvent(int ID, int NumPlayers);
    public delegate void GDFileDoneMessageEvent(int ID, GDFile gdFile);
    public delegate void GDFileProgressMessageEvent(int ID, int Position, int Size);
    public delegate void GDFileStartMessageEvent(int ID, string Name, int Size, in byte Response);
    public delegate void GDMessageErrorEvent(int ID, int Result);
    public delegate void GDMessageEvent(object Data, in int Size, in int ToID, in int FromID);
    public delegate void GDPingMessageEvent(int PingMS, int PingPlayer);
    public delegate void GDSessionLostEvent();
}
