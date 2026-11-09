namespace Aow.Rtl;

// The offline DirectPlay component (aowDPlayComp.pas). It lists the TCP/IP provider that
// DirectPlay lists on any current Windows, but it never connects and never joins a session, so
// the original's guards on Connected and InSession take their offline branch.
public class TGDPlay : TComponent
{
    public TGDPlay(TComponent? owner = null) : base(owner) { }

    // DPlay.pas: the TCP/IP service provider.
    public static readonly TGUID DPSPGUID_TCPIP = "36E95EE0-8577-11CF-960C-0080C7534E82";
    public const int E_FAIL = unchecked((int)0x80004005);

    public TGUID AppGUID { get; set; }
    public bool Connected => false;
    public bool InSession => false;
    public TConnectionType ConnectionType => TConnectionType.conNone;
    public object? DPlay => null;
    public int PlayerID => 0;
    public string SessionName { get; set; } = "";
    // SetPlayerName renames the session player only when connected and in a session.
    public string PlayerName { get; set; } = "";
    public string TCPPort { get; set; } = "";
    // The service providers found by GetConnections: their names and their TGDConnection items.
    public TStringList Connections { get; } = new();
    public TList ConnectionItems { get; } = new();

    public void CheckMessages() { }

    // The original's EnumConnections callback adds the name and the item of each provider.
    public int GetConnections()
    {
        Connections.Clear();
        ConnectionItems.Clear();
        Connections.Add("Internet TCP/IP Connection For DirectPlay");
        ConnectionItems.Add(new TGDConnection { FGuid = DPSPGUID_TCPIP, FType = TConnectionType.conTCP });
        return 0;
    }

    // The offline component has no DirectPlay object to initialize the provider with.
    public int Connect(int index) => E_FAIL;
    // The original fails without a connection.
    public int CreateSession() => E_FAIL;

    // The original sets its close flag only in a session.
    public void CloseSession() { }

    // The original sets its disconnect flag only when connected.
    public void DisConnect() { }
}

[LegacyObject]
// aowDPlayComp.pas: a service provider found by EnumConnections.
public class TGDConnection
{
    public TGUID FGuid;
    public TConnectionType FType;

    public TGDConnection()
    {
        CaptureAddress.Assign(this);
    }
}

[LegacyObject]
public class TGDFile
{
    public TGDFile()
    {
        CaptureAddress.Assign(this);
    }
}

[LegacyObject]
public class TGDMessage
{
    public TGDMessage()
    {
        CaptureAddress.Assign(this);
    }
}
