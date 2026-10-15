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

        // KONDISI 2: Ada GameObject terpilih
        GUILayout.BeginHorizontal();
        GUILayout.Space(12);
        GUILayout.Label($"Objek Terpilih: {selectedObj.name}", EditorStyles.boldLabel);
        GUILayout.EndHorizontal();

        GUILayout.Space(8);

        // Loop semua rules secara otomatis!
        foreach (var rule in diagnosticRules)
        {
            if (rule.Evaluate(selectedObj))
            {
                rule.DrawDiagnosticUI(selectedObj);
                GUILayout.Space(6);
            }
        }
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
