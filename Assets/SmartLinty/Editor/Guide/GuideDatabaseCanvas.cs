using System.Collections.Generic;
using UnityEngine;

public interface ICanvasGuideable
{
    GuideItem GetData(Canvas canvas);
}

public class GuideDatabaseCanvas : ICanvasGuideable
{
    public virtual GuideItem GetData(Canvas canvas) => default;

    public static readonly Dictionary<string, GuideItem> CanvasGuides = new Dictionary<
        string,
        GuideItem
    >
    {
        ["Canvas_RenderMode"] = GuideDatabaseHelpers.Item(
            "Render Mode",
            "Menentukan cara Canvas dirender terhadap kamera dan ruang dunia.",
            "Canvas menggunakan Screen Space - Overlay.",
            "Canvas menggunakan Screen Space - Camera.",
            "Canvas menggunakan World Space."
        ),
        ["Canvas_PixelPerfect"] = GuideDatabaseHelpers.Item(
            "Pixel Perfect",
            "Membuat elemen UI dirender lebih tajam dengan menyesuaikan posisi piksel.",
            "Pixel Perfect nonaktif.",
            "Pixel Perfect aktif."
        ),
        ["Canvas_RenderCamera"] = GuideDatabaseHelpers.Item(
            "Render Camera",
            "Menentukan Camera yang digunakan Canvas pada mode Screen Space - Camera.",
            "Canvas tidak memiliki Camera khusus.",
            "Canvas menggunakan Camera yang dipilih."
        ),
        ["Canvas_PlaneDistance"] = GuideDatabaseHelpers.Item(
            "Plane Distance",
            "Mengatur jarak Canvas dari Render Camera.",
            "Canvas dekat dengan Camera.",
            "Canvas berada pada jarak normal.",
            "Canvas jauh dari Camera."
        ),
        ["Canvas_SortingLayer"] = GuideDatabaseHelpers.Item(
            "Sorting Layer",
            "Menentukan lapisan render Canvas.",
            "Canvas berada pada layer default.",
            "Canvas berada pada layer khusus."
        ),
        ["Canvas_OrderInLayer"] = GuideDatabaseHelpers.Item(
            "Order in Layer",
            "Mengatur urutan Canvas dalam Sorting Layer yang sama.",
            "Canvas memiliki urutan rendah.",
            "Canvas memiliki urutan tinggi."
        ),
        ["Canvas_TargetDisplay"] = GuideDatabaseHelpers.Item(
            "Target Display",
            "Menentukan display tujuan untuk menampilkan Canvas.",
            "Canvas tampil pada Display 1.",
            "Canvas tampil pada display lain."
        ),
        ["Canvas_ShaderChannels"] = GuideDatabaseHelpers.Item(
            "Additional Shader Channels",
            "Menentukan data tambahan yang dikirim Canvas ke shader.",
            "Tidak ada channel shader tambahan.",
            "Canvas mengirim channel shader tambahan."
        ),
    };
}

public sealed class CanvasRenderModeGuide : GuideDatabaseCanvas
{
    private readonly string key = "Canvas_RenderMode";

    public override GuideItem GetData(Canvas c) =>
        GuideDatabaseHelpers.State(CanvasGuides[key], c == null ? 0 : (int)c.renderMode);
}

public sealed class CanvasPixelPerfectGuide : GuideDatabaseCanvas
{
    private readonly string key = "Canvas_PixelPerfect";

    public override GuideItem GetData(Canvas c) =>
        GuideDatabaseHelpers.State(CanvasGuides[key], c != null && c.pixelPerfect ? 1 : 0);
}

public sealed class CanvasRenderCameraGuide : GuideDatabaseCanvas
{
    private readonly string key = "Canvas_RenderCamera";

    public override GuideItem GetData(Canvas c) =>
        GuideDatabaseHelpers.State(CanvasGuides[key], c != null && c.worldCamera != null ? 1 : 0);
}

public sealed class CanvasPlaneDistanceGuide : GuideDatabaseCanvas
{
    private readonly string key = "Canvas_PlaneDistance";

    public override GuideItem GetData(Canvas c) =>
        GuideDatabaseHelpers.State(
            CanvasGuides[key],
            c == null ? 0 : GuideDatabaseHelpers.Range(c.planeDistance, 1f, 10f)
        );
}

public sealed class CanvasSortingLayerGuide : GuideDatabaseCanvas
{
    private readonly string key = "Canvas_SortingLayer";

    public override GuideItem GetData(Canvas c) =>
        GuideDatabaseHelpers.State(CanvasGuides[key], c != null && c.sortingLayerID != 0 ? 1 : 0);
}

public sealed class CanvasOrderInLayerGuide : GuideDatabaseCanvas
{
    private readonly string key = "Canvas_OrderInLayer";

    public override GuideItem GetData(Canvas c) =>
        GuideDatabaseHelpers.State(CanvasGuides[key], c != null && c.sortingOrder > 0 ? 1 : 0);
}

public sealed class CanvasTargetDisplayGuide : GuideDatabaseCanvas
{
    private readonly string key = "Canvas_TargetDisplay";

    public override GuideItem GetData(Canvas c) =>
        GuideDatabaseHelpers.State(CanvasGuides[key], c != null && c.targetDisplay > 0 ? 1 : 0);
}

public sealed class CanvasShaderChannelsGuide : GuideDatabaseCanvas
{
    private readonly string key = "Canvas_ShaderChannels";

    public override GuideItem GetData(Canvas c) =>
        GuideDatabaseHelpers.State(
            CanvasGuides[key],
            c != null && c.additionalShaderChannels != AdditionalCanvasShaderChannels.None ? 1 : 0
        );
}
