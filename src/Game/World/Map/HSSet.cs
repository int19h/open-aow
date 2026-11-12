// Translated from HSEngine.pas.
using Aow.Engine;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Audio;

using Aow.Game.Draw.Images;

using Aow.Rtl;

namespace Aow.Game.World.Map;

// _status, _hexControl, _roadControl, _activatedEngine, _mapObjectResourceList,
// _resourceEditSettings, _imageLibraryManager, _sfxManager, _pluginControl, _name,
// FTerrainTypes, FOverlayTypes, FDecorationTypes. The hexagon sprite set: the resources a map
// uses (an.hss file, THSEngine.LoadHSS). ReadWrite stores the name (id 4), the map object
// resources (5), the resource edit settings (6) and the plug-ins (7). the type name lists are
// filled by the game (TAoWHSSet.Create) and not stored
public partial class HSSet : ECustomOwner, IChildList<HSSetChild>
{
    // HSEngine.pas:3394
    public new const int ClassId = 66049;

    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only the kinds of HSSetChild, so the read never fails.
    public HSSetChild this[int index] => HSSetChild.Of(GetChild(index));

    protected sealed override bool AcceptsChild(EngineObject child) => child is EResourceList or MapObjectResourceTerrainControl or ControlStyleObject;

    int IChildSink<HSSetChild>.AddChild(HSSetChild child) => AddChild(child.Node);

    public HSSet(ECustomNode owner) : this(null, owner) { }

    private protected HSSet(ConstructionContext context, ECustomNode owner) : base(context, owner)
    {
        SetControlStyle(GetControlStyle() + Set<ControlStyleElement>.Of(EngineServices.Reserved2));
        _pluginControl = new PluginControl(null);
        _terrainTypes = new StringList();
        _overlayTypes = new StringList();
        _decorationTypes = new StringList();
        _hexControl = new HexControl(this);
        _hexControl.MapObjectClass = typeof(AbstractHexagon);
        _roadControl = new RoadControl(this);
        _roadControl.MapObjectClass = typeof(AbstractRoad);
        _mapObjectResourceList = new MapObjectResourceList(this);
        _mapObjectResourceList.SetControlStyle(Set<ControlStyleElement>.Of(EngineServices.Stored, EngineServices.Shared));
        _resourceEditSettings = new ResourceEditSettings(this);
        _resourceEditSettings.SetControlStyle(Set<ControlStyleElement>.Of(EngineServices.Stored, EngineServices.Shared));
        _imageLibraryManager = new ImageLibraryManager(null);
        _sfxManager = new SfxManager(null);
    }

    private Set<HexSpriteSetStatusElement> _status;
    private HexControl _hexControl;
    private RoadControl _roadControl;
    private GameEngine _activatedEngine;

    public GameEngine ActivatedEngine { get => _activatedEngine; }

    private MapObjectResourceList _mapObjectResourceList;
    private ResourceEditSettings _resourceEditSettings;
    private ImageLibraryManager _imageLibraryManager;
    private SfxManager _sfxManager;
    private PluginControl _pluginControl;
    private string _name = "";
    private StringList _terrainTypes;
    private StringList _overlayTypes;
    private StringList _decorationTypes;

    // HSEngine.pas:3368
    public override void Dispose()
    {
        EngineMessage message = default;
        _status = _status + Set<HexSpriteSetStatusElement>.Of(HexSpriteSetStatusElement.Destroying);
        EngineMessage.InitializeMessage(ref message, 66564, this);
        if (_activatedEngine != null && _activatedEngine.ControlNode != null)
        {
            _activatedEngine.ControlNode.MessageProc(ref message, this);
        }

        base.Dispose();
        _pluginControl?.Dispose();
        _terrainTypes?.Dispose();
        _terrainTypes = null;
        _overlayTypes?.Dispose();
        _overlayTypes = null;
        _decorationTypes?.Dispose();
        _decorationTypes = null;
        _imageLibraryManager?.Dispose();
        _imageLibraryManager = null;
        _sfxManager?.Dispose();
        _sfxManager = null;
        EngineMessage.InitializeMessage(ref message, 66565, this);
        if (_activatedEngine != null && _activatedEngine.ControlNode != null)
        {
            _activatedEngine.ControlNode.MessageProc(ref message, this);
        }

        _status = _status - Set<HexSpriteSetStatusElement>.Of(HexSpriteSetStatusElement.Destroying);
    }

    // HSEngine.pas:3410
    public override void ReadWrite(EStorageStream stream)
    {
        SetEngine(stream.Engine);
        _pluginControl.Engine = stream.Engine;
        base.ReadWrite(stream);
        stream.rwString(ref _name, 4);
        stream.rwEObject(_mapObjectResourceList, 5);
        stream.rwEObject(_resourceEditSettings, 6);
        stream.rwEObject(_pluginControl, 7);
    }

    // HSEngine.pas:3405
    public override void Clear()
    {
        base.Clear();
    }

    // HSEngine.pas:3399
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 536870920)
        {
            _activatedEngine = message.Payload.As<GameEngine>();
        }
    }

    // HSEngine.pas:3421
    public void Activate(bool edit)
    {
        EngineMessage message = default;
        EngineMessage.InitializeMessage(ref message, 66560, this);
        GetEngine().ControlNode.MessageProc(ref message, this);
        _status = _status + Set<HexSpriteSetStatusElement>.Of(HexSpriteSetStatusElement.Activated);
        if (edit)
        {
            EngineMessage.InitializeMessage(ref message, 66562, this);
            GetEngine().ControlNode.MessageProc(ref message, this);
            _status = _status + Set<HexSpriteSetStatusElement>.Of(HexSpriteSetStatusElement.Editting);
        }
    }

    // HSEngine.pas:3436
    public void DeActivate()
    {
        EngineMessage message = default;
        if (_status.Contains(HexSpriteSetStatusElement.Activated))
        {
            EngineMessage.InitializeMessage(ref message, 66561, this);
            GetEngine().ControlNode.MessageProc(ref message, this);
            _status = _status - Set<HexSpriteSetStatusElement>.Of(HexSpriteSetStatusElement.Activated);
            if (_status.Contains(HexSpriteSetStatusElement.Editting))
            {
                EngineMessage.InitializeMessage(ref message, 66563, this);
                GetEngine().ControlNode.MessageProc(ref message, this);
                _status = _status - Set<HexSpriteSetStatusElement>.Of(HexSpriteSetStatusElement.Editting);
            }
        }
    }

    // HSEngine.pas:3454
    public void SetModified()
    {
        if (_status.Contains(HexSpriteSetStatusElement.Editting))
        {
            _status = _status + Set<HexSpriteSetStatusElement>.Of(HexSpriteSetStatusElement.Modified);
        }
    }

    // HSEngine.pas:3459
    public void ResetModified()
    {
        _status = _status - Set<HexSpriteSetStatusElement>.Of(HexSpriteSetStatusElement.Modified);
    }

    // HSEngine.pas:3464
    public void SetEditting()
    {
        _status = _status + Set<HexSpriteSetStatusElement>.Of(HexSpriteSetStatusElement.Editting);
    }

    // HSEngine.pas:3468
    public void ResetEditting()
    {
        _status = _status - Set<HexSpriteSetStatusElement>.Of(HexSpriteSetStatusElement.Editting);
    }

    public Set<HexSpriteSetStatusElement> Status { get => _status; }
    public HexControl HexControl { get => _hexControl; }
    public RoadControl RoadControl { get => _roadControl; }
    public MapObjectResourceList MapObjectResourceList { get => _mapObjectResourceList; }
    public ResourceEditSettings ResourceEditSettings { get => _resourceEditSettings; }
    public ImageLibraryManager ImageLibraryManager { get => _imageLibraryManager; }
    public SfxManager SfxManager { get => _sfxManager; }
    public PluginControl PluginControl { get => _pluginControl; }
    public string Name { get => _name; set => _name = value; }
    public StringList TerrainTypes { get => _terrainTypes; }
    public StringList OverlayTypes { get => _overlayTypes; }
    public StringList DecorationTypes { get => _decorationTypes; }

}
