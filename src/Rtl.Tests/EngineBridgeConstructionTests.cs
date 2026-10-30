using System.Reflection;
using Aow.Units.Engine;
using EngineUnit = Aow.Units.Engine.Engine;

namespace Aow.Rtl.Tests;

public sealed class EngineBridgeConstructionTests
{
    private interface IProbe
    {
        int OwnerCalls { get; set; }
        int StyleCalls { get; set; }
        int References { get; set; }
        int Destructions { get; set; }
    }

    private static FieldInfo? Field(object value, string name) => value.GetType()
        .GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

    private static void BeforeOwner(TEObject value)
    {
        ((IProbe)value).OwnerCalls++;
        Field(value, "FControlStyle")?.SetValue(value, TSet<TEControlStyleElement>.Of(EngineUnit.csShared));
        Field(value, "FUpdateCount")?.SetValue(value, 17);
        if (value is TESortNode sort) sort.FItems = new TSortItem[1];
    }

    private static int AddReference(TEObject value)
    {
        // Reference-list insertion clears ownership before the virtual reference call.
        Assert.Null(value.Owner);
        Assert.Null(Field(value, "FChildren")?.GetValue(value));
        return ++((IProbe)value).References;
    }

    private sealed class Owner : TECustomNode
    {
        internal readonly List<TEObject> Members = [];
        internal readonly List<TEObject> Removed = [];
        public override int AddChild(TEObject child)
        {
            Assert.Same(this, child.Owner);
            Assert.Null(Field(child, "FChildren")?.GetValue(child));
            Assert.Null(Field(child, "FEvents")?.GetValue(child));
            Members.Add(child);
            return Members.Count - 1;
        }
        public override int RemoveChild(TEObject child)
        {
            int index = Members.IndexOf(child);
            Members.Remove(child);
            Removed.Add(child);
            return index;
        }
    }

    private sealed class ConvertedTENode : TENode, IProbe
    {
        public int OwnerCalls { get; set; }
        public int StyleCalls { get; set; }
        public int References { get; set; }
        public int Destructions { get; set; }
        internal ConvertedTENode(TECustomNode owner) : base(owner) { }
        internal ConvertedTENode(ConstructionContext? context, TECustomNode owner, bool fail = false)
            : base(context, owner)
        {
            if (fail) throw new InvalidOperationException("bridge failure");
        }
        public override void SetOwner(TECustomNode owner) { BeforeOwner(this); base.SetOwner(owner); }
        public override TSet<TEControlStyleElement> GetControlStyle()
        {
            StyleCalls++;
            return TSet<TEControlStyleElement>.Of(EngineUnit.csShared);
        }
        public override int AddRef() => AddReference(this);
        public override int Release() => --References;
        public override void Destroy() { Destructions++; base.Destroy(); }
    }

    private sealed class LegacyTENode : TENode, IProbe
    {
        public int OwnerCalls { get; set; }
        public int StyleCalls { get; set; }
        public int References { get; set; }
        public int Destructions { get; set; }
        public override void SetOwner(TECustomNode owner) { BeforeOwner(this); base.SetOwner(owner); }
        public override TSet<TEControlStyleElement> GetControlStyle()
        {
            StyleCalls++;
            return TSet<TEControlStyleElement>.Of(EngineUnit.csShared);
        }
        public override int AddRef() => AddReference(this);
        public override int Release() => --References;
        public override void Destroy() { Destructions++; base.Destroy(); }
    }

    private sealed class ConvertedTEResource : TEResource, IProbe
    {
        public int OwnerCalls { get; set; }
        public int StyleCalls { get; set; }
        public int References { get; set; }
        public int Destructions { get; set; }
        internal ConvertedTEResource(TECustomNode owner) : base(owner) { }
        internal ConvertedTEResource(ConstructionContext? context, TECustomNode owner, bool fail = false)
            : base(context, owner)
        {
            if (fail) throw new InvalidOperationException("bridge failure");
        }
        public override void SetOwner(TECustomNode owner) { BeforeOwner(this); base.SetOwner(owner); }
        public override TSet<TEControlStyleElement> GetControlStyle()
        {
            StyleCalls++;
            return TSet<TEControlStyleElement>.Of(EngineUnit.csShared);
        }
        public override int AddRef() => AddReference(this);
        public override int Release() => --References;
        public override void Destroy() { Destructions++; base.Destroy(); }
    }

    private sealed class LegacyTEResource : TEResource, IProbe
    {
        public int OwnerCalls { get; set; }
        public int StyleCalls { get; set; }
        public int References { get; set; }
        public int Destructions { get; set; }
        public override void SetOwner(TECustomNode owner) { BeforeOwner(this); base.SetOwner(owner); }
        public override TSet<TEControlStyleElement> GetControlStyle()
        {
            StyleCalls++;
            return TSet<TEControlStyleElement>.Of(EngineUnit.csShared);
        }
        public override int AddRef() => AddReference(this);
        public override int Release() => --References;
        public override void Destroy() { Destructions++; base.Destroy(); }
    }

    private sealed class ConvertedTEChangeNotifyNode : TEChangeNotifyNode, IProbe
    {
        public int OwnerCalls { get; set; }
        public int StyleCalls { get; set; }
        public int References { get; set; }
        public int Destructions { get; set; }
        internal ConvertedTEChangeNotifyNode(TECustomNode owner) : base(owner) { }
        internal ConvertedTEChangeNotifyNode(ConstructionContext? context, TECustomNode owner, bool fail = false)
            : base(context, owner)
        {
            if (fail) throw new InvalidOperationException("bridge failure");
        }
        public override void SetOwner(TECustomNode owner) { BeforeOwner(this); base.SetOwner(owner); }
        public override TSet<TEControlStyleElement> GetControlStyle()
        {
            StyleCalls++;
            return TSet<TEControlStyleElement>.Of(EngineUnit.csShared);
        }
        public override int AddRef() => AddReference(this);
        public override int Release() => --References;
        public override void Destroy() { Destructions++; base.Destroy(); }
    }

    private sealed class LegacyTEChangeNotifyNode : TEChangeNotifyNode, IProbe
    {
        public int OwnerCalls { get; set; }
        public int StyleCalls { get; set; }
        public int References { get; set; }
        public int Destructions { get; set; }
        public override void SetOwner(TECustomNode owner) { BeforeOwner(this); base.SetOwner(owner); }
        public override TSet<TEControlStyleElement> GetControlStyle()
        {
            StyleCalls++;
            return TSet<TEControlStyleElement>.Of(EngineUnit.csShared);
        }
        public override int AddRef() => AddReference(this);
        public override int Release() => --References;
        public override void Destroy() { Destructions++; base.Destroy(); }
    }

    private sealed class ConvertedTEReferenceNode : TEReferenceNode, IProbe
    {
        public int OwnerCalls { get; set; }
        public int StyleCalls { get; set; }
        public int References { get; set; }
        public int Destructions { get; set; }
        internal ConvertedTEReferenceNode(TECustomNode owner) : base(owner) { }
        internal ConvertedTEReferenceNode(ConstructionContext? context, TECustomNode owner, bool fail = false)
            : base(context, owner)
        {
            if (fail) throw new InvalidOperationException("bridge failure");
        }
        public override void SetOwner(TECustomNode owner) { BeforeOwner(this); base.SetOwner(owner); }
        public override TSet<TEControlStyleElement> GetControlStyle()
        {
            StyleCalls++;
            return TSet<TEControlStyleElement>.Of(EngineUnit.csShared);
        }
        public override int AddRef() => AddReference(this);
        public override int Release() => --References;
        public override void Destroy() { Destructions++; base.Destroy(); }
    }

    private sealed class LegacyTEReferenceNode : TEReferenceNode, IProbe
    {
        public int OwnerCalls { get; set; }
        public int StyleCalls { get; set; }
        public int References { get; set; }
        public int Destructions { get; set; }
        public override void SetOwner(TECustomNode owner) { BeforeOwner(this); base.SetOwner(owner); }
        public override TSet<TEControlStyleElement> GetControlStyle()
        {
            StyleCalls++;
            return TSet<TEControlStyleElement>.Of(EngineUnit.csShared);
        }
        public override int AddRef() => AddReference(this);
        public override int Release() => --References;
        public override void Destroy() { Destructions++; base.Destroy(); }
    }

    private sealed class ConvertedTECustomOwner : TECustomOwner, IProbe
    {
        public int OwnerCalls { get; set; }
        public int StyleCalls { get; set; }
        public int References { get; set; }
        public int Destructions { get; set; }
        internal ConvertedTECustomOwner(TECustomNode owner) : base(owner) { }
        internal ConvertedTECustomOwner(ConstructionContext? context, TECustomNode owner, bool fail = false)
            : base(context, owner)
        {
            if (fail) throw new InvalidOperationException("bridge failure");
        }
        public override void SetOwner(TECustomNode owner) { BeforeOwner(this); base.SetOwner(owner); }
        public override TSet<TEControlStyleElement> GetControlStyle()
        {
            StyleCalls++;
            return TSet<TEControlStyleElement>.Of(EngineUnit.csShared);
        }
        public override int AddRef() => AddReference(this);
        public override int Release() => --References;
        public override void Destroy() { Destructions++; base.Destroy(); }
    }

    private sealed class LegacyTECustomOwner : TECustomOwner, IProbe
    {
        public int OwnerCalls { get; set; }
        public int StyleCalls { get; set; }
        public int References { get; set; }
        public int Destructions { get; set; }
        public override void SetOwner(TECustomNode owner) { BeforeOwner(this); base.SetOwner(owner); }
        public override TSet<TEControlStyleElement> GetControlStyle()
        {
            StyleCalls++;
            return TSet<TEControlStyleElement>.Of(EngineUnit.csShared);
        }
        public override int AddRef() => AddReference(this);
        public override int Release() => --References;
        public override void Destroy() { Destructions++; base.Destroy(); }
    }

    private sealed class ConvertedTEResourceList : TEResourceList, IProbe
    {
        public int OwnerCalls { get; set; }
        public int StyleCalls { get; set; }
        public int References { get; set; }
        public int Destructions { get; set; }
        internal ConvertedTEResourceList(TECustomNode owner) : base(owner) { }
        internal ConvertedTEResourceList(ConstructionContext? context, TECustomNode owner, bool fail = false)
            : base(context, owner)
        {
            if (fail) throw new InvalidOperationException("bridge failure");
        }
        public override void SetOwner(TECustomNode owner) { BeforeOwner(this); base.SetOwner(owner); }
        public override TSet<TEControlStyleElement> GetControlStyle()
        {
            StyleCalls++;
            return TSet<TEControlStyleElement>.Of(EngineUnit.csShared);
        }
        public override int AddRef() => AddReference(this);
        public override int Release() => --References;
        public override void Destroy() { Destructions++; base.Destroy(); }
    }

    private sealed class LegacyTEResourceList : TEResourceList, IProbe
    {
        public int OwnerCalls { get; set; }
        public int StyleCalls { get; set; }
        public int References { get; set; }
        public int Destructions { get; set; }
        public override void SetOwner(TECustomNode owner) { BeforeOwner(this); base.SetOwner(owner); }
        public override TSet<TEControlStyleElement> GetControlStyle()
        {
            StyleCalls++;
            return TSet<TEControlStyleElement>.Of(EngineUnit.csShared);
        }
        public override int AddRef() => AddReference(this);
        public override int Release() => --References;
        public override void Destroy() { Destructions++; base.Destroy(); }
    }

    private sealed class ConvertedTEControlStyleNode : TEControlStyleNode, IProbe
    {
        public int OwnerCalls { get; set; }
        public int StyleCalls { get; set; }
        public int References { get; set; }
        public int Destructions { get; set; }
        internal ConvertedTEControlStyleNode(TECustomNode owner) : base(owner) { }
        internal ConvertedTEControlStyleNode(ConstructionContext? context, TECustomNode owner, bool fail = false)
            : base(context, owner)
        {
            if (fail) throw new InvalidOperationException("bridge failure");
        }
        public override void SetOwner(TECustomNode owner) { BeforeOwner(this); base.SetOwner(owner); }
        public override TSet<TEControlStyleElement> GetControlStyle()
        {
            StyleCalls++;
            return TSet<TEControlStyleElement>.Of(EngineUnit.csShared);
        }
        public override int AddRef() => AddReference(this);
        public override int Release() => --References;
        public override void Destroy() { Destructions++; base.Destroy(); }
    }

    private sealed class LegacyTEControlStyleNode : TEControlStyleNode, IProbe
    {
        public int OwnerCalls { get; set; }
        public int StyleCalls { get; set; }
        public int References { get; set; }
        public int Destructions { get; set; }
        public override void SetOwner(TECustomNode owner) { BeforeOwner(this); base.SetOwner(owner); }
        public override TSet<TEControlStyleElement> GetControlStyle()
        {
            StyleCalls++;
            return TSet<TEControlStyleElement>.Of(EngineUnit.csShared);
        }
        public override int AddRef() => AddReference(this);
        public override int Release() => --References;
        public override void Destroy() { Destructions++; base.Destroy(); }
    }

    private sealed class ConvertedTEChangeNotifyEventList : TEChangeNotifyEventList, IProbe
    {
        public int OwnerCalls { get; set; }
        public int StyleCalls { get; set; }
        public int References { get; set; }
        public int Destructions { get; set; }
        internal ConvertedTEChangeNotifyEventList(TECustomNode owner) : base(owner) { }
        internal ConvertedTEChangeNotifyEventList(ConstructionContext? context, TECustomNode owner, bool fail = false)
            : base(context, owner)
        {
            if (fail) throw new InvalidOperationException("bridge failure");
        }
        public override void SetOwner(TECustomNode owner) { BeforeOwner(this); base.SetOwner(owner); }
        public override TSet<TEControlStyleElement> GetControlStyle()
        {
            StyleCalls++;
            return TSet<TEControlStyleElement>.Of(EngineUnit.csShared);
        }
        public override int AddRef() => AddReference(this);
        public override int Release() => --References;
        public override void Destroy() { Destructions++; base.Destroy(); }
    }

    private sealed class LegacyTEChangeNotifyEventList : TEChangeNotifyEventList, IProbe
    {
        public int OwnerCalls { get; set; }
        public int StyleCalls { get; set; }
        public int References { get; set; }
        public int Destructions { get; set; }
        public override void SetOwner(TECustomNode owner) { BeforeOwner(this); base.SetOwner(owner); }
        public override TSet<TEControlStyleElement> GetControlStyle()
        {
            StyleCalls++;
            return TSet<TEControlStyleElement>.Of(EngineUnit.csShared);
        }
        public override int AddRef() => AddReference(this);
        public override int Release() => --References;
        public override void Destroy() { Destructions++; base.Destroy(); }
    }

    private sealed class ConvertedTEObjectEditor : TEObjectEditor, IProbe
    {
        public int OwnerCalls { get; set; }
        public int StyleCalls { get; set; }
        public int References { get; set; }
        public int Destructions { get; set; }
        internal ConvertedTEObjectEditor(TECustomNode owner) : base(owner) { }
        internal ConvertedTEObjectEditor(ConstructionContext? context, TECustomNode owner, bool fail = false)
            : base(context, owner)
        {
            if (fail) throw new InvalidOperationException("bridge failure");
        }
        public override void SetOwner(TECustomNode owner) { BeforeOwner(this); base.SetOwner(owner); }
        public override TSet<TEControlStyleElement> GetControlStyle()
        {
            StyleCalls++;
            return TSet<TEControlStyleElement>.Of(EngineUnit.csShared);
        }
        public override int AddRef() => AddReference(this);
        public override int Release() => --References;
        public override void Destroy() { Destructions++; base.Destroy(); }
    }

    private sealed class LegacyTEObjectEditor : TEObjectEditor, IProbe
    {
        public int OwnerCalls { get; set; }
        public int StyleCalls { get; set; }
        public int References { get; set; }
        public int Destructions { get; set; }
        public override void SetOwner(TECustomNode owner) { BeforeOwner(this); base.SetOwner(owner); }
        public override TSet<TEControlStyleElement> GetControlStyle()
        {
            StyleCalls++;
            return TSet<TEControlStyleElement>.Of(EngineUnit.csShared);
        }
        public override int AddRef() => AddReference(this);
        public override int Release() => --References;
        public override void Destroy() { Destructions++; base.Destroy(); }
    }

    private sealed class ConvertedTEInverseSortNode : TEInverseSortNode, IProbe
    {
        public int OwnerCalls { get; set; }
        public int StyleCalls { get; set; }
        public int References { get; set; }
        public int Destructions { get; set; }
        internal ConvertedTEInverseSortNode(TECustomNode owner) : base(owner) { }
        internal ConvertedTEInverseSortNode(ConstructionContext? context, TECustomNode owner, bool fail = false)
            : base(context, owner)
        {
            if (fail) throw new InvalidOperationException("bridge failure");
        }
        public override void SetOwner(TECustomNode owner) { BeforeOwner(this); base.SetOwner(owner); }
        public override TSet<TEControlStyleElement> GetControlStyle()
        {
            StyleCalls++;
            return TSet<TEControlStyleElement>.Of(EngineUnit.csShared);
        }
        public override int AddRef() => AddReference(this);
        public override int Release() => --References;
        public override void Destroy() { Destructions++; base.Destroy(); }
    }

    private sealed class LegacyTEInverseSortNode : TEInverseSortNode, IProbe
    {
        public int OwnerCalls { get; set; }
        public int StyleCalls { get; set; }
        public int References { get; set; }
        public int Destructions { get; set; }
        public override void SetOwner(TECustomNode owner) { BeforeOwner(this); base.SetOwner(owner); }
        public override TSet<TEControlStyleElement> GetControlStyle()
        {
            StyleCalls++;
            return TSet<TEControlStyleElement>.Of(EngineUnit.csShared);
        }
        public override int AddRef() => AddReference(this);
        public override int Release() => --References;
        public override void Destroy() { Destructions++; base.Destroy(); }
    }

    private sealed class ConvertedTEControlStyleInverseSortNode : TEControlStyleInverseSortNode, IProbe
    {
        public int OwnerCalls { get; set; }
        public int StyleCalls { get; set; }
        public int References { get; set; }
        public int Destructions { get; set; }
        internal ConvertedTEControlStyleInverseSortNode(TECustomNode owner) : base(owner) { }
        internal ConvertedTEControlStyleInverseSortNode(ConstructionContext? context, TECustomNode owner, bool fail = false)
            : base(context, owner)
        {
            if (fail) throw new InvalidOperationException("bridge failure");
        }
        public override void SetOwner(TECustomNode owner) { BeforeOwner(this); base.SetOwner(owner); }
        public override TSet<TEControlStyleElement> GetControlStyle()
        {
            StyleCalls++;
            return TSet<TEControlStyleElement>.Of(EngineUnit.csShared);
        }
        public override int AddRef() => AddReference(this);
        public override int Release() => --References;
        public override void Destroy() { Destructions++; base.Destroy(); }
    }

    private sealed class LegacyTEControlStyleInverseSortNode : TEControlStyleInverseSortNode, IProbe
    {
        public int OwnerCalls { get; set; }
        public int StyleCalls { get; set; }
        public int References { get; set; }
        public int Destructions { get; set; }
        public override void SetOwner(TECustomNode owner) { BeforeOwner(this); base.SetOwner(owner); }
        public override TSet<TEControlStyleElement> GetControlStyle()
        {
            StyleCalls++;
            return TSet<TEControlStyleElement>.Of(EngineUnit.csShared);
        }
        public override int AddRef() => AddReference(this);
        public override int Release() => --References;
        public override void Destroy() { Destructions++; base.Destroy(); }
    }

    private static TEObject Converted(int kind, TECustomNode owner, int route, bool fail = false)
    {
        if (route == 2)
            return ConstructionContext.Create(context => WithContext(kind, context, owner, fail));
        if (route == 1) return WithContext(kind, null, owner, fail);
        return kind switch
        {
            0 => new ConvertedTENode(owner),
            1 => new ConvertedTEResource(owner),
            2 => new ConvertedTEChangeNotifyNode(owner),
            3 => new ConvertedTEReferenceNode(owner),
            4 => new ConvertedTECustomOwner(owner),
            5 => new ConvertedTEResourceList(owner),
            6 => new ConvertedTEControlStyleNode(owner),
            7 => new ConvertedTEChangeNotifyEventList(owner),
            8 => new ConvertedTEObjectEditor(owner),
            9 => new ConvertedTEInverseSortNode(owner),
            10 => new ConvertedTEControlStyleInverseSortNode(owner),
            _ => throw new ArgumentOutOfRangeException(nameof(kind))
        };
    }

    private static TEObject WithContext(int kind, ConstructionContext? context, TECustomNode owner, bool fail) => kind switch
    {
        0 => new ConvertedTENode(context, owner, fail),
        1 => new ConvertedTEResource(context, owner, fail),
        2 => new ConvertedTEChangeNotifyNode(context, owner, fail),
        3 => new ConvertedTEReferenceNode(context, owner, fail),
        4 => new ConvertedTECustomOwner(context, owner, fail),
        5 => new ConvertedTEResourceList(context, owner, fail),
        6 => new ConvertedTEControlStyleNode(context, owner, fail),
        7 => new ConvertedTEChangeNotifyEventList(context, owner, fail),
        8 => new ConvertedTEObjectEditor(context, owner, fail),
        9 => new ConvertedTEInverseSortNode(context, owner, fail),
        10 => new ConvertedTEControlStyleInverseSortNode(context, owner, fail),
        _ => throw new ArgumentOutOfRangeException(nameof(kind))
    };

    private static TEObject RawLegacy(int kind) => kind switch
    {
        0 => new LegacyTENode(),
        1 => new LegacyTEResource(),
        2 => new LegacyTEChangeNotifyNode(),
        3 => new LegacyTEReferenceNode(),
        4 => new LegacyTECustomOwner(),
        5 => new LegacyTEResourceList(),
        6 => new LegacyTEControlStyleNode(),
        7 => new LegacyTEChangeNotifyEventList(),
        8 => new LegacyTEObjectEditor(),
        9 => new LegacyTEInverseSortNode(),
        10 => new LegacyTEControlStyleInverseSortNode(),
        _ => throw new ArgumentOutOfRangeException(nameof(kind))
    };

    private static readonly Type[] ConvertedTypes = [
        typeof(ConvertedTENode),
        typeof(ConvertedTEResource),
        typeof(ConvertedTEChangeNotifyNode),
        typeof(ConvertedTEReferenceNode),
        typeof(ConvertedTECustomOwner),
        typeof(ConvertedTEResourceList),
        typeof(ConvertedTEControlStyleNode),
        typeof(ConvertedTEChangeNotifyEventList),
        typeof(ConvertedTEObjectEditor),
        typeof(ConvertedTEInverseSortNode),
        typeof(ConvertedTEControlStyleInverseSortNode)
    ];

    private static readonly Type[] LegacyTypes = [
        typeof(LegacyTENode),
        typeof(LegacyTEResource),
        typeof(LegacyTEChangeNotifyNode),
        typeof(LegacyTEReferenceNode),
        typeof(LegacyTECustomOwner),
        typeof(LegacyTEResourceList),
        typeof(LegacyTEControlStyleNode),
        typeof(LegacyTEChangeNotifyEventList),
        typeof(LegacyTEObjectEditor),
        typeof(LegacyTEInverseSortNode),
        typeof(LegacyTEControlStyleInverseSortNode)
    ];

    static EngineBridgeConstructionTests()
    {
        for (int index = 0; index < ConvertedTypes.Length; index++)
        {
            int kind = index;
            var parent = EClass.Of(ConvertedTypes[kind].BaseType!);
            EClass.Register(ConvertedTypes[kind], parent.ClassID, owner => Converted(kind, owner, 2), parent.InstanceSize);
            MetaRegistry.Register(ConvertedTypes[kind], NewMeta(kind), parent.InstanceSize);
            EClass.Register(LegacyTypes[kind], parent.ClassID, owner => EClass.Initialize(RawLegacy(kind), owner), parent.InstanceSize);
            MetaRegistry.Register(LegacyTypes[kind], NewMeta(kind), () => RawLegacy(kind), parent.InstanceSize);
        }
    }

    private static TEObject.Meta NewMeta(int kind) => kind switch
    {
        0 => new TENode.Meta(),
        1 => new TEResource.Meta(),
        2 => new TEChangeNotifyNode.Meta(),
        3 => new TENode.Meta(),
        4 => new TECustomOwner.Meta(),
        5 => new TEResourceList.Meta(),
        6 => new TEControlStyleNode.Meta(),
        7 => new TEChangeNotifyNode.Meta(),
        8 => new TEObjectEditor.Meta(),
        9 => new TEInverseSortNode.Meta(),
        10 => new TEControlStyleInverseSortNode.Meta(),
        _ => throw new ArgumentOutOfRangeException(nameof(kind))
    };

    private static TEObject ThroughTypedMeta(int kind, TEObject.Meta meta, TECustomNode owner) => kind switch
    {
        0 => ((TENode.Meta)meta).Create(owner),
        1 => ((TEResource.Meta)meta).Create(owner),
        2 => ((TEChangeNotifyNode.Meta)meta).Create(owner),
        3 => ((TENode.Meta)meta).Create(owner),
        4 => ((TECustomOwner.Meta)meta).Create(owner),
        5 => ((TEResourceList.Meta)meta).Create(owner),
        6 => ((TEControlStyleNode.Meta)meta).Create(owner),
        7 => ((TEChangeNotifyNode.Meta)meta).Create(owner),
        8 => ((TEObjectEditor.Meta)meta).Create(owner),
        9 => ((TEInverseSortNode.Meta)meta).Create(owner),
        10 => ((TEControlStyleInverseSortNode.Meta)meta).Create(owner),
        _ => throw new ArgumentOutOfRangeException(nameof(kind))
    };

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(9)]
    [InlineData(10)]
    public void TypedMetaclassFacadesUseCompletedCreationAndKeepLegacyRawAllocation(int kind)
    {
        var owner = new Owner();
        foreach (Type type in new[] { ConvertedTypes[kind], LegacyTypes[kind] })
        {
            var meta = (TEObject.Meta)MetaRegistry.Of(type);
            foreach (bool owned in new[] { false, true })
            {
                TECustomNode argument = owned ? owner : null!;
                TEObject typed = ThroughTypedMeta(kind, meta, argument);
                TEObject root = meta.Create(argument);
                try
                {
                    Assert.IsType(type, typed);
                    Assert.IsType(type, root);
                    AssertState(typed, argument);
                    AssertState(root, argument);
                }
                finally { typed.Destroy(); root.Destroy(); }
            }
            if (type == ConvertedTypes[kind]) Assert.Throws<InvalidOperationException>(() => meta.NewInstance());
            else
            {
                TEObject raw = (TEObject)meta.NewInstance();
                Assert.Equal(0, ((IProbe)raw).OwnerCalls);
                raw.Create__init(null!);
                raw.Destroy();
            }
        }
        Assert.Empty(owner.Members);
    }

    private static int ChildAllocations(int kind) => kind switch { 1 or 8 or 9 or 10 => 0, 7 => 2, _ => 1 };

    private static void AssertState(TEObject value, TECustomNode owner)
    {
        Assert.Same(owner, value.Owner);
        Assert.Equal(1, ((IProbe)value).OwnerCalls);
        Assert.Equal(0, ((IProbe)value).StyleCalls);
        if (Field(value, "FChildren") is { } children)
            Assert.Equal(0, Assert.IsType<TEObjectList>(children.GetValue(value)).Count);
        if (Field(value, "FEvents") is { } events)
            Assert.Equal(0, Assert.IsType<TEventList>(events.GetValue(value)).Count);
        if (Field(value, "FControlStyle") is { } style)
            Assert.Equal(TSet<TEControlStyleElement>.Of(EngineUnit.csStored), style.GetValue(value));
        if (Field(value, "FUpdateCount") is { } updates) Assert.Equal(0, updates.GetValue(value));
        if (value is TEResource) Assert.Same(owner, Field(value, "FResourceList")!.GetValue(value));
        if (value is TECustomOwner) Assert.Null(Field(value, "FEngine")!.GetValue(value));
        if (value is TESortNode sort)
        {
            Assert.Empty(sort.FItems);
            Assert.Equal(0, sort.FCount);
            Assert.Equal(0, sort.FCapacity);
            Assert.False(sort.FSorted);
        }
    }

    public static TheoryData<int, bool, int> BridgeCases
    {
        get
        {
            var cases = new TheoryData<int, bool, int>();
            for (int kind = 0; kind < 11; kind++)
                foreach (bool owned in new[] { false, true })
                    for (int route = 0; route < 3; route++) cases.Add(kind, owned, route);
            return cases;
        }
    }

    [Theory]
    [MemberData(nameof(BridgeCases))]
    public void SemanticAndLegacyChainsRunBodiesOnceWithMatchingState(int kind, bool owned, int route)
    {
        var owner = new Owner();
        TECustomNode argument = owned ? owner : null!;
        int before = new TObject().InstanceId;
        TEObject converted = Converted(kind, argument, route);
        Assert.Equal(ChildAllocations(kind) + 2, new TObject().InstanceId - before);
        TEObject legacy = RawLegacy(kind);
        Assert.Equal(0, ((IProbe)legacy).OwnerCalls);
        Assert.Null(Field(legacy, "FChildren")?.GetValue(legacy));
        before = new TObject().InstanceId;
        legacy.Create__init(argument);
        Assert.Equal(ChildAllocations(kind) + 1, new TObject().InstanceId - before);
        try
        {
            AssertState(converted, argument);
            AssertState(legacy, argument);
            Assert.Equal(owned ? 2 : 0, owner.Members.Count);
        }
        finally { converted.Destroy(); legacy.Destroy(); }
        Assert.Empty(owner.Members);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(9)]
    [InlineData(10)]
    public void ContextFailureDestroysOnceAndRemovesThePublishedObject(int kind)
    {
        var owner = new Owner();
        Assert.Equal("bridge failure", Assert.Throws<InvalidOperationException>(
            () => Converted(kind, owner, 2, fail: true)).Message);
        Assert.Empty(owner.Members);
        Assert.Equal(1, ((IProbe)Assert.Single(owner.Removed)).Destructions);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(9)]
    [InlineData(10)]
    public void ReferenceOwnerClearsBeforeAddRefAndKeepsTheLegacyCount(int kind)
    {
        var owner = TEReferenceNode.Create(null!);
        TEObject converted = Converted(kind, owner, 0);
        TEObject legacy = RawLegacy(kind);
        legacy.Create__init(owner);
        Assert.Equal(1, ((IProbe)converted).References);
        Assert.Equal(1, ((IProbe)legacy).References);
        Assert.Same(legacy.Owner, converted.Owner);
        Assert.Equal(kind is 4 or 5 ? owner : null, converted.Owner);
        Assert.Equal(2, owner.FChildren.Count);
        owner.Destroy();
        Assert.Equal(0, ((IProbe)converted).References);
        Assert.Equal(0, ((IProbe)legacy).References);
        converted.Destroy();
        legacy.Destroy();
    }

    private sealed class ByteDescendant : TByteList { }
    private sealed class IntegerDescendant : TIntegerList { }

    [Fact]
    public void AllocationWrappersAndTheirLegacyDescendantsKeepEmptyStorageAndListOperations()
    {
        foreach (TByteList list in new TByteList[] { new(), new ByteDescendant() })
        {
            Assert.Equal(0, list.Count); Assert.Equal(0, list.Capacity); Assert.Empty(list.List);
            Assert.Equal(0, list.Default);
            list.Add(3); list.Add(7);
            Assert.Equal(new byte[] { 3, 7 }, list.List.Take(list.Count));
            list.Destroy();
        }
        foreach (TIntegerList list in new TIntegerList[] { new(), new IntegerDescendant() })
        {
            Assert.Equal(0, list.Count); Assert.Equal(0, list.Capacity); Assert.Empty(list.List);
            Assert.Equal(0, list.Default);
            list.Add(-3); list.Add(700);
            Assert.Equal(new[] { -3, 700 }, list.List.Take(list.Count));
            list.Destroy();
        }
        Assert.Null(typeof(TByteList).GetMethod("Create", BindingFlags.Public | BindingFlags.Static));
        Assert.Null(typeof(TIntegerList).GetMethod("Create", BindingFlags.Public | BindingFlags.Static));
    }
}
