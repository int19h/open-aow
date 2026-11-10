// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Net;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Commands;

public partial class StructureTE : TokenEvent
{
    public StructureTE(ECustomNode owner) : base(owner)
    { }

    public new const int ClassID = 135440;

    public sbyte FX;
    public sbyte FY;
    public sbyte FL;
    public sbyte FPlayer;
    public int FStructureClassID;
    public int FAction;
    public bool FRazed;
    public int FParam;
    public int FParam2;

    // Structur.PAS:3051
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FAction, 15);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FX), 16);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FY), 17);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FL), 18);
        Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FPlayer), 19);
        Stream.rwInteger(ref FStructureClassID, 20);
        Stream.rwBool(ref FRazed, 21);
        Stream.rwInteger(ref FParam, 22);
        Stream.rwInteger(ref FParam2, 23);
    }

    // Structur.PAS:3077
    public override ExecuteResult Execute(TokenExecuter TE)
    {
        ExecuteResult Result;
        if (TE.ExecuteCount != 0 || Validate())
        {
            Result = ((Structure)AoWHSMap.Current.MapContainer.GetField(FX, FY, FL).FindOwnedChild(FStructureClassID)).ExecuteTE(this, TE);
        }
        else
        {
            Result = ExecuteResult.DoneHalt;
        }

        return Result;
    }

    // Structur.PAS:3065
    public virtual bool Validate()
    {
        bool Result;
        Structure Structure;
        AoWMapField MapField;
        MapField = (AoWMapField)AoWHSMap.Current.MapContainer.GetField(FX, FY, FL);
        Structure = (Structure)MapField.FindOwnedChild(FStructureClassID);
        if (Structure != null && Structure.EClass.ClassID == FStructureClassID)
        {
            Result = MapField.GetPlayer() == FPlayer;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Structur.PAS:2537
    public void Initialize(int AClassID, sbyte X, sbyte Y, sbyte L)
    {
        AoWMapField MapField;
        Structure Structure;
        FX = X;
        FY = Y;
        FL = L;
        MapField = (AoWMapField)AoWHSMap.Current.MapContainer.GetField(FX, FY, FL);
        Structure = (Structure)MapField.FindOwnedChild(AClassID);
        Assert(AClassID == Structure.EClass.ClassID, "Invalid position (StructureTE)", "Structur.PAS", 3045);
        FStructureClassID = AClassID;
        FRazed = Structure.GetRazed();
        FPlayer = ((AoWMapField)AoWHSMap.Current.MapContainer.GetField(FX, FY, FL)).GetPlayer();
    }

    // NetworkE.pas:230

}
