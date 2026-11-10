// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

// items, generation mode
public partial class ItemHS : SingleHS
{

    public new const int ClassID = 131697;

    public ItemHSList FItems;
    public ItemGenerateMode FGenerateMode;

    // Army.PAS:5221
    public override void Dispose()
    {
        base.Dispose();
        FItems?.Dispose();
        FItems = null;
    }

    // Army.PAS:5278
    public override void ReadWrite(EStorageStream Stream)
    {
        FItems.BeginUpdate();
        base.ReadWrite(Stream);
        Stream.rwEObject(FItems, 4);
        Stream.rwData(Bytes.Of(ref FGenerateMode, 1), 1, 5);
        FItems.EndUpdate();
    }

    // Army.PAS:5321
    public override void MsgProc(ref EMsg Msg, EObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 537001985 && AoWHSMap.Current.FDay == 1)
        {
            GenerateItems();
        }
    }

    // Army.PAS:5235
    public override byte GetLevel() => 26;

    // Army.PAS:5240
    public override int GetShowPriority()
    {
        int Xhp = default;
        int Yhp = default;
        HexagonSprite HS;
        HexGeometry.HXtoHP(((MapField)Owner).Xhx, ((MapField)Owner).Yhx, ref Xhp, ref Yhp);
        int Result = 1744830464 + ((Yhp + 33) << 13) + (Xhp + 16);
        HS = ((MapField)Owner).GetMaxLevelNoneTransparentHS();
        if (HS != this && HS != null)
        {
            if (Result <= HS.GetShowPriority())
            {
                Result = HS.GetShowPriority() + 1;
            }
        }

        return Result;
    }

    // Army.PAS:5287
    public override void Show(int XhpSurf, int YhpSurf, in SurfaceDesc SurfaceDesc)
    {
        int Xhp = default;
        int Yhp = default;
        if (!GetSelected() || (AoWHSMap.Current.Frame & 6) != 6)
        {
            HexGeometry.HXtoHP(((MapField)Owner).Xhx, ((MapField)Owner).Yhx, ref Xhp, ref Yhp);
            AoWHSSet.Current.FGeneralImages[0].Show(Xhp - XhpSurf, Yhp - YhpSurf, SurfaceDesc);
        }
    }

    // Army.PAS:5267
    public override void Activate()
    {
        base.Activate();
        FItems.RegisterItems();
    }

    // Army.PAS:5273
    public override void Deactivate()
    {
        base.Deactivate();
    }

    // Army.PAS:5260
    public override bool CanSelect(int Xhp, int Yhp)
    {
        bool Result;
        if (Unsafe.As<ushort, WordRec>(ref Unsafe.As<int, LongRec>(ref ((AoWMapField)Owner).FFieldFlags).Hi).Lo == 0 && AoWHSMap.Current.FFogEnabled)
        {
            Result = false;
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // Army.PAS:5255
    public override string EditName() => AbilityRules.TranslateRStr(ItemBagRStr);

    // Army.PAS:5297
    public override bool CanPlace(MapContainer Map, sbyte Xhx, sbyte Yhx, sbyte Lhx) => base.CanPlace(Map, Xhx, Yhx, Lhx);

    // Army.PAS:5228
    public void ItemsChanged(object Sender)
    {
        if (FItems.Count == 0 && (FGenerateMode == ItemGenerateMode.UserDefined || AoWHSMap.Current.FDay > 0))
        {
            Dispose();
        }
        else
        {
            Changed();
        }
    }

    // Army.PAS:5302
    public void GenerateItems()
    {
        byte I = default;
        Item Item = default;
        if (FGenerateMode != ItemGenerateMode.UserDefined)
        {
            RandSeed = AoWHSMap.Current.Random(16777215);
            FItems.FOnChanged = null;
            if (FGenerateMode == ItemGenerateMode.Random)
            {
                FGenerateMode = (ItemGenerateMode)(AoWHSMap.Current.Random(3) + 1);
            }

            FItems.Clear();
            for (int iOrdinal = 1, last = (int)FGenerateMode; iOrdinal <= last; iOrdinal++)
            {
                I = (byte)iOrdinal;
                Item = AoWHSMap.Current.FItemControl.GenerateItem(Set<ItemType>.Of(ItemType.Head, ItemType.Torso, ItemType.Attack, ItemType.Defense, ItemType.Ring, ItemType.Use), 0, 65535);
                if (Item != null)
                {
                    Item.SetOwner(FItems);
                }
            }

            FItems.FOnChanged = ItemsChanged;
            ItemsChanged(FItems);
        }
    }

    private static readonly string ItemBagRStr = "Item Bag";

    public enum ItemGenerateMode : byte
    {
        UserDefined = 0, Low = 1, Average = 2, High = 3, Random = 4
    }

    // intf_P2_04.inc:216

    // intf_P2_04.inc:216

}
