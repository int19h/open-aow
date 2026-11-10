using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Engine.Storage;
using Aow.Game.Magic.Abilities;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.World.Map;

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
        if (FTokenEventLogging || Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Status.Contains(THSMapStatusElement.msEditting)) return;
        try
        {
            try { tokenLog = RtlUnit.Files.Open(Aow.Game.Magic.Abilities.GameTables.AoWEngine.StartupDirectory + "Debug\\SYNC_LOG" + RtlUnit.IntToStr(Aow.Game.Draw.PixelFormats.GetElapsedMilliSeconds()) + ".TXT", RtlUnit.fmCreate); }
            catch (IOException) { }
            catch (UnauthorizedAccessException) { }
            WriteTokenLog("Map Session: " + FMapName);
            int player = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.FLocalHumanPlayers[0];
            WriteTokenLog("Local Player: " + RtlUnit.IntToStr(player));
            WriteTokenLog("Local Race: " + Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameTables.RaceNamesPluralRStr[Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers((sbyte)player).FPlayerRace]));
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
            WriteTokenLog(token.GetType().Name + " (P:" + RtlUnit.IntToStr(FTokenManager.TokenExecuter.ActiveTCIndex) +
                "/T:" + RtlUnit.IntToStr(token.TokenCounter) + "/S:" + RtlUnit.IntToStr(FDefendersSeed) +
                "/D:" + RtlUnit.IntToStr(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FDay) + "/AIPC:" + RtlUnit.IntToStr(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FAIExecuter.FProcessCycles), true);
    }

    public void OnTokenManagerMessage(object Sender, string message)
    {
        if (FTokenEventLogging) WriteTokenLog(message);
        Aow.Game.Magic.Abilities.GameTables.AoWEngine.OutputDebugStr(message);
    }

    public void OnStreamError(TEStorageStream stream, string message, ref bool handled)
    {
        if (FIgnoreStreamErrors) return;
        int answer = (RtlUnit.Dialogs ?? throw new InvalidOperationException(message)).Show(new(message + " Ignore failure?", "Error", 0, 7));
        handled = answer is 6 or 8;
        FIgnoreStreamErrors = answer == 8;
    }
}
