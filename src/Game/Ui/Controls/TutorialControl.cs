// Translated from AoWE.pas.
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Events;

using Aow.Game.Session;

using Aow.Game.World.Map;

namespace Aow.Game.Ui.Controls;

// list of shown message IDs
public partial class TutorialControl : EObject
{
    // AoWHSMap.PAS:1
    public TutorialControl(ECustomNode owner) : base(owner)
    {
        FShownMessages = new IntegerList();
    }

    public IntegerList FShownMessages;

    // AoWHSMap.PAS:6
    public override void Dispose()
    {
        FShownMessages?.Dispose();
        base.Dispose();
    }

    // AoWHSMap.PAS:11
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwStreamProc(20, FShownMessages.LoadFromStream, FShownMessages.SaveToStream);
    }

    // AoWHSMap.PAS:16
    public void ShowMessage(int ID, bool Immediate, bool OnceOnly)
    {
        TutorialMessageEventLog Log;
        if (AoWHSMap.Current.FGameType == GameType.Tutorial && AoWHSMap.Current.FTextControl.FindID(ID) != null)
        {
            if (OnceOnly && FShownMessages.IndexOf(ID) != -1)
            {
                return;
            }

            Log = new TutorialMessageEventLog(null);
            var id = AoWHSMap.Current.FTextControl.FindID(ID);
            Log.FMessage.Assign(id.FText);
            AoWHSMap.Current.TriggerExecuteEventLog(Log, Immediate, null);
            Log.Release();
            FShownMessages.Add(ID);
        }
    }

    // intf_P1_05.inc:187

}
