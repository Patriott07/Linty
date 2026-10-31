using UnityEditor;
using UnityEngine;

public class WelcomeLintyPopup : EditorWindow
{
    private const float WindowWidth = 480f;
    private const float WindowHeight = 420f;
    private static Texture2D mascotTexture;
    private static Texture2D buttonTexture;

    [InitializeOnLoadMethod]
    private static void ShowOnFirstLoad()
    {
        if (!EditorPrefs.GetBool("SmartLinty_Welcomed", false))
        {
            EditorPrefs.SetBool("SmartLinty_Welcomed", true);
            EditorApplication.delayCall += OpenWindow;
        }
    }

    [MenuItem("Tools/Welcome Window")]
    public static void OpenWindow()
    {
        WelcomeLintyPopup window = GetWindow<WelcomeLintyPopup>(
            true,
            "Welcome - Listy Assistant",
            true
        );

        window.minSize = new Vector2(WindowWidth, WindowHeight);
        window.maxSize = new Vector2(WindowWidth, WindowHeight);
        window.position = CenteredRect(WindowWidth, WindowHeight);
        window.ShowUtility();
    }

    private void OnEnable()
    {
        mascotTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(
            "Assets/SmartLinty/Editor/AiraMascot.png"
        );

        if (buttonTexture == null)
            buttonTexture = MakeTexture(new Color(0.38f, 0.62f, 0.20f));
    }

    private void OnGUI()
    {
        EditorGUI.DrawRect(
            new Rect(0, 0, position.width, position.height),
            new Color(0.18f, 0.18f, 0.18f)
        );

        GUIStyle titleStyle = MakeLabel(24, FontStyle.Bold, Color.white);
        GUIStyle subtitleStyle = MakeLabel(16, FontStyle.Bold, new Color(0.68f, 0.68f, 0.68f));
        GUIStyle descriptionStyle = MakeLabel(11, FontStyle.Normal, new Color(0.72f, 0.72f, 0.72f));
        descriptionStyle.wordWrap = true;
        GUIStyle orangeStyle = MakeLabel(12, FontStyle.Bold, new Color(0.95f, 0.47f, 0.16f));
        GUIStyle nextStyle = MakeButtonStyle(buttonTexture, 13);
        GUIStyle linkStyle = MakeLabel(12, FontStyle.Bold, new Color(0.45f, 0.68f, 0.90f));
        linkStyle.alignment = TextAnchor.MiddleRight;

        GUILayout.Space(28);
        GUILayout.BeginHorizontal();
        GUILayout.Space(20);

        if (mascotTexture != null)
            GUILayout.Label(mascotTexture, GUIStyle.none, GUILayout.Width(190), GUILayout.Height(190));
        else
            GUILayout.Box("AiraMascot.png", GUILayout.Width(190), GUILayout.Height(190));

        GUILayout.Space(18);
        GUILayout.BeginVertical();
        GUILayout.Space(66);
        GUILayout.Label("WELCOME!", titleStyle);
        GUILayout.Label("Listy Assistant", subtitleStyle);
        GUILayout.EndVertical();

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        GUILayout.Space(15);
        GUILayout.BeginHorizontal();
        GUILayout.Space(20);
        GUILayout.BeginVertical();

        GUILayout.Label(
            "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since 1966, when designers at Letraset and James",
            descriptionStyle,
            GUILayout.Width(430),
            GUILayout.Height(58)
        );

        GUILayout.Space(4);
        GUILayout.Label("What next? Help Our System know what you made", orangeStyle);

        GUILayout.Space(24);
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Next →", nextStyle, GUILayout.Width(100), GUILayout.Height(35)))
        {
            ProjectSetupLintyPopup.OpenWindow();
            Close();
        }

        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Open pdf file", linkStyle, GUILayout.Width(115), GUILayout.Height(28)))
        {
            string path = Application.dataPath + "/SmartLinty/Docs/LintyDocumentation.pdf";
            if (System.IO.File.Exists(path))
                EditorUtility.OpenWithDefaultApp(path);
            else
                EditorUtility.DisplayDialog("Documentation", "File PDF tidak ditemukan.", "OK");
        }

        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.Space(20);
        GUILayout.EndHorizontal();
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
