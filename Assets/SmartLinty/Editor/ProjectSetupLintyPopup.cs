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

    [MenuItem("Tools/Project Setup Window")]
    public static void OpenWindow()
    {
        ProjectSetupLintyPopup window = GetWindow<ProjectSetupLintyPopup>(
            true,
            "Project Setup - Listy Asistant",
            true
        );

        float width = 450;
        float height = 430;

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
        limeTex = MakeRoundedRectTex(78, 40, 10, new Color(0.38f, 0.62f, 0.2f)); // Hijau Lime Next
        darkLimeTex = MakeRoundedRectTex(1, 1, 10, new Color(0.28f, 0.38f, 0.18f)); // Hijau Gelap Previous
        darkInputTex = MakeRoundedRectTex(1, 1, 5, new Color(0.13f, 0.13f, 0.13f)); // Gelap Input Box
    }

    private void OnGUI()
    {
        // Background utama gelap
        EditorGUI.DrawRect(
            new Rect(0, 0, position.width, position.height),
            new Color(0.18f, 0.18f, 0.18f)
        );

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
            GUILayout.Width(400)
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
            padding = new RectOffset(10, 10, 0, 0),
            normal =
            {
                background = darkInputTex,
                textColor = string.IsNullOrEmpty(projectName)
                    ? new Color(0.45f, 0.45f, 0.45f)
                    : new Color(0.85f, 0.85f, 0.85f),
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
        GUILayout.Space(4);

        // Handling Placeholder Text
        string displayProjectName = string.IsNullOrEmpty(projectName)
            ? "Give a name.."
            : projectName;
        string newName = EditorGUILayout.TextField(
            displayProjectName,
            customInputStyle,
            GUILayout.Width(390)
        );
        if (newName != "Give a name..")
        {
            projectName = newName;
        }

        GUILayout.Space(12 * 2);

        // 2. Genre Game Dropdown
        GUILayout.Label("Genre Game", formLabelStyle);
        GUILayout.Space(4);
        selectedGenreIndex = EditorGUILayout.Popup(
            selectedGenreIndex,
            genreOptions,
            customPopupStyle,
            GUILayout.Width(390),
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
            GUILayout.Width(390),
            GUILayout.Height(32)
        );

        GUILayout.Space(30);

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
            Close();
        }

        GUILayout.FlexibleSpace();

        // Indikator Stepper Dots (Dot pertama aktif warna lime)
        DrawStepperDots(0, 4);

        GUILayout.Space(10);
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

    // Fungsi generator tekstur rounded corner
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
