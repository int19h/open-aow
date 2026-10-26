namespace Aow.Rtl;

public class EAbort(AnsiString message = default) : Exception(message)
{
    public static new EAbort Create(AnsiString message = default) => new(message);
    public static new EAbort CreateFmt(AnsiString format, params TVarRec[] args) => new(ExceptionText.Format(format, args));
}

public class EConvertError(AnsiString message = default) : Exception(message)
{
    public static new EConvertError Create(AnsiString message = default) => new(message);
    public static new EConvertError CreateFmt(AnsiString format, params TVarRec[] args) => new(ExceptionText.Format(format, args));
}

public class EListError(AnsiString message = default) : Exception(message)
{
    public static new EListError Create(AnsiString message = default) => new(message);
    public static new EListError CreateFmt(AnsiString format, params TVarRec[] args) => new(ExceptionText.Format(format, args));
}

public class EStreamError(AnsiString message = default) : Exception(message)
{
    public static new EStreamError Create(AnsiString message = default) => new(message);
    public static new EStreamError CreateFmt(AnsiString format, params TVarRec[] args) => new(ExceptionText.Format(format, args));
}

public class EReadError(AnsiString message = default) : EFilerError(message)
{
    public static new EReadError Create(AnsiString message = default) => new(message);
    public static new EReadError CreateFmt(AnsiString format, params TVarRec[] args) => new(ExceptionText.Format(format, args));
}

public class EWriteError(AnsiString message = default) : EFilerError(message)
{
    public static new EWriteError Create(AnsiString message = default) => new(message);
    public static new EWriteError CreateFmt(AnsiString format, params TVarRec[] args) => new(ExceptionText.Format(format, args));
}

public class EFOpenError(AnsiString message = default) : EStreamError(message)
{
    public static new EFOpenError Create(AnsiString message = default) => new(message);
    public static new EFOpenError CreateFmt(AnsiString format, params TVarRec[] args) => new(ExceptionText.Format(format, args));
}

public class EFCreateError(AnsiString message = default) : EStreamError(message)
{
    public static new EFCreateError Create(AnsiString message = default) => new(message);
    public static new EFCreateError CreateFmt(AnsiString format, params TVarRec[] args) => new(ExceptionText.Format(format, args));
}

public class EInvalidCast(AnsiString message = default) : Exception(message)
{
    public static new EInvalidCast Create(AnsiString message = default) => new(message);
    public static new EInvalidCast CreateFmt(AnsiString format, params TVarRec[] args) => new(ExceptionText.Format(format, args));
}

public class EAccessViolation(AnsiString message = default) : Exception(message)
{
    public static new EAccessViolation Create(AnsiString message = default) => new(message);
    public static new EAccessViolation CreateFmt(AnsiString format, params TVarRec[] args) => new(ExceptionText.Format(format, args));
}

public class EDivByZero(AnsiString message = default) : EIntError(message)
{
    public static new EDivByZero Create(AnsiString message = default) => new(message);
    public static new EDivByZero CreateFmt(AnsiString format, params TVarRec[] args) => new(ExceptionText.Format(format, args));
}

public class ERangeError(AnsiString message = default) : EIntError(message)
{
    public static new ERangeError Create(AnsiString message = default) => new(message);
    public static new ERangeError CreateFmt(AnsiString format, params TVarRec[] args) => new(ExceptionText.Format(format, args));
}

public class EIntOverflow(AnsiString message = default) : EIntError(message)
{
    public static new EIntOverflow Create(AnsiString message = default) => new(message);
    public static new EIntOverflow CreateFmt(AnsiString format, params TVarRec[] args) => new(ExceptionText.Format(format, args));
}

public class EInvalidOp(AnsiString message = default) : EMathError(message)
{
    public static new EInvalidOp Create(AnsiString message = default) => new(message);
    public static new EInvalidOp CreateFmt(AnsiString format, params TVarRec[] args) => new(ExceptionText.Format(format, args));
}

public class EAssertionFailed(AnsiString message = default) : Exception(message)
{
    public static new EAssertionFailed Create(AnsiString message = default) => new(message);
    public static new EAssertionFailed CreateFmt(AnsiString format, params TVarRec[] args) => new(ExceptionText.Format(format, args));
}

public class EComponentError(AnsiString message = default) : Exception(message)
{
    public static new EComponentError Create(AnsiString message = default) => new(message);
    public static new EComponentError CreateFmt(AnsiString format, params TVarRec[] args) => new(ExceptionText.Format(format, args));
}

public class EInvalidOperation(AnsiString message = default) : Exception(message)
{
    public static new EInvalidOperation Create(AnsiString message = default) => new(message);
    public static new EInvalidOperation CreateFmt(AnsiString format, params TVarRec[] args) => new(ExceptionText.Format(format, args));
}

public class EOutOfMemory(AnsiString message = default) : Exception(message)
{
    public static new EOutOfMemory Create(AnsiString message = default) => new(message);
    public static new EOutOfMemory CreateFmt(AnsiString format, params TVarRec[] args) => new(ExceptionText.Format(format, args));
}

public class EInOutError(AnsiString message = default) : Exception(message)
{
    public int ErrorCode;
    public static new EInOutError Create(AnsiString message = default) => new(message);
    public static new EInOutError CreateFmt(AnsiString format, params TVarRec[] args) => new(ExceptionText.Format(format, args));
}

public class EAbstractError(AnsiString message = default) : Exception(message)
{
    public static new EAbstractError Create(AnsiString message = default) => new(message);
    public static new EAbstractError CreateFmt(AnsiString format, params TVarRec[] args) => new(ExceptionText.Format(format, args));
}

public class EIntError(AnsiString message = default) : Exception(message)
{
    public static new EIntError Create(AnsiString message = default) => new(message);
    public static new EIntError CreateFmt(AnsiString format, params TVarRec[] args) => new(ExceptionText.Format(format, args));
}

public class EMathError(AnsiString message = default) : Exception(message)
{
    public static new EMathError Create(AnsiString message = default) => new(message);
    public static new EMathError CreateFmt(AnsiString format, params TVarRec[] args) => new(ExceptionText.Format(format, args));
}

public class EFilerError(AnsiString message = default) : EStreamError(message)
{
    public static new EFilerError Create(AnsiString message = default) => new(message);
    public static new EFilerError CreateFmt(AnsiString format, params TVarRec[] args) => new(ExceptionText.Format(format, args));
}

public class EZeroDivide(AnsiString message = default) : EMathError(message)
{
    public static new EZeroDivide Create(AnsiString message = default) => new(message);
    public static new EZeroDivide CreateFmt(AnsiString format, params TVarRec[] args) => new(ExceptionText.Format(format, args));
}

public class EOverflow(AnsiString message = default) : EMathError(message)
{
    public static new EOverflow Create(AnsiString message = default) => new(message);
    public static new EOverflow CreateFmt(AnsiString format, params TVarRec[] args) => new(ExceptionText.Format(format, args));
}
