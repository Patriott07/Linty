using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public sealed class FileRelocatorTabRenderer
{
    private sealed class MisplacedFileItem
    {
        public string currentPath;
        public string suggestedPath;
        public string fileName;
    }

    private readonly Dictionary<string, string> extensionRules =
        new Dictionary<string, string>()
        {
            { ".cs", "Assets/Scripts/" },
            { ".png", "Assets/Textures/" },
            { ".jpg", "Assets/Textures/" },
            { ".jpeg", "Assets/Textures/" },
            { ".tga", "Assets/Textures/" },
            { ".prefab", "Assets/Prefabs/" },
            { ".mat", "Assets/Materials/" },
            { ".wav", "Assets/Audio/" },
            { ".mp3", "Assets/Audio/" },
            { ".ogg", "Assets/Audio/" },
            { ".anim", "Assets/Animations/" },
            { ".controller", "Assets/Animations/" }
        };

    private readonly List<MisplacedFileItem> misplacedFiles =
        new List<MisplacedFileItem>();
    private Vector2 contentScrollPosition;
    private Vector2 fileListScrollPosition;
    private bool scanned;
    private int totalFilesScanned;
    private float availableHeight;

    public void Draw(Rect windowRect, Texture2D normalTexture, Texture2D hoverTexture, Texture2D activeTexture)
    {
        availableHeight = windowRect.height;
        GUIStyle titleStyle = MakeLabel(18, FontStyle.Bold, new Color(0.87f, 0.87f, 0.87f));
        GUIStyle orangeStyle = MakeLabel(11, FontStyle.Bold, new Color(0.95f, 0.47f, 0.16f));
        orangeStyle.wordWrap = true;
        GUIStyle mutedStyle = MakeLabel(11, FontStyle.Normal, new Color(0.68f, 0.68f, 0.68f));
        mutedStyle.wordWrap = true;
        GUIStyle actionStyle = MakeButtonStyle(normalTexture, hoverTexture, activeTexture);

        contentScrollPosition = EditorGUILayout.BeginScrollView(contentScrollPosition);
        GUILayout.Space(8);
        GUILayout.BeginHorizontal();
        GUILayout.Space(12);
        GUILayout.BeginVertical(GUILayout.Width(Mathf.Max(220, windowRect.width - 36)));

        GUILayout.Label("File Relocator", titleStyle);
        GUILayout.Space(5);
        GUILayout.Label(
            "Scan dan pindahkan file yang berada di lokasi yang tidak sesuai berdasarkan aturan folder project.",
            orangeStyle
        );
        GUILayout.Space(10);

        if (GUILayout.Button("Scan Misplaced Project Files", actionStyle, GUILayout.Height(32)))
            ScanMisplacedFiles();

        if (scanned)
        {
            GUILayout.Space(10);
            DrawSummaryPanel();

            if (misplacedFiles.Count > 0)
            {
                GUILayout.Space(12);
                GUILayout.Label("File yang perlu dipindahkan", mutedStyle);
                GUILayout.Space(4);
                DrawMisplacedFileGroup(mutedStyle);
                GUILayout.Space(10);

                if (GUILayout.Button(
                    "Move All (" + misplacedFiles.Count + ")",
                    actionStyle,
                    GUILayout.Height(32)
                ))
                {
                    MoveAllMisplacedFiles();
                }
            }
        }
        else
        {
            GUILayout.Space(8);
            EditorGUILayout.HelpBox(
                "Tekan tombol scan untuk melihat file yang perlu dipindahkan.",
                MessageType.Info
            );
        }

        GUILayout.EndVertical();
        GUILayout.Space(12);
        GUILayout.EndHorizontal();
        EditorGUILayout.EndScrollView();
    }

    private void DrawSummaryPanel()
    {
        Rect panelRect = EditorGUILayout.GetControlRect(
            false,
            misplacedFiles.Count > 0 ? 78 : 78
        );
        EditorGUI.DrawRect(panelRect, new Color(0.10f, 0.10f, 0.10f));
        EditorGUI.DrawRect(
            new Rect(panelRect.x, panelRect.y, 5, panelRect.height),
            new Color(0.95f, 0.47f, 0.16f)
        );

        GUI.Label(
            new Rect(panelRect.x + 15, panelRect.y + 9, panelRect.width - 25, 20),
            "Hasil Pemindaian",
            MakeLabel(12, FontStyle.Bold, Color.white)
        );
        GUI.Label(
            new Rect(panelRect.x + 15, panelRect.y + 31, panelRect.width - 25, 18),
            "Total file dipindai: " + totalFilesScanned +
                "    |    Perlu dipindahkan: " + misplacedFiles.Count,
            MakeLabel(11, FontStyle.Normal, new Color(0.72f, 0.95f, 0.35f))
        );
        GUI.Label(
            new Rect(panelRect.x + 15, panelRect.y + 50, panelRect.width - 25, 23),
            misplacedFiles.Count == 0
                ? "Semua file sudah berada di folder yang sesuai."
                : "Periksa daftar file dan tujuan yang disarankan sebelum memindahkan.",
            MakeLabel(10, FontStyle.Normal, new Color(0.68f, 0.68f, 0.68f))
        );
    }

    private void DrawMisplacedFileGroup(GUIStyle mutedStyle)
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        GUILayout.Space(8);

        fileListScrollPosition = EditorGUILayout.BeginScrollView(
            fileListScrollPosition,
            GUILayout.Height(Mathf.Clamp(availableHeight - 245, 150, 260))
        );

        for (int i = misplacedFiles.Count - 1; i >= 0; i--)
        {
            MisplacedFileItem item = misplacedFiles[i];
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.Space(8);
            GUILayout.BeginHorizontal();
            GUILayout.Space(8);
            GUILayout.BeginVertical();

            EditorGUILayout.LabelField(item.fileName, EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Asal: " + item.currentPath, mutedStyle);
            EditorGUILayout.LabelField("Tujuan: " + item.suggestedPath, mutedStyle);

            GUILayout.Space(5);
            if (GUILayout.Button(
                "Pindahkan File Ini",
                GUILayout.ExpandWidth(true),
                GUILayout.Height(26)
            ))
            {
                RelocateSingleFile(item);
            }

            GUILayout.EndVertical();
            GUILayout.Space(8);
            GUILayout.EndHorizontal();
            GUILayout.Space(8);
            EditorGUILayout.EndVertical();
            GUILayout.Space(4);
        }

        EditorGUILayout.EndScrollView();
        GUILayout.Space(8);
        EditorGUILayout.EndVertical();
    }

    private void ScanMisplacedFiles()
    {
        misplacedFiles.Clear();
        totalFilesScanned = 0;

        string[] allFilePaths = Directory.GetFiles("Assets", "*.*", SearchOption.AllDirectories);
        foreach (string fullPath in allFilePaths)
        {
            string unityPath = fullPath.Replace("\\", "/");
            if (unityPath.EndsWith(".meta") || unityPath.Contains("/Editor/"))
                continue;

            totalFilesScanned++;
            string extension = Path.GetExtension(unityPath).ToLower();
            string fileName = Path.GetFileName(unityPath);

            if (
                extensionRules.TryGetValue(extension, out string targetFolder) &&
                !unityPath.StartsWith(targetFolder)
            )
            {
                misplacedFiles.Add(
                    new MisplacedFileItem
                    {
                        currentPath = unityPath,
                        suggestedPath = targetFolder + fileName,
                        fileName = fileName
                    }
                );
            }
        }

        scanned = true;
    }

    private void RelocateSingleFile(MisplacedFileItem item)
    {
        EnsureFolderExists(item.suggestedPath);

        string error = AssetDatabase.MoveAsset(item.currentPath, item.suggestedPath);
        if (string.IsNullOrEmpty(error))
        {
            misplacedFiles.Remove(item);
            AssetDatabase.Refresh();
        }
        else
        {
            Debug.LogError("[File Relocator] Gagal memindahkan file: " + error);
        }
    }

    private void MoveAllMisplacedFiles()
    {
        int successCount = 0;

        for (int i = misplacedFiles.Count - 1; i >= 0; i--)
        {
            MisplacedFileItem item = misplacedFiles[i];
            EnsureFolderExists(item.suggestedPath);

            string error = AssetDatabase.MoveAsset(item.currentPath, item.suggestedPath);
            if (string.IsNullOrEmpty(error))
            {
                misplacedFiles.RemoveAt(i);
                successCount++;
            }
            else
            {
                Debug.LogError("[File Relocator] Gagal memindahkan file: " + error);
            }
        }

        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog(
            "File Relocator Completed",
            "Berhasil memindahkan " + successCount + " file ke lokasi tujuan.",
            "OK"
        );
    }

    private void EnsureFolderExists(string assetPath)
    {
        string targetDirectory = Path.GetDirectoryName(assetPath).Replace("\\", "/");
        if (!Directory.Exists(targetDirectory))
            Directory.CreateDirectory(targetDirectory);
    }

    private static GUIStyle MakeLabel(int fontSize, FontStyle fontStyle, Color color)
    {
        return new GUIStyle(EditorStyles.label)
        {
            fontSize = fontSize,
            fontStyle = fontStyle,
            normal = { textColor = color }
        };
    }

    private static GUIStyle MakeButtonStyle(
        Texture2D normalTexture,
        Texture2D hoverTexture,
        Texture2D activeTexture
    )
    {
        return new GUIStyle(GUI.skin.button)
        {
            fontSize = 11,
            fontStyle = FontStyle.Bold,
            normal = { background = normalTexture, textColor = Color.white },
            hover = { background = hoverTexture, textColor = Color.white },
            active = { background = activeTexture, textColor = Color.white }
        };
    }
}

