namespace Aow.Rtl;

public readonly record struct RandomTraceEvent(string Kind, int? Argument, double Result, int Before, int After);
