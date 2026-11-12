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
        ShownMessages = new Int32List();
    }

    public Int32List ShownMessages;

    // AoWHSMap.PAS:6
    public override void Dispose()
    {
        ShownMessages?.Dispose();
        base.Dispose();
    }

    // AoWHSMap.PAS:11
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwStreamProc(20, ShownMessages.LoadFromStream, ShownMessages.SaveToStream);
    }

    // AoWHSMap.PAS:16
    public void ShowMessage(int id, bool immediate, bool onceOnly)
    {
        TutorialMessageEventLog log;
        if (AoWHSMap.Current.GameType == GameType.Tutorial && AoWHSMap.Current.TextControl.FindId(id) != null)
        {
            if (onceOnly && ShownMessages.IndexOf(id) != -1)
            {
                return;
            }

            log = new TutorialMessageEventLog(null);
            var idLocal = AoWHSMap.Current.TextControl.FindId(id);
            log.Message.Assign(idLocal.Text);
            AoWHSMap.Current.TriggerExecuteEventLog(log, immediate, null);
            log.Release();
            ShownMessages.Add(id);
        }
    }

    // intf_P1_05.inc:187

}
