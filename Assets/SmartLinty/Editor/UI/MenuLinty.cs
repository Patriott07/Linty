using System.Collections;
using System.Collections.Generic;
using Custom.Interface;
using UnityEditor;
using UnityEngine;

public class MenuLinty : EditorWindow
{
    // Indeks tab yang sedang aktif (0: Diagnosa, 1: Guide, 2: Structure, 3: Setting)
    private int selectedTab = 0;
    private Texture2D activeTabTex;
    private Texture2D inactiveTabTex;
    private Texture2D normal_text,
        hover_text,
        active_text;
    private readonly string[] tabNames = { "Diagnosa", "Guide", "Structure", "Setting" };

    private List<IDiagnosticRule> diagnosticRules = new List<IDiagnosticRule>()
    {
        // new TransformDiagnostic(),
        // new Rigidbody2DDiagnostic(),
        // new BoxCollider2DDiagnostic(),
        // new UIEventSystemDiagnostic(),
    };

    // Menambahkan Menu di bagian atas Unity
    [MenuItem("Tools/Linty Assistant Menu")]
    public static void ShowWindow()
    {
        MenuLinty window = GetWindow<MenuLinty>("Linty Asisten");
        window.minSize = new Vector2(380, 500);
    }

    // Pastikan tekstur ini di-generate (bisa ditaruh di OnEnable atau dipanggil pas dibutuhkan)
    private void InitTabTextures()
    {
        if (activeTabTex == null)
        {
            activeTabTex = new Texture2D(1, 1);
            activeTabTex.SetPixel(0, 0, GenerateColorByHexa255(197, 104, 47)); // Warna Aktif (Kuning/Oranye Linty)
            activeTabTex.Apply();
        }

        if (inactiveTabTex == null)
        {
            inactiveTabTex = new Texture2D(1, 1);
            inactiveTabTex.SetPixel(0, 0, GenerateColorByHexa255(18, 18, 18)); // Warna Non-Aktif (Abu-abu gelap rapi)
            inactiveTabTex.Apply();
        }

        if (normal_text == null)
        {
            normal_text = new Texture2D(1, 1);
            normal_text.SetPixel(0, 0, GenerateColorByHexa255(39, 174, 96)); // Warna Non-Aktif (Abu-abu gelap rapi)
            normal_text.Apply();
        }
        if (hover_text == null)
        {
            hover_text = new Texture2D(1, 1);
            hover_text.SetPixel(0, 0, GenerateColorByHexa255(39, 184, 106)); // Warna Non-Aktif (Abu-abu gelap rapi)
            hover_text.Apply();
        }
        if (active_text == null)
        {
            active_text = new Texture2D(1, 1);
            active_text.SetPixel(0, 0, GenerateColorByHexa255(20, 154, 86)); // Warna Non-Aktif (Abu-abu gelap rapi)
            active_text.Apply();
        }
    }

    Color GenerateColorByHexa255(float r, float g, float b)
    {
        return new Color(r / 255, g / 255, b / 255);
    }

    private void OnEnable()
    {
        InitTabTextures();
        // Daftarkan listener agar UI mendeteksi perubahan seleksi di Hierarchy
        Selection.selectionChanged += OnSelectionChanged;
    }

    private void OnDisable()
    {
        // Lepas listener agar tidak memicu memory leak
        Selection.selectionChanged -= OnSelectionChanged;
    }

    private void OnSelectionChanged()
    {
        // Minta Unity menggambar ulang UI saat ada objek baru dipilih
        Repaint();
    }

    private void OnGUI()
    {
        // Debug.Log("position.width : " + position.width);
        // 1. Latar Belakang Gelap Kustom
        Color darkBackground = new Color(0.18f, 0.18f, 0.18f);
        EditorGUI.DrawRect(new Rect(0, 0, position.width, position.height), darkBackground);

        GUILayout.Space(12);

        // 2. Judul Tab Navigasi
        GUIStyle titleStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            normal = { textColor = Color.white },
            fontSize = 12,
        };

        GUILayout.BeginHorizontal();
        GUILayout.Space(12);
        GUILayout.Label("Tab Navigasi :", titleStyle);
        GUILayout.EndHorizontal();

        GUILayout.Space(6);

        // 3. Render Tombol Navigasi Tab
        DrawTabNavigation();

        GUILayout.Space(12);

        // 4. Render Konten Sesuai Tab yang Dipilih
        switch (selectedTab)
        {
            case 0:
                DrawDiagnosaTab();
                break;
            case 1:
                DrawGuideTab();
                break;
            case 2:
                DrawStructureTab();
                break;
            case 3:
                DrawSettingTab();
                break;
        }
    }

    private void DrawTabNavigation()
    {
        InitTabTextures();

        GUILayout.BeginHorizontal();
        GUILayout.Space(10);

        for (int i = 0; i < tabNames.Length; i++)
        {
            bool isActive = (i == selectedTab);

            // Styling tombol menggunakan background tekstur kustom murni (bebas dari tint bawaan Unity)
            GUIStyle btnStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = 12,
                fontStyle = FontStyle.Bold,
                fixedHeight = 28,
                fixedWidth = 82,
                alignment = TextAnchor.MiddleCenter,
                normal =
                {
                    background = isActive ? activeTabTex : inactiveTabTex,
                    textColor = Color.white,
                },
                hover = { background = activeTabTex, textColor = Color.white },
                active = { background = activeTabTex, textColor = new Color(0.9f, 0.9f, 0.9f) },
            };

            if (GUILayout.Button(tabNames[i], btnStyle))
            {
                selectedTab = i;
            }
        }

        GUILayout.EndHorizontal();
    }

    #region Tab Content Renderers
    private void DrawDiagnosaTab()
    {
        GameObject selectedObj = Selection.activeGameObject;

        // KONDISI 1: Tidak ada GameObject yang dipilih di Hierarchy
        if (selectedObj == null)
        {
            DrawWarningBanner("Tidak ada Component ditemukan.");

            GUILayout.Space(8);
            GUILayout.BeginHorizontal();
            GUILayout.Space(12);

            GUIStyle infoStyle = new GUIStyle(EditorStyles.label)
            {
                normal = { textColor = Color.white },
                fontSize = 11,
                fontStyle = FontStyle.Bold,
            };
            GUILayout.Label("Mulai Pilih Object Di dalam Hierarchy Panelmu.", infoStyle);
            GUILayout.EndHorizontal();

            return;
        }

        // 1. Bagian Informasi Dasar
        DrawBasicInfoSection(selectedObj);

        // Horizontal divider di bawah Basic Info
        // GUILayout.Space(4);
        GUILayout.BeginHorizontal();
        GUILayout.Space(12);
        Rect dividerRect = new Rect(12, GUILayoutUtility.GetLastRect().yMax, position.width -24, 1);
        EditorGUI.DrawRect(dividerRect, new Color(0.5f, 0.5f, 0.5f));
        GUILayout.Space(12);
        GUILayout.EndHorizontal();
        GUILayout.Space(6);

        // 2. Bagian Diagnosa Komponen
        DrawComponentDiagnosisSection(selectedObj);

        // 3. Tombol Aksi Massal di paling bawah
        GUIStyle fixAllBtnStyle = new GUIStyle(GUI.skin.button)
        {
            fontSize = 12,
            fontStyle = FontStyle.Bold,
            fixedHeight = 36,
            fixedWidth = 0,
            alignment = TextAnchor.MiddleCenter,
            normal =
            {
                background = normal_text, // Warna Hijau Linty solid
                textColor = Color.white,
            },
            hover =
            {
                background = hover_text, // hover state
                textColor = Color.white,
            },
            active =
            {
                background = active_text, // active state
                textColor = new Color(0.8f, 0.9f, 0.8f),
            },
        };

        GUILayout.BeginHorizontal();
        GUILayout.Space(12);
        if (GUILayout.Button("Fix All Suggestion", fixAllBtnStyle, GUILayout.ExpandWidth(true)))
        {
            // Execute all fixes logic
        }
        GUILayout.EndHorizontal();
    }

    private bool showDesc = true;
    private bool enable = true;

    private void DrawBasicInfoSection(GameObject obj)
    {
        // Draw manual background rect (sharp corners, no skin box rounding)
        // Rect infoRect = GUILayoutUtility.GetLastRect();
        // infoRect = new Rect(12, infoRect.yMin, position.width - 24, 80);
        // EditorGUI.DrawRect(infoRect, new Color(0.2f, 0.2f, 0.2f));

        GUILayout.BeginHorizontal();
        GUILayout.Space(12);
        GUILayout.Label($"Nama Object: {obj.name}", EditorStyles.boldLabel);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Space(12);
        GUILayout.Label($"Tags: [Default]", EditorStyles.label);
        GUILayout.EndHorizontal();
        // GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();
        GUILayout.Space(12);
        GUILayout.Label($"Layer: {LayerMask.LayerToName(obj.layer)}", EditorStyles.label);
        GUILayout.Space(12);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Space(12);
        // showDesc = GUILayout.Toggle(showDesc, "Show Desc", GUILayout.Width(80));
        // GUILayout.FlexibleSpace();
        enable = GUILayout.Toggle(enable, "Enable Diagnostic This Object", GUILayout.Width(position.width - 24));
        GUILayout.EndHorizontal();

         GUILayout.Space(8);
    }

    private void DrawComponentDiagnosisSection(GameObject obj)
    {
        // Hitung komponen bermasalah
        int errorCount = 0;
        bool hasRigidbody = obj.TryGetComponent<Rigidbody>(out var rb);
        bool hasBoxCollider = obj.TryGetComponent<BoxCollider>(out var bc);
        bool hasMeshCollider = obj.TryGetComponent<MeshCollider>(out var mc);
        bool hasAnimator = obj.TryGetComponent<Animator>(out var anim);

        // Rigidbody check
        if (hasRigidbody && (rb.constraints & RigidbodyConstraints.FreezeRotationZ) == 0)
            errorCount++;
        // BoxCollider check
        if (hasBoxCollider && (bc.size == Vector3.zero || bc.isTrigger))
            errorCount++;
        // MeshCollider check
        if (hasMeshCollider && (!mc.convex || mc.isTrigger))
            errorCount++;
        // Animator check
        if (hasAnimator && anim.runtimeAnimatorController == null)
            errorCount++;

        // Warning banner ringkasan
        if (errorCount > 0)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(12);
            Rect bannerRect = EditorGUILayout.GetControlRect(false, 28);
            bannerRect.x += 10;
            bannerRect.width -= 20;
            EditorGUI.DrawRect(bannerRect, new Color(0.85f, 0.45f, 0.15f));
            GUI.Label(
                bannerRect,
                $" ⚠️  {errorCount} Component Mistake Setting",
                new GUIStyle(EditorStyles.boldLabel)
                {
                    normal = { textColor = Color.white },
                    fontSize = 12,
                    alignment = TextAnchor.MiddleLeft,
                }
            );
            GUILayout.EndHorizontal();
            GUILayout.Space(6);
        }

        // Daftar komponen dengan dot indicator
        // Rigidbody
        DrawComponentRow(
            "Rigidbody",
            hasRigidbody,
            hasRigidbody && (rb.constraints & RigidbodyConstraints.FreezeRotationZ) == 0,
            "Freeze Rotation Z belum diaktifkan",
            rb
        );

        // Box Collider
        DrawComponentRow(
            "Box Collider",
            hasBoxCollider,
            hasBoxCollider && bc.size != Vector3.zero && !bc.isTrigger,
            "Collider size valid",
            bc
        );

        // Mesh Collider
        DrawComponentRow(
            "Mesh Collider",
            hasMeshCollider,
            hasMeshCollider && mc.convex && !mc.isTrigger,
            "Mesh Collider konfigurasikan dengan baik",
            mc
        );

        // Animator
        DrawComponentRow(
            "Animator",
            hasAnimator,
            hasAnimator && anim.runtimeAnimatorController != null,
            "Animator Controller terassign",
            anim
        );
    }

    private void DrawComponentRow(
        string componentName,
        bool present,
        bool isHealthy,
        string message,
        Component comp
    )
    {
        Color dotColor = isHealthy ? new Color(0.2f, 0.8f, 0.2f) : new Color(0.9f, 0.3f, 0.1f);
        Color textColor = isHealthy ? Color.white : new Color(0.9f, 0.9f, 0.9f);

        GUILayout.BeginHorizontal();
        GUILayout.Space(12);

        // Dot indicator (lingkaran siku-siku)
        Rect dotRect = GUILayoutUtility.GetRect(12, 12);
        EditorGUI.DrawRect(new Rect(dotRect.x, dotRect.center.y - 6, 12, 12), new Color(0, 0, 0));
        EditorGUI.DrawRect(new Rect(dotRect.x + 1, dotRect.center.y - 5, 10, 10), dotColor);

        // Component name
        GUILayout.Label(componentName, GUILayout.Width(100));

        // Status teks
        GUILayout.Label(isHealthy ? "✓" : "⚠", GUILayout.Width(20));

        // Detail message & Apply Fix button
        GUILayout.Label(message, GUILayout.Width(150));
        if (GUILayout.Button("Apply Fix", GUILayout.Width(80)))
        {
            // Apply fix logic for this component
            Undo.RecordObject(comp, $"Fix {componentName}");
            if (comp is Rigidbody rb)
                rb.constraints |= RigidbodyConstraints.FreezeRotationZ;
            if (comp is BoxCollider bc)
            {
                if (bc.isTrigger)
                    bc.isTrigger = false;
                if (bc.size == Vector3.zero)
                    bc.size = new Vector3(1, 1, 1);
            }
            if (comp is MeshCollider mc)
            {
                if (!mc.convex)
                    mc.convex = true;
                if (mc.isTrigger)
                    mc.isTrigger = false;
            }
            if (comp is Animator anim)
            {
                if (anim.runtimeAnimatorController == null)
                    anim.runtimeAnimatorController = null;
            }
            Repaint();
        }

        GUILayout.EndHorizontal();
        GUILayout.Space(4);
    }

    private void DrawGuideTab()
    {
        EditorGUILayout.HelpBox(
            "Halaman Guide: Tempat panduan penggunaan komponen.",
            MessageType.Info
        );
    }

    private void DrawStructureTab()
    {
        EditorGUILayout.HelpBox(
            "Halaman Structure: Tempat mengelola struktur folder project.",
            MessageType.Info
        );
    }

    private void DrawSettingTab()
    {
        EditorGUILayout.HelpBox("Halaman Setting: Pengaturan SmartLinty.", MessageType.Info);
    }
    #endregion

    #region Component Diagnostics & UI Helpers
    private void DrawWarningBanner(string message)
    {
        Rect bannerRect = EditorGUILayout.GetControlRect(false, 38);
        bannerRect.x += 10;
        bannerRect.width -= 20;

        // Gambar latar oranye
        EditorGUI.DrawRect(bannerRect, new Color(0.85f, 0.45f, 0.15f));
        GUI.Label(
            bannerRect,
            $" ⚠️  {message}",
            new GUIStyle(EditorStyles.boldLabel)
            {
                normal = { textColor = Color.white },
                fontSize = 12,
                alignment = TextAnchor.MiddleLeft,
            }
        );
    }

    private void DrawRigidbodyGuide(Rigidbody rb)
    {
        // 1. Dokumen / Penjelasan
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        EditorGUILayout.LabelField("📘 Rigidbody Guide", EditorStyles.boldLabel);
        EditorGUILayout.LabelField(
            "Fungsi: Memberikan simulasi fisika 3D (gravitasi, massa, dan tabrakan). Berguna untuk melakukan Collision System.",
            EditorStyles.wordWrappedLabel
        );
        EditorGUILayout.EndVertical();

        GUILayout.Space(8);

        // 2. Smart Diagnostic & Fix
        if ((rb.constraints & RigidbodyConstraints.FreezeRotationZ) == 0)
        {
            EditorGUILayout.HelpBox(
                "⚠️ Warning: Freeze Rotation Z belum tercentang! Karakter bisa berguling/tumbang saat berjalan.",
                MessageType.Warning
            );

            if (GUILayout.Button("Fix Instantly (Freeze Rotation Z)", GUILayout.Height(30)))
            {
                Undo.RecordObject(rb, "Fix Rigidbody Rotation");
                rb.constraints |= RigidbodyConstraints.FreezeRotationZ;
            }
        }
        else
        {
            EditorGUILayout.HelpBox(
                "✅ Rigidbody dikonfigurasi dengan baik (Freeze Rotation Z Aktif).",
                MessageType.Info
            );
        }
    }
    #endregion
}
