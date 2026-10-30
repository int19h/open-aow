// Translated from EList.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace Aow.Units.EList
{
    // { EList.TListObject (instance size 4): no fields. TObjectList.LoadFromStream constructs
//     items with their ordinary constructor. The stream methods are empty here and overridden
//     by the items. }
    public partial class TListObject : Aow.Rtl.TObject
    {
        // EList.pas:50
        public virtual void SaveToStream(Aow.Rtl.TStream Stream)
        {
        }

        // EList.pas:54
        public virtual void LoadFromStream(Aow.Rtl.TStream Stream)
        {
        }

    }

    // { EList.TObjectList (instance size 16): a TList that owns its TListObjects. A slot may be nil;
//     the stream format is the count followed by a nil flag per slot and the item's own data. }
    public partial class TObjectList : Aow.Rtl.TList
    {
        // EList.pas:66
        internal Aow.Units.EList.TListObject GetListObject(int Index)
        {
            Aow.Units.EList.TListObject Result = default;
            Result = (Aow.Units.EList.TListObject)List[Index];
            return Result;
        }

        // EList.pas:71
        internal void SetListObject(int Index, Aow.Units.EList.TListObject Value)
        {
            while (Index >= Count)
            {
                Add(null);
            }

            if (List[Index] != null)
            {
                ((Aow.Units.EList.TListObject)List[Index]).Destroy();
            }

            List[Index] = Value;
        }

        // EList.pas:60
        // { TObjectList }
        public override void Destroy()
        {
            DestroyItems();
            base.Destroy();
        }

        // EList.pas:78
        public void DestroyItems()
        {
            int I = default;
            {
                I = Count - 1;
                byte end__1 = 0;
                for (; I >= end__1; I -= 1)
                {
                    if (List[I] != null)
                    {
                        ((Aow.Units.EList.TListObject)List[I]).Destroy();
                    }

                    Delete(I);
                }
            }
        }

        // EList.pas:89
        public void LoadFromStream(Aow.Rtl.TStream Stream)
        {
            int N = default;
            bool IsNil = default;
            int I = default;
            Aow.Units.EList.TListObject Obj = default;
            DestroyItems();
            Stream.Read(Aow.Rtl.Bytes.Of(ref N, 4));
            {
                I = 0;
                int end__2 = N - 1;
                for (; I <= end__2; I += 1)
                {
                    Stream.Read(Aow.Rtl.Bytes.Of(ref IsNil, 1));
                    if (!IsNil)
                    {
                        Obj = new Aow.Units.EList.TListObject();
                        Obj.LoadFromStream(Stream);
                        Add(Obj);
                    }
                    else
                    {
                        Add(null);
                    }
                }
            }
        }

        // EList.pas:112
        public void SaveToStream(Aow.Rtl.TStream Stream)
        {
            bool IsNil = default;
            int I = default;
            Stream.Write(Aow.Rtl.Rtl.ValueBytes<int>(Count, 4));
            {
                I = 0;
                int end__3 = Count - 1;
                for (; I <= end__3; I += 1)
                {
                    IsNil = List[I] == null;
                    Stream.Write(Aow.Rtl.Bytes.Of(ref IsNil, 1));
                    if (!IsNil)
                    {
                        ((Aow.Units.EList.TListObject)List[I]).SaveToStream(Stream);
                    }
                }
            }
        }

        public new Aow.Units.EList.TListObject this[int Index] { get => GetListObject(Index); set => SetListObject(Index, value); }
    }

    public static partial class EList
    {
    }
}
