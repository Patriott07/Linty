# SmartLinty Project Architectural Guidelines

## Overview
SmartLinty is a Unity Editor extension for code quality and project structure analysis. This document defines the architectural standards all developers must follow when adding or modifying features.

## Core Principles

### 1. Modular / Factory Pattern
- **Never create monolithic files**. Each feature must be modular and extensible.
- Use **interfaces** for extensibility (Open/Closed Principle).
- New modules can be added **without modifying existing code**.
- Register modules via composition (e.g., `List<IDiagnosticRule>`).

**Example Structure:**
```
Assets/SmartLinty/Editor/Diagnostic/
├── IDiagnosticRule.cs          (interface - DO NOT MODIGade signature)
├── IFeatureXRule.cs           (new interface if needed)
└── FeatureXDiagnostic.cs      (implements IDiagnosticRule)
```

### 2. UI Window vs Product System Separation
- **UI Window** (`EditorWindow`): Handles ONLY presentation, layout, and user input.
- **Product System**: Logic, data evaluation, and "fix" functionality live in separate model/service classes.
- UI windows **use** product systems; they do NOT contain business logic.

**Example Structure:**
```
Assets/SmartLinty/Editor/UI/
├── FeatureXWindow.cs         (EditorWindow - GUI ONLY)
└── FeatureXModel.cs          (data/logic - Product System)
```

### 3. Single Responsibility Per File
- **Max 1 public class per file**.
- Helper/extension methods in separate utility files.
- Interfaces in their own files.

### 4. Namespace & Organization
- Use consistent namespace: `Custom.FeatureName`
- Interface-first design (similar to `Custom.Interface.IDiagnosticRule`).
- Folders match namespace structure.

## Existing Pattern Examples

### Factory Pattern (Existing)
```csharp
// Assets/SmartLinty/Editor/UI/MenuLinty.cs line 15-21
private List<IDiagnosticRule> diagnosticRules = new List<IDiagnosticRule>()
{
    // new TransformDiagnostic(),
    // new Rigidbody2DDiagnostic(),
    // new BoxCollider2DDiagnostic(),
    // new UIEventSystemDiagnostic(),
};
```

### Modular UI Windows (Existing)
- `WelcomeLintyPopup.cs` - Welcome window
- `ProjectSetupLintyPopup.cs` - Project configuration
- `MenuLinty.cs` - Main assistant menu
- `FolderStructureLintyPopup.cs` - Folder structure generator

Each is a separate `EditorWindow` with single responsibility.

## Coding Standards

### Interface Definition
```csharp
namespace Custom.Interface
{
    public interface IDiagnosticRule
    {
        bool Evaluate(GameObject targetObject);
        void DrawDiagnosticUI(GameObject targetObject);
    }
}
```

### File Template (New Feature)
```
<File Header with namespace>
using UnityEngine;
using UnityEditor; // only if EditorWindow

namespace Custom.FeatureName
{
    // Interface if needed: IFeatureNameRule : IDiagnosticRule
    // Implementation: FeatureNameDiagnostic : IFeatureNameRule / IDiagnosticRule
}
```

### GUI Styling Conventions
- Dark background: `new Color(0.18f, 0.18f, 0.18f)`
- Lime accent color: `GenerateColorByHexa255(197, 104, 47)`
- Consistent button styles with custom textures

## Adding New Diagnostic Rule

1. **Create interface** (if new behavior needed) in `Assets/SmartLinty/Editor/Diagnostic/`
2. **Create implementation** implementing `IDiagnosticRule`
3. **Register in MenuLinty** via `diagnosticRules.Add(new YourRule())`
4. **No code changes** to existing rules required (extensible)

## Adding New UI Window

1. **Create Model class** with logic/data (`Assets/SmartLinty/Editor/`)
2. **Create Window** (`EditorWindow`) that uses the Model for GUI
3. **Register menu item** with `[MenuItem("Tools/Linty/...")]`
4. **Wire up in Tab Navigation** if integrating into MenuLinty

## Existing Files Status
- `IDiagnosticRule.cs` - **DO NOT MODIFY** (interface contract)
- `MenuLinty.cs` - **CAN EXTEND** (add rules to list, add tab cases)
- UI Windows - **CAN EXTEND** (create new windows, add to tab navigation)
- All existing functionality **MUST remain working**

## Migration Checklist (Before Committing)
- [ ] New code follows namespace conventions (`Custom.FeatureName`)
- [ ] No monolithic files created (max 1 public class/file)
- [ ] UI logic separated from business logic
- [ ] Interfaces used for extensibility
- [ ] All existing tests/scenarios still pass
- [ ] GUI styling matches existing conventions