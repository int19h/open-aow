namespace Aow.Rtl;

public class IniFile
{
}

public class Filer
{
}

public class IToolServices
{
    public virtual string GetProjectName() => throw new NotImplementedException("Delphi IDE project service");
}

public static partial class RtlUnit
{
    public static IToolServices? ToolServices => null;
}
