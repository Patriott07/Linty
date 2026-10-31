using System.Collections.Generic;
using UnityEngine;

public interface IAnimatorGuideable { GuideItem GetData(Animator animator); }

public class GuideDatabaseAnimator : IAnimatorGuideable
{
    public virtual GuideItem GetData(Animator animator) => default;
    public static readonly Dictionary<string, GuideItem> AnimatorGuides = new Dictionary<string, GuideItem>
    {
        ["Animator_Controller"] = GuideDatabaseHelpers.Item("Controller", "Menentukan Animator Controller.", "Animator tidak memiliki Controller.", "Animator memiliki Controller."),
        ["Animator_Avatar"] = GuideDatabaseHelpers.Item("Avatar", "Menentukan Avatar model humanoid.", "Animator tidak memiliki Avatar.", "Animator memiliki Avatar."),
        ["Animator_RootMotion"] = GuideDatabaseHelpers.Item("Apply Root Motion", "Menentukan apakah gerak animasi diterapkan ke Transform.", "Root Motion tidak diterapkan.", "Root Motion diterapkan."),
        ["Animator_UpdateMode"] = GuideDatabaseHelpers.Item("Update Mode", "Menentukan waktu pembaruan Animator.", "Animator menggunakan Normal.", "Animator menggunakan Animate Physics.", "Animator menggunakan Unscaled Time."),
        ["Animator_CullingMode"] = GuideDatabaseHelpers.Item("Culling Mode", "Menentukan perilaku Animator saat tidak terlihat Camera.", "Animator selalu diperbarui.", "Animator dapat dihentikan saat tidak terlihat."),
        ["Animator_Parameters"] = GuideDatabaseHelpers.Item("Parameters", "Menyediakan nilai untuk mengontrol transisi animasi.", "Animator tidak memiliki parameter.", "Animator memiliki parameter."),
        ["Animator_Layers"] = GuideDatabaseHelpers.Item("Layers dan Transitions", "Mengatur layer dan perpindahan state animasi.", "Animator tidak memiliki layer aktif.", "Animator memiliki layer animasi.")
    };
}

public sealed class AnimatorControllerGuide : GuideDatabaseAnimator { private readonly string key = "Animator_Controller"; public override GuideItem GetData(Animator c) => GuideDatabaseHelpers.State(AnimatorGuides[key], c != null && c.runtimeAnimatorController != null ? 1 : 0); }
public sealed class AnimatorAvatarGuide : GuideDatabaseAnimator { private readonly string key = "Animator_Avatar"; public override GuideItem GetData(Animator c) => GuideDatabaseHelpers.State(AnimatorGuides[key], c != null && c.avatar != null ? 1 : 0); }
public sealed class AnimatorRootMotionGuide : GuideDatabaseAnimator { private readonly string key = "Animator_RootMotion"; public override GuideItem GetData(Animator c) => GuideDatabaseHelpers.State(AnimatorGuides[key], c != null && c.applyRootMotion ? 1 : 0); }
public sealed class AnimatorUpdateModeGuide : GuideDatabaseAnimator { private readonly string key = "Animator_UpdateMode"; public override GuideItem GetData(Animator c) => GuideDatabaseHelpers.State(AnimatorGuides[key], c == null ? 0 : (int)c.updateMode); }
public sealed class AnimatorCullingModeGuide : GuideDatabaseAnimator { private readonly string key = "Animator_CullingMode"; public override GuideItem GetData(Animator c) => GuideDatabaseHelpers.State(AnimatorGuides[key], c != null && c.cullingMode == AnimatorCullingMode.AlwaysAnimate ? 0 : 1); }
public sealed class AnimatorParametersGuide : GuideDatabaseAnimator { private readonly string key = "Animator_Parameters"; public override GuideItem GetData(Animator c) => GuideDatabaseHelpers.State(AnimatorGuides[key], c != null && c.parameterCount > 0 ? 1 : 0); }
public sealed class AnimatorLayersGuide : GuideDatabaseAnimator { private readonly string key = "Animator_Layers"; public override GuideItem GetData(Animator c) => GuideDatabaseHelpers.State(AnimatorGuides[key], c != null && c.layerCount > 0 ? 1 : 0); }
