#nullable disable
using System.Globalization;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Magic.Abilities;
using Aow.Rtl;

namespace Aow.Game.World.Map;

public sealed partial class StrategicMap
{
    private Stream _tokenLog;

    // Delphi I- suppresses file errors on these diagnostic writes.
    private void WriteTokenLog(string text, bool flush = false)
    {
        try
        {
            if (_tokenLog is null)
            {
                return;
            }

            _tokenLog.Write(Cp1252.Encode($"{text}\r\n"));
            if (flush)
            {
                _tokenLog.Flush();
            }
        }
        catch (IOException) { }
    }

    public void StartTokenEventLogging()
    {
        if (TokenEventLogging || Current.Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            return;
        }

        try
        {
            try
            {
                _tokenLog = RtlHost.Files.Open($"{GetEngine().StartupDirectory}Debug\\SYNC_LOG{PixelFormats.ElapsedMilliseconds.ToString(CultureInfo.InvariantCulture)}.TXT", mode: FileOpenMode.Create);
            }
            catch (IOException) { }
            catch (UnauthorizedAccessException) { }
            WriteTokenLog($"Map Session: {MapName}");
            int player = Current.Players.LocalHumanPlayers[0];
            WriteTokenLog($"Local Player: {player.ToString(CultureInfo.InvariantCulture)}");
            WriteTokenLog($"Local Race: {AbilityRules.TranslateResourceString(GetEngine(), S: GameTables.RaceNamesPluralTexts[Current.Players.GetPlayers((sbyte)player).PlayerRace])}");
            TokenEventLogging = true;
        }
        catch { }
    }

    public void StopTokenEventLogging()
    {
        if (!TokenEventLogging)
        {
            return;
        }

        try
        {
            _tokenLog?.Dispose();
        }
        catch (IOException) { }
        _tokenLog = null;
        TokenEventLogging = false;
    }

    public void OnExecuteTokenEvent(TokenEvent token)
    {
        if (token.EngineClass.ClassId != PassTokenTokenEvent.ClassId)
        {
            SetModified();
        }

        if (TokenEventLogging && TokenManager.TokenExecutor.ExecuteCount == 0)
        {
            WriteTokenLog($"{token.GetType().Name} (P:{TokenManager.TokenExecutor.ActiveTacticalBattleIndex.ToString(CultureInfo.InvariantCulture)}/T:{token.TokenCounter.ToString(CultureInfo.InvariantCulture)}/S:{DefendersSeed.ToString(CultureInfo.InvariantCulture)}/D:{Current.Day.ToString(CultureInfo.InvariantCulture)}/AIPC:{Current.AIExecutor.ProcessCycles.ToString(CultureInfo.InvariantCulture)}", flush: true);
        }
    }

    public void OnTokenManagerMessage(object sender, string message)
    {
        if (TokenEventLogging)
        {
            WriteTokenLog(message);
        }

        GetEngine().OutputDebugString(message);
    }

    public void OnStreamError(string message, ref bool handled)
    {
        if (IgnoreStreamErrors)
        {
            return;
        }

        int answer = (RtlHost.Dialogs ?? throw new InvalidOperationException(message)).Show(new($"{message} Ignore failure?", Caption: "Error", Flags: 0, DefaultResult: 7));
        handled = answer is 6 or 8;
        IgnoreStreamErrors = answer == 8;
    }
}
