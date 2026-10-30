// Translated from E3IMSTypes.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace Aow.Units.E3IMSTypes
{
    // Imported IMS objects used by E3. List instances are opaque and their methods are imported
    // from ims.dpl.
    public partial class TmsMessageHeader : Aow.Rtl.TObject
    {
        public Aow.Rtl.AnsiString FFrom;
    }

    public partial class TmsAddressList : Aow.Rtl.TObject
    {
        // E3IMSTypes.pas:19
        public void Clear()
        {
            /* TODO: Implement TmsAddressList.Clear. */
            throw new System.NotImplementedException();
        }

        // E3IMSTypes.pas:20
        public void AddAddress(Aow.Rtl.AnsiString Address, Aow.Rtl.AnsiString Name)
        {
            /* TODO: Implement TmsAddressList.AddAddress. */
            throw new System.NotImplementedException();
        }
    }

    public partial class TmsAttList : Aow.Rtl.TObject
    {
        // E3IMSTypes.pas:24
        public void Clear()
        {
            /* TODO: Implement TmsAttList.Clear. */
            throw new System.NotImplementedException();
        }

        // E3IMSTypes.pas:25
        public void AddFile(Aow.Rtl.AnsiString FileName)
        {
            /* TODO: Implement TmsAttList.AddFile. */
            throw new System.NotImplementedException();
        }
    }

    public static partial class E3IMSTypes
    {
    }
}
