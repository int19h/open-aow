// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Net;

using Aow.Game.Session;

namespace Aow.Game.Commands;

// Operation, Action
public partial class DiplomaticActionTokenEvent : TokenEvent
{
    public DiplomaticActionTokenEvent(ECustomNode owner) : base(owner)
    { }

    // Diplomacy.pas:1125
    public new const int ClassId = 135432;

    public int Operation;
    public DiplomaticAction Action;

    // Diplomacy.pas:1117
    public override void Dispose()
    {
        DiplomaticAction action;
        base.Dispose();
        action = Action;
        if (action != null)
        {
            action.Release();
        }
    }

    // Diplomacy.pas:1140
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwCreateEObject(ref Action, null, 20);
        stream.rwInteger(ref Operation, 21);
    }

    // Diplomacy.pas:1147
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        string errorText = "";
        switch (Operation)
        {
            case 0:
                if (Action.CanSend(ref errorText))
                {
                    Action.ExecuteSend(this);
                    Action.UpdateAIDiplomaticActionsLog(Action.Source);
                }

                break;
            case 1:
                if (Action.CanAccept(ref errorText))
                {
                    Action.ExecuteAccept(this);
                    Action.UpdateAIDiplomaticActionsLog(Action.Target);
                }

                break;
            case 2:
                if (Action.CanReject(ref errorText))
                {
                    Action.ExecuteReject(this);
                    Action.UpdateAIDiplomaticActionsLog(Action.Target);
                }

                break;
        }

        return ExecuteResult.Done;
    }

    // Diplomacy.pas:1128
    public void SetDiplomaticAction(DiplomaticAction action)
    {
        DiplomaticAction old;
        old = Action;
        if (old != action)
        {
            if (old != null)
            {
                old.Release();
            }

            Action = action;
            if (Action != null)
            {
                Action.AddRef();
            }
        }
    }

    // NetworkE.pas:230

}
