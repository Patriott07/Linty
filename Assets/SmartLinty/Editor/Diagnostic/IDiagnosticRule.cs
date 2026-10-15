using UnityEngine;

namespace  Custom.Interface
{
    public interface IDiagnosticRule
{
    // Mengecek apakah komponen memenuhi syarat atau ada error
    bool Evaluate(GameObject targetObject);

    // Merender UI peringatan dan tombol "Fix" di Inspector/Editor Window
    void DrawDiagnosticUI(GameObject targetObject);
}
}
