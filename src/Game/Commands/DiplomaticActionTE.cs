// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Net;

using Aow.Game.Session;

namespace Aow.Game.Commands;

// Operation, Action
public partial class DiplomaticActionTE : TokenEvent
{
    public DiplomaticActionTE(ECustomNode owner) : base(owner)
    { }

    // Diplomacy.pas:1125
    public new const int ClassID = 135432;

    public int FOperation;
    public DiplomaticAction FAction;

    // Diplomacy.pas:1117
    public override void Dispose()
    {
        DiplomaticAction Action;
        base.Dispose();
        Action = FAction;
        if (Action != null)
        {
            Action.Release();
        }
    }

    // Diplomacy.pas:1140
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwCreateEObject(ref FAction, null, 20);
        Stream.rwInteger(ref FOperation, 21);
    }

    // Diplomacy.pas:1147
    public override ExecuteResult Execute(TokenExecuter TE)
    {
        string ErrorText = "";
        switch (FOperation)
        {
            case 0:
                if (FAction.CanSend(ref ErrorText))
                {
                    FAction.ExecuteSend(this);
                    FAction.UpdateAIDiplomaticActionsLog(FAction.FSource);
                }

                break;
            case 1:
                if (FAction.CanAccept(ref ErrorText))
                {
                    FAction.ExecuteAccept(this);
                    FAction.UpdateAIDiplomaticActionsLog(FAction.FTarget);
                }

                break;
            case 2:
                if (FAction.CanReject(ref ErrorText))
                {
                    FAction.ExecuteReject(this);
                    FAction.UpdateAIDiplomaticActionsLog(FAction.FTarget);
                }

                break;
        }

        return ExecuteResult.Done;
    }

    // Diplomacy.pas:1128
    public void SetDiplomaticAction(DiplomaticAction Action)
    {
        DiplomaticAction Old;
        Old = FAction;
        if (Old != Action)
        {
            if (Old != null)
            {
                Old.Release();
            }

            FAction = Action;
            if (FAction != null)
            {
                FAction.AddRef();
            }
        }
    }

    // NetworkE.pas:230

}
