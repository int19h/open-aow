// Translated from AoWE.pas.
using System.Runtime.CompilerServices;
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Tree;
using Aow.Game.Net;

using Aow.Game.World.Map;

using Aow.Game.World.Structures;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Commands;

public partial class StructureTokenEvent : TokenEvent
{
    public StructureTokenEvent(CustomNode owner) : base(owner)
    { }

    public new const int ClassId = 135440;

    private sbyte _x;
    private sbyte _y;
    private sbyte _mapLevel;

    public sbyte Player { get; private set; }

    private int _structureClassId;

    public int Action { get; set; }

    private bool _razed;

    public int Param { get; set; }

    private int _param2;

    // Structur.PAS:3051
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Action = persister.Int32("Action", value: Action);
        _x = PersistSupport.UnsignedByte(persister, name: "X", value: _x);
        _y = PersistSupport.UnsignedByte(persister, name: "Y", value: _y);
        _mapLevel = PersistSupport.UnsignedByte(persister, name: "MapLevel", value: _mapLevel);
        Player = PersistSupport.UnsignedByte(persister, name: "Player", value: Player);
        _structureClassId = persister.Int32("StructureClassId", value: _structureClassId);
        _razed = persister.Boolean("Razed", value: _razed);
        Param = persister.Int32("Param", value: Param);
        _param2 = persister.Int32("Param2", value: _param2);
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
    public void Initialize(int classId, sbyte x, sbyte y, sbyte mapLevel)
    {
        MapField mapField;
        Structure structure;
        this._x = x;
        this._y = y;
        _mapLevel = mapLevel;
        mapField = StrategicMap.Current.MapContainer.GetField(this._x, this._y, _mapLevel);
        structure = (Structure)mapField.FindOwnedChild(classId);
        Assert(classId == structure.EngineClass.ClassId, "Invalid position (StructureTE)", "Structur.PAS", 3045);
        _structureClassId = classId;
        _razed = structure.GetRazed();
        Player = StrategicMap.Current.MapContainer.GetField(this._x, this._y, _mapLevel).GetPlayer();
    }

    // NetworkE.pas:230

}
