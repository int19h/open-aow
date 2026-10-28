namespace Aow.Units.Teleport;
public partial class TTeleport { public override bool GetRazed() => (FState.Image & 1) != 0; }
