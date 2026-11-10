# Metaclass retirement (milestone 9.4)

Status 2026-10-30. Complete through CF-3. Briefs 9.4-1 to 9.4-6, CF-2 and CF-3 retire the
product shim. `src/Game/ClassRegistry.cs` registers every descriptor through the single
`Catalog<TKey, TValue>` type. `EClass.ClassName` is a `string`. The class-id snapshot remains
unchanged. The private transpiler still emits `Meta` in its reference tree, which diverges from
the product source. The driver runs the full gate separately from this brief's build and tests.
The construction form and roadmap in the private repository govern the final registration form.

Original specification: Fable, 2026-10-29. Base: `main` at `bbdc15b` (9.3b-8 merged). Product paths are under
`D:/git/open-aow`, other paths under `D:/git/open-aow-private`. The milestone row reads: the engine's
class registry carries creation by class id, generics with static abstract interface members carry
the remaining class-side dispatch. Done measure: O3 holds and the shim's `Meta` type is deleted.

## 1. Where main stands

`grep -rn "\.Meta\b\|Meta\.Of<\|ClassType\b\|NewInstance()" src` finds 1,787 lines outside the
generated `src/Units/MetaRegistry.cs`, which adds 954 registrations (43 `EObjectCreation.Register`,
13 `LibraryImageCreation.Register`, 898 raw `MetaRegistry.Register`). By category:

| Category | Count on main | Where |
|---|---|---|
| Declarations that go with the type | 542 nested `Meta` types, 538 `Of<T>()`, 29 typed `ClassType` accessors, 472 `(TFoo)NewInstance()` inside `Meta.Create` factories | every unit |
| Engine class registration | 136 `RegisterEClasses` and `UnregisterEClasses` lines holding 575 of the 659 `Meta.Of<T>()` sites | unit initialization sections |
| Creation by class id | `Engine.CreateEObject` (Delphi `TEngine`), `GetEClass`, `RegisterEClasses`, `UnregisterEClasses`, `FEClasses` (`EObjectClassList` (Delphi `TEObjectClassList`)), `FClassTable`, `GetDescendantClasses`, `VerifyEObjectClass`, seven `rwCreateEObject` overrides, `HSMap.AddLevel` (Delphi `THSMap`) and `New` (`LevelClass.Create(this)`) | `Engine.cs`, `HSEngine.cs` |
| Creation by `Type` or class reference | `EObject.Copy` (Delphi `TEObject`) (`ClassType.Create(null)`), `MapLevel.MapFieldClass()` and the per-field loop, `GroupControlClass().Create`, `AbilityDataClass().Create(null)`, the AI group and player action class lists, the image class lists (`ImageClass.Create`), the sound system list and `SampleClass().Create(data)`, `SampleDataClass().Create` and `CreateEx`, `GrabInfo` (Delphi `TGrabInfo`) | `Engine.cs`, `HSEngine.MapFields.cs`, `AoWE.cs`, `ImageLib.cs`, `GFXE.cs`, `Sound.cs` |
| Class-side virtual methods | 25 introductions. Overrides: `ClassID` 477, `ResourceEditName` 93, `GroupControlClass` 17, `Behavior` 17, `Name` 16, `Priority` 13, `AbilityDataClass` 10, `ID` 4, `AnimationControlMode` 2, one each of `EditorClassID`, `SupportedPhases`, `ControlMode`, `GetExtension`, `GetDescription`, `GetPixelFormatID`, `CreateSurface`, `LoadToSurfaceDesc`. Instance-side calls through `ClassType`: `ClassID` 99, `Error` 80, `Behavior` 19, `AnimationControlMode` 9, `Name` 3, `ID` 3, `ClassVersion` 2, `ResourceEditName` 2, `AbilityDataClass` 2, one each of `SupportedPhases`, `GroupControlClass`, `DynamicIsometricHexagonClass`, `InstanceSize`. Calls through a class reference without an instance: `ClassID` (registration, `FindClass`, `FindLibraryImageClass`, the editor walk), `Priority` 2, `SupportedPhases` 1, `Behavior` 1, `EditorClassID` 1, `Edit` 1, `GetName` 4, `GetExtension` 3 | everywhere |
| `ClassParent`, `InheritsFrom`, `ClassName`, `Type` | the editor walk in `EObject.Editor` (`Engine.cs:4493`), `GetDescendantClasses`, `EResource.Meta.ResourceEditName` (Delphi `TEResource`) (`this.ClassName`), `Editor` loop test `C.Type != typeof(EObject)` | `Engine.cs` |
| `InstanceSize` | one read, `HSEngine.MapFields.cs:23` | |
| Raw `NewInstance` outside a factory | `IsoHex.DynamicOwnership.cs:40` (then `Attach`), `SoundSystem.Meta.Create` (Delphi `TSoundSystem`) | |
| Editor and resource class lists | `EObjectEditorList` (Delphi `TEObjectEditorList`) (a `EObjectClassList` of editor classes, `FindEditor(id)`), `ItemGrid.cs:169` (`Item.Editor().Edit(...)`), `ResourceEditName` on resources | `Engine.cs`, `ItemGrid.cs`, `HSEngine.cs:4281`, `ItemExpS.cs:180` |
| Ability and sound class-side dispatch | `Ability.Meta.AbilityDataClass` (Delphi `TAbility`) (10 overrides, one creation site), `SoundSystem.Meta.GetName` and `Create`, `Sample.Meta` (Delphi `TSample`) and `SampleData.Meta` (Delphi `TSampleData`) holding creation delegates, `SdlSoundSystem.cs` (8 lines), `Sound.NullSound.cs` (5 lines) | `AoWE.cs`, `Sound.cs`, `src/Platform` |
| RTL and tests | `TObject.cs`, `Persistent.cs` (`Component.Meta` (Delphi `TComponent`)), `Values.cs` (`TVarRec` from `Meta`), about 110 lines in nine `Rtl.Tests` files | `src/Rtl`, `src/Rtl.Tests` |

In flight. Briefs 9.3b-9 (AoWE rules), 9.3b-10 (units), 9.3b-11 (the rest) and 9.3b-12 (layout
session) convert constructors in worktrees under `D:/git/open-aow-wt`. Each moves creation from the
raw `() => new TFoo()` delegate into `EObjectCreation.Register(..., static owner => new TFoo(owner), size)`:
the 9.3b-9 worktree has 503 typed entries against 43 on main. They all rewrite `MetaRegistry.cs` and
`Engine.cs`, and 9.3b-10 and 9.3b-11 are based before 9.3b-6 merged (their registry lacks the
image entries). 9.4 starts after those four merge. The picture then: nearly every `EObject` class has
a completed-creation delegate, the retained initializer families (`AoWEngine` (Delphi `TAoWEngine`), `EventLog` (Delphi `TEventLog`),
`AoWCombatMap` (Delphi `TAoWCombatMap`), `AbstractUnit` (Delphi `TAbstractUnit`), `DebugWriteStream` (Delphi `TDebugWriteStream`)) keep a guarded static factory, and the raw
`NewInstance` route survives only for those and for the non-engine closures. 9.4 changes the
container the delegates live in, not the delegates.

## 2. Target shape

The rule. A class-side virtual whose every caller holds an instance becomes an instance virtual,
moved from `Meta` into the class body unchanged (Delphi dispatched it on the instance's class
anyway). A class-side value or behavior that a list or registry reads without an instance lives in
a descriptor, one object per class, built from static members that an interface with static
abstract members makes the compiler check. An instance reaches its descriptor through a cached
property. Creation goes through a typed delegate in the descriptor. Nothing is emitted per class
beyond the constants and statics the source declares.

### 2.1 The engine descriptor and class ids

```csharp
namespace Aow.Units.Engine;

public sealed class EClass
{
    public Type Type { get; }
    public int ClassID { get; }
    public int InstanceSize { get; }
    public EClass? Parent { get; }                 // the base class's descriptor, null at EObject
    public string ClassName => TObject.ClassNameOf(Type);
    public EObject Create(ECustomNode? owner) => create(owner);
    public bool InheritsFrom(Type type) => type.IsAssignableFrom(Type);
    public static EClass Of(Type type);            // one instance per type, throws when unregistered
    public static EClass Of<T>() where T : EObject => Of(typeof(T));
    public static EClass Register<T>(int classId, Func<ECustomNode?, T> create, int instanceSize) where T : EObject;
}
```

Every `EObject` descendant has one `EClass`, registered by the hand-maintained
`ClassRegistry.cs` in inheritance order from a module initializer, as today. The id constant moves
from the `Meta.ClassID()` body into the class:

```csharp
public partial class AoWHSMap : HSMap
{
    public new const int ClassID = 131089;         // Engine.pas:2461 spelling kept in the comment
```

The registry line reads it statically, `EClass.Register<TFoo>(TFoo.ClassID, static owner => new TFoo(owner), 16)`.
C# resolves `TFoo.ClassID` to the nearest declaring ancestor, which is what an inherited Delphi
class method returns, so an inherit-only class (`AIBudgetManager` (Delphi `TAIBudgetManager`), `EUser` (Delphi `TEUser`), `NewCombatRoundCA` (Delphi `TNewCombatRoundCA`))
gets the ancestor's id and its own creation delegate. A class with a retained guarded factory
registers `static owner => TFoo.Create(owner)`.

The instance side:

```csharp
public partial class EObject
{
    private EClass? eclass;
    public EClass EClass => eclass ??= EClass.Of(GetType());
```

`obj.ClassType.ClassID() == 131089` becomes `obj.EClass.ClassID == AoWHSMap.ClassID`.

`Engine` keeps the same members with `EClass` in place of `EObject.Meta`: `RegisterEClasses(ReadOnlySpan<EClass>)`,
`UnregisterEClasses`, `EClass GetEClass(int)`, `CreateEObject(id, owner) => GetEClass(id).Create(owner)`,
`FEClasses` a `List<EClass>` behind the existing `EClasses[]` accessor, `FClassTable` unchanged with
`EClass` values, `GetDescendantClasses(Type)` through `InheritsFrom`, `VerifyEObjectClass(EClass)`.
`EObject.Copy` is `Result = EClass.Create(null)`. The editor walk is
`for (var c = EClass; c.Type != typeof(EObject) && result == null; c = c.Parent!)`.

### 2.2 Instance virtuals

These move from `Meta` to the class body, signature unchanged, overrides included:
`AnimationControlMode` (`Animation` (Delphi `TAnimation`)), `ControlMode` (`HexagonSprite` (Delphi `THexagonSprite`)), `ResourceEditName`
(`EResource`, the base returns `ClassNameOf(GetType())`), `Name` (`Storm` (Delphi `TStorm`)), `ID`
(`ConstructionControl` (Delphi `TConstructionControl`)), `GroupBehavior` (`AIGroup` (Delphi `TAIGroup`)), `AbilityDataClass` and
`GroupControlClass` (returning `EClass`, the callers cast after `Create`), `ClassVersion` and `Name`
on `LibraryImage` (Delphi `TLibraryImage`). `Error` (one introduction, no override, 80 calls) becomes a static method on
its list class, `ClassType.Error(...)` becomes `Error(...)`. `DynamicIsometricHexagonClass` has no
override, so it goes and `IsometricHexagon` (Delphi `TIsometricHexagon`) constructs `new DynamicIsometricHexagon(this)`,
whose constructor runs the `EObject` body with a null owner, then `hexagon = owner` and
`Field.AddHS(this, 0)`, the order `Attach` has now.

### 2.3 Descriptor families with static abstract members

CF-2 uses explicit `Register<T>()`, `Of<T>()`,
`Of(Type)` and `All` over `Catalog<Type, TDescriptor>`. The catalog owns identity and rejects
duplicate keys. `ClassRegistry` registers parents before children, then the family descriptors.
It reserves subsequent blocks for kinds and format mappings when those features arrive.

Where a list holds classes and reads them without an instance, the class declares statics through
an interface. Explicit registration captures the class-side values and publishes one descriptor:

```csharp
public interface IAIPlayerActionClass { static abstract int Priority { get; } static abstract Set<AIPlayerProcessPhase> SupportedPhases { get; } }
public sealed class AIPlayerActionClass
{
    private static readonly Catalog<Type, AIPlayerActionClass> catalog = new("AI player action class");
    public EClass Class { get; }
    public int Priority => priority();
    private readonly Func<int> priority;
    public Set<AIPlayerProcessPhase> SupportedPhases { get; }
    public static AIPlayerActionClass Register<T>() where T : AIPlayerAction, IAIPlayerActionClass =>
        catalog.Add(typeof(T), new AIPlayerActionClass(EClass.Of<T>(), static () => T.Priority, T.SupportedPhases));
    public static AIPlayerActionClass Of<T>() where T : AIPlayerAction, IAIPlayerActionClass => catalog[typeof(T)];
    public static AIPlayerActionClass Of(Type type) => catalog[type];
    public static IReadOnlyList<AIPlayerActionClass> All => catalog.All;
}
```

An instance reads `ActionClass.SupportedPhases` through a cached property like `EClass`. The
families: `AIPlayerAction` (Delphi `TAIPlayerAction`) (`Priority`, `SupportedPhases`), `AIGroup` (`Behavior`, read by the
group manager before `Create(this)`), `EObjectEditor` (Delphi `TEObjectEditor`) (`EditorClassID`, `Edit(obj, engine, done)`,
the editor list becomes `List<EditorClass>` and `EObject.Editor()` returns `EditorClass`),
`ImageFile` (Delphi `TImageFile`) (`GetName`, `GetExtension`, `GetDescription`, `GetPixelFormatID`,
`LoadToSurfaceDesc`, `CreateSurface`, one implementor), `GrabInfo` (`GrabImages`, creation),
`LibraryImage` (`ClassID`, `ClassVersion`, `Name`, creation `Func<CustomImageLibrary, LibraryImage>`,
replacing `LibraryImageCreation`), `SoundSystem` (`Name`, `Func<SoundSystem> Create`, registered
through `RegisterSoundSystem(SoundSystemClass)`, the SDL and Galaxy aliases are two descriptors
over one type), `Sample` and `SampleData` (their `Meta` already holds the delegates, the
descriptor keeps the constructor signatures `SampleClass(Func<SampleData, Sample>)` and
`SampleDataClass(Func<string, SampleData>, MemoryCreation)`). `ListObject` (Delphi `TListObject`) and
`AIMoveExecuterControl` (Delphi `TAIMoveExecuterControl`) are class-of targets with no class method and one creation site each,
which becomes `new`. The four `BehaviorRegistrator.Meta.Of<T>().MapLoaded` handlers become static method
groups, equal by method for `Remove`.

### 2.4 The shim after 9.4

`TObject` keeps `ClassNameOf`, `Destroy`, `FreeInstance`. The construction counter later left, and address reads use `IdentityHash.Of`. `Meta`, `ClassType`,
`MetaRegistry`, `Component.Meta` and the `TVarRec` conversion from `Meta` are deleted. `rtl-shim.md`
1.1 and `transpiler.md` 4.4 get a note that the metaclass design was retired here and the
transpiler's reference diffs are read with that in mind.

## 3. What stays bit for bit

- Saved class ids: each constant moves verbatim. A snapshot `data/metaclass/classids.json` taken on
  main before 9.4-1 (type name, id, instance size, parent) is compared by a test against the full
  `EClass` table and against each engine's registered set after `RegisterEClasses`.
- Creation order: descriptors are not `TObject`s (`Meta` is not one
  either), registration allocates no instance, `eoVerifyClasses` still creates and frees one object
  per registered class in registration order, and every creation site keeps its position and
  argument (`Copy` and the save path pass `null`).
- Exact runtime type for inherit-only classes: `NewCombatRoundCA` and the 299 inherit-only classes
  keep their own delegate, never an inherited static.
- The binary read path `Engine.cs:12846`: `CreateEObject(AClassID, null)`, `FOwner` set directly,
  `MainReadWrite`, `FOwner` cleared, `SetOwner(AOwner)`, the catch freeing the object and restoring
  the table. Only the body of `CreateEObject` changes. The messages `Unregistered E-ClassID found (`
  and `EObject Class ID double defined!` stay.
- The dynamic map field class: `MapFieldClass()` stays a virtual on `MapLevel` (Delphi `TMapLevel`) returning `EClass`
  (`AoWMapLevel` (Delphi `TAoWMapLevel`) overrides it), `FFieldSize = type.InstanceSize` reads the same numbers from the
  registry, the per-field `type.Create(this)` loop keeps row order, and `FHNOffsets` follow.
- The isometric raw creation keeps its order (allocate, `EObject` body with null owner, attach,
  `AddHS`), now inside one constructor.
- The 11 `TEObjectClass` typed constants of `inc/Combat.pas` evaluate `EClass.Of<T>()` during type
  initialization, so the registry stays a module initializer.

## 4. Brief sequence

Each brief runs `python tools/check.py` (13 layers: product, transpiler, remaining-todos, forms,
localization, probes, persistence, oracle, frames, audio, pacing, language-frames, scenarios) with
unchanged counts, `dotnet build` with zero warnings and `dotnet test`. All start after 9.3b-9 to
9.3b-12 merge.

| Brief | Scope | Removes | Done measure beyond the gate |
|---|---|---|---|
| 9.4-1 ids and registry | `EClass`, `const ClassID` in every class with an override (477, a script under `tools/` in the private repository writes the edit, the diff is reviewed), `Meta.ClassID()` bodies become `=> TFoo.ClassID`, the generated file registers an `EClass` per `EObject` class beside `Meta`, `Engine` and `HSMap.AddLevel` and `New` on `EClass`, the 136 registration lines on `EClass.Of<T>()`, `Meta.Create` forwards to `EClass.Of(Type).Create` | the engine's use of `EObject.Meta`, `EObjectCreation` | the id snapshot test passes, `grep -n "EObject.Meta" src/Units/Engine*.cs` finds only the facade |
| 9.4-2 instance ids and creation by type | `EObject.EClass`, the 99 `ClassType.ClassID()` sites, `Copy`, `Editor` walk on `Parent`, `MapFieldClass`, `VerifyEObjectClass`, `GetDescendantClasses`, the 84 non-registration `Meta.Of<T>()` sites in the engine closure | `ClassType.ClassID`, `ClassParent`, `InstanceSize` on `Meta` | `grep -rn "ClassType.ClassID\|ClassParent\|\.InstanceSize" src/Units` finds only `EClass` reads, a test walks every `EClass` to `EObject` through `Parent` |
| 9.4-3 instance virtuals | section 2.2, with the isometric constructor | `ClassType.X()` for the listed members, `Error` | `grep -rn "ClassType\." src/Units` finds only `Behavior`, `SupportedPhases` and `Priority` sites, a map-load scenario with water and open sides exercises the dynamic hexagon |
| 9.4-4 descriptor families in the engine closure | `AIPlayerAction`, `AIGroup`, `EObjectEditor` through section 2.3 | the last class-side members on engine `Meta` types | a test registers and unregisters the group and action classes and checks list identity, the editor test drives `ItemGrid` through `EditorClass.Edit` |
| 9.4-5 image library and image files | `LibraryImage`, `ImageFile`, `GrabInfo` descriptors, `GraphicsConstructionTests` on the descriptor | `LibraryImageCreation`, the image `Meta` types' members | frame fixtures hold, a test loads a library through `FindLibraryImageClass` and reads `ClassVersion` and `Name` for every registered image class against the snapshot |
| 9.4-6 sound, component and the small targets | `SoundSystem`, `Sample`, `SampleData` descriptors with `SdlSoundSystem.cs` and `Sound.NullSound.cs`, `ListObject`, `AIMoveExecuterControl`, the `BehaviorRegistrator` (Delphi `TRegistrator`) handlers, `Component.Meta` with `ClassesTests` | the non-engine `Meta` members, `Component.Meta` | the audio layer holds, the 9.3b-8 missing and corrupt sample tests pass through the descriptors, `Find("SDL")` and `Find("Galaxy")` return distinct descriptors over one type, the owner-removal fixture asserts through `new Component(owner)` |
| 9.4-7 delete | every nested `Meta`, `Of<T>()`, typed `ClassType` accessor and `Meta.Create` factory, `MetaRegistry`, `TObject.Meta`, `TObject.ClassType`, `TVarRec` from `Meta`, the test fixtures (`GeneratedStyleSample`, `CoreTests` metaclass cases, the `Assert.Same(meta, value.ClassType)` assertions become `EClass` checks), `MetaRegistry.cs` renamed `ClassRegistry.cs` | the type | `grep -rn "\bMeta\b\|MetaRegistry\|ClassType\b\|NewInstance()" src` is empty, the snapshot test still passes, `docs/milestones.md` row 9.4 marked done with the report |

9.4-1 and 9.4-2 are sequential. 9.4-3 to 9.4-6 touch disjoint files after 9.4-2 and run in
parallel worktrees, except that 9.4-3 and 9.4-4 both edit `AoWE.cs` and go in that order. 9.4-7
waits for all.

## 5. Risks and the evidence each brief needs beyond the gate

- An id constant mistyped or resolved to the wrong ancestor. The oracle saves cover only the classes
  the scenarios write. Evidence: the snapshot test of 9.4-1, kept through 9.4-7, over the full table
  and every engine's registered set.
- `EClass.Parent` undefined. Today `ClassParent` is `MetaRegistry.Of(Type.BaseType)` and throws
  outside the closure, and the editor walk stops at `EObject`. Evidence: the walk test of 9.4-2
  over every registered type.
- Hot paths. `FHSList[i].ClassType.ClassID()` in `HSEngine.FieldSprites.cs:331` and the field scans
  at `HSEngine.cs:309` and `343` trade a virtual call for a cached field read. Evidence: the pacing
  layer and the frame fixtures, with the cache in place from 9.4-2.
- Descriptor identity. `UnregisterEClasses`, `UnRegisterClasses`, `UnregisterAIGroupClasses`, the
  sound system `Find` and `OnHSMLoaded.Remove` rely on one object per class. Evidence: the
  identity tests of 9.4-4 and 9.4-6, and one explicit catalog registration per family type.
- Retained initializer families. Their guarded static factory is the delegate, so 9.4 never calls
  `NewInstance` on them. Evidence: 9.4-1 lists each with its delegate, and the 9.3b failure tests
  (dynamic level, sample data, `ArmyHS` (Delphi `TArmyHS`)) keep passing.
- Platform assembly. `SdlSoundSystem.cs` subclasses `SoundSystem.Meta` privately for the two names.
  Evidence: the audio layer at volume 0 and the 9.3b-8 sample tests, no sound played.
- Type initialization order. The `TEObjectClass` typed constants evaluate during type initialization.
  Evidence: a test in 9.4-1 that touches one of those static fields before any engine exists.
- Transpiler reference diffs. After 9.4 the generated reference tree still has `Meta`. Evidence:
  the transpiler layer of the gate keeps passing, and the 9.4-7 report records the divergence for
  `transpiler.md` 4.4.
- In-flight merges. Starting 9.4-1 before 9.3b-9 to 9.3b-12 merge conflicts in `MetaRegistry.cs`
  and `Engine.cs` on every line. Evidence: the 9.4-1 brief names the merged commit it is based on.

## 6. CF-3 report, 2026-10-30

The product has 970 generic engine registrations in `ClassRegistry.cs`. The shim, its typed
accessors and factories, and the `TVarRec` class case are deleted. The class-id snapshot is
unchanged. The two construction fingerprint files change only for compiler-generated delegate
names after the registry rename. All 12 recordings reproduce their original hashes when only
those names are translated back.

The build reports zero warnings and zero errors. The unit run reports 3020 passed, zero failed
and zero skipped. The registry suite covers every engine class and descriptor family. Creation
with a null owner returns the exact registered type for 968 entries. `FlagControl` (Delphi `TFlagControl`) and
`DynamicIsometricHexagon` (Delphi `TDynamicIsometricHexagon`) retain their existing null-owner rejection. Null-owner teardown of
incomplete world objects and abstract nodes is recorded separately from creation identity.
The source deletion scan is empty. The private registration census reports 970 generic lines
and zero legacy forms. The driver runs O3 and the full gate separately.
