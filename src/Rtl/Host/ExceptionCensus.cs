using System.Runtime.ExceptionServices;
using System.Text;
using System.Text.Json;

namespace Aow.Rtl.Host;

/// <summary>
/// The exception census of the gate (strings.md section 4). A host calls <see cref="Start"/> at entry.
/// When <c>AOW_EXCEPTION_CENSUS</c> names a file, the census counts first-chance exceptions of six
/// kinds and appends one JSON line per run to that file when the process ends. An exception counts at
/// its throw, before a broad catch, <c>Rtl.Wrap</c> or <c>AoWE.Format</c> hides it. The file stands
/// alone so that private hosts without a reference to the RTL can link it. A rethrow raises the
/// event again, so one error counts twice, and an exception that <c>Rtl.Wrap</c> turns into another
/// kind counts once under each. Both are deterministic, which is what a base comparison needs.
/// </summary>
public static class ExceptionCensus
{
    public const string Variable = "AOW_EXCEPTION_CENSUS";

    /// <summary>The counted kinds. An exception counts under the first kind among its type and its bases.</summary>
    public static IReadOnlyList<string> Kinds { get; } = ["NullReferenceException", "IndexOutOfRangeException",
        "ERangeError", "EAccessViolation", "EConvertError", "NullStringException"];

    private static readonly long[] counts = new long[6];
    private static string host = "", path = "";
    private static string[] arguments = [];
    private static int started, written;

    public static bool Active => Volatile.Read(ref started) != 0;

    /// <summary>Registers the census for this process when the variable names a file. A second call does nothing.</summary>
    public static void Start(string hostName, string[] args)
    {
        string? file = Environment.GetEnvironmentVariable(Variable);
        if (string.IsNullOrWhiteSpace(file) || Interlocked.Exchange(ref started, 1) != 0) return;
        path = Path.GetFullPath(file);
        host = hostName;
        arguments = [.. args];
        AppDomain.CurrentDomain.FirstChanceException += Count;
        AppDomain.CurrentDomain.ProcessExit += (_, _) => Finish();
        AppDomain.CurrentDomain.UnhandledException += (_, _) => Finish();
    }

    /// <summary>The index of the kind an exception counts under, or -1.</summary>
    public static int KindOf(System.Exception exception)
    {
        for (Type? type = exception.GetType(); type is not null; type = type.BaseType)
            for (int index = 0; index < Kinds.Count; index++)
                if (type.Name == Kinds[index]) return index;
        return -1;
    }

    private static void Count(object? sender, FirstChanceExceptionEventArgs e)
    {
        int index = KindOf(e.Exception);
        if (index >= 0) Interlocked.Increment(ref counts[index]);
    }

    /// <summary>Appends this run's line once. The process exit and an unhandled exception both call it.</summary>
    public static void Finish()
    {
        if (!Active || Interlocked.Exchange(ref written, 1) != 0) return;
        long[] snapshot = Enumerable.Range(0, counts.Length).Select(index => Interlocked.Read(ref counts[index])).ToArray();
        Append(path, Line(host, arguments, snapshot));
    }

    /// <summary>One run as a JSON object on one line: the host, its arguments and the count of each kind.</summary>
    public static string Line(string hostName, IReadOnlyList<string> args, IReadOnlyList<long> values)
    {
        if (values.Count != Kinds.Count) throw new ArgumentException("one count per kind", nameof(values));
        using var buffer = new MemoryStream();
        using (var writer = new Utf8JsonWriter(buffer))
        {
            writer.WriteStartObject();
            writer.WriteString("host", hostName);
            writer.WriteStartArray("arguments");
            foreach (string argument in args) writer.WriteStringValue(argument);
            writer.WriteEndArray();
            writer.WriteStartObject("counts");
            for (int index = 0; index < Kinds.Count; index++) writer.WriteNumber(Kinds[index], values[index]);
            writer.WriteEndObject();
            writer.WriteEndObject();
        }
        return Encoding.UTF8.GetString(buffer.ToArray());
    }

    /// <summary>Appends a line under an exclusive share, so that parallel hosts never interleave their lines.</summary>
    public static void Append(string file, string line)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(file))!);
        byte[] bytes = Encoding.UTF8.GetBytes(line + "\n");
        for (int attempt = 0; ; attempt++)
        {
            try
            {
                using var stream = new FileStream(file, FileMode.Append, FileAccess.Write, FileShare.None);
                stream.Write(bytes);
                return;
            }
            catch (IOException) when (attempt < 400)
            {
                Thread.Sleep(25);
            }
        }
    }
}
