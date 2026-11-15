using System.Reflection;
using System.Reflection.Emit;

namespace Aow.Game.Tests;

/// <summary>
/// One place that names a method allowed to reach the original image, and why.
/// </summary>
internal readonly record struct AllowedSite(string TypeName, string MethodName, string Reason);

/// <summary>A call to the original image from a method that is not an allowed site.</summary>
internal readonly record struct BackendReach(string Caller, string Target)
{
    public override string ToString() => Caller + " calls " + Target;
}

/// <summary>A public member that exposes a stored id, a storage width, a raw image or residue.</summary>
internal readonly record struct ExposedMember(string Owner, string Member, string Kind, string? UsedBy)
{
    public override string ToString() =>
        UsedBy is null ? Kind + ": " + Owner + "." + Member : Kind + ": " + Owner + "." + Member + " used by " + UsedBy;
}

/// <summary>
/// Reads the built assemblies. An own disk path fails when it calls <c>OriginalBackend</c>, or calls
/// <c>ImageTree</c> from outside the original codecs, unless the caller is an allowed site. The public
/// surface of <c>Aow.Formats.Tree</c> fails when it exposes a stored id, a storage width, raw image
/// bytes or residue. A grid width stays, because the type also has a height. The bit-list image keeps
/// its bit bytes, because the engine reads them. Residue in a document is the member $reserved.
/// </summary>
internal static class PersistenceInventory
{
    private const string OriginalBackendName = "Aow.Formats.Tree.OriginalBackend";
    private const string ImageTreeName = "Aow.Formats.Tree.ImageTree";
    private const string TreeNamespace = "Aow.Formats.Tree";

    internal static readonly AllowedSite[] AllowedSites =
    [
        new("Aow.Formats.GameBoundary", "ReadImageTree",
            "Import reads an original image the game already holds, such as a campaign map that is still an image, a preview or an imported file."),
        new("Aow.Formats.GameBoundary", "WriteImage",
            "Export writes the original image of a map the image can carry. A map the image cannot carry is stored as the own document instead."),
        new("Aow.Formats.GameBoundary", "WritePacket",
            "The packet adapter writes the legacy packet image. A value that image cannot carry is refused and the packet is not written."),
        new("Aow.Formats.GameBoundary", "ReadPacket",
            "The packet adapter reads the legacy packet image."),
        new("Aow.Formats.GameBoundary", "WriteWrittenObject",
            "Original files stay original until milestone 10. This writes the game set files, the system hero and item libraries and the e-mail game index."),
        new("Aow.Formats.GameBoundary", "ReadWrittenObject",
            "Original files stay original until milestone 10. This reads the game set files, the system hero and item libraries and the e-mail game index."),
        new("Aow.Engine.PersistSupport", "BitListOverRead",
            "A bit-list load reads up to 3 bytes past the stored bits. This rebuilds those bytes from the members that follow and writes no file. Own documents still use the document backend."),
    ];

    private static readonly OperandType[] OneByte = new OperandType[256];
    private static readonly bool[] OneByteKnown = new bool[256];
    private static readonly OperandType[] TwoByte = new OperandType[256];
    private static readonly bool[] TwoByteKnown = new bool[256];

    static PersistenceInventory()
    {
        foreach (var field in typeof(OpCodes).GetFields(BindingFlags.Public | BindingFlags.Static))
        {
            if (field.GetValue(null) is not OpCode op) continue;
            if (op.Size == 1)
            {
                OneByte[op.Value & 0xFF] = op.OperandType;
                OneByteKnown[op.Value & 0xFF] = true;
            }
            else if (op.Size == 2)
            {
                TwoByte[op.Value & 0xFF] = op.OperandType;
                TwoByteKnown[op.Value & 0xFF] = true;
            }
        }
    }

    internal static IReadOnlyList<BackendReach> BackendReaches(params Assembly[] assemblies) =>
        Collect(assemblies, only: null).Reaches;

    internal static IReadOnlyList<BackendReach> BackendReaches(Type only) =>
        Collect([only.Assembly], only).Reaches;

    internal static IReadOnlyList<ExposedMember> ExposedMembers(params Assembly[] assemblies)
    {
        var found = new List<ExposedMember>();
        foreach (var assembly in assemblies)
        {
            if (assembly.GetName().Name is "Aow.Formats")
            {
                foreach (var type in Types(assembly))
                {
                    if (type.Namespace != TreeNamespace || !type.IsVisible) continue;
                    found.AddRange(Declared(type));
                }
            }
        }
        found.AddRange(Collect(assemblies, only: null).Uses);
        found.Sort(CompareExposed);
        return found;
    }

    internal static IReadOnlyList<ExposedMember> ExposedMembers(Type type)
    {
        var found = Declared(type);
        found.Sort(CompareExposed);
        return found;
    }

    private readonly record struct Collected(IReadOnlyList<BackendReach> Reaches, IReadOnlyList<ExposedMember> Uses);

    private static Collected Collect(Assembly[] assemblies, Type? only)
    {
        var reaches = new List<BackendReach>();
        var uses = new List<ExposedMember>();
        foreach (var assembly in assemblies)
        {
            bool usesCount = assembly.GetName().Name is "Aow.Game" or "Aow.Engine";
            foreach (var type in Types(assembly))
            {
                foreach (var method in Methods(type))
                {
                    if (!InScope(method.DeclaringType, only)) continue;
                    var (callerType, callerMethod) = Logical(method);
                    string caller = callerType + "." + callerMethod;
                    Walk(method, (target, token) =>
                    {
                        if (target is null)
                        {
                            reaches.Add(new BackendReach(caller, "unresolved method token 0x" + token.ToString("X8")));
                            return;
                        }
                        var declaring = Definition(target.DeclaringType);
                        if (declaring is null) return;
                        string declaringName = declaring.FullName ?? declaring.Name;
                        if (declaringName == OriginalBackendName)
                        {
                            if (callerType != OriginalBackendName && !Allowed(callerType, callerMethod))
                                reaches.Add(new BackendReach(caller, declaringName + "." + target.Name));
                            return;
                        }
                        if (declaringName == ImageTreeName)
                        {
                            if (!IsImageImplementation(callerType) && !Allowed(callerType, callerMethod))
                                reaches.Add(new BackendReach(caller, declaringName + "." + target.Name));
                            return;
                        }
                        if (!usesCount || declaring.Namespace != TreeNamespace) return;
                        if (Exposure(target, declaring) is not { } hit) return;
                        uses.Add(hit with { UsedBy = caller });
                    });
                }
            }
        }
        reaches.Sort(static (left, right) =>
        {
            int name = string.CompareOrdinal(left.Caller, right.Caller);
            return name != 0 ? name : string.CompareOrdinal(left.Target, right.Target);
        });
        uses.Sort(CompareExposed);
        return new Collected(reaches, uses);
    }

    private static List<ExposedMember> Declared(Type type)
    {
        const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
        var found = new List<ExposedMember>();
        string owner = type.FullName ?? type.Name;
        foreach (var property in type.GetProperties(flags))
        {
            if (Kind(property.Name, property.PropertyType, type) is { } kind)
                found.Add(new ExposedMember(owner, property.Name, kind, UsedBy: null));
        }
        foreach (var field in type.GetFields(flags))
        {
            if (field.IsSpecialName) continue;
            if (Kind(field.Name, field.FieldType, type) is { } kind)
                found.Add(new ExposedMember(owner, field.Name, kind, UsedBy: null));
        }
        foreach (var method in type.GetMethods(flags))
        {
            if (method.IsSpecialName) continue;
            if (IsStoredId(method.Name) && IsIntegral(method.ReturnType))
                found.Add(new ExposedMember(owner, method.Name, "stored id", UsedBy: null));
        }
        return found;
    }

    private static ExposedMember? Exposure(MemberInfo member, Type declaring)
    {
        if (member is MethodInfo method && TryProperty(method, out string name, out var type))
        {
            if (Kind(name, type, declaring) is not { } kind) return null;
            return new ExposedMember(declaring.FullName ?? declaring.Name, name, kind, UsedBy: null);
        }
        if (member is FieldInfo field)
        {
            if (Kind(field.Name, field.FieldType, declaring) is not { } kind) return null;
            return new ExposedMember(declaring.FullName ?? declaring.Name, field.Name, kind, UsedBy: null);
        }
        if (member is not MethodBase plain || plain.IsSpecialName) return null;
        if (!IsStoredId(plain.Name)) return null;
        if (plain is not MethodInfo info || !IsIntegral(info.ReturnType)) return null;
        return new ExposedMember(declaring.FullName ?? declaring.Name, plain.Name, "stored id", UsedBy: null);
    }

    private static string? Kind(string name, Type memberType, Type declaring)
    {
        if (IsStoredId(name) && IsIntegral(memberType)) return "stored id";
        if (name == "Width" && IsIntegral(memberType) && !HasPublicHeight(declaring)) return "storage width";
        if (name is "Image" or "Data" && IsRawBytes(memberType)) return "raw image";
        if (name == "Mask" && IsIntegral(memberType)) return "raw image";
        if (name is "Residue" or "Reserved") return "residue";
        return null;
    }

    private static bool IsStoredId(string name) => name is "Id" or "PropertyId" or "ClassId" or "ReservedId";

    private static bool IsIntegral(Type type)
    {
        if (Nullable.GetUnderlyingType(type) is { } under) type = under;
        return type == typeof(byte) || type == typeof(sbyte) || type == typeof(short) || type == typeof(ushort)
            || type == typeof(int) || type == typeof(uint) || type == typeof(long) || type == typeof(ulong);
    }

    private static bool IsRawBytes(Type type)
    {
        if (type == typeof(byte[])) return true;
        if (!type.IsGenericType) return false;
        var definition = type.GetGenericTypeDefinition();
        return definition == typeof(ReadOnlyMemory<>) || definition == typeof(Memory<>)
            ? type.GetGenericArguments()[0] == typeof(byte)
            : false;
    }

    private static bool HasPublicHeight(Type type) =>
        type.GetProperty("Height", BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly) is not null
        || type.BaseType is { } parent && parent != typeof(object) && parent != typeof(ValueType) && HasPublicHeight(parent);

    private static bool Allowed(string typeName, string methodName)
    {
        foreach (var site in AllowedSites)
            if (site.TypeName == typeName && site.MethodName == methodName) return true;
        return false;
    }

    private static bool IsImageImplementation(string typeName) =>
        typeName == ImageTreeName || typeName == OriginalBackendName
        || typeName.StartsWith("Aow.Formats.Original.", StringComparison.Ordinal);

    private static bool InScope(Type? declaring, Type? only)
    {
        if (only is null) return true;
        for (var type = declaring; type is not null; type = type.DeclaringType)
            if (type == only) return true;
        return false;
    }

    private static (string TypeName, string MethodName) Logical(MethodBase method)
    {
        var type = method.DeclaringType ?? throw new InvalidOperationException("A method has no declaring type.");
        string name = method.Name;
        while (type.IsNested && type.Name.IndexOf('<') >= 0)
        {
            var match = System.Text.RegularExpressions.Regex.Match(type.Name, "<([^>]+)>");
            if (match.Success && match.Groups[1].Value.Length > 0) name = match.Groups[1].Value;
            if (type.DeclaringType is not { } outer) break;
            type = outer;
        }
        var methodMatch = System.Text.RegularExpressions.Regex.Match(name, "^<([^>]+)>");
        if (methodMatch.Success && methodMatch.Groups[1].Value.Length > 0) name = methodMatch.Groups[1].Value;
        return (type.FullName ?? type.Name, name);
    }

    private static Type? Definition(Type? type)
    {
        if (type is null) return null;
        return type.IsGenericType ? type.GetGenericTypeDefinition() : type;
    }

    private static IEnumerable<MethodBase> Methods(Type type)
    {
        const BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
        foreach (var method in type.GetMethods(flags)) yield return method;
        foreach (var ctor in type.GetConstructors(flags)) yield return ctor;
        if (type.TypeInitializer is { } initializer) yield return initializer;
    }

    private static Type[] Types(Assembly assembly)
    {
        try
        {
            return assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException error)
        {
            string reason = string.Join("\n", error.LoaderExceptions.Select(item => item?.Message));
            throw new InvalidOperationException("Loading types from " + assembly.GetName().Name + " failed.\n" + reason, error);
        }
    }

    private static void Walk(MethodBase method, Action<MemberInfo?, int> onMethod)
    {
        MethodBody? body;
        try
        {
            body = method.GetMethodBody();
        }
        catch (InvalidOperationException)
        {
            return;
        }
        if (body?.GetILAsByteArray() is not { Length: > 0 } il) return;
        var module = method.Module;
        int index = 0;
        while (index < il.Length)
        {
            int code = il[index];
            bool wide = code == 0xFE;
            OperandType operand;
            int operandAt;
            if (wide)
            {
                if (index + 1 >= il.Length) Fail(method, index);
                code = il[index + 1];
                if (!TwoByteKnown[code]) Fail(method, index);
                operand = TwoByte[code];
                operandAt = index + 2;
            }
            else
            {
                if (!OneByteKnown[code]) Fail(method, index);
                operand = OneByte[code];
                operandAt = index + 1;
            }
            int size = OperandBytes(il, operandAt, operand, method);
            if (IsMethodOpcode(code, wide))
            {
                int token = ReadInt(il, operandAt, method);
                onMethod(ResolveMethod(module, token, method), token);
            }
            else if (!wide && IsFieldOpcode(code))
            {
                int token = ReadInt(il, operandAt, method);
                onMethod(ResolveField(module, token, method), token);
            }
            index = operandAt + size;
        }
    }

    private static bool IsMethodOpcode(int code, bool wide) =>
        wide ? code is 0x06 or 0x07 : code is 0x28 or 0x6F or 0x27;

    private static bool IsFieldOpcode(int code) => code is 0x7B or 0x7C or 0x7D or 0x7E or 0x7F or 0x80;

    private static int OperandBytes(byte[] il, int at, OperandType operand, MethodBase method) => operand switch
    {
        OperandType.InlineNone => 0,
        OperandType.ShortInlineBrTarget or OperandType.ShortInlineI or OperandType.ShortInlineVar => 1,
        OperandType.InlineVar => 2,
        OperandType.InlineI or OperandType.InlineBrTarget or OperandType.InlineMethod or OperandType.InlineField
            or OperandType.InlineType or OperandType.InlineString or OperandType.InlineSig or OperandType.InlineTok
            or OperandType.ShortInlineR => 4,
        OperandType.InlineI8 or OperandType.InlineR => 8,
        OperandType.InlineSwitch => SwitchBytes(il, at, method),
        _ => throw new InvalidOperationException(Label(method) + " has operand " + operand + "."),
    };

    private static int SwitchBytes(byte[] il, int at, MethodBase method)
    {
        int count = ReadInt(il, at, method);
        if (count < 0) Fail(method, at);
        return checked(4 + count * 4);
    }

    private static int ReadInt(byte[] il, int at, MethodBase method)
    {
        if (at < 0 || at + 4 > il.Length) Fail(method, at);
        return BitConverter.ToInt32(il, at);
    }

    private static MethodBase? ResolveMethod(Module module, int token, MethodBase caller)
    {
        var typeArgs = caller.DeclaringType is { IsGenericType: true } declaring ? declaring.GetGenericArguments() : null;
        var methodArgs = caller.IsGenericMethod ? caller.GetGenericArguments() : null;
        try
        {
            return module.ResolveMethod(token, typeArgs, methodArgs);
        }
        catch (Exception error) when (error is ArgumentException or TypeLoadException or FileNotFoundException or BadImageFormatException)
        {
            try
            {
                return module.ResolveMethod(token);
            }
            catch (Exception again) when (again is ArgumentException or TypeLoadException or FileNotFoundException or BadImageFormatException)
            {
                return null;
            }
        }
    }

    private static FieldInfo? ResolveField(Module module, int token, MethodBase caller)
    {
        var typeArgs = caller.DeclaringType is { IsGenericType: true } declaring ? declaring.GetGenericArguments() : null;
        var methodArgs = caller.IsGenericMethod ? caller.GetGenericArguments() : null;
        try
        {
            return module.ResolveField(token, typeArgs, methodArgs);
        }
        catch (Exception error) when (error is ArgumentException or TypeLoadException or FileNotFoundException or BadImageFormatException)
        {
            try
            {
                return module.ResolveField(token);
            }
            catch (Exception again) when (again is ArgumentException or TypeLoadException or FileNotFoundException or BadImageFormatException)
            {
                return null;
            }
        }
    }

    private static bool TryProperty(MethodInfo method, out string name, out Type type)
    {
        name = "";
        type = typeof(void);
        if (!method.IsSpecialName) return false;
        if (method.Name.StartsWith("get_", StringComparison.Ordinal) && method.Name.Length > 4)
        {
            name = method.Name[4..];
            type = method.ReturnType;
            return true;
        }
        if (method.Name.StartsWith("set_", StringComparison.Ordinal) && method.Name.Length > 4 && method.GetParameters() is { Length: > 0 } parameters)
        {
            name = method.Name[4..];
            type = parameters[^1].ParameterType;
            return true;
        }
        return false;
    }

    private static int CompareExposed(ExposedMember left, ExposedMember right)
    {
        int owner = string.CompareOrdinal(left.Owner, right.Owner);
        if (owner != 0) return owner;
        int member = string.CompareOrdinal(left.Member, right.Member);
        if (member != 0) return member;
        int kind = string.CompareOrdinal(left.Kind, right.Kind);
        return kind != 0 ? kind : string.CompareOrdinal(left.UsedBy, right.UsedBy);
    }

    private static void Fail(MethodBase method, int offset) =>
        throw new InvalidOperationException(Label(method) + " has an instruction the inventory cannot read at offset " + offset.ToString() + ".");

    private static string Label(MethodBase method) =>
        (method.DeclaringType?.FullName ?? method.Module.Name) + "." + method.Name;
}
