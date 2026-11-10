// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

namespace Aow.Game.Net;

// { Interface declaration run owned by P5. }
// name. unique local ID. unique ID. security ID. email address. last day (not serialized by the
// original)
public partial class EmailGame : EObject
{
    // Diplomacy.pas:2462
    public new const int ClassID = 136960;

    public EmailGame(ECustomNode owner) : base(owner)
    { }

    public string FName = "";
    public int FUniqueLocalID;
    public int FUniqueID;
    public int FSecurityID;
    public string FEmailAddress = "";
    public int FLastDay;

    // Diplomacy.pas:2465
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwString(ref FName, 20);
        Stream.rwInteger(ref FUniqueLocalID, 21);
        Stream.rwInteger(ref FUniqueID, 22);
        Stream.rwString(ref FEmailAddress, 23);
        Stream.rwInteger(ref FSecurityID, 24);
    }

    // Engine.pas:524

}
