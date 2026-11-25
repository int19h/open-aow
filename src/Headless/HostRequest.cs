using System.Globalization;

namespace Aow.Headless;

/// <summary>The play command. Persistence measurement stays on its own flag in Program.</summary>
internal sealed record HostRequest
{
    public static string DefaultRoot => Environment.GetEnvironmentVariable("AOW_GAME_ROOT") ?? @"D:\Games\GOG\Age of Wonders";

    public required string Input { get; init; }

    public required string Output { get; init; }

    public string Root { get; init; } = DefaultRoot;

    public int Seed { get; init; } = 123_456_789;

    public int Turns { get; init; }

    public string? Settings { get; init; }

    public string MovementSpeed { get; init; } = "normal";

    public bool Tactical { get; init; }

    public bool ExerciseItems { get; init; }

    public int TacticalBattles { get; init; }

    public int GlobalSpell { get; init; }

    public int Dispel { get; init; }

    public string? EndEnchantment { get; init; }

    public string? RandomTracePath { get; init; }

    public string? FileLogPath { get; init; }

    public bool TraceMovement { get; init; }

    public int AnimationFps { get; init; } = 30;

    public int ViewportWidth { get; init; } = 1_600;

    public int ViewportHeight { get; init; } = 937;

    /// <summary>The AI plays every human seat. The seat stays Human. Off, a seated human still blocks.</summary>
    public bool CpuHumans { get; init; }

    /// <summary>The next scenario node of a campaign that is between maps.</summary>
    public string? NextScenario { get; init; }

    public static HostRequest Parse(string[] arguments)
    {
        HostRequest request = new()
        {
            Input = "",
            Output = "",
        };
        for (int i = 0; i < arguments.Length; i++)
        {
            string Value() => ++i < arguments.Length ? arguments[i] : throw new ArgumentException("missing option value");
            switch (arguments[i])
            {
                case "--load":
                    request = request with { Input = Path.GetFullPath(Value()) };
                    break;
                case "--out":
                    request = request with { Output = Path.GetFullPath(Value()) };
                    break;
                case "--seed":
                    string seedText = Value();
                    long seedValue = seedText.StartsWith("0x", comparisonType: StringComparison.OrdinalIgnoreCase)
                        ? long.Parse(seedText.AsSpan(start: 2), style: NumberStyles.HexNumber, provider: CultureInfo.InvariantCulture)
                        : long.Parse(seedText, provider: CultureInfo.InvariantCulture);
                    if (seedValue is < int.MinValue or > uint.MaxValue)
                    {
                        throw new ArgumentOutOfRangeException(paramName: "seed");
                    }

                    request = request with { Seed = unchecked((int)seedValue) };
                    break;
                case "--turns":
                    request = request with { Turns = int.Parse(Value(), provider: CultureInfo.InvariantCulture) };
                    break;
                case "--game-root":
                    request = request with { Root = Path.GetFullPath(Value()) };
                    break;
                case "--settings":
                    request = request with { Settings = Value() };
                    break;
                case "--movement-speed":
                    request = request with { MovementSpeed = Value() };
                    break;
                case "--tactical":
                    request = request with { Tactical = true };
                    break;
                case "--exercise-items":
                    request = request with { ExerciseItems = true };
                    break;
                case "--tactical-battles":
                    request = request with { TacticalBattles = int.Parse(Value(), provider: CultureInfo.InvariantCulture) };
                    break;
                case "--global-spell":
                    request = request with { GlobalSpell = int.Parse(Value(), provider: CultureInfo.InvariantCulture) };
                    break;
                case "--dispel":
                    int dispel = int.Parse(Value(), provider: CultureInfo.InvariantCulture);
                    if (dispel != 51)
                    {
                        throw new ArgumentException("the dispel input casts Disjunction (spell 51)");
                    }

                    request = request with { Dispel = dispel };
                    break;
                case "--end-enchantment":
                    string endEnchantment = Value();
                    if (endEnchantment is not "remove" and not "upkeep")
                    {
                        throw new ArgumentException("--end-enchantment must be remove or upkeep");
                    }

                    request = request with { EndEnchantment = endEnchantment };
                    break;
                case "--trace-random":
                    request = request with { RandomTracePath = Path.GetFullPath(Value()) };
                    break;
                case "--trace-movement":
                    request = request with { TraceMovement = true };
                    break;
                case "--file-log":
                    request = request with { FileLogPath = Path.GetFullPath(Value()) };
                    break;
                case "--animation-fps":
                    request = request with { AnimationFps = int.Parse(Value(), provider: CultureInfo.InvariantCulture) };
                    break;
                case "--viewport":
                    string[] dimensions = Value().Split('x');
                    request = request with
                    {
                        ViewportWidth = int.Parse(dimensions[0], provider: CultureInfo.InvariantCulture),
                        ViewportHeight = int.Parse(dimensions[1], provider: CultureInfo.InvariantCulture),
                    };
                    break;
                case "--cpu-humans":
                    request = request with { CpuHumans = true };
                    break;
                case "--next-scenario":
                    string nextScenario = Value();
                    if (nextScenario.Length == 0)
                    {
                        throw new ArgumentException("--next-scenario names a scenario node");
                    }

                    request = request with { NextScenario = nextScenario };
                    break;
                default:
                    throw new ArgumentException($"unknown option: {arguments[i]}");
            }
        }

        return request.Input.Length == 0 || request.Output.Length == 0 || request.Turns < 0
            ? throw new ArgumentException("--load <save> --seed <n> --turns <n> --out <dir> [--cpu-humans] [--next-scenario <node>] [--tactical] [--global-spell <id>] [--dispel 51] [--end-enchantment remove|upkeep] [--file-log <path>]")
            : request;
    }
}
