// Layout translated from recovered Delphi forms.
#nullable disable warnings
using System.Diagnostics.CodeAnalysis;
using Aow.Rtl;

namespace Aow.Game.Layout;

public sealed class LayoutDirectPlay(Component owner) : GDPlay(owner)
{
    [SuppressMessage("Style", "IDE0052:Remove unread private members", Justification = "Public event ChatMessageReceived keeps this backing field until the port raises it.")]
    private GDChatMessageEvent _chatMessageReceived;

    public event GDChatMessageEvent ChatMessageReceived { add => _chatMessageReceived += value; remove => _chatMessageReceived -= value; }

    [SuppressMessage("Style", "IDE0052:Remove unread private members", Justification = "Public event Ejected keeps this backing field until the port raises it.")]
    private NotifyEvent<LayoutDirectPlay> _ejected;

    public event NotifyEvent<LayoutDirectPlay> Ejected { add => _ejected += value; remove => _ejected -= value; }

    public int FilePacketSize { get; set; }

    [SuppressMessage("Style", "IDE0052:Remove unread private members", Justification = "Public event FileTransferCompleted keeps this backing field until the port raises it.")]
    private GDFileDoneMessageEvent _fileTransferCompleted;

    public event GDFileDoneMessageEvent FileTransferCompleted { add => _fileTransferCompleted += value; remove => _fileTransferCompleted -= value; }

    [SuppressMessage("Style", "IDE0052:Remove unread private members", Justification = "Public event FileTransferProgressed keeps this backing field until the port raises it.")]
    private GDFileProgressMessageEvent _fileTransferProgressed;

    public event GDFileProgressMessageEvent FileTransferProgressed { add => _fileTransferProgressed += value; remove => _fileTransferProgressed -= value; }

    [SuppressMessage("Style", "IDE0052:Remove unread private members", Justification = "Public event FileTransferStarting keeps this backing field until the port raises it.")]
    private GDFileStartMessageEvent _fileTransferStarting;

    public event GDFileStartMessageEvent FileTransferStarting { add => _fileTransferStarting += value; remove => _fileTransferStarting -= value; }

    public bool GuaranteeMessages { get; set; }
    public int MaxPlayers { get; set; }

    [SuppressMessage("Style", "IDE0052:Remove unread private members", Justification = "Public event MessageFailed keeps this backing field until the port raises it.")]
    private GDMessageErrorEvent _messageFailed;

    public event GDMessageErrorEvent MessageFailed { add => _messageFailed += value; remove => _messageFailed -= value; }

    [SuppressMessage("Style", "IDE0052:Remove unread private members", Justification = "Public event MessageReceived keeps this backing field until the port raises it.")]
    private GDMessageEvent _messageReceived;

    public event GDMessageEvent MessageReceived { add => _messageReceived += value; remove => _messageReceived -= value; }

    [SuppressMessage("Style", "IDE0052:Remove unread private members", Justification = "Public event PingMessage keeps this backing field until the port raises it.")]
    private GDPingMessageEvent _pingMessage;

    public event GDPingMessageEvent PingMessage { add => _pingMessage += value; remove => _pingMessage -= value; }

    [SuppressMessage("Style", "IDE0052:Remove unread private members", Justification = "Public event PlayerDestroyed keeps this backing field until the port raises it.")]
    private GDDestroyPlayerEvent _playerDestroyed;

    public event GDDestroyPlayerEvent PlayerDestroyed { add => _playerDestroyed += value; remove => _playerDestroyed -= value; }

    [SuppressMessage("Style", "IDE0052:Remove unread private members", Justification = "Public event SessionLost keeps this backing field until the port raises it.")]
    private GDSessionLostEvent _sessionLost;

    public event GDSessionLostEvent SessionLost { add => _sessionLost += value; remove => _sessionLost -= value; }

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
