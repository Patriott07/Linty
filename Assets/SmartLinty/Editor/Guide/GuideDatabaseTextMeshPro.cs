using System.Collections.Generic;
using TMPro;

public interface ITextMeshProGuideable { GuideItem GetData(TMP_Text textMeshPro); }

public class GuideDatabaseTextMeshPro : ITextMeshProGuideable
{
    public virtual GuideItem GetData(TMP_Text textMeshPro) => default;
    public static readonly Dictionary<string, GuideItem> TextMeshProGuides = new Dictionary<string, GuideItem>
    {
        ["TextMeshPro_Text"] = GuideDatabaseHelpers.Item("Text Input", "Menentukan isi teks yang ditampilkan.", "TextMeshPro tidak memiliki isi teks.", "TextMeshPro menampilkan teks."),
        ["TextMeshPro_Font"] = GuideDatabaseHelpers.Item("Font Asset", "Menentukan font asset yang digunakan.", "TextMeshPro tidak memiliki Font Asset.", "TextMeshPro memiliki Font Asset."),
        ["TextMeshPro_Material"] = GuideDatabaseHelpers.Item("Material Preset", "Menentukan material dan shader teks.", "Teks menggunakan material default.", "Teks menggunakan material preset."),
        ["TextMeshPro_FontSize"] = GuideDatabaseHelpers.Item("Font Size", "Mengatur ukuran karakter teks.", "Teks berukuran kecil.", "Teks berukuran normal.", "Teks berukuran besar."),
        ["TextMeshPro_Color"] = GuideDatabaseHelpers.Item("Color", "Menentukan warna dan transparansi teks.", "Teks transparan.", "Teks memiliki warna."),
        ["TextMeshPro_Alignment"] = GuideDatabaseHelpers.Item("Alignment", "Mengatur posisi perataan teks.", "Teks menggunakan alignment default.", "Teks menggunakan alignment khusus."),
        ["TextMeshPro_AutoSize"] = GuideDatabaseHelpers.Item("Auto Size", "Menentukan penyesuaian ukuran font otomatis.", "Ukuran teks mengikuti Font Size.", "Ukuran teks menyesuaikan area."),
        ["TextMeshPro_Wrapping"] = GuideDatabaseHelpers.Item("Word Wrapping", "Menentukan apakah teks dapat berpindah baris.", "Teks tidak melakukan wrapping.", "Teks dapat berpindah baris."),
        ["TextMeshPro_Overflow"] = GuideDatabaseHelpers.Item("Overflow", "Menentukan perilaku teks di luar area.", "Teks menggunakan overflow normal.", "Teks menggunakan overflow khusus."),
        ["TextMeshPro_RichText"] = GuideDatabaseHelpers.Item("Rich Text", "Mengaktifkan dukungan tag format teks.", "Rich Text nonaktif.", "Rich Text aktif."),
        ["TextMeshPro_Raycast"] = GuideDatabaseHelpers.Item("Raycast Target", "Menentukan apakah teks menerima raycast UI.", "Teks tidak menerima raycast.", "Teks menerima raycast."),
        ["TextMeshPro_Extra"] = GuideDatabaseHelpers.Item("Extra Settings", "Mengatur spacing dan pengaturan teks tambahan.", "Teks menggunakan spacing default.", "Teks memiliki pengaturan spacing tambahan.")
    };
}

public sealed class TextMeshProTextGuide : GuideDatabaseTextMeshPro { private readonly string key = "TextMeshPro_Text"; public override GuideItem GetData(TMP_Text c) => GuideDatabaseHelpers.State(TextMeshProGuides[key], c != null && !string.IsNullOrEmpty(c.text) ? 1 : 0); }
public sealed class TextMeshProFontGuide : GuideDatabaseTextMeshPro { private readonly string key = "TextMeshPro_Font"; public override GuideItem GetData(TMP_Text c) => GuideDatabaseHelpers.State(TextMeshProGuides[key], c != null && c.font != null ? 1 : 0); }
public sealed class TextMeshProMaterialGuide : GuideDatabaseTextMeshPro { private readonly string key = "TextMeshPro_Material"; public override GuideItem GetData(TMP_Text c) => GuideDatabaseHelpers.State(TextMeshProGuides[key], c != null && c.fontSharedMaterial != null ? 1 : 0); }
public sealed class TextMeshProFontSizeGuide : GuideDatabaseTextMeshPro { private readonly string key = "TextMeshPro_FontSize"; public override GuideItem GetData(TMP_Text c) => GuideDatabaseHelpers.State(TextMeshProGuides[key], c == null ? 1 : GuideDatabaseHelpers.Range(c.fontSize, 12f, 48f)); }
public sealed class TextMeshProColorGuide : GuideDatabaseTextMeshPro { private readonly string key = "TextMeshPro_Color"; public override GuideItem GetData(TMP_Text c) => GuideDatabaseHelpers.State(TextMeshProGuides[key], c == null ? 0 : c.color.a <= 0f ? 0 : 1); }
public sealed class TextMeshProAlignmentGuide : GuideDatabaseTextMeshPro { private readonly string key = "TextMeshPro_Alignment"; public override GuideItem GetData(TMP_Text c) => GuideDatabaseHelpers.State(TextMeshProGuides[key], c != null && c.alignment != TextAlignmentOptions.TopLeft ? 1 : 0); }
public sealed class TextMeshProAutoSizeGuide : GuideDatabaseTextMeshPro { private readonly string key = "TextMeshPro_AutoSize"; public override GuideItem GetData(TMP_Text c) => GuideDatabaseHelpers.State(TextMeshProGuides[key], c != null && c.enableAutoSizing ? 1 : 0); }
public sealed class TextMeshProWrappingGuide : GuideDatabaseTextMeshPro { private readonly string key = "TextMeshPro_Wrapping"; public override GuideItem GetData(TMP_Text c) => GuideDatabaseHelpers.State(TextMeshProGuides[key], c != null && c.textWrappingMode != TextWrappingModes.NoWrap ? 1 : 0); }
public sealed class TextMeshProOverflowGuide : GuideDatabaseTextMeshPro { private readonly string key = "TextMeshPro_Overflow"; public override GuideItem GetData(TMP_Text c) => GuideDatabaseHelpers.State(TextMeshProGuides[key], c != null && c.overflowMode != TextOverflowModes.Overflow ? 1 : 0); }
public sealed class TextMeshProRichTextGuide : GuideDatabaseTextMeshPro { private readonly string key = "TextMeshPro_RichText"; public override GuideItem GetData(TMP_Text c) => GuideDatabaseHelpers.State(TextMeshProGuides[key], c != null && c.richText ? 1 : 0); }
public sealed class TextMeshProRaycastGuide : GuideDatabaseTextMeshPro { private readonly string key = "TextMeshPro_Raycast"; public override GuideItem GetData(TMP_Text c) => GuideDatabaseHelpers.State(TextMeshProGuides[key], c != null && c.raycastTarget ? 1 : 0); }
public sealed class TextMeshProExtraSettingsGuide : GuideDatabaseTextMeshPro { private readonly string key = "TextMeshPro_Extra"; public override GuideItem GetData(TMP_Text c) => GuideDatabaseHelpers.State(TextMeshProGuides[key], c != null && (c.characterSpacing != 0f || c.wordSpacing != 0f || c.lineSpacing != 0f) ? 1 : 0); }
