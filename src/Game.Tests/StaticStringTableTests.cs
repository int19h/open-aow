using System.Collections.Immutable;
using System.Diagnostics;
using System.Reflection;

using Aow.Game.Audio;

namespace Aow.Game.Tests;

// The static string tables are string arrays and immutable string arrays. After the unit
// initialization of a fresh process no table item and no static string field of the units holds
// null (strings.md 2.2).
public sealed class StaticStringTableTests
{
    [Fact]
    public async Task EveryStaticStringTableElementIsNonNullAfterUnitInitialization()
    {
        // Unit initialization runs once per process, so the check starts a fresh one.
        if (Environment.GetEnvironmentVariable("AOW_STATIC_TABLES_ISOLATED") is null)
        {
            ProcessStartInfo start = new("dotnet") { UseShellExecute = false, CreateNoWindow = true, RedirectStandardOutput = true, RedirectStandardError = true };
            start.ArgumentList.Add("vstest");
            start.ArgumentList.Add(typeof(StaticStringTableTests).Assembly.Location);
            start.ArgumentList.Add("/TestCaseFilter:FullyQualifiedName~StaticStringTableTests");
            start.Environment["AOW_STATIC_TABLES_ISOLATED"] = "1";
            using Process process = Process.Start(start)!;
            Task<string> stdout = process.StandardOutput.ReadToEndAsync();
            Task<string> stderr = process.StandardError.ReadToEndAsync();
            using CancellationTokenSource timeout = new(TimeSpan.FromSeconds(60));
            try
            {
                await process.WaitForExitAsync(timeout.Token);
            }
            catch (OperationCanceledException)
            {
                process.Kill(entireProcessTree: true);
                throw;
            }
            Assert.True(process.ExitCode == 0, userMessage: await stdout + await stderr);
            return;
        }
        using EngineFixture.RecordingHost host = new("fixture");
        RtlHost.InstallHost(host);
        NullSoundSystem.Install();
        Startup.InitializeAll();
        List<string> nulls = [];
        int tables = 0;
        foreach (Type type in typeof(Startup).Assembly.GetTypes())
        {
            foreach (FieldInfo field in type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
            {
                if (field.IsLiteral || field.FieldType.ContainsGenericParameters || type.ContainsGenericParameters)
                {
                    continue;
                }

                string name = $"{type.FullName}.{field.Name}";
                if (field.FieldType == typeof(string))
                {
                    if (field.GetValue(null) is null)
                    {
                        nulls.Add(name);
                    }
                }
                else if (field.FieldType == typeof(string[]) || field.FieldType == typeof(ImmutableArray<string>))
                {
                    tables++;
                    IReadOnlyList<string>? items = field.GetValue(null) switch
                    {
                        string[] array => array,
                        ImmutableArray<string> { IsDefault: false } immutable => immutable,
                        _ => null,
                    };
                    if (items is null)
                    {
                        nulls.Add(name);
                    }
                    else
                    {
                        for (int i = 0; i < items.Count; i++)
                        {
                            if (items[i] is null)
                            {
                                nulls.Add($"{name}[{i}]");
                            }
                        }
                    }
                }
            }
        }

        Assert.True(tables >= 24, userMessage: $"{tables} static string tables");
        Assert.Empty(nulls);
    }
}
