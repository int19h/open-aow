#nullable enable
using System.Buffers.Binary;

using System.Security.Cryptography;

using Aow.Engine.Nodes;
using Aow.Formats;
using Aow.Rtl;
using Aow.Rtl.Host;

namespace Aow.Game.Saves;

/// <summary>
/// The game's file boundary for the in-scope profiles (own-formats.md 4.1 and 4.2). Every
/// in-scope engine routine reads and writes through it with an explicit profile. Reads go
/// through the host's file service and run the whole preflight before the engine touches the
/// current game. Writes convert the engine-written image, validate and serialize it, write a
/// sibling temporary through the file service and replace the old file atomically.
/// </summary>
public static class FileBoundary
{
    /// <summary>The host's file log (--file-log). Null records nothing.</summary>
    public static Action<FileLogRecord>? Log { get; set; }

    /// <summary>The last refused open or write, for the caller that reports it.</summary>
    public static FileLogRecord? LastFailure { get; private set; }

    /// <summary>
    /// Reads a file and prepares its engine image. A refusal returns null, is logged and leaves
    /// the current game and the file untouched.
    /// </summary>
    public static byte[]? Load(string fileName, GameProfile profile, string operation = "open")
    {
        var files = RtlUnit.Files;
        byte[] bytes;
        try
        {
            bytes = ReadAll(files, fileName);
        }
        catch (Exception e) when (e is IOException or UnauthorizedAccessException or ArgumentException)
        {
            var report = new GameFileReport { Operation = operation, Profile = profile, Error = new FormatDiagnostic(Codes.Output, "The file cannot be read: " + e.Message) { File = fileName } };
            Record(files, fileName, report);
            return null;
        }
        string layer = LayerOf(files, fileName);
        // Previews and listings reuse a validated image of the same bytes (own-formats.md 4.3).
        // A final open always validates the actual bytes.
        bool preview = operation != "open";
        var key = preview ? new PreviewKey(layer, fileName, profile.Name, OwnFormat.CurrentVersion, Convert.ToHexStringLower(SHA256.HashData(bytes))) : default;
        if (preview && Previews.TryGet(key, out var cached))
        {
            Record(new FileLogRecord(fileName, layer, cached.Report, cached: true));
            return cached.Image;
        }
        var result = GameBoundary.PrepareLoad(bytes, fileName, profile, FormatMapping.Validators, operation);
        Record(new FileLogRecord(fileName, layer, result.Report));
        if (preview && result.Bytes is not null) Previews.Add(key, new PreviewEntry(result.Bytes, result.Report));
        return result.Bytes;
    }

    /// <summary>The session preview cache. Only successful preflights enter it.</summary>
    public static PreviewCache Previews { get; } = new(64L << 20);

    /// <summary>
    /// Like Load, for the routines whose original raised on a failed open: the refusal raises
    /// EGameFileError after it is logged, before the caller changes anything.
    /// </summary>
    public static byte[] LoadOrRaise(string fileName, GameProfile profile) =>
        Load(fileName, profile) ?? throw new GameFileErrorException(LastFailure!);

    /// <summary>
    /// Converts and writes an engine-written image. Serialization and validation finish before
    /// anything is written. A failure raises EGameFileError and keeps the old file.
    /// </summary>
    public static void Save(string fileName, GameProfile profile, byte[] image)
    {
        var files = RtlUnit.Files;
        var result = GameBoundary.PrepareSave(image, fileName, profile, FormatMapping.Validators);
        if (result.Bytes is null)
        {
            Record(files, fileName, result.Report);
            throw new GameFileErrorException(LastFailure!);
        }
        try
        {
            Replace(files, fileName, result.Bytes);
        }
        catch (Exception e) when (e is IOException or UnauthorizedAccessException or ArgumentException)
        {
            result.Report.Error = new FormatDiagnostic(Codes.Output, "The file cannot be written: " + e.Message) { File = fileName };
            Record(files, fileName, result.Report);
            throw new GameFileErrorException(LastFailure!);
        }
        Record(files, fileName, result.Report);
    }

    /// <summary>
    /// The leading bytes of a listed file's engine image, for header listings such as
    /// THSEngine.GetHSSetFiles. An original lists its own leading bytes as before. An own
    /// document lists the header of its validated image. A file that fails lists nothing.
    /// </summary>
    public static byte[] ListingHeader(string fileName, GameProfile profile, int count)
    {
        var files = RtlUnit.Files;
        byte[] head;
        try
        {
            using var stream = files.Open(fileName, RtlUnit.fmOpenRead | RtlUnit.fmShareDenyWrite);
            head = new byte[count];
            int total = 0, read;
            while (total < count && (read = stream.Read(head, total, count - total)) > 0) total += read;
            head = head[..total];
        }
        catch (Exception e) when (e is IOException or UnauthorizedAccessException or ArgumentException)
        {
            return [];
        }
        if (!GameFiles.IsGzip(head) && !GameFiles.IsJsonCandidate(head))
        {
            // An original lists its leading bytes without a preflight. The open is logged like
            // every other one.
            Record(files, fileName, new GameFileReport
            {
                Operation = "list",
                Profile = profile,
                Format = DetectedFormat.Original.ToString(),
                OriginalRoundTrip = "not applicable: header listing",
                Validation = "not applicable: header listing",
            });
            return head.Length == count ? head : [];
        }
        var image = Load(fileName, profile, "list");
        return image is not null && image.Length >= count ? image[..count] : [];
    }

    /// <summary>
    /// The CRC forms of in-scope files (own-formats.md 4.1): Engine.GetCRC32 over the engine image,
    /// so an original and its own document give one CRC. A refused file gives 0, which matches
    /// no stored CRC, and is logged.
    /// </summary>
    public static int ImageCrc32(string fileName, GameProfile profile) =>
        Load(fileName, profile, "crc") is { } image ? EngineServices.GetCrc32(image, image.Length) : 0;

    /// <summary>
    /// The four bytes at the end of the engine image, the stored CRC of a scenario set that
    /// TAoWHSSet.RetrieveHSSCRC reads. A refusal raises, as the original's failed open did.
    /// </summary>
    public static int ImageTailInt32(string fileName, GameProfile profile)
    {
        // A scenario set image passed its preflight, so it holds its header and trailing CRC.
        var image = Load(fileName, profile, "crc") ?? throw new GameFileErrorException(LastFailure!);
        return BinaryPrimitives.ReadInt32LittleEndian(image.AsSpan(image.Length - 4));
    }

    /// <summary>
    /// The name a game write takes (own-formats.md 4.2). A name with an original extension takes
    /// its kind's own extension, so an original file of that name stays untouched beside the new
    /// document. A file under EmailIn keeps its name, since the play-by-e-mail exchange names its
    /// game files with the original extension. A host that writes original extensions
    /// (IRtlHost.WritesOwnExtensions) keeps every name.
    /// </summary>
    public static string WriteName(string fileName)
    {
        return WriteName(fileName, "");
    }

    /// <summary>WriteName for a name relative to a directory the caller knows, so the EmailIn
    /// decision does not rest on the current directory. The result stays relative.</summary>
    public static string WriteName(string fileName, string directory)
    {
        if (!RtlUnit.WritesOwnExtensions) return fileName;
        var kind = DocumentKind.ForOriginalExtension(RtlUnit.ExtractFileExt(fileName));
        if (kind is null || IsEmailExchange(directory + fileName)) return fileName;
        return RtlUnit.ChangeFileExt(fileName, kind.OwnExtension);
    }

    /// <summary>
    /// The extension a game write of a kind takes: the own one, or the original one where the host
    /// writes original extensions (IRtlHost.WritesOwnExtensions).
    /// </summary>
    public static string WriteExtension(DocumentKind kind) => RtlUnit.WritesOwnExtensions ? kind.OwnExtension : kind.OriginalExtension;

    /// <summary>
    /// The file a read of a name the game derives opens (own-formats.md 2.9). A name with an
    /// original extension opens its own sibling when that exists, else the name itself. A name
    /// with an own extension that does not exist opens its original sibling when that exists. The
    /// content still decides the format.
    /// </summary>
    public static string ReadName(string fileName)
    {
        string extension = RtlUnit.ExtractFileExt(fileName);
        var kind = DocumentKind.ForExtension(extension);
        if (kind is null) return fileName;
        if (kind.IsOwnExtension(extension))
        {
            if (RtlUnit.FileExists(fileName)) return fileName;
            string original = RtlUnit.ChangeFileExt(fileName, kind.OriginalExtension);
            return RtlUnit.FileExists(original) ? original : fileName;
        }
        string own = RtlUnit.ChangeFileExt(fileName, kind.OwnExtension);
        return RtlUnit.FileExists(own) ? own : fileName;
    }

    /// <summary>Whether a file lies directly in an EmailIn directory.</summary>
    public static bool IsEmailExchange(string fileName)
    {
        string directory = RtlUnit.ExtractFileDir(RtlUnit.ExpandFileName(fileName));
        return RtlUnit.CompareText(RtlUnit.ExtractFileName(directory), "EmailIn") == 0;
    }

    /// <summary>
    /// The names of the documents of a kind in a directory, for a listing that loads each one
    /// (the user hero and item lists). Originals come first in FindEntries order, then own
    /// documents. A name present in both forms lists its own document only, in the original's
    /// place, so a list saved again by the port loads once.
    /// </summary>
    public static List<string> FindDocuments(string directory, DocumentKind kind)
    {
        var originals = FindNames(directory + "*" + kind.OriginalExtension);
        var owns = FindNames(directory + "*" + kind.OwnExtension);
        var result = new List<string>();
        foreach (var name in originals)
        {
            string own = RtlUnit.ChangeFileExt(name, kind.OwnExtension);
            int index = owns.FindIndex(o => RtlUnit.CompareText(o, own) == 0);
            if (index < 0)
            {
                result.Add(name);
                continue;
            }
            result.Add(owns[index]);
            owns.RemoveAt(index);
        }
        result.AddRange(owns);
        return result;
    }

    private static List<string> FindNames(string mask)
    {
        var names = new List<string>();
        foreach (var entry in RtlUnit.Files.FindEntries(mask, attributes: 63 & ~16)) names.Add(entry.Name);
        return names;
    }

    /// <summary>The temporary of a replacement: a sibling the game never opens.</summary>
    public static string TemporaryFor(string fileName) => fileName + ".openaow-tmp";

    /// <summary>
    /// Writes a sibling temporary, closes it and replaces the target atomically. The target is
    /// never truncated. A failure removes the temporary and keeps the old target.
    /// </summary>
    public static void Replace(IFileSystem files, string fileName, byte[] bytes)
    {
        string temporary = TemporaryFor(fileName);
        bool replaced = false;
        try
        {
            using (var stream = files.Open(temporary, RtlUnit.fmCreate))
            {
                stream.Write(bytes);
                // The temporary reaches the disk before it replaces the old file.
                if (stream is FileStream file) file.Flush(true);
                else stream.Flush();
            }
            if (!files.ReplaceFile(temporary, fileName))
                throw new IOException("The file service did not replace " + fileName + ".");
            replaced = true;
        }
        finally
        {
            if (!replaced && files.FileExists(temporary)) files.DeleteFile(temporary);
        }
    }

    private static byte[] ReadAll(IFileSystem files, string fileName)
    {
        using var stream = files.Open(fileName, RtlUnit.fmOpenRead | RtlUnit.fmShareDenyWrite);
        using var memory = new MemoryStream();
        stream.CopyTo(memory);
        return memory.ToArray();
    }

    private static string LayerOf(IFileSystem files, string fileName)
    {
        try
        {
            return files.Layer(fileName);
        }
        catch (Exception e) when (e is IOException or UnauthorizedAccessException or ArgumentException)
        {
            return "unresolved";
        }
    }

    private static void Record(IFileSystem files, string fileName, GameFileReport report) =>
        Record(new FileLogRecord(fileName, LayerOf(files, fileName), report));

    private static void Record(FileLogRecord record)
    {
        if (!record.Succeeded) LastFailure = record;
        Log?.Invoke(record);
    }
}
