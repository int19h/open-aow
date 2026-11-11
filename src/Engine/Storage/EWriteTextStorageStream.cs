// Translated from Engine.pas.
using Aow.Engine.Nodes;

using Aow.Game;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Engine.Storage;

// FPath, FBuffer. Each object's properties are collected in FBuffer and written behind the
// "[:path:]" line
public partial class EWriteTextStorageStream : EStorageStream
{
    internal string FPath = "";
    protected internal BufferStream FBuffer;

    // Engine.pas:10355
    internal void WriteTextBuffer(Stream Stream)
    {
        string S;
        if (FPropertyTable.Count != 0 && FBuffer.Length != 0)
        {
            S = "[:" + FPath + ":]" + "\r" + "\n";
            if (!IsEmpty(FPath))
            {
                S = "\r\n" + S;
            }

            Stream.Write(Cp1252.Encode(S).AsSpan().Slice(1 - 1, S.Length));
            FBuffer.SaveToStream(Stream);
        }
    }

    // Engine.pas:10335
    public EWriteTextStorageStream(Stream AStream, GameEngine AEngine) : base(AStream, AEngine)
    {
        FPath = "";
        FBuffer = new BufferStream();
        FPropertyTable = new PropertyTable();
    }

    // Engine.pas:10343
    public override void Dispose()
    {
        WriteTextBuffer(FStream);
        FBuffer?.Dispose();
        FPropertyTable?.Dispose();
    }

    // Engine.pas:10350
    public override Set<StorageModeElement> StorageMode() => Set<StorageModeElement>.Of(StorageModeElement.Write, EngineServices.smText);

    // Engine.pas:10368
    public override void rwCreateEObject(ref EngineObject Obj, ECustomNode AOwner, int ID)
    {
        EngineObject O;
        O = Obj;
        if (O != null)
        {
            rwEObject(O, ID);
        }
    }

    // Engine.pas:10376
    public override void rwEObject(EngineObject Obj, int ID)
    {
        BufferStream SavedBuffer;
        string SavedPath;
        bool Continue;
        if (!FPropertyTable.PropertyExist(ID))
        {
            SavedBuffer = FBuffer;
            SavedPath = FPath;
            FPropertyTable.BackupSettings();
            FBuffer = new BufferStream();
            if (!IsEmpty(FPath))
            {
                FPath = FPath + "\\" + IntToStr(ID);
            }
            else
            {
                FPath = IntToStr(ID);
            }

            try
            {
                Obj.MainReadWrite(this);
                WriteTextBuffer(FStream);
                FBuffer?.Dispose();
                FPropertyTable.RestoreSettings();
                FBuffer = SavedBuffer;
                FPath = SavedPath;
                FPropertyTable.Add(ID, 0);
            }
            catch
            {
                FBuffer?.Dispose();
                FBuffer = SavedBuffer;
                FPropertyTable.RestoreSettings();
                FPath = SavedPath;
                Continue = true;
                if (FOnError != null)
                {
                    var classID = Obj.EClass.ClassID;
                    FOnError(this, "Error writing object (cid:" + IntToStr(classID) + ")", ref Continue);
                }

                if (!Continue)
                {
                    throw;
                }
            }
        }
        else
        {
            throw new DoublePriorityIdException("Double priorityID found");
        }
    }

    // Engine.pas:10412
    public override void rwEObjectStream(EngineObject Obj, int ID)
    {
        rwEObject(Obj, ID);
    }

    // Engine.pas:10417
    public override void rwString(ref string S, int ID)
    {
        string Line;
        if (!FPropertyTable.PropertyExist(ID))
        {
            if (!IsEmpty(S))
            {
                Line = "[" + IntToStr(ID) + "]=" + S + "\r" + "\n";
                FBuffer.Write(Cp1252.Encode(Line).AsSpan().Slice(1 - 1, Line.Length));
                FPropertyTable.Add(ID, 0);
            }
        }
        else
        {
            throw new DoublePriorityIdException("Double priorityID found");
        }
    }

    // Engine.pas:10434
    public override void rwStrings(StringCollection Strings, int ID)
    {
        string Line;
        string P;
        if (!FPropertyTable.PropertyExist(ID))
        {
            if (Strings.Count != 0)
            {
                P = Strings.Text;
                {
                    Line = "[" + IntToStr(ID) + "]=";
                    Line += NullTerminatedText(P);

                    FBuffer.Write(Cp1252.Encode(Line).AsSpan().Slice(1 - 1, Line.Length));
                    FPropertyTable.Add(ID, 0);
                }
            }
        }
        else
        {
            throw new DoublePriorityIdException("Double priorityID found");
        }
    }

    // Engine.pas:10458
    public override void rwStringsEx(StringCollection Strings, int ID)
    {
        rwStrings(Strings, ID);
    }

    // Engine.pas:10463
    public override void rwChildren(ECustomNode Node, int ID)
    {
        string SavedPath;
        EngineObject C;
        if (!FPropertyTable.PropertyExist(ID))
        {
            SavedPath = FPath;
            if (!IsEmpty(FPath))
            {
                FPath = FPath + "\\" + IntToStr(ID);
            }
            else
            {
                FPath = IntToStr(ID);
            }

            for (int I = 0, last = Node.Count - 1; I <= last; I++)
            {
                C = Node.Children[I];
                if (C != null && C.Owner == Node && !C.GetControlStyle().Contains(EngineServices.csShared) && C.GetControlStyle().Contains(EngineServices.csStored))
                {
                    rwEObject(C, I);
                }
            }

            FPath = SavedPath;
            FPropertyTable.Add(ID, 0);
        }
        else
        {
            throw new DoublePriorityIdException("Double priorityID found");
        }
    }

}
