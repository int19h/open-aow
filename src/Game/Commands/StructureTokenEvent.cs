// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Net;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Commands;

public partial class StructureTokenEvent : TokenEvent
{
    public StructureTokenEvent(ECustomNode owner) : base(owner)
    { }

    public new const int ClassId = 135440;

    public sbyte X;
    public sbyte Y;
    public sbyte MapLevel;
    public sbyte Player;
    public int StructureClassId;
    public int Action;
    public bool Razed;
    public int Param;
    public int Param2;

    // Structur.PAS:3051
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwInteger(ref Action, 15);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref X), 16);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref Y), 17);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref MapLevel), 18);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref Player), 19);
        stream.rwInteger(ref StructureClassId, 20);
        stream.rwBool(ref Razed, 21);
        stream.rwInteger(ref Param, 22);
        stream.rwInteger(ref Param2, 23);
    }

    // Structur.PAS:3077
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        ExecuteResult result;
        if (tokenEvent.ExecuteCount != 0 || Validate())
        {
            result = ((Structure)AoWHSMap.Current.MapContainer.GetField(X, Y, MapLevel).FindOwnedChild(StructureClassId)).ExecuteTokenEvent(this, tokenEvent);
        }
        else
        {
            result = ExecuteResult.DoneHalt;
        }

        return result;
    }

    // Structur.PAS:3065
    public virtual bool Validate()
    {
        bool result;
        Structure structure;
        AoWMapField mapField;
        mapField = (AoWMapField)AoWHSMap.Current.MapContainer.GetField(X, Y, MapLevel);
        structure = (Structure)mapField.FindOwnedChild(StructureClassId);
        if (structure != null && structure.EngineClass.ClassId == StructureClassId)
        {
            result = mapField.GetPlayer() == Player;
        }
        else
        {
            result = false;
        }

        return result;
    }

    // Structur.PAS:2537
    public void Initialize(int classId, sbyte X, sbyte Y, sbyte L)
    {
        AoWMapField mapField;
        Structure structure;
        this.X = X;
        this.Y = Y;
        MapLevel = L;
        mapField = (AoWMapField)AoWHSMap.Current.MapContainer.GetField(this.X, this.Y, MapLevel);
        structure = (Structure)mapField.FindOwnedChild(classId);
        Assert(classId == structure.EngineClass.ClassId, "Invalid position (StructureTE)", "Structur.PAS", 3045);
        StructureClassId = classId;
        Razed = structure.GetRazed();
        Player = ((AoWMapField)AoWHSMap.Current.MapContainer.GetField(this.X, this.Y, MapLevel)).GetPlayer();
    }

    // NetworkE.pas:230

}
