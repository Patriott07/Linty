using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class FolderStructureLintyPopup : EditorWindow
{
    private const float WindowWidth = 500f;
    private const float ConfirmationHeight = 390;
    private const float ResultHeight = 380;
    private const float FolderListMaxHeight = 170f;

    private int selectedGenreIndex;
    private int selectedProjectTypeIndex;
    private int expandedFolderIndex;
    private Vector2 confirmationScrollPosition;
    private Vector2 folderListScrollPosition;
    private Vector2 resultWindowScrollPosition;
    private Vector2 resultScrollPosition;
    private bool showResult;
    private List<string> availableFolders = new List<string>();
    private Dictionary<string, bool> folderSelections = new Dictionary<string, bool>();
    private List<string> createdFolders = new List<string>();

    private readonly string[] genreOptions = FolderMakerDatabase.Genres;
    private readonly string[] projectTypeOptions = FolderMakerDatabase.ProjectTypes;

    private Texture2D limeTexture;
    private Texture2D darkTexture;

    public static void OpenWindow()
    {
        FolderStructureLintyPopup window = GetWindow<FolderStructureLintyPopup>(
            true,
            "Folder Setup - Listy Assistant",
            true
        );

        window.minSize = new Vector2(WindowWidth, ConfirmationHeight);
        window.maxSize = new Vector2(WindowWidth, ResultHeight);
        window.position = CenteredRect(WindowWidth, ConfirmationHeight);
        window.ShowUtility();
    }

    private void OnEnable()
    {
        limeTexture = MakeTexture(new Color(0.38f, 0.62f, 0.20f));
        darkTexture = MakeTexture(new Color(0.12f, 0.12f, 0.12f));

        selectedGenreIndex = IndexOf(
            genreOptions,
            EditorPrefs.GetString("SmartLinty_Genre", "")
        );
        selectedProjectTypeIndex = IndexOf(
            projectTypeOptions,
            EditorPrefs.GetString("SmartLinty_ProjectType", "")
        );

        expandedFolderIndex = 0;
        showResult = false;
        createdFolders.Clear();
        RefreshAvailableFolders();
    }

    private void OnGUI()
    {
        DrawBackground();

        if (showResult)
            DrawResult();
        else
            DrawConfirmation();
    }

    private void DrawConfirmation()
    {
        GUIStyle titleStyle = MakeLabel(20, FontStyle.Bold, new Color(0.87f, 0.87f, 0.87f));
        GUIStyle instructionStyle = MakeLabel(12, FontStyle.Bold, new Color(0.95f, 0.47f, 0.16f));
        instructionStyle.wordWrap = true;
        GUIStyle mutedStyle = MakeLabel(11, FontStyle.Normal, new Color(0.68f, 0.68f, 0.68f));
        GUIStyle toggleStyle = new GUIStyle(EditorStyles.toggle)
        {
            fontSize = 12,
            normal = { textColor = new Color(0.86f, 0.86f, 0.86f) },
            onNormal = { textColor = Color.white }
        };
        GUIStyle makeStyle = MakeButtonStyle(limeTexture, 13);
        GUIStyle skipStyle = MakeLabel(13, FontStyle.Normal, new Color(0.68f, 0.68f, 0.68f));
        skipStyle.alignment = TextAnchor.MiddleRight;

        confirmationScrollPosition = GUILayout.BeginScrollView(
            confirmationScrollPosition,
            false,
            true,
            GUILayout.Width(position.width),
            GUILayout.Height(position.height)
        );

        GUILayout.BeginHorizontal();
        GUILayout.Space(25);
        GUILayout.BeginVertical(GUILayout.Width(position.width - 50));

        GUILayout.Space(18);
        GUILayout.Label("Thanks for filling project detail", titleStyle);
        GUILayout.Space(7);
        GUILayout.Label(
            "Now I suggest you to making structure standart folder",
            instructionStyle,
            GUILayout.Width(position.width - 60),
            GUILayout.Height(20)
        );

        GUILayout.Space(10);
        GUILayout.Label(
            $"Preset folder: {genreOptions[selectedGenreIndex]} + {projectTypeOptions[selectedProjectTypeIndex]}",
            mutedStyle
        );

        GUILayout.Space(7);
        GUILayout.Label("Folder yang akan dibuat", mutedStyle);
        GUILayout.Space(3);
        DrawFolderSelectionList(toggleStyle);
        GUILayout.Space(20);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Make it", makeStyle, GUILayout.Width(100), GUILayout.Height(35)))
        {
            if (!ValidateSelection())
                return;

            SaveSelection();
            if (!CreateSelectedFolders())
                return;

            showResult = true;
            minSize = new Vector2(WindowWidth, ResultHeight);
            maxSize = new Vector2(WindowWidth, ResultHeight);
            position = CenteredRect(WindowWidth, ResultHeight);
            Repaint();
        }

        GUILayout.Space(9);
        Rect skipRect = GUILayoutUtility.GetRect(130, 35, GUILayout.Width(130), GUILayout.Height(35));
        if (GUI.Button(new Rect(skipRect.x, skipRect.y + 6, 130, 24), "Tidak Sekarang", skipStyle))
            Close();

        GUILayout.EndHorizontal();
        GUILayout.Space(25);
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();
        GUILayout.EndScrollView();
    }

    private void DrawFolderSelectionList(GUIStyle toggleStyle)
    {
        GUIStyle pathStyle = MakeLabel(11, FontStyle.Normal, new Color(0.84f, 0.84f, 0.84f));
        pathStyle.alignment = TextAnchor.MiddleLeft;

        folderListScrollPosition = GUILayout.BeginScrollView(
            folderListScrollPosition,
            false,
            true,
            GUILayout.Width(position.width - 58),
            GUILayout.Height(FolderListMaxHeight)
        );

        foreach (string folder in availableFolders)
        {
            Rect rowRect = GUILayoutUtility.GetRect(
                position.width - 58,
                25,
                GUILayout.Height(25),
                GUILayout.ExpandWidth(true)
            );
            EditorGUI.DrawRect(rowRect, new Color(0.14f, 0.14f, 0.14f));

            Rect toggleRect = new Rect(rowRect.x + 5, rowRect.y + 3, 18, 19);
            folderSelections[folder] = GUI.Toggle(
                toggleRect,
                folderSelections[folder],
                GUIContent.none,
                toggleStyle
            );

            GUIContent folderIcon = EditorGUIUtility.IconContent("Folder Icon");
            if (folderIcon.image != null)
            {
                GUI.DrawTexture(
                    new Rect(rowRect.x + 29, rowRect.y + 4, 18, 17),
                    folderIcon.image,
                    ScaleMode.ScaleToFit
                );
            }

            GUI.Label(
                new Rect(rowRect.x + 54, rowRect.y + 2, rowRect.width - 60, 21),
                folder,
                pathStyle
            );
        }

        GUILayout.EndScrollView();
    }

    private void RefreshAvailableFolders()
    {
        availableFolders = new List<string>();
        folderSelections.Clear();

        if (selectedGenreIndex == 0 || selectedProjectTypeIndex == 0)
            return;

        availableFolders = FolderMakerDatabase.GetAllFolders(
            genreOptions[selectedGenreIndex],
            projectTypeOptions[selectedProjectTypeIndex],
            true,
            true,
            true,
            true,
            true,
            true
        );

        foreach (string folder in availableFolders)
            folderSelections[folder] = true;
    }

    private void DrawResult()
    {
        GUIStyle titleStyle = MakeLabel(22, FontStyle.Bold, new Color(0.87f, 0.87f, 0.87f));
        GUIStyle countStyle = MakeLabel(12, FontStyle.Bold, new Color(0.68f, 0.68f, 0.68f));
        GUIStyle itemStyle = MakeLabel(13, FontStyle.Normal, new Color(0.90f, 0.90f, 0.90f));
        GUIStyle footerStyle = MakeLabel(12, FontStyle.Normal, new Color(0.60f, 0.60f, 0.60f));
        GUIStyle nextStyle = MakeButtonStyle(limeTexture, 13);

        resultWindowScrollPosition = GUILayout.BeginScrollView(
            resultWindowScrollPosition,
            false,
            true,
            GUILayout.Width(position.width),
            GUILayout.Height(position.height)
        );

        GUILayout.BeginHorizontal();
        GUILayout.Space(25);
        GUILayout.BeginVertical(GUILayout.Width(position.width - 50));

        GUILayout.Space(18);
        GUILayout.Label("Done", titleStyle);
        GUILayout.Space(8);
        GUILayout.Label(
            $"{createdFolders.Count} Folder Siap Digunakan, kamu bisa baca kegunaan tiap\nfolder dibawah",
            countStyle,
            GUILayout.Height(32)
        );

        resultScrollPosition = GUILayout.BeginScrollView(
            resultScrollPosition,
            false,
            true,
            GUILayout.Height(FolderListMaxHeight),
            GUILayout.Width(position.width - 58)
        );

        for (int i = 0; i < createdFolders.Count; i++)
        {
            Rect rowRect = GUILayoutUtility.GetRect(
                position.width - 83,
                30,
                GUILayout.Height(30),
                GUILayout.ExpandWidth(true)
            );
            DrawFolderRow(rowRect, createdFolders[i], i, itemStyle);

            if (expandedFolderIndex == i)
            {
                Rect detailRect = GUILayoutUtility.GetRect(
                    position.width - 103,
                    72,
                    GUILayout.Height(72),
                    GUILayout.ExpandWidth(true)
                );
                EditorGUI.DrawRect(detailRect, new Color(0.12f, 0.12f, 0.12f));
                GUI.Label(
                    new Rect(detailRect.x + 10, detailRect.y + 8, detailRect.width - 20, detailRect.height - 16),
                    FolderDescription(createdFolders[i]),
                    MakeLabel(11, FontStyle.Normal, new Color(0.68f, 0.68f, 0.68f))
                );
            }
        }

        GUILayout.EndScrollView();
        GUILayout.Space(7);
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Next →", nextStyle, GUILayout.Width(90), GUILayout.Height(35)))
            Close();

        GUILayout.FlexibleSpace();
        GUILayout.Label("You also can read it later", footerStyle);
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
        GUILayout.Space(25);
        GUILayout.EndHorizontal();
        GUILayout.EndScrollView();
    }

    private void DrawFolderRow(Rect rowRect, string path, int index, GUIStyle itemStyle)
    {
        EditorGUI.DrawRect(rowRect, new Color(0.18f, 0.18f, 0.18f));

        GUIContent folderIcon = EditorGUIUtility.IconContent("Folder Icon");
        if (folderIcon.image != null)
            GUI.DrawTexture(new Rect(rowRect.x + 5, rowRect.y + 7, 20, 16), folderIcon.image, ScaleMode.ScaleToFit);

        GUI.Label(
            new Rect(rowRect.x + 34, rowRect.y + 4, rowRect.width - 75, 23),
            FolderDisplayName(path),
            itemStyle
        );

        GUIStyle arrowStyle = MakeLabel(20, FontStyle.Bold, new Color(0.72f, 0.72f, 0.72f));
        arrowStyle.alignment = TextAnchor.MiddleCenter;
        if (GUI.Button(
            new Rect(rowRect.x + rowRect.width - 34, rowRect.y + 1, 30, 28),
            expandedFolderIndex == index ? "⌃" : "⌄",
            arrowStyle
        ))
        {
            expandedFolderIndex = expandedFolderIndex == index ? -1 : index;
            Repaint();
        }
    }

    private bool CreateSelectedFolders()
    {
        List<string> folders = new List<string>();
        foreach (string folder in availableFolders)
        {
            if (folderSelections.TryGetValue(folder, out bool selected) && selected)
                folders.Add(folder);
        }

        if (folders.Count == 0)
        {
            EditorUtility.DisplayDialog(
                "Folder Setup",
                "Pilih minimal satu folder yang ingin dibuat.",
                "OK"
            );
            return false;
        }

        createdFolders = new List<string>(folders);
        foreach (string folder in folders)
            CreateFolderRecursive(folder);

        EditorPrefs.SetString("SmartLinty_CreatedFolders", string.Join("\n", folders));
        EditorPrefs.SetString(
            "SmartLinty_LastFolderPreset",
            $"{genreOptions[selectedGenreIndex]} + {projectTypeOptions[selectedProjectTypeIndex]}"
        );
        EditorPrefs.SetString("SmartLinty_FolderCreatedAt", System.DateTime.Now.ToString("g"));

        AssetDatabase.Refresh();
        Debug.Log(
            $"Smart Linty: {folders.Count} folders created for {genreOptions[selectedGenreIndex]} + {projectTypeOptions[selectedProjectTypeIndex]}."
        );
        return true;
    }

    private bool ValidateSelection()
    {
        if (selectedGenreIndex == 0 || selectedProjectTypeIndex == 0)
        {
            EditorUtility.DisplayDialog(
                "Folder Setup",
                "Pilih Genre Game dan Project Type terlebih dahulu.",
                "OK"
            );
            return false;
        }

        return true;
    }

    private void SaveSelection()
    {
        EditorPrefs.SetString("SmartLinty_Genre", genreOptions[selectedGenreIndex]);
        EditorPrefs.SetString("SmartLinty_ProjectType", projectTypeOptions[selectedProjectTypeIndex]);
    }

    private void CreateFolderRecursive(string path)
    {
        if (AssetDatabase.IsValidFolder(path))
            return;

        string parent = Path.GetDirectoryName(path).Replace("\\", "/");
        string folderName = Path.GetFileName(path);

        if (string.IsNullOrEmpty(parent) || string.IsNullOrEmpty(folderName))
            return;

        if (!AssetDatabase.IsValidFolder(parent))
            CreateFolderRecursive(parent);

        AssetDatabase.CreateFolder(parent, folderName);
    }

    private static string FolderDisplayName(string path)
    {
        string[] parts = path.Split('/');
        return parts[parts.Length - 1];
    }

    private static string FolderDescription(string path)
    {
        if (path.Contains("/Scripts"))
            return "Folder untuk script dan logic yang berkaitan dengan sistem project.";
        if (path.Contains("/Prefabs"))
            return "Folder untuk menyimpan prefab yang dapat digunakan kembali.";
        if (path.Contains("/Scenes") || path.Contains("/Levels"))
            return "Folder untuk scene, level, map, atau potongan area permainan.";
        if (path.Contains("/Audio"))
            return "Folder untuk asset suara, musik, voice, dan ambience.";
        if (path.Contains("/UI"))
            return "Folder untuk interface, HUD, menu, dan elemen tampilan.";
        if (path.Contains("/Models") || path.Contains("/Sprites") || path.Contains("/Textures"))
            return "Folder untuk asset visual sesuai project type yang dipilih.";
        if (path.Contains("/Data"))
            return "Folder untuk data konfigurasi dan data gameplay.";
        return "Folder bagian dari struktur standar project berdasarkan preset yang dipilih.";
    }

    private void DrawBackground()
    {
        EditorGUI.DrawRect(
            new Rect(0, 0, position.width, position.height),
            new Color(0.18f, 0.18f, 0.18f)
        );
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
