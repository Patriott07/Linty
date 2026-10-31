using UnityEditor;
using UnityEngine;

public class ProjectSetupLintyPopup : EditorWindow
{
    private const float WindowWidth = 480f;
    private const float WindowHeight = 365f;
    private const string ProjectNameKey = "SmartLinty_ProjectName";
    private const string GenreKey = "SmartLinty_Genre";
    private const string ProjectTypeKey = "SmartLinty_ProjectType";

    private string projectName = "";
    private int selectedGenreIndex;
    private int selectedProjectTypeIndex;

    private readonly string[] genreOptions = FolderMakerDatabase.Genres;
    private readonly string[] projectTypeOptions = FolderMakerDatabase.ProjectTypes;

    private Texture2D limeTexture;
    private Texture2D darkTexture;
    private Texture2D inputTexture;

    [MenuItem("Tools/Project Setup Window")]
    public static void OpenWindow()
    {
        ProjectSetupLintyPopup window = GetWindow<ProjectSetupLintyPopup>(
            true,
            "Project Setup - Listy Assistant",
            true
        );

        window.minSize = new Vector2(WindowWidth, WindowHeight);
        window.maxSize = new Vector2(WindowWidth, WindowHeight);
        window.position = CenteredRect(WindowWidth, WindowHeight);
        window.ShowUtility();
    }

    private void OnEnable()
    {
        projectName = EditorPrefs.GetString(ProjectNameKey, "");
        selectedGenreIndex = IndexOf(genreOptions, EditorPrefs.GetString(GenreKey, ""));
        selectedProjectTypeIndex = IndexOf(projectTypeOptions, EditorPrefs.GetString(ProjectTypeKey, ""));

        limeTexture = MakeTexture(new Color(0.38f, 0.62f, 0.20f));
        darkTexture = MakeTexture(new Color(0.12f, 0.12f, 0.12f));
        inputTexture = MakeTexture(new Color(0.09f, 0.09f, 0.09f));
    }

    private void OnGUI()
    {
        EditorGUI.DrawRect(
            new Rect(0, 0, position.width, position.height),
            new Color(0.18f, 0.18f, 0.18f)
        );

        GUIStyle titleStyle = MakeLabel(22, FontStyle.Bold, new Color(0.87f, 0.87f, 0.87f));
        GUIStyle instructionStyle = MakeLabel(12, FontStyle.Bold, new Color(0.95f, 0.47f, 0.16f));
        instructionStyle.wordWrap = true;
        GUIStyle fieldLabelStyle = MakeLabel(11, FontStyle.Bold, new Color(0.68f, 0.68f, 0.68f));
        GUIStyle inputStyle = new GUIStyle(EditorStyles.textField)
        {
            fontSize = 12,
            fixedHeight = 36,
            padding = new RectOffset(10, 10, 8, 8),
            normal = { background = inputTexture, textColor = Color.white },
            focused = { background = inputTexture, textColor = Color.white }
        };
        GUIStyle popupStyle = new GUIStyle(EditorStyles.popup)
        {
            fontSize = 12,
            fixedHeight = 36,
            padding = new RectOffset(10, 10, 0, 0),
            normal = { background = inputTexture, textColor = new Color(0.72f, 0.72f, 0.72f) },
            focused = { background = inputTexture, textColor = Color.white }
        };
        GUIStyle previousStyle = MakeButtonStyle(darkTexture, 13);
        GUIStyle nextStyle = MakeButtonStyle(limeTexture, 13);

        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        GUILayout.Space(25);
        GUILayout.BeginVertical();

        GUILayout.Label("Whats you build?", titleStyle);
        GUILayout.Space(8);
        GUILayout.Label(
            "Please Fill Correctly For Better Inspection Suggestion System",
            instructionStyle,
            GUILayout.Width(430),
            GUILayout.Height(32)
        );

        GUILayout.Space(11);
        GUILayout.Label("Name Project", fieldLabelStyle);
        projectName = EditorGUILayout.TextField(projectName, inputStyle, GUILayout.Height(36));

        GUILayout.Space(10);
        GUILayout.Label("Genre Game", fieldLabelStyle);
        selectedGenreIndex = EditorGUILayout.Popup(
            selectedGenreIndex,
            genreOptions,
            popupStyle,
            GUILayout.Height(36)
        );

        GUILayout.Space(10);
        GUILayout.Label("Project Type (2d/3d)", fieldLabelStyle);
        selectedProjectTypeIndex = EditorGUILayout.Popup(
            selectedProjectTypeIndex,
            projectTypeOptions,
            popupStyle,
            GUILayout.Height(36)
        );

        GUILayout.Space(15);
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("← Previous", previousStyle, GUILayout.Width(115), GUILayout.Height(36)))
        {
            SaveDraft();
            WelcomeLintyPopup.OpenWindow();
            Close();
        }

        GUILayout.Space(5);

        if (GUILayout.Button("Next →", nextStyle, GUILayout.Width(85), GUILayout.Height(36)))
        {
            if (!ValidateInput())
                return;

            SaveDraft();
            FolderStructureLintyPopup.OpenWindow();
            Close();
        }

        GUILayout.FlexibleSpace();
        DrawStepperDots(1, 3);
        GUILayout.Space(20);
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
        GUILayout.Space(25);
        GUILayout.EndHorizontal();
    }

    private bool ValidateInput()
    {
        if (string.IsNullOrWhiteSpace(projectName))
        {
            EditorUtility.DisplayDialog("Project Setup", "Nama project wajib diisi terlebih dahulu.", "OK");
            return false;
        }

        if (selectedGenreIndex == 0 || selectedProjectTypeIndex == 0)
        {
            EditorUtility.DisplayDialog(
                "Project Setup",
                "Pilih Genre Game dan Project Type terlebih dahulu.",
                "OK"
            );
            return false;
        }

        return true;
    }

    private void SaveDraft()
    {
        EditorPrefs.SetString(ProjectNameKey, projectName.Trim());
        EditorPrefs.SetString(GenreKey, genreOptions[selectedGenreIndex]);
        EditorPrefs.SetString(ProjectTypeKey, projectTypeOptions[selectedProjectTypeIndex]);
    }

    private void DrawStepperDots(int activeIndex, int totalDots)
    {
        GUILayout.BeginHorizontal();
        for (int i = 0; i < totalDots; i++)
        {
            Rect dotRect = GUILayoutUtility.GetRect(12, 12, GUILayout.Width(12), GUILayout.Height(12));
            EditorGUI.DrawRect(
                new Rect(dotRect.x + 3, dotRect.y + 3, 6, 6),
                i == activeIndex
                    ? new Color(0.70f, 0.95f, 0.30f)
                    : new Color(0.45f, 0.45f, 0.45f)
            );
            GUILayout.Space(3);
        }
        GUILayout.EndHorizontal();
    }

    private static int IndexOf(string[] options, string value)
    {
        for (int i = 0; i < options.Length; i++)
        {
            if (options[i] == value)
                return i;
        }

        return 0;
    }

    private static Rect CenteredRect(float width, float height)
    {
        return new Rect(
            (Screen.currentResolution.width - width) * 0.5f,
            (Screen.currentResolution.height - height) * 0.5f,
            width,
            height
        );
    }

    private static GUIStyle MakeLabel(int size, FontStyle fontStyle, Color color)
    {
        return new GUIStyle(EditorStyles.label)
        {
            fontSize = size,
            fontStyle = fontStyle,
            normal = { textColor = color }
        };
    }

    private static GUIStyle MakeButtonStyle(Texture2D background, int size)
    {
        return new GUIStyle(GUI.skin.button)
        {
            fontSize = size,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter,
            normal = { background = background, textColor = Color.white },
            hover = { background = background, textColor = Color.white },
            active = { background = background, textColor = Color.white }
        };
    }

    private static Texture2D MakeTexture(Color color)
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, color);
        texture.Apply();
        return texture;
    }
}
