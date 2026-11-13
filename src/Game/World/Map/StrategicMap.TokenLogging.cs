using Aow.Engine.Storage;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.World.Map;

public partial class StrategicMap
{
    private Stream _tokenLog;

    // Delphi I- suppresses file errors on these diagnostic writes.
    private void WriteTokenLog(string text, bool flush = false)
    {
        try
        {
            if (_tokenLog is null) return;
            _tokenLog.Write(Cp1252.Encode(text + "\r\n"));
            if (flush) _tokenLog.Flush();
        }
        catch (IOException) { }
    }

    public void StartTokenEventLogging()
    {
        if (TokenEventLogging || Current.Status.Contains(HexSpriteMapStatusElement.Editting)) return;
        try
        {
            try { _tokenLog = RtlUnit.Files.Open(GetEngine().StartupDirectory + "Debug\\SYNC_LOG" + RtlUnit.IntToStr(PixelFormats.GetElapsedMilliseconds()) + ".TXT", RtlUnit.fmCreate); }
            catch (IOException) { }
            catch (UnauthorizedAccessException) { }
            WriteTokenLog("Map Session: " + MapName);
            int player = Current.Players.LocalHumanPlayers[0];
            WriteTokenLog("Local Player: " + RtlUnit.IntToStr(player));
            WriteTokenLog("Local Race: " + AbilityRules.TranslateResourceString(GetEngine(), GameTables.RaceNamesPluralTexts[Current.Players.GetPlayers((sbyte)player).PlayerRace]));
            TokenEventLogging = true;
        }
        catch { }
    }

    public void StopTokenEventLogging()
    {
        if (!TokenEventLogging) return;
        try { _tokenLog?.Dispose(); }
        catch (IOException) { }
        _tokenLog = null;
        TokenEventLogging = false;
    }

    public void OnExecuteTokenEvent(TokenEvent token)
    {
        if (token.EngineClass.ClassId != 0x554) SetModified();
        if (TokenEventLogging && TokenManager.TokenExecutor.ExecuteCount == 0)
            WriteTokenLog(token.GetType().Name + " (P:" + RtlUnit.IntToStr(TokenManager.TokenExecutor.ActiveTacticalBattleIndex) +
                "/T:" + RtlUnit.IntToStr(token.TokenCounter) + "/S:" + RtlUnit.IntToStr(DefendersSeed) +
                "/D:" + RtlUnit.IntToStr(Current.Day) + "/AIPC:" + RtlUnit.IntToStr(Current.AIExecutor.ProcessCycles), true);
    }

    public void OnTokenManagerMessage(object sender, string message)
    {
        if (TokenEventLogging) WriteTokenLog(message);
        GetEngine().OutputDebugString(message);
    }

    public void OnStreamError(StorageStream stream, string message, ref bool handled)
    {
        if (IgnoreStreamErrors) return;
        int answer = (RtlUnit.Dialogs ?? throw new InvalidOperationException(message)).Show(new(message + " Ignore failure?", "Error", 0, 7));
        handled = answer is 6 or 8;
        IgnoreStreamErrors = answer == 8;
    }
}
