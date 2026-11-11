// Translated from AoWE.pas.
using System.Runtime.CompilerServices;

using Aow.Engine.Buffers;

using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Audio;

using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Draw.Images;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Abilities;

// ID, owner name, status, references, library ID, library name, value, name, type, spell ID,
// description, graphics index, hidden, availability, attack, defense, damage, resistance
public partial class Item : AbilityOwner
{
    public new const int ClassID = 131688;

    // Army.PAS:4211
    public Item(ECustomNode owner) : base(owner)
    {
        FItemGFXIndex = -1;
        FDescription = new StringList();
        FLibraryID = -1;
        FRefCount = 1;
    }

    public int FItemID;
    public string FOwnerName = "";
    public Set<ItemStatusElement> FStatus;
    public int FRefCount;
    public int FLibraryID;
    public string FLibraryName = "";
    public int FObtainValue;
    public string FName = "";
    public ItemType FItemType;
    public int FSpellID;
    public StringList FDescription;
    public int FItemGFXIndex;
    public bool FHidden;
    public Availability FAvailability;
    public sbyte FAttack;
    public sbyte FDefense;
    public sbyte FDamage;
    public sbyte FResistance;

    // Army.PAS:4220
    public override void Dispose()
    {
        AddRef();
        if (FStatus.Contains(ItemStatusElement.Activated))
        {
            Deactivate();
        }

        if (FStatus.Contains(ItemStatusElement.Registered))
        {
            AoWHSMap.Current.FItemControl.UnRegisterItem(this);
        }

        FDescription?.Dispose();
        base.Dispose();
    }

    // Army.PAS:4241
    public override void SetOwner(ECustomNode AOwner)
    {
        if (AOwner != FOwner)
        {
            AddRef();
            if (FOwner != null)
            {
                FOwner.RemoveChild(this);
            }

            FOwner = AOwner;
            if (FOwner != null)
            {
                if (FOwner.AddChild(this) == -1)
                {
                    FOwner = null;
                }
            }

            Changed();
            Release();
        }
    }

    // Army.PAS:4448
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        if (!Stream.StorageMode().Contains(EngineServices.smText) || !GetLibraryItem())
        {
            Stream.rwInteger(ref FItemGFXIndex, 7);
        }

        if (!Stream.StorageMode().Contains(EngineServices.smText) || FOwner is ItemLibrary || !GetLibraryItem())
        {
            Stream.rwString(ref FOwnerName, 8);
        }

        if (!Stream.StorageMode().Contains(EngineServices.smText) || !GetLibraryItem())
        {
            Stream.rwString(ref FName, 9);
            Stream.rwData(Bytes.Of(ref FAvailability, 1), 1, 10);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FAttack), 11);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FDefense), 12);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FDamage), 13);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FResistance), 14);
            Stream.rwData(Bytes.Of(ref FItemType, 1), 1, 15);
            Stream.rwBool(ref FHidden, 16);
            Stream.rwInteger(ref FItemID, 17);
            Stream.rwStrings(FDescription, 18);
            Stream.rwInteger(ref FObtainValue, 19);
            Stream.rwInteger(ref FSpellID, 20);
            Stream.rwIntegerEx(ref FLibraryID, -1, 21);
            if (!Stream.StorageMode().Contains(EngineServices.smText))
            {
                Stream.rwString(ref FLibraryName, 22);
            }
        }
    }

    // Army.PAS:4229
    public override int AddRef()
    {
        FRefCount += 1;
        return FRefCount;
    }

    // Army.PAS:4232
    public override int Release()
    {
        int Count;
        FRefCount -= 1;
        Count = FRefCount;
        int Result = Count;
        if (Count == 0)
        {
            Dispose();
        }

        return Result;
    }

    // Army.PAS:4510
    public override void MsgProc(ref EngineMessage Msg, EngineObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 65541)
        {
            /* TODO: Implement the managed record view field mapping. */
            throw new NotImplementedException();
        }
    }

    // Army.PAS:4311
    public override void SetArrayOwner(ECustomNode AOwner, int Index)
    {
        AddRef();
        base.SetArrayOwner(AOwner, Index);
        if (FOwner != null)
        {
            AddRef();
        }

        Changed();
        Release();
    }

    // Army.PAS:4399
    public override string GetOwnerName() => FOwnerName;

    // Army.PAS:4402
    public override Set<AbilitySelectionType> GetAbilitySelectionTypes()
    {
        Set<AbilitySelectionType> Result;
        switch (FItemType)
        {
            case ItemType.Head:
                Result = Set<AbilitySelectionType>.Of(AbilitySelectionType.HeadItem);
                break;
            case ItemType.Torso:
                Result = Set<AbilitySelectionType>.Of(AbilitySelectionType.TorsoItem);
                break;
            case ItemType.Attack:
                Result = Set<AbilitySelectionType>.Of(AbilitySelectionType.AttackItem);
                break;
            case ItemType.Defense:
                Result = Set<AbilitySelectionType>.Of(AbilitySelectionType.DefenseItem);
                break;
            case ItemType.Ring:
                Result = Set<AbilitySelectionType>.Of(AbilitySelectionType.RingItem);
                break;
            case ItemType.Use:
                Result = Set<AbilitySelectionType>.Of(AbilitySelectionType.UseItem);
                break;
            default:
                Result = Set<AbilitySelectionType>.Of();
                break;
        }

        return Result;
    }

    // Army.PAS:4345
    public override void Changed()
    {
        if (FStatus.Contains(ItemStatusElement.Registered))
        {
            AoWHSMap.Current.FItemControl.ItemChanged(this);
        }
    }

    // Army.PAS:4276
    public virtual bool CanUse(Hero Hero, ref string Reason)
    {
        bool Result;
        if (FItemType == ItemType.Scroll && FSpellID != 0)
        {
            if (AoWHSMap.Current.FPlayers.GetPlayers(Hero.FPlayer).FMagicControl.FSpells.IndexOf(FSpellID) == -1)
            {
                Result = true;
            }
            else
            {
                Result = false;
                var spell = AoWHSSet.Current.FSpellControl.GetSpell(FSpellID);
                Reason = AbilityRules.Format(AbilityRules.TranslateRStr(ItemSpellAlreadyResearchedRStr), [spell.FSpellName]);
            }
        }
        else
        {
            Result = false;
            Reason = AbilityRules.TranslateRStr(ItemNotUseableRStr);
        }

        return Result;
    }

    // Army.PAS:4296
    public virtual bool Use(Hero Hero)
    {
        bool Result;
        string Reason = "";
        ItemTokenEvent Event;
        if (CanUse(Hero, ref Reason) && !AoWHSMap.Current.FPlayers.GetPlayers(Hero.FPlayer).GetBusy())
        {
            Event = new ItemTokenEvent(null);
            Event.FItemID = FItemID;
            Event.FHeroID = Hero.FUnitID;
            AoWHSMap.Current.FTokenManager.GetTokenControl(Hero.FPlayer).AddEvent(Event);
            Event.Release();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Army.PAS:4255
    public virtual bool ExecuteUse(Hero Hero)
    {
        string Reason = "";
        // { Original reports success even when CanUse rejects the item. }
        bool Result = true;
        if (CanUse(Hero, ref Reason))
        {
            switch (FItemType)
            {
                case ItemType.Scroll:
                    AoWHSMap.Current.FPlayers.GetPlayers(Hero.FPlayer).FMagicControl.ExecuteSpellResearched(FSpellID);
                    if (AoWHSMap.Current.FSeatedPlayer == Hero.FPlayer)
                    {
                        AudioPlayback.PlaySample("SFX\\ATTACK\\MAGICB01.WAV");
                        var spell = AoWHSSet.Current.FSpellControl.GetSpell(FSpellID);
                        var format = AbilityRules.Format(AbilityRules.TranslateRStr(ItemSpellResearchedRStr), [spell.FSpellName]);
                        AoWHSMap.Current.ShowMessage(format);
                    }

                    Dispose();
                    Hero.Changed();
                    break;
            }
        }

        return Result;
    }

    // Army.PAS:4418
    public virtual void Show(int X, int Y, in LockedSurface Surface)
    {
        ItemGraphics GFX;
        GFX = AoWHSSet.Current.FItemGFXResources.GetItemGFX(FItemGFXIndex);
        if (GFX != null)
        {
            GFX.AddRef();
            GFX.GetItemImage().Show(X, Y, Surface);
            GFX.Release();
        }
    }

    // Army.PAS:4331
    public virtual void Activate()
    {
        if (!FStatus.Contains(ItemStatusElement.Activated))
        {
            if (AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting) || AoWHSMap.Current.FDay == 0)
            {
                UpdateFromLibrary();
            }

            FStatus = FStatus + Set<ItemStatusElement>.Of(ItemStatusElement.Activated);
        }
    }

    // Army.PAS:4340
    public virtual void Deactivate()
    {
        if (FStatus.Contains(ItemStatusElement.Activated))
        {
            FStatus = FStatus - Set<ItemStatusElement>.Of(ItemStatusElement.Activated);
        }
    }

    // Army.PAS:4320
    public void GetLocation(ref sbyte X, ref sbyte Y, ref sbyte L)
    {
        if (FOwner != null && FOwner is ItemList)
        {
            ((ItemList)FOwner).GetLocation(ref X, ref Y, ref L);
        }
        else
        {
            X = -1;
            Y = -1;
            L = -1;
        }
    }

    // Army.PAS:4353
    public int GetCampaignTransferPoints() => GetObtainValue();

    // Army.PAS:4356
    public LibraryImage GetImage()
    {
        LibraryImage Result;
        ItemGraphics GFX;
        GFX = AoWHSSet.Current.FItemGFXResources.GetItemGFX(FItemGFXIndex);
        if (GFX != null)
        {
            GFX.AddRef();
            Result = GFX.GetItemImage();
            GFX.Release();
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Army.PAS:4369
    public bool GetLibraryItem() => FLibraryID != -1;

    // Army.PAS:4372
    public string GetLibraryName()
    {
        string Result = "";
        if (FLibraryID != -1)
        {
            Result = FLibraryName;
            if (IsEmpty(Result))
            {
                Result = AbilityRules.TranslateRStr(ItemSystemLibraryRStr);
            }
        }

        return Result;
    }

    // Army.PAS:4381
    public void UpdateFromLibrary()
    {
        int ID;
        Item Item;
        if (FLibraryID != -1)
        {
            Item = AoWHSSet.Current.FItemLibraries.FindItem(FLibraryName, FLibraryID);
            if (Item != null)
            {
                ID = FItemID;
                Assign(Item, GameTables.AoWEngine);
                FItemID = ID;
                Changed();
            }
            else
            {
                if (FOwner == null)
                {
                    this.Dispose();
                }
                else
                {
                    FLibraryID = -1;
                }
            }
        }
    }

    // Army.PAS:4415
    public void SetItemGFXIndex(int Value)
    {
        FItemGFXIndex = Value;
    }

    // Army.PAS:4430
    public void SetItemType(ItemType Value)
    {
        ItemType ItemType;
        FAttack = 0;
        FDefense = 0;
        FResistance = 0;
        FDamage = 0;
        ItemType = Value;
        FItemType = ItemType;
        FItemGFXIndex = AoWHSSet.Current.FItemGFXResources.FindItemTypeGFX(ItemType);
        base.Clear();
    }

    // Army.PAS:4443
    public int GetObtainValue()
    {
        int Result;
        if (FObtainValue > 10)
        {
            Result = FObtainValue;
        }
        else
        {
            Result = 10;
        }

        return Result;
    }

    // Army.PAS:4474
    public bool PlaceOnMap(GameMap Map, sbyte X, sbyte Y, sbyte L)
    {
        bool Result;
        ItemHexSprite HS;
        if (X >= 0 && Y >= 0 && L >= 0 && X < Map.MapContainer.Width && Y < Map.MapContainer.Height && L < Map.MapContainer.LevelCount && Map.MapContainer.GetField(X, Y, L).TerrainType != 15)
        {
            HS = (ItemHexSprite)Map.MapContainer.GetField(X, Y, L).FindOwnedChild(131697);
            if (HS == null)
            {
                HS = new ItemHexSprite(null);
                if (!Map.PlaceHX(HS, X, Y, L))
                {
                    HS?.Dispose();
                    HS = null;
                }
            }

            if (HS != null)
            {
                SetOwner(HS.FItems);
                Result = true;
            }
            else
            {
                Result = false;
            }
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // Army.PAS:4501
    public void EditDropMsg(ref DropMessage Msg)
    {
        int X = default;
        int Y = default;
        int L;
        L = Msg.Lhp;
        HexGeometry.HPtoHX(Msg.Xhp, Msg.Yhp, ref X, ref Y);
        if (!PlaceOnMap(AoWHSMap.Current, (sbyte)X, (sbyte)Y, (sbyte)L))
        {
            Msg.Result = -1;
        }
        else
        {
            Msg.Result = 0;
        }
    }

    private static readonly string ItemSpellResearchedRStr = "%s researched";
    private static readonly string ItemSpellAlreadyResearchedRStr = "%s already researched";
    private static readonly string ItemNotUseableRStr = "This is not an usable item";
    private static readonly string ItemSystemLibraryRStr = "System";

    public enum Availability : byte
    {
        Exceptional = 0, Rare = 1, Common = 2, Frequent = 3
    }

    // { Interface declaration run owned by P2. }
    // The editor drop message carries screen X/Y, level, and result.
    public struct DropMessage
    {
        public int MsgID;
        public int Seq;
        public EngineObject Sender;
        public Int32Array3 Param;
        public int Xhp;
        public int Yhp;
        public int Lhp;
        public int Reserved0;
        public int Reserved1;
        public int Result;
    }

    // intf_P2_04.inc:84

    // intf_P2_04.inc:84

}
