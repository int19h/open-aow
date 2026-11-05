// Translated from E3IMSTypes.pas.
using Aow.Rtl;

namespace Aow.Units.E3IMSTypes;

// Imported IMS objects used by E3. List instances are opaque and their methods are imported
// from ims.dpl.
public partial class TmsMessageHeader : TObject
{
    public string FFrom = "";
}

public partial class TmsAddressList : TObject
{
    // E3IMSTypes.pas:19
    public void Clear()
    {
        /* TODO: Implement TmsAddressList.Clear. */
        throw new NotImplementedException();
    }

    // E3IMSTypes.pas:20
    public void AddAddress(string Address, string Name)
    {
        /* TODO: Implement TmsAddressList.AddAddress. */
        throw new NotImplementedException();
    }
}

public partial class TmsAttList : TObject
{
    // E3IMSTypes.pas:24
    public void Clear()
    {
        /* TODO: Implement TmsAttList.Clear. */
        throw new NotImplementedException();
    }

    // E3IMSTypes.pas:25
    public void AddFile(string FileName)
    {
        /* TODO: Implement TmsAttList.AddFile. */
        throw new NotImplementedException();
    }
}

public static partial class E3IMSTypes
{
}
