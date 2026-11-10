// Translated from Engine.pas.
using Aow.Engine.Nodes;

using Aow.Game;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Engine.Storage;

// a write storage stream that writes every object's data straight into the stream (FBuffer is
// the stream itself)
public partial class DebugWriteStream : EWriteStorageStream
{
    // Engine.pas:10489
    // Retained: The debug writer uses the supplied stream as FBuffer and skips the normal writer buffer allocation. Retire with a replacement-base storage protocol.
    public DebugWriteStream(Stream AStream, GameEngine AEngine) : base(AStream, AEngine, (EBufferStream)AStream)
    { }

    // Engine.pas:10499
    public override void Dispose()
    {
        FPropertyTable.SaveToStream(FStream);
        FPropertyTable?.Dispose();
    }

    // Engine.pas:10505
    public override void rwCreateEObject(ref EObject Obj, ECustomNode AOwner, int ID)
    {
        int AClassID;
        EObject O;
        if (!FPropertyTable.PropertyExist(ID))
        {
            O = Obj;
            if (O != null)
            {
                AClassID = O.EClass.ClassID;
            }
            else
            {
                AClassID = 0;
            }

            if (AClassID != 0)
            {
                FPropertyTable.BackupSettings();
                try
                {
                    Obj.MainReadWrite(this);
                    FBuffer.Write(Bytes.Of(ref AClassID, 4));
                    FPropertyTable.SaveToStream(FBuffer);
                    FBuffer?.Dispose();
                    FPropertyTable.RestoreSettings();
                    FPropertyTable.Add(ID, (int)(FBuffer.Position));
                }
                catch (Exception error)
                {
                    FPropertyTable.RestoreSettings();
                    if (!HandleException(error))
                    {
                        throw;
                    }
                }
            }
            else
            {
                FBuffer.Write(Bytes.Of(ref AClassID, 4));
                FPropertyTable.Add(ID, (int)(FBuffer.Position));
            }
        }
        else
        {
            throw new DoublePrioriyIDException("Double priorityID found (" + IntToStr(ID) + ")");
        }
    }

    // Engine.pas:10542
    public override void rwChildren(ECustomNode Node, int ID)
    {
        EObject C = default;
        if (!FPropertyTable.PropertyExist(ID))
        {
            FPropertyTable.BackupSettings();
            for (int I = 0, last = Node.Count - 1; I <= last; I++)
            {
                C = Node.Children[I];
                if (C != null && C.Owner == Node && !C.GetControlStyle().Contains(EngineServices.csShared) && C.GetControlStyle().Contains(EngineServices.csStored))
                {
                    rwCreateEObject(ref C, Node, I);
                }
            }

            FPropertyTable.SaveToStream(FBuffer);
            FPropertyTable.RestoreSettings();
            FPropertyTable.Add(ID, (int)(FBuffer.Position));
        }
        else
        {
            throw new DoublePrioriyIDException("Double priorityID found (" + IntToStr(ID) + ")");
        }
    }

    // Engine.pas:10565
    public override void rwEObject(EObject Obj, int ID)
    {
        if (!FPropertyTable.PropertyExist(ID))
        {
            FPropertyTable.BackupSettings();
            try
            {
                Obj.MainReadWrite(this);
                FPropertyTable.SaveToStream(FBuffer);
                FPropertyTable.RestoreSettings();
                FPropertyTable.Add(ID, (int)(FBuffer.Position));
            }
            catch (Exception error)
            {
                FPropertyTable.RestoreSettings();
                if (!HandleException(error))
                {
                    throw;
                }
            }
        }
        else
        {
            throw new DoublePrioriyIDException("Double priorityID found (" + Obj.GetType().Name + ":" + IntToStr(ID) + ")");
        }
    }

    // Engine.pas:10587
    public override void rwEObjectStream(EObject Obj, int ID)
    {
        int Size = default;
        FPropertyTable.BackupSettings();
        try
        {
            Obj.MainReadWrite(this);
            FBuffer.Write(Bytes.Of(ref Size, 4));
            FPropertyTable.SaveToStream(FBuffer);
            FBuffer.Write(Bytes.Of(ref Size, 4));
            FPropertyTable.RestoreSettings();
        }
        catch (Exception error)
        {
            FPropertyTable.RestoreSettings();
            Size = 0;
            FBuffer.Write(Bytes.Of(ref Size, 4));
            if (!HandleException(error))
            {
                throw;
            }
        }
    }
}
