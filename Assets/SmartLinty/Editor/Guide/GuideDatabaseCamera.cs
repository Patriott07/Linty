using System.Collections.Generic;
using UnityEngine;

public interface ICameraGuideable { GuideItem GetData(Camera camera); }

public class GuideDatabaseCamera : ICameraGuideable
{
    public virtual GuideItem GetData(Camera camera) => default;
    public static readonly Dictionary<string, GuideItem> CameraGuides = new Dictionary<string, GuideItem>
    {
        ["Camera_ClearFlags"] = GuideDatabaseHelpers.Item("Clear Flags", "Menentukan cara Camera membersihkan tampilan.", "Camera menggunakan Skybox atau default.", "Camera menggunakan Solid Color.", "Camera hanya membersihkan depth."),
        ["Camera_Projection"] = GuideDatabaseHelpers.Item("Projection", "Menentukan perspektif atau proyeksi ortografis Camera.", "Camera menggunakan Perspective.", "Camera menggunakan Orthographic."),
        ["Camera_FieldOfView"] = GuideDatabaseHelpers.Item("Field of View", "Mengatur lebar area pandang Camera.", "Area pandang sempit.", "Area pandang normal.", "Area pandang luas."),
        ["Camera_CullingMask"] = GuideDatabaseHelpers.Item("Culling Mask", "Menentukan Layer yang dirender Camera.", "Camera tidak merender Layer.", "Camera merender Layer yang dipilih."),
        ["Camera_ClippingPlanes"] = GuideDatabaseHelpers.Item("Clipping Planes", "Menentukan jarak objek yang dirender Camera.", "Objek di luar rentang clipping.", "Objek berada dalam rentang clipping."),
        ["Camera_ViewportRect"] = GuideDatabaseHelpers.Item("Viewport Rect", "Menentukan area layar yang digunakan Camera.", "Camera menggunakan seluruh layar.", "Camera menggunakan sebagian layar."),
        ["Camera_Depth"] = GuideDatabaseHelpers.Item("Depth", "Menentukan urutan Camera saat merender.", "Camera dirender lebih awal.", "Camera dirender lebih akhir."),
        ["Camera_TargetTexture"] = GuideDatabaseHelpers.Item("Target Texture", "Menentukan Render Texture tujuan hasil render.", "Camera merender ke layar.", "Camera merender ke Render Texture."),
        ["Camera_Rendering"] = GuideDatabaseHelpers.Item("HDR dan Occlusion Culling", "Mengatur dukungan HDR dan penghilangan objek tertutup.", "Kedua fitur tidak aktif.", "Salah satu fitur aktif.", "Kedua fitur aktif.")
    };
}

public sealed class CameraClearFlagsGuide : GuideDatabaseCamera { private readonly string key = "Camera_ClearFlags"; public override GuideItem GetData(Camera c) => GuideDatabaseHelpers.State(CameraGuides[key], c == null ? 0 : c.clearFlags == CameraClearFlags.SolidColor ? 1 : c.clearFlags == CameraClearFlags.Depth ? 2 : 0); }
public sealed class CameraProjectionGuide : GuideDatabaseCamera { private readonly string key = "Camera_Projection"; public override GuideItem GetData(Camera c) => GuideDatabaseHelpers.State(CameraGuides[key], c != null && c.orthographic ? 1 : 0); }
public sealed class CameraFieldOfViewGuide : GuideDatabaseCamera { private readonly string key = "Camera_FieldOfView"; public override GuideItem GetData(Camera c) => GuideDatabaseHelpers.State(CameraGuides[key], c == null ? 1 : GuideDatabaseHelpers.Range(c.fieldOfView, 45f, 90f)); }
public sealed class CameraCullingMaskGuide : GuideDatabaseCamera { private readonly string key = "Camera_CullingMask"; public override GuideItem GetData(Camera c) => GuideDatabaseHelpers.State(CameraGuides[key], c != null && c.cullingMask != 0 ? 1 : 0); }
public sealed class CameraClippingPlanesGuide : GuideDatabaseCamera { private readonly string key = "Camera_ClippingPlanes"; public override GuideItem GetData(Camera c) => GuideDatabaseHelpers.State(CameraGuides[key], c != null && c.nearClipPlane < c.farClipPlane ? 1 : 0); }
public sealed class CameraViewportGuide : GuideDatabaseCamera { private readonly string key = "Camera_ViewportRect"; public override GuideItem GetData(Camera c) => GuideDatabaseHelpers.State(CameraGuides[key], c != null && (c.rect.x != 0 || c.rect.y != 0 || c.rect.width != 1 || c.rect.height != 1) ? 1 : 0); }
public sealed class CameraDepthGuide : GuideDatabaseCamera { private readonly string key = "Camera_Depth"; public override GuideItem GetData(Camera c) => GuideDatabaseHelpers.State(CameraGuides[key], c != null && c.depth > 0 ? 1 : 0); }
public sealed class CameraTargetTextureGuide : GuideDatabaseCamera { private readonly string key = "Camera_TargetTexture"; public override GuideItem GetData(Camera c) => GuideDatabaseHelpers.State(CameraGuides[key], c != null && c.targetTexture != null ? 1 : 0); }
public sealed class CameraRenderingGuide : GuideDatabaseCamera { private readonly string key = "Camera_Rendering"; public override GuideItem GetData(Camera c) => GuideDatabaseHelpers.State(CameraGuides[key], c == null ? 0 : (c.allowHDR ? 1 : 0) + (c.useOcclusionCulling ? 1 : 0)); }
