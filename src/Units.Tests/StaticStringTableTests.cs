using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Aow.Units.Tests;

// The static string tables are [InlineArray] structs whose element field skips every initializer.
// After the unit initialization of a fresh process no element, no string array item and no static
// string field of the units holds null (strings.md 2.2).
public sealed class StaticStringTableTests
{
    [Fact]
    public async Task EveryStaticStringTableElementIsNonNullAfterUnitInitialization()
    {
        // Unit initialization runs once per process, so the check starts a fresh one.
        if (Environment.GetEnvironmentVariable("AOW_STATIC_TABLES_ISOLATED") is null)
        {
            var start = new ProcessStartInfo("dotnet") { UseShellExecute = false, CreateNoWindow = true, RedirectStandardOutput = true, RedirectStandardError = true };
            start.ArgumentList.Add("vstest");
            start.ArgumentList.Add(typeof(StaticStringTableTests).Assembly.Location);
            start.ArgumentList.Add("/TestCaseFilter:FullyQualifiedName~StaticStringTableTests");
            start.Environment["AOW_STATIC_TABLES_ISOLATED"] = "1";
            using var process = Process.Start(start)!;
            var stdout = process.StandardOutput.ReadToEndAsync();
            var stderr = process.StandardError.ReadToEndAsync();
            using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(60));
            try { await process.WaitForExitAsync(timeout.Token); }
            catch (OperationCanceledException) { process.Kill(entireProcessTree: true); throw; }
            Assert.True(process.ExitCode == 0, await stdout + await stderr);
            return;
        }
        using var host = new EngineFixture.RecordingHost("fixture");
        Runtime.InstallHost(host);
        Aow.Units.Sound.NullSoundSystem.Install();
        Startup.InitializeAll();
        var nulls = new List<string>();
        int tables = 0;
        foreach (var type in typeof(Startup).Assembly.GetTypes())
            foreach (var field in type.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
            {
                if (field.Name == "captureOrdinal") continue;
                if (field.IsLiteral || field.FieldType.ContainsGenericParameters || type.ContainsGenericParameters) continue;
                string name = type.FullName + "." + field.Name;
                if (field.FieldType == typeof(string))
                {
                    if (field.GetValue(null) is null) nulls.Add(name);
                }
                else if (field.FieldType == typeof(string[]))
                {
                    if (field.GetValue(null) is string[] items)
                        for (int i = 0; i < items.Length; i++) if (items[i] is null) nulls.Add($"{name}[{i}]");
                }
                else if (field.FieldType.GetCustomAttribute<InlineArrayAttribute>() is { } inline
                    && field.FieldType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Single().FieldType == typeof(string))
                {
                    tables++;
                    var items = (string[])typeof(StaticStringTableTests).GetMethod(nameof(Elements), BindingFlags.Static | BindingFlags.NonPublic)!
                        .MakeGenericMethod(field.FieldType).Invoke(null, [field.GetValue(null), inline.Length])!;
                    for (int i = 0; i < items.Length; i++) if (items[i] is null) nulls.Add($"{name}[{i}]");
                }
            }
        Assert.True(tables >= 30, $"{tables} static string tables");
        Assert.Empty(nulls);
    }

    private static string[] Elements<T>(T table, int length) where T : struct
        => MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<T, string>(ref table), length).ToArray();
}
