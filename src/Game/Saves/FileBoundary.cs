#nullable enable
using System.Buffers.Binary;

using System.Security.Cryptography;

using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats;
using Aow.Formats.Json;
using Aow.Formats.Original;
using Aow.Formats.Schema;
using Aow.Formats.Tree;
using Aow.Rtl;
using Aow.Rtl.Host;

namespace Aow.Game.Saves;

/// <summary>
/// The game's file boundary for the in-scope profiles (own-formats.md 4.1 and 4.2). Every
/// in-scope engine routine reads and writes through it with an explicit profile. Reads go
/// through the host's file service and run the whole preflight before the engine touches the
/// current game, and a load reads the tree of the validated document. Writes write the tree,
/// validate and serialize it, write a sibling temporary through the file service and replace the
/// old file atomically.
/// </summary>
public static class FileBoundary
{
    /// <summary>The host's file log (--file-log). Null records nothing.</summary>
    public static Action<FileLogRecord>? Log { get; set; }

    /// <summary>The last refused open or write, for the caller that reports it.</summary>
    public static FileLogRecord? LastFailure { get; private set; }

    /// <summary>
    /// The engine image of a validated file. A refusal returns null. A document the original
    /// image cannot carry has no image, and this returns null rather than the file bytes. The
    /// current game and the file stay untouched.
    /// </summary>
    public static byte[]? Load(string fileName, GameProfile profile, string operation = "open") =>
        Prepare(fileName, profile: profile, operation: operation) is { Image: { } image } ? image : null;

    /// <summary>
    /// Reads a file and prepares the tree the engine loads (own-formats.md 4.1, persistence-tree.md
    /// 6): an own document through the document backend, an original through the original backend
    /// over the image its G1 check passed. A refusal returns null, is logged and leaves the current
    /// game and the file untouched.
    /// </summary>
    public static GameFileTree? Open(string fileName, GameProfile profile, string operation = "open")
    {
        if (Prepare(fileName, profile: profile, operation: operation) is not { } prepared)
        {
            return null;
        }

        try
        {
            return prepared.Result is { } result
                ? GameBoundary.ReadTree(result, use: profile)
                : prepared.Image is { } image ? GameBoundary.ReadImageTree(image, use: profile) : null;
        }
        catch (ConversionException error)
        {
            GameFileReport report = prepared.Report;
            report.Error = error.Diagnostic;
            report.Validation = "refused";
            Record(RtlHost.Files, fileName: fileName, report: report);
            return null;
        }
    }

    /// <summary>
    /// Like Open, for the routines whose original raised on a failed open: the refusal raises
    /// EGameFileError after it is logged, before the caller changes anything.
    /// </summary>
    public static GameFileTree OpenOrRaise(string fileName, GameProfile profile) =>
        Open(fileName, profile) ?? throw new GameFileErrorException(LastFailure!);

    // One preflight. Image is the engine image, or null when the validated document has none.
    // It is never the own document's file bytes. A cached preview has an image and no result.
    private sealed record Prepared(byte[]? Image, GameFileReport Report, GameFileResult? Result);

    private static Prepared? Prepare(string fileName, GameProfile profile, string operation)
    {
        IFileSystem files = RtlHost.Files;
        byte[] bytes;
        try
        {
            bytes = ReadAll(files, fileName);
        }
        catch (Exception e) when (e is IOException or UnauthorizedAccessException or ArgumentException)
        {
            Record(report: new GameFileReport { Operation = operation, Profile = profile, Error = new FormatDiagnostic(Codes.Output, "The file cannot be read: " + e.Message) { File = fileName } }, files: files, fileName: fileName);
            return null;
        }
        string layer = LayerOf(files, fileName);
        // Previews and listings reuse a validated image of the same bytes (own-formats.md 4.3).
        // A final open always validates the actual bytes. A document the original image cannot
        // carry has no image, so a preview of it is not cached.
        bool preview = operation != "open";
        PreviewKey key = preview ? new PreviewKey(layer, fileName, profile.Name, OwnFormat.CurrentVersion, Convert.ToHexStringLower(SHA256.HashData(bytes))) : default;
        if (preview && Previews.TryGet(key, out PreviewEntry? cached))
        {
            Record(new FileLogRecord(fileName, layer, cached.Report, cached: true));
            return new Prepared(cached.Image, Report: cached.Report, Result: null);
        }
        GameFileResult result = GameBoundary.PrepareLoad(bytes, fileName, profile, FormatMapping.Validators, operation);
        Record(new FileLogRecord(fileName, layer, result.Report));
        if (!result.Succeeded)
        {
            return null;
        }
        // HasImage is the only way Bytes is an engine image. A save's document bytes and a
        // port-only load never land in Image.
        byte[]? image = result.HasImage ? result.Bytes : null;
        if (preview && image is not null)
        {
            Previews.Add(key, new PreviewEntry(image, result.Report));
        }

        return new Prepared(image, Report: result.Report, Result: result);
    }

    /// <summary>The session preview cache. Only successful preflights enter it.</summary>
    public static PreviewCache Previews { get; } = new(64L << 20);

    /// <summary>
    /// The engine image of a file whose original raised on a failed open. A refusal raises
    /// EGameFileError after it is logged. A document the original image cannot carry raises the
    /// packet profile's refusal at the member path and returns no bytes.
    /// </summary>
    public static byte[] LoadOrRaise(string fileName, GameProfile profile)
    {
        if (Prepare(fileName, profile: profile, operation: "open") is not { } prepared)
        {
            throw new GameFileErrorException(LastFailure!);
        }

        if (prepared.Image is { } image)
        {
            return image;
        }

        RefuseThroughPacket(prepared, profile);
        throw new InvalidOperationException("A document without an engine image produced a packet.");
    }

    /// <summary>
    /// Writes an object through the tree (own-formats.md 4.2, persistence-tree.md 6): the tree
    /// writer, the document backend, the validation, the serialization and the atomic replacement.
    /// Everything before the replacement finishes before anything is written. A failure raises
    /// EGameFileError and keeps the old file. <paramref name="headerName"/> is the name a scenario
    /// set stores in its header.
    /// </summary>
    public static void Save(string fileName, GameProfile profile, EngineObject root, ProgressEvent? progress = null, string? headerName = null)
    {
        IFileSystem files = RtlHost.Files;
        GameFileResult result;
        try
        {
            result = GameBoundary.PrepareSave(TreeWriter.Write(root, factory: new EnginePersistableFactory(root.GetEngine(), progress: progress)), className: NameTable.Stored(root.GetType()), fileName: fileName, use: profile, validators: FormatMapping.Validators, headerName: headerName);
        }
        catch (ConversionException error)
        {
            result = new GameFileResult
            {
                Report = new GameFileReport { Operation = "write", Profile = profile, Kind = profile.WriteKind, Format = DetectedFormat.OwnJson.ToString(), Error = error.Diagnostic },
            };
        }
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
    /// document with an image lists that image's header. A document the image cannot carry
    /// lists the header taken from the validated document. A file that fails lists nothing.
    /// </summary>
    public static byte[] ListingHeader(string fileName, GameProfile profile, int count)
    {
        IFileSystem files = RtlHost.Files;
        byte[] head;
        try
        {
            using Stream stream = files.Open(fileName, DelphiStreams.fmOpenRead | DelphiStreams.fmShareDenyWrite);
            head = new byte[count];
            int total = 0, read;
            while (total < count && (read = stream.Read(head, total, count - total)) > 0)
            {
                total += read;
            }

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
        if (Prepare(fileName, profile: profile, operation: "list") is not { } prepared)
        {
            return [];
        }

        if (prepared.Image is { } image)
        {
            return image.Length >= count ? image[..count] : [];
        }

        if (prepared.Result?.Document is not JsonObject document)
        {
            return [];
        }

        byte[] header = ListingHeaderFromDocument(document, profile);
        return header.Length >= count ? header[..count] : [];
    }

    /// <summary>
    /// The CRC forms of in-scope files (own-formats.md 4.1). When the file has an engine image, the
    /// input is that image, so an original and its own document give one CRC. When the document has
    /// no engine image, the input is the canonical document bytes from <c>GameFiles.Write</c> of the
    /// validated document. A refused file gives 0, which matches no stored CRC, and is logged. A
    /// transfer of a document with no image is refused, so this value is never compared with a
    /// received image.
    /// </summary>
    public static int ImageCrc32(string fileName, GameProfile profile)
    {
        if (Prepare(fileName, profile: profile, operation: "crc") is not { } prepared)
        {
            return 0;
        }

        if (prepared.Image is { } image)
        {
            return EngineServices.GetCrc32(image, image.Length);
        }

        if (prepared.Result?.Document is not JsonObject document)
        {
            return 0;
        }

        byte[] text = GameFiles.Write(document);
        return EngineServices.GetCrc32(text, text.Length);
    }

    /// <summary>
    /// The four bytes at the end of the engine image, the stored CRC of a scenario set that
    /// TAoWHSSet.RetrieveHSSCRC reads. A refusal raises, as the original's failed open did. A
    /// document the original image cannot carry has no image and therefore no stored CRC. The
    /// call refuses through the packet profile and does not read the document bytes.
    /// </summary>
    public static int ImageTailInt32(string fileName, GameProfile profile)
    {
        if (Prepare(fileName, profile: profile, operation: "crc") is not { } prepared)
        {
            throw new GameFileErrorException(LastFailure!);
        }

        if (prepared.Image is not { } image)
        {
            RefuseThroughPacket(prepared, profile);
            throw new InvalidOperationException("A document without an engine image has no stored CRC.");
        }
        return BinaryPrimitives.ReadInt32LittleEndian(image.AsSpan(image.Length - 4));
    }

    // The packet profile names the member a transfer cannot carry. WritePacket throws, and the
    // caller sends nothing. A nil packet is not an image of the file.
    private static void RefuseThroughPacket(Prepared prepared, GameProfile profile)
    {
        GameFileTree tree = GameBoundary.ReadTree(prepared.Result ?? throw new InvalidOperationException("A document without an engine image has no prepared result."), use: profile);
        _ = GameBoundary.WritePacket(tree.Root, classId: tree.ClassId, className: tree.ClassName);
    }

    // The header a listing shows when the document has no engine image. A scenario set's 52
    // bytes are size, the HSS identifier, the short-string name from $container.header.Name and
    // zeros. An HSM's 16 bytes are size, the HSM magic, the reserved word and the root class id.
    // Neither form is a parse of the file bytes.
    private static byte[] ListingHeaderFromDocument(JsonObject document, GameProfile profile) => profile.Profile is ScenarioSetProfile
        ? ScenarioSetHeader(document)
        : profile.Profile is HsmProfile
        ? HsmHeader(document)
        : [];

    private static byte[] ScenarioSetHeader(JsonObject document)
    {
        if (document["$container"] is not JsonObject container)
        {
            return [];
        }

        if (container["header"] is not JsonObject header)
        {
            return [];
        }

        if (header["Name"] is not JsonString name)
        {
            return [];
        }

        byte[] encoded = Cp1252.Encode(name.Value);
        if (encoded.Length > 40)
        {
            return [];
        }

        byte[] head = new byte[52];
        BinaryPrimitives.WriteInt32LittleEndian(head, 52);
        head[4] = 4;
        "HSS\0"u8.CopyTo(head.AsSpan(5));
        head[9] = (byte)encoded.Length;
        encoded.CopyTo(head.AsSpan(10));
        return head;
    }

    private static byte[] HsmHeader(JsonObject document)
    {
        if (document["root"] is not JsonObject root)
        {
            return [];
        }

        if (root["$class"] is not JsonString className)
        {
            return [];
        }

        if (FormatSchema.Current.Find(className.Value)?.Id is not int classId)
        {
            return [];
        }

        int reserved = 0;
        if (document["$container"] is JsonObject container && container["header"] is JsonObject header && header["Reserved"] is JsonInteger word)
        {
            reserved = (int)word.Value;
        }

        byte[] head = new byte[16];
        BinaryPrimitives.WriteInt32LittleEndian(head, 16);
        HsmProfile.Magic.CopyTo(head.AsSpan(4));
        BinaryPrimitives.WriteInt32LittleEndian(head.AsSpan(8), reserved);
        BinaryPrimitives.WriteInt32LittleEndian(head.AsSpan(12), classId);
        return head;
    }

    /// <summary>
    /// The name a game write takes (own-formats.md 4.2). A name with an original extension takes
    /// its kind's own extension, so an original file of that name stays untouched beside the new
    /// document. A file under EmailIn keeps its name, since the play-by-e-mail exchange names its
    /// game files with the original extension. A host that writes original extensions
    /// (IRtlHost.WritesOwnExtensions) keeps every name.
    /// </summary>
    public static string WriteName(string fileName) => WriteName(fileName, "");

    /// <summary>WriteName for a name relative to a directory the caller knows, so the EmailIn
    /// decision does not rest on the current directory. The result stays relative.</summary>
    public static string WriteName(string fileName, string directory)
    {
        if (!RtlHost.WritesOwnExtensions)
        {
            return fileName;
        }

        DocumentKind? kind = DocumentKind.ForOriginalExtension(DelphiPaths.ExtractFileExt(fileName));
        return kind is null || IsEmailExchange(directory + fileName) ? fileName : DelphiPaths.ChangeFileExt(fileName, kind.OwnExtension);
    }

    /// <summary>
    /// The extension a game write of a kind takes: the own one, or the original one where the host
    /// writes original extensions (IRtlHost.WritesOwnExtensions).
    /// </summary>
    public static string WriteExtension(DocumentKind kind) => RtlHost.WritesOwnExtensions ? kind.OwnExtension : kind.OriginalExtension;

    /// <summary>
    /// The file a read of a name the game derives opens (own-formats.md 2.9). A name with an
    /// original extension opens its own sibling when that exists, else the name itself. A name
    /// with an own extension that does not exist opens its original sibling when that exists. The
    /// content still decides the format.
    /// </summary>
    public static string ReadName(string fileName)
    {
        string extension = DelphiPaths.ExtractFileExt(fileName);
        DocumentKind? kind = DocumentKind.ForExtension(extension);
        if (kind is null)
        {
            return fileName;
        }

        if (kind.IsOwnExtension(extension))
        {
            if (DelphiFiles.FileExists(fileName))
            {
                return fileName;
            }

            string original = DelphiPaths.ChangeFileExt(fileName, kind.OriginalExtension);
            return DelphiFiles.FileExists(original) ? original : fileName;
        }
        string own = DelphiPaths.ChangeFileExt(fileName, kind.OwnExtension);
        return DelphiFiles.FileExists(own) ? own : fileName;
    }

    /// <summary>Whether a file lies directly in an EmailIn directory.</summary>
    public static bool IsEmailExchange(string fileName) => DelphiTextCompare.CompareText(DelphiPaths.ExtractFileName(DelphiPaths.ExtractFileDir(DelphiPaths.ExpandFileName(fileName))), "EmailIn") == 0;

    /// <summary>
    /// The names of the documents of a kind in a directory, for a listing that loads each one
    /// (the user hero and item lists). Originals come first in FindEntries order, then own
    /// documents. A name present in both forms lists its own document only, in the original's
    /// place, so a list saved again by the port loads once.
    /// </summary>
    public static List<string> FindDocuments(string directory, DocumentKind kind)
    {
        List<string> originals = FindNames(directory + "*" + kind.OriginalExtension);
        List<string> owns = FindNames(directory + "*" + kind.OwnExtension);
        List<string> result = [];
        foreach (string name in originals)
        {
            string own = DelphiPaths.ChangeFileExt(name, kind.OwnExtension);
            int index = owns.FindIndex(o => DelphiTextCompare.CompareText(o, own) == 0);
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
        List<string> names = [];
        foreach (RtlFileEntry entry in RtlHost.Files.FindEntries(mask, attributes: 63 & ~16))
        {
            names.Add(entry.Name);
        }

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
            using (Stream stream = files.Open(temporary, DelphiStreams.fmCreate))
            {
                stream.Write(bytes);
                // The temporary reaches the disk before it replaces the old file.
                if (stream is FileStream file)
                {
                    file.Flush(true);
                }
                else
                {
                    stream.Flush();
                }
            }
            if (!files.ReplaceFile(temporary, fileName))
            {
                throw new IOException("The file service did not replace " + fileName + ".");
            }

            replaced = true;
        }
        finally
        {
            if (!replaced && files.FileExists(temporary))
            {
                _ = files.DeleteFile(temporary);
            }
        }
    }

    private static byte[] ReadAll(IFileSystem files, string fileName)
    {
        using Stream stream = files.Open(fileName, DelphiStreams.fmOpenRead | DelphiStreams.fmShareDenyWrite);
        using MemoryStream memory = new();
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
        if (!record.Succeeded)
        {
            LastFailure = record;
        }

        Log?.Invoke(record);
    }
}
