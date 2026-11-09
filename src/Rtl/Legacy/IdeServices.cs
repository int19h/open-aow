namespace Aow.Rtl;

[LegacyObject]
public class TIniFile
{
    public TIniFile()
    {
        CaptureAddress.Assign(this);
    }
}

[LegacyObject]
public class TFiler
{
    public TFiler()
    {
        CaptureAddress.Assign(this);
    }
}

[LegacyObject]
public class TIToolServices
{
    public virtual string GetProjectName() => throw new NotImplementedException("Delphi IDE project service");

    public TIToolServices()
    {
        CaptureAddress.Assign(this);
    }
}

public static partial class RtlUnit
{
    public static TIToolServices? ToolServices => null;
}
