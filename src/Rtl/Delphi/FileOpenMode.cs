namespace Aow.Rtl;

// The modes of CLASSES.PAS TFileStream.Create: the access in the two low bits and a share mode in
// bits 4 to 6, or Create, which creates the file for reading and writing.
[Flags]
public enum FileOpenMode
{
    Read = 0,
    Write = 1,
    ReadWrite = 2,
    ShareDenyWrite = 32,
    Create = 65535,
}
