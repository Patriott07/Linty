using System.Collections.Generic;
using UnityEngine;

public struct GuideItem
{
    public string propertyName;
    public string oneLineDesc;
    public string currentEffect;
    public Dictionary<int, string> effectConditions; // Array kondisi live
}

// Class kecil untuk menampung state buka/tutup tiap kartu
[System.Serializable]
public class PropertyState
{
    public bool isEffectVisible = false;
}
