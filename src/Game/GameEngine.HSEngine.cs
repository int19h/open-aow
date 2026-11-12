// Translated from HSEngine.pas.
using System.Runtime.InteropServices;
using Aow.Engine;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats;

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

    private EventStore<GameEngine> _onHssLoadedStore;

    internal event NotifyEvent<GameEngine> HexSpriteSetLoaded
    {
        add => _onHssLoadedStore.Add(value);
        remove => _onHssLoadedStore.Remove(value);
    }

    private EventStore<GameEngine> _onHssCloseStore;

    internal event NotifyEvent<GameEngine> HexSpriteSetClosing
    {
        add => _onHssCloseStore.Add(value);
        remove => _onHssCloseStore.Remove(value);
    }

    // HSEngine.pas:5210
    // The preflight of the file boundary finishes before the current map is freed. A refused
    // file leaves the current game and the file as they were (own-formats.md 2.7 and 4.1).
    public virtual void LoadHsm(string fileName, bool edit, Aow.Engine.ProgressEvent progress, GameProfile profile)
    {
        CompressStream stream;
        HsmHeader header = default;
        byte[] image = FileBoundary.Load(fileName, profile);
        if (image == null)
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
            stream = new CompressStream();
            try
            {
                stream.LoadImage(image);
                stream.ReadExactly(Bytes.Of(ref header, 16));
                stream.Position = header.Size;
                if (((ReadOnlySpan<byte>)header.Id).SequenceEqual(HsmId))
                {
                    SetHexSpriteMap((GameMap)CreateEngineObject(header.ClassId, this));
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
                        ReadEngineObject(stream, _hexSpriteMap, progress);
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
            }
            finally
            {
                stream?.Dispose();
            }
        }
        catch
        {
        }
    }

    // HSEngine.pas:5137
    public virtual GameMap CreatePreviewHsm(string fileName, GameProfile profile)
    {
        CompressStream stream;
        string fullName;
        HsmHeader header = default;
        GameMap result = null;
        byte[] image;
        fullName = ExpandFileName(fileName);
        image = FileBoundary.Load(fullName, profile, "preview");
        if (image == null)
        {
            return null;
        }

        try
        {
            stream = new CompressStream();
            try
            {
                stream.LoadImage(image);
                stream.ReadExactly(Bytes.Of(ref header, 16));
                stream.Position = header.Size;
                if (((ReadOnlySpan<byte>)header.Id).SequenceEqual(HsmId))
                {
                    result = (GameMap)CreateEngineObject(header.ClassId, this);
                    result.FileName = ExpandFileName(fileName);
                    _status = _status + Set<HexSpriteEngineStatusElement>.Of(HexSpriteEngineStatusElement.LoadingHsm);
                    try
                    {
                        ReadPreviewEngineObject(stream, result, null);
                    }
                    catch
                    {
                        result?.Dispose();
                        result = null;
                    }

                    _status = _status - Set<HexSpriteEngineStatusElement>.Of(HexSpriteEngineStatusElement.LoadingHsm);
                }
            }
            finally
            {
                stream?.Dispose();
            }
        }
        catch
        {
        }

        return result;
    }

    // HSEngine.pas:5291
    // The engine writes the image in memory and the file boundary writes the document, so a
    // failed conversion or write keeps the old file (own-formats.md 4.2).
    public virtual void SaveHsm(string fileName, Aow.Engine.ProgressEvent progress, GameProfile profile)
    {
        CompressStream stream;
        HsmHeader header;
        if (_hexSpriteMap != null)
        {
            header = default;
            header.Size = 16;
            var classId = _hexSpriteMap.EngineClass.ClassId;
            header.ClassId = classId;
            header.Reserved = 0;
            HsmId.CopyTo(header.Id);
            stream = new CompressStream();
            _status = _status + Set<HexSpriteEngineStatusElement>.Of(HexSpriteEngineStatusElement.SavingHsm);
            try
            {
                stream.Write(Bytes.Of(ref header, 16));
                WriteEngineObject(stream, _hexSpriteMap, progress);
                FileBoundary.Save(fileName, profile, stream.GetBuffer().AsSpan(0, (int)stream.Length).ToArray());
            }
            finally
            {
                _status = _status - Set<HexSpriteEngineStatusElement>.Of(HexSpriteEngineStatusElement.SavingHsm);
                stream?.Dispose();
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
    // The preflight raises before the current set is cleared (own-formats.md 2.7).
    public void LoadHss(string fileName, bool edit, Aow.Engine.ProgressEvent progress, GameProfile profile)
    {
        MemoryStream memory;
        HssHeader header = default;
        EngineMessage message = default;
        int crc;
        byte[] image = FileBoundary.LoadOrRaise(ExpandFileName(fileName), profile);
        _hexSpriteSet.Clear();
        _hssFileName = ExpandFileName(fileName);
        _status = _status + Set<HexSpriteEngineStatusElement>.Of(HexSpriteEngineStatusElement.LoadingHss);
        memory = ImageStream(image);
        try
        {
            memory.Read(Bytes.Of(ref header, 52));
            memory.Position = header.Size;
            var size = (int)memory.Length;
            crc = MemoryMarshal.Read<int>(memory.GetBuffer().AsSpan(size - 4, 4));
            bool condition = header.Id != "HSS\0";
            if (!condition)
            {
                var memorySize = (int)memory.Length;
                condition = crc != EngineServices.GetCrc32(memory.GetBuffer(), memorySize - 4);
            }

            if (condition)
            {
                throw new ArgumentException("Invalid HSSET");
            }

            ReadEngineObject(memory, _hexSpriteSet, progress);
            ActivateEngineObject(_hexSpriteSet);
            EngineMessage.InitializeMessage(ref message, 65556, this);
            _hexSpriteSet.MessageProc(ref message, this);
            ControlNode.MessageProc(ref message, this);
            _onHssLoadedStore.Raise(this);
        }
        finally
        {
            _status = _status - Set<HexSpriteEngineStatusElement>.Of(HexSpriteEngineStatusElement.LoadingHss);
            memory?.Dispose();
        }
    }

    // HSEngine.pas:5060
    public void SaveHss(string fileName, Aow.Engine.ProgressEvent progress, GameProfile profile)
    {
        MemoryStream memory;
        int crc = default;
        HssHeader header = default;
        if (_hexSpriteSet != null)
        {
            header.Size = 52;
            header.Name = _hexSpriteSet.Name;
            header.Id = "HSS\0";
            _hssFileName = ExpandFileName(fileName);
            _status = _status + Set<HexSpriteEngineStatusElement>.Of(HexSpriteEngineStatusElement.SavingHss);
            memory = new MemoryStream();
            try
            {
                memory.Write(Bytes.Of(ref header, 52));
                WriteEngineObject(memory, _hexSpriteSet, progress);
                var size = (int)memory.Length;
                crc = EngineServices.GetCrc32(memory.GetBuffer(), size);
                memory.Write(Bytes.Of(ref crc, 4));
                FileBoundary.Save(_hssFileName, profile, memory.GetBuffer().AsSpan(0, (int)memory.Length).ToArray());
            }
            finally
            {
                _status = _status - Set<HexSpriteEngineStatusElement>.Of(HexSpriteEngineStatusElement.SavingHss);
                memory?.Dispose();
            }
        }
    }

    // HSEngine.pas:5100
    public void LoadPreviewHsm(string fileName, GameProfile profile)
    {
        CompressStream stream;
        HsmHeader header = default;
        byte[] image = FileBoundary.Load(fileName, profile, "preview");
        if (image == null)
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
            stream = new CompressStream();
            try
            {
                stream.LoadImage(image);
                stream.ReadExactly(Bytes.Of(ref header, 16));
                stream.Position = header.Size;
                if (((ReadOnlySpan<byte>)header.Id).SequenceEqual(HsmId))
                {
                    SetHexSpriteMap((GameMap)CreateEngineObject(header.ClassId, this));
                    _hexSpriteMap.Status = _hexSpriteMap.Status + Set<HexSpriteMapStatusElement>.Of(HexSpriteMapStatusElement.PreviewMode);
                    var expandFileName = ExpandFileName(fileName);
                    _hexSpriteMap.FileName = expandFileName;
                    _status = _status + Set<HexSpriteEngineStatusElement>.Of(HexSpriteEngineStatusElement.LoadingHsm);
                    try
                    {
                        ReadEngineObject(stream, _hexSpriteMap, null);
                    }
                    catch
                    {
                        _hexSpriteMap?.Dispose();
                        SetHexSpriteMap(null);
                    }

                    _status = _status - Set<HexSpriteEngineStatusElement>.Of(HexSpriteEngineStatusElement.LoadingHsm);
                }
            }
            finally
            {
                stream?.Dispose();
            }
        }
        catch
        {
        }
    }

    // HSEngine.pas:5171
    public GameMap LoadHsmMap(string fileName, bool edit, Aow.Engine.ProgressEvent progress, GameProfile profile)
    {
        CompressStream stream;
        string fullName;
        HsmHeader header = default;
        GameMap result = null;
        byte[] image;
        fullName = ExpandFileName(fileName);
        image = FileBoundary.Load(fullName, profile);
        if (image == null)
        {
            return null;
        }

        try
        {
            stream = new CompressStream();
            try
            {
                stream.LoadImage(image);
                stream.ReadExactly(Bytes.Of(ref header, 16));
                stream.Position = header.Size;
                if (((ReadOnlySpan<byte>)header.Id).SequenceEqual(HsmId))
                {
                    result = (GameMap)CreateEngineObject(header.ClassId, null);
                    result.FileName = fullName;
                    _status = _status + Set<HexSpriteEngineStatusElement>.Of(HexSpriteEngineStatusElement.LoadingHsm);
                    if (edit)
                    {
                        result.Status = result.Status + Set<HexSpriteMapStatusElement>.Of(HexSpriteMapStatusElement.Editting);
                    }

                    try
                    {
                        result.SetProgressHandler(progress);
                        ReadEngineObject(stream, result, progress);
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
            }
            finally
            {
                stream?.Dispose();
            }
        }
        catch
        {
        }

        return result;
    }

    // HSEngine.pas:5251
    public void LoadHsmFromStream(Stream stream, bool edit, Aow.Engine.ProgressEvent progress)
    {
        CompressStream cStream;
        HsmHeader header = default;
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
                cStream.ReadExactly(Bytes.Of(ref header, 16));
                cStream.Position = header.Size;
                if (((ReadOnlySpan<byte>)header.Id).SequenceEqual(HsmId))
                {
                    SetHexSpriteMap((GameMap)CreateEngineObject(header.ClassId, this));
                    _status = _status + Set<HexSpriteEngineStatusElement>.Of(HexSpriteEngineStatusElement.LoadingHsm);
                    if (edit)
                    {
                        _hexSpriteMap.Status = _hexSpriteMap.Status + Set<HexSpriteMapStatusElement>.Of(HexSpriteMapStatusElement.Editting);
                    }

                    try
                    {
                        _hexSpriteMap.SetProgressHandler(progress);
                        ReadEngineObject(cStream, _hexSpriteMap, progress);
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
    public void SaveHsmToStream(Stream stream, Aow.Engine.ProgressEvent progress)
    {
        CompressStream cStream;
        HsmHeader header;
        if (_hexSpriteMap != null)
        {
            header = default;
            header.Size = 16;
            var classId = _hexSpriteMap.EngineClass.ClassId;
            header.ClassId = classId;
            header.Reserved = 0;
            HsmId.CopyTo(header.Id);
            cStream = new CompressStream();
            _status = _status + Set<HexSpriteEngineStatusElement>.Of(HexSpriteEngineStatusElement.SavingHsm);
            try
            {
                cStream.Write(Bytes.Of(ref header, 16));
                WriteEngineObject(cStream, _hexSpriteMap, progress);
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
            EngineMessage.InitializeMessage(ref message, 65557, this);
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

    private static ReadOnlySpan<byte> HsmId => "HSM\0"u8;

}
