// Translated from AoWSMTPDlg.pas.

namespace Aow.Game.UI.Dialogs;

public partial class IMSSendThread
{
    public bool FSuccess;

    // AoWSMTPDlg.pas:105
    protected internal void Execute()
    {
        try
        {
            DialogHosts.SMTPDlg.IMSSMTP.Send();
            FSuccess = true;
        }
        catch
        {
            FSuccess = false;
        }
    }

    // Classes.pas:771
    public IMSSendThread(bool CreateSuspended) : base()
    {
    }
}
