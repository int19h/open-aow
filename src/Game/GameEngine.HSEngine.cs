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
using static Aow.Rtl.RtlUnit;

namespace Aow.Game;

// _status, _hexSpriteSet, _hexSpriteMap, _hexSpriteSetSearchPath, _hssFileName, HexSpriteMapLoaded, HexSpriteMapClosing,
// FOnHSSLoaded, FOnHSSClose. The engine of the hex map: it owns the set (.hss: a 52-byte
// header, the stored set and a CRC32) and the map (.hsm: a 16-byte header and the stored map,
// compressed). the event lists are told when a map or a set is loaded or closed
public partial class GameEngine : ENode
{
    private Set<HexSpriteEngineStatusElement> _status;
    private GamePack _hexSpriteSet;
    private GameMap _hexSpriteMap;
    private string _hexSpriteSetSearchPath = "";
    private string _hssFileName = "";

    internal EventStore<GameMap> OnHsmLoadedStore { get; set; }

    internal event NotifyEvent<GameMap> HexSpriteMapLoaded
    {
        add => OnHsmLoadedStore.Add(value);
        remove => OnHsmLoadedStore.Remove(value);
    }

    internal EventStore<GameMap> OnHsmCloseStore { get; set; }

    internal event NotifyEvent<GameMap> HexSpriteMapClosing
    {
        add => OnHsmCloseStore.Add(value);
        remove => OnHsmCloseStore.Remove(value);
    }

    private readonly EventStore<GameEngine> _onHssLoadedStore;

    internal event NotifyEvent<GameEngine> HexSpriteSetLoaded
    {
        add => _onHssLoadedStore.Add(value);
        remove => _onHssLoadedStore.Remove(value);
    }

    private readonly EventStore<GameEngine> _onHssCloseStore;

    internal event NotifyEvent<GameEngine> HexSpriteSetClosing
    {
        add => _onHssCloseStore.Add(value);
        remove => _onHssCloseStore.Remove(value);
    }

    // HSEngine.pas:5210
    // The preflight of the file boundary finishes before the current map is freed. A refused
    // file leaves the current game and the file as they were (own-formats.md 2.7 and 4.1). The map
    // loads through the tree of the validated document, with the editor's error handler when it
    // loads for editing (persistence-tree.md 7).
    public virtual void LoadHsm(string fileName, bool edit, Aow.Engine.ProgressEvent progress, GameProfile profile)
    {
        var tree = FileBoundary.Open(fileName, profile: profile);
        if (tree == null)
        {
            return;
        }

        if (_hexSpriteMap != null)
        {
            _hexSpriteMap?.Dispose();
            SetHexSpriteMap(null);
        }

        try
        {
            SetHexSpriteMap((GameMap)CreateEngineObject(tree.ClassId, this));
            var expandFileName = ExpandFileName(fileName);
            _hexSpriteMap.FileName = expandFileName;
            _status = _status + Set<HexSpriteEngineStatusElement>.Of(HexSpriteEngineStatusElement.LoadingHsm);
            if (edit)
            {
                _hexSpriteMap.Status = _hexSpriteMap.Status + Set<HexSpriteMapStatusElement>.Of(HexSpriteMapStatusElement.Editting);
            }

            try
            {
                _hexSpriteMap.SetProgressHandler(progress);
                ReadTree(tree, objectValue: _hexSpriteMap, progress: progress, onError: EditorErrorHandler(_hexSpriteMap));
                ActivateEngineObject(_hexSpriteMap);
                _hexSpriteMap.ClearProgressHandler();
                OnHsmLoadedStore.Raise(_hexSpriteMap);
            }
            catch
            {
                _hexSpriteMap?.Dispose();
                SetHexSpriteMap(null);
            }

            _status = _status - Set<HexSpriteEngineStatusElement>.Of(HexSpriteEngineStatusElement.LoadingHsm);
        }
        catch
        {
        }
    }

    // TAoWHSMap.ReadWrite assigns OnStreamError as the stream's error handler when the map loads
    // for editing. The tree load takes it as the load's handler (persistence-tree.md 7). Another
    // load has none, so a failed nested object is dropped and the load goes on.
    private static LoadErrorHandler EditorErrorHandler(GameMap map)
    {
        if (map is not StrategicMap strategicMap || !strategicMap.Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            return null;
        }

        return (string message, out bool continueLoad) =>
        {
            bool handled = true;
            strategicMap.OnStreamError(stream: null, message: message, handled: ref handled);
            continueLoad = handled;
        };
    }

    // HSEngine.pas:5137
    public virtual GameMap CreatePreviewHsm(string fileName, GameProfile profile)
    {
        string fullName;
        GameMap result = null;
        fullName = ExpandFileName(fileName);
        var tree = FileBoundary.Open(fullName, profile: profile, operation: "preview");
        if (tree == null)
        {
            return null;
        }

        try
        {
            result = (GameMap)CreateEngineObject(tree.ClassId, this);
            result.FileName = ExpandFileName(fileName);
            _status = _status + Set<HexSpriteEngineStatusElement>.Of(HexSpriteEngineStatusElement.LoadingHsm);
            try
            {
                ReadTree(tree, objectValue: result, progress: null, preview: true);
            }
            catch
            {
                result?.Dispose();
                result = null;
            }

            _status = _status - Set<HexSpriteEngineStatusElement>.Of(HexSpriteEngineStatusElement.LoadingHsm);
        }
        catch
        {
        }

        return result;
    }

    // HSEngine.pas:5291
    // The tree of the map goes to the file boundary, which writes the document, so a failed
    // conversion or write keeps the old file (own-formats.md 4.2). The header's class is the
    // stored class of the map.
    public virtual void SaveHsm(string fileName, Aow.Engine.ProgressEvent progress, GameProfile profile)
    {
        if (_hexSpriteMap != null)
        {
            _status = _status + Set<HexSpriteEngineStatusElement>.Of(HexSpriteEngineStatusElement.SavingHsm);
            try
            {
                FileBoundary.Save(fileName, profile: profile, root: _hexSpriteMap, progress: progress);
            }
            finally
            {
                _status = _status - Set<HexSpriteEngineStatusElement>.Of(HexSpriteEngineStatusElement.SavingHsm);
            }
        }
    }

    // HSEngine.pas:5093
    public virtual void CloseHss()
    {
        _onHssCloseStore.Raise(this);
        _hexSpriteSet.Clear();
        _hssFileName = "noname.hss";
    }

    // HSEngine.pas:4967
    public StringList GetHexSpriteSetFiles(string mask)
    {
        StringList files;
        void Search(string directory)
        {
            StringList directories;
            HssHeader header = default;
            foreach (var entry in Files.FindEntries(directory + mask, attributes: 63 & ~16))
            {
                header = default;
                // An own scenario set lists the header of its validated image (own-formats.md 4.1).
                var head = FileBoundary.ListingHeader(directory + entry.Name, GameProfile.ScenarioSet, 52);
                if (head.Length == 52)
                {
                    header = MemoryMarshal.Read<HssHeader>(head);
                }

                if (header.Id == "HSS\0")
                {
                    files.Add(header.Name + "|" + directory + entry.Name);
                }
            }

            directories = new StringList();
            foreach (var entry in Files.FindEntries(directory + "*.*", attributes: 16))
            {
                if ((entry.Attribute & 16) != 0 && entry.Name != "." && entry.Name != "..")
                {
                    directories.Add(directory + entry.Name + "\\");
                }
            }

            for (int I = 0, last = directories.Count - 1; I <= last; I++)
            {
                Search(directories[I]);
            }

            directories?.Dispose();
        }

        files = new StringList();
        Search(StartupDirectory);
        return files;
    }

    // HSEngine.pas:5024
    // The preflight raises before the current set is cleared (own-formats.md 2.7). It checked the
    // header and the CRC of an original, so the set loads through the tree of the validated
    // document.
    public void LoadHss(string fileName, bool edit, Aow.Engine.ProgressEvent progress, GameProfile profile)
    {
        EngineMessage message = default;
        var tree = FileBoundary.OpenOrRaise(ExpandFileName(fileName), profile: profile);
        _hexSpriteSet.Clear();
        _hssFileName = ExpandFileName(fileName);
        _status = _status + Set<HexSpriteEngineStatusElement>.Of(HexSpriteEngineStatusElement.LoadingHss);
        try
        {
            ReadTree(tree, objectValue: _hexSpriteSet, progress: progress);
            ActivateEngineObject(_hexSpriteSet);
            EngineMessage.InitializeMessage(out message, 65556, this);
            _hexSpriteSet.MessageProc(ref message, this);
            ControlNode.MessageProc(ref message, this);
            _onHssLoadedStore.Raise(this);
        }
        finally
        {
            _status = _status - Set<HexSpriteEngineStatusElement>.Of(HexSpriteEngineStatusElement.LoadingHss);
        }
    }

    // HSEngine.pas:5060
    // The header names the set as the original header does, a short string of at most 40
    // characters. The file boundary writes the header and the tree of the set.
    public void SaveHss(string fileName, Aow.Engine.ProgressEvent progress, GameProfile profile)
    {
        HssHeader header = default;
        if (_hexSpriteSet != null)
        {
            header.Name = _hexSpriteSet.Name;
            _hssFileName = ExpandFileName(fileName);
            _status = _status + Set<HexSpriteEngineStatusElement>.Of(HexSpriteEngineStatusElement.SavingHss);
            try
            {
                FileBoundary.Save(_hssFileName, profile: profile, root: _hexSpriteSet, progress: progress, headerName: header.Name);
            }
            finally
            {
                _status = _status - Set<HexSpriteEngineStatusElement>.Of(HexSpriteEngineStatusElement.SavingHss);
            }
        }
    }

    // HSEngine.pas:5100
    public void LoadPreviewHsm(string fileName, GameProfile profile)
    {
        var tree = FileBoundary.Open(fileName, profile: profile, operation: "preview");
        if (tree == null)
        {
            return;
        }

        if (_hexSpriteMap != null)
        {
            _hexSpriteMap?.Dispose();
            SetHexSpriteMap(null);
        }

        try
        {
            SetHexSpriteMap((GameMap)CreateEngineObject(tree.ClassId, this));
            _hexSpriteMap.Status = _hexSpriteMap.Status + Set<HexSpriteMapStatusElement>.Of(HexSpriteMapStatusElement.PreviewMode);
            var expandFileName = ExpandFileName(fileName);
            _hexSpriteMap.FileName = expandFileName;
            _status = _status + Set<HexSpriteEngineStatusElement>.Of(HexSpriteEngineStatusElement.LoadingHsm);
            try
            {
                ReadTree(tree, objectValue: _hexSpriteMap, progress: null);
            }
            catch
            {
                _hexSpriteMap?.Dispose();
                SetHexSpriteMap(null);
            }

            _status = _status - Set<HexSpriteEngineStatusElement>.Of(HexSpriteEngineStatusElement.LoadingHsm);
        }
        catch
        {
        }
    }

    // HSEngine.pas:5171
    public GameMap LoadHsmMap(string fileName, bool edit, Aow.Engine.ProgressEvent progress, GameProfile profile)
    {
        string fullName;
        GameMap result = null;
        fullName = ExpandFileName(fileName);
        var tree = FileBoundary.Open(fullName, profile: profile);
        if (tree == null)
        {
            return null;
        }

        try
        {
            result = (GameMap)CreateEngineObject(tree.ClassId, null);
            result.FileName = fullName;
            _status = _status + Set<HexSpriteEngineStatusElement>.Of(HexSpriteEngineStatusElement.LoadingHsm);
            if (edit)
            {
                result.Status = result.Status + Set<HexSpriteMapStatusElement>.Of(HexSpriteMapStatusElement.Editting);
            }

            try
            {
                result.SetProgressHandler(progress);
                ReadTree(tree, objectValue: result, progress: progress, onError: EditorErrorHandler(result));
                ActivateEngineObject(result);
                result.ClearProgressHandler();
                OnHsmLoadedStore.Raise(result);
            }
            catch
            {
                result?.Dispose();
                result = null;
            }

            _status = _status - Set<HexSpriteEngineStatusElement>.Of(HexSpriteEngineStatusElement.LoadingHsm);
        }
        catch
        {
        }

        return result;
    }

    // HSEngine.pas:5251
    // The stream holds an original map file (a campaign's map, a map a network host sent). The
    // original backend reads its tree from the inflated image, and the map loads through the tree.
    public void LoadHsmFromStream(Stream stream, bool edit, Aow.Engine.ProgressEvent progress)
    {
        CompressStream cStream;
        if (_hexSpriteMap != null)
        {
            _hexSpriteMap?.Dispose();
            SetHexSpriteMap(null);
        }

        try
        {
            cStream = new CompressStream();
            try
            {
                cStream.LoadFromStream(stream);
                var tree = GameBoundary.ReadImageTree(cStream.ToArray(), use: GameProfile.WorldMap);
                SetHexSpriteMap((GameMap)CreateEngineObject(tree.ClassId, this));
                _status = _status + Set<HexSpriteEngineStatusElement>.Of(HexSpriteEngineStatusElement.LoadingHsm);
                if (edit)
                {
                    _hexSpriteMap.Status = _hexSpriteMap.Status + Set<HexSpriteMapStatusElement>.Of(HexSpriteMapStatusElement.Editting);
                }

                try
                {
                    _hexSpriteMap.SetProgressHandler(progress);
                    ReadTree(tree, objectValue: _hexSpriteMap, progress: progress, onError: EditorErrorHandler(_hexSpriteMap));
                    ActivateEngineObject(_hexSpriteMap);
                    _hexSpriteMap.ClearProgressHandler();
                    OnHsmLoadedStore.Raise(_hexSpriteMap);
                }
                catch
                {
                    _hexSpriteMap?.Dispose();
                    SetHexSpriteMap(null);
                }

                _status = _status - Set<HexSpriteEngineStatusElement>.Of(HexSpriteEngineStatusElement.LoadingHsm);
            }
            finally
            {
                cStream?.Dispose();
            }
        }
        catch
        {
        }
    }

    // HSEngine.pas:5316
    // The original backend writes the image of the map's tree, the 16-byte header and the root,
    // which the stream compresses as before.
    public void SaveHsmToStream(Stream stream, Aow.Engine.ProgressEvent progress)
    {
        CompressStream cStream;
        if (_hexSpriteMap != null)
        {
            cStream = new CompressStream();
            _status = _status + Set<HexSpriteEngineStatusElement>.Of(HexSpriteEngineStatusElement.SavingHsm);
            try
            {
                var tree = TreeWriter.Write(_hexSpriteMap, factory: new EnginePersistableFactory(this, progress: progress));
                cStream.Write(GameBoundary.WriteImage(tree, className: NameTable.Stored(_hexSpriteMap.GetType()), use: GameProfile.WorldMap));
                cStream.SaveToStream(stream);
            }
            finally
            {
                _status = _status - Set<HexSpriteEngineStatusElement>.Of(HexSpriteEngineStatusElement.SavingHsm);
                cStream?.Dispose();
            }
        }
    }

    // HSEngine.pas:5341
    // Every caller derives the name (RELEASE.HSS, the set a map names), so the own sibling opens
    // first (own-formats.md 2.9).
    public void InitializeHexSpriteSet(string fileName, Aow.Engine.ProgressEvent progress)
    {
        EngineMessage message = default;
        fileName = FileBoundary.ReadName(fileName);
        if (_hexSpriteSet == null || CompareText(fileName, _hssFileName) != 0)
        {
            LoadHss(fileName, false, progress, GameProfile.ScenarioSet);
        }
        else
        {
            EngineMessage.InitializeMessage(out message, 65557, this);
            _hexSpriteSet.MessageProc(ref message, this);
        }
    }

    // HSEngine.pas:5354
    public GameMap NewHsm(int classId)
    {
        if (_hexSpriteMap != null)
        {
            _hexSpriteMap?.Dispose();
            SetHexSpriteMap(null);
        }

        SetHexSpriteMap((GameMap)CreateEngineObject(classId, this));
        _hexSpriteMap.Status = _hexSpriteMap.Status + Set<HexSpriteMapStatusElement>.Of(HexSpriteMapStatusElement.Editting);
        ActivateEngineObject(_hexSpriteMap);
        OnHsmLoadedStore.Raise(_hexSpriteMap);
        return _hexSpriteMap;
    }

    public Set<HexSpriteEngineStatusElement> Status { get => _status; }
    public GamePack HexSpriteSet { get => _hexSpriteSet; }
    public GameMap HexSpriteMap { get => _hexSpriteMap; set => SetHexSpriteMap(value); }

    internal void SetHexSpriteMapWithoutCurrentMap(GameMap value) => _hexSpriteMap = value;

    public string HexSpriteSetSearchPath { get => _hexSpriteSetSearchPath; set => _hexSpriteSetSearchPath = value; }
    public string HssFileName { get => _hssFileName; }

    public event NotifyEvent<GameMap> OnHsmLoaded
    {
        add => OnHsmLoadedStore.Add(value);
        remove => OnHsmLoadedStore.Remove(value);
    }

    public event NotifyEvent<GameMap> OnHsmClose
    {
        add => OnHsmCloseStore.Add(value);
        remove => OnHsmCloseStore.Remove(value);
    }

    public event NotifyEvent<GameEngine> OnHssLoaded
    {
        add => _onHssLoadedStore.Add(value);
        remove => _onHssLoadedStore.Remove(value);
    }

    public event NotifyEvent<GameEngine> OnHssClose
    {
        add => _onHssCloseStore.Add(value);
        remove => _onHssCloseStore.Remove(value);
    }

}
