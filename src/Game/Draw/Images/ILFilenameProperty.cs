// Translated from ImageLib.pas.
using Aow.Rtl;

namespace Aow.Game.Draw.Images;

// (declared in the implementation): the property editor of TCustomImageLibrary.FileName
public partial class ILFilenameProperty : StringProperty
{
    // ImageLib.pas:10669
    public override void Edit()
    {
        /* TODO: Implement DsgnIntf.GetComponent in the platform or editor service. */
        throw new NotImplementedException();
    }

    // ImageLib.pas:10720
    public override Set<PropertyAttribute> GetAttributes() => Set<PropertyAttribute>.Of(PropertyAttribute.Dialog, PropertyAttribute.Revertable);

    // ImageLib.pas:10663
    public override string GetValue()
    {
        /* TODO: Implement DsgnIntf.GetValue in the platform or editor service. */
        throw new NotImplementedException();
    }
}
