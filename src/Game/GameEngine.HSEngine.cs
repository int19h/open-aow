#nullable disable warnings
// Translated from HSEngine.pas.
using System.Runtime.InteropServices;
using Aow.Engine;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats;
using Aow.Formats.Tree;

using Aow.Game.Saves;

using Aow.Game.World.Map;

using Aow.Rtl;
using Aow.Rtl.Host;
using static Aow.Rtl.DelphiPaths;
using static Aow.Rtl.DelphiTextCompare;
using static Aow.Rtl.RtlHost;

namespace Aow.Game;

// _status, _hexSpriteSet, _hexSpriteMap, _hexSpriteSetSearchPath, _hssFileName, HexSpriteMapLoaded, HexSpriteMapClosing,
// FOnHSSLoaded, FOnHSSClose. The engine of the hex map: it owns the set (.hss: a 52-byte
// header, the stored set and a CRC32) and the map (.hsm: a 16-byte header and the stored map,
// compressed). the event lists are told when a map or a set is loaded or closed
public partial class GameEngine : OwningNode
{
    private GameMap? _hexSpriteMap;

    internal event NotifyEvent<GameMap>? HexSpriteMapLoaded;

    internal event NotifyEvent<GameMap>? HexSpriteMapClosing;

    internal event NotifyEvent<GameEngine>? HexSpriteSetLoaded;

    internal event NotifyEvent<GameEngine>? HexSpriteSetClosing;

    // HSEngine.pas:5210
    // The preflight of the file boundary finishes before the current map is freed. A refused
    // file leaves the current game and the file as they were (own-formats.md 2.7 and 4.1). The map
    // loads through the tree of the validated document, with the editor's error handler when it
    // loads for editing (persistence-tree.md 7). A player-facing load passes a list for the objects
    // the load goes on without (issue 384).
    public virtual void LoadHsm(string fileName, bool edit, ProgressEvent? progress, GameProfile profile, ICollection<DroppedObject>? dropped = null)
    {
        GameFileTree? tree = FileBoundary.Open(fileName, profile: profile);
        if (tree == null)
        {
            return;
        }

        if (_hexSpriteMap is not null)
        {
            _hexSpriteMap?.Dispose();
            SetHexSpriteMap(null);
        }

        try
        {
            SetHexSpriteMap(CreateEngineObject<GameMap>(tree.ClassId, owner: this));
            string expandFileName = ExpandFileName(fileName);
            _hexSpriteMap.FileName = expandFileName;
            Status |= HexSpriteEngineStatuses.LoadingHsm;
            if (edit)
            {
                _hexSpriteMap.Status |= HexSpriteMapStatuses.Editting;
            }

            try
            {
                _hexSpriteMap.SetProgressHandler(progress);
                ReadTree(tree, objectValue: _hexSpriteMap, progress: progress, onError: EditorErrorHandler(_hexSpriteMap), dropped: dropped);
                ActivateEngineObject(_hexSpriteMap);
                _hexSpriteMap.SetProgressHandler(null);
                HexSpriteMapLoaded?.Invoke(_hexSpriteMap);
            }
            catch
            {
                _hexSpriteMap?.Dispose();
                SetHexSpriteMap(null);
            }

            Status &= ~HexSpriteEngineStatuses.LoadingHsm;
        }
        catch (Exception error)
        {
            HostLog.FallbackAtCallSite(Logger, error: error);
        }
    }

    // TAoWHSMap.ReadWrite assigns OnStreamError as the stream's error handler when the map loads
    // for editing. The tree load takes it as the load's handler (persistence-tree.md 7). Another
    // load has none, so a failed nested object is dropped and the load goes on.
    private static LoadErrorHandler? EditorErrorHandler(GameMap map)
    {
        return map is not StrategicMap strategicMap || !strategicMap.Status.Contains(HexSpriteMapStatuses.Editting)
            ? null
            : ((string message, out bool continueLoad) =>
        {
            bool handled = true;
            strategicMap.OnStreamError(message: message, handled: ref handled);
            continueLoad = handled;
        });
    }

    // HSEngine.pas:5137
    public virtual GameMap? CreatePreviewHsm(string fileName, GameProfile profile)
    {
        GameMap? result = null;
        string fullName = ExpandFileName(fileName);
        GameFileTree? tree = FileBoundary.Open(fullName, profile: profile, operation: "preview");
        if (tree == null)
        {
            return null;
        }

        try
        {
            result = CreateEngineObject<GameMap>(tree.ClassId, owner: this);
            result.FileName = ExpandFileName(fileName);
            Status |= HexSpriteEngineStatuses.LoadingHsm;
            try
            {
                ReadTree(tree, objectValue: result, progress: null, preview: true);
            }
            catch
            {
                result?.Dispose();
                result = null;
            }

            Status &= ~HexSpriteEngineStatuses.LoadingHsm;
        }
        catch (Exception error)
        {
            HostLog.FallbackAtCallSite(Logger, error: error);
        }

        return result;
    }

    // The file stores the previewed map. A file that stores another map class throws, the same way
    // the call site did.
    public T CreatePreviewHsm<T>(string fileName, GameProfile profile) where T : GameMap => (T)CreatePreviewHsm(fileName, profile: profile);

    // HSEngine.pas:5291
    // The tree of the map goes to the file boundary, which writes the document, so a failed
    // conversion or write keeps the old file (own-formats.md 4.2). The header's class is the
    // stored class of the map.
    public virtual void SaveHsm(string fileName, ProgressEvent? progress, GameProfile profile)
    {
        if (_hexSpriteMap is not null)
        {
            Status |= HexSpriteEngineStatuses.SavingHsm;
            try
            {
                FileBoundary.Save(fileName, profile: profile, root: _hexSpriteMap, progress: progress);
            }
            finally
            {
                Status &= ~HexSpriteEngineStatuses.SavingHsm;
            }
        }
    }

    // HSEngine.pas:5093
    public virtual void CloseHss()
    {
        NotifyEvent<GameEngine>? handlers = HexSpriteSetClosing;
        EventChecks.AtMostOneHandler(handlers, owner: this, eventName: nameof(HexSpriteSetClosing));
        handlers?.Invoke(this);
        GamePack.Clear();
        HssFileName = "noname.hss";
    }

    // HSEngine.pas:4967
    public DelphiStringList GetHexSpriteSetFiles(string mask)
    {
        DelphiStringList files;
        void Search(string directory)
        {
            foreach (RtlFileEntry entry in Files.FindEntries(directory + mask, attributes: 0b0011_1111 & ~0b0001_0000))
            {
                HssHeader header = default;
                // An own scenario set lists the 52-byte header of its validated image, or the header
                // taken from the document when that image cannot carry the set (own-formats.md 4.1).
                byte[] head = FileBoundary.ListingHeader(directory + entry.Name, profile: GameProfile.ScenarioSet, count: 52);
                if (head.Length == 52)
                {
                    header = MemoryMarshal.Read<HssHeader>(head);
                }

                if (header.Id == "HSS\0")
                {
                    _ = files.Add($"{header.Name}|{directory}{entry.Name}");
                }
            }

            DelphiStringList directories = [];
            foreach (RtlFileEntry entry in Files.FindEntries($"{directory}*.*", attributes: 16))
            {
                if ((entry.Attribute & 0b0001_0000) != 0 && entry.Name != "." && entry.Name != "..")
                {
                    _ = directories.Add($"{directory}{entry.Name}\\");
                }
            }

            for (int i = 0, last = directories.Count - 1; i <= last; i++)
            {
                Search(directories[i]);
            }
        }

        files = [];
        Search(StartupDirectory);
        return files;
    }

    // HSEngine.pas:5024
    // The preflight raises before the current set is cleared (own-formats.md 2.7). It checked the
    // header and the CRC of an original, so the set loads through the tree of the validated
    // document.
    public void LoadHss(string fileName, bool edit, ProgressEvent? progress, GameProfile profile)
    {
        GameFileTree tree = FileBoundary.OpenOrRaise(ExpandFileName(fileName), profile: profile);
        GamePack.Clear();
        HssFileName = ExpandFileName(fileName);
        Status |= HexSpriteEngineStatuses.LoadingHss;
        try
        {
            ReadTree(tree, objectValue: GamePack, progress: progress);
            ActivateEngineObject(GamePack);
            EngineMessage.InitializeMessage(out EngineMessage message, messageId: 65556, sender: this);
            GamePack.MessageProc(ref message, sender: this);
            ControlNode.MessageProc(ref message, sender: this);
            NotifyEvent<GameEngine>? handlers = HexSpriteSetLoaded;
            EventChecks.AtMostOneHandler(handlers, owner: this, eventName: nameof(HexSpriteSetLoaded));
            handlers?.Invoke(this);
        }
        finally
        {
            Status &= ~HexSpriteEngineStatuses.LoadingHss;
        }
    }

    // HSEngine.pas:5060
    // The header names the set as the original header does, a short string of at most 40
    // characters. The file boundary writes the header and the tree of the set.
    public void SaveHss(string fileName, ProgressEvent? progress, GameProfile profile)
    {
        HssHeader header = default;
        if (GamePack is not null)
        {
            header.Name = GamePack.Name;
            HssFileName = ExpandFileName(fileName);
            Status |= HexSpriteEngineStatuses.SavingHss;
            try
            {
                FileBoundary.Save(HssFileName, profile: profile, root: GamePack, progress: progress, headerName: header.Name);
            }
            finally
            {
                Status &= ~HexSpriteEngineStatuses.SavingHss;
            }
        }
    }

    // HSEngine.pas:5100
    public void LoadPreviewHsm(string fileName, GameProfile profile)
    {
        GameFileTree? tree = FileBoundary.Open(fileName, profile: profile, operation: "preview");
        if (tree == null)
        {
            return;
        }

        if (_hexSpriteMap is not null)
        {
            _hexSpriteMap?.Dispose();
            SetHexSpriteMap(null);
        }

        try
        {
            SetHexSpriteMap(CreateEngineObject<GameMap>(tree.ClassId, owner: this));
            _hexSpriteMap.Status |= HexSpriteMapStatuses.PreviewMode;
            string expandFileName = ExpandFileName(fileName);
            _hexSpriteMap.FileName = expandFileName;
            Status |= HexSpriteEngineStatuses.LoadingHsm;
            try
            {
                ReadTree(tree, objectValue: _hexSpriteMap, progress: null);
            }
            catch
            {
                _hexSpriteMap?.Dispose();
                SetHexSpriteMap(null);
            }

            Status &= ~HexSpriteEngineStatuses.LoadingHsm;
        }
        catch (Exception error)
        {
            HostLog.FallbackAtCallSite(Logger, error: error);
        }
    }

    // HSEngine.pas:5171
    public GameMap? LoadHsmMap(string fileName, bool edit, ProgressEvent? progress, GameProfile profile)
    {
        GameMap? result = null;
        string fullName = ExpandFileName(fileName);
        GameFileTree? tree = FileBoundary.Open(fullName, profile: profile);
        if (tree == null)
        {
            return null;
        }

        try
        {
            result = CreateEngineObject<GameMap>(tree.ClassId, owner: null);
            result.FileName = fullName;
            Status |= HexSpriteEngineStatuses.LoadingHsm;
            if (edit)
            {
                result.Status |= HexSpriteMapStatuses.Editting;
            }

            try
            {
                result.SetProgressHandler(progress);
                ReadTree(tree, objectValue: result, progress: progress, onError: EditorErrorHandler(result));
                ActivateEngineObject(result);
                result.SetProgressHandler(null);
                HexSpriteMapLoaded?.Invoke(result);
            }
            catch
            {
                result?.Dispose();
                result = null;
            }

            Status &= ~HexSpriteEngineStatuses.LoadingHsm;
        }
        catch (Exception error)
        {
            HostLog.FallbackAtCallSite(Logger, error: error);
        }

        return result;
    }

    // The file stores the map. A file that stores another map class throws, the same way the call site did.
    public T LoadHsmMap<T>(string fileName, bool edit, ProgressEvent? progress, GameProfile profile)
        where T : GameMap
        => (T)LoadHsmMap(fileName, edit: edit, progress: progress, profile: profile);

    // HSEngine.pas:5251
    // The stream holds an original map file (a campaign's map, a map a network host sent) or the
    // map's own document. An image inflates and the original backend reads its root. A document
    // is read through the document backend, which keeps port-only state. A network map is an image.
    // A player-facing load passes a list for the objects the load goes on without (issue 384).
    public void LoadHsmFromStream(Stream stream, bool edit, ProgressEvent? progress, ICollection<DroppedObject>? dropped = null)
    {
        if (_hexSpriteMap is not null)
        {
            _hexSpriteMap?.Dispose();
            SetHexSpriteMap(null);
        }

        try
        {
            CompressStream cStream = new();
            try
            {
                cStream.LoadFromStream(stream);
                byte[] payload = cStream.ToArray();
                GameFileTree tree = IsOwnMapDocument(payload)
                    ? ReadOwnMap(payload)
                    : GameBoundary.ReadImageTree(payload, use: GameProfile.WorldMap);
                SetHexSpriteMap(CreateEngineObject<GameMap>(tree.ClassId, owner: this));
                Status |= HexSpriteEngineStatuses.LoadingHsm;
                if (edit)
                {
                    _hexSpriteMap.Status |= HexSpriteMapStatuses.Editting;
                }

                try
                {
                    _hexSpriteMap.SetProgressHandler(progress);
                    ReadTree(tree, objectValue: _hexSpriteMap, progress: progress, onError: EditorErrorHandler(_hexSpriteMap), dropped: dropped);
                    ActivateEngineObject(_hexSpriteMap);
                    _hexSpriteMap.SetProgressHandler(null);
                    HexSpriteMapLoaded?.Invoke(_hexSpriteMap);
                }
                catch
                {
                    _hexSpriteMap?.Dispose();
                    SetHexSpriteMap(null);
                }

                Status &= ~HexSpriteEngineStatuses.LoadingHsm;
            }
            finally
            {
                cStream?.Dispose();
            }
        }
        catch (Exception error)
        {
            HostLog.FallbackAtCallSite(Logger, error: error);
        }
    }

    // HSEngine.pas:5316
    // A map the original image can carry is written and compressed as before. A map it cannot
    // carry is stored as that map's own document, uncompressed, so the campaign document nests
    // the map and keeps the port-only members.
    public void SaveHsmToStream(Stream stream, ProgressEvent? progress)
    {
        if (_hexSpriteMap is null)
        {
            return;
        }

        Status |= HexSpriteEngineStatuses.SavingHsm;
        try
        {
            ObjectNode tree = TreeWriter.Write(_hexSpriteMap, factory: new EnginePersistableFactory(this, progress: progress));
            string className = NameTable.Stored(_hexSpriteMap.GetType());
            byte[] image;
            try
            {
                image = GameBoundary.WriteImage(tree, className: className, use: GameProfile.WorldMap);
            }
            catch (ConversionException error) when (GameBoundary.LacksImage(error))
            {
                GameFileResult prepared = GameBoundary.PrepareSave(tree, className: className, fileName: null, use: GameProfile.WorldMap, validators: FormatMapping.Validators);
                if (!prepared.Succeeded || prepared.Bytes is null)
                {
                    throw new ConversionException(prepared.Report.Error ?? error.Diagnostic);
                }

                stream.Write(prepared.Bytes);
                return;
            }

            using CompressStream cStream = new();
            cStream.Write(image);
            cStream.SaveToStream(stream);
        }
        finally
        {
            Status &= ~HexSpriteEngineStatuses.SavingHsm;
        }
    }

    // An own document is not an HSM image. CompressStream leaves those bytes unchanged.
    private static bool IsOwnMapDocument(byte[] payload)
        => GameFiles.IsGzip(payload) || GameFiles.IsJsonCandidate(payload);

    private static GameFileTree ReadOwnMap(byte[] payload)
    {
        GameFileResult prepared = GameBoundary.PrepareLoad(payload, fileName: null, use: GameProfile.WorldMap, validators: FormatMapping.Validators);
        return !prepared.Succeeded
            ? throw new ConversionException(prepared.Report.Error ?? new FormatDiagnostic(Codes.Format, Message: "The campaign map document was refused."))
            : GameBoundary.ReadTree(prepared, use: GameProfile.WorldMap);
    }

    // HSEngine.pas:5341
    // Every caller derives the name (RELEASE.HSS, the set a map names), so the own sibling opens
    // first (own-formats.md 2.9).
    public void InitializeHexSpriteSet(string fileName, ProgressEvent? progress)
    {
        fileName = FileBoundary.ReadName(fileName);
        if (GamePack is null || CompareText(fileName, HssFileName) != 0)
        {
            LoadHss(fileName, edit: false, progress, profile: GameProfile.ScenarioSet);
        }
        else
        {
            EngineMessage.InitializeMessage(out EngineMessage message, messageId: 65557, sender: this);
            GamePack.MessageProc(ref message, sender: this);
        }
    }

    // HSEngine.pas:5354
    public GameMap NewHsm(int classId)
    {
        if (_hexSpriteMap is not null)
        {
            _hexSpriteMap?.Dispose();
            SetHexSpriteMap(null);
        }

        SetHexSpriteMap(CreateEngineObject<GameMap>(classId, owner: this));
        _hexSpriteMap.Status |= HexSpriteMapStatuses.Editting;
        ActivateEngineObject(_hexSpriteMap);
        HexSpriteMapLoaded?.Invoke(_hexSpriteMap);
        return _hexSpriteMap;
    }

    public HexSpriteEngineStatuses Status { get; private set; }
    public GameMap? HexSpriteMap { get => _hexSpriteMap; set => SetHexSpriteMap(value); }

    internal void SetHexSpriteMapWithoutCurrentMap(GameMap? value) => _hexSpriteMap = value;

    public string HexSpriteSetSearchPath { get; } = "";
    public string HssFileName { get; private set; } = "";
}
