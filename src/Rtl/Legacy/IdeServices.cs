namespace Aow.Rtl;

[LegacyObject]
public class IniFile
{
    public IniFile()
    {
        CaptureAddress.Assign(this);
    }
}

[LegacyObject]
public class Filer
{
    public Filer()
    {
        CaptureAddress.Assign(this);
    }
}

[LegacyObject]
public class IToolServices
{
    public virtual string GetProjectName() => throw new NotImplementedException("Delphi IDE project service");

    public IToolServices()
    {
        CaptureAddress.Assign(this);
    }
}

public static partial class RtlUnit
{
    public static IToolServices? ToolServices => null;
}
