using UnityEditor;
using UnityEngine;

public class WelcomeLintyPopup : EditorWindow
{
    private static Texture2D mascotTexture;

    Color GenerateColorByHexa255(float r, float g, float b)
    {
        return new Color(r / 255, g / 255, b / 255);
    }

    [InitializeOnLoadMethod]
    static void ShowOnFirstLoad()
    {
        if (!SessionState.GetBool("SmartLinty_Welcomed", false))
        {
            SessionState.SetBool("SmartLinty_Welcomed", true);
            EditorApplication.delayCall += OpenWindow;
        }
    }

    [MenuItem("Tools/Welcome Window")]
    public static void OpenWindow()
    {
        WelcomeLintyPopup window = GetWindow<WelcomeLintyPopup>(
            true,
            "Welcome - Listy Asistant",
            false
        );

        float width = 480;
        float height = 420;

        // Posisi di tengah layar (Center)
        float x = (Screen.currentResolution.width - width) * 0.5f;
        float y = (Screen.currentResolution.height - height) * 0.5f;

        window.position = new Rect(x, y, width, height);
        window.minSize = new Vector2(width, height);
        window.maxSize = new Vector2(width, height);
        window.Show();
    }

    private void OnEnable()
    {
        // Sesuaikan path ini dengan letak file gambar maskot kamu di folder Assets
        mascotTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(
            "Assets/SmartLinty/Editor/AiraMascot.png"
        );
    }

    private void OnGUI()
    {
        // Background gelap ala tema UI
        EditorGUI.DrawRect(
            new Rect(0, 0, position.width, position.height),
            new Color(0.18f, 0.18f, 0.18f)
        );

        GUILayout.Space(20);

        // Baris Atas: Maskot di Kiri, Teks Welcome di Kanan
        GUILayout.BeginHorizontal();
        GUILayout.Space(15);

        // Menampilkan Gambar Maskot
        if (mascotTexture != null)
        {
            GUILayout.Label(mascotTexture, GUILayout.Width(210), GUILayout.Height(210));
        }
        else
        {
            // Placeholder jika gambar belum dimasukkan
            GUILayout.Box(
                "Masukin Gambar\nAiraMascot.png",
                GUILayout.Width(160),
                GUILayout.Height(160)
            );
        }

        GUILayout.Space(15);

        // Teks Judul
        GUILayout.BeginVertical();
        GUILayout.Space(80);
        GUIStyle titleStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 24,
            normal = { textColor = Color.white },
        };
        GUIStyle subStyle = new GUIStyle(EditorStyles.label)
        {
            fontSize = 16,
            normal = { textColor = new Color(0.7f, 0.7f, 0.7f) },
        };

        GUILayout.Label("WELCOME!", titleStyle);
        GUILayout.Label("Listy Asistant", subStyle);
        GUILayout.EndVertical();

        GUILayout.EndHorizontal();

        GUILayout.Space(15);

        // Bagian Deskripsi & Tombol Bawah
        GUILayout.BeginHorizontal();
        GUILayout.Space(20);
        GUILayout.BeginVertical();

        GUIStyle descStyle = new GUIStyle(EditorStyles.wordWrappedLabel)
        {
            fontSize = 11,
            normal = { textColor = new Color(0.75f, 0.75f, 0.75f) },
        };
        GUILayout.Label(
            "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since 1966, when designers at Letraset and James",
            descStyle,
            GUILayout.Width(430)
        );

        GUILayout.Space(10);

        GUIStyle nextStepStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 12,
            normal = { textColor = new Color(0.95f, 0.55f, 0.15f) },
        };
        GUILayout.Label("What next? Help Our System know what you made", nextStepStyle);

        GUILayout.Space(25);

        // Tombol Next & Link PDF
        GUILayout.BeginHorizontal();

        // 1. Buat tekstur warna hijau lime
        Texture2D limeTex = new Texture2D(1, 1);
        limeTex.SetPixel(0, 0, new Color(0.4f, 0.75f, 0.2f)); // Warna Lime
        limeTex.Apply();

        // 2. Buat style khusus tombol Next
        GUIStyle nextButtonStyle = new GUIStyle(GUI.skin.button)
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = 13,
            fontStyle = FontStyle.Bold,
            normal = { background = limeTex, textColor = Color.white },
            active = { background = limeTex, textColor = Color.gray },
        };

        // 3. Panggil button dengan style baru
        if (GUILayout.Button("Next ➔", nextButtonStyle, GUILayout.Width(100), GUILayout.Height(30)))
        {
            Debug.Log("Next clicked! Lanjut ke Popup II (Setup Project)");
            ProjectSetupLintyPopup.OpenWindow();
            Close();
        }

        GUILayout.FlexibleSpace();

        GUIStyle linkStyle = new GUIStyle(EditorStyles.label)
        {
            normal = { textColor = new Color(0.3f, 0.6f, 1f) },
        };

        if (GUILayout.Button("Open pdf file", linkStyle))
        {
            string path = Application.dataPath + "/SmartLinty/Docs/LintyDocumentation.pdf";
            EditorUtility.OpenWithDefaultApp(path);
        }

        GUILayout.Space(25);
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
        GUILayout.Space(20);
        GUILayout.EndHorizontal();
    }
}
