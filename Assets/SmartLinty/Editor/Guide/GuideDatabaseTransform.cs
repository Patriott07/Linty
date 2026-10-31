using System.Collections.Generic;
using UnityEngine;

public interface ITransformGuideable
{
    GuideItem GetData(Transform transform);
}

public abstract class GuideDatabaseTransform : ITransformGuideable
{
    public abstract GuideItem GetData(Transform transform);

    protected static GuideItem GetGuide(string key)
    {
        return TransformGuides.TryGetValue(key, out GuideItem guide) ? guide : default;
    }

    protected static void SetEffect(ref GuideItem guide, int state)
    {
        if (guide.effectConditions != null && guide.effectConditions.TryGetValue(state, out string effect))
        {
            guide.currentEffect = effect;
        }
    }

    public static readonly Dictionary<string, GuideItem> TransformGuides =
        new Dictionary<string, GuideItem>
        {
            ["Transform_Position"] = new GuideItem
            {
                propertyName = "Position",
                oneLineDesc = "Menentukan koordinat letak GameObject di dalam world space.",
                effectConditions = new Dictionary<int, string>
                {
                    [0] = "Position berada di titik origin (0, 0, 0).",
                    [1] = "Position memiliki nilai positif pada setidaknya satu sumbu.",
                    [2] = "Position memiliki nilai negatif pada setidaknya satu sumbu.",
                    [3] = "Position memiliki kombinasi nilai positif dan negatif."
                }
            },
            ["Transform_Rotation"] = new GuideItem
            {
                propertyName = "Rotation",
                oneLineDesc = "Menentukan sudut kemiringan atau arah hadap GameObject pada sumbu X, Y, dan Z.",
                effectConditions = new Dictionary<int, string>
                {
                    [0] = "Rotation berada pada orientasi awal (0, 0, 0).",
                    [1] = "Objek berotasi pada sumbu Z, pola umum untuk game 2D.",
                    [2] = "Objek memiliki rotasi pada sumbu X atau Y dalam ruang 3D."
                }
            },
            ["Transform_Scale"] = new GuideItem
            {
                propertyName = "Scale",
                oneLineDesc = "Menentukan ukuran relatif GameObject pada sumbu X, Y, dan Z.",
                effectConditions = new Dictionary<int, string>
                {
                    [0] = "Scale bernilai 1; objek menggunakan ukuran standar.",
                    [1] = "Scale lebih besar dari 1 pada setidaknya satu sumbu; objek diperbesar.",
                    [2] = "Scale berada di antara 0 dan 1; objek diperkecil.",
                    [3] = "Scale memiliki nilai negatif; objek ter-flip pada sumbu terkait.",
                    [4] = "Scale memiliki kombinasi perubahan pembesaran, pengecilan, atau flip."
                }
            }
        };
}

public sealed class TransformPositionGuide : GuideDatabaseTransform
{
    public override GuideItem GetData(Transform component)
    {
        GuideItem guide = GetGuide("Transform_Position");
        if (component != null)
        {
            Vector3 value = component.position;
            bool hasPositive = value.x > 0f || value.y > 0f || value.z > 0f;
            bool hasNegative = value.x < 0f || value.y < 0f || value.z < 0f;
            SetEffect(ref guide, !hasPositive && !hasNegative ? 0 : hasPositive && hasNegative ? 3 : hasPositive ? 1 : 2);
        }
        return guide;
    }
}

public sealed class TransformRotationGuide : GuideDatabaseTransform
{
    public override GuideItem GetData(Transform component)
    {
        GuideItem guide = GetGuide("Transform_Rotation");
        if (component != null)
        {
            Vector3 value = component.eulerAngles;
            bool hasXOrY = !Mathf.Approximately(Mathf.Repeat(value.x, 360f), 0f) ||
                !Mathf.Approximately(Mathf.Repeat(value.y, 360f), 0f);
            bool hasZ = !Mathf.Approximately(Mathf.Repeat(value.z, 360f), 0f);
            SetEffect(ref guide, !hasXOrY && !hasZ ? 0 : hasXOrY ? 2 : 1);
        }
        return guide;
    }
}

public sealed class TransformScaleGuide : GuideDatabaseTransform
{
    public override GuideItem GetData(Transform component)
    {
        GuideItem guide = GetGuide("Transform_Scale");
        if (component != null)
        {
            Vector3 value = component.localScale;
            bool hasNegative = value.x < 0f || value.y < 0f || value.z < 0f;
            bool hasLarge = value.x > 1f || value.y > 1f || value.z > 1f;
            bool hasSmall = (value.x >= 0f && value.x < 1f) ||
                (value.y >= 0f && value.y < 1f) ||
                (value.z >= 0f && value.z < 1f);
            bool isDefault = Mathf.Approximately(value.x, 1f) &&
                Mathf.Approximately(value.y, 1f) &&
                Mathf.Approximately(value.z, 1f);

            int state = isDefault ? 0 :
                hasNegative && !hasLarge && !hasSmall ? 3 :
                !hasNegative && hasLarge && !hasSmall ? 1 :
                !hasNegative && !hasLarge && hasSmall ? 2 : 4;
            SetEffect(ref guide, state);
        }
        return guide;
    }
}
