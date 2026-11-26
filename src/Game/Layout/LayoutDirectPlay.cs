// Layout translated from recovered Delphi forms.
#nullable disable warnings
using System.Diagnostics.CodeAnalysis;
using Aow.Game.Net;
using Aow.Rtl;

namespace Aow.Game.Layout;

public sealed class LayoutDirectPlay(Component owner) : GDPlay(owner)
{
    [SuppressMessage("Style", "IDE0052:Remove unread private members", Justification = "Public event ChatMessageReceived keeps this backing field until the port raises it.")]
    private EventHandler<ChatMessageEventArgs> _chatMessageReceived;

    public event EventHandler<ChatMessageEventArgs> ChatMessageReceived { add => _chatMessageReceived += value; remove => _chatMessageReceived -= value; }

    [SuppressMessage("Style", "IDE0052:Remove unread private members", Justification = "Public event Ejected keeps this backing field until the port raises it.")]
    private EventHandler _ejected;

    public event EventHandler Ejected { add => _ejected += value; remove => _ejected -= value; }

    public int FilePacketSize { get; set; }

    [SuppressMessage("Style", "IDE0052:Remove unread private members", Justification = "Public event FileTransferCompleted keeps this backing field until the port raises it.")]
    private EventHandler<FileTransferCompletedEventArgs> _fileTransferCompleted;

    public event EventHandler<FileTransferCompletedEventArgs> FileTransferCompleted { add => _fileTransferCompleted += value; remove => _fileTransferCompleted -= value; }

    [SuppressMessage("Style", "IDE0052:Remove unread private members", Justification = "Public event FileTransferProgressed keeps this backing field until the port raises it.")]
    private EventHandler<FileTransferProgressEventArgs> _fileTransferProgressed;

    public event EventHandler<FileTransferProgressEventArgs> FileTransferProgressed { add => _fileTransferProgressed += value; remove => _fileTransferProgressed -= value; }

    [SuppressMessage("Style", "IDE0052:Remove unread private members", Justification = "Public event FileTransferStarting keeps this backing field until the port raises it.")]
    private EventHandler<FileTransferStartingEventArgs> _fileTransferStarting;

    public event EventHandler<FileTransferStartingEventArgs> FileTransferStarting { add => _fileTransferStarting += value; remove => _fileTransferStarting -= value; }

    public bool GuaranteeMessages { get; set; }
    public int MaxPlayers { get; set; }

    [SuppressMessage("Style", "IDE0052:Remove unread private members", Justification = "Public event MessageFailed keeps this backing field until the port raises it.")]
    private EventHandler<MessageFailedEventArgs> _messageFailed;

    public event EventHandler<MessageFailedEventArgs> MessageFailed { add => _messageFailed += value; remove => _messageFailed -= value; }

    [SuppressMessage("Style", "IDE0052:Remove unread private members", Justification = "Public event MessageReceived keeps this backing field until the port raises it.")]
    private EventHandler<MessageReceivedEventArgs> _messageReceived;

    public event EventHandler<MessageReceivedEventArgs> MessageReceived { add => _messageReceived += value; remove => _messageReceived -= value; }

    [SuppressMessage("Style", "IDE0052:Remove unread private members", Justification = "Public event PingReceived keeps this backing field until the port raises it.")]
    private EventHandler<PingEventArgs> _pingReceived;

    public event EventHandler<PingEventArgs> PingReceived { add => _pingReceived += value; remove => _pingReceived -= value; }

    [SuppressMessage("Style", "IDE0052:Remove unread private members", Justification = "Public event PlayerDestroyed keeps this backing field until the port raises it.")]
    private EventHandler<PlayerDestroyedEventArgs> _playerDestroyed;

    public event EventHandler<PlayerDestroyedEventArgs> PlayerDestroyed { add => _playerDestroyed += value; remove => _playerDestroyed -= value; }

    [SuppressMessage("Style", "IDE0052:Remove unread private members", Justification = "Public event SessionLost keeps this backing field until the port raises it.")]
    private EventHandler _sessionLost;

    public event EventHandler SessionLost { add => _sessionLost += value; remove => _sessionLost -= value; }

    public bool UseCodes { get; set; }
    public bool UseSetupDialogs { get; set; }
}
