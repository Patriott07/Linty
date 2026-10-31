using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public interface ILight2DGuideable { GuideItem GetData(Light2D light2D); }

public class GuideDatabaseLight2D : ILight2DGuideable
{
    public virtual GuideItem GetData(Light2D light2D) => default;
    public static readonly Dictionary<string, GuideItem> Light2DGuides = new Dictionary<string, GuideItem>
    {
        ["Light2D_Type"] = GuideDatabaseHelpers.Item("Light Type", "Menentukan bentuk dan penyebaran cahaya 2D.", "Cahaya menggunakan Point atau Global.", "Cahaya menggunakan Freeform.", "Cahaya menggunakan Sprite."),
        ["Light2D_BlendStyle"] = GuideDatabaseHelpers.Item("Blend Style", "Menentukan cara warna cahaya digabungkan dengan Sprite.", "Blend Style menggunakan default.", "Blend Style menggunakan style yang dipilih."),
        ["Light2D_Color"] = GuideDatabaseHelpers.Item("Color", "Menentukan warna cahaya 2D.", "Cahaya transparan.", "Cahaya memiliki warna."),
        ["Light2D_Intensity"] = GuideDatabaseHelpers.Item("Intensity", "Mengatur kekuatan cahaya 2D.", "Cahaya redup.", "Cahaya memiliki intensitas normal.", "Cahaya kuat."),
        ["Light2D_SortingLayers"] = GuideDatabaseHelpers.Item("Target Sorting Layers", "Menentukan Sorting Layer yang menerima cahaya.", "Cahaya menggunakan layer default.", "Cahaya memiliki target Sorting Layer."),
        ["Light2D_Falloff"] = GuideDatabaseHelpers.Item("Falloff Intensity", "Mengatur pelemahan cahaya berdasarkan jarak.", "Cahaya menyebar lebih jauh.", "Cahaya melemah lebih cepat."),
        ["Light2D_PointRadius"] = GuideDatabaseHelpers.Item("Point Light Radius", "Menentukan radius pengaruh Point Light.", "Cahaya memiliki radius kecil.", "Cahaya memiliki radius besar."),
        ["Light2D_Shadow"] = GuideDatabaseHelpers.Item("Shadow Strength", "Mengatur kekuatan bayangan 2D.", "Bayangan tidak aktif atau lembut.", "Bayangan aktif.")
    };
}

public sealed class Light2DTypeGuide : GuideDatabaseLight2D { private readonly string key = "Light2D_Type"; public override GuideItem GetData(Light2D c) => GuideDatabaseHelpers.State(Light2DGuides[key], c == null ? 0 : c.lightType == Light2D.LightType.Freeform ? 1 : c.lightType == Light2D.LightType.Sprite ? 2 : 0); }
public sealed class Light2DBlendStyleGuide : GuideDatabaseLight2D { private readonly string key = "Light2D_BlendStyle"; public override GuideItem GetData(Light2D c) => GuideDatabaseHelpers.State(Light2DGuides[key], c != null && c.blendStyleIndex > 0 ? 1 : 0); }
public sealed class Light2DColorGuide : GuideDatabaseLight2D { private readonly string key = "Light2D_Color"; public override GuideItem GetData(Light2D c) => GuideDatabaseHelpers.State(Light2DGuides[key], c == null ? 0 : c.color.a <= 0f ? 0 : 1); }
public sealed class Light2DIntensityGuide : GuideDatabaseLight2D { private readonly string key = "Light2D_Intensity"; public override GuideItem GetData(Light2D c) => GuideDatabaseHelpers.State(Light2DGuides[key], c == null ? 0 : GuideDatabaseHelpers.Range(c.intensity, 0f, 1f)); }
public sealed class Light2DSortingLayersGuide : GuideDatabaseLight2D { private readonly string key = "Light2D_SortingLayers"; public override GuideItem GetData(Light2D c) => GuideDatabaseHelpers.State(Light2DGuides[key], c != null && c.lightType != Light2D.LightType.Global ? 1 : 0); }
public sealed class Light2DFalloffGuide : GuideDatabaseLight2D { private readonly string key = "Light2D_Falloff"; public override GuideItem GetData(Light2D c) => GuideDatabaseHelpers.State(Light2DGuides[key], c != null && c.falloffIntensity > 0.5f ? 1 : 0); }
public sealed class Light2DPointRadiusGuide : GuideDatabaseLight2D { private readonly string key = "Light2D_PointRadius"; public override GuideItem GetData(Light2D c) => GuideDatabaseHelpers.State(Light2DGuides[key], c != null && c.pointLightOuterRadius > 1f ? 1 : 0); }
public sealed class Light2DShadowGuide : GuideDatabaseLight2D { private readonly string key = "Light2D_Shadow"; public override GuideItem GetData(Light2D c) => GuideDatabaseHelpers.State(Light2DGuides[key], c != null && c.shadowsEnabled ? 1 : 0); }
