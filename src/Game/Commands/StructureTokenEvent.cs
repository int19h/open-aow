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

    private sbyte _x;
    private sbyte _y;
    private sbyte _mapLevel;
    public sbyte Player;
    private int _structureClassId;
    public int Action;
    private bool _razed;
    public int Param;
    private int _param2;

    // Structur.PAS:3051
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwInteger(ref Action, 15);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref _x), 16);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref _y), 17);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref _mapLevel), 18);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref Player), 19);
        stream.rwInteger(ref _structureClassId, 20);
        stream.rwBool(ref _razed, 21);
        stream.rwInteger(ref Param, 22);
        stream.rwInteger(ref _param2, 23);
    }

    // Structur.PAS:3077
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        ExecuteResult result;
        if (tokenEvent.ExecuteCount != 0 || Validate())
        {
            result = ((Structure)StrategicMap.Current.MapContainer.GetField(_x, _y, _mapLevel).FindOwnedChild(_structureClassId)).ExecuteTokenEvent(this, tokenEvent);
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
        MapField mapField;
        mapField = StrategicMap.Current.MapContainer.GetField(_x, _y, _mapLevel);
        structure = (Structure)mapField.FindOwnedChild(_structureClassId);
        if (structure != null && structure.EngineClass.ClassId == _structureClassId)
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
        MapField mapField;
        Structure structure;
        this._x = X;
        this._y = Y;
        _mapLevel = L;
        mapField = StrategicMap.Current.MapContainer.GetField(this._x, this._y, _mapLevel);
        structure = (Structure)mapField.FindOwnedChild(classId);
        Assert(classId == structure.EngineClass.ClassId, "Invalid position (StructureTE)", "Structur.PAS", 3045);
        _structureClassId = classId;
        _razed = structure.GetRazed();
        Player = StrategicMap.Current.MapContainer.GetField(this._x, this._y, _mapLevel).GetPlayer();
    }

    // NetworkE.pas:230

}
