using System.Runtime.CompilerServices;
using Aow.Rtl.Localize;

namespace Aow.Rtl;

public sealed record TranslationTarget(AnsiString ClassName, AnsiString PropertyName, int Options = 0);

public static class TranslatorProperties
{
    internal sealed record Binding(Type Type, string Name, Func<TComponent, AnsiString[]> Read,
        Action<TComponent, AnsiString[]> Write, bool IsList);
    private static readonly Dictionary<(Type, string), Binding> bindings = [];

    public static void Register<T>(string name, Func<T, AnsiString> read, Action<T, AnsiString> write) where T : TComponent
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

    private ConditionalWeakTable<TComponent, Dictionary<string, AnsiString[]>> nativeValues = new();
    public AnsiString DictionaryName { get; set; } = "AoW";
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
        AnsiString form = TObject.ClassNameOf(module.GetType());
        Visit(module);

        void Visit(TComponent component)
        {
            foreach (var target in TargetsData)
            {
                if (!Matches(component.GetType(), target.ClassName)) continue;
                string name = target.PropertyName.ToString();
                var binding = TranslatorProperties.Find(component.GetType(), name);
                if (binding is null) continue;
                var values = nativeValues.GetOrCreateValue(component);
                if (!values.TryGetValue(name, out var native))
                {
                    native = binding.Read(component);
                    values.Add(name, native);
                }
                AnsiString componentName = ReferenceEquals(component, module) ? default : component.Name;
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

    private static bool Matches(Type type, AnsiString name)
    {
        if (name.Length == 0) return true;
        for (Type? current = type; current is not null; current = current.BaseType)
            if ((AnsiString)TObject.ClassNameOf(current) == name) return true;
        return false;
    }

    private static AnsiString Join(AnsiString[] values)
    {
        AnsiString result = default;
        for (int index = 0; index < values.Length; index++)
        {
            if (index > 0) result += "<eol>";
            result += values[index];
        }
        return result;
    }

    private static AnsiString[] Split(AnsiString value)
    {
        var result = new List<AnsiString>();
        do
        {
            int marker = Rtl.Pos("<eol>", value), lineFeed = Rtl.Pos(AnsiString.FromBytes(10), value);
            int end = marker != 0 && (lineFeed == 0 || marker < lineFeed) ? marker : lineFeed;
            if (end == 0) { result.Add(value); break; }
            result.Add(Rtl.Copy(value, 1, end - 1));
            int skip = end == marker ? 5 : 1;
            value = Rtl.Copy(value, end + skip, value.Length - end - skip + 1);
        } while (value.Length > 0);
        return result.ToArray();
    }
}
