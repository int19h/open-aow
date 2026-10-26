5 TODOs

# Engine emission report

## 1. Units processed

| Unit | Lines in | Lines out |
|---|---:|---:|
| EList | 126 | 184 |
| Engine | 11392 | 16029 |
| FMemStr | 189 | 294 |
| PlugInM | 146 | 206 |

## 2. Errors

- D:\git\aow\src\delphi3\Engine.pas:8530:3: T-4.4, Image-library drawing is outside the Engine set. Source: `BltLibraryImage(Canvas, GetCellImage, Rect, Mode, ScaleX, ScaleY, BkColor, Color, False, R);`
- D:\git\aow\src\delphi3\Engine.pas:9391:14: T-5.4, Native Delphi packages have no managed loader. Source: `H := LoadPackage(Dir + SR.Name);`
- D:\git\aow\src\delphi3\Engine.pas:9429:7: T-5.4, Native Delphi packages have no managed loader. Source: `UnloadPackage(HMODULE(S));`
- D:\git\aow\src\delphi3\PlugInM.pas:111:31: T-5.4, Native Delphi packages have no managed loader. Source: `Objects[I] := TObject(LoadPackage(Strings[I]));`
- D:\git\aow\src\delphi3\PlugInM.pas:130:11: T-5.4, Native Delphi packages have no managed loader. Source: `UnloadPackage(HMODULE(Objects[I]));`

## 3. Normalizations


### N10: 25

- D:\git\aow\src\delphi3\Engine.pas:3280:7: `System.Move` -> `keep System.Move`
- D:\git\aow\src\delphi3\Engine.pas:3306:5: `System.Move` -> `keep System.Move`
- D:\git\aow\src\delphi3\Engine.pas:3373:5: `System.Move` -> `keep System.Move`
- D:\git\aow\src\delphi3\Engine.pas:3560:7: `System.Delete` -> `keep System.Delete`
- D:\git\aow\src\delphi3\Engine.pas:3615:7: `System.Move` -> `keep System.Move`
- D:\git\aow\src\delphi3\Engine.pas:3641:5: `System.Move` -> `keep System.Move`
- D:\git\aow\src\delphi3\Engine.pas:3708:5: `System.Move` -> `keep System.Move`
- D:\git\aow\src\delphi3\Engine.pas:3906:7: `System.Delete` -> `keep System.Delete`
- D:\git\aow\src\delphi3\Engine.pas:3962:7: `System.Move` -> `keep System.Move`
- D:\git\aow\src\delphi3\Engine.pas:3988:5: `System.Move` -> `keep System.Move`
- D:\git\aow\src\delphi3\Engine.pas:4055:5: `System.Move` -> `keep System.Move`
- D:\git\aow\src\delphi3\Engine.pas:4273:7: `System.Move` -> `keep System.Move`
- D:\git\aow\src\delphi3\Engine.pas:4299:5: `System.Move` -> `keep System.Move`
- D:\git\aow\src\delphi3\Engine.pas:4366:5: `System.Move` -> `keep System.Move`
- D:\git\aow\src\delphi3\Engine.pas:4583:7: `System.Move` -> `keep System.Move`
- D:\git\aow\src\delphi3\Engine.pas:4822:7: `System.Delete` -> `keep System.Delete`
- D:\git\aow\src\delphi3\Engine.pas:4866:7: `System.Move` -> `keep System.Move`
- D:\git\aow\src\delphi3\Engine.pas:4892:5: `System.Move` -> `keep System.Move`
- D:\git\aow\src\delphi3\Engine.pas:4963:5: `System.Move` -> `keep System.Move`
- D:\git\aow\src\delphi3\Engine.pas:5068:7: `System.Move` -> `keep System.Move`
- D:\git\aow\src\delphi3\Engine.pas:5103:5: `System.Move` -> `keep System.Move`
- D:\git\aow\src\delphi3\Engine.pas:5184:5: `System.Move` -> `keep System.Move`
- D:\git\aow\src\delphi3\Engine.pas:5464:7: `System.Move` -> `keep System.Move`
- D:\git\aow\src\delphi3\Engine.pas:5501:5: `System.Move` -> `keep System.Move`
- D:\git\aow\src\delphi3\Engine.pas:5597:5: `System.Move` -> `keep System.Move`

### N12: 2

- D:\git\aow\src\delphi3\Engine.pas:1459:3: `VariableDeclaration` -> `EngineLoaded aliases VerifyEObjectEnabled`
- D:\git\aow\src\delphi3\Engine.pas:11123:3: `EngineLoaded` -> `VerifyEObjectEnabled`

### N3: 1

- D:\git\aow\src\delphi3\Engine.pas:8443:11: `TListFields(FEObjects).FCount` -> `TList.FCount`

### N5: 2

- D:\git\aow\src\delphi3\Engine.pas:4954:6: `TMethod(FList^[Result]).Code <> M.Code` -> `delegate inequality`
- D:\git\aow\src\delphi3\Engine.pas:5008:49: `TMethod(FList^[I]).Code <> M.Code` -> `delegate inequality`

### N7: 6

- D:\git\aow\src\delphi3\Engine.pas:2225:12: `Integer(Strings.Objects[I])` -> `Strings.Objects[I].InstanceId`
- D:\git\aow\src\delphi3\Engine.pas:8687:18: `Integer(FEditors.Objects[I])` -> `FEditors.Objects[I].InstanceId`
- D:\git\aow\src\delphi3\Engine.pas:8706:15: `Integer(FEditors.Objects[I])` -> `FEditors.Objects[I].InstanceId`
- D:\git\aow\src\delphi3\Engine.pas:10703:14: `Integer(FSections.Objects[Index])` -> `FSections.Objects[Index].InstanceId`
- D:\git\aow\src\delphi3\Engine.pas:10705:15: `Integer(FSections.Objects[Index + 1])` -> `FSections.Objects[Index + 1].InstanceId`
- D:\git\aow\src\delphi3\PlugInM.pas:130:25: `HMODULE(Objects[I])` -> `Objects[I].InstanceId`

## 4. Overrides

Override sources compile into Aow.Generated. Partial classes share one assembly, which preserves access to generated internal fields. No separate override assembly is used.

- Engine.FillInteger: assembly fill, Pascal equivalent from asm-routines.json, file `src/Overrides/Engine/Fill.cs`, exists: true
- Engine.FillWord: assembly fill, Pascal equivalent from asm-routines.json, file `src/Overrides/Engine/Fill.cs`, exists: true
- Engine.FillLargeInteger: assembly fill, Pascal equivalent from asm-routines.json, file `src/Overrides/Engine/Fill.cs`, exists: true
- Engine.FillQuadInteger: assembly fill, Pascal equivalent from asm-routines.json, file `src/Overrides/Engine/Fill.cs`, exists: true
- Engine.TBitList.Get: assembly bit test, Pascal equivalent from asm-routines.json, file `src/Overrides/Engine/TBitList.cs`, exists: true
- Engine.TBitList.Put: assembly bit update, Pascal equivalent from asm-routines.json, file `src/Overrides/Engine/TBitList.cs`, exists: true
- Engine.TECopyComponent: editor clipboard boundary stub from design section 3, file `src/Overrides/Engine/Clipboard.cs`, exists: true
- Engine.CopyStreamToClipboard: editor clipboard boundary stub from design section 3, file `src/Overrides/Engine/Clipboard.cs`, exists: true
- Engine.PasteStreamFromClipboard: editor clipboard boundary stub from design section 3, file `src/Overrides/Engine/Clipboard.cs`, exists: true
- Engine.TEObjectEditor.UpdateMultiPopupMenu: editor popup boundary stub from design section 3, file `src/Overrides/Engine/Popup.cs`, exists: true
- Engine.TEObjectEditor.UpdatePopupMenu: editor popup boundary stub from design section 3, file `src/Overrides/Engine/Popup.cs`, exists: true

Assembler mappings: 8.

- D:\git\aow\src\delphi3\Engine.pas:3312:3: list-error, raise without ReturnAddr
- D:\git\aow\src\delphi3\Engine.pas:3647:3: list-error, raise without ReturnAddr
- D:\git\aow\src\delphi3\Engine.pas:3994:3: list-error, raise without ReturnAddr
- D:\git\aow\src\delphi3\Engine.pas:4305:3: list-error, raise without ReturnAddr
- D:\git\aow\src\delphi3\Engine.pas:4615:3: list-error, raise without ReturnAddr
- D:\git\aow\src\delphi3\Engine.pas:4898:3: list-error, raise without ReturnAddr
- D:\git\aow\src\delphi3\Engine.pas:5109:3: list-error, raise without ReturnAddr
- D:\git\aow\src\delphi3\Engine.pas:5507:3: list-error, raise without ReturnAddr

## 5. Unchecked casts

0 manifest marks.

## 6. rwData calls

3 calls.

- D:\git\aow\src\delphi3\Engine.pas:8245:3: TSet image, TEControlStyle
- D:\git\aow\src\delphi3\Engine.pas:8217:3: TSet image, TEControlStyle
- D:\git\aow\src\delphi3\Engine.pas:8285:3: TSet image, TEControlStyle

## 7. Destructor review

- D:\git\aow\src\delphi3\EList.pas:60:1: TObjectList.Destroy
- D:\git\aow\src\delphi3\Engine.pas:3268:1: TIntegerList.Destroy
- D:\git\aow\src\delphi3\Engine.pas:3603:1: TByteList.Destroy
- D:\git\aow\src\delphi3\Engine.pas:3950:1: TShortIntList.Destroy
- D:\git\aow\src\delphi3\Engine.pas:4261:1: TWordList.Destroy
- D:\git\aow\src\delphi3\Engine.pas:4571:1: TBitList.Destroy
- D:\git\aow\src\delphi3\Engine.pas:4854:1: TEventList.Destroy
- D:\git\aow\src\delphi3\Engine.pas:5056:1: TDoubleItemList.Destroy
- D:\git\aow\src\delphi3\Engine.pas:5452:1: TQuadItemList.Destroy
- D:\git\aow\src\delphi3\Engine.pas:2253:1: TPropertyTable.Destroy
- D:\git\aow\src\delphi3\Engine.pas:2436:1: TEObject.Destroy
- D:\git\aow\src\delphi3\Engine.pas:2602:1: TECustomNode.Destroy
- D:\git\aow\src\delphi3\Engine.pas:8745:1: TEngine.Destroy
- D:\git\aow\src\delphi3\Engine.pas:9153:1: TPlugIn.Destroy
- D:\git\aow\src\delphi3\Engine.pas:9262:1: TPlugInControl.Destroy
- D:\git\aow\src\delphi3\Engine.pas:7212:1: TEChangeNotifyNode.Destroy
- D:\git\aow\src\delphi3\inc\HashTab.PAS:6:1: THashList.Destroy
- D:\git\aow\src\delphi3\Engine.pas:1472:1: TESortNode.Destroy
- D:\git\aow\src\delphi3\Engine.pas:7112:1: TEUser.Destroy
- D:\git\aow\src\delphi3\Engine.pas:8635:1: TEComponent.Destroy
- D:\git\aow\src\delphi3\Engine.pas:5852:1: TCallBackEventList.Destroy
- D:\git\aow\src\delphi3\Engine.pas:10343:1: TEWriteTextStorageStream.Destroy
- D:\git\aow\src\delphi3\Engine.pas:9575:1: TEWriteStorageStream.Destroy
- D:\git\aow\src\delphi3\Engine.pas:10499:1: TDebugWriteStream.Destroy
- D:\git\aow\src\delphi3\FMemStr.pas:90:1: TFastMemoryStream.Destroy
- D:\git\aow\src\delphi3\PlugInM.pas:53:1: TPlugInManager.Destroy

## 8. Qualified collisions


## 9. Shim demand

See [shim-demand.md](shim-demand.md). Counts refer to emitted expression uses and the imported types in generated declarations. Constants and intrinsics retain their Delphi names in the list even when they fold into C# operators. Overridden bodies are excluded.


## 10. Catalogue checks and initialization

The emit driver requires zero catalogue disagreements before writing any output. Source-derived checks use --source-catalogues.

Initialization table order: FMemStr, Engine, EList.
Finalization table order: EList, Engine, FMemStr.
Skipped null table positions: 3, 35, 42, 325.

| Catalogue check | Count | Disagreements |
|---|---:|---:|
| assembler routines | 14 | 0 |
| class fields | 120 | 0 |
| class layouts | 62 | 0 |
| initialization sections | 1 | 0 |
| layout views | 1 | 0 |
| shape casts | 1 | 0 |

Found only by the transpiler: 0. Not covered by a catalogue: 0. Empty initialization sections outside the table: 0.


Evaluation orders lowered: 344. The generic argument, operand, receiver and short-circuit shapes have executable emitter tests. The following source occurrences remain review entries for an oracle probe with the same callees.

- D:\git\aow\src\delphi3\Engine.pas:3381:3: Get(FCount-1)
- D:\git\aow\src\delphi3\Engine.pas:3414:35: this[I]
- D:\git\aow\src\delphi3\Engine.pas:3475:5: Aow.Rtl.Rtl.CompareMem(MemoryMarshal.AsBytes(FList.Array.AsSpan(FList.Index,FCount)),MemoryMarshal.AsBytes(List.FList.Array.AsSpan(List.FList.Index,FCount)),FCount*4)
- D:\git\aow\src\delphi3\Engine.pas:3716:3: Get(FCount-1)
- D:\git\aow\src\delphi3\Engine.pas:3749:35: this[I]
- D:\git\aow\src\delphi3\Engine.pas:3810:5: Aow.Rtl.Rtl.CompareMem(FList.Array.AsSpan(FList.Index,FCount*1),List.FList.Array.AsSpan(List.FList.Index,FCount*1),FCount*1)
- D:\git\aow\src\delphi3\Engine.pas:4063:3: Get(FCount-1)
- D:\git\aow\src\delphi3\Engine.pas:4096:35: this[I]
- D:\git\aow\src\delphi3\Engine.pas:4157:5: Aow.Rtl.Rtl.CompareMem(MemoryMarshal.AsBytes(FList.Array.AsSpan(FList.Index,FCount*1)),MemoryMarshal.AsBytes(List.FList.Array.AsSpan(List.FList.Index,FCount*1)),FCount*1)
- D:\git\aow\src\delphi3\Engine.pas:4374:3: Get(FCount-1)
- D:\git\aow\src\delphi3\Engine.pas:4407:35: this[I]
- D:\git\aow\src\delphi3\Engine.pas:4468:5: Aow.Rtl.Rtl.CompareMem(MemoryMarshal.AsBytes(FList.Array.AsSpan(FList.Index,FCount)),MemoryMarshal.AsBytes(List.FList.Array.AsSpan(List.FList.Index,FCount)),FCount*2)
- D:\git\aow\src\delphi3\Engine.pas:4804:5: Get(I)
- D:\git\aow\src\delphi3\Engine.pas:4610:37: Get(I+1)
- D:\git\aow\src\delphi3\Engine.pas:4631:3: Get(Index2)
- D:\git\aow\src\delphi3\Engine.pas:4691:3: (Result<FCount)
- D:\git\aow\src\delphi3\Engine.pas:4705:41: Get(Index)
- D:\git\aow\src\delphi3\Engine.pas:4714:3: Get(FCount-1)
- D:\git\aow\src\delphi3\Engine.pas:4740:35: this[I]
- D:\git\aow\src\delphi3\Engine.pas:4779:5: Aow.Rtl.Rtl.CompareMem(FList.Array.AsSpan(FList.Index,(int)(((uint)(FCount+7))>>3)),List.FList.Array.AsSpan(List.FList.Index,(int)(((uint)(FCount+7))>>3)),(int)(((uint)(FCount+7))>>3))
- D:\git\aow\src\delphi3\Engine.pas:4971:3: Get(FCount-1)
- D:\git\aow\src\delphi3\Engine.pas:5043:5: Aow.Rtl.Rtl.PointerSequenceEqual(FList,List.FList,FCount)
- D:\git\aow\src\delphi3\Engine.pas:5192:3: Get(FCount-1)
- D:\git\aow\src\delphi3\Engine.pas:5228:5: this[I]
- D:\git\aow\src\delphi3\Engine.pas:5331:5: Aow.Rtl.Rtl.CompareMem(MemoryMarshal.AsBytes(FList.Array.AsSpan(FList.Index,FCount)),MemoryMarshal.AsBytes(List.FList.Array.AsSpan(List.FList.Index,FCount)),FCount*8)
- D:\git\aow\src\delphi3\Engine.pas:5605:3: Get(FCount-1)
- D:\git\aow\src\delphi3\Engine.pas:5781:5: Aow.Rtl.Rtl.CompareMem(MemoryMarshal.AsBytes(FList.Array.AsSpan(FList.Index,FCount)),MemoryMarshal.AsBytes(List.FList.Array.AsSpan(List.FList.Index,FCount)),FCount*16)
- D:\git\aow\src\delphi3\Engine.pas:2560:3: FOwner.GetEngine()
- D:\git\aow\src\delphi3\Engine.pas:2516:3: ClassType.ClassID(), Obj.ClassType.ClassID()
- D:\git\aow\src\delphi3\Engine.pas:2522:5: S1.Size, S2.Size, (call__76==call__77)
- D:\git\aow\src\delphi3\Engine.pas:2477:7: ((Aow.Units.Engine.TEObject.Meta)C).ClassID(), E.FEditors.FindEditor(call__81)
- D:\git\aow\src\delphi3\Engine.pas:2631:3: Count
- D:\git\aow\src\delphi3\Engine.pas:2635:7: Children[I]
- D:\git\aow\src\delphi3\Engine.pas:2641:7: Children[I]
- D:\git\aow\src\delphi3\Engine.pas:2643:9: (C.Owner==this)
- D:\git\aow\src\delphi3\Engine.pas:2615:5: Children[I]
- D:\git\aow\src\delphi3\Engine.pas:2618:3: Count
- D:\git\aow\src\delphi3\Engine.pas:2621:7: Children[I]
- D:\git\aow\src\delphi3\Engine.pas:2658:34: Children[I]
- D:\git\aow\src\delphi3\Engine.pas:2683:9: Children[I]
- D:\git\aow\src\delphi3\Engine.pas:2684:9: (C!=null)
- D:\git\aow\src\delphi3\Engine.pas:2693:9: Children[I]
- D:\git\aow\src\delphi3\Engine.pas:2694:9: (C!=null)
- D:\git\aow\src\delphi3\Engine.pas:2704:9: Children[I]
- D:\git\aow\src\delphi3\Engine.pas:2710:9: Children[I]
- D:\git\aow\src\delphi3\Engine.pas:2795:5: Children[I]
- D:\git\aow\src\delphi3\Engine.pas:2776:5: (Result==null)
- D:\git\aow\src\delphi3\Engine.pas:2778:7: Children[I]
- D:\git\aow\src\delphi3\Engine.pas:2779:9: Children[I], ((Aow.Units.Engine.TECustomNode)call__114).FindChildInTree(AClassID)
- D:\git\aow\src\delphi3\Engine.pas:2735:3: !Found
- D:\git\aow\src\delphi3\Engine.pas:2737:5: Children[I]
- D:\git\aow\src\delphi3\Engine.pas:2753:3: !Found
- D:\git\aow\src\delphi3\Engine.pas:2755:5: Children[I]
- D:\git\aow\src\delphi3\Engine.pas:2756:5: (C!=null)&&(C.Owner==this)
- D:\git\aow\src\delphi3\Engine.pas:2764:3: Count
- D:\git\aow\src\delphi3\Engine.pas:2765:3: (Result>=0)
- D:\git\aow\src\delphi3\Engine.pas:8779:3: FDebugIDs.IndexOfObject(Aow.Rtl.Rtl.IntToObject(Result))
- D:\git\aow\src\delphi3\Engine.pas:8733:3: Aow.Units.Engine.TEventList.Create()
- D:\git\aow\src\delphi3\Engine.pas:8734:3: Aow.Units.Engine.TEObjectClassList.Create()
- D:\git\aow\src\delphi3\Engine.pas:8735:3: Aow.Units.Engine.TIntegerHashTable256.Create()
- D:\git\aow\src\delphi3\Engine.pas:8736:3: Aow.Units.Engine.TEObjectEditorList.Create()
- D:\git\aow\src\delphi3\Engine.pas:8737:3: Aow.Units.Engine.TEControlStyleNode.Create(this)
- D:\git\aow\src\delphi3\Engine.pas:8739:3: newAow.Rtl.TStringList()
- D:\git\aow\src\delphi3\Engine.pas:8768:3: Aow.Rtl.Rtl.Application.ExeName, Aow.Rtl.Rtl.ExtractFilePath(call__134)
- D:\git\aow\src\delphi3\Engine.pas:8862:3: FClassTable.Find(AClassID)
- D:\git\aow\src\delphi3\Engine.pas:8792:3: FDebugIDs.IndexOfObject(Aow.Rtl.Rtl.IntToObject(ID))
- D:\git\aow\src\delphi3\Engine.pas:8842:5: AClasses[I].ClassID(), FClassTable.Find(call__139)
- D:\git\aow\src\delphi3\Engine.pas:8845:5: AClasses[I].ClassID()
- D:\git\aow\src\delphi3\Engine.pas:8856:5: AClasses[I].ClassID()
- D:\git\aow\src\delphi3\Engine.pas:8874:5: FEClasses[I], ((Aow.Units.Engine.TEObject.Meta)call__145).InheritsFrom(AClass)
- D:\git\aow\src\delphi3\Engine.pas:8874:62: FEClasses[I]
- D:\git\aow\src\delphi3\Engine.pas:9008:7: M.Size
- D:\git\aow\src\delphi3\Engine.pas:9046:5: M.Size, Aow.Units.Engine.Engine.GetCRC32(Ptr<byte>.To(M.Memory,0),call__149)
- D:\git\aow\src\delphi3\Engine.pas:9048:5: M.Size, Aow.Units.Engine.Engine.GetCRC32(Ptr<byte>.To(M.Memory,0),call__151)
- D:\git\aow\src\delphi3\Engine.pas:9052:7: M.Size
- D:\git\aow\src\delphi3\Engine.pas:9073:5: M.Size
- D:\git\aow\src\delphi3\Engine.pas:9076:7: M.Size
- D:\git\aow\src\delphi3\Engine.pas:9077:7: M.Size, Aow.Units.Engine.Engine.GetCRC32(Ptr<byte>.To(M.Memory,0)+4,call__156-4), (CRC1==call__157)
- D:\git\aow\src\delphi3\Engine.pas:9132:3: Aow.Rtl.Rtl.GetHeapStatus()
- D:\git\aow\src\delphi3\Engine.pas:9133:3: AClass.Create(null)
- D:\git\aow\src\delphi3\Engine.pas:9135:5: Aow.Rtl.Rtl.GetHeapStatus()
- D:\git\aow\src\delphi3\Engine.pas:9136:7: Aow.Rtl.Rtl.GetHeapStatus(), Aow.Rtl.Exception.Create(((((AnsiString)AClass.ClassName)+((AnsiString)" contains memory leak constructor/destructor("))+Aow.Rtl.Rtl.IntToStr(((int)call__164.TotalAllocated)-Allocated))+((AnsiString)" bytes)"))
- D:\git\aow\src\delphi3\Engine.pas:8358:3: AClass.EditorClassID(), FindEditor(call__166)
- D:\git\aow\src\delphi3\Engine.pas:8369:3: (I!=Count)
- D:\git\aow\src\delphi3\Engine.pas:8561:5: GetEngine(), call__170.CreateEObject(UserClassID,null)
- D:\git\aow\src\delphi3\Engine.pas:8530:3: GetCellImage()
- D:\git\aow\src\delphi3\Engine.pas:1739:9: D.Read(Buffer.Array.AsSpan(Buffer.Index,8096))
- D:\git\aow\src\delphi3\Engine.pas:1748:5: Stream.Size, Stream.Position
- D:\git\aow\src\delphi3\Engine.pas:1759:3: Stream.Size
- D:\git\aow\src\delphi3\Engine.pas:1767:7: Stream.Size, Size
- D:\git\aow\src\delphi3\Engine.pas:9147:3: newAow.Rtl.TList()
- D:\git\aow\src\delphi3\Engine.pas:9148:3: Aow.Units.Engine.TIntegerList.Create()
- D:\git\aow\src\delphi3\Engine.pas:9155:32: FEngines[FEngines.Count-1]
- D:\git\aow\src\delphi3\Engine.pas:9183:3: FEngines.IndexOf(AEngine)
- D:\git\aow\src\delphi3\Engine.pas:9192:5: FCounts[I]
- D:\git\aow\src\delphi3\Engine.pas:9201:3: FEngines.IndexOf(AEngine)
- D:\git\aow\src\delphi3\Engine.pas:9204:5: FCounts[I]
- D:\git\aow\src\delphi3\Engine.pas:9205:5: FCounts[I]
- D:\git\aow\src\delphi3\Engine.pas:9213:7: FCounts[I]
- D:\git\aow\src\delphi3\Engine.pas:9307:5: FUsedPlugIns[I], Aow.Units.Engine.Engine.FindPlugIn(call__189)
- D:\git\aow\src\delphi3\Engine.pas:9313:7: FUsedPlugIns[I], Aow.Units.Engine.EPlugInMissing.Create((((AnsiString)"PlugIn Missing (")+call__191)+AnsiString.FromBytes(41))
- D:\git\aow\src\delphi3\Engine.pas:9326:5: FUsedPlugIns[I], Aow.Units.Engine.Engine.FindPlugIn(call__194)
- D:\git\aow\src\delphi3\Engine.pas:9255:3: newAow.Rtl.TStringList()
- D:\git\aow\src\delphi3\Engine.pas:7138:3: this[Index]
- D:\git\aow\src\delphi3\Engine.pas:7155:5: (List[I]!=null)
- D:\git\aow\src\delphi3\Engine.pas:7170:5: (List[I]!=null)
- D:\git\aow\src\delphi3\Engine.pas:7197:5: (List[I]==null)
- D:\git\aow\src\delphi3\Engine.pas:2810:3: Aow.Units.Engine.TEObjectList.Create()
- D:\git\aow\src\delphi3\Engine.pas:2844:3: Count
- D:\git\aow\src\delphi3\Engine.pas:2850:9: (C.Owner==this)
- D:\git\aow\src\delphi3\Engine.pas:2873:3: Count
- D:\git\aow\src\delphi3\Engine.pas:2897:17: w__215.GetLastMsg()
- D:\git\aow\src\delphi3\Engine.pas:2910:17: w__218.GetLastMsg()
- D:\git\aow\src\delphi3\Engine.pas:2943:17: w__225.GetLastMsg()
- D:\git\aow\src\delphi3\Engine.pas:2955:17: w__228.GetLastMsg()
- D:\git\aow\src\delphi3\Engine.pas:2985:5: FChildren.IndexOf(Child)
- D:\git\aow\src\delphi3\Engine.pas:2989:5: FChildren.Remove(Child)
- D:\git\aow\src\delphi3\Engine.pas:3018:3: Count
- D:\git\aow\src\delphi3\Engine.pas:3030:5: (C!=null)
- D:\git\aow\src\delphi3\Engine.pas:1866:5: FTimerLists[Result-1].IndexOf(Child)
- D:\git\aow\src\delphi3\Engine.pas:1825:23: newAow.Rtl.TList()
- D:\git\aow\src\delphi3\Engine.pas:1878:5: FTimerLists[L-1].Remove(Child)
- D:\git\aow\src\delphi3\Engine.pas:1887:3: FTimerLists[1-1].Add(Child)
- D:\git\aow\src\delphi3\Engine.pas:1917:3: FTimerLists[Timer-1].Add(Child)
- D:\git\aow\src\delphi3\Engine.pas:1923:3: FTimerLists[Timer-1].Remove(Child)
- D:\git\aow\src\delphi3\Engine.pas:7208:3: Aow.Units.Engine.TEObjectList.Create()
- D:\git\aow\src\delphi3\Engine.pas:7235:7: (C.Owner==this)
- D:\git\aow\src\delphi3\Engine.pas:7258:3: Count
- D:\git\aow\src\delphi3\Engine.pas:7996:17: w__257.GetLastMsg()
- D:\git\aow\src\delphi3\Engine.pas:8009:17: w__260.GetLastMsg()
- D:\git\aow\src\delphi3\Engine.pas:8042:17: w__267.GetLastMsg()
- D:\git\aow\src\delphi3\Engine.pas:8054:17: w__270.GetLastMsg()
- D:\git\aow\src\delphi3\Engine.pas:8084:5: FChildren.IndexOf(Child)
- D:\git\aow\src\delphi3\Engine.pas:8088:5: FChildren.Remove(Child)
- D:\git\aow\src\delphi3\Engine.pas:8119:3: Count
- D:\git\aow\src\delphi3\Engine.pas:8128:3: Aow.Units.Engine.TEventList.Create()
- D:\git\aow\src\delphi3\inc\HashTab.PAS:88:24: Aow.Units.Engine.TIntegerHashList.Create()
- D:\git\aow\src\delphi3\inc\HashTab.PAS:118:3: FLists[Key&255].Find(Key)
- D:\git\aow\src\delphi3\inc\HashTab.PAS:123:3: FLists[Key&255].Find(Key)
- D:\git\aow\src\delphi3\inc\HashTab.PAS:139:3: FLists[MinIndex].Find((I<<8)+MinIndex)
- D:\git\aow\src\delphi3\inc\HashTab.PAS:204:24: Aow.Units.Engine.TStringHashList.Create()
- D:\git\aow\src\delphi3\inc\HashTab.PAS:255:3: FLists[H].Find(Key)
- D:\git\aow\src\delphi3\Engine.pas:8298:3: this[Index]
- D:\git\aow\src\delphi3\Engine.pas:8310:5: this[I], ((Aow.Units.Engine.TEObject.Meta)call__296).ClassID()
- D:\git\aow\src\delphi3\Engine.pas:8313:17: this[I-1]
- D:\git\aow\src\delphi3\Engine.pas:8325:5: this[I], ((Aow.Units.Engine.TEObject.Meta)call__299).ClassID()
- D:\git\aow\src\delphi3\Engine.pas:8340:3: AClass.ClassID(), FindClass(call__301), (call__302==null)
- D:\git\aow\src\delphi3\Engine.pas:8350:3: AClass.ClassID(), FindClassIndex(call__305)
- D:\git\aow\src\delphi3\Engine.pas:8684:3: FEditors.Count
- D:\git\aow\src\delphi3\Engine.pas:8687:5: FEditors.Objects[I]
- D:\git\aow\src\delphi3\Engine.pas:8690:7: FEditors.Count
- D:\git\aow\src\delphi3\Engine.pas:8653:3: newAow.Rtl.TStringList()
- D:\git\aow\src\delphi3\Engine.pas:8672:3: FEditors.IndexOf(S)
- D:\git\aow\src\delphi3\Engine.pas:8704:3: FEditors.IndexOf(Name)
- D:\git\aow\src\delphi3\Engine.pas:8706:5: FEditors.Objects[I]
- D:\git\aow\src\delphi3\Engine.pas:8716:3: FEditors.IndexOf(Name)
- D:\git\aow\src\delphi3\Engine.pas:8179:3: FChildren.Remove(Child)
- D:\git\aow\src\delphi3\Engine.pas:8607:5: GetResource(Index)
- D:\git\aow\src\delphi3\Engine.pas:8626:7: GetResource(Index)
- D:\git\aow\src\delphi3\Engine.pas:8629:7: GetResource(Index)
- D:\git\aow\src\delphi3\Engine.pas:5849:3: newAow.Rtl.TList()
- D:\git\aow\src\delphi3\Engine.pas:10657:13: Text.Count
- D:\git\aow\src\delphi3\Engine.pas:10700:3: FSections.IndexOf(FPath)
- D:\git\aow\src\delphi3\Engine.pas:10703:5: FSections.Objects[Index]
- D:\git\aow\src\delphi3\Engine.pas:10704:5: FSections.Count
- D:\git\aow\src\delphi3\Engine.pas:10705:7: FSections.Objects[Index+1]
- D:\git\aow\src\delphi3\Engine.pas:10707:7: FText.Count
- D:\git\aow\src\delphi3\Engine.pas:10713:7: FText[I], Aow.Units.Engine.Engine.GetLineID(call__332)
- D:\git\aow\src\delphi3\Engine.pas:10718:11: FValues.Count
- D:\git\aow\src\delphi3\Engine.pas:10721:11: FText[I], Aow.Units.Engine.Engine.GetLineValue(call__335)
- D:\git\aow\src\delphi3\Engine.pas:10724:11: FText[I]
- D:\git\aow\src\delphi3\Engine.pas:10729:9: FText[I], Aow.Units.Engine.Engine.GetLineValue(call__338)
- D:\git\aow\src\delphi3\Engine.pas:10735:7: FValues.Count
- D:\git\aow\src\delphi3\Engine.pas:10615:22: FStream.Size, FStream.Position
- D:\git\aow\src\delphi3\Engine.pas:10616:3: newAow.Rtl.TStringList()
- D:\git\aow\src\delphi3\Engine.pas:10617:3: Aow.Units.Engine.TPropertyTable.Create()
- D:\git\aow\src\delphi3\Engine.pas:10618:3: newAow.Rtl.TStringList()
- D:\git\aow\src\delphi3\Engine.pas:10619:3: newAow.Rtl.TStringList()
- D:\git\aow\src\delphi3\Engine.pas:10759:3: newAow.Rtl.TStringList()
- D:\git\aow\src\delphi3\Engine.pas:10774:7: Obj.ClassType.ClassID()
- D:\git\aow\src\delphi3\Engine.pas:10788:3: FPropertyTable.FindOffset(ID)
- D:\git\aow\src\delphi3\Engine.pas:10789:24: FValues[Offset]
- D:\git\aow\src\delphi3\Engine.pas:10796:3: FPropertyTable.FindOffset(ID)
- D:\git\aow\src\delphi3\Engine.pas:10797:24: FValues[Offset]
- D:\git\aow\src\delphi3\Engine.pas:10816:5: Node.Children[I]
- D:\git\aow\src\delphi3\Engine.pas:10817:5: (C!=null)&&(C.Owner==Node), bool__355
- D:\git\aow\src\delphi3\Engine.pas:10359:3: (FPropertyTable.Count!=0)
- D:\git\aow\src\delphi3\Engine.pas:10339:3: Aow.Units.Engine.TEBufferStream.Create()
- D:\git\aow\src\delphi3\Engine.pas:10340:3: Aow.Units.Engine.TPropertyTable.Create()
- D:\git\aow\src\delphi3\Engine.pas:10382:3: FPropertyTable.PropertyExist(ID)
- D:\git\aow\src\delphi3\Engine.pas:10387:5: Aow.Units.Engine.TEBufferStream.Create()
- D:\git\aow\src\delphi3\Engine.pas:10404:9: Obj.ClassType.ClassID()
- D:\git\aow\src\delphi3\Engine.pas:10421:3: FPropertyTable.PropertyExist(ID)
- D:\git\aow\src\delphi3\Engine.pas:10439:3: FPropertyTable.PropertyExist(ID)
- D:\git\aow\src\delphi3\Engine.pas:10441:5: Strings.Count
- D:\git\aow\src\delphi3\Engine.pas:10469:3: FPropertyTable.PropertyExist(ID)
- D:\git\aow\src\delphi3\Engine.pas:10475:7: Node.Children[I]
- D:\git\aow\src\delphi3\Engine.pas:10476:7: (C!=null)&&(C.Owner==Node), bool__372
- D:\git\aow\src\delphi3\Engine.pas:9971:3: Aow.Units.Engine.TPropertyTable.Create()
- D:\git\aow\src\delphi3\Engine.pas:9973:3: FStream.Position
- D:\git\aow\src\delphi3\Engine.pas:9995:3: FPropertyTable.FindOffset(ID)
- D:\git\aow\src\delphi3\Engine.pas:9998:5: FStream.Position
- D:\git\aow\src\delphi3\Engine.pas:10009:3: FPropertyTable.FindOffset(ID)
- D:\git\aow\src\delphi3\Engine.pas:10012:5: FStream.Position
- D:\git\aow\src\delphi3\Engine.pas:10023:3: FPropertyTable.FindOffset(ID)
- D:\git\aow\src\delphi3\Engine.pas:10026:5: FStream.Position
- D:\git\aow\src\delphi3\Engine.pas:10037:3: FPropertyTable.FindOffset(ID)
- D:\git\aow\src\delphi3\Engine.pas:10040:5: FStream.Position
- D:\git\aow\src\delphi3\Engine.pas:10051:3: FPropertyTable.FindOffset(ID)
- D:\git\aow\src\delphi3\Engine.pas:10054:5: FStream.Position
- D:\git\aow\src\delphi3\Engine.pas:10065:3: FPropertyTable.FindOffset(ID)
- D:\git\aow\src\delphi3\Engine.pas:10068:5: FStream.Position
- D:\git\aow\src\delphi3\Engine.pas:10079:3: FPropertyTable.FindOffset(ID)
- D:\git\aow\src\delphi3\Engine.pas:10082:5: FStream.Position
- D:\git\aow\src\delphi3\Engine.pas:10093:3: FPropertyTable.FindOffset(ID)
- D:\git\aow\src\delphi3\Engine.pas:10096:5: FStream.Position
- D:\git\aow\src\delphi3\Engine.pas:10107:3: FPropertyTable.FindOffset(ID)
- D:\git\aow\src\delphi3\Engine.pas:10110:5: FStream.Position
- D:\git\aow\src\delphi3\Engine.pas:10121:3: FPropertyTable.FindOffset(ID)
- D:\git\aow\src\delphi3\Engine.pas:10124:5: FStream.Position
- D:\git\aow\src\delphi3\Engine.pas:10135:3: FPropertyTable.FindOffset(ID)
- D:\git\aow\src\delphi3\Engine.pas:10138:5: FStream.Position
- D:\git\aow\src\delphi3\Engine.pas:10149:3: FPropertyTable.FindOffset(ID)
- D:\git\aow\src\delphi3\Engine.pas:10152:5: FStream.Position
- D:\git\aow\src\delphi3\Engine.pas:10161:3: FPropertyTable.FindOffset(ID)
- D:\git\aow\src\delphi3\Engine.pas:10164:5: FStream.Position
- D:\git\aow\src\delphi3\Engine.pas:10176:3: FPropertyTable.FindOffset(ID)
- D:\git\aow\src\delphi3\Engine.pas:10179:5: FStream.Position
- D:\git\aow\src\delphi3\Engine.pas:10188:7: FPropertyTable.FindOffset(ID)
- D:\git\aow\src\delphi3\Engine.pas:10213:3: FPropertyTable.FindOffset(ID)
- D:\git\aow\src\delphi3\Engine.pas:10216:5: FStream.Position
- D:\git\aow\src\delphi3\Engine.pas:10223:7: FStream.Position
- D:\git\aow\src\delphi3\Engine.pas:10225:9: FEngine.CreateEObject(AClassID,null)
- D:\git\aow\src\delphi3\Engine.pas:10251:3: FPropertyTable.FindOffset(ID)
- D:\git\aow\src\delphi3\Engine.pas:10256:5: FStream.Position
- D:\git\aow\src\delphi3\Engine.pas:10258:5: FStream.Position
- D:\git\aow\src\delphi3\Engine.pas:10282:3: FPropertyTable.FindOffset(ID)
- D:\git\aow\src\delphi3\Engine.pas:10286:5: FStream.Position
- D:\git\aow\src\delphi3\Engine.pas:10288:5: FStream.Position
- D:\git\aow\src\delphi3\Engine.pas:10308:3: FStream.Position
- D:\git\aow\src\delphi3\Engine.pas:10316:7: FStream.Position
- D:\git\aow\src\delphi3\Engine.pas:9570:3: Aow.Units.Engine.TEBufferStream.Create()
- D:\git\aow\src\delphi3\Engine.pas:9571:3: Aow.Units.Engine.TPropertyTable.Create()
- D:\git\aow\src\delphi3\Engine.pas:9599:3: FPropertyTable.PropertyExist(ID)
- D:\git\aow\src\delphi3\Engine.pas:9613:3: FPropertyTable.PropertyExist(ID)
- D:\git\aow\src\delphi3\Engine.pas:9627:3: FPropertyTable.PropertyExist(ID)
- D:\git\aow\src\delphi3\Engine.pas:9641:3: FPropertyTable.PropertyExist(ID)
- D:\git\aow\src\delphi3\Engine.pas:9655:3: FPropertyTable.PropertyExist(ID)
- D:\git\aow\src\delphi3\Engine.pas:9669:3: FPropertyTable.PropertyExist(ID)
- D:\git\aow\src\delphi3\Engine.pas:9683:3: FPropertyTable.PropertyExist(ID)
- D:\git\aow\src\delphi3\Engine.pas:9697:3: FPropertyTable.PropertyExist(ID)
- D:\git\aow\src\delphi3\Engine.pas:9711:3: FPropertyTable.PropertyExist(ID)
- D:\git\aow\src\delphi3\Engine.pas:9725:3: FPropertyTable.PropertyExist(ID)
- D:\git\aow\src\delphi3\Engine.pas:9727:5: Strings.Count
- D:\git\aow\src\delphi3\Engine.pas:9739:3: FPropertyTable.PropertyExist(ID)
- D:\git\aow\src\delphi3\Engine.pas:9741:5: Strings.Count
- D:\git\aow\src\delphi3\Engine.pas:9753:3: FPropertyTable.PropertyExist(ID)
- D:\git\aow\src\delphi3\Engine.pas:9767:3: FPropertyTable.PropertyExist(ID)
- D:\git\aow\src\delphi3\Engine.pas:9781:3: FPropertyTable.PropertyExist(ID)
- D:\git\aow\src\delphi3\Engine.pas:9795:3: FPropertyTable.PropertyExist(ID)
- D:\git\aow\src\delphi3\Engine.pas:9826:3: FPropertyTable.PropertyExist(ID)
- D:\git\aow\src\delphi3\Engine.pas:9834:7: Aow.Units.Engine.TEBufferStream.Create()
- D:\git\aow\src\delphi3\Engine.pas:9873:3: FPropertyTable.PropertyExist(ID)
- D:\git\aow\src\delphi3\Engine.pas:9877:5: Aow.Units.Engine.TEBufferStream.Create()
- D:\git\aow\src\delphi3\Engine.pas:9881:7: Node.Children[I]
- D:\git\aow\src\delphi3\Engine.pas:9882:7: (C!=null)&&(C.Owner==Node), bool__450
- D:\git\aow\src\delphi3\Engine.pas:9902:3: FPropertyTable.PropertyExist(ID)
- D:\git\aow\src\delphi3\Engine.pas:9906:5: Aow.Units.Engine.TEBufferStream.Create()
- D:\git\aow\src\delphi3\Engine.pas:9938:3: Aow.Units.Engine.TEBufferStream.Create()
- D:\git\aow\src\delphi3\Engine.pas:10494:3: Aow.Units.Engine.TPropertyTable.Create()
- D:\git\aow\src\delphi3\Engine.pas:10510:3: FPropertyTable.PropertyExist(ID)
- D:\git\aow\src\delphi3\Engine.pas:10547:3: FPropertyTable.PropertyExist(ID)
- D:\git\aow\src\delphi3\Engine.pas:10552:7: Node.Children[I]
- D:\git\aow\src\delphi3\Engine.pas:10553:7: (C!=null)&&(C.Owner==Node), bool__465
- D:\git\aow\src\delphi3\Engine.pas:10567:3: FPropertyTable.PropertyExist(ID)
- D:\git\aow\src\delphi3\Engine.pas:10828:3: Aow.Units.Engine.TPropertyTable.Create()
- D:\git\aow\src\delphi3\Engine.pas:10877:7: Obj.ClassType.ClassID()
- D:\git\aow\src\delphi3\Engine.pas:10894:5: Node.Children[I]
- D:\git\aow\src\delphi3\Engine.pas:10895:5: (C!=null)&&(C.Owner==Node), bool__476
- D:\git\aow\src\delphi3\Engine.pas:10937:3: FPropertyTable.FindOffset(ID)
- D:\git\aow\src\delphi3\Engine.pas:10940:5: FStream.Position
- D:\git\aow\src\delphi3\Engine.pas:10947:7: FStream.Position
- D:\git\aow\src\delphi3\Engine.pas:10964:3: FPropertyTable.FindOffset(ID)
- D:\git\aow\src\delphi3\Engine.pas:10969:5: FStream.Position
- D:\git\aow\src\delphi3\Engine.pas:10971:5: FStream.Position
- D:\git\aow\src\delphi3\Engine.pas:10977:9: Node.Children[Index]
- D:\git\aow\src\delphi3\Engine.pas:10984:7: Node.Count, (Start<call__489), bool__490
- D:\git\aow\src\delphi3\Engine.pas:10990:9: Node.Count
- D:\git\aow\src\delphi3\Engine.pas:10992:11: Node.Children[(I-FPropertyTable.First)+Start]
- D:\git\aow\src\delphi3\Engine.pas:11037:3: Strings.Text, L.Text
- D:\git\aow\src\delphi3\Engine.pas:11046:3: Strings.Text, L.Text
- D:\git\aow\src\delphi3\Engine.pas:2009:3: Strings.Count
- D:\git\aow\src\delphi3\Engine.pas:2014:5: Strings.Count
- D:\git\aow\src\delphi3\Engine.pas:2031:3: Strings.Count
- D:\git\aow\src\delphi3\Engine.pas:2049:5: Strings.Count
- D:\git\aow\src\delphi3\Engine.pas:9233:5: Aow.Units.Engine.Engine.PlugIns[I], ((Aow.Units.Engine.TPlugIn)call__509).Name(), PlugIn.Name()
- D:\git\aow\src\delphi3\Engine.pas:9239:5: PlugIn.Name(), Aow.Rtl.Exception.Create((((AnsiString)"PlugIn ")+call__512)+((AnsiString)" already registered"))
- D:\git\aow\src\delphi3\Engine.pas:9244:3: Aow.Units.Engine.Engine.PlugIns.IndexOf(PlugIn)
- D:\git\aow\src\delphi3\Engine.pas:9341:38: Aow.Units.Engine.Engine.PlugIns[I]
- D:\git\aow\src\delphi3\Engine.pas:9348:38: Aow.Units.Engine.Engine.PlugIns[I]
- D:\git\aow\src\delphi3\Engine.pas:9354:5: Aow.Units.Engine.Engine.PlugIns[Index]
- D:\git\aow\src\delphi3\Engine.pas:9369:7: Aow.Units.Engine.Engine.PlugIns[I], ((Aow.Units.Engine.TPlugIn)call__520).Name()
- D:\git\aow\src\delphi3\Engine.pas:9369:47: Aow.Units.Engine.Engine.PlugIns[I]
- D:\git\aow\src\delphi3\Engine.pas:9394:9: Aow.Rtl.Exception.Create((((AnsiString)"Unable to load PlugIn: ")+Dir)+SR.Name)
- D:\git\aow\src\delphi3\Engine.pas:9408:37: Dirs[I]
- D:\git\aow\src\delphi3\Engine.pas:9432:7: Aow.Units.Engine.Engine.Packages[I]
- D:\git\aow\src\delphi3\Engine.pas:9427:7: Aow.Units.Engine.Engine.Packages[I]
- D:\git\aow\src\delphi3\Engine.pas:3109:7: M.Size, Aow.Units.Engine.Engine.GetCRC32(Ptr<byte>.To(M.Memory,0),call__530)
- D:\git\aow\src\delphi3\Engine.pas:9461:11: Aow.Units.Engine.Engine.Engines[Index]
- D:\git\aow\src\delphi3\Engine.pas:9479:7: Aow.Units.Engine.Engine.Engines[I], ((Aow.Units.Engine.TEngine)call__533).ClassType.ClassID()
- D:\git\aow\src\delphi3\Engine.pas:9482:19: Aow.Units.Engine.Engine.Engines[I-1]
- D:\git\aow\src\delphi3\Engine.pas:1987:5: Ctx.Strings[M]
- D:\git\aow\src\delphi3\Engine.pas:1988:5: Ctx.Strings.Objects[M]
- D:\git\aow\src\delphi3\Engine.pas:1990:7: Ctx.Strings.Objects[I], Ctx.Strings[I], Ctx.Compare(call__539,P,call__538,PO)
- D:\git\aow\src\delphi3\Engine.pas:1991:7: Ctx.Strings.Objects[J], Ctx.Strings[J], Ctx.Compare(call__542,P,call__541,PO)
- D:\git\aow\src\delphi3\Engine.pas:2225:5: Strings.Objects[I]
- D:\git\aow\src\delphi3\Engine.pas:3449:7: SCompare(SortList[I],P)
- D:\git\aow\src\delphi3\Engine.pas:3450:7: SCompare(SortList[J],P)
- D:\git\aow\src\delphi3\Engine.pas:3784:7: SCompare(SortList[I],P)
- D:\git\aow\src\delphi3\Engine.pas:3785:7: SCompare(SortList[J],P)
- D:\git\aow\src\delphi3\Engine.pas:4131:7: SCompare(SortList[I],P)
- D:\git\aow\src\delphi3\Engine.pas:4132:7: SCompare(SortList[J],P)
- D:\git\aow\src\delphi3\Engine.pas:4442:7: SCompare(SortList[I],P)
- D:\git\aow\src\delphi3\Engine.pas:4443:7: SCompare(SortList[J],P)
- D:\git\aow\src\delphi3\Engine.pas:5265:7: SCompare(SortList[I],P)
- D:\git\aow\src\delphi3\Engine.pas:5266:7: SCompare(SortList[J],P)
- D:\git\aow\src\delphi3\Engine.pas:5675:7: SCompare(SortList[I],P)
- D:\git\aow\src\delphi3\Engine.pas:5676:7: SCompare(SortList[J],P)
- D:\git\aow\src\delphi3\Engine.pas:11124:3: newAow.Rtl.TList()
- D:\git\aow\src\delphi3\Engine.pas:11125:3: newAow.Rtl.TList()
- D:\git\aow\src\delphi3\Engine.pas:11126:3: newAow.Rtl.TStringList()
- D:\git\aow\src\delphi3\Engine.pas:11127:3: Aow.Rtl.Rtl.RegisterClipboardFormat((AnsiString)"ECLIPBOARD FORMAT")
- D:\git\aow\src\delphi3\FMemStr.pas:142:9: Ptr<byte>.Alloc(NewCapacity)
- D:\git\aow\src\delphi3\FMemStr.pas:144:9: FMemory.Realloc(NewCapacity)
- D:\git\aow\src\delphi3\FMemStr.pas:108:3: Stream.Size
- D:\git\aow\src\delphi3\PlugInM.pas:71:5: Aow.Rtl.Rtl.FindFirst((Dir+((AnsiString)"*."))+FPlugInExtension,63-16,refSR)
- D:\git\aow\src\delphi3\PlugInM.pas:87:37: Dirs[I]
- D:\git\aow\src\delphi3\PlugInM.pas:49:3: newAow.Rtl.TStringList()
- D:\git\aow\src\delphi3\PlugInM.pas:101:3: Aow.Rtl.Rtl.ExpandFileName(Directory)
- D:\git\aow\src\delphi3\PlugInM.pas:114:9: w__6[I]
- D:\git\aow\src\delphi3\PlugInM.pas:110:41: w__6[I]
- D:\git\aow\src\delphi3\PlugInM.pas:134:9: w__12[I]
- D:\git\aow\src\delphi3\PlugInM.pas:128:9: w__12.Objects[I]
- D:\git\aow\src\delphi3\PlugInM.pas:130:11: w__12.Objects[I]

### Assembler census

- Engine.pas:3200: FillInteger, fill, manifest override
- Engine.pas:3210: FillWord, fill, manifest override
- Engine.pas:3220: FillLargeInteger, fill, manifest override
- Engine.pas:3240: FillQuadInteger, fill, manifest override
- Engine.pas:3312: ReturnAddr, list-error, raise without a return address
- Engine.pas:3647: ReturnAddr, list-error, raise without a return address
- Engine.pas:3994: ReturnAddr, list-error, raise without a return address
- Engine.pas:4305: ReturnAddr, list-error, raise without a return address
- Engine.pas:4615: ReturnAddr, list-error, raise without a return address
- Engine.pas:4646: TBitList.Get, bitop, manifest override
- Engine.pas:4660: TBitList.Put, bitop, manifest override
- Engine.pas:4898: ReturnAddr, list-error, raise without a return address
- Engine.pas:5109: ReturnAddr, list-error, raise without a return address
- Engine.pas:5507: ReturnAddr, list-error, raise without a return address

The current Engine source has eight ReturnAddr helpers. The brief names nine. All eight catalogue occurrences are checked and mapped.

Metaclass closure: 30 classes, 25 own Meta types.

- Aow.Units.EList.TListObject: Aow.Units.EList.TListObject.Meta
- Aow.Units.Engine.TBitList: Aow.Units.Engine.TBitList.Meta
- Aow.Units.Engine.TByteList: Aow.Units.Engine.TByteList.Meta
- Aow.Units.Engine.TDoubleItemList: Aow.Units.Engine.TDoubleItemList.Meta
- Aow.Units.Engine.TEChangeNotifyEventList: Aow.Units.Engine.TEChangeNotifyNode.Meta
- Aow.Units.Engine.TEChangeNotifyNode: Aow.Units.Engine.TEChangeNotifyNode.Meta
- Aow.Units.Engine.TEControlStyleInverseSortNode: Aow.Units.Engine.TEControlStyleInverseSortNode.Meta
- Aow.Units.Engine.TEControlStyleNode: Aow.Units.Engine.TEControlStyleNode.Meta
- Aow.Units.Engine.TEControlStyleObject: Aow.Units.Engine.TEControlStyleObject.Meta
- Aow.Units.Engine.TEControlStyleSortNode: Aow.Units.Engine.TEControlStyleSortNode.Meta
- Aow.Units.Engine.TECustomNode: Aow.Units.Engine.TEObject.Meta
- Aow.Units.Engine.TECustomOwner: Aow.Units.Engine.TECustomOwner.Meta
- Aow.Units.Engine.TEInverseSortNode: Aow.Units.Engine.TEInverseSortNode.Meta
- Aow.Units.Engine.TENode: Aow.Units.Engine.TENode.Meta
- Aow.Units.Engine.TEObject: Aow.Units.Engine.TEObject.Meta
- Aow.Units.Engine.TEObjectEditor: Aow.Units.Engine.TEObjectEditor.Meta
- Aow.Units.Engine.TEReferenceNode: Aow.Units.Engine.TENode.Meta
- Aow.Units.Engine.TEResource: Aow.Units.Engine.TEResource.Meta
- Aow.Units.Engine.TEResourceEditSettings: Aow.Units.Engine.TEResourceEditSettings.Meta
- Aow.Units.Engine.TEResourceList: Aow.Units.Engine.TEResourceList.Meta
- Aow.Units.Engine.TESortNode: Aow.Units.Engine.TESortNode.Meta
- Aow.Units.Engine.TETimerNode: Aow.Units.Engine.TETimerNode.Meta
- Aow.Units.Engine.TEUser: Aow.Units.Engine.TEObject.Meta
- Aow.Units.Engine.TEngine: Aow.Units.Engine.TEngine.Meta
- Aow.Units.Engine.TEventList: Aow.Units.Engine.TEventList.Meta
- Aow.Units.Engine.TIntegerList: Aow.Units.Engine.TIntegerList.Meta
- Aow.Units.Engine.TPlugInControl: Aow.Units.Engine.TEObject.Meta
- Aow.Units.Engine.TQuadItemList: Aow.Units.Engine.TQuadItemList.Meta
- Aow.Units.Engine.TShortIntList: Aow.Units.Engine.TShortIntList.Meta
- Aow.Units.Engine.TWordList: Aow.Units.Engine.TWordList.Meta

## Statically true class tests

0 folded sites in emitted bodies. These tests return true even for nil operands.


## Build warnings

Disabled warning numbers: CS0168, CS0219, CS0162, CS0465, CA2255. The project comments give the reason for each number.


## Shim additions and proposed specification changes

- Named indexed properties return a generated sealed accessor class. A struct result triggers CS1612 for assignments through the property. The accessor keeps the adapter out of method bodies.
- Inline-array constants use a generated From(ReadOnlySpan<T>) factory. C# 15 rejects a collection expression that targets an inline-array struct directly.
- Engine's character set literals include ordinal values above 63, including the hexadecimal letter tests. They use the shim's explicit 0..255 bounds. Declared enumeration sets retain their smaller bounds.
- TFastMemoryStream overrides the shim's span Read and Write signatures, public SetSize and int Seek origin. The original Count is the span length. Scalar fast paths read and write the physical byte span.
- Integer values stored as TObject in TStrings.Objects use canonical integer boxes. ObjectToInt recovers those values and uses InstanceId for real objects. Applying N7 to a numeric box as an object identity loses stored editor ids and section offsets.
- TEventList.Pack compares the target and code halves separately with AND. MethodTarget and MethodCode retain that condition. Full delegate inequality changes which items are deleted.
- Shim additions required by this output: TLargeInteger, TRect, opaque TCanvas, TLibraryImage and TPopupMenu boundary types, TBltMode, TStreamProc, RwData, THeapStatus, GetHeapStatus, TCompressionLevel, TCompressionStream, TDecompressionStream, TApplication, Application.ExeName, MessageHandler, ShowMessage, RegisterClass, RegisterComponents, RegisterClipboardFormat, ValueBytes, StringBytes, FillBytes, ArrayEquals, MethodCode, MethodTarget, IntToObject, ObjectToInt, RwSet, PointerSequenceEqual and ReadString.
- GetHeapStatus.TotalAllocated uses GC.GetTotalMemory(false). The source uses it only in VerifyEObjectClass's swallowed leak diagnostic. Managed heap retention changes that diagnostic, while the constructor, destructor and propagated construction failures remain intact.
- The compression adapter uses the BCL zlib stream and leaves the caller's stream open. Compression bytes depend on that backend. Decompressed bytes and the CFS header retain the source format. The image types expose signatures only and the drawing body remains an explicit TODO.
- The source places clipboard operations in TECopyComponent and unit routines, and popup hooks in TEObjectEditor.Meta. The override manifest records those actual owners.
- File headers use the emitter version date to keep regeneration deterministic across days.
- The asm-stubs runner compares Engine overrides with Delphi recordings and lists routines from other units as pending. See tests/probes/README.md for the current differences.

## Emitted method samples

### TIntegerList.Add

Engine.pas:3287

| Delphi | C# |
|---|---|
| <pre>function TIntegerList.Add(Item: Integer): Integer;<br>begin<br>  Result := FCount;<br>  if Result = FCapacity then Grow;<br>  FList^[Result] := Item;<br>  Inc(FCount);<br>end;</pre> | <pre>// Engine.pas:3287<br>public int Add(int Item)<br>{<br>    int Result = default;<br>    Result = FCount;<br>    if (Result == FCapacity)<br>    {<br>        Grow();<br>    }<br><br>    FList[Result] = Item;<br>    FCount += 1;<br>    return Result;<br>}</pre> |

### TEObject.GetControlStyle

Engine.pas:2563

| Delphi | C# |
|---|---|
| <pre>function TEObject.GetControlStyle: TEControlStyles;<br>begin<br>  Result := [csStored];<br>end;</pre> | <pre>// Engine.pas:2563<br>public virtual TSet&lt;Aow.Units.Engine.TEControlStyleElement&gt; GetControlStyle()<br>{<br>    TSet&lt;Aow.Units.Engine.TEControlStyleElement&gt; Result = default;<br>    Result = TSet&lt;Aow.Units.Engine.TEControlStyleElement&gt;.Of(Aow.Units.Engine.Engine.csStored);<br>    return Result;<br>}</pre> |

### TEObject.ClassID

Engine.pas:2461

| Delphi | C# |
|---|---|
| <pre>class function TEObject.ClassID: Integer;<br>begin<br>  Result := 80;<br>end;</pre> | <pre>// Engine.pas:2461<br>public virtual int ClassID()<br>{<br>    int Result = default;<br>    Result = 80;<br>    return Result;<br>}</pre> |

