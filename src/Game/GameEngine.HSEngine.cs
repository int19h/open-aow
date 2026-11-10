// Translated from HSEngine.pas.
using System.Runtime.InteropServices;
using Aow.Engine;
using Aow.Engine.Buffers;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats;

using Aow.Game.Saves;

using Aow.Game.World.Map;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game;

// FStatus, FHSSet, FHSMap, FHSSetSearchPath, FHSSFileName, FOnHSMLoaded, FOnHSMClose,
// FOnHSSLoaded, FOnHSSClose. The engine of the hex map: it owns the set (.hss: a 52-byte
// header, the stored set and a CRC32) and the map (.hsm: a 16-byte header and the stored map,
// compressed). the event lists are told when a map or a set is loaded or closed
public partial class GameEngine : ENode
{
    internal Set<HSEngineStatusElement> FStatus;
    protected internal HSSet FHSSet;
    protected internal HSMap FHSMap;
    internal string FHSSetSearchPath = "";
    internal string FHSSFileName = "";

    internal EventStore<HSMap> FOnHSMLoadedStore;

    internal event NotifyEvent<HSMap> FOnHSMLoaded
    {
        add => FOnHSMLoadedStore.Add(value);
        remove => FOnHSMLoadedStore.Remove(value);
    }

    internal EventStore<HSMap> FOnHSMCloseStore;

    internal event NotifyEvent<HSMap> FOnHSMClose
    {
        add => FOnHSMCloseStore.Add(value);
        remove => FOnHSMCloseStore.Remove(value);
    }

    internal EventStore<GameEngine> FOnHSSLoadedStore;

    internal event NotifyEvent<GameEngine> FOnHSSLoaded
    {
        add => FOnHSSLoadedStore.Add(value);
        remove => FOnHSSLoadedStore.Remove(value);
    }

    internal EventStore<GameEngine> FOnHSSCloseStore;

    internal event NotifyEvent<GameEngine> FOnHSSClose
    {
        add => FOnHSSCloseStore.Add(value);
        remove => FOnHSSCloseStore.Remove(value);
    }

    // HSEngine.pas:5210
    // The preflight of the file boundary finishes before the current map is freed. A refused
    // file leaves the current game and the file as they were (own-formats.md 2.7 and 4.1).
    public virtual void LoadHSM(string FileName, bool Edit, Aow.Engine.ProgressEvent AProgress, GameProfile Profile)
    {
        CompressStream Stream;
        HSMHeader Header = default;
        byte[] Image = FileBoundary.Load(FileName, Profile);
        if (Image == null)
        {
            return;
        }

        if (FHSMap != null)
        {
            FHSMap?.Dispose();
            SetHSMap(null);
        }

        try
        {
            Stream = new CompressStream();
            try
            {
                Stream.LoadImage(Image);
                Stream.ReadBytes(Bytes.Of(ref Header, 16));
                Stream.Position = Header.Size;
                if (ArrayEquals<FixedArray_byte_4, byte>(Header.ID, HSMID))
                {
                    SetHSMap((HSMap)CreateEObject(Header.ClassID, this));
                    var expandFileName = ExpandFileName(FileName);
                    FHSMap.FFileName = expandFileName;
                    FStatus = FStatus + Set<HSEngineStatusElement>.Of(HSEngineStatusElement.LoadingHSM);
                    if (Edit)
                    {
                        FHSMap.FStatus = FHSMap.FStatus + Set<HSMapStatusElement>.Of(HSMapStatusElement.Editting);
                    }

                    try
                    {
                        FHSMap.FOnProgress = AProgress;
                        ReadEObject(Stream, FHSMap, AProgress);
                        ActivateEObject(FHSMap);
                        FHSMap.FOnProgress = null;
                        FOnHSMLoadedStore.Raise(FHSMap);
                    }
                    catch
                    {
                        FHSMap?.Dispose();
                        SetHSMap(null);
                    }

                    FStatus = FStatus - Set<HSEngineStatusElement>.Of(HSEngineStatusElement.LoadingHSM);
                }
            }
            finally
            {
                Stream?.Dispose();
            }
        }
        catch
        {
        }
    }

    // HSEngine.pas:5137
    public virtual HSMap CreatePreviewHSM(string FileName, GameProfile Profile)
    {
        CompressStream Stream;
        string FullName;
        HSMHeader Header = default;
        HSMap Result = null;
        byte[] Image;
        FullName = ExpandFileName(FileName);
        Image = FileBoundary.Load(FullName, Profile, "preview");
        if (Image == null)
        {
            return null;
        }

        try
        {
            Stream = new CompressStream();
            try
            {
                Stream.LoadImage(Image);
                Stream.ReadBytes(Bytes.Of(ref Header, 16));
                Stream.Position = Header.Size;
                if (ArrayEquals<FixedArray_byte_4, byte>(Header.ID, HSMID))
                {
                    Result = (HSMap)CreateEObject(Header.ClassID, this);
                    Result.FFileName = ExpandFileName(FileName);
                    FStatus = FStatus + Set<HSEngineStatusElement>.Of(HSEngineStatusElement.LoadingHSM);
                    try
                    {
                        ReadPreviewEObject(Stream, Result, null);
                    }
                    catch
                    {
                        Result?.Dispose();
                        Result = null;
                    }

                    FStatus = FStatus - Set<HSEngineStatusElement>.Of(HSEngineStatusElement.LoadingHSM);
                }
            }
            finally
            {
                Stream?.Dispose();
            }
        }
        catch
        {
        }

        return Result;
    }

    // HSEngine.pas:5291
    // The engine writes the image in memory and the file boundary writes the document, so a
    // failed conversion or write keeps the old file (own-formats.md 4.2).
    public virtual void SaveHSM(string FileName, Aow.Engine.ProgressEvent AProgress, GameProfile Profile)
    {
        CompressStream Stream;
        HSMHeader Header;
        if (FHSMap != null)
        {
            Header = default;
            Header.Size = 16;
            var classID = FHSMap.EClass.ClassID;
            Header.ClassID = classID;
            Header.Reserved = 0;
            Header.ID = HSMID;
            Stream = new CompressStream();
            FStatus = FStatus + Set<HSEngineStatusElement>.Of(HSEngineStatusElement.SavingHSM);
            try
            {
                Stream.Write(Bytes.Of(ref Header, 16));
                WriteEObject(Stream, FHSMap, AProgress);
                FileBoundary.Save(FileName, Profile, Stream.GetBuffer().AsSpan(0, (int)Stream.Length).ToArray());
            }
            finally
            {
                FStatus = FStatus - Set<HSEngineStatusElement>.Of(HSEngineStatusElement.SavingHSM);
                Stream?.Dispose();
            }
        }
    }

    // HSEngine.pas:5093
    public virtual void CloseHSS()
    {
        FOnHSSCloseStore.Raise(this);
        FHSSet.Clear();
        FHSSFileName = "noname.hss";
    }

    // HSEngine.pas:4967
    public StringList GetHSSetFiles(string Mask)
    {
        StringList Files;
        void Search(string Dir)
        {
            StringList Dirs;
            HSSHeader Header = default;
            SearchRec SR = default;
            bool Found;
            Found = FindFirst(Dir + Mask, 63 & ~16, ref SR) == 0;
            while (Found)
            {
                Header = default;
                // An own scenario set lists the header of its validated image (own-formats.md 4.1).
                var head = FileBoundary.ListingHeader(Dir + SR.Name, GameProfile.ScenarioSet, 52);
                if (head.Length == 52)
                {
                    Header = MemoryMarshal.Read<HSSHeader>(head);
                }

                if (Header.ID == "HSS\0")
                {
                    Files.Add(Header.Name + "|" + Dir + SR.Name);
                }

                Found = FindNext(ref SR) == 0;
            }

            FindClose(ref SR);
            Dirs = new StringList();
            Found = FindFirst(Dir + "*.*", 16, ref SR) == 0;
            while (Found)
            {
                if ((SR.Attr & 16) != 0 && SR.Name != "." && SR.Name != "..")
                {
                    Dirs.Add(Dir + SR.Name + "\\");
                }

                Found = FindNext(ref SR) == 0;
            }

            FindClose(ref SR);
            for (int I = 0, last = Dirs.Count - 1; I <= last; I++)
            {
                Search(Dirs[I]);
            }

            Dirs?.Dispose();
        }

        Files = new StringList();
        Search(StartupDirectory);
        return Files;
    }

    // HSEngine.pas:5024
    // The preflight raises before the current set is cleared (own-formats.md 2.7).
    public void LoadHSS(string FileName, bool Edit, Aow.Engine.ProgressEvent AProgress, GameProfile Profile)
    {
        MemoryStream Mem;
        HSSHeader Header = default;
        EMsg Msg = default;
        int CRC;
        byte[] Image = FileBoundary.LoadOrRaise(ExpandFileName(FileName), Profile);
        FHSSet.Clear();
        FHSSFileName = ExpandFileName(FileName);
        FStatus = FStatus + Set<HSEngineStatusElement>.Of(HSEngineStatusElement.LoadingHSS);
        Mem = ImageStream(Image);
        try
        {
            Mem.Read(Bytes.Of(ref Header, 52));
            Mem.Position = Header.Size;
            var size = (int)Mem.Length;
            CRC = MemoryMarshal.Read<int>(Mem.GetBuffer().AsSpan(size - 4, 4));
            bool condition = Header.ID != "HSS\0";
            if (!condition)
            {
                var memSize = (int)Mem.Length;
                condition = CRC != EngineServices.GetCRC32(Mem.GetBuffer(), memSize - 4);
            }

            if (condition)
            {
                throw new ArgumentException("Invalid HSSET");
            }

            ReadEObject(Mem, FHSSet, AProgress);
            ActivateEObject(FHSSet);
            EMsg.InitMsg(ref Msg, 65556, this);
            FHSSet.MsgProc(ref Msg, this);
            ControlNode.MsgProc(ref Msg, this);
            FOnHSSLoadedStore.Raise(this);
        }
        finally
        {
            FStatus = FStatus - Set<HSEngineStatusElement>.Of(HSEngineStatusElement.LoadingHSS);
            Mem?.Dispose();
        }
    }

    // HSEngine.pas:5060
    public void SaveHSS(string FileName, Aow.Engine.ProgressEvent AProgress, GameProfile Profile)
    {
        MemoryStream Mem;
        int CRC = default;
        HSSHeader Header = default;
        if (FHSSet != null)
        {
            Header.Size = 52;
            Header.Name = FHSSet.Name;
            Header.ID = "HSS\0";
            FHSSFileName = ExpandFileName(FileName);
            FStatus = FStatus + Set<HSEngineStatusElement>.Of(HSEngineStatusElement.SavingHSS);
            Mem = new MemoryStream();
            try
            {
                Mem.Write(Bytes.Of(ref Header, 52));
                WriteEObject(Mem, FHSSet, AProgress);
                var size = (int)Mem.Length;
                CRC = EngineServices.GetCRC32(Mem.GetBuffer(), size);
                Mem.Write(Bytes.Of(ref CRC, 4));
                FileBoundary.Save(FHSSFileName, Profile, Mem.GetBuffer().AsSpan(0, (int)Mem.Length).ToArray());
            }
            finally
            {
                FStatus = FStatus - Set<HSEngineStatusElement>.Of(HSEngineStatusElement.SavingHSS);
                Mem?.Dispose();
            }
        }
    }

    // HSEngine.pas:5100
    public void LoadPreviewHSM(string FileName, GameProfile Profile)
    {
        CompressStream Stream;
        HSMHeader Header = default;
        byte[] Image = FileBoundary.Load(FileName, Profile, "preview");
        if (Image == null)
        {
            return;
        }

        if (FHSMap != null)
        {
            FHSMap?.Dispose();
            SetHSMap(null);
        }

        try
        {
            Stream = new CompressStream();
            try
            {
                Stream.LoadImage(Image);
                Stream.ReadBytes(Bytes.Of(ref Header, 16));
                Stream.Position = Header.Size;
                if (ArrayEquals<FixedArray_byte_4, byte>(Header.ID, HSMID))
                {
                    SetHSMap((HSMap)CreateEObject(Header.ClassID, this));
                    FHSMap.FStatus = FHSMap.FStatus + Set<HSMapStatusElement>.Of(HSMapStatusElement.PreviewMode);
                    var expandFileName = ExpandFileName(FileName);
                    FHSMap.FFileName = expandFileName;
                    FStatus = FStatus + Set<HSEngineStatusElement>.Of(HSEngineStatusElement.LoadingHSM);
                    try
                    {
                        ReadEObject(Stream, FHSMap, null);
                    }
                    catch
                    {
                        FHSMap?.Dispose();
                        SetHSMap(null);
                    }

                    FStatus = FStatus - Set<HSEngineStatusElement>.Of(HSEngineStatusElement.LoadingHSM);
                }
            }
            finally
            {
                Stream?.Dispose();
            }
        }
        catch
        {
        }
    }

    // HSEngine.pas:5171
    public HSMap LoadHSMEx(string FileName, bool Edit, Aow.Engine.ProgressEvent AProgress, GameProfile Profile)
    {
        CompressStream Stream;
        string FullName;
        HSMHeader Header = default;
        HSMap Result = null;
        byte[] Image;
        FullName = ExpandFileName(FileName);
        Image = FileBoundary.Load(FullName, Profile);
        if (Image == null)
        {
            return null;
        }

        try
        {
            Stream = new CompressStream();
            try
            {
                Stream.LoadImage(Image);
                Stream.ReadBytes(Bytes.Of(ref Header, 16));
                Stream.Position = Header.Size;
                if (ArrayEquals<FixedArray_byte_4, byte>(Header.ID, HSMID))
                {
                    Result = (HSMap)CreateEObject(Header.ClassID, null);
                    Result.FFileName = FullName;
                    FStatus = FStatus + Set<HSEngineStatusElement>.Of(HSEngineStatusElement.LoadingHSM);
                    if (Edit)
                    {
                        Result.FStatus = Result.FStatus + Set<HSMapStatusElement>.Of(HSMapStatusElement.Editting);
                    }

                    try
                    {
                        Result.FOnProgress = AProgress;
                        ReadEObject(Stream, Result, AProgress);
                        ActivateEObject(Result);
                        Result.FOnProgress = null;
                        FOnHSMLoadedStore.Raise(Result);
                    }
                    catch
                    {
                        Result?.Dispose();
                        Result = null;
                    }

                    FStatus = FStatus - Set<HSEngineStatusElement>.Of(HSEngineStatusElement.LoadingHSM);
                }
            }
            finally
            {
                Stream?.Dispose();
            }
        }
        catch
        {
        }

        return Result;
    }

    // HSEngine.pas:5251
    public void LoadHSMfromStream(Stream Stream, bool Edit, Aow.Engine.ProgressEvent AProgress)
    {
        CompressStream CStream;
        HSMHeader Header = default;
        if (FHSMap != null)
        {
            FHSMap?.Dispose();
            SetHSMap(null);
        }

        try
        {
            CStream = new CompressStream();
            try
            {
                CStream.LoadFromStream(Stream);
                CStream.ReadBytes(Bytes.Of(ref Header, 16));
                CStream.Position = Header.Size;
                if (ArrayEquals<FixedArray_byte_4, byte>(Header.ID, HSMID))
                {
                    SetHSMap((HSMap)CreateEObject(Header.ClassID, this));
                    FStatus = FStatus + Set<HSEngineStatusElement>.Of(HSEngineStatusElement.LoadingHSM);
                    if (Edit)
                    {
                        FHSMap.FStatus = FHSMap.FStatus + Set<HSMapStatusElement>.Of(HSMapStatusElement.Editting);
                    }

                    try
                    {
                        FHSMap.FOnProgress = AProgress;
                        ReadEObject(CStream, FHSMap, AProgress);
                        ActivateEObject(FHSMap);
                        FHSMap.FOnProgress = null;
                        FOnHSMLoadedStore.Raise(FHSMap);
                    }
                    catch
                    {
                        FHSMap?.Dispose();
                        SetHSMap(null);
                    }

                    FStatus = FStatus - Set<HSEngineStatusElement>.Of(HSEngineStatusElement.LoadingHSM);
                }
            }
            finally
            {
                CStream?.Dispose();
            }
        }
        catch
        {
        }
    }

    // HSEngine.pas:5316
    public void SaveHSMtoStream(Stream Stream, Aow.Engine.ProgressEvent AProgress)
    {
        CompressStream CStream;
        HSMHeader Header;
        if (FHSMap != null)
        {
            Header = default;
            Header.Size = 16;
            var classID = FHSMap.EClass.ClassID;
            Header.ClassID = classID;
            Header.Reserved = 0;
            Header.ID = HSMID;
            CStream = new CompressStream();
            FStatus = FStatus + Set<HSEngineStatusElement>.Of(HSEngineStatusElement.SavingHSM);
            try
            {
                CStream.Write(Bytes.Of(ref Header, 16));
                WriteEObject(CStream, FHSMap, AProgress);
                CStream.SaveToStream(Stream);
            }
            finally
            {
                FStatus = FStatus - Set<HSEngineStatusElement>.Of(HSEngineStatusElement.SavingHSM);
                CStream?.Dispose();
            }
        }
    }

    // HSEngine.pas:5341
    // Every caller derives the name (RELEASE.HSS, the set a map names), so the own sibling opens
    // first (own-formats.md 2.9).
    public void InitializeHSSet(string FileName, Aow.Engine.ProgressEvent AProgress)
    {
        EMsg Msg = default;
        FileName = FileBoundary.ReadName(FileName);
        if (FHSSet == null || CompareText(FileName, FHSSFileName) != 0)
        {
            LoadHSS(FileName, false, AProgress, GameProfile.ScenarioSet);
        }
        else
        {
            EMsg.InitMsg(ref Msg, 65557, this);
            FHSSet.MsgProc(ref Msg, this);
        }
    }

    // HSEngine.pas:5354
    public HSMap NewHSM(int AClassID)
    {
        if (FHSMap != null)
        {
            FHSMap?.Dispose();
            SetHSMap(null);
        }

        SetHSMap((HSMap)CreateEObject(AClassID, this));
        FHSMap.FStatus = FHSMap.FStatus + Set<HSMapStatusElement>.Of(HSMapStatusElement.Editting);
        ActivateEObject(FHSMap);
        FOnHSMLoadedStore.Raise(FHSMap);
        return FHSMap;
    }

    public Set<HSEngineStatusElement> Status { get => FStatus; }
    public HSSet HSSet { get => FHSSet; }
    public HSMap HSMap { get => FHSMap; set => SetHSMap(value); }
    public string HSSetSearchPath { get => FHSSetSearchPath; set => FHSSetSearchPath = value; }
    public string HSSFileName { get => FHSSFileName; }

    public event NotifyEvent<HSMap> OnHSMLoaded
    {
        add => FOnHSMLoadedStore.Add(value);
        remove => FOnHSMLoadedStore.Remove(value);
    }

    public event NotifyEvent<HSMap> OnHSMClose
    {
        add => FOnHSMCloseStore.Add(value);
        remove => FOnHSMCloseStore.Remove(value);
    }

    public event NotifyEvent<GameEngine> OnHSSLoaded
    {
        add => FOnHSSLoadedStore.Add(value);
        remove => FOnHSSLoadedStore.Remove(value);
    }

    public event NotifyEvent<GameEngine> OnHSSClose
    {
        add => FOnHSSCloseStore.Add(value);
        remove => FOnHSSCloseStore.Remove(value);
    }

    private static FixedArray_byte_4 HSMID = FixedArray_byte_4.From("HSM\0"u8);
    internal static int HSEngineDID;

}
