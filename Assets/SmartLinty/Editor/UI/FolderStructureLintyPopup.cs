using System.Collections.Generic;
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
    private bool createGenreSpecific = true;

    private int selectedGenreIndex = 0;
    private int selectedProjectTypeIndex = 0;
    private Vector2 scrollPos;

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

    public static void OpenWindow()
    {
        FolderStructureLintyPopup window = GetWindow<FolderStructureLintyPopup>(
            true,
            "Project Setup - Listy Asistant",
            true
        );

        float width = 480;
        float height = 620;

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

        // Baca pilihan dari popup sebelumnya (ProjectSetupLintyPopup)
        string savedGenre = EditorPrefs.GetString("SmartLinty_Genre", "");
        string savedType = EditorPrefs.GetString("SmartLinty_ProjectType", "");
        selectedGenreIndex = IndexOf(genreOptions, savedGenre);
        selectedProjectTypeIndex = IndexOf(projectTypeOptions, savedType);
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
        GUILayout.Space(20);

        // Header Title & Subtitle
        GUIStyle titleStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 20,
            normal = { textColor = new Color(0.9f, 0.9f, 0.9f) },
        };
        GUIStyle subStyle = new GUIStyle(EditorStyles.label)
        {
            fontSize = 11,
            wordWrap = true,
            normal = { textColor = new Color(0.92f, 0.52f, 0.2f) },
        };
        GUIStyle infoStyle = new GUIStyle(EditorStyles.label)
        {
            fontSize = 11,
            wordWrap = true,
            normal = { textColor = new Color(0.65f, 0.65f, 0.65f) },
        };

        GUILayout.Label("Thanks for filling project detail", titleStyle);
        GUILayout.Space(4);
        GUILayout.Label(
            "Struktur folder di bawah otomatis disesuaikan dengan Genre x Project Type yang kamu pilih.",
            subStyle,
            GUILayout.Width(availableWidth)
        );

        GUILayout.Space(10);

        scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.Width(availableWidth + 10), GUILayout.Height(380));

        // --- Pilihan Genre & Type (bisa override di sini) ---
        selectedGenreIndex = EditorGUILayout.Popup("Genre Game", selectedGenreIndex, genreOptions);
        selectedProjectTypeIndex = EditorGUILayout.Popup("Project Type", selectedProjectTypeIndex, projectTypeOptions);

        GUILayout.Space(6);
        GUILayout.Label(
            $"Preset aktif: {genreOptions[selectedGenreIndex]} + {projectTypeOptions[selectedProjectTypeIndex]}",
            infoStyle
        );

        GUILayout.Space(10);

        // Style Checkbox Custom
        GUIStyle toggleStyle = new GUIStyle(EditorStyles.toggle)
        {
            fontSize = 12,
            fontStyle = FontStyle.Bold,
            normal = { textColor = new Color(0.85f, 0.85f, 0.85f) },
            onNormal = { textColor = Color.white },
        };

        // --- Base folders (standar umum) ---
        GUILayout.Label("Base Folders", EditorStyles.boldLabel);
        createAnimations = EditorGUILayout.Toggle("Animations Folder", createAnimations, toggleStyle);
        create2DSprite = EditorGUILayout.Toggle("2D Sprite Folder", create2DSprite, toggleStyle);
        createUI = EditorGUILayout.Toggle("UI Folder", createUI, toggleStyle);
        createAudio = EditorGUILayout.Toggle("Audio Folder", createAudio, toggleStyle);
        createScripts = EditorGUILayout.Toggle("Scripts Folder", createScripts, toggleStyle);
        createPostProcessing = EditorGUILayout.Toggle(
            "Post Processing Folder",
            createPostProcessing,
            toggleStyle
        );

        GUILayout.Space(8);

        // --- Genre-specific folders ---
        GUILayout.Label("Genre Specific Folders", EditorStyles.boldLabel);
        createGenreSpecific = EditorGUILayout.Toggle(
            "Buat struktur khusus genre",
            createGenreSpecific,
            toggleStyle
        );

        if (createGenreSpecific)
        {
            string[] preset = GetStructureFor(
                genreOptions[selectedGenreIndex],
                projectTypeOptions[selectedProjectTypeIndex]
            );
            foreach (string folder in preset)
            {
                GUILayout.Label("• " + folder, infoStyle);
            }
        }

        GUILayout.EndScrollView();
        GUILayout.Space(10);

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
            // Simpan kembali override genre/type agar konsisten antar popup
            EditorPrefs.SetString("SmartLinty_Genre", genreOptions[selectedGenreIndex]);
            EditorPrefs.SetString("SmartLinty_ProjectType", projectTypeOptions[selectedProjectTypeIndex]);
            CreateSelectedFolders();
            Close();
        }

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
        GUILayout.EndHorizontal();
    }

    // Fungsi otomatis eksekusi pembuatan folder ke Asset Database Unity
    private void CreateSelectedFolders()
    {
        List<string> folders = new List<string>();

        if (createAnimations) folders.Add("Assets/Animations");
        if (create2DSprite) folders.Add("Assets/Sprites");
        if (createUI)
        {
            folders.Add("Assets/UI/HUD");
            folders.Add("Assets/UI/Menus");
        }
        if (createAudio)
        {
            folders.Add("Assets/Audio/Music");
            folders.Add("Assets/Audio/SFX");
        }
        if (createScripts) folders.Add("Assets/Scripts");
        if (createPostProcessing) folders.Add("Assets/Post Processing");

        if (createGenreSpecific)
        {
            folders.AddRange(
                GetStructureFor(
                    genreOptions[selectedGenreIndex],
                    projectTypeOptions[selectedProjectTypeIndex]
                )
            );
        }

        foreach (string folder in folders)
        {
            CreateFolderRecursive(folder);
        }

        AssetDatabase.Refresh();
        Debug.Log(
            $"Smart Linty: {folders.Count} folders created for {genreOptions[selectedGenreIndex]} + {projectTypeOptions[selectedProjectTypeIndex]}!"
        );
    }

    private void CreateFolderRecursive(string path)
    {
        if (AssetDatabase.IsValidFolder(path)) return;

        string parent = Path.GetDirectoryName(path).Replace("\\", "/");
        string folderName = Path.GetFileName(path);

        if (string.IsNullOrEmpty(parent) || string.IsNullOrEmpty(folderName)) return;
        if (!AssetDatabase.IsValidFolder(parent)) CreateFolderRecursive(parent);
        AssetDatabase.CreateFolder(parent, folderName);
    }

    private string[] GetStructureFor(string genre, string projectType)
    {
        switch (genre)
        {
            case "Platformer":
                if (projectType == "3D") return GetPlatformer3D();
                if (projectType == "2D Physics in 3D Space") return GetPlatformerHybrid();
                return GetPlatformer2D();
            case "TopDown / RPG":
                return projectType == "3D" ? GetTopDownRPG3D() : GetTopDownRPG2D();
            case "Endless Runner":
                return projectType == "3D" ? GetEndlessRunner3D() : GetEndlessRunner2D();
            case "Puzzle":
                return projectType == "3D" ? GetPuzzle3D() : GetPuzzle2D();
            case "UI Narrative / Visual Novel":
                return GetVisualNovel();
            default:
                return GetDefaultExtra();
        }
    }

    // ---- Contoh 1: 2D Platformer (Tilemap + Sprites) ----
    private string[] GetPlatformer2D() => new string[]
    {
        "Assets/Sprites/Player",
        "Assets/Sprites/Enemies",
        "Assets/Sprites/Environment",
        "Assets/Sprites/Items",
        "Assets/Tilemaps/Textures",
        "Assets/Tilemaps/Palettes",
        "Assets/Tilemaps/Prefabs",
        "Assets/Prefabs/Player",
        "Assets/Prefabs/Enemies",
        "Assets/Prefabs/Level",
        "Assets/Levels/World1",
        "Assets/Scripts/Player",
        "Assets/Scripts/Enemies",
        "Assets/Scripts/Systems",
        "Assets/Post Processing/Profiles2D",
    };

    // ---- Contoh 2: 3D Platformer (Models + Materials) ----
    private string[] GetPlatformer3D() => new string[]
    {
        "Assets/Models/Character",
        "Assets/Models/Environment",
        "Assets/Models/Props",
        "Assets/Materials/Character",
        "Assets/Materials/Environment",
        "Assets/Textures/Environment",
        "Assets/Prefabs/Character",
        "Assets/Prefabs/Items",
        "Assets/Prefabs/LevelPieces",
        "Assets/Levels/Stage1",
        "Assets/Scripts/Character",
        "Assets/Scripts/Items",
        "Assets/Scripts/Systems",
        "Assets/Animations/Character",
        "Assets/Post Processing/Profiles3D",
    };

    private string[] GetPlatformerHybrid() => new string[]
    {
        "Assets/Sprites/Player",
        "Assets/Sprites/Environment",
        "Assets/Models/Environment",
        "Assets/Materials/Environment",
        "Assets/Tilemaps/Textures",
        "Assets/Prefabs/Player",
        "Assets/Prefabs/LevelPieces",
        "Assets/Levels/Stage1",
        "Assets/Scripts/Player",
        "Assets/Scripts/Systems",
    };

    private string[] GetTopDownRPG2D() => new string[]
    {
        "Assets/Sprites/Characters",
        "Assets/Sprites/Tilesets",
        "Assets/Sprites/Items",
        "Assets/Tilemaps/Textures",
        "Assets/Tilemaps/Prefabs",
        "Assets/Dialogue/Dialogues",
        "Assets/Dialogue/Portraits",
        "Assets/Inventory/Items",
        "Assets/Prefabs/NPC",
        "Assets/Prefabs/Enemies",
        "Assets/Levels/Dungeon1",
        "Assets/Scripts/Dialogue",
        "Assets/Scripts/Inventory",
        "Assets/Scripts/Quests",
    };

    private string[] GetTopDownRPG3D() => new string[]
    {
        "Assets/Models/Characters",
        "Assets/Models/Environment",
        "Assets/Models/Props",
        "Assets/Materials/Characters",
        "Assets/Materials/Environment",
        "Assets/Dialogue/Dialogues",
        "Assets/Quests/Data",
        "Assets/Prefabs/NPC",
        "Assets/Prefabs/Items",
        "Assets/Levels/Stage1",
        "Assets/Scripts/Dialogue",
        "Assets/Scripts/Quests",
        "Assets/Scripts/Inventory",
    };

    private string[] GetEndlessRunner2D() => new string[]
    {
        "Assets/Sprites/Player",
        "Assets/Sprites/Obstacles",
        "Assets/Sprites/Backgrounds",
        "Assets/Prefabs/Chunks",
        "Assets/Prefabs/Obstacles",
        "Assets/Levels/Chunks",
        "Assets/Scripts/Player",
        "Assets/Scripts/Spawner",
        "Assets/Scripts/Systems",
    };

    private string[] GetEndlessRunner3D() => new string[]
    {
        "Assets/Models/Player",
        "Assets/Models/Obstacles",
        "Assets/Materials/Environment",
        "Assets/Prefabs/Chunks",
        "Assets/Prefabs/Obstacles",
        "Assets/Levels/Chunks",
        "Assets/Scripts/Player",
        "Assets/Scripts/Spawner",
    };

    private string[] GetPuzzle2D() => new string[]
    {
        "Assets/Sprites/Pieces",
        "Assets/Sprites/Backgrounds",
        "Assets/Prefabs/Pieces",
        "Assets/Levels/Level1",
        "Assets/Scripts/Mechanics",
        "Assets/Scripts/Systems",
    };

    private string[] GetPuzzle3D() => new string[]
    {
        "Assets/Models/Pieces",
        "Assets/Materials/Pieces",
        "Assets/Prefabs/Pieces",
        "Assets/Levels/Level1",
        "Assets/Scripts/Mechanics",
    };

    private string[] GetVisualNovel() => new string[]
    {
        "Assets/Dialogue/Scripts",
        "Assets/Dialogue/Characters",
        "Assets/Dialogue/Backgrounds",
        "Assets/UI/Dialogue",
        "Assets/UI/Menus",
        "Assets/Audio/Voice",
        "Assets/Audio/Music",
        "Assets/Sprites/Backgrounds",
        "Assets/Scripts/Dialogue",
    };

    private string[] GetDefaultExtra() => new string[]
    {
        "Assets/Prefabs",
        "Assets/Levels",
    };

    private int IndexOf(string[] arr, string value)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] == value) return i;
        }
        return 0;
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
