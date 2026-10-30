using UnityEditor;
using UnityEngine;

public class ProjectSetupLintyPopup : EditorWindow
{
    private string projectName = "";
    private int selectedGenreIndex = 0;
    private int selectedProjectTypeIndex = 0;

    private readonly string[] genreOptions = new string[]
    {
        "Select Option here..",
        "Platformer",
        "TopDown / RPG",
        "Endless Runner",
        "Puzzle",
        "UI Narrative / Visual Novel",
    };

    private readonly string[] projectTypeOptions = new string[]
    {
        "Select Option here..",
        "2D",
        "3D",
        "2D Physics in 3D Space",
    };

    private static Texture2D limeTex;
    private static Texture2D darkLimeTex;
    private static Texture2D darkInputTex;

    Color GenerateColorByHexa255(float r, float g, float b)
    {
        return new Color(r / 255, g / 255, b / 255);
    }

    [MenuItem("Tools/Project Setup Window")]
    public static void OpenWindow()
    {
        ProjectSetupLintyPopup window = GetWindow<ProjectSetupLintyPopup>(
            true,
            "Project Setup - Listy Asistant",
            true
        );

        float width = 480;
        float height = 365;

        float x = (Screen.currentResolution.width - width) * 0.5f;
        float y = (Screen.currentResolution.height - height) * 0.5f;

        window.position = new Rect(x, y, width, height);
        window.minSize = new Vector2(width, height);
        window.maxSize = new Vector2(width, height);
        window.ShowUtility(); // Mengunci ukuran & menghilangkan maximize
    }

    private void OnEnable()
    {
        // Tekstur warna kustom
        limeTex = MakeTex(1, 1, new Color(0.38f, 0.62f, 0.2f)); // Hijau Lime Next
        darkLimeTex = MakeTex(1, 1, new Color(0.28f, 0.38f, 0.18f)); // Hijau Gelap Previous
        darkInputTex = MakeTex(1, 1, GenerateColorByHexa255(35, 35, 35)); // Gelap Input Box
    }

    
    private void OnGUI()
    {
        // Background utama gelap
        EditorGUI.DrawRect(
            new Rect(0, 0, position.width, position.height),
            new Color(0.18f, 0.18f, 0.18f)
        );

        float margin = 25f;
        // Lebar konten otomatis menyesuaikan lebar window dikurangi margin kiri & kanan
        float availableWidth = position.width - (margin * 2);

        // --- 1. MASTER CONTAINER (Analoginya: Wrapper/Div utama dengan Padding/Margin) ---
        // GUILayout.BeginHorizontal();
        // GUILayout.Space(25); // Margin Kiri (Gap dari pinggir kiri window)

        // GUILayout.BeginVertical();
        // GUILayout.Space(20); // Margin Atas (Gap dari pinggir atas window)

        GUILayout.Space(25);

        // Header Title & Subtitle
        GUILayout.BeginHorizontal();
        GUILayout.Space(25);
        GUILayout.BeginVertical();

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

        GUILayout.Label("Whats you build?", titleStyle);
        GUILayout.Space(4);
        GUILayout.Label(
            "Please Fill Correctly For Better Inspection Suggestion System",
            subStyle,
            GUILayout.Width(availableWidth)
        );

        GUILayout.Space(20);

        // Style Label Form
        GUIStyle formLabelStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 11,
            normal = { textColor = new Color(0.65f, 0.65f, 0.65f) },
        };

        // Style Input Field Kustom (Mirip Referensi Gambar)
        GUIStyle customInputStyle = new GUIStyle(EditorStyles.textField)
        {
            fontSize = 12,
            fixedHeight = 32,
            alignment = TextAnchor.MiddleLeft,
            padding = new RectOffset(10, 10, 8, 8),
            normal =
            {
                background = darkInputTex,
                textColor = string.IsNullOrEmpty(projectName)
                    ? new Color(0.35f, 0.35f, 0.35f)
                    : GenerateColorByHexa255(138, 138, 138),
            },
        };

        // Style Dropdown Popup Kustom
        GUIStyle customPopupStyle = new GUIStyle(EditorStyles.popup)
        {
            fontSize = 12,
            fixedHeight = 32,
            alignment = TextAnchor.MiddleLeft,
            padding = new RectOffset(10, 25, 0, 0),
            normal = { background = darkInputTex, textColor = new Color(0.75f, 0.75f, 0.75f) },
        };

        // 1. Name Project Input
        GUILayout.Label("Name Project", formLabelStyle);
        // GUILayout.Space(4);

        // Handling Placeholder Text
        // string displayProjectName = string.IsNullOrEmpty(projectName) ? "" : projectName;
        string newName = EditorGUILayout.TextField(
            // displayProjectName,
            null,
            customInputStyle,
            GUILayout.Width(availableWidth)
        );

        GUILayout.Space(12 * 2);

        // 2. Genre Game Dropdown
        GUILayout.Label("Genre Game", formLabelStyle);
        GUILayout.Space(4);
        selectedGenreIndex = EditorGUILayout.Popup(
            selectedGenreIndex,
            genreOptions,
            customPopupStyle,
            GUILayout.Width(availableWidth),
            GUILayout.Height(32)
        );

        GUILayout.Space(12);

        // 3. Project Type Dropdown
        GUILayout.Label("Project Type (2d/3d)", formLabelStyle);
        GUILayout.Space(4);
        selectedProjectTypeIndex = EditorGUILayout.Popup(
            selectedProjectTypeIndex,
            projectTypeOptions,
            customPopupStyle,
            GUILayout.Width(availableWidth),
            GUILayout.Height(32)
        );

        GUILayout.Space(15);

        // Baris Bawah: Tombol Navigasi & Dots Stepper
        GUILayout.BeginHorizontal();

        GUIStyle prevButtonStyle = new GUIStyle(GUI.skin.button)
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = 13,
            fontStyle = FontStyle.Bold,
            normal = { background = darkLimeTex, textColor = Color.white },
        };

        GUIStyle nextButtonStyle = new GUIStyle(GUI.skin.button)
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = 13,
            fontStyle = FontStyle.Bold,
            normal = { background = limeTex, textColor = Color.white },
        };

        if (
            GUILayout.Button(
                "← Previous",
                prevButtonStyle,
                GUILayout.Width(115),
                GUILayout.Height(36)
            )
        )
        {
            WelcomeLintyPopup.OpenWindow();
            Close();
        }

        GUILayout.Space(8);

        if (GUILayout.Button("Next →", nextButtonStyle, GUILayout.Width(85), GUILayout.Height(36)))
        {
            EditorPrefs.SetString("SmartLinty_ProjectName", projectName);
            EditorPrefs.SetString("SmartLinty_Genre", genreOptions[selectedGenreIndex]);
            EditorPrefs.SetString(
                "SmartLinty_ProjectType",
                projectTypeOptions[selectedProjectTypeIndex]
            );

            Debug.Log(
                $"Saved Setup: {projectName} | {genreOptions[selectedGenreIndex]} | {projectTypeOptions[selectedProjectTypeIndex]}"
            );

            //Open next
            FolderStructureLintyPopup.OpenWindow();
            Close();
        }

        GUILayout.FlexibleSpace();

        // Indikator Stepper Dots (Dot pertama aktif warna lime)
        DrawStepperDots(0, 2);

        GUILayout.Space(20);
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }

    private void DrawStepperDots(int activeIndex, int totalDots)
    {
        GUILayout.BeginHorizontal();
        for (int i = 0; i < totalDots; i++)
        {
            Color dotColor =
                (i == activeIndex) ? new Color(0.7f, 0.95f, 0.3f) : new Color(0.45f, 0.45f, 0.45f);
            Rect dotRect = GUILayoutGetRect(12, 12);
            Handles.BeginGUI();
            Handles.color = dotColor;
            Handles.DrawSolidDisc(
                new Vector3(dotRect.x + 6, dotRect.y + 12, 0),
                Vector3.forward,
                5
            );
            Handles.EndGUI();
            GUILayout.Space(4);
        }
        GUILayout.EndHorizontal();
    }

    private Rect GUILayoutGetRect(float width, float height)
    {
        return GUILayoutUtility.GetRect(
            width,
            height,
            GUILayout.Width(width),
            GUILayout.Height(height)
        );
    }

    private Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; ++i)
            pix[i] = col;
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }
}
