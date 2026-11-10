namespace Aow.Rtl;

// The offline DirectPlay component (aowDPlayComp.pas). It lists the TCP/IP provider that
// DirectPlay lists on any current Windows, but it never connects and never joins a session, so
// the original's guards on Connected and InSession take their offline branch.
public class GDPlay : Component
{
    public GDPlay(Component? owner = null) : base(owner) { }

    // DPlay.pas: the TCP/IP service provider.
    public static readonly GUID DPSPGUID_TCPIP = "36E95EE0-8577-11CF-960C-0080C7534E82";
    public const int E_FAIL = unchecked((int)0x80004005);

    public GUID AppGUID { get; set; }
    public bool Connected => false;
    public bool InSession => false;
    public ConnectionType ConnectionType => Rtl.ConnectionType.None;
    public object? DPlay => null;
    public int PlayerID => 0;
    public string SessionName { get; set; } = "";
    // SetPlayerName renames the session player only when connected and in a session.
    public string PlayerName { get; set; } = "";
    public string TCPPort { get; set; } = "";
    // The service providers found by GetConnections: their names and their TGDConnection items.
    public StringList Connections { get; } = new();
    public DelphiList ConnectionItems { get; } = new();

    public void CheckMessages() { }

    // The original's EnumConnections callback adds the name and the item of each provider.
    public int GetConnections()
    {
        Connections.Clear();
        ConnectionItems.Clear();
        Connections.Add("Internet TCP/IP Connection For DirectPlay");
        ConnectionItems.Add(new GDConnection { FGuid = DPSPGUID_TCPIP, FType = Rtl.ConnectionType.TCP });
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
public class GDConnection
{
    public GUID FGuid;
    public ConnectionType FType;

    public GDConnection()
    {
        CaptureAddress.Assign(this);
    }
}

[LegacyObject]
public class GDFile
{
    public GDFile()
    {
        CaptureAddress.Assign(this);
    }
}

[LegacyObject]
public class GDMessage
{
    public GDMessage()
    {
        CaptureAddress.Assign(this);
    }
}
