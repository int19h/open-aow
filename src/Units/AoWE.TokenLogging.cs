using Aow.Rtl;
using Aow.Units.Engine;
using Aow.Units.GFXE;
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
        if (FTokenEventLogging || AoWEUnit.AoWHSMap.Status.Contains(THSMapStatusElement.msEditting)) return;
        try
        {
            try { tokenLog = RtlUnit.Files.Open(AoWEUnit.AoWEngine.StartupDirectory + "Debug\\SYNC_LOG" + RtlUnit.IntToStr(GFXEUnit.GetElapsedMilliSeconds()) + ".TXT", RtlUnit.fmCreate); }
            catch (IOException) { }
            catch (UnauthorizedAccessException) { }
            WriteTokenLog("Map Session: " + FMapName);
            int player = AoWEUnit.AoWHSMap.FPlayers.FLocalHumanPlayers[0];
            WriteTokenLog("Local Player: " + RtlUnit.IntToStr(player));
            WriteTokenLog("Local Race: " + AoWEUnit.TranslateRStr(AoWEUnit.RaceNamesPluralRStr[AoWEUnit.AoWHSMap.FPlayers.GetPlayers((sbyte)player).FPlayerRace]));
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
            WriteTokenLog(ClassNameOf(token.GetType()) + " (P:" + RtlUnit.IntToStr(FTokenManager.TokenExecuter.ActiveTCIndex) +
                "/T:" + RtlUnit.IntToStr(token.TokenCounter) + "/S:" + RtlUnit.IntToStr(FDefendersSeed) +
                "/D:" + RtlUnit.IntToStr(AoWEUnit.AoWHSMap.FDay) + "/AIPC:" + RtlUnit.IntToStr(AoWEUnit.AoWHSMap.FAIExecuter.FProcessCycles), true);
    }

    public void OnTokenManagerMessage(TObject sender, string message)
    {
        if (FTokenEventLogging) WriteTokenLog(message);
        AoWEUnit.AoWEngine.OutputDebugStr(message);
    }

    public void OnStreamError(TEStorageStream stream, string message, ref bool handled)
    {
        if (FIgnoreStreamErrors) return;
        int answer = (RtlUnit.Dialogs ?? throw new EInvalidOperation(message)).Show(new(message + " Ignore failure?", "Error", 0, 7));
        handled = answer is 6 or 8;
        FIgnoreStreamErrors = answer == 8;
    }
}
