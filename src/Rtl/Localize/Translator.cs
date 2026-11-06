using System.Runtime.CompilerServices;
using Aow.Rtl.Localize;

namespace Aow.Rtl;

public sealed record TranslationTarget(string ClassName, string PropertyName, int Options = 0);

public static class TranslatorProperties
{
    internal sealed record Binding(Type Type, string Name, Func<TComponent, string[]> Read,
        Action<TComponent, string[]> Write, bool IsList);
    private static readonly Dictionary<(Type, string), Binding> bindings = [];

    public static void Register<T>(string name, Func<T, string> read, Action<T, string> write) where T : TComponent
        => bindings[(typeof(T), name)] = new(typeof(T), name, component => [read((T)component)],
            (component, values) => write((T)component, values[0]), false);

    public static void RegisterStrings<T>(string name, Func<T, TStrings?> read) where T : TComponent
    {
        bindings[(typeof(T), name)] = new(typeof(T), name, component =>
        {
            var strings = read((T)component);
            return strings is null ? [] : Enumerable.Range(0, strings.Count).Select(i => strings[i]).ToArray();
        }, (component, values) =>
        {
            var strings = read((T)component);
            if (strings is null) return;
            strings.BeginUpdate();
            try
            {
                strings.Clear();
                foreach (var value in values) strings.Add(value);
            }
            finally { strings.EndUpdate(); }
        }, true);
    }

    internal static Binding? Find(Type type, string name)
    {
        for (Type? current = type; current is not null; current = current.BaseType)
            if (bindings.TryGetValue((current, name), out var binding)) return binding;
        return null;
    }
}

public class TIvCustomTranslator : TComponent
{
    public TIvCustomTranslator(TComponent? owner = null) : base(owner) { }

    private ConditionalWeakTable<TComponent, Dictionary<string, string[]>> nativeValues = new();
    public string DictionaryName { get; set; } = "AoW";
    public IList<TranslationTarget> TargetsData { get; } = new List<TranslationTarget>();
    public MldDictionary? Dictionary { get; set; }
    public int? Language { get; set; }

    public virtual void Translate()
    {
        if (Owner is not null) Translate(Owner);
    }

    public void ClearNativeValues() => nativeValues = new();

    public void Translate(TComponent module)
    {
        var dictionary = Dictionary ?? LocalizationRuntime.Dictionary(DictionaryName, Language);
        if (Language is int language && dictionary.Language != language) dictionary.SetLanguage(language);
        string form = TObject.ClassNameOf(module.GetType());
        Visit(module);

        void Visit(TComponent component)
        {
            foreach (var target in TargetsData)
            {
                if (!Matches(component.GetType(), target.ClassName)) continue;
                string name = target.PropertyName;
                var binding = TranslatorProperties.Find(component.GetType(), name);
                if (binding is null) continue;
                var values = nativeValues.GetOrCreateValue(component);
                if (!values.TryGetValue(name, out var native))
                {
                    native = binding.Read(component);
                    values.Add(name, native);
                }
                string componentName = ReferenceEquals(component, module) ? "" : component.Name;
                if (binding.IsList)
                {
                    if (native.Length == 0) continue;
                    var joined = Join(native);
                    var translated = dictionary.TranslateContext(joined, form, componentName);
                    binding.Write(component, translated == joined ? native : Split(translated));
                }
                else binding.Write(component, [dictionary.TranslateContext(native[0], form, componentName)]);
            }
            for (int index = 0; index < component.ComponentCount; index++) Visit(component.Components[index]);
        }
    }

    private static bool Matches(Type type, string name)
    {
        if (name.Length == 0) return true;
        for (Type? current = type; current is not null; current = current.BaseType)
            if (TObject.ClassNameOf(current) == name) return true;
        return false;
    }

    private static string Join(string[] values)
    {
        string result = "";
        for (int index = 0; index < values.Length; index++)
        {
            if (index > 0) result += "<eol>";
            result += values[index];
        }
        return result;
    }

    private static string[] Split(string value)
    {
        var result = new List<string>();
        do
        {
            int marker = RtlUnit.Pos("<eol>", value), lineFeed = RtlUnit.Pos("\n", value);
            int end = marker != 0 && (lineFeed == 0 || marker < lineFeed) ? marker : lineFeed;
            if (end == 0) { result.Add(value); break; }
            result.Add(RtlUnit.Copy(value, 1, end - 1));
            int skip = end == marker ? 5 : 1;
            value = RtlUnit.Copy(value, end + skip, value.Length - end - skip + 1);
        } while (value.Length > 0);
        return result.ToArray();
    }
}
