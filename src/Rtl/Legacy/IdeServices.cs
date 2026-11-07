namespace Aow.Rtl;

public class TIniFile : TObject { }

public class TFiler : TObject { }

public class TIToolServices : TObject
{
    public virtual string GetProjectName() => throw new NotImplementedException("Delphi IDE project service");
}

public static partial class RtlUnit
{
    public static TIToolServices? ToolServices => null;
}
