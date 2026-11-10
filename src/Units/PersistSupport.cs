using System.Globalization;
using Aow.Formats.Tree;
using Aow.Game.Engine;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Engine.Nodes
{

    // The engine side of the persistence tree. Formats does not name these types.
    public sealed class EnginePersistableFactory : IPersistableFactory
    {
        private readonly AoWEngine engine;
        private readonly Dictionary<string, EClass> byName;

        public EnginePersistableFactory(AoWEngine engine)
        {
            this.engine = engine ?? throw new InvalidOperationException("The engine is not running.");
            byName = new Dictionary<string, EClass>();
            foreach (var eclass in EClass.All)
                if (!byName.TryAdd(eclass.ClassName, eclass))
                    throw new InvalidOperationException("Two engine classes are named " + eclass.ClassName + ".");
        }

        public string ClassName(IPersistable obj) => NameTable.Stored(obj.GetType());

        public void Release(IPersistable obj) => (obj as IDisposable)?.Dispose();

        public IPersistable Create(string className)
        {
            if (!byName.TryGetValue(className, out var eclass))
            {
                var resolved = NameTable.ResolveStored(className);
                if (resolved is null || !byName.TryGetValue(resolved, out eclass))
                    throw new InvalidOperationException("The engine has no class " + className + ".");
            }
            if (engine.ClassTable.Find(eclass.ClassID) is not EClass registered || registered.ClassName != eclass.ClassName)
                throw new KeyNotFoundException("Unregistered E-ClassID found (" + eclass.ClassID.ToString(CultureInfo.InvariantCulture) + ")");
            return registered.Create(null);
        }
    }

    public sealed class NodeChildren(ECustomNode node) : IChildCollection
    {
        public int Count => node.GetCount();

        public IPersistable Get(int index) => node.GetChild(index);

        public bool IsStored(int index)
        {
            var child = node.GetChild(index);
            return child != null && child.Owner == node
                && !child.GetControlStyle().Contains(EngineServices.csShared)
                && child.GetControlStyle().Contains(EngineServices.csStored);
        }

        public int Slot(int index) => index;

        public bool KeepIndex => node.GetControlStyle().Contains(EngineServices.csKeepIndex);

        public void Attach(IPersistable child) => ((EObject)child).FOwner = node;

        public void Place(int slot, IPersistable child)
        {
            var obj = (EObject)child;
            obj.FOwner = null;
            if (KeepIndex) obj.SetArrayOwner(node, slot);
            else obj.SetOwner(node);
        }
    }
}

namespace Aow.Game.Engine
{

    public static class PersistSupport
    {
        public static void Record<T>(IPersister persister, string name, ref T value) where T : unmanaged =>
            RecordValues.Transfer(persister, name, ref value);

        public static void UnsignedByte(IPersister persister, string name, ref sbyte value)
        {
            byte stored = unchecked((byte)value);
            persister.Byte(name, ref stored);
            value = unchecked((sbyte)stored);
        }

        public static void Int32Array(IPersister persister, string name, Span<int> values)
        {
            var list = new List<int>(values.Length);
            for (int i = 0; i < values.Length; i++) list.Add(values[i]);
            persister.List(name, list, static (IItemSlot slot, ref int item) => slot.Int(ref item));
            if (!persister.Loading) return;
            if (list.Count > values.Length)
                throw new InvalidOperationException(name + " holds at most " + values.Length + " values.");
            for (int i = 0; i < values.Length; i++) values[i] = i < list.Count ? list[i] : 0;
        }

        public static void UInt16Array(IPersister persister, string name, Span<ushort> values)
        {
            var list = new List<ushort>(values.Length);
            for (int i = 0; i < values.Length; i++) list.Add(values[i]);
            persister.List(name, list, static (IItemSlot slot, ref ushort item) => slot.UShort(ref item));
            if (!persister.Loading) return;
            if (list.Count > values.Length)
                throw new InvalidOperationException(name + " holds at most " + values.Length + " values.");
            for (int i = 0; i < values.Length; i++) values[i] = i < list.Count ? list[i] : (ushort)0;
        }

        public static void Lines(IPersister persister, string name, StringCollection lines)
        {
            var copy = new List<string>(lines.Count);
            for (int i = 0; i < lines.Count; i++) copy.Add(lines[i]);
            persister.Lines(name, copy);
            if (!persister.Loading) return;
            lines.BeginUpdate();
            try
            {
                lines.Clear();
                foreach (var line in copy) lines.Add(line);
            }
            finally
            {
                lines.EndUpdate();
            }
        }

        public static void LinesEx(IPersister persister, string name, StringCollection lines)
        {
            var copy = new List<string>(lines.Count);
            var objects = new List<int>(lines.Count);
            for (int i = 0; i < lines.Count; i++)
            {
                copy.Add(lines[i]);
                objects.Add(ObjectToInt(lines.Objects[i]));
            }
            persister.LinesWithObjects(name, copy, objects);
            if (!persister.Loading) return;
            lines.BeginUpdate();
            try
            {
                lines.Clear();
                for (int i = 0; i < copy.Count; i++) lines.AddObject(copy[i], IntToObject(objects[i]));
            }
            finally
            {
                lines.EndUpdate();
            }
        }
    }
}
