using UnityEngine;

public interface ICanvasGroupGuideable { GuideItem GetData(CanvasGroup component); }

public sealed class CanvasGroupAlphaGuide : ICanvasGroupGuideable
{
    public GuideItem GetData(CanvasGroup component) => GuideDatabaseHelpers.State(GuideDatabaseHelpers.Item("Alpha", "Mengatur tingkat transparansi Canvas Group.", "Canvas Group transparan.", "Canvas Group semi-transparan.", "Canvas Group terlihat penuh."), component == null ? 0 : GuideDatabaseHelpers.Range(component.alpha, 0f, 1f));
}

public sealed class CanvasGroupInteractableGuide : ICanvasGroupGuideable
{
    public GuideItem GetData(CanvasGroup component) => GuideDatabaseHelpers.State(GuideDatabaseHelpers.Item("Interactable", "Menentukan apakah elemen UI dapat menerima interaksi.", "Elemen UI tidak dapat berinteraksi.", "Elemen UI dapat berinteraksi."), component != null && component.interactable ? 1 : 0);
}

public sealed class CanvasGroupBlocksRaycastsGuide : ICanvasGroupGuideable
{
    public GuideItem GetData(CanvasGroup component) => GuideDatabaseHelpers.State(GuideDatabaseHelpers.Item("Blocks Raycasts", "Menentukan apakah Canvas Group menghalangi raycast UI.", "Raycast melewati Canvas Group.", "Canvas Group menghalangi raycast."), component != null && component.blocksRaycasts ? 1 : 0);
}

public sealed class CanvasGroupIgnoreParentGuide : ICanvasGroupGuideable
{
    public GuideItem GetData(CanvasGroup component) => GuideDatabaseHelpers.State(GuideDatabaseHelpers.Item("Ignore Parent Groups", "Menentukan apakah pengaturan Canvas Group parent diabaikan.", "Pengaturan parent diteruskan.", "Pengaturan parent diabaikan."), component != null && component.ignoreParentGroups ? 1 : 0);
}
