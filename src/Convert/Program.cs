using System.Text.Json;
using Aow.Formats;
using Aow.Rtl;

namespace Aow.Convert;

/// <summary>
/// aowconv (own-formats.md 3.2). Exit codes: 0 complete success, 1 comparison difference,
/// 2 error, 3 successful explicit lossy conversion. Errors outrank differences, which outrank
/// losses.
/// </summary>
public static class Program
{
    public const int Success = 0;
    public const int Difference = 1;
    public const int Error = 2;
    public const int Lossy = 3;

    public static int Main(string[] args)
    {
        ProcessSafety.Initialize();
        return Run(args, Console.Out, Console.Error);
    }

    public static int Run(string[] args, TextWriter output, TextWriter errors)
    {
        try
        {
            var options = Options.Parse(args);
            return options.Command switch
            {
                "to-own" => Commands.ToOwn(options, output, errors),
                "to-original" => Commands.ToOriginal(options, output, errors),
                "normalize" => Commands.Normalize(options, output, errors),
                "check" => new Checker(output, errors).Run(options),
                "info" => Commands.Info(options, output, errors),
                _ => Usage(errors, $"Unknown command \"{options.Command}\"."),
            };
        }
        catch (UsageException error)
        {
            return Usage(errors, error.Message);
        }
        catch (System.Exception error) when (error is IOException or UnauthorizedAccessException or JsonException)
        {
            errors.WriteLine($"{Codes.Output}: {error.Message}");
            return Error;
        }
    }

    private static int Usage(TextWriter errors, string message)
    {
        errors.WriteLine(message);
        errors.WriteLine("""
            Usage:
              aowconv to-own <in> <out> [--kind <kind>] [--profile <profile>] [--overwrite]
              aowconv to-original <in> <out> [--exact-layout] [--allow-lossy] [--overwrite]
              aowconv to-own --dir <in> --out <dir> [--profiles <manifest>] [--overwrite]
              aowconv to-original --dir <in> --out <dir> [--allow-lossy] [--overwrite]
              aowconv normalize <in> <out> [--overwrite]
              aowconv check <file or directory>... [--expect <census.json>]
              aowconv info <file> [--kind <kind>] [--profile <profile>]
            """);
        return Error;
    }
}

public sealed class UsageException(string message) : System.Exception(message);

/// <summary>The parsed command line.</summary>
public sealed class Options
{
    public string Command { get; private set; } = "";
    public List<string> Positional { get; } = [];
    public string? Kind { get; private set; }
    public string? Profile { get; private set; }
    public bool Overwrite { get; private set; }
    public bool ExactLayout { get; private set; }
    public bool AllowLossy { get; private set; }
    public string? Dir { get; private set; }
    public string? Out { get; private set; }
    public string? Profiles { get; private set; }
    public string? Expect { get; private set; }

    public static Options Parse(string[] args)
    {
        if (args.Length == 0) throw new UsageException("No command.");
        var o = new Options { Command = args[0] };
        for (int i = 1; i < args.Length; i++)
        {
            string a = args[i];
            string Value() => i + 1 < args.Length ? args[++i] : throw new UsageException($"{a} needs a value.");
            switch (a)
            {
                case "--kind": o.Kind = Value(); break;
                case "--profile": o.Profile = Value(); break;
                case "--overwrite": o.Overwrite = true; break;
                case "--exact-layout": o.ExactLayout = true; break;
                case "--allow-lossy": o.AllowLossy = true; break;
                case "--dir": o.Dir = Value(); break;
                case "--out": o.Out = Value(); break;
                case "--profiles": o.Profiles = Value(); break;
                case "--expect": o.Expect = Value(); break;
                default:
                    if (a.StartsWith("--", StringComparison.Ordinal)) throw new UsageException($"Unknown option {a}.");
                    o.Positional.Add(a);
                    break;
            }
        }
        if (o.ExactLayout && o.AllowLossy) throw new UsageException("--exact-layout conflicts with --allow-lossy.");
        return o;
    }
}
