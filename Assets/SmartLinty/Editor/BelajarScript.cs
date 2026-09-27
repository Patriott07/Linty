using UnityEditor;
using UnityEngine;

public class BelajarScript : EditorWindow
{
    // 2. Menambahkan Menu di bagian atas Unity (Tools -> PAHAM Assistant)
    [MenuItem("Tools/Linty Assisten Menu")]
    public static void ShowWindow()
    {
        // Buka atau fokus ke jendela
        GetWindow<BelajarScript>("Linty Assistant");
    }

    private void OnEnable()
    {
        // Dipanggil saat jendela terbuka.
        // Kita daftarkan fungsi agar mengecek setiap kali user memilih objek baru di Hierarchy
        Selection.selectionChanged += OnSelectionChanged;
    }

    private void OnDisable()
    {
        // Selalu lepas event listener saat jendela ditutup agar tidak bocor memori
        Selection.selectionChanged -= OnSelectionChanged;
    }

    private void OnSelectionChanged()
    {
        // Minta Unity menggambar ulang UI jendela saat ada objek lain yang dipilih
        Repaint();
    }

    // 3. Fungsi untuk menggambar UI Jendela Editor (Metode IMGUI)
    private void OnGUI()
    {
        // Judul Jendela
        GUILayout.Label("SmartLinty Assistant", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        // Ambil objek yang sedang di-klik/dipilih oleh user di Hierarchy
        GameObject selectedObj = Selection.activeGameObject;

        // Jika tidak ada objek yang dipilih
        if (selectedObj == null)
        {
            EditorGUILayout.HelpBox("Pilih salah satu GameObject di Hierarchy untuk melihat panduan.", MessageType.Info);
            return;
        }

        // Tampilkan Nama Objek yang dipilih
        EditorGUILayout.LabelField("Objek Terpilih:", selectedObj.name, EditorStyles.boldLabel);
        EditorGUILayout.Space();

        // CONTOH CEK KOMPONEN: Cek apakah objek punya Rigidbody2D
        Rigidbody rb = selectedObj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            DrawRigidbodyGuide(rb);
        }
        else
        {
            EditorGUILayout.HelpBox("Komponen biasa / tidak ada komponen yang dikenali.", MessageType.None);
        }
    }

    // Fungsi khusus untuk menampilkan bantuan Rigidbody2D
    private void DrawRigidbodyGuide(Rigidbody rb)
    {
        // 1. Tampilan Dokumen / Penjelasan
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        EditorGUILayout.LabelField("📘 Rigidbody Guide", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Fungsi: Memberikan simulasi fisika 3D (gravitasi, massa, dan tabrakan). Berguna untuk melakukan Collision System", EditorStyles.wordWrappedLabel);
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space();
        
        // 2. SMART DIAGNOSTIC & FIX BUTTON
        // Contoh Rule: Cek apakah Freeze Rotation Z tidak tercentang pada Dynamic Rigidbody
        if ((rb.constraints & RigidbodyConstraints.FreezeRotationZ) == 0)
        {
            EditorGUILayout.HelpBox("⚠️ Warning: Freeze Rotation Z belum tercentang! Karakter bisa berguling/tumbang saat berjalan.", MessageType.Warning);

            if (GUILayout.Button("Fix Instantly (Freeze Rotation Z)"))
            {
                // PENTING: Gunakan Undo agar tindakan ini bisa di-CTRL+Z oleh user
                Undo.RecordObject(rb, "Fix Rigidbody2D Rotation");
                
                // Centang FreezeRotationZ
                rb.constraints |= RigidbodyConstraints.FreezeRotationZ;
            }
        }
    }
}
