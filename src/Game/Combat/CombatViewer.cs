// Translated from AoWE.pas.
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.Combat;

// FStatus. FCombat. FDelay. FSpeed. FPosition. FTime. FLogbook
public partial class CombatViewer : IDisposable
{
    // Combat2.pas:546
    public CombatViewer()
    {
        FCombat = new Battle(null);
        FTime = -1;
        FSpeed = 5;
        FDelay = 5;
    }

    public Set<CombatViewerStatusElement> FStatus;
    public Battle FCombat;
    public byte FDelay;
    public byte FSpeed;
    public int FPosition;
    public int FTime;
    public CombatLogbook FLogbook;

    // Combat2.pas:555
    public virtual void Dispose()
    {
        if (FStatus.Contains(CombatViewerStatusElement.Initialized))
        {
            Finalize();
        }

        FCombat?.Dispose();
    }

    // Combat2.pas:562
    public void Initialize(CombatLogbook Logbook)
    {
        if (FStatus.Contains(CombatViewerStatusElement.Initialized))
        {
            Finalize();
        }

        FStatus = Set<CombatViewerStatusElement>.Of(CombatViewerStatusElement.Initialized);
        FLogbook = Logbook;
        FLogbook.AddRef();
        FPosition = 0;
        FTime = -1;
        FCombat.FCombatData.Assign(Logbook.FInitialData, GameTables.AoWEngine);
        FCombat.Activate();
        FDelay = FSpeed;
    }

    // Combat2.pas:575
    public void Finalize()
    {
        if (FStatus.Contains(CombatViewerStatusElement.Initialized))
        {
            FCombat.Deactivate();
            FCombat.Clear();
            FLogbook.Release();
            FLogbook = null;
            FStatus = FStatus - Set<CombatViewerStatusElement>.Of(CombatViewerStatusElement.Initialized);
        }
    }

    // Combat2.pas:587
    public int GetEndPosition() => FLogbook.FActions.Count + 1;

    // Combat2.pas:590
    public void SetSpeed(byte Speed)
    {
        if (Speed == 0)
        {
            Speed = 1;
        }

        FSpeed = Speed;
    }

    // Combat2.pas:593
    public void SetPosition(int Position)
    {
        if (FStatus.Contains(CombatViewerStatusElement.Initialized))
        {
            FTime = -1;
            if (Position > GetEndPosition())
            {
                Position = GetEndPosition();
            }

            if (Position > 0 && Position <= FLogbook.FActions.Count)
            {
                FCombat.Deactivate();
                FCombat.FCombatData.Assign(FLogbook.FInitialData, GameTables.AoWEngine);
                FCombat.Activate();
                FPosition = 0;
                while (Position > FPosition)
                {
                    FPosition += 1;
                    if (Position != FPosition)
                    {
                        FLogbook.FActions.GetCombatActions(FPosition - 1).Execute(FCombat);
                    }
                }
            }
            else
            {
                if (Position == 0)
                {
                    FPosition = 0;
                    FCombat.Deactivate();
                    FCombat.FCombatData.Assign(FLogbook.FInitialData, GameTables.AoWEngine);
                    FCombat.Activate();
                }
                else
                {
                    FPosition = GetEndPosition();
                    FCombat.Deactivate();
                    FCombat.FCombatData.Assign(FLogbook.FFinalData, GameTables.AoWEngine);
                    FCombat.Activate();
                }
            }
        }
        else
        {
            FPosition = Position;
        }
    }

    // Combat2.pas:629
    public void Play()
    {
        FStatus = FStatus + Set<CombatViewerStatusElement>.Of(CombatViewerStatusElement.Playing) - Set<CombatViewerStatusElement>.Of(CombatViewerStatusElement.Paused);
    }

    // Combat2.pas:632
    public void Pause()
    {
        FStatus = FStatus + Set<CombatViewerStatusElement>.Of(CombatViewerStatusElement.Paused);
    }

    // Combat2.pas:635
    public void Update()
    {
        bool Playing;
        if ((FStatus & Set<CombatViewerStatusElement>.Of(CombatViewerStatusElement.Initialized, CombatViewerStatusElement.Playing, CombatViewerStatusElement.Paused)) == Set<CombatViewerStatusElement>.Of(CombatViewerStatusElement.Initialized, CombatViewerStatusElement.Playing) && GetEndPosition() != FPosition)
        {
            Playing = false;
            if (FPosition == 0)
            {
                FTime += 1;
                if (FTime >= FDelay)
                {
                    FPosition = 1;
                    FTime = -1;
                }
            }

            while (true)
            {
                bool condition = FPosition > 0;
                if (condition)
                {
                    var count = FLogbook.FActions.Count;
                    condition = FPosition <= count;
                }

                if (!(condition && !Playing))
                    break;
                FTime += 1;
                Playing = FLogbook.FActions.GetCombatActions(FPosition - 1).Play(this);
                if (!Playing)
                {
                    FPosition += 1;
                    FDelay = FSpeed;
                    FTime = -1;
                }
            }

            if (GetEndPosition() == FPosition)
            {
                SetPosition(GetEndPosition());
            }
        }
    }

    // Combat2.pas:667
    public void ShowObject(CombatObject Obj, int Frame, int Direction, int X, int Y, in LockedSurface Surface)
    {
        if (FLogbook.FActions.Count >= FPosition)
        {
            FLogbook.FActions.GetCombatActions(FPosition - 1).ShowObject(this, Obj, Frame, Direction, X, Y, Surface);
        }
        else
        {
            Obj.Show(Frame, Direction, X, Y, Surface);
        }
    }

    // Combat2.pas:674
    public void ShowFieldObject(CombatObject Obj, int Frame, int Direction, int X, int Y, in LockedSurface Surface)
    {
        bool condition = FPosition > 0;
        if (condition)
        {
            var count = FLogbook.FActions.Count;
            condition = FPosition <= count;
        }

        if (condition)
        {
            FLogbook.FActions.GetCombatActions(FPosition - 1).ShowFieldObject(this, Obj, Frame, Direction, X, Y, Surface);
        }
        else
        {
            Obj.Show(Frame, Direction, X, Y, Surface);
        }
    }

    // Combat2.pas:681
    public void ShowFightingObjects(int Frame, int X, int Y, in LockedSurface Surface)
    {
        bool condition = FPosition > 0;
        if (condition)
        {
            var count = FLogbook.FActions.Count;
            condition = FPosition <= count;
        }

        if (condition)
        {
            FLogbook.FActions.GetCombatActions(FPosition - 1).ShowFightingObjects(this, Frame, X, Y, Surface);
        }
    }

    // Combat2.pas:688
    public void DefaultShowFightingObjects(CombatObject Source, CombatObject Target, int Frame, int X, int Y, in LockedSurface Surface)
    {
        if (Target != null)
        {
            if (Target.GetSide() == CombatSideIndex.Attacker)
            {
                ShowObject(Target, Frame, 3, X, Y, Surface);
            }
            else
            {
                ShowObject(Target, Frame, 5, X + 25, Y, Surface);
            }
        }

        if (Source != null)
        {
            if (Source.GetSide() == CombatSideIndex.Attacker)
            {
                ShowObject(Source, Frame, 2, X, Y + 15, Surface);
            }
            else
            {
                ShowObject(Source, Frame, 6, X + 25, Y + 15, Surface);
            }
        }
    }

    // intf_P3_02.inc:19

}
