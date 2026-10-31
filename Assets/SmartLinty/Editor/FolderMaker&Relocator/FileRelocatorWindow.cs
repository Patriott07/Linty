using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class FileRelocatorWindow : EditorWindow
{
    private class MisplacedFileItem
    {
        public string currentPath;
        public string suggestedPath;
        public string fileName;
        public string extension;
    }

    private List<MisplacedFileItem> detectedFiles = new List<MisplacedFileItem>();
    private Vector2 scrollPosition;
    private bool isScanned = false;
    private int totalFilesScanned = 0;

    // Aturan Pemetaaan Folder Sesuai Ekstensi File
    private readonly Dictionary<string, string> extensionRules = new Dictionary<string, string>()
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

    [MenuItem("Tools/File Relocator")]
    public static void ShowWindow()
    {
        FileRelocatorWindow window = GetWindow<FileRelocatorWindow>("File Relocator");
        window.minSize = new Vector2(500, 400);
    }

    private void OnGUI()
    {
        GUILayout.Space(10);
        DrawHeader();
        GUILayout.Space(10);

        // --- 1. ONE BUTTON ACTION SCAN ---
        if (GUILayout.Button("🔍 Scan Misplaced Project Files", GUILayout.Height(35)))
        {
            ScanProjectFiles();
        }

        GUILayout.Space(10);

        // --- 2. INFORMATION PANEL ---
        if (isScanned)
        {
            DrawInfoPanel();
            GUILayout.Space(10);

            // --- 5. BULK ACTION: MOVE ALL ---
            if (detectedFiles.Count > 0)
            {
                GUI.backgroundColor = new Color(0.3f, 0.8f, 0.3f);
                if (GUILayout.Button($"🚀 Move All ({detectedFiles.Count}) to Destination Suggest", GUILayout.Height(30)))
                {
                    MoveAllFiles();
                }
                GUI.backgroundColor = Color.white;

                GUILayout.Space(10);
            }

            // --- 3 & 4. LIST FILE TERDETEKSI & PER-FILE ACTION ---
            DrawFileList();
        }
    }

    private void DrawHeader()
    {
        EditorGUILayout.LabelField("📁 File Relocator Engine", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox(
            "Tool ini memindai file yang diletakkan di tempat kurang pas dan memindahkannya secara aman menggunakan AssetDatabase (menjaga GUID & reference).", 
            MessageType.Info
        );
    }

    private void DrawInfoPanel()
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        EditorGUILayout.LabelField("📊 Hasil Pemindaian:", EditorStyles.boldLabel);
        EditorGUILayout.LabelField($"• Total File Dipindai: {totalFilesScanned}");
        
        if (detectedFiles.Count > 0)
        {
            EditorGUILayout.LabelField($"• File Salah Tempat: {detectedFiles.Count}", EditorStyles.boldLabel);
        }
        else
        {
            EditorGUILayout.LabelField("✅ Semua file sudah tersusun rapi di tempat yang sesuai!", EditorStyles.wordWrappedMiniLabel);
        }
        EditorGUILayout.EndVertical();
    }

    private void DrawFileList()
    {
        if (detectedFiles.Count == 0) return;

        EditorGUILayout.LabelField("List File Yang Perlu Dipindahkan:", EditorStyles.boldLabel);
        
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.ExpandHeight(true));

        for (int i = detectedFiles.Count - 1; i >= 0; i--)
        {
            var item = detectedFiles[i];

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            
            // Nama File
            EditorGUILayout.LabelField($"📄 {item.fileName}", EditorStyles.boldLabel);
            
            // Lokasi Saat Ini vs Lokasi Tujuan
            EditorGUILayout.LabelField($"   Asal   : {item.currentPath}", EditorStyles.miniLabel);
            EditorGUILayout.LabelField($"   Tujuan : {item.suggestedPath}", EditorStyles.miniBoldLabel);

            GUILayout.Space(4);

            // --- 4. PER-FILE RELOCATE ACTION ---
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Pindahkan File Ini", GUILayout.Width(150)))
            {
                RelocateSingleFile(item);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();
            GUILayout.Space(2);
        }

        EditorGUILayout.EndScrollView();
    }

    // ==========================================
    // LOGIC SYSTEM
    // ==========================================

    private void ScanProjectFiles()
    {
        detectedFiles.Clear();
        totalFilesScanned = 0;

        // Ambil seluruh file di folder Assets (mengabaikan folder Editor atau Packages)
        string[] allFilePaths = Directory.GetFiles("Assets", "*.*", SearchOption.AllDirectories);

        foreach (string fullPath in allFilePaths)
        {
            // Format path agar menggunakan slash standar Unity (/)
            string unityPath = fullPath.Replace("\\", "/");

            // Abaikan file .meta, folder Editor, dan file buatan sistem
            if (unityPath.EndsWith(".meta") || unityPath.Contains("/Editor/")) continue;

            totalFilesScanned++;

            string ext = Path.GetExtension(unityPath).ToLower();
            string fileName = Path.GetFileName(unityPath);

            // Cek apakah ekstensi didukung dalam aturan
            if (extensionRules.TryGetValue(ext, out string targetFolder))
            {
                string suggestedPath = targetFolder + fileName;

                // Jika lokasi saat ini tidak diawali dengan target folder yang disarankan
                if (!unityPath.StartsWith(targetFolder))
                {
                    detectedFiles.Add(new MisplacedFileItem()
                    {
                        currentPath = unityPath,
                        suggestedPath = suggestedPath,
                        fileName = fileName,
                        extension = ext
                    });
                }
            }
        }

        isScanned = true;
    }

    private void RelocateSingleFile(MisplacedFileItem item)
    {
        EnsureFolderExists(Path.GetDirectoryName(item.suggestedPath));

        // Menggunakan AssetDatabase.MoveAsset agar GUID & References tidak rusak!
        string error = AssetDatabase.MoveAsset(item.currentPath, item.suggestedPath);

        if (string.IsNullOrEmpty(error))
        {
            Debug.Log($"<color=green>[File Relocator]</color> Berhasil memindahkan {item.fileName} ke {item.suggestedPath}");
            detectedFiles.Remove(item);
            AssetDatabase.Refresh();
        }
        else
        {
            Debug.LogError($"[File Relocator] Gagal memindahkan file: {error}");
        }
    }

    private void MoveAllFiles()
    {
        int successCount = 0;

        // Loop mundur agar penghapusan item dari list berjalan aman
        for (int i = detectedFiles.Count - 1; i >= 0; i--)
        {
            var item = detectedFiles[i];
            EnsureFolderExists(Path.GetDirectoryName(item.suggestedPath));

            string error = AssetDatabase.MoveAsset(item.currentPath, item.suggestedPath);

            if (string.IsNullOrEmpty(error))
            {
                successCount++;
                detectedFiles.RemoveAt(i);
            }
            else
            {
                Debug.LogError($"[File Relocator] Gagal memindahkan {item.fileName}: {error}");
            }
        }

        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("File Relocator Completed", $"Berhasil memindahkan {successCount} file ke lokasi tujuan ideal!", "OK");
    }

    private void EnsureFolderExists(string targetDirectory)
    {
        if (!Directory.Exists(targetDirectory))
        {
            Directory.CreateDirectory(targetDirectory);
        }
    }
}