# Folder Maker by Project Type and Genre Game Combination

Dokumen ini menjelaskan standar Folder Maker SmartLinty berdasarkan kombinasi `Project Type` dan `Genre Game`.

## Analisis Implementasi Sebelumnya

Implementasi awal pada `FolderStructureLintyPopup.cs` sudah memiliki konsep dasar yang benar, yaitu membuat folder berdasarkan pilihan genre dan tipe project. Namun, beberapa bagian masih perlu diperbaiki:

- Daftar genre dan project type ditulis ulang di beberapa popup sehingga mudah tidak sinkron.
- Struktur folder disimpan sebagai banyak method hardcoded di dalam popup.
- Beberapa kombinasi genre baru belum memiliki preset.
- Folder dasar dan folder khusus genre dapat menghasilkan path duplikat.
- Field `Name Project` sebelumnya belum mengisi nilai `projectName` dengan benar.
- Pilihan default `Select Option here..` masih dapat diteruskan tanpa validasi.
- Folder Maker dan File Relocator masih merupakan dua fitur terpisah dengan aturan yang berbeda.

## Prinsip Folder Maker Baru

- Daftar genre dan project type menjadi sumber data terpusat pada `FolderMakerDatabase.cs`.
- Folder umum dipisahkan dari folder khusus kombinasi genre dan project type.
- Path folder dinormalisasi menggunakan tanda `/`.
- Folder duplikat dihapus sebelum proses pembuatan.
- Folder hanya dibuat jika belum tersedia melalui `AssetDatabase`.
- Struktur preset dapat dipreview sebelum tombol `Make it` ditekan.
- Project Setup memvalidasi nama project, genre, dan project type sebelum berpindah ke tahap berikutnya.

## Project Type yang Didukung

| Project Type | Fokus Struktur |
|---|---|
| 2D | Sprite, Tilemap, Palette, prefab 2D, dan level 2D. |
| 3D | Model, material, texture environment, prefab 3D, dan level 3D. |
| 2D Physics in 3D Space | Sprite atau Tilemap yang berinteraksi dalam ruang 3D. |
| 2.5D Hybrid | Gabungan asset 2D dan 3D dengan material serta level hybrid. |
| VR / AR | Struktur XR, interaction, prefab XR, dan networking bila diperlukan. |

## Genre Game yang Didukung

| Genre Game | Folder Khusus Utama |
|---|---|
| Platformer | Player, Enemies, Level, Systems. |
| TopDown / RPG | Dialogue, Inventory, Quests, NPC, Dungeon. |
| Endless Runner | Chunks, Obstacles, Spawner, Systems. |
| Puzzle | Pieces, Mechanics, Level, Systems. |
| UI Narrative / Visual Novel | Dialogue, Characters, Backgrounds, Voice, UI Dialogue. |
| Metroidvania | World Map, Rooms, Abilities, Enemies, World Systems. |
| Tower Defense | Towers, Waves, Maps, Tower Systems, Wave Systems. |
| Strategy / RTS | Units, Buildings, Factions, AI, Maps. |
| Card / Deckbuilder | Cards, Card Art, Card Data, Decks, Card Systems. |
| Horror / Survival | Locations, Items, Enemies, Ambience, Survival Systems. |
| Simulation / Sandbox | Sandbox Levels, Simulation Systems, Data, System Prefabs. |
| Multiplayer / Co-op | Network, Network Prefabs, Match Data, Networking Systems. |

## Folder Dasar

Folder dasar bersifat opsional dan dikontrol melalui checkbox pada popup:

- `Assets/Animations`
- `Assets/Sprites`
- `Assets/Textures`
- `Assets/UI`
- `Assets/UI/HUD`
- `Assets/UI/Menus`
- `Assets/Audio`
- `Assets/Audio/Music`
- `Assets/Audio/SFX`
- `Assets/Scripts`
- `Assets/Post Processing`

## Kombinasi Folder

Folder Maker menggunakan dua lapisan struktur:

1. Folder dasar berdasarkan kebutuhan umum project.
2. Folder profile berdasarkan kombinasi `Genre Game` dan `Project Type`.

Contoh kombinasi:

### Platformer + 2D

- `Assets/Tilemaps`
- `Assets/Tilemaps/Palettes`
- `Assets/Tilemaps/Textures`
- `Assets/Levels/World1`
- `Assets/Prefabs/Level`
- `Assets/Scripts/Player`
- `Assets/Scripts/Enemies`
- `Assets/Scripts/Systems`

### Tower Defense + 3D

- `Assets/Models`
- `Assets/Materials/Environment`
- `Assets/Textures/Environment`
- `Assets/Levels/Maps`
- `Assets/Prefabs/Towers`
- `Assets/Prefabs/Waves`
- `Assets/Scripts/Towers`
- `Assets/Scripts/Waves`
- `Assets/Data`

### Card / Deckbuilder + 2D

- `Assets/Tilemaps`
- `Assets/Cards`
- `Assets/Cards/Art`
- `Assets/Cards/Data`
- `Assets/Decks`
- `Assets/Scripts/Cards`
- `Assets/Scripts/Decks`

### Multiplayer / Co-op + 3D

- `Assets/Models`
- `Assets/Materials/Environment`
- `Assets/Textures/Environment`
- `Assets/Network`
- `Assets/Network/Prefabs`
- `Assets/Network/Scenes`
- `Assets/Scripts/Networking`
- `Assets/Scripts/Player`
- `Assets/Data/Match`

## Alur Penggunaan

1. Pengguna mengisi nama project.
2. Pengguna memilih genre game.
3. Pengguna memilih project type.
4. Folder Maker menampilkan preview preset struktur.
5. Pengguna memilih folder dasar yang ingin dibuat.
6. Pengguna menekan `Make it`.
7. Folder dasar dan folder profile digabungkan.
8. Path yang duplikat dihapus.
9. Folder dibuat secara rekursif melalui Asset Database.
10. Asset Database di-refresh setelah proses selesai.

## Batasan dan Pengembangan Berikutnya

- Folder Maker belum memindahkan asset yang sudah ada; fungsi tersebut tetap menjadi tanggung jawab File Relocator.
- File Relocator masih menggunakan aturan ekstensi global dan sebaiknya dikembangkan agar membaca profile project.
- Preset genre dapat diperluas tanpa mengubah flow popup.
- Sistem berikutnya dapat menambahkan template folder custom dan export/import preset.
## Matriks Lengkap Combination

Folder Maker saat ini memiliki 12 genre game dan 5 project type, sehingga tersedia 60 kombinasi preset. Setiap kombinasi selalu mendapatkan folder umum berikut:

- `Assets/Prefabs`
- `Assets/Scenes`
- `Assets/Materials`

Setelah itu Folder Maker menggabungkan folder `Project Type` dan folder `Genre Game`. Daftar di bawah ini menjabarkan seluruh kombinasi yang tersedia. Folder yang sama tetap hanya dibuat satu kali karena sistem melakukan deduplikasi path.

### Referensi Folder Project Type

| Project Type | Folder Type |
|---|---|
| 2D | `Assets/Tilemaps`, `Assets/Tilemaps/Palettes`, `Assets/Tilemaps/Textures` |
| 3D | `Assets/Models`, `Assets/Materials/Environment`, `Assets/Textures/Environment` |
| 2D Physics in 3D Space | `Assets/Models`, `Assets/Tilemaps`, `Assets/Materials/Environment`, `Assets/Textures/Environment` |
| 2.5D Hybrid | `Assets/Models`, `Assets/Tilemaps`, `Assets/Materials/Environment`, `Assets/Textures/Environment` |
| VR / AR | `Assets/XR`, `Assets/XR/Interactions`, `Assets/XR/Prefabs`, `Assets/Materials/Environment` |

### 1. Platformer

| Combination | Folder Genre |
|---|---|
| Platformer + 2D | `Assets/Levels/World1`, `Assets/Scripts/Player`, `Assets/Scripts/Enemies`, `Assets/Scripts/Systems`, `Assets/Prefabs/Level` |
| Platformer + 3D | `Assets/Levels/World1`, `Assets/Scripts/Player`, `Assets/Scripts/Enemies`, `Assets/Scripts/Systems`, `Assets/Prefabs/Level` |
| Platformer + 2D Physics in 3D Space | `Assets/Levels/World1`, `Assets/Scripts/Player`, `Assets/Scripts/Enemies`, `Assets/Scripts/Systems`, `Assets/Prefabs/Level` |
| Platformer + 2.5D Hybrid | `Assets/Levels/World1`, `Assets/Scripts/Player`, `Assets/Scripts/Enemies`, `Assets/Scripts/Systems`, `Assets/Prefabs/Level` |
| Platformer + VR / AR | `Assets/Levels/World1`, `Assets/Scripts/Player`, `Assets/Scripts/Enemies`, `Assets/Scripts/Systems`, `Assets/Prefabs/Level` |

### 2. TopDown / RPG

| Combination | Folder Genre |
|---|---|
| TopDown / RPG + 2D | `Assets/Levels/Dungeon1`, `Assets/Dialogue/Dialogues`, `Assets/Dialogue/Portraits`, `Assets/Inventory/Items`, `Assets/Quests/Data`, `Assets/Scripts/Quests` |
| TopDown / RPG + 3D | `Assets/Levels/Dungeon1`, `Assets/Dialogue/Dialogues`, `Assets/Dialogue/Portraits`, `Assets/Inventory/Items`, `Assets/Quests/Data`, `Assets/Scripts/Quests` |
| TopDown / RPG + 2D Physics in 3D Space | `Assets/Levels/Dungeon1`, `Assets/Dialogue/Dialogues`, `Assets/Dialogue/Portraits`, `Assets/Inventory/Items`, `Assets/Quests/Data`, `Assets/Scripts/Quests` |
| TopDown / RPG + 2.5D Hybrid | `Assets/Levels/Dungeon1`, `Assets/Dialogue/Dialogues`, `Assets/Dialogue/Portraits`, `Assets/Inventory/Items`, `Assets/Quests/Data`, `Assets/Scripts/Quests` |
| TopDown / RPG + VR / AR | `Assets/Levels/Dungeon1`, `Assets/Dialogue/Dialogues`, `Assets/Dialogue/Portraits`, `Assets/Inventory/Items`, `Assets/Quests/Data`, `Assets/Scripts/Quests` |

### 3. Endless Runner

| Combination | Folder Genre |
|---|---|
| Endless Runner + 2D | `Assets/Levels/Chunks`, `Assets/Prefabs/Chunks`, `Assets/Prefabs/Obstacles`, `Assets/Scripts/Spawner`, `Assets/Scripts/Systems` |
| Endless Runner + 3D | `Assets/Levels/Chunks`, `Assets/Prefabs/Chunks`, `Assets/Prefabs/Obstacles`, `Assets/Scripts/Spawner`, `Assets/Scripts/Systems` |
| Endless Runner + 2D Physics in 3D Space | `Assets/Levels/Chunks`, `Assets/Prefabs/Chunks`, `Assets/Prefabs/Obstacles`, `Assets/Scripts/Spawner`, `Assets/Scripts/Systems` |
| Endless Runner + 2.5D Hybrid | `Assets/Levels/Chunks`, `Assets/Prefabs/Chunks`, `Assets/Prefabs/Obstacles`, `Assets/Scripts/Spawner`, `Assets/Scripts/Systems` |
| Endless Runner + VR / AR | `Assets/Levels/Chunks`, `Assets/Prefabs/Chunks`, `Assets/Prefabs/Obstacles`, `Assets/Scripts/Spawner`, `Assets/Scripts/Systems` |

### 4. Puzzle

| Combination | Folder Genre |
|---|---|
| Puzzle + 2D | `Assets/Levels/Level1`, `Assets/Prefabs/Pieces`, `Assets/Scripts/Mechanics`, `Assets/Scripts/Systems` |
| Puzzle + 3D | `Assets/Levels/Level1`, `Assets/Prefabs/Pieces`, `Assets/Scripts/Mechanics`, `Assets/Scripts/Systems` |
| Puzzle + 2D Physics in 3D Space | `Assets/Levels/Level1`, `Assets/Prefabs/Pieces`, `Assets/Scripts/Mechanics`, `Assets/Scripts/Systems` |
| Puzzle + 2.5D Hybrid | `Assets/Levels/Level1`, `Assets/Prefabs/Pieces`, `Assets/Scripts/Mechanics`, `Assets/Scripts/Systems` |
| Puzzle + VR / AR | `Assets/Levels/Level1`, `Assets/Prefabs/Pieces`, `Assets/Scripts/Mechanics`, `Assets/Scripts/Systems` |

### 5. UI Narrative / Visual Novel

| Combination | Folder Genre |
|---|---|
| UI Narrative / Visual Novel + 2D | `Assets/Dialogue/Scripts`, `Assets/Dialogue/Characters`, `Assets/Dialogue/Backgrounds`, `Assets/UI/Dialogue`, `Assets/Audio/Voice` |
| UI Narrative / Visual Novel + 3D | `Assets/Dialogue/Scripts`, `Assets/Dialogue/Characters`, `Assets/Dialogue/Backgrounds`, `Assets/UI/Dialogue`, `Assets/Audio/Voice` |
| UI Narrative / Visual Novel + 2D Physics in 3D Space | `Assets/Dialogue/Scripts`, `Assets/Dialogue/Characters`, `Assets/Dialogue/Backgrounds`, `Assets/UI/Dialogue`, `Assets/Audio/Voice` |
| UI Narrative / Visual Novel + 2.5D Hybrid | `Assets/Dialogue/Scripts`, `Assets/Dialogue/Characters`, `Assets/Dialogue/Backgrounds`, `Assets/UI/Dialogue`, `Assets/Audio/Voice` |
| UI Narrative / Visual Novel + VR / AR | `Assets/Dialogue/Scripts`, `Assets/Dialogue/Characters`, `Assets/Dialogue/Backgrounds`, `Assets/UI/Dialogue`, `Assets/Audio/Voice` |

### 6. Metroidvania

| Combination | Folder Genre |
|---|---|
| Metroidvania + 2D | `Assets/Levels/WorldMap`, `Assets/Levels/Rooms`, `Assets/Prefabs/Abilities`, `Assets/Prefabs/Enemies`, `Assets/Scripts/Abilities`, `Assets/Scripts/World` |
| Metroidvania + 3D | `Assets/Levels/WorldMap`, `Assets/Levels/Rooms`, `Assets/Prefabs/Abilities`, `Assets/Prefabs/Enemies`, `Assets/Scripts/Abilities`, `Assets/Scripts/World` |
| Metroidvania + 2D Physics in 3D Space | `Assets/Levels/WorldMap`, `Assets/Levels/Rooms`, `Assets/Prefabs/Abilities`, `Assets/Prefabs/Enemies`, `Assets/Scripts/Abilities`, `Assets/Scripts/World` |
| Metroidvania + 2.5D Hybrid | `Assets/Levels/WorldMap`, `Assets/Levels/Rooms`, `Assets/Prefabs/Abilities`, `Assets/Prefabs/Enemies`, `Assets/Scripts/Abilities`, `Assets/Scripts/World` |
| Metroidvania + VR / AR | `Assets/Levels/WorldMap`, `Assets/Levels/Rooms`, `Assets/Prefabs/Abilities`, `Assets/Prefabs/Enemies`, `Assets/Scripts/Abilities`, `Assets/Scripts/World` |

### 7. Tower Defense

| Combination | Folder Genre |
|---|---|
| Tower Defense + 2D | `Assets/Levels/Maps`, `Assets/Prefabs/Towers`, `Assets/Prefabs/Waves`, `Assets/Scripts/Towers`, `Assets/Scripts/Waves`, `Assets/Data` |
| Tower Defense + 3D | `Assets/Levels/Maps`, `Assets/Prefabs/Towers`, `Assets/Prefabs/Waves`, `Assets/Scripts/Towers`, `Assets/Scripts/Waves`, `Assets/Data` |
| Tower Defense + 2D Physics in 3D Space | `Assets/Levels/Maps`, `Assets/Prefabs/Towers`, `Assets/Prefabs/Waves`, `Assets/Scripts/Towers`, `Assets/Scripts/Waves`, `Assets/Data` |
| Tower Defense + 2.5D Hybrid | `Assets/Levels/Maps`, `Assets/Prefabs/Towers`, `Assets/Prefabs/Waves`, `Assets/Scripts/Towers`, `Assets/Scripts/Waves`, `Assets/Data` |
| Tower Defense + VR / AR | `Assets/Levels/Maps`, `Assets/Prefabs/Towers`, `Assets/Prefabs/Waves`, `Assets/Scripts/Towers`, `Assets/Scripts/Waves`, `Assets/Data` |

### 8. Strategy / RTS

| Combination | Folder Genre |
|---|---|
| Strategy / RTS + 2D | `Assets/Levels/Maps`, `Assets/Prefabs/Units`, `Assets/Prefabs/Buildings`, `Assets/Scripts/AI`, `Assets/Scripts/Units`, `Assets/Data/Factions` |
| Strategy / RTS + 3D | `Assets/Levels/Maps`, `Assets/Prefabs/Units`, `Assets/Prefabs/Buildings`, `Assets/Scripts/AI`, `Assets/Scripts/Units`, `Assets/Data/Factions` |
| Strategy / RTS + 2D Physics in 3D Space | `Assets/Levels/Maps`, `Assets/Prefabs/Units`, `Assets/Prefabs/Buildings`, `Assets/Scripts/AI`, `Assets/Scripts/Units`, `Assets/Data/Factions` |
| Strategy / RTS + 2.5D Hybrid | `Assets/Levels/Maps`, `Assets/Prefabs/Units`, `Assets/Prefabs/Buildings`, `Assets/Scripts/AI`, `Assets/Scripts/Units`, `Assets/Data/Factions` |
| Strategy / RTS + VR / AR | `Assets/Levels/Maps`, `Assets/Prefabs/Units`, `Assets/Prefabs/Buildings`, `Assets/Scripts/AI`, `Assets/Scripts/Units`, `Assets/Data/Factions` |

### 9. Card / Deckbuilder

| Combination | Folder Genre |
|---|---|
| Card / Deckbuilder + 2D | `Assets/Cards`, `Assets/Cards/Art`, `Assets/Cards/Data`, `Assets/Decks`, `Assets/Scripts/Cards`, `Assets/Scripts/Decks` |
| Card / Deckbuilder + 3D | `Assets/Cards`, `Assets/Cards/Art`, `Assets/Cards/Data`, `Assets/Decks`, `Assets/Scripts/Cards`, `Assets/Scripts/Decks` |
| Card / Deckbuilder + 2D Physics in 3D Space | `Assets/Cards`, `Assets/Cards/Art`, `Assets/Cards/Data`, `Assets/Decks`, `Assets/Scripts/Cards`, `Assets/Scripts/Decks` |
| Card / Deckbuilder + 2.5D Hybrid | `Assets/Cards`, `Assets/Cards/Art`, `Assets/Cards/Data`, `Assets/Decks`, `Assets/Scripts/Cards`, `Assets/Scripts/Decks` |
| Card / Deckbuilder + VR / AR | `Assets/Cards`, `Assets/Cards/Art`, `Assets/Cards/Data`, `Assets/Decks`, `Assets/Scripts/Cards`, `Assets/Scripts/Decks` |

### 10. Horror / Survival

| Combination | Folder Genre |
|---|---|
| Horror / Survival + 2D | `Assets/Levels/Locations`, `Assets/Prefabs/Items`, `Assets/Prefabs/Enemies`, `Assets/Audio/Ambience`, `Assets/Scripts/Survival`, `Assets/Scripts/Events` |
| Horror / Survival + 3D | `Assets/Levels/Locations`, `Assets/Prefabs/Items`, `Assets/Prefabs/Enemies`, `Assets/Audio/Ambience`, `Assets/Scripts/Survival`, `Assets/Scripts/Events` |
| Horror / Survival + 2D Physics in 3D Space | `Assets/Levels/Locations`, `Assets/Prefabs/Items`, `Assets/Prefabs/Enemies`, `Assets/Audio/Ambience`, `Assets/Scripts/Survival`, `Assets/Scripts/Events` |
| Horror / Survival + 2.5D Hybrid | `Assets/Levels/Locations`, `Assets/Prefabs/Items`, `Assets/Prefabs/Enemies`, `Assets/Audio/Ambience`, `Assets/Scripts/Survival`, `Assets/Scripts/Events` |
| Horror / Survival + VR / AR | `Assets/Levels/Locations`, `Assets/Prefabs/Items`, `Assets/Prefabs/Enemies`, `Assets/Audio/Ambience`, `Assets/Scripts/Survival`, `Assets/Scripts/Events` |

### 11. Simulation / Sandbox

| Combination | Folder Genre |
|---|---|
| Simulation / Sandbox + 2D | `Assets/Levels/Sandbox`, `Assets/Prefabs/Systems`, `Assets/Scripts/Simulation`, `Assets/Scripts/Systems`, `Assets/Data/Simulation` |
| Simulation / Sandbox + 3D | `Assets/Levels/Sandbox`, `Assets/Prefabs/Systems`, `Assets/Scripts/Simulation`, `Assets/Scripts/Systems`, `Assets/Data/Simulation` |
| Simulation / Sandbox + 2D Physics in 3D Space | `Assets/Levels/Sandbox`, `Assets/Prefabs/Systems`, `Assets/Scripts/Simulation`, `Assets/Scripts/Systems`, `Assets/Data/Simulation` |
| Simulation / Sandbox + 2.5D Hybrid | `Assets/Levels/Sandbox`, `Assets/Prefabs/Systems`, `Assets/Scripts/Simulation`, `Assets/Scripts/Systems`, `Assets/Data/Simulation` |
| Simulation / Sandbox + VR / AR | `Assets/Levels/Sandbox`, `Assets/Prefabs/Systems`, `Assets/Scripts/Simulation`, `Assets/Scripts/Systems`, `Assets/Data/Simulation` |

### 12. Multiplayer / Co-op

| Combination | Folder Genre |
|---|---|
| Multiplayer / Co-op + 2D | `Assets/Network`, `Assets/Network/Prefabs`, `Assets/Network/Scenes`, `Assets/Scripts/Networking`, `Assets/Scripts/Player`, `Assets/Data/Match` |
| Multiplayer / Co-op + 3D | `Assets/Network`, `Assets/Network/Prefabs`, `Assets/Network/Scenes`, `Assets/Scripts/Networking`, `Assets/Scripts/Player`, `Assets/Data/Match` |
| Multiplayer / Co-op + 2D Physics in 3D Space | `Assets/Network`, `Assets/Network/Prefabs`, `Assets/Network/Scenes`, `Assets/Scripts/Networking`, `Assets/Scripts/Player`, `Assets/Data/Match` |
| Multiplayer / Co-op + 2.5D Hybrid | `Assets/Network`, `Assets/Network/Prefabs`, `Assets/Network/Scenes`, `Assets/Scripts/Networking`, `Assets/Scripts/Player`, `Assets/Data/Match` |
| Multiplayer / Co-op + VR / AR | `Assets/Network`, `Assets/Network/Prefabs`, `Assets/Network/Scenes`, `Assets/Scripts/Networking`, `Assets/Scripts/Player`, `Assets/Data/Match` |

### Cara Membaca Matriks

Contoh `Tower Defense + 3D` menghasilkan folder umum, folder type 3D, dan folder genre Tower Defense. Contoh `Tower Defense + 2D` menggunakan folder genre yang sama, tetapi mengganti folder type 3D dengan folder Tilemap 2D. Dengan pola ini, setiap combination memiliki preset yang jelas tanpa perlu membuat 60 method terpisah di popup.
