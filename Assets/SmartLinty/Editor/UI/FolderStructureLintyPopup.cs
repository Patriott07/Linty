using System.IO;
using UnityEditor;
using UnityEngine;

public class FolderStructureLintyPopup : EditorWindow
{
    private bool createAnimations = true;
    private bool create2DSprite = true;
    private bool createUI = true;
    private bool createAudio = true;
    private bool createScripts = true;
    private bool createPostProcessing = true;

    private static Texture2D limeTex;

    public static void OpenWindow()
    {
        FolderStructureLintyPopup window = GetWindow<FolderStructureLintyPopup>(
            true,
            "Project Setup - Listy Asistant",
            true
        );

        float width = 420;
        float height = 300;

        float x = (Screen.currentResolution.width - width) * 0.5f;
        float y = (Screen.currentResolution.height - height) * 0.5f;

        window.position = new Rect(x, y, width, height);
        window.minSize = new Vector2(width, height);
        window.maxSize = new Vector2(width, height);
        window.ShowUtility();
    }

    private void OnEnable()
    {
        // Tekstur rounded hijau untuk tombol "Make it"
        limeTex = MakeRoundedRectTex(110, 36, 0, new Color(0.38f, 0.62f, 0.2f));
    }

    private void OnGUI()
    {
        // Background utama gelap
        EditorGUI.DrawRect(
            new Rect(0, 0, position.width, position.height),
            new Color(0.18f, 0.18f, 0.18f)
        );

        float margin = 25f;
        float availableWidth = position.width - (margin * 2);

        GUILayout.BeginHorizontal();
        GUILayout.Space(margin);

        GUILayout.BeginVertical();
        GUILayout.Space(25);

        // Header Title & Subtitle
        GUIStyle titleStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 22,
            normal = { textColor = new Color(0.9f, 0.9f, 0.9f) },
        };
        GUIStyle subStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 11,
            normal = { textColor = new Color(0.92f, 0.52f, 0.2f) },
        };

        GUILayout.Label("Thanks for filling project detail", titleStyle);
        GUILayout.Space(4);
        GUILayout.Label(
            "Now I suggest you to making structure standart folder lorem sghaghdd avdhsahdgsa dsdadhjashdas dsjdjashdja dhsjdhasjd ",
            subStyle,
            GUILayout.Width(availableWidth)
        );

        GUILayout.Space(15);

        // Style Checkbox Custom
        GUIStyle toggleStyle = new GUIStyle(EditorStyles.toggle)
        {
            fontSize = 13,
            fontStyle = FontStyle.Bold,
            normal = { textColor = new Color(0.85f, 0.85f, 0.85f) },
            onNormal = { textColor = Color.white },
        };

        // Render Daftar Checkboxes
        GUILayout.BeginHorizontal();
        createAnimations = EditorGUILayout.Toggle(
            "Animations Folder",
            createAnimations,
            toggleStyle
        );
        GUILayout.FlexibleSpace();
        GUILayout.Label("Assets/Animation", GUILayout.Width(availableWidth));
        GUILayout.EndHorizontal();

        GUILayout.Space(1);
        create2DSprite = EditorGUILayout.Toggle("2D Sprite Folder", create2DSprite, toggleStyle);
        GUILayout.Space(1);
        createUI = EditorGUILayout.Toggle("UI Folder", createUI, toggleStyle);
        GUILayout.Space(1);
        createAudio = EditorGUILayout.Toggle("Audio Folder", createAudio, toggleStyle);
        GUILayout.Space(1);
        createScripts = EditorGUILayout.Toggle("Scripts Folder", createScripts, toggleStyle);
        GUILayout.Space(1);
        createPostProcessing = EditorGUILayout.Toggle(
            "Post Processing Folder",
            createPostProcessing,
            toggleStyle
        );

        GUILayout.Space(20);

        // Baris Bawah: Tombol "Make it" & Link "Tidak Sekarang"
        GUILayout.BeginHorizontal();

        GUIStyle makeItButtonStyle = new GUIStyle(GUI.skin.button)
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = 13,
            fontStyle = FontStyle.Bold,
            normal = { background = limeTex, textColor = Color.white },
        };

        if (
            GUILayout.Button(
                "Make it",
                makeItButtonStyle,
                GUILayout.Width(110),
                GUILayout.Height(36)
            )
        )
        {
            CreateSelectedFolders();
            Close();
        }

        // GUILayout.Space(120);
        GUILayout.FlexibleSpace();

        // Style untuk teks "Tidak Sekarang" (Gaya Link Interaktif)
        GUIStyle skipStyle = new GUIStyle(EditorStyles.label)
        {
            padding = new RectOffset(0, 0, 0, 0),
            alignment = TextAnchor.LowerRight,
            fontSize = 13,
            normal = { textColor = new Color(0.6f, 0.6f, 0.6f) },
            hover = { textColor = Color.white },
        };

        Rect skipRect = GUILayoutUtility.GetRect(100, 36, GUILayout.Width(130));
        if (GUI.Button(new Rect(skipRect.x, skipRect.y + 8, 100, 20), "Tidak Sekarang", skipStyle))
        {
            Debug.Log("Smart Linty: Folder creation skipped.");
            Close();
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
        // GUILayout.Space(margin);
        GUILayout.EndHorizontal();
    }

    // Fungsi otomatis eksekusi pembuatan folder ke Asset Database Unity
    private void CreateSelectedFolders()
    {
        string root = "Assets";
        if (createAnimations && !Directory.Exists(Path.Combine(root, "Animations")))
            AssetDatabase.CreateFolder(root, "Animations");
        if (create2DSprite && !Directory.Exists(Path.Combine(root, "2D Sprite")))
            AssetDatabase.CreateFolder(root, "2D Sprite");
        if (createUI && !Directory.Exists(Path.Combine(root, "UI")))
            AssetDatabase.CreateFolder(root, "UI");
        if (createAudio && !Directory.Exists(Path.Combine(root, "Audio")))
            AssetDatabase.CreateFolder(root, "Audio");
        if (createScripts && !Directory.Exists(Path.Combine(root, "Scripts")))
            AssetDatabase.CreateFolder(root, "Scripts");
        if (createPostProcessing && !Directory.Exists(Path.Combine(root, "Post Processing")))
            AssetDatabase.CreateFolder(root, "Post Processing");

        AssetDatabase.Refresh();
        Debug.Log("Smart Linty: Standard project folders created successfully!");
    }

    private Texture2D MakeRoundedRectTex(int width, int height, int radius, Color col)
    {
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        Color[] pix = new Color[width * height];
        Color transparent = new Color(0, 0, 0, 0);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int rx =
                    x < radius ? radius - x : (x >= width - radius ? x - (width - radius - 1) : 0);
                int ry =
                    y < radius
                        ? radius - y
                        : (y >= height - radius ? y - (height - radius - 1) : 0);

                if (rx == 0 || ry == 0 || (rx * rx + ry * ry <= radius * radius))
                    pix[y * width + x] = col;
                else
                    pix[y * width + x] = transparent;
            }
        }
        tex.SetPixels(pix);
        tex.Apply();
        return tex;
    }
}
