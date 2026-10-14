using UnityEditor;
using UnityEngine;

public class MenuLinty : EditorWindow
{
    // Indeks tab yang sedang aktif (0: Diagnosa, 1: Guide, 2: Structure, 3: Setting)
    private int selectedTab = 0;
    private readonly string[] tabNames = { "Diagnosa", "Guide", "Structure", "Setting" };

    // Menambahkan Menu di bagian atas Unity
    [MenuItem("Tools/Linty Assistant Menu")]
    public static void ShowWindow()
    {
        MenuLinty window = GetWindow<MenuLinty>("Linty Asisten");
        window.minSize = new Vector2(380, 500);
    }

    private void OnEnable()
    {
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
        // 1. Latar Belakang Gelap Kustom
        Color darkBackground = new Color(0.18f, 0.18f, 0.18f);
        EditorGUI.DrawRect(new Rect(0, 0, position.width, position.height), darkBackground);

        GUILayout.Space(12);

        // 2. Judul Tab Navigasi
        GUIStyle titleStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            normal = { textColor = Color.white },
            fontSize = 12
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

    #region Tab Navigation Drawer
    private void DrawTabNavigation()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(10);

        for (int i = 0; i < tabNames.Length; i++)
        {
            bool isActive = (i == selectedTab);

            // Styling tombol
            GUIStyle btnStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = 11,
                fontStyle = FontStyle.Bold,
                fixedHeight = 28,
                fixedWidth = 82
            };

            // Beri warna pembeda untuk tab aktif
            if (isActive)
            {
                GUI.backgroundColor = new Color(0.92f, 0.75f, 0.28f); // Warna Kuning Linty
                btnStyle.normal.textColor = Color.black;
            }
            else
            {
                GUI.backgroundColor = new Color(0.25f, 0.25f, 0.25f); // Warna Ababu gelap
                btnStyle.normal.textColor = Color.white;
            }

            if (GUILayout.Button(tabNames[i], btnStyle))
            {
                selectedTab = i;
            }

            // Reset warna background ke default Unity
            GUI.backgroundColor = Color.white;
        }

        GUILayout.EndHorizontal();
    }
    #endregion

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
                fontStyle = FontStyle.Bold
            };
            GUILayout.Label("Mulai Pilih Object Di dalam Hierarchy Panelmu.", infoStyle);
            GUILayout.EndHorizontal();
            
            return;
        }

        // KONDISI 2: Ada GameObject terpilih
        GUILayout.BeginHorizontal();
        GUILayout.Space(12);
        GUILayout.Label($"Objek Terpilih: {selectedObj.name}", EditorStyles.boldLabel);
        GUILayout.EndHorizontal();
        
        GUILayout.Space(8);

        // Cek Komponen Rigidbody (3D)
        Rigidbody rb = selectedObj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            DrawRigidbodyGuide(rb);
        }
        else
        {
            EditorGUILayout.HelpBox("Objek terpilih tidak memiliki komponen yang dipantau (misal: Rigidbody).", MessageType.Info);
        }
    }

    private void DrawGuideTab()
    {
        EditorGUILayout.HelpBox("Halaman Guide: Tempat panduan penggunaan komponen.", MessageType.Info);
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

        // Gambar latar oranye
        EditorGUI.DrawRect(bannerRect, new Color(0.85f, 0.45f, 0.15f));

        GUILayout.BeginArea(bannerRect);
        GUILayout.BeginHorizontal();
        GUILayout.Space(10);

        GUIStyle warningStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            normal = { textColor = Color.white },
            fontSize = 11,
            alignment = TextAnchor.MiddleLeft
        };

        EditorGUILayout.LabelField($"⚠️  {message}", warningStyle, GUILayout.Height(38));

        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    private void DrawRigidbodyGuide(Rigidbody rb)
    {
        // 1. Dokumen / Penjelasan
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        EditorGUILayout.LabelField("📘 Rigidbody Guide", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Fungsi: Memberikan simulasi fisika 3D (gravitasi, massa, dan tabrakan). Berguna untuk melakukan Collision System.", EditorStyles.wordWrappedLabel);
        EditorGUILayout.EndVertical();

        GUILayout.Space(8);

        // 2. Smart Diagnostic & Fix
        if ((rb.constraints & RigidbodyConstraints.FreezeRotationZ) == 0)
        {
            EditorGUILayout.HelpBox("⚠️ Warning: Freeze Rotation Z belum tercentang! Karakter bisa berguling/tumbang saat berjalan.", MessageType.Warning);

            if (GUILayout.Button("Fix Instantly (Freeze Rotation Z)", GUILayout.Height(30)))
            {
                Undo.RecordObject(rb, "Fix Rigidbody Rotation");
                rb.constraints |= RigidbodyConstraints.FreezeRotationZ;
            }
        }
        else
        {
            EditorGUILayout.HelpBox("✅ Rigidbody dikonfigurasi dengan baik (Freeze Rotation Z Aktif).", MessageType.Info);
        }
    }
    #endregion
}