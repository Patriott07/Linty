using System.Collections;
using System.Collections.Generic;
using Custom.Interface;
using UnityEditor;
using UnityEngine;

public class MenuLinty : EditorWindow
{
    private int selectedTab = 0;
    private Texture2D activeTabTex;
    private Texture2D inactiveTabTex;
    private readonly string[] tabNames = { "Diagnosa", "Guide", "Structure", "Setting" };

    // Status state untuk Tab Guide
    private bool showBoxCollider2DGuide = true;
    private bool showRigidbody2DGuide = true;

    private bool isTriggerEffectVisible = true;
    private bool sizeEffectVisible = true;
    private bool bodyTypeEffectVisible = true;

    private Vector2 guideScrollPos;
    private Vector2 diagScrollPos;

    private List<IDiagnosticRule> diagnosticRules = new List<IDiagnosticRule>();

    [MenuItem("Tools/Linty Assistant Menu")]
    public static void ShowWindow()
    {
        MenuLinty window = GetWindow<MenuLinty>("Linty Asisten");
        window.minSize = new Vector2(380, 500);
    }

    private void InitTabTextures()
    {
        if (activeTabTex == null)
        {
            activeTabTex = new Texture2D(1, 1);
            activeTabTex.SetPixel(0, 0, GenerateColorByHexa255(197, 104, 47)); 
            activeTabTex.Apply();
        }

        if (inactiveTabTex == null)
        {
            inactiveTabTex = new Texture2D(1, 1);
            inactiveTabTex.SetPixel(0, 0, GenerateColorByHexa255(18, 18, 18)); 
            inactiveTabTex.Apply();
        }
    }

    Color GenerateColorByHexa255(float r, float g, float b)
    {
        return new Color(r / 255, g / 255, b / 255);
    }

    private void OnEnable()
    {
        InitTabTextures();
        Selection.selectionChanged += OnSelectionChanged;
    }

    private void OnDisable()
    {
        Selection.selectionChanged -= OnSelectionChanged;
    }

    private void OnSelectionChanged()
    {
        Repaint();
    }

    private void OnGUI()
    {
        Color darkBackground = new Color(0.18f, 0.18f, 0.18f);
        EditorGUI.DrawRect(new Rect(0, 0, position.width, position.height), darkBackground);

        GUILayout.Space(12);

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

        DrawTabNavigation();

        GUILayout.Space(12);

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
        diagScrollPos = EditorGUILayout.BeginScrollView(diagScrollPos);
        GameObject selectedObj = Selection.activeGameObject;

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

            EditorGUILayout.EndScrollView();
            return;
        }

        GUILayout.BeginHorizontal();
        GUILayout.Space(12);
        GUILayout.Label($"Objek Terpilih: {selectedObj.name}", EditorStyles.boldLabel);
        GUILayout.EndHorizontal();

        GUILayout.Space(8);

        // Fallback Diagnosa langsung jika diagnosticRules belum diisi
        bool evaluatedAny = false;
        foreach (var rule in diagnosticRules)
        {
            if (rule.Evaluate(selectedObj))
            {
                rule.DrawDiagnosticUI(selectedObj);
                evaluatedAny = true;
                GUILayout.Space(6);
            }
        }

        // Jika list rule kosong, berikan diagnosa standar untuk Rigidbody2D & BoxCollider2D
        if (!evaluatedAny)
        {
            Rigidbody2D rb2d = selectedObj.GetComponent<Rigidbody2D>();
            BoxCollider2D box2d = selectedObj.GetComponent<BoxCollider2D>();

            if (rb2d == null && box2d == null)
            {
                EditorGUILayout.HelpBox("Objek ini tidak memiliki komponen Rigidbody 2D atau Box Collider 2D untuk didiagnosa.", MessageType.Info);
            }

            if (rb2d != null)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.LabelField("🟢 Rigidbody 2D Terdeteksi", EditorStyles.boldLabel);
                EditorGUILayout.LabelField($"Body Type: {rb2d.bodyType}", EditorStyles.label);
                EditorGUILayout.EndVertical();
                GUILayout.Space(5);
            }

            if (box2d != null)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                EditorGUILayout.LabelField("📦 Box Collider 2D Terdeteksi", EditorStyles.boldLabel);
                EditorGUILayout.LabelField($"Is Trigger: {box2d.isTrigger}", EditorStyles.label);
                EditorGUILayout.LabelField($"Size: {box2d.size}", EditorStyles.label);
                EditorGUILayout.EndVertical();
            }
        }

        EditorGUILayout.EndScrollView();
    }

    private void DrawGuideTab()
    {
        guideScrollPos = EditorGUILayout.BeginScrollView(guideScrollPos);

        GUILayout.Space(5);

        // 1. KELOMPOK BOX COLLIDER 2D
        DrawComponentHeaderBox("📦  Box Collider 2D", ref showBoxCollider2DGuide);
        if (showBoxCollider2DGuide)
        {
            GUILayout.Space(5);
            
            // Is Trigger Guide
            DrawGuideCard(
                "Is Trigger",
                "Menentukan apakah kotak kolider berfungsi sebagai pemicu kejadian (trigger) alih-alih penghalang fisik.",
                ref isTriggerEffectVisible,
                "1. When True : Kolider menjadi zona pemicu tembus pandang/tembus benda, tidak menahan objek lain.\n2. When False : Kolider berfungsi sebagai benda padat fisik murni yang menahan objek lain."
            );

            GUILayout.Space(8);

            // Size Guide
            DrawGuideCard(
                "Size",
                "Menentukan lebar dan tinggi dari kotak kolider dalam satuan ruang lokal (local space).",
                ref sizeEffectVisible,
                "1. When Adjusted to Sprite : Menutupi seluruh area visual objek dengan pas.\n2. When Too Large/Small : Objek dapat mengalami masalah aneh seperti melayang."
            );
        }

        GUILayout.Space(15);

        // 2. KELOMPOK RIGIDBODY 2D
        DrawComponentHeaderBox("🟢  Rigidbody 2D", ref showRigidbody2DGuide);
        if (showRigidbody2DGuide)
        {
            GUILayout.Space(5);

            // Body Type Guide
            DrawGuideCard(
                "Body Type",
                "Menentukan jenis perilaku fisika dasar objek apakah bergerak bebas, statis, atau kinematik.",
                ref bodyTypeEffectVisible,
                "1. When Dynamic : Objek aktif terpengaruh gravitasi & gaya luar.\n2. When Kinematic : Digerakkan manual lewat skrip/animasi.\n3. When Static : Dibekukan permanen sebagai penghalang."
            );
        }

        GUILayout.Space(10);
        EditorGUILayout.EndScrollView();
    }

    private void DrawStructureTab()
    {
        EditorGUILayout.HelpBox("Halaman Structure: Tempat mengelola struktur folder project.", MessageType.Info);
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

    private void DrawComponentHeaderBox(string title, ref bool foldoutStatus)
    {
        GUIStyle headerStyle = new GUIStyle(EditorStyles.toolbarButton)
        {
            alignment = TextAnchor.MiddleLeft,
            fontStyle = FontStyle.Bold,
            fontSize = 11,
            normal = { textColor = Color.white }
        };

        Color oldBg = GUI.backgroundColor;
        GUI.backgroundColor = new Color(0.28f, 0.28f, 0.28f);
        
        if (GUILayout.Button($" {(foldoutStatus ? "▼" : "▶")}  {title}", headerStyle, GUILayout.Height(24)))
        {
            foldoutStatus = !foldoutStatus;
        }
        GUI.backgroundColor = oldBg;
    }

private void DrawGuideCard(string propertyName, string oneLineDesc, ref bool isEffectShown, string effectDetailText)
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        
        // Baris Atas: Nama Property & Tombol "Tell Me More"
        GUILayout.BeginHorizontal();
        GUIStyle propNameStyle = new GUIStyle(EditorStyles.boldLabel) { normal = { textColor = Color.white } };
        GUILayout.Label(propertyName, propNameStyle);
        
        GUILayout.FlexibleSpace();
        
        GUIStyle linkStyle = new GUIStyle(EditorStyles.label) 
        { 
            // PERBAIKAN: Menggunakan 'new Color(...)' yang benar
            normal = { textColor = new Color(0.4f, 0.8f, 0.4f) }, 
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleRight
        };
        if (GUILayout.Button("Tell Me More", linkStyle, GUILayout.Width(90))) { }
        GUILayout.EndHorizontal();

        GUILayout.Space(3);

        // Deskripsi Singkat
        GUIStyle descStyle = new GUIStyle(EditorStyles.wordWrappedLabel) 
        { 
            normal = { textColor = new Color(0.8f, 0.8f, 0.8f) }, 
            fontSize = 10 
        };
        GUILayout.Label(oneLineDesc, descStyle);

        GUILayout.Space(6);

        // Styling untuk teks di dalam kotak hijau
        GUIStyle greenTextStyle = new GUIStyle(EditorStyles.label)
        {
            normal = { textColor = Color.black },
            fontSize = 9,
            fontStyle = FontStyle.Bold,
            wordWrap = true
        };

        string displayText = isEffectShown ? $"Current Effect :\n{effectDetailText}" : "Current Effect : ...";

        // PERBAIKAN: Tinggikan minimal box menjadi 95f agar teks panjang muat dengan sempurna tanpa terpotong
        float calculatedHeight = isEffectShown ? 95f : 26f;

        // Ambil rect untuk kartu utama
        Rect cardRect = EditorGUILayout.GetControlRect(false, calculatedHeight);
        
        // Background gelap kartu
        EditorGUI.DrawRect(cardRect, new Color(0.15f, 0.15f, 0.15f));

        // Kotak Hijau Muda
        Rect greenBoxRect = new Rect(cardRect.x + 6, cardRect.y + 4, cardRect.width - 40, cardRect.height - 8);
        EditorGUI.DrawRect(greenBoxRect, new Color(0.72f, 0.95f, 0.35f)); 

        // Render Teks di dalam kotak hijau dengan margin aman
        Rect textRect = new Rect(greenBoxRect.x + 6, greenBoxRect.y + 6, greenBoxRect.width - 12, greenBoxRect.height - 12);
        GUI.Label(textRect, displayText, greenTextStyle);

        // Tombol Ikon Mata (Eye Toggle) di sebelah kanan kotak hijau
        Rect eyeBtnRect = new Rect(cardRect.x + cardRect.width - 32, cardRect.y + (cardRect.height / 2) - 10, 26, 20);
        GUIStyle eyeStyle = new GUIStyle(GUI.skin.button) { fontSize = 11 };
        if (GUI.Button(eyeBtnRect, "👁️", eyeStyle))
        {
            isEffectShown = !isEffectShown;
        }

        EditorGUILayout.EndVertical();
    }
    #endregion
}