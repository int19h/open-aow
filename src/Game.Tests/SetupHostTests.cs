using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Game.Net;
using Aow.Game.Session;
using Aow.Game.UI.Dialogs;

namespace Aow.Game.Tests;

// Issue 349 part C. Each setup request reaches the host or the transport once with the expected
// arguments and in the old order, the unattended host and the unported transport throw, and the
// reply methods do what the MapLoaded overloads did. The fixture engine carries the recording
// input.
[Collection("Engine")]
[EngineState]
public sealed class SetupHostTests
{
    private sealed class RecordingHost(List<string> log) : ISetupHost
    {
        public void LoadMap(string fileName) => log.Add($"LoadMap:{fileName}");

        public void CustomizeLeader(CustomizeLeaderControl control) => log.Add($"CustomizeLeader:{PlayerIdExtensions.ToOriginal(control.PlayerIndex)}");

        public void CloseIncompatibleSession(int majorVersion, int minorVersion) => log.Add($"CloseIncompatibleSession:{majorVersion}:{minorVersion}");

        public void StartGame() => log.Add("StartGame");

        public void Synchronize() => log.Add("Synchronize");

        public void EjectPlayer(int remoteId) => log.Add($"EjectPlayer:{remoteId}");
    }

    private sealed class RecordingTransport(List<string> log) : ISetupTransport
    {
        public void SendMap(ReadOnlyMemory<byte> image, PlayerId recipient) => log.Add($"SendMap:{recipient}:{image.Length}");

        public bool SendData(ReadOnlyMemory<byte> data, PlayerId recipient)
        {
            log.Add($"SendData:{recipient}:{data.Length}");
            return true;
        }
    }

    private static SetupControl HostControl(List<string> log)
    {
        SetupEvents events = new(new UIContext(EngineFixture.Engine));
        SetupControl control = new(events, host: new RecordingHost(log), transport: new RecordingTransport(log));
        _ = control.BeginHosting(name: "Gorbag", mode: 0, remoteId: 0);
        return control;
    }

    private static SetupControl HostRemoteControl(List<string> log)
    {
        SetupEvents events = new(new UIContext(EngineFixture.Engine));
        SetupControl control = new(events, host: new RecordingHost(log), transport: new RecordingTransport(log));
        _ = control.BeginHosting(name: "Gorbag", mode: 0, remoteId: 7);
        return control;
    }

    private static SetupControl ClientControl(List<string> log)
    {
        SetupEvents events = new(new UIContext(EngineFixture.Engine));
        SetupControl control = new(events, host: new RecordingHost(log), transport: new RecordingTransport(log))
        {
            Status = SetupStatuses.Remote | SetupStatuses.Busy,
        };
        control.Settings.HostRemoteId = 1;
        return control;
    }

    private static void Receive(SetupControl control, int messageId, EngineObject? data = null, int param2 = 0, int param1 = 0)
    {
        using SetupPackage package = new(null) { Holder = EngineFixture.Engine, MessageId = messageId, Param2 = param2, Param1 = param1 };
        if (data is not null)
        {
            package.Data = data;
        }

        control.PackageReceived(package, senderId: 1);
    }

    [Fact]
    public void UnattendedThrows()
    {
        _ = Assert.Throws<InvalidOperationException>(() => SetupHost.Unattended.LoadMap("islands.hsm"));
        _ = Assert.Throws<InvalidOperationException>(() => SetupHost.Unattended.CustomizeLeader(null!));
        _ = Assert.Throws<InvalidOperationException>(() => SetupHost.Unattended.CloseIncompatibleSession(1, minorVersion: 36));
        _ = Assert.Throws<InvalidOperationException>(SetupHost.Unattended.StartGame);
        _ = Assert.Throws<InvalidOperationException>(SetupHost.Unattended.Synchronize);
        _ = Assert.Throws<InvalidOperationException>(() => SetupHost.Unattended.EjectPlayer(5));
        _ = Assert.Throws<NotImplementedException>(() => SetupTransport.Unported.SendMap(ReadOnlyMemory<byte>.Empty, recipient: PlayerId.Player5));
        _ = Assert.Throws<NotImplementedException>(() => SetupTransport.Unported.SendData(ReadOnlyMemory<byte>.Empty, recipient: PlayerId.Independent));
    }

    // An owner-only control carries the throwing defaults, so a host request through it pins
    // the throw instead of doing nothing.
    [Fact]
    public void OwnerOnlyControlThrowsOnHostRequest()
    {
        SetupEvents events = new(new UIContext(EngineFixture.Engine));
        using SetupControl control = new(events);

        _ = Assert.Throws<InvalidOperationException>(control.LoadMap);
    }

    // Package 9 names the versions, as the IncompatibleVersion raise passed them.
    [Fact]
    public void Package9ClosesTheIncompatibleSession()
    {
        List<string> log = [];
        using SetupControl control = ClientControl(log);

        Receive(control, messageId: 9, param2: 1, param1: 36);

        Assert.Equal(["CloseIncompatibleSession:1:36"], log);
    }

    // Package 10 only shows the transfer screen, as the SendingMaps raise did on its own.
    [Fact]
    public void Package10RaisesSendingMaps()
    {
        List<string> log = [];
        using SetupControl control = ClientControl(log);
        bool shown = false;
        control.SendingMaps += _ => shown = true;

        Receive(control, messageId: 10);

        Assert.True(shown);
        Assert.Empty(log);
    }

    // Package 11 with no maps to send loads the map, with the host asked before anything sends.
    [Fact]
    public void Package11HostLoadsTheMap()
    {
        List<string> log = [];
        using SetupControl control = HostControl(log);

        Receive(control, messageId: 11);

        Assert.Equal([$"LoadMap:{EngineFixture.Engine.StartupDirectory}"], log);
    }

    // A version the host does not share ejects the player, with no package answered.
    [Fact]
    public void Package1WithAForeignVersionEjectsThePlayer()
    {
        List<string> log = [];
        using SetupControl control = HostControl(log);
        using RemotePlayer player = new(null!) { Holder = EngineFixture.Engine, RemoteId = 5, Version = 0x0100_0000 };

        Receive(control, messageId: 1, data: player);

        Assert.Equal(["EjectPlayer:5"], log);
    }

    // The client reports its load by synchronizing first and then sending package 7, as the
    // parameterless MapLoaded ordered the synchronize raise before the send.
    [Fact]
    public void ReportMapLoadedClientSynchronizesThenSends()
    {
        EngineClass[] enlisted = EnlistSessionClasses();
        try
        {
            List<string> log = [];
            using SetupControl control = ClientControl(log);
            control.Settings.MapDay = 1;

            control.ReportMapLoaded();

            Assert.Equal(2, log.Count);
            Assert.Equal("Synchronize", log[0]);
            Assert.StartsWith("SendData:0:", log[1]);
        }
        finally
        {
            EngineFixture.Engine.UnregisterEngineClasses(enlisted);
        }
    }

    // The host marks its own player loaded, as the indexed MapLoaded did: the flag lands and
    // the settings update runs without a synchronize or a customize call.
    [Fact]
    public void SetPlayerMapLoadedMarksLoaded()
    {
        List<string> log = [];
        using SetupControl control = HostControl(log);
        control.Settings.CustomizeLeaders = true;
        bool updated = false;
        control.SettingChanged += _ => updated = true;
        PlayerSetupSettings setting = new(null) { Holder = EngineFixture.Engine };
        setting.SetOwner(control.Settings);

        control.SetPlayerMapLoaded(PlayerId.Independent);

        Assert.True(setting.Status.Contains(PlayerSetupStatuses.Loaded));
        Assert.True(updated);
        Assert.Empty(log);
    }

    // The host without a loaded flag on every player does nothing, as the indexed MapLoaded
    // skipped a non-host.
    [Fact]
    public void SetPlayerMapLoadedSkipsAClient()
    {
        List<string> log = [];
        using SetupControl control = ClientControl(log);

        control.SetPlayerMapLoaded(PlayerId.Independent);

        Assert.Empty(log);
    }

    // With leaders to customize the report asks the host once, as the parameterless MapLoaded
    // raised CustomizeLeader for the first uncustomized human player.
    [Fact]
    public void ReportMapLoadedHostCustomizesTheLeader()
    {
        List<string> log = [];
        using SetupControl control = HostControl(log);
        control.Settings.CustomizeLeaders = true;
        PlayerSetupSettings setting = new(null) { Holder = EngineFixture.Engine };
        setting.SetOwner(control.Settings);

        control.ReportMapLoaded();

        Assert.Equal(["CustomizeLeader:0"], log);
        Assert.True(setting.Status.Contains(PlayerSetupStatuses.Loaded));
    }

    // PlayerSetupDone ejects the remote players with no settings row before it sends the
    // settings and loads the map, and the completion event leads the host calls.
    [Fact]
    public void PlayerSetupDoneEjectsBeforeSending()
    {
        EngineClass[] enlisted = EnlistSessionClasses();
        try
        {
            List<string> log = [];
            using SetupControl control = HostRemoteControl(log);
            PlayerSetupSettings human = new(null) { Holder = EngineFixture.Engine };
            human.Behavior = human.Behavior with { Flags = 7 };
            human.SetOwner(control.Settings);
            PlayerSetupSettings cpu = new(null) { Holder = EngineFixture.Engine };
            cpu.Behavior = cpu.Behavior with { PlayerType = 1 };
            cpu.SetOwner(control.Settings);
            control.Settings.RemotePlayers.GetPlayers(control.Settings.RemotePlayers.IndexOfRemoteId(7)).MapValidation = MapValidationInfo.Valid;
            RemotePlayer gone = new(null!) { Holder = EngineFixture.Engine, Name = "Gone", RemoteId = 5, MapValidation = MapValidationInfo.Valid };
            gone.SetOwner(control.Settings.RemotePlayers);
            control.PlayerSetupCompleted += _ => log.Add("PlayerSetupCompleted");

            Assert.True(control.PlayerSetupDone());

            Assert.Equal(4, log.Count);
            Assert.Equal("PlayerSetupCompleted", log[0]);
            Assert.Equal("EjectPlayer:5", log[1]);
            Assert.StartsWith("SendData:0:", log[2]);
            Assert.Equal($"LoadMap:{EngineFixture.Engine.StartupDirectory}", log[3]);
        }
        finally
        {
            EngineFixture.Engine.UnregisterEngineClasses(enlisted);
        }
    }

    // ValidateSendMap sends the map image to the remote player whose copy is invalid, and the
    // stream dies on the send. The map file is the installed save staged in the fixture root.
    [InstalledFact]
    public void ValidateSendMapSendsTheMapImage()
    {
        string mapName = "islands.asg";
        string staged = Path.Combine(EngineFixture.Root, mapName);
        File.WriteAllBytes(staged, bytes: File.ReadAllBytes(InstalledSave.Save));
        try
        {
            List<string> log = [];
            using SetupControl control = HostRemoteControl(log);
            control.Settings.MapName = mapName;
            EngineFixture.Engine.StartupDirectory = EngineFixture.Root + Path.DirectorySeparatorChar;
            PlayerSetupSettings setting = new(null) { Holder = EngineFixture.Engine };
            setting.Behavior = setting.Behavior with { Flags = 5 };
            setting.SetOwner(control.Settings);
            RemotePlayer gone = new(null!) { Holder = EngineFixture.Engine, Name = "Gone", RemoteId = 5, MapValidation = MapValidationInfo.InvalidMap };
            gone.SetOwner(control.Settings.RemotePlayers);

            Assert.False(control.ValidateSendMap());

            _ = Assert.Single(log);
            Assert.StartsWith("SendMap:5:", log[0]);
        }
        finally
        {
            File.Delete(staged);
        }
    }

    // The fixture engine holds only the classes of the engine's start, so a test that stores a
    // package enlists the session classes and takes them back off, as the persistence tests do.
    private static EngineClass[] EnlistSessionClasses()
    {
        Dictionary<int, EngineClass?> table = EngineFixture.Engine.ClassTable;
        Dictionary<int, EngineClass> added = [];
        foreach (EngineClass engineClass in EngineClass.All)
        {
            if (engineClass.Type.Assembly != EngineFixture.Engine.GetType().Assembly || engineClass.Type.Namespace is not { } space)
            {
                continue;
            }

            if (!s_sessionNamespaces.Any(prefix => space == prefix || space.StartsWith($"{prefix}.", comparisonType: StringComparison.Ordinal)))
            {
                continue;
            }

            if (table.ContainsKey(engineClass.ClassId) || added.ContainsKey(engineClass.ClassId))
            {
                continue;
            }

            added.Add(engineClass.ClassId, engineClass);
        }
        EngineClass[] classes = [.. added.Values];
        EngineFixture.Engine.RegisterEngineClasses(classes);
        return classes;
    }

    private static readonly string[] s_sessionNamespaces =
        ["Aow.Game.Session", "Aow.Game.Events", "Aow.Game.AI", "Aow.Game.Magic", "Aow.Game.UI", "Aow.Game.Audio"];
}
