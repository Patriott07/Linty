using System.Collections.Generic;
using UnityEngine;

public interface IGuideable
{
    GuideItem GetData(BoxCollider2D boxCollider2D);
}

public class GuideDatabaseBoxCollider2D : IGuideable
{
    // Method virtual agar bisa di-override oleh child class
    public virtual GuideItem GetData(BoxCollider2D boxCollider2D)
    {
        // Implementasi default (bisa dikembalikan kosong atau data pertama)
        return new GuideItem();
    }

    // Dictionary pusat data panduan
    public static Dictionary<string, GuideItem> BoxColliderGuides = new Dictionary<string, GuideItem>()
    {
        {
            "BoxCollider2D_Material",
            new GuideItem
            {
                propertyName = "Material",
                oneLineDesc =
                    "Menentukan Physics Material 2D untuk mengatur tingkat gesekan dan elastisitas pantulan kolider.",
                effectConditions = new Dictionary<int, string>()
                {
                    {
                        0,
                        "Kolider menggunakan nilai default bawaan dari Global Physics 2D Settings."
                    },
                    {
                        1,
                        "Kolider mengikuti parameter gesekan dan elastisitas pantulan sesuai file Physics Material 2D."
                    },
                },
            }
        },
        {
            "BoxCollider2D_IsTrigger",
            new GuideItem
            {
                propertyName = "Is Trigger",
                oneLineDesc =
                    "Menentukan apakah kotak kolider berfungsi sebagai pemicu kejadian (trigger) alih-alih penghalang fisik.",
                effectConditions = new Dictionary<int, string>()
                {
                    {
                        0,
                        "Objek berfungsi sebagai benda padat fisik murni yang menahan dan memantulkan objek lain saat bertabrakan."
                    },
                    {
                        1,
                        "Objek menjadi pemicu/sensor tembus pandang; tidak menahan objek lain namun memicu callback skrip saat disentuh."
                    },
                },
            }
        },
        {
            "BoxCollider2D_UsedByEffector",
            new GuideItem
            {
                propertyName = "Used By Effector",
                oneLineDesc = "Menghubungkan kotak kolider agar dapat berinteraksi khusus dengan komponen Effector 2D.",
                effectConditions = new Dictionary<int, string>()
                {
                    {
                        0,
                        "Kolider mengabaikan keberadaan komponen Effector 2D dan hanya bertindak sebagai kolider atau pemicu standar biasa."
                    },
                    {
                        1,
                        "Kolider memproses dan merespons efek gaya khusus yang dipancarkan oleh komponen Effector 2D yang terpasang."
                    }
                }
            }
        },
        {
            "BoxCollider2D_UsedByComposite",
            new GuideItem
            {
                propertyName = "Used By Composite",
                oneLineDesc = "Menggabungkan kotak kolider ini ke dalam sistem Composite Collider 2D untuk optimasi geometri.",
                effectConditions = new Dictionary<int, string>()
                {
                    {
                        0,
                        "Kolider berdiri sendiri sebagai entitas terpisah dan tidak digabungkan dengan komponen komposit apa pun di sekitarnya."
                    },
                    {
                        1,
                        "Geometri kotak kolider digabungkan secara otomatis ke dalam komponen Composite Collider 2D untuk membentuk satu kesatuan bidang tabrakan yang efisien."
                    }
                }
            }
        },
        {
            "BoxCollider2D_AutoTiling",
            new GuideItem
            {
                propertyName = "Auto Tiling",
                oneLineDesc = "Menyesuaikan ukuran kotak kolider secara otomatis ketika ukuran Sprite Renderer diubah (Tiled Mode).",
                effectConditions = new Dictionary<int, string>()
                {
                    {
                        0,
                        "Ukuran kotak kolider bersifat statis independen dan tidak akan berubah secara otomatis meskipun ukuran visual sprite diubah."
                    },
                    {
                        1,
                        "Ukuran dan bentuk Box Collider 2D akan otomatis menyesuaikan diri secara real-time mengikuti perubahan dimensi sprite yang berulang (tiled)."
                    }
                }
            }
        },
        {
            "BoxCollider2D_Offset",
            new GuideItem
            {
                propertyName = "Offset",
                oneLineDesc = "Mengatur pergeseran titik pusat posisi kotak kolider relatif terhadap titik pusat GameObject.",
                effectConditions = new Dictionary<int, string>()
                {
                    {
                        1,
                        "Titik pusat kotak kolider berada tepat di tengah-tengah titik poros (pivot) GameObject."
                    },
                    {
                        0,
                        "Titik pusat kotak kolider bergeser sejauh nilai koordinat X dan Y yang dimasukkan dari titik pusat aslinya."
                    }
                }
            }
        },
        {
            "BoxCollider2D_Size",
            new GuideItem
            {
                propertyName = "Size",
                oneLineDesc = "Menentukan lebar dan tinggi dari kotak kolider dalam satuan ruang lokal (local space).",
                effectConditions = new Dictionary<int, string>()
                {
                    {
                        1,
                        "Kotak kolider menutupi seluruh area visual objek dengan pas, memberikan deteksi benturan yang akurat dan natural."
                    },
                    {
                        2,
                        "Objek dapat mengalami masalah aneh seperti melayang di udara sebelum menyentuh lantai atau menembus batas visual karena ukuran kolider tidak sesuai dengan gambarnya."
                    }
                }
            }
        },
        {
            "BoxCollider2D_EdgeRadius",
            new GuideItem
            {
                propertyName = "Edge Radius",
                oneLineDesc = "Memberikan radius lengkungan pada sudut-sudut kotak kolider untuk menghindari benturan tajam.",
                effectConditions = new Dictionary<int, string>()
                {
                    {
                        0,
                        "Sudut kotak kolider berbentuk siku-siku tajam sempurna tanpa ada lengkungan radius tambahan di bagian tepinya."
                    },
                    {
                        1,
                        "Sudut dan tepi kotak kolider menjadi tumpul melingkar, membuat objek lebih mulus meluncur melewati celah atau sudut tajam."
                    }
                }
            }
        }
    };
}

public class CustomBoxCollider2D_Material : GuideDatabaseBoxCollider2D
{
    private string key = "BoxCollider2D_Material";

    public override GuideItem GetData(BoxCollider2D boxCollider2D)
    {
        if (boxCollider2D == null)
            return default;

        // Ambil data dari dictionary statis di parent class
        if (BoxColliderGuides.TryGetValue(key, out GuideItem guide))
        {
            // Tentukan key status aktif (True = 1, False = 0)
            int currentStateKey = boxCollider2D.sharedMaterial == null ? 0 : 1;
            guide.currentEffect = guide.effectConditions[currentStateKey];

            // Di sini kamu bisa memproses atau mengembalikan datanya
            return guide;
        }

        return default;
    }
}

public class CustomBoxCollider2D_Istrigger : GuideDatabaseBoxCollider2D
{
    private string key = "BoxCollider2D_IsTrigger";

    public override GuideItem GetData(BoxCollider2D boxCollider2D)
    {
        if (boxCollider2D == null)
            return default;

        // Ambil data dari dictionary statis di parent class
        if (BoxColliderGuides.TryGetValue(key, out GuideItem guide))
        {
            // Tentukan key status aktif (True = 1, False = 0)
            int currentStateKey = boxCollider2D.isTrigger ? 1 : 0;
            guide.currentEffect = guide.effectConditions[currentStateKey];

            // Di sini kamu bisa memproses atau mengembalikan datanya
            return guide;
        }

        return default;
    }
}

public class CustomBoxCollider2D_UsedByEfector : GuideDatabaseBoxCollider2D
{
    private string key = "BoxCollider2D_UsedByEffector";

    public override GuideItem GetData(BoxCollider2D boxCollider2D)
    {
        if (boxCollider2D == null)
            return default;

        // Ambil data dari dictionary statis di parent class
        if (BoxColliderGuides.TryGetValue(key, out GuideItem guide))
        {
            // Tentukan key status aktif (True = 1, False = 0)
            int currentStateKey = boxCollider2D.usedByEffector ? 1 : 0;
            guide.currentEffect = guide.effectConditions[currentStateKey];

            // Di sini kamu bisa memproses atau mengembalikan datanya
            return guide;
        }

        return default;
    }
}

public class CustomBoxCollider2D_UsedByComposite : GuideDatabaseBoxCollider2D
{
    private string key = "BoxCollider2D_UsedByComposite";

    public override GuideItem GetData(BoxCollider2D boxCollider2D)
    {
        if (boxCollider2D == null)
            return default;

        // Ambil data dari dictionary statis di parent class
        if (BoxColliderGuides.TryGetValue(key, out GuideItem guide))
        {
            // Tentukan key status aktif (True = 1, False = 0)
            int currentStateKey = boxCollider2D.compositeCapable ? 1 : 0;
            guide.currentEffect = guide.effectConditions[currentStateKey];

            // Di sini kamu bisa memproses atau mengembalikan datanya
            return guide;
        }

        return default;
    }
}

public class CustomBoxCollider2D_AutoTiling : GuideDatabaseBoxCollider2D
{
    private string key = "BoxCollider2D_AutoTiling";

    public override GuideItem GetData(BoxCollider2D boxCollider2D)
    {
        if (boxCollider2D == null)
            return default;

        // Ambil data dari dictionary statis di parent class
        if (BoxColliderGuides.TryGetValue(key, out GuideItem guide))
        {
            // Tentukan key status aktif (True = 1, False = 0)
            int currentStateKey = boxCollider2D.autoTiling ? 1 : 0;
            guide.currentEffect = guide.effectConditions[currentStateKey];

            // Di sini kamu bisa memproses atau mengembalikan datanya
            return guide;
        }

        return default;
    }
}

public class CustomBoxCollider2D_Offset : GuideDatabaseBoxCollider2D
{
    private string key = "BoxCollider2D_Offset";

    public override GuideItem GetData(BoxCollider2D boxCollider2D)
    {
        if (boxCollider2D == null)
            return default;

        // Ambil data dari dictionary statis di parent class
        if (BoxColliderGuides.TryGetValue(key, out GuideItem guide))
        {
            // Tentukan key status aktif (True = 1, False = 0)
            int currentStateKey = 1;

            // Default: Anggap pas
            SpriteRenderer sr = boxCollider2D.GetComponent<SpriteRenderer>();

            if (sr != null && sr.sprite != null)
            {
                Vector2 spriteSize = sr.sprite.bounds.size;
                Vector2 colSize = boxCollider2D.size;

                float diff =
                    Mathf.Abs(colSize.x - spriteSize.x) +
                    Mathf.Abs(colSize.y - spriteSize.y);

                // Jika selisihnya jauh, anggap terlalu besar/kecil (Key 2)
                currentStateKey = (diff < 0.1f) ? 1 : 2;
            }

            guide.currentEffect = guide.effectConditions[currentStateKey];

            // Di sini kamu bisa memproses atau mengembalikan datanya
            return guide;
        }

        return default;
    }
}

public class CustomBoxCollider2D_EdgeRadius : GuideDatabaseBoxCollider2D
{
    private string key = "BoxCollider2D_EdgeRadius";

    public override GuideItem GetData(BoxCollider2D boxCollider2D)
    {
        if (boxCollider2D == null)
            return default;

        // Ambil data dari dictionary statis di parent class
        if (BoxColliderGuides.TryGetValue(key, out GuideItem guide))
        {
            // Tentukan key status aktif (True = 1, False = 0)
            int currentStateKey = boxCollider2D.edgeRadius == 0 ? 0 : 1;
            guide.currentEffect = guide.effectConditions[currentStateKey];

            // Di sini kamu bisa memproses atau mengembalikan datanya
            return guide;
        }

        return default;
    }
}