// Translated from AoWE.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Events;

using Aow.Game.Session;

using Aow.Game.World.Map;

namespace Aow.Game.UI.Controls;

// list of shown message IDs
public partial class TutorialControl : EngineObject
{
    // AoWHSMap.PAS:1
    public TutorialControl(ECustomNode owner) : base(owner)
    {
        _shownMessages = new Int32List();
    }

    private Int32List _shownMessages;

    // AoWHSMap.PAS:6
    public override void Dispose()
    {
        _shownMessages?.Dispose();
        base.Dispose();
    }

    // AoWHSMap.PAS:11
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Payload(20, _shownMessages.LoadFromStream, _shownMessages.SaveToStream);
    }

    // AoWHSMap.PAS:16
    public void ShowMessage(int id, bool immediate, bool onceOnly)
    {
        TutorialMessageEventLog log;
        if (StrategicMap.Current.GameType == GameType.Tutorial && StrategicMap.Current.TextControl.FindId(id) != null)
        {
            if (onceOnly && _shownMessages.IndexOf(id) != -1)
            {
                return;
            }

            log = new TutorialMessageEventLog(null);
            var idLocal = StrategicMap.Current.TextControl.FindId(id);
            log.Message.Assign(idLocal.Text);
            StrategicMap.Current.TriggerExecuteEventLog(log, immediate, null);
            log.Release();
            _shownMessages.Add(id);
        }
    }

    // intf_P1_05.inc:187

}
