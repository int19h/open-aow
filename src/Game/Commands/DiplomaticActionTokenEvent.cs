#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Net;
using Aow.Game.Session;

namespace Aow.Game.Commands;

// Operation, Action
public class DiplomaticActionTokenEvent(CustomNode owner) : TokenEvent(owner)
{

    // Diplomacy.pas:1125
    public new const int ClassId = 135432;

    public int Operation { get; set; }
    public DiplomaticAction Action { get; set; }

    // Diplomacy.pas:1117
    public override void Dispose()
    {
        base.Dispose();
        DiplomaticAction action = Action;
        _ = (action?.Release());
    }

    // Diplomacy.pas:1140
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Action = persister.Created("Action", objectValue: Action);
        Operation = persister.Int32("Operation", value: Operation);
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
                if (Action.CanAccept(errorText))
                {
                    Action.ExecuteAccept(this);
                    Action.UpdateAIDiplomaticActionsLog(Action.Target);
                }

                break;
            case 2:
                if (Action.CanReject(errorText))
                {
                    Action.ExecuteReject(this);
                    Action.UpdateAIDiplomaticActionsLog(Action.Target);
                }

                break;
            default:
                break;
        }

        return ExecuteResult.Done;
    }

    // Diplomacy.pas:1128
    public void SetDiplomaticAction(DiplomaticAction action)
    {
        DiplomaticAction old = Action;
        if (old != action)
        {
            _ = (old?.Release());

            Action = action;
            _ = (Action?.AddRef());
        }
    }
}
