using System.Collections.Generic;

public static class FolderMakerDatabase
{
    public static readonly string[] Genres =
    {
        "Select Option here..",
        "Platformer",
        "TopDown / RPG",
        "Endless Runner",
        "Puzzle",
        "UI Narrative / Visual Novel",
        "Metroidvania",
        "Tower Defense",
        "Strategy / RTS",
        "Card / Deckbuilder",
        "Horror / Survival",
        "Simulation / Sandbox",
        "Multiplayer / Co-op"
    };

    public static readonly string[] ProjectTypes =
    {
        "Select Option here..",
        "2D",
        "3D",
        "2D Physics in 3D Space",
        "2.5D Hybrid",
        "VR / AR"
    };

    public static List<string> GetBaseFolders(bool animations, bool sprites, bool ui, bool audio, bool scripts, bool postProcessing)
    {
        var folders = new List<string>();
        if (animations) folders.Add("Assets/Animations");
        if (sprites) folders.AddRange(new[] { "Assets/Sprites", "Assets/Sprites/Character", "Assets/Sprites/Object", "Assets/Sprites/Environtment", "Assets/Textures" });
        if (ui) folders.AddRange(new[] { "Assets/UI", "Assets/UI/HUD", "Assets/UI/Menus" });
        if (audio) folders.AddRange(new[] { "Assets/Audio", "Assets/Audio/Music", "Assets/Audio/SFX" });
        if (scripts) folders.Add("Assets/Scripts");
        if (postProcessing) folders.Add("Assets/Post Processing");
        return folders;
    }

    public static List<string> GetProfileFolders(string genre, string projectType)
    {
        var folders = new List<string>
        {
            "Assets/Prefabs",
            "Assets/Scenes",
            "Assets/Materials"
        };

        switch (projectType)
        {
            case "2D":
                folders.AddRange(new[] { "Assets/Tilemaps", "Assets/Tilemaps/Palettes", "Assets/Tilemaps/Textures" });
                break;
            case "3D":
                folders.AddRange(new[] { "Assets/Models", "Assets/Materials/Environment", "Assets/Textures/Environment" });
                break;
            case "2D Physics in 3D Space":
            case "2.5D Hybrid":
                folders.AddRange(new[] { "Assets/Models", "Assets/Tilemaps", "Assets/Materials/Environment", "Assets/Textures/Environment" });
                break;
            case "VR / AR":
                folders.AddRange(new[] { "Assets/XR", "Assets/XR/Interactions", "Assets/XR/Prefabs", "Assets/Materials/Environment" });
                break;
        }

        switch (genre)
        {
            case "Platformer":
                folders.AddRange(new[] { "Assets/Levels/World1", "Assets/Scripts/Player", "Assets/Scripts/Enemies", "Assets/Scripts/Systems", "Assets/Prefabs/Level" });
                break;
            case "TopDown / RPG":
                folders.AddRange(new[] { "Assets/Levels/Dungeon1", "Assets/Dialogue/Dialogues", "Assets/Dialogue/Portraits", "Assets/Inventory/Items", "Assets/Quests/Data", "Assets/Scripts/Quests" });
                break;
            case "Endless Runner":
                folders.AddRange(new[] { "Assets/Levels/Chunks", "Assets/Prefabs/Chunks", "Assets/Prefabs/Obstacles", "Assets/Scripts/Spawner", "Assets/Scripts/Systems" });
                break;
            case "Puzzle":
                folders.AddRange(new[] { "Assets/Levels/Level1", "Assets/Prefabs/Pieces", "Assets/Scripts/Mechanics", "Assets/Scripts/Systems" });
                break;
            case "UI Narrative / Visual Novel":
                folders.AddRange(new[] { "Assets/Dialogue/Scripts", "Assets/Dialogue/Characters", "Assets/Dialogue/Backgrounds", "Assets/UI/Dialogue", "Assets/Audio/Voice" });
                break;
            case "Metroidvania":
                folders.AddRange(new[] { "Assets/Levels/WorldMap", "Assets/Levels/Rooms", "Assets/Prefabs/Abilities", "Assets/Prefabs/Enemies", "Assets/Scripts/Abilities", "Assets/Scripts/World" });
                break;
            case "Tower Defense":
                folders.AddRange(new[] { "Assets/Levels/Maps", "Assets/Prefabs/Towers", "Assets/Prefabs/Waves", "Assets/Scripts/Towers", "Assets/Scripts/Waves", "Assets/Data" });
                break;
            case "Strategy / RTS":
                folders.AddRange(new[] { "Assets/Levels/Maps", "Assets/Prefabs/Units", "Assets/Prefabs/Buildings", "Assets/Scripts/AI", "Assets/Scripts/Units", "Assets/Data/Factions" });
                break;
            case "Card / Deckbuilder":
                folders.AddRange(new[] { "Assets/Cards", "Assets/Cards/Art", "Assets/Cards/Data", "Assets/Decks", "Assets/Scripts/Cards", "Assets/Scripts/Decks" });
                break;
            case "Horror / Survival":
                folders.AddRange(new[] { "Assets/Levels/Locations", "Assets/Prefabs/Items", "Assets/Prefabs/Enemies", "Assets/Audio/Ambience", "Assets/Scripts/Survival", "Assets/Scripts/Events" });
                break;
            case "Simulation / Sandbox":
                folders.AddRange(new[] { "Assets/Levels/Sandbox", "Assets/Prefabs/Systems", "Assets/Scripts/Simulation", "Assets/Scripts/Systems", "Assets/Data/Simulation" });
                break;
            case "Multiplayer / Co-op":
                folders.AddRange(new[] { "Assets/Network", "Assets/Network/Prefabs", "Assets/Network/Scenes", "Assets/Scripts/Networking", "Assets/Scripts/Player", "Assets/Data/Match" });
                break;
        }

        return Deduplicate(folders);
    }

    public static List<string> GetAllFolders(string genre, string projectType, bool animations, bool sprites, bool ui, bool audio, bool scripts, bool postProcessing)
    {
        var folders = GetBaseFolders(animations, sprites, ui, audio, scripts, postProcessing);
        folders.AddRange(GetProfileFolders(genre, projectType));
        return Deduplicate(folders);
    }

    private static List<string> Deduplicate(List<string> folders)
    {
        var result = new List<string>();
        var seen = new HashSet<string>();
        foreach (string folder in folders)
        {
            string normalized = folder.Replace("\\", "/").TrimEnd('/');
            if (seen.Add(normalized)) result.Add(normalized);
        }
        return result;
    }
}
