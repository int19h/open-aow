namespace Aow.Rtl.Host;

/// <summary>
/// The switches a host reads at entry (strings.md sections 2.2 and 4). <c>AOW_STRICT_STRINGS=1</c> turns
/// on <see cref="DelphiStrings.StrictStrings"/>, and <c>AOW_EXCEPTION_CENSUS</c> starts the exception census.
/// <c>AOW_EMULATE_ORIGINAL_BUGS=1</c> turns on <see cref="EmulateOriginalBugs"/>, which only the
/// hosts that compare the port with the original set (deviations.md).
/// The game hosts also stop at start when the process runs with culture data, because text
/// comparisons and number formats follow the invariant culture only.
/// </summary>
public static class GateSwitches
{
    public const string StrictStringsVariable = "AOW_STRICT_STRINGS";
    public const string InvariantSwitch = "System.Globalization.Invariant";
    public const string EmulateOriginalBugsVariable = "AOW_EMULATE_ORIGINAL_BUGS";

    /// <summary>
    /// True when the host emulates the original's bugs that the port fixes, so that a comparison
    /// with the original's results holds (deviations.md). Off by default. The loads of a host take
    /// it as the default of their load context. A host's entry sets it once.
    /// </summary>
    public static bool EmulateOriginalBugs { get; private set; }

    /// <summary>True when the runtime configuration turns on invariant globalization.</summary>
    public static bool InvariantGlobalization => AppContext.TryGetSwitch(InvariantSwitch, isEnabled: out bool on) && on;

    /// <summary>The message a game host prints before it stops, or null when the switch is on.</summary>
    public static string? CultureError(string host) => InvariantGlobalization ? null :
        $"{host} stops: {InvariantSwitch} is off. The port runs with invariant globalization only. Build it with <InvariantGlobalization>true</InvariantGlobalization> (Directory.Build.props) or set \"System.Globalization.Invariant\": true in its runtimeconfig.json.";

    /// <summary>Starts the census and reads the strict switch. Call it first thing in a host.</summary>
    public static void Enter(string host, string[] arguments)
    {
        ExceptionCensus.Start(host, arguments);
        if (Environment.GetEnvironmentVariable(StrictStringsVariable) == "1")
        {
            DelphiStrings.StrictStrings = true;
        }

        EmulateOriginalBugs = Environment.GetEnvironmentVariable(EmulateOriginalBugsVariable) == "1";
    }
}
