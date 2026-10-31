using System.Collections.Generic;
using UnityEngine;

public interface IRigidbody2DGuideable
{
    GuideItem GetData(Rigidbody2D rigidbody2D);
}

public abstract class GuideDatabaseRigidbody2D : IRigidbody2DGuideable
{
    public abstract GuideItem GetData(Rigidbody2D rigidbody2D);

    protected static GuideItem GetGuide(string key)
    {
        return Rigidbody2DGuides.TryGetValue(key, out GuideItem guide) ? guide : default;
    }

    protected static void SetEffect(ref GuideItem guide, int state)
    {
        if (guide.effectConditions != null && guide.effectConditions.TryGetValue(state, out string effect))
        {
            guide.currentEffect = effect;
        }
    }

    public static readonly Dictionary<string, GuideItem> Rigidbody2DGuides =
        new Dictionary<string, GuideItem>
        {
            ["Rigidbody2D_BodyType"] = new GuideItem
            {
                propertyName = "Body Type",
                oneLineDesc = "Menentukan jenis perilaku fisika dasar objek apakah bergerak bebas, statis, atau kinematik.",
                effectConditions = new Dictionary<int, string>
                {
                    [0] = "Dynamic: objek terpengaruh gravitasi dan gaya fisika luar.",
                    [1] = "Kinematic: objek digerakkan secara manual dan tidak terpengaruh gaya luar.",
                    [2] = "Static: objek menjadi penghalang permanen yang tidak bergerak."
                }
            },
            ["Rigidbody2D_Material"] = new GuideItem
            {
                propertyName = "Material",
                oneLineDesc = "Menentukan Physics Material 2D untuk mengatur gesekan dan pantulan permukaan objek.",
                effectConditions = new Dictionary<int, string>
                {
                    [0] = "Tidak ada material khusus; objek menggunakan pengaturan fisika default.",
                    [1] = "Objek mengikuti gesekan dan pantulan dari Physics Material 2D yang dipilih."
                }
            },
            ["Rigidbody2D_Simulated"] = new GuideItem
            {
                propertyName = "Simulated",
                oneLineDesc = "Mengaktifkan atau menonaktifkan partisipasi objek dalam simulasi fisika.",
                effectConditions = new Dictionary<int, string>
                {
                    [0] = "Simulasi fisika nonaktif; gravitasi dan tabrakan tidak diproses.",
                    [1] = "Objek berpartisipasi penuh dalam simulasi fisika."
                }
            },
            ["Rigidbody2D_UseAutoMass"] = new GuideItem
            {
                propertyName = "Use Auto Mass",
                oneLineDesc = "Menghitung massa secara otomatis berdasarkan Collider 2D yang terpasang.",
                effectConditions = new Dictionary<int, string>
                {
                    [0] = "Massa ditentukan secara manual melalui properti Mass.",
                    [1] = "Massa dihitung otomatis berdasarkan luas Collider 2D."
                }
            },
            ["Rigidbody2D_Mass"] = new GuideItem
            {
                propertyName = "Mass",
                oneLineDesc = "Menentukan bobot objek yang mempengaruhi momentum dan tumbukan fisika.",
                effectConditions = new Dictionary<int, string>
                {
                    [0] = "Massa rendah (≤ 0.1); objek mudah terdorong atau terpental.",
                    [1] = "Massa berada pada rentang normal.",
                    [2] = "Massa tinggi (≥ 10); objek lebih sulit didorong."
                }
            },
            ["Rigidbody2D_LinearDamping"] = new GuideItem
            {
                propertyName = "Linear Damping",
                oneLineDesc = "Mengatur hambatan pergerakan linier untuk memperlambat laju objek.",
                effectConditions = new Dictionary<int, string>
                {
                    [0] = "Nilai 0; objek tidak memiliki hambatan linier.",
                    [1] = "Objek mengalami hambatan linier sedang.",
                    [2] = "Hambatan linier tinggi; laju objek cepat teredam."
                }
            },
            ["Rigidbody2D_AngularDamping"] = new GuideItem
            {
                propertyName = "Angular Damping",
                oneLineDesc = "Mengatur hambatan rotasi untuk memperlambat putaran objek.",
                effectConditions = new Dictionary<int, string>
                {
                    [0] = "Nilai 0; putaran tidak memiliki hambatan angular.",
                    [1] = "Objek mengalami hambatan rotasi sedang.",
                    [2] = "Hambatan rotasi tinggi; putaran cepat berhenti."
                }
            },
            ["Rigidbody2D_GravityScale"] = new GuideItem
            {
                propertyName = "Gravity Scale",
                oneLineDesc = "Mengatur pengali gravitasi dunia untuk objek 2D ini.",
                effectConditions = new Dictionary<int, string>
                {
                    [0] = "Nilai negatif; objek bergerak melawan arah gravitasi dunia.",
                    [1] = "Nilai 0; objek tidak terpengaruh gravitasi.",
                    [2] = "Nilai positif; objek mengikuti arah gravitasi dunia."
                }
            },
            ["Rigidbody2D_CollisionDetection"] = new GuideItem
            {
                propertyName = "Collision Detection",
                oneLineDesc = "Menentukan ketelitian deteksi tabrakan untuk mencegah tunneling.",
                effectConditions = new Dictionary<int, string>
                {
                    [0] = "Discrete; deteksi tabrakan standar dengan biaya komputasi lebih rendah.",
                    [1] = "Continuous; lintasan objek diperiksa lebih teliti untuk mencegah tunneling."
                }
            },
            ["Rigidbody2D_SleepingMode"] = new GuideItem
            {
                propertyName = "Sleeping Mode",
                oneLineDesc = "Mengatur perilaku sleep objek untuk menghemat perhitungan fisika.",
                effectConditions = new Dictionary<int, string>
                {
                    [0] = "Start Awake; objek aktif sejak scene dimulai.",
                    [1] = "Start Asleep; objek mulai tidur sampai dibangunkan fisika.",
                    [2] = "Never Sleep; objek tidak menggunakan mekanisme sleep otomatis."
                }
            },
            ["Rigidbody2D_Interpolate"] = new GuideItem
            {
                propertyName = "Interpolate",
                oneLineDesc = "Menghaluskan pergerakan visual objek antar frame fisika.",
                effectConditions = new Dictionary<int, string>
                {
                    [0] = "None; posisi visual mengikuti hasil kalkulasi fisika secara langsung.",
                    [1] = "Interpolate; posisi visual dihaluskan dari frame sebelumnya.",
                    [2] = "Extrapolate; posisi visual diestimasi ke depan."
                }
            },
            ["Rigidbody2D_Constraints"] = new GuideItem
            {
                propertyName = "Constraints",
                oneLineDesc = "Membatasi pergerakan posisi atau rotasi dari pengaruh fisika eksternal.",
                effectConditions = new Dictionary<int, string>
                {
                    [0] = "Tidak ada sumbu yang dibekukan; posisi dan rotasi bebas.",
                    [1] = "Sebagian sumbu posisi atau rotasi dibekukan.",
                    [2] = "Seluruh sumbu posisi dan rotasi yang tersedia dibekukan."
                }
            }
        };
}

public sealed class Rigidbody2DBodyTypeGuide : GuideDatabaseRigidbody2D
{
    public override GuideItem GetData(Rigidbody2D component)
    {
        GuideItem guide = GetGuide("Rigidbody2D_BodyType");
        if (component != null) SetEffect(ref guide, (int)component.bodyType);
        return guide;
    }
}

public sealed class Rigidbody2DMaterialGuide : GuideDatabaseRigidbody2D
{
    public override GuideItem GetData(Rigidbody2D component)
    {
        GuideItem guide = GetGuide("Rigidbody2D_Material");
        if (component != null) SetEffect(ref guide, component.sharedMaterial == null ? 0 : 1);
        return guide;
    }
}

public sealed class Rigidbody2DSimulatedGuide : GuideDatabaseRigidbody2D
{
    public override GuideItem GetData(Rigidbody2D component)
    {
        GuideItem guide = GetGuide("Rigidbody2D_Simulated");
        if (component != null) SetEffect(ref guide, component.simulated ? 1 : 0);
        return guide;
    }
}

public sealed class Rigidbody2DAutoMassGuide : GuideDatabaseRigidbody2D
{
    public override GuideItem GetData(Rigidbody2D component)
    {
        GuideItem guide = GetGuide("Rigidbody2D_UseAutoMass");
        if (component != null) SetEffect(ref guide, component.useAutoMass ? 1 : 0);
        return guide;
    }
}

public sealed class Rigidbody2DMassGuide : GuideDatabaseRigidbody2D
{
    public override GuideItem GetData(Rigidbody2D component)
    {
        GuideItem guide = GetGuide("Rigidbody2D_Mass");
        if (component != null) SetEffect(ref guide, component.mass <= 0.1f ? 0 : component.mass >= 10f ? 2 : 1);
        return guide;
    }
}

public sealed class Rigidbody2DLinearDampingGuide : GuideDatabaseRigidbody2D
{
    public override GuideItem GetData(Rigidbody2D component)
    {
        GuideItem guide = GetGuide("Rigidbody2D_LinearDamping");
        if (component != null) SetEffect(ref guide, component.linearDamping == 0f ? 0 : component.linearDamping >= 1f ? 2 : 1);
        return guide;
    }
}

public sealed class Rigidbody2DAngularDampingGuide : GuideDatabaseRigidbody2D
{
    public override GuideItem GetData(Rigidbody2D component)
    {
        GuideItem guide = GetGuide("Rigidbody2D_AngularDamping");
        if (component != null) SetEffect(ref guide, component.angularDamping == 0f ? 0 : component.angularDamping >= 1f ? 2 : 1);
        return guide;
    }
}

public sealed class Rigidbody2DGravityScaleGuide : GuideDatabaseRigidbody2D
{
    public override GuideItem GetData(Rigidbody2D component)
    {
        GuideItem guide = GetGuide("Rigidbody2D_GravityScale");
        if (component != null) SetEffect(ref guide, component.gravityScale < 0f ? 0 : Mathf.Approximately(component.gravityScale, 0f) ? 1 : 2);
        return guide;
    }
}

public sealed class Rigidbody2DCollisionDetectionGuide : GuideDatabaseRigidbody2D
{
    public override GuideItem GetData(Rigidbody2D component)
    {
        GuideItem guide = GetGuide("Rigidbody2D_CollisionDetection");
        if (component != null) SetEffect(ref guide, component.collisionDetectionMode == CollisionDetectionMode2D.Continuous ? 1 : 0);
        return guide;
    }
}

public sealed class Rigidbody2DSleepingModeGuide : GuideDatabaseRigidbody2D
{
    public override GuideItem GetData(Rigidbody2D component)
    {
        GuideItem guide = GetGuide("Rigidbody2D_SleepingMode");
        if (component != null) SetEffect(ref guide, (int)component.sleepMode);
        return guide;
    }
}

public sealed class Rigidbody2DInterpolateGuide : GuideDatabaseRigidbody2D
{
    public override GuideItem GetData(Rigidbody2D component)
    {
        GuideItem guide = GetGuide("Rigidbody2D_Interpolate");
        if (component != null) SetEffect(ref guide, (int)component.interpolation);
        return guide;
    }
}

public sealed class Rigidbody2DConstraintsGuide : GuideDatabaseRigidbody2D
{
    public override GuideItem GetData(Rigidbody2D component)
    {
        GuideItem guide = GetGuide("Rigidbody2D_Constraints");
        if (component != null)
        {
            RigidbodyConstraints2D all = RigidbodyConstraints2D.FreezePositionX |
                RigidbodyConstraints2D.FreezePositionY |
                RigidbodyConstraints2D.FreezeRotation;
            int state = component.constraints == RigidbodyConstraints2D.None ? 0 :
                component.constraints == all ? 2 : 1;
            SetEffect(ref guide, state);
        }
        return guide;
    }
}
