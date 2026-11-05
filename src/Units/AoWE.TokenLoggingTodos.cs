using Aow.Rtl;
using Aow.Units.Engine;
using Aow.Units.HSEngine;
using Aow.Units.NetworkE;

namespace Aow.Units.AoWE;

public partial class TAoWHSMap
{
    private Stream tokenLog;

    // Delphi I- suppresses file errors on these diagnostic writes.
    private void WriteTokenLog(string text, bool flush = false)
    {
        try
        {
            if (tokenLog is null) return;
            tokenLog.Write(Cp1252.Encode(text + "\r\n"));
            if (flush) tokenLog.Flush();
        }
        catch (IOException) { }
    }

    public void StartTokenEventLogging()
    {
        if (FTokenEventLogging || AoWE.AoWHSMap.Status.Contains(THSMapStatusElement.msEditting)) return;
        try
        {
            try { tokenLog = Rtl.Rtl.Files.Open(AoWE.AoWEngine.StartupDirectory + "Debug\\SYNC_LOG" + Rtl.Rtl.IntToStr(GFXE.GFXE.GetElapsedMilliSeconds()) + ".TXT", Rtl.Rtl.fmCreate); }
            catch (IOException) { }
            catch (UnauthorizedAccessException) { }
            WriteTokenLog("Map Session: " + FMapName);
            int player = AoWE.AoWHSMap.FPlayers.FLocalHumanPlayers[0];
            WriteTokenLog("Local Player: " + Rtl.Rtl.IntToStr(player));
            WriteTokenLog("Local Race: " + AoWE.TranslateRStr(AoWE.RaceNamesPluralRStr[AoWE.AoWHSMap.FPlayers.GetPlayers((sbyte)player).FPlayerRace]));
            FTokenEventLogging = true;
        }
        catch { }
    }

    public void StopTokenEventLogging()
    {
        if (!FTokenEventLogging) return;
        try { tokenLog?.Dispose(); }
        catch (IOException) { }
        tokenLog = null;
        FTokenEventLogging = false;
    }

    public void OnExecuteTokenEvent(TTokenEvent token)
    {
        if (token.EClass.ClassID != 0x554) SetModified();
        if (FTokenEventLogging && FTokenManager.TokenExecuter.ExecuteCount == 0)
            WriteTokenLog(ClassNameOf(token.GetType()) + " (P:" + Rtl.Rtl.IntToStr(FTokenManager.TokenExecuter.ActiveTCIndex) +
                "/T:" + Rtl.Rtl.IntToStr(token.TokenCounter) + "/S:" + Rtl.Rtl.IntToStr(FDefendersSeed) +
                "/D:" + Rtl.Rtl.IntToStr(AoWE.AoWHSMap.FDay) + "/AIPC:" + Rtl.Rtl.IntToStr(AoWE.AoWHSMap.FAIExecuter.FProcessCycles), true);
    }

    public void OnTokenManagerMessage(TObject sender, string message)
    {
        if (FTokenEventLogging) WriteTokenLog(message);
        AoWE.AoWEngine.OutputDebugStr(message);
    }

    public void OnStreamError(TEStorageStream stream, string message, ref bool handled)
    {
        if (FIgnoreStreamErrors) return;
        int answer = (Rtl.Rtl.Dialogs ?? throw new EInvalidOperation(message)).Show(new(message + " Ignore failure?", "Error", 0, 7));
        handled = answer is 6 or 8;
        FIgnoreStreamErrors = answer == 8;
    }
}
