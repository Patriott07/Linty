using System.Collections.Generic;
using UnityEngine;

public interface ISpriteRendererGuideable { GuideItem GetData(SpriteRenderer spriteRenderer); }

public class GuideDatabaseSpriteRenderer : ISpriteRendererGuideable
{
    public virtual GuideItem GetData(SpriteRenderer spriteRenderer) => default;
    public static readonly Dictionary<string, GuideItem> SpriteRendererGuides = new Dictionary<string, GuideItem>
    {
        ["SpriteRenderer_Sprite"] = GuideDatabaseHelpers.Item("Sprite", "Menentukan gambar Sprite yang ditampilkan.", "Sprite Renderer tidak memiliki Sprite.", "Sprite ditampilkan."),
        ["SpriteRenderer_Color"] = GuideDatabaseHelpers.Item("Color", "Mengatur warna dan transparansi Sprite.", "Sprite transparan.", "Sprite memiliki warna normal.", "Sprite memiliki warna penuh."),
        ["SpriteRenderer_Flip"] = GuideDatabaseHelpers.Item("Flip X dan Flip Y", "Membalik tampilan Sprite secara horizontal atau vertikal.", "Sprite tidak dibalik.", "Sprite dibalik."),
        ["SpriteRenderer_Material"] = GuideDatabaseHelpers.Item("Material", "Menentukan material dan shader Sprite.", "Sprite menggunakan material default.", "Sprite menggunakan material khusus."),
        ["SpriteRenderer_DrawMode"] = GuideDatabaseHelpers.Item("Draw Mode", "Menentukan cara Sprite dirender saat ukurannya diubah.", "Sprite menggunakan Simple.", "Sprite menggunakan Sliced atau Tiled."),
        ["SpriteRenderer_Size"] = GuideDatabaseHelpers.Item("Size", "Menentukan ukuran area Sprite.", "Sprite menggunakan ukuran default.", "Sprite menggunakan ukuran khusus."),
        ["SpriteRenderer_Mask"] = GuideDatabaseHelpers.Item("Mask Interaction", "Menentukan hubungan Sprite dengan Sprite Mask.", "Sprite tidak dipengaruhi Mask.", "Sprite menggunakan Mask Interaction."),
        ["SpriteRenderer_SortingLayer"] = GuideDatabaseHelpers.Item("Sorting Layer", "Menentukan lapisan render Sprite.", "Sprite berada pada layer default.", "Sprite berada pada layer khusus."),
        ["SpriteRenderer_Order"] = GuideDatabaseHelpers.Item("Order in Layer", "Mengatur urutan Sprite dalam Sorting Layer.", "Sprite memiliki urutan rendah.", "Sprite memiliki urutan tinggi."),
        ["SpriteRenderer_SortPoint"] = GuideDatabaseHelpers.Item("Sprite Sort Point", "Menentukan titik acuan pengurutan Sprite.", "Sprite menggunakan Center.", "Sprite menggunakan Pivot.")
    };
}

public sealed class SpriteRendererSpriteGuide : GuideDatabaseSpriteRenderer { private readonly string key = "SpriteRenderer_Sprite"; public override GuideItem GetData(SpriteRenderer c) => GuideDatabaseHelpers.State(SpriteRendererGuides[key], c != null && c.sprite != null ? 1 : 0); }
public sealed class SpriteRendererColorGuide : GuideDatabaseSpriteRenderer { private readonly string key = "SpriteRenderer_Color"; public override GuideItem GetData(SpriteRenderer c) => GuideDatabaseHelpers.State(SpriteRendererGuides[key], c == null ? 0 : GuideDatabaseHelpers.Range(c.color.a, 0f, 1f)); }
public sealed class SpriteRendererFlipGuide : GuideDatabaseSpriteRenderer { private readonly string key = "SpriteRenderer_Flip"; public override GuideItem GetData(SpriteRenderer c) => GuideDatabaseHelpers.State(SpriteRendererGuides[key], c != null && (c.flipX || c.flipY) ? 1 : 0); }
public sealed class SpriteRendererMaterialGuide : GuideDatabaseSpriteRenderer { private readonly string key = "SpriteRenderer_Material"; public override GuideItem GetData(SpriteRenderer c) => GuideDatabaseHelpers.State(SpriteRendererGuides[key], c != null && c.sharedMaterial != null ? 1 : 0); }
public sealed class SpriteRendererDrawModeGuide : GuideDatabaseSpriteRenderer { private readonly string key = "SpriteRenderer_DrawMode"; public override GuideItem GetData(SpriteRenderer c) => GuideDatabaseHelpers.State(SpriteRendererGuides[key], c != null && c.drawMode != SpriteDrawMode.Simple ? 1 : 0); }
public sealed class SpriteRendererSizeGuide : GuideDatabaseSpriteRenderer { private readonly string key = "SpriteRenderer_Size"; public override GuideItem GetData(SpriteRenderer c) => GuideDatabaseHelpers.State(SpriteRendererGuides[key], c != null && c.drawMode != SpriteDrawMode.Simple && c.size != Vector2.zero ? 1 : 0); }
public sealed class SpriteRendererMaskGuide : GuideDatabaseSpriteRenderer { private readonly string key = "SpriteRenderer_Mask"; public override GuideItem GetData(SpriteRenderer c) => GuideDatabaseHelpers.State(SpriteRendererGuides[key], c != null && c.maskInteraction != SpriteMaskInteraction.None ? 1 : 0); }
public sealed class SpriteRendererSortingLayerGuide : GuideDatabaseSpriteRenderer { private readonly string key = "SpriteRenderer_SortingLayer"; public override GuideItem GetData(SpriteRenderer c) => GuideDatabaseHelpers.State(SpriteRendererGuides[key], c != null && c.sortingLayerID != 0 ? 1 : 0); }
public sealed class SpriteRendererOrderGuide : GuideDatabaseSpriteRenderer { private readonly string key = "SpriteRenderer_Order"; public override GuideItem GetData(SpriteRenderer c) => GuideDatabaseHelpers.State(SpriteRendererGuides[key], c != null && c.sortingOrder > 0 ? 1 : 0); }
public sealed class SpriteRendererSortPointGuide : GuideDatabaseSpriteRenderer { private readonly string key = "SpriteRenderer_SortPoint"; public override GuideItem GetData(SpriteRenderer c) => GuideDatabaseHelpers.State(SpriteRendererGuides[key], c != null && c.spriteSortPoint == SpriteSortPoint.Pivot ? 1 : 0); }
