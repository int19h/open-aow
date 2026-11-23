using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;

namespace Aow.Analyzers;

/// <summary>Why an argument of a call carries a name or needs none (README.md, AOW0001).</summary>
public enum ArgumentReason
{
    /// <summary>The argument needs a name and has none. The analyzer reports it.</summary>
    NeedsName,

    /// <summary>The first argument of the list needs no name.</summary>
    First,

    /// <summary>The argument carries a name already.</summary>
    Named,

    /// <summary>The argument is an element of a <c>params</c> list in expanded form, which takes no name.</summary>
    ParamsElement,

    /// <summary>The argument is a lambda or anonymous method in the last place.</summary>
    TrailingLambda,

    /// <summary>The argument is a name equal to the parameter name.</summary>
    SameName,

    /// <summary>The argument is one of a positional group such as <c>x, y</c> or <c>width, height</c>.</summary>
    Convention,

    /// <summary>The parameter is one of a numbered series such as <c>val1, val2</c>.</summary>
    NumberedSeries,

    /// <summary>The call is inside an expression tree, which takes no named argument.</summary>
    ExpressionTree,

    /// <summary>The argument binds to no parameter with a name (an error, a dynamic call or a nameless parameter).</summary>
    Unbound,
}

/// <summary>The verdict on one argument of a call.</summary>
/// <param name="Syntax">The argument.</param>
/// <param name="Position">The argument's place in the list, from 0.</param>
/// <param name="Parameter">The parameter the argument binds to, if any.</param>
/// <param name="Reason">Why the argument needs a name or needs none.</param>
/// <param name="IsBoolean">True when the parameter is a boolean.</param>
/// <param name="Group">The positional group of a <see cref="ArgumentReason.Convention"/> argument.</param>
public readonly record struct ArgumentVerdict(
    ArgumentSyntax Syntax,
    int Position,
    IParameterSymbol? Parameter,
    ArgumentReason Reason,
    bool IsBoolean,
    string? Group);

/// <summary>
/// The rule of AGENTS.md "How new code is written" for named arguments. A call with one argument
/// needs no name. From the second argument on, arguments are named, except where a positional
/// convention carries the meaning. A boolean argument is always named unless it is the first. The
/// exceptions and the edge cases are in README.md.
/// </summary>
public static class NamedArgumentRules
{
    /// <summary>
    /// The positional groups. A run of consecutive parameters whose names are one of these, in
    /// this order, after a prefix and a suffix common to the run are taken off, needs no names.
    /// </summary>
    public static ImmutableArray<ImmutableArray<string>> Conventions { get; } =
    [
        ["year", "month", "day", "hour", "minute", "second"],
        ["x", "y", "z"],
        ["r", "g", "b"],
        ["x", "y", "level"],
        ["year", "month", "day"],
        ["hour", "minute", "second"],
        ["x", "y", "width", "height"],
        ["left", "top", "right", "bottom"],
        ["left", "top", "width", "height"],
        ["x", "y"],
        ["width", "height"],
        ["left", "top"],
        ["right", "bottom"],
        ["column", "row"],
        ["row", "column"],
        ["expected", "actual"],
        ["key", "value"],
        ["name", "value"],
        ["sender", "e"],
        ["left", "right"],
        ["a", "b"],
        ["min", "max"],
        ["minimum", "maximum"],
        ["old", "new"],
        ["low", "high"],
        ["lo", "hi"],
    ];

    private static readonly ImmutableArray<ImmutableArray<string>> s_longestFirst = [.. Conventions.OrderByDescending(convention => convention.Length)];

    /// <summary>The verdicts on the arguments of a call, an object creation or a constructor initializer.</summary>
    public static ImmutableArray<ArgumentVerdict> Classify(IOperation operation)
    {
        (ArgumentListSyntax? list, IMethodSymbol? method, ImmutableArray<IArgumentOperation> arguments) = operation switch
        {
            IInvocationOperation invocation => (ArgumentList(invocation.Syntax), invocation.TargetMethod, invocation.Arguments),
            IObjectCreationOperation creation => (ArgumentList(creation.Syntax), creation.Constructor, creation.Arguments),
            _ => (null, null, []),
        };
        if (list is null || method is null || list.Arguments.Count < 2)
        {
            return [];
        }
        Dictionary<ArgumentSyntax, IArgumentOperation> bound = [];
        foreach (IArgumentOperation argument in arguments)
        {
            // The operation of an argument is its expression in some trees (a suppressed null
            // warning, for one), so the argument is found by ancestry.
            if (argument.ArgumentKind == ArgumentKind.Explicit
                && argument.Syntax.FirstAncestorOrSelf<ArgumentSyntax>() is ArgumentSyntax syntax
                && syntax.Parent == list)
            {
                bound[syntax] = argument;
            }
        }
        // An extension method called on its receiver binds the receiver to its first parameter.
        int receiverOffset = arguments.Any(argument => argument.Parameter is { Ordinal: 0 } && argument.Syntax.FirstAncestorOrSelf<ArgumentSyntax>()?.Parent != list && method.IsExtensionMethod) ? 1 : 0;
        bool inExpressionTree = InExpressionTree(list, model: operation.SemanticModel);
        Dictionary<IParameterSymbol, string> groups = ConventionGroups(method, list, bound);
        ImmutableArray<ArgumentVerdict>.Builder verdicts = ImmutableArray.CreateBuilder<ArgumentVerdict>(list.Arguments.Count);
        for (int position = 0; position < list.Arguments.Count; position++)
        {
            ArgumentSyntax syntax = list.Arguments[position];
            IParameterSymbol? parameter = bound.TryGetValue(syntax, out IArgumentOperation? argument) ? argument.Parameter : null;
            bool isBoolean = parameter is not null && IsBoolean(parameter.Type);
            string? group = null;
            ArgumentReason reason;
            if (position == 0)
            {
                reason = ArgumentReason.First;
            }
            else if (syntax.NameColon is not null)
            {
                reason = ArgumentReason.Named;
            }
            else if (parameter is null)
            {
                reason = IsParamsElement(method, ordinal: position + receiverOffset) ? ArgumentReason.ParamsElement : ArgumentReason.Unbound;
            }
            else if (parameter.Name.Length == 0 || method.MethodKind == MethodKind.FunctionPointerSignature)
            {
                reason = ArgumentReason.Unbound;
            }
            else if (inExpressionTree)
            {
                reason = ArgumentReason.ExpressionTree;
            }
            else if (SameName(syntax.Expression, parameterName: parameter.Name))
            {
                reason = ArgumentReason.SameName;
            }
            else if (position == list.Arguments.Count - 1 && syntax.Expression is AnonymousFunctionExpressionSyntax)
            {
                reason = ArgumentReason.TrailingLambda;
            }
            else if (isBoolean)
            {
                reason = ArgumentReason.NeedsName;
            }
            else if (groups.TryGetValue(parameter, out string? found))
            {
                reason = ArgumentReason.Convention;
                group = found;
            }
            else
            {
                reason = InNumberedSeries(method, parameter) ? ArgumentReason.NumberedSeries : ArgumentReason.NeedsName;
            }
            verdicts.Add(new ArgumentVerdict(Syntax: syntax, Position: position, Parameter: parameter, Reason: reason, IsBoolean: isBoolean, Group: group));
        }
        return verdicts.MoveToImmutable();
    }

    /// <summary>The argument list of a call's syntax, or null when the call has none in source.</summary>
    public static ArgumentListSyntax? ArgumentList(SyntaxNode syntax) => syntax switch
    {
        InvocationExpressionSyntax invocation => invocation.ArgumentList,
        BaseObjectCreationExpressionSyntax creation => creation.ArgumentList,
        ConstructorInitializerSyntax initializer => initializer.ArgumentList,
        PrimaryConstructorBaseTypeSyntax baseType => baseType.ArgumentList,
        _ => null,
    };

    /// <summary>The name an argument takes for a parameter, escaped where it is a keyword.</summary>
    public static string ArgumentName(IParameterSymbol parameter)
        => SyntaxFacts.GetKeywordKind(parameter.Name) != SyntaxKind.None || SyntaxFacts.GetContextualKeywordKind(parameter.Name) == SyntaxKind.UnderscoreToken
            ? "@" + parameter.Name
            : parameter.Name;

    private static bool IsBoolean(ITypeSymbol type)
        => type.SpecialType == SpecialType.System_Boolean
        || (type is INamedTypeSymbol { OriginalDefinition.SpecialType: SpecialType.System_Nullable_T, TypeArguments: [{ SpecialType: SpecialType.System_Boolean }] });

    private static bool IsParamsElement(IMethodSymbol method, int ordinal)
        => method.Parameters.Length > 0 && method.Parameters[^1].IsParams && ordinal >= method.Parameters.Length - 1;

    private static bool InExpressionTree(SyntaxNode node, SemanticModel? model)
    {
        if (model is null)
        {
            return false;
        }
        foreach (SyntaxNode ancestor in node.Ancestors())
        {
            if (ancestor is LambdaExpressionSyntax lambda
                && model.GetTypeInfo(lambda).ConvertedType is INamedTypeSymbol { Name: "Expression", ContainingNamespace: { Name: "Expressions", ContainingNamespace: { Name: "Linq", ContainingNamespace.Name: "System" } } })
            {
                return true;
            }
            if (ancestor is MemberDeclarationSyntax)
            {
                return false;
            }
        }
        return false;
    }

    /// <summary>True when the argument is a name, a member access or an out variable that equals the parameter name.</summary>
    public static bool SameName(ExpressionSyntax expression, string parameterName)
    {
        string? name = TrailingName(expression);
        return name is not null && string.Equals(FieldStem(name), parameterName.TrimStart('@'), comparisonType: StringComparison.OrdinalIgnoreCase);
    }

    private static string? TrailingName(ExpressionSyntax expression) => expression switch
    {
        IdentifierNameSyntax identifier => identifier.Identifier.ValueText,
        MemberAccessExpressionSyntax { Name: IdentifierNameSyntax name } => name.Identifier.ValueText,
        ConditionalAccessExpressionSyntax { WhenNotNull: MemberBindingExpressionSyntax { Name: IdentifierNameSyntax name } } => name.Identifier.ValueText,
        DeclarationExpressionSyntax { Designation: SingleVariableDesignationSyntax designation } => designation.Identifier.ValueText,
        PostfixUnaryExpressionSyntax { RawKind: (int)SyntaxKind.SuppressNullableWarningExpression } suppressed => TrailingName(suppressed.Operand),
        ParenthesizedExpressionSyntax parenthesized => TrailingName(parenthesized.Expression),
        _ => null,
    };

    private static string FieldStem(string name)
        => name.StartsWith("s_", comparisonType: StringComparison.Ordinal) ? name[2..] : name.TrimStart('_');

    private static bool InNumberedSeries(IMethodSymbol method, IParameterSymbol parameter)
        => InNumberedSeries([.. method.Parameters.Select(other => other.Name)], ordinal: parameter.Ordinal);

    private static bool InNumberedSeries(string[] names, int ordinal)
    {
        (string stem, string number) = SplitNumber(names[ordinal]);
        return number.Length > 0
            && names.Where((_, index) => index != ordinal).Any(other => SplitNumber(other) is var (otherStem, otherNumber)
                && otherNumber.Length > 0 && otherNumber != number && string.Equals(otherStem, stem, comparisonType: StringComparison.OrdinalIgnoreCase));
    }

    private static (string Stem, string Number) SplitNumber(string name)
    {
        int end = name.Length;
        while (end > 0 && char.IsDigit(name[end - 1]))
        {
            end--;
        }
        return (name[..end], name[end..]);
    }

    // The parameters that a complete, positional, in-order run of a convention covers.
    private static Dictionary<IParameterSymbol, string> ConventionGroups(IMethodSymbol method, ArgumentListSyntax list, Dictionary<ArgumentSyntax, IArgumentOperation> bound)
    {
        Dictionary<int, int> positionOfParameter = [];
        for (int position = 0; position < list.Arguments.Count; position++)
        {
            ArgumentSyntax syntax = list.Arguments[position];
            // A boolean argument always takes a name, so a run that holds one is no convention:
            // the run would break once the fix names the boolean.
            if (syntax.NameColon is null && bound.TryGetValue(syntax, out IArgumentOperation? argument) && argument.Parameter is not null && !IsBoolean(argument.Parameter.Type))
            {
                positionOfParameter[argument.Parameter.Ordinal] = position;
            }
        }
        Dictionary<IParameterSymbol, string> groups = [with(SymbolEqualityComparer.Default)];
        foreach ((int ordinal, string group) in ConventionRuns([.. method.Parameters.Select(parameter => parameter.Name)], positionOfParameter))
        {
            groups[method.Parameters[ordinal]] = group;
        }
        return groups;
    }

    // The ordinals of the parameters that a complete, positional, in-order run of a convention covers.
    private static Dictionary<int, string> ConventionRuns(string[] names, Dictionary<int, int> positionOfParameter)
    {
        Dictionary<int, string> groups = [];
        int start = 0;
        while (start < names.Length)
        {
            int matched = 0;
            foreach (ImmutableArray<string> convention in s_longestFirst)
            {
                if (start + convention.Length <= names.Length
                    && Matches(names, start, convention)
                    && Enumerable.Range(start, count: convention.Length).All(ordinal => positionOfParameter.TryGetValue(ordinal, out int position) && position == positionOfParameter[start] + (ordinal - start)))
                {
                    string name = string.Join(", ", values: convention);
                    for (int ordinal = start; ordinal < start + convention.Length; ordinal++)
                    {
                        groups[ordinal] = name;
                    }
                    matched = convention.Length;
                    break;
                }
            }
            start += Math.Max(matched, 1);
        }
        return groups;
    }

    /// <summary>
    /// Which arguments of a call need a name, for a generator that writes the call without a
    /// semantic model. The arguments bind in order, one to each parameter, with no <c>params</c>
    /// list and no receiver. The rules are those of <see cref="Classify"/>.
    /// </summary>
    public static ImmutableArray<bool> NeedsNames(IReadOnlyList<(string Name, bool IsBoolean)> parameters, ArgumentListSyntax list)
    {
        string[] names = [.. parameters.Select(parameter => parameter.Name)];
        Dictionary<int, int> positionOfParameter = [];
        for (int position = 0; position < list.Arguments.Count && position < names.Length; position++)
        {
            if (list.Arguments[position].NameColon is null && !parameters[position].IsBoolean)
            {
                positionOfParameter[position] = position;
            }
        }
        Dictionary<int, string> groups = ConventionRuns(names, positionOfParameter: positionOfParameter);
        ImmutableArray<bool>.Builder needed = ImmutableArray.CreateBuilder<bool>(list.Arguments.Count);
        for (int position = 0; position < list.Arguments.Count; position++)
        {
            ArgumentSyntax argument = list.Arguments[position];
            needed.Add(position > 0
                && position < names.Length
                && argument.NameColon is null
                && !SameName(argument.Expression, parameterName: names[position])
                && !(position == list.Arguments.Count - 1 && argument.Expression is AnonymousFunctionExpressionSyntax)
                && (parameters[position].IsBoolean || (!groups.ContainsKey(position) && !InNumberedSeries(names, ordinal: position))));
        }
        return needed.MoveToImmutable();
    }

    private static bool Matches(string[] allNames, int start, ImmutableArray<string> convention)
    {
        string[] names = allNames[start..(start + convention.Length)];
        int prefix = CommonPrefix(names);
        int suffix = CommonSuffix(names, prefix);
        for (int index = 0; index < names.Length; index++)
        {
            string name = names[index];
            string core = name.Substring(prefix, length: name.Length - prefix - suffix);
            string word = convention[index];
            bool leadingWord = name.Length > word.Length
                && name.StartsWith(word, comparisonType: StringComparison.OrdinalIgnoreCase)
                && char.IsUpper(name[word.Length]);
            bool trailingWord = name.Length > word.Length
                && name.EndsWith(word, comparisonType: StringComparison.OrdinalIgnoreCase)
                && char.IsUpper(name[^word.Length])
                && char.IsLower(name[^(word.Length + 1)]);
            if (!string.Equals(name, word, comparisonType: StringComparison.OrdinalIgnoreCase) && !string.Equals(core, word, comparisonType: StringComparison.OrdinalIgnoreCase) && !leadingWord && !trailingWord)
            {
                return false;
            }
        }
        return true;
    }

    private static int CommonPrefix(string[] names)
    {
        int length = names.Min(name => name.Length);
        int prefix = 0;
        while (prefix < length && names.All(name => name[prefix] == names[0][prefix]))
        {
            prefix++;
        }
        return prefix;
    }

    private static int CommonSuffix(string[] names, int prefix)
    {
        int length = names.Min(name => name.Length) - prefix;
        int suffix = 0;
        while (suffix < length && names.All(name => name[name.Length - 1 - suffix] == names[0][names[0].Length - 1 - suffix]))
        {
            suffix++;
        }
        return suffix;
    }
}
