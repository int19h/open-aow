// Translated from HSEngine.pas.
using Aow.Engine;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Audio;

using Aow.Game.Draw.Images;

using Aow.Rtl;

namespace Aow.Game.World.Map;

// FStatus, FHexagonControl, FRoadControl, FActivatedEngine, FMapObjectResourceList,
// FResourceEditSettings, FImageLibraryManager, FSFXManager, FPlugInControl, FName,
// FTerrainTypes, FOverlayTypes, FDecorationTypes. The hexagon sprite set: the resources a map
// uses (an.hss file, THSEngine.LoadHSS). ReadWrite stores the name (id 4), the map object
// resources (5), the resource edit settings (6) and the plug-ins (7). the type name lists are
// filled by the game (TAoWHSSet.Create) and not stored
public partial class HSSet : ECustomOwner
{
    // HSEngine.pas:3394
    public new const int ClassID = 66049;

    public HSSet(ECustomNode owner) : this(null, owner) { }

    private protected HSSet(ConstructionContext context, ECustomNode owner) : base(context, owner)
    {
        SetControlStyle(GetControlStyle() + Set<EControlStyleElement>.Of(EngineServices.csReserved2));
        FPlugInControl = new PlugInControl(null);
        FTerrainTypes = new StringList();
        FOverlayTypes = new StringList();
        FDecorationTypes = new StringList();
        FHexagonControl = new HexagonControl(this);
        FHexagonControl.MOClass = typeof(AbstractHexagon);
        FRoadControl = new RoadControl(this);
        FRoadControl.MOClass = typeof(AbstractRoad);
        FMapObjectResourceList = new MapObjectResourceList(this);
        FMapObjectResourceList.SetControlStyle(Set<EControlStyleElement>.Of(EngineServices.csStored, EngineServices.csShared));
        FResourceEditSettings = new EResourceEditSettings(this);
        FResourceEditSettings.SetControlStyle(Set<EControlStyleElement>.Of(EngineServices.csStored, EngineServices.csShared));
        FImageLibraryManager = new ImageLibraryManager(null);
        FSFXManager = new SFXManager(null);
    }

    internal Set<HSSetStatusElement> FStatus;
    internal HexagonControl FHexagonControl;
    internal RoadControl FRoadControl;
    internal GameEngine FActivatedEngine;

    public GameEngine ActivatedEngine { get => FActivatedEngine; }

    internal MapObjectResourceList FMapObjectResourceList;
    internal EResourceEditSettings FResourceEditSettings;
    internal ImageLibraryManager FImageLibraryManager;
    internal SFXManager FSFXManager;
    internal PlugInControl FPlugInControl;
    internal string FName = "";
    internal StringList FTerrainTypes;
    internal StringList FOverlayTypes;
    internal StringList FDecorationTypes;

    // HSEngine.pas:3368
    public override void Dispose()
    {
        EMsg Msg = default;
        FStatus = FStatus + Set<HSSetStatusElement>.Of(HSSetStatusElement.Destroying);
        EMsg.InitMsg(ref Msg, 66564, this);
        if (FActivatedEngine != null && FActivatedEngine.ControlNode != null)
        {
            FActivatedEngine.ControlNode.MsgProc(ref Msg, this);
        }

        base.Dispose();
        FPlugInControl?.Dispose();
        FTerrainTypes?.Dispose();
        FTerrainTypes = null;
        FOverlayTypes?.Dispose();
        FOverlayTypes = null;
        FDecorationTypes?.Dispose();
        FDecorationTypes = null;
        FImageLibraryManager?.Dispose();
        FImageLibraryManager = null;
        FSFXManager?.Dispose();
        FSFXManager = null;
        EMsg.InitMsg(ref Msg, 66565, this);
        if (FActivatedEngine != null && FActivatedEngine.ControlNode != null)
        {
            FActivatedEngine.ControlNode.MsgProc(ref Msg, this);
        }

        FStatus = FStatus - Set<HSSetStatusElement>.Of(HSSetStatusElement.Destroying);
    }

    // HSEngine.pas:3410
    public override void ReadWrite(EStorageStream Stream)
    {
        SetEngine(Stream.Engine);
        FPlugInControl.Engine = Stream.Engine;
        base.ReadWrite(Stream);
        Stream.rwString(ref FName, 4);
        Stream.rwEObject(FMapObjectResourceList, 5);
        Stream.rwEObject(FResourceEditSettings, 6);
        Stream.rwEObject(FPlugInControl, 7);
    }

    // HSEngine.pas:3405
    public override void Clear()
    {
        base.Clear();
    }

    // HSEngine.pas:3399
    public override void MsgProc(ref EMsg Msg, EObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 536870920)
        {
            FActivatedEngine = Msg.Payload.As<GameEngine>();
        }
    }

    // HSEngine.pas:3421
    public void Activate(bool Edit)
    {
        EMsg Msg = default;
        EMsg.InitMsg(ref Msg, 66560, this);
        GetEngine().ControlNode.MsgProc(ref Msg, this);
        FStatus = FStatus + Set<HSSetStatusElement>.Of(HSSetStatusElement.Activated);
        if (Edit)
        {
            EMsg.InitMsg(ref Msg, 66562, this);
            GetEngine().ControlNode.MsgProc(ref Msg, this);
            FStatus = FStatus + Set<HSSetStatusElement>.Of(HSSetStatusElement.Editting);
        }
    }

    // HSEngine.pas:3436
    public void DeActivate()
    {
        EMsg Msg = default;
        if (FStatus.Contains(HSSetStatusElement.Activated))
        {
            EMsg.InitMsg(ref Msg, 66561, this);
            GetEngine().ControlNode.MsgProc(ref Msg, this);
            FStatus = FStatus - Set<HSSetStatusElement>.Of(HSSetStatusElement.Activated);
            if (FStatus.Contains(HSSetStatusElement.Editting))
            {
                EMsg.InitMsg(ref Msg, 66563, this);
                GetEngine().ControlNode.MsgProc(ref Msg, this);
                FStatus = FStatus - Set<HSSetStatusElement>.Of(HSSetStatusElement.Editting);
            }
        }
    }

    // HSEngine.pas:3454
    public void SetModified()
    {
        if (FStatus.Contains(HSSetStatusElement.Editting))
        {
            FStatus = FStatus + Set<HSSetStatusElement>.Of(HSSetStatusElement.Modified);
        }
    }

    // HSEngine.pas:3459
    public void ResetModified()
    {
        FStatus = FStatus - Set<HSSetStatusElement>.Of(HSSetStatusElement.Modified);
    }

    // HSEngine.pas:3464
    public void SetEditting()
    {
        FStatus = FStatus + Set<HSSetStatusElement>.Of(HSSetStatusElement.Editting);
    }

    // HSEngine.pas:3468
    public void ResetEditting()
    {
        FStatus = FStatus - Set<HSSetStatusElement>.Of(HSSetStatusElement.Editting);
    }

    public Set<HSSetStatusElement> Status { get => FStatus; }
    public HexagonControl HexagonControl { get => FHexagonControl; }
    public RoadControl RoadControl { get => FRoadControl; }
    public MapObjectResourceList MapObjectResourceList { get => FMapObjectResourceList; }
    public EResourceEditSettings ResourceEditSettings { get => FResourceEditSettings; }
    public ImageLibraryManager ImageLibraryManager { get => FImageLibraryManager; }
    public SFXManager SFXManager { get => FSFXManager; }
    public PlugInControl PlugInControl { get => FPlugInControl; }
    public string Name { get => FName; set => FName = value; }
    public StringList TerrainTypes { get => FTerrainTypes; }
    public StringList OverlayTypes { get => FOverlayTypes; }
    public StringList DecorationTypes { get => FDecorationTypes; }

}
