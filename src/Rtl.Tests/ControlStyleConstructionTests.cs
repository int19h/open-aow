using System.Reflection;
using Aow.Units.AoWE;
using Aow.Units.Engine;
using Aow.Units.HSEngine;
using Aow.Units.ImageLib;
using Game = Aow.Units.AoWE.AoWE;
using EngineUnit = Aow.Units.Engine.Engine;

namespace Aow.Rtl.Tests;

public sealed class ControlStyleConstructionTests
{
    [Theory]
    [InlineData(0, false)]
    [InlineData(0, true)]
    [InlineData(1, false)]
    [InlineData(1, true)]
    [InlineData(2, false)]
    [InlineData(2, true)]
    public void PlainCreationRoutesKeepTheExactTypeAndOwner(int kind, bool owned)
    {
        var engine = CreateEngine();
        var owner = new Owner();
        TECustomNode argument = owned ? owner : null!;
        Type type = kind switch { 0 => typeof(TEControlStyleObject), 1 => typeof(TEResourceEditSettings), _ => typeof(TPadControl) };
        var meta = (TEObject.Meta)MetaRegistry.Of(type);
        if (kind == 2)
        {
            // The pad has the inherited ID 81, so isolate its class entry in this test engine.
            engine.UnregisterEClasses([EClass.Of<TEControlStyleObject>()]);
            engine.RegisterEClasses([EClass.Of(type)]);
        }
        var direct = kind switch
        {
            0 => new TEControlStyleObject(argument),
            1 => new TEResourceEditSettings(argument),
            _ => (TEControlStyleObject)new TPadControl(argument)
        };
        TEObject[] objects = [direct, meta.Create(argument), engine.CreateEObject(meta.ClassID(), argument)];
        TEObject? copy = null;
        try
        {
            Assert.Equal(kind == 1 ? 1026 : 81, meta.ClassID());
            Assert.Equal(kind == 0 ? 12 : kind == 1 ? 16 : 20, meta.InstanceSize);
            Assert.Same(meta, MetaRegistry.Of(type));
            foreach (var value in objects)
            {
                Assert.IsType(type, value);
                Assert.Same(argument, value.Owner);
                Assert.Same(meta, value.ClassType);
            }
            if (direct is TEResourceEditSettings settings)
                settings.Editors.AddObject("Editor", Rtl.IntToObject(17));
            direct.SetControlStyle(TSet<TEControlStyleElement>.Of(EngineUnit.csStored, EngineUnit.csShared));
            copy = direct.Copy(engine);
            Assert.IsType(type, copy);
            Assert.Null(copy.Owner);
            Assert.Equal(direct.GetControlStyle(), copy.GetControlStyle());
            if (copy is TEResourceEditSettings copied)
            {
                Assert.NotSame(((TEResourceEditSettings)direct).Editors, copied.Editors);
                Assert.Equal((AnsiString)"Editor", copied.Editors[0]);
                Assert.Equal(17, Rtl.ObjectToInt(copied.Editors.Objects[0]));
            }
            if (direct is TPadControl pad)
            {
                Assert.NotNull(pad.FTerrainTypes);
                Assert.NotNull(pad.FPadResources);
            }
            Assert.Equal(owned ? 3 : 0, owner.Members.Count);
            Assert.Throws<InvalidOperationException>(() => meta.NewInstance());
        }
        finally
        {
            copy?.Destroy();
            foreach (var value in objects) value.Destroy();
            engine.Destroy();
        }
        Assert.Empty(owner.Members);
    }

    private sealed class Converted : TEControlStyleObject
    {
        internal int Calls;
        internal int Marker;
        internal int ObservedMarker;
        internal TSet<TEControlStyleElement> ObservedStyle;
        internal Converted(TECustomNode owner) : base(owner) { Marker = 77; }
        public override void SetOwner(TECustomNode owner)
        {
            Calls++;
            ObservedMarker = Marker;
            ObservedStyle = GetControlStyle();
            base.SetOwner(owner);
        }
    }

    private sealed class Legacy : TEObject
    {
        internal int Calls;
        public override void SetOwner(TECustomNode owner) { Calls++; base.SetOwner(owner); }
    }

    [Fact]
    public void ConvertedAndLegacyChainsRunTheOwnerBodyOnce()
    {
        EClass.Register(typeof(Converted), TEControlStyleObject.ClassID, static owner => new Converted(owner), 12);
        MetaRegistry.Register(typeof(Converted), new TEControlStyleObject.Meta(), 12);
        EClass.Register(typeof(Legacy), TEObject.ClassID, static owner => EClass.Initialize(new Legacy(), owner), 8);
        MetaRegistry.Register(typeof(Legacy), new TEObject.Meta(), static () => new Legacy(), 8);
        Assert.Equal(0, new Legacy().Calls);
        foreach (bool owned in new[] { false, true })
        {
            var owner = new Owner();
            TECustomNode argument = owned ? owner : null!;
            var converted = (Converted)TEObject.Meta.Of<Converted>().Create(argument);
            var legacy = (Legacy)TEObject.Meta.Of<Legacy>().Create(argument);
            Assert.Equal(1, converted.Calls);
            Assert.Equal(1, legacy.Calls);
            Assert.Same(argument, converted.Owner);
            Assert.Same(argument, legacy.Owner);
            Assert.Equal(0, converted.ObservedMarker);
            Assert.Equal(default, converted.ObservedStyle);
            Assert.Equal(77, converted.Marker);
            Assert.Equal(TSet<TEControlStyleElement>.Of(EngineUnit.csStored), converted.GetControlStyle());
            converted.Destroy();
            legacy.Destroy();
            Assert.Empty(owner.Members);
        }
    }

    [Fact]
    public void NullFlagOwnerFailsBeforeAnUnacquiredLinkIsReleased()
    {
        using var fixture = new FlagFixture(loaded: true);
        Assert.Throws<NullReferenceException>(() => TFlagControl.Create(null!));
        Assert.Throws<NullReferenceException>(() => TEObject.Meta.Of<TFlagControl>().Create(null!));
        var raw = new TFlagControl();
        Assert.Null(raw.Owner);
        Assert.Null(raw.FFlags);
        Assert.Equal(default, raw.GetControlStyle());
        Assert.Equal(0, fixture.Entry.RefCount);
        Assert.Empty(fixture.Owner.Members);
    }

    private static FieldInfo Field(Type type, string name) =>
        type.GetField(name, BindingFlags.Instance | BindingFlags.NonPublic)!;

    private sealed class FlagOwner : TAoWHSSet
    {
        internal readonly List<TEObject> Members = [];
        public override int AddChild(TEObject child) { Members.Add(child); return Members.Count - 1; }
        public override int RemoveChild(TEObject child) { int index = Members.IndexOf(child); Members.Remove(child); return index; }
    }

    private sealed class FlagFixture : IDisposable
    {
        internal readonly FlagOwner Owner = new();
        internal readonly TImageLibraryManager Manager = TImageLibraryManager.Create(null!);
        internal readonly TILManagerObject Entry;
        private readonly TAoWHSSet previous = Game.AoWHSSet;

        internal FlagFixture(bool loaded)
        {
            Field(typeof(THSSet), "FImageLibraryManager").SetValue(Owner, Manager);
            Field(typeof(TImageLibraryManager), "FSearchPath").SetValue(Manager, (AnsiString)"9.3b-2-missing/");
            var disk = (TILManagerList)Field(typeof(TImageLibraryManager), "FDiskList").GetValue(Manager)!;
            Entry = disk.AddILFile("Flags.ILB");
            if (loaded)
                Field(typeof(TILManagerObject), "FImageLibrary").SetValue(Entry, TImageLibrary.Create(null!));
            Game.AoWHSSet = Owner;
        }

        public void Dispose()
        {
            try { foreach (var child in Owner.Members.ToArray()) child.Destroy(); }
            finally { Game.AoWHSSet = previous; Manager.Destroy(); }
        }
    }

    private static TFlagControl CreateFlag(TECustomNode owner, bool throughMeta) => throughMeta
        ? (TFlagControl)TEObject.Meta.Of<TFlagControl>().Create(owner)
        : TFlagControl.Create(owner);

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void FlagLinkCountAndOwnerMatchTheLegacyFactory(bool throughMeta)
    {
        using var fixture = new FlagFixture(loaded: true);
        Assert.Equal(0, fixture.Entry.RefCount);
        var control = CreateFlag(fixture.Owner, throughMeta);
        Assert.IsType<TFlagControl>(control);
        Assert.Same(fixture.Owner, control.Owner);
        Assert.Same(fixture.Entry.ImageLibrary, control.FFlags);
        Assert.Equal(1, fixture.Entry.RefCount);
        Assert.Same(control, Assert.Single(fixture.Owner.Members));
        control.Destroy();
        Assert.Equal(0, fixture.Entry.RefCount);
        Assert.Null(control.Owner);
        Assert.Empty(fixture.Owner.Members);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void FlagLoadFailureRestoresTheLinkCountAndOwner(bool throughMeta)
    {
        using var fixture = new FlagFixture(loaded: false);
        Assert.Equal(0, fixture.Entry.RefCount);
        var error = Assert.Throws<Aow.Rtl.Exception>(() => CreateFlag(fixture.Owner, throughMeta));
        Assert.Equal((AnsiString)"Error loading: 9.3b-2-missing/FLAGS.ILB", error.Message);
        Assert.Equal(0, fixture.Entry.RefCount);
        Assert.Empty(fixture.Owner.Members);
        Assert.NotNull(fixture.Entry.ImageLibrary);
    }

    private sealed class Owner : TECustomNode
    {
        internal readonly List<TEObject> Members = [];
        internal int Removals;
        public override int AddChild(TEObject child) { Members.Add(child); return Members.Count - 1; }
        public override int RemoveChild(TEObject child) { Removals++; int index = Members.IndexOf(child); Members.Remove(child); return index; }
    }

    private sealed class ReadEngine : TEngine
    {
        internal TEObject? Created;
        public override TEObject CreateEObject(int classId, TECustomNode owner)
        {
            Created = base.CreateEObject(classId, owner);
            return Created;
        }
    }

    // An exact-read stream makes a short binary read fail at the truncated payload.
    private sealed class TruncatedStream : TMemoryStream
    {
        public override int Read(Span<byte> buffer)
        {
            int count = base.Read(buffer);
            if (count != buffer.Length) throw new EReadError("Stream read error");
            return count;
        }
    }

    private static ReadEngine CreateEngine()
    {
        if (EngineUnit.Engines is null) EngineUnit.Initialize();
        var engine = new ReadEngine();
        engine.Create__init(null!);
        return engine;
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void TruncatedBinaryReadDestroysOnlyTheNewObject(bool propagate)
    {
        var engine = CreateEngine();
        var owner = new Owner();
        var existing = TEObject.Create(owner);
        TEObject source = TEObject.Meta.Of<TEControlStyleObject>().Create(null!);
        using var stream = new TruncatedStream();
        try
        {
            var writer = new TEWriteStorageStream(stream, engine, false);
            writer.rwCreateEObject(ref source, null!, 7);
            writer.Destroy();
            stream.Size--;
            stream.Position = 0;
            var reader = new TEReadStorageStream(stream, engine, false);
            AnsiString message = default;
            reader.OnError = (TEStorageStream _, AnsiString text, ref bool proceed) =>
            {
                message = text;
                proceed = !propagate;
            };
            TEObject result = null!;
            try
            {
                if (propagate)
                    Assert.Equal((AnsiString)"Stream read error", Assert.Throws<EReadError>(() => reader.rwCreateEObject(ref result, owner, 7)).Message);
                else
                    reader.rwCreateEObject(ref result, owner, 7);
                Assert.Equal((AnsiString)"Stream read error", message);
                Assert.Null(result);
                Assert.IsType<TEControlStyleObject>(engine.Created);
                Assert.Null(engine.Created!.Owner);
                Assert.Same(existing, Assert.Single(owner.Members));
                Assert.Equal(1, owner.Removals);
            }
            finally { reader.Destroy(); }
        }
        finally { source.Destroy(); existing.Destroy(); engine.Destroy(); }
    }
}
