#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;

using Aow.Formats.Tree;

using Aow.Game.Events;

using Aow.Game.Session;

using Aow.Game.World.Map;

namespace Aow.Game.UI.Controls;

// list of shown message IDs
public sealed class TutorialControl(CustomNode? owner) : EngineObject(owner)
{

    private readonly Int32List _shownMessages = [];

    // AoWHSMap.PAS:6
    public override void Dispose() => base.Dispose();

    // AoWHSMap.PAS:11
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        persister.Int32List(name: "ShownMessages", list: _shownMessages);
    }

    // AoWHSMap.PAS:16
    public void ShowMessage(int id, bool immediate, bool onceOnly)
    {
        StrategicMap strategicMap = GetEngine().StrategicMap;
        if (strategicMap.GameType == GameType.Tutorial && strategicMap.TextControl.FindId(id) is not null)
        {
            if (onceOnly && _shownMessages.IndexOf(id) != -1)
            {
                return;
            }

            TutorialMessageEventLog log = new(null, holder: CreationHolder());
            TextControlItem? idLocal = GetEngine().StrategicMap.TextControl.FindId(id);
            log.Message.Assign(idLocal.Text);
            GetEngine().StrategicMap.TriggerExecuteEventLog(log, immediate, done: null);
            _ = log.Release();
            _ = _shownMessages.Add(id);
        }
    }
}
