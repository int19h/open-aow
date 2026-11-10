using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Engine.Storage;
using Aow.Game.Magic.Abilities;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.World.Map;

public partial class AoWHSMap
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
        if (FTokenEventLogging || Current.Status.Contains(HSMapStatusElement.Editting)) return;
        try
        {
            try { tokenLog = RtlUnit.Files.Open(GameTables.AoWEngine.StartupDirectory + "Debug\\SYNC_LOG" + RtlUnit.IntToStr(PixelFormats.GetElapsedMilliSeconds()) + ".TXT", RtlUnit.fmCreate); }
            catch (IOException) { }
            catch (UnauthorizedAccessException) { }
            WriteTokenLog("Map Session: " + FMapName);
            int player = Current.FPlayers.FLocalHumanPlayers[0];
            WriteTokenLog("Local Player: " + RtlUnit.IntToStr(player));
            WriteTokenLog("Local Race: " + AbilityRules.TranslateRStr(GameTables.RaceNamesPluralRStr[Current.FPlayers.GetPlayers((sbyte)player).FPlayerRace]));
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

    public void OnExecuteTokenEvent(TokenEvent token)
    {
        if (token.EClass.ClassID != 0x554) SetModified();
        if (FTokenEventLogging && FTokenManager.TokenExecuter.ExecuteCount == 0)
            WriteTokenLog(token.GetType().Name + " (P:" + RtlUnit.IntToStr(FTokenManager.TokenExecuter.ActiveTCIndex) +
                "/T:" + RtlUnit.IntToStr(token.TokenCounter) + "/S:" + RtlUnit.IntToStr(FDefendersSeed) +
                "/D:" + RtlUnit.IntToStr(Current.FDay) + "/AIPC:" + RtlUnit.IntToStr(Current.FAIExecuter.FProcessCycles), true);
    }

    public void OnTokenManagerMessage(object Sender, string message)
    {
        if (FTokenEventLogging) WriteTokenLog(message);
        GameTables.AoWEngine.OutputDebugStr(message);
    }

    public void OnStreamError(EStorageStream stream, string message, ref bool handled)
    {
        if (FIgnoreStreamErrors) return;
        int answer = (RtlUnit.Dialogs ?? throw new InvalidOperationException(message)).Show(new(message + " Ignore failure?", "Error", 0, 7));
        handled = answer is 6 or 8;
        FIgnoreStreamErrors = answer == 8;
    }
}
