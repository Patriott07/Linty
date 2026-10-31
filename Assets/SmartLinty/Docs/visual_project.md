# Visual Project — Listy Assistant

Dokumen ini menjadi referensi visual dan interaksi untuk popup onboarding Listy Assistant. Referensi utama berasal dari:

- `Assets/SmartLinty/Docs/Images/modal_popup1.png`
- `Assets/SmartLinty/Docs/Images/modal_popup2.png`
- `Assets/SmartLinty/Docs/Images/modal_popup3.png`
- `Assets/SmartLinty/Docs/Images/modal_popup4.png`

## Tema Visual Global

- Menggunakan tema dark mode dengan background abu-abu gelap.
- Warna teks utama adalah putih atau abu-abu terang.
- Warna oranye digunakan untuk instruksi dan informasi penting.
- Warna hijau lime digunakan untuk tombol aksi utama.
- Tombol sekunder menggunakan warna gelap dengan teks putih.
- Tampilan menggunakan layout flat, sederhana, dan minimalis.
- Heading menggunakan font tebal dengan ukuran lebih besar.
- Jarak antar elemen cukup longgar agar popup mudah dibaca.
- Setiap popup memiliki ukuran tetap dan diposisikan di tengah layar.

## Popup 1 — Welcome

File implementasi: `WelcomeLintyPopup.cs`

### Tampilan

- Background dark gray.
- Ilustrasi maskot berada di sisi kiri bagian atas.
- Judul besar `WELCOME!` berada di sisi kanan maskot.
- Subjudul `Listy Assistant` berada di bawah judul.
- Deskripsi singkat berada di bagian bawah area maskot dan judul.
- Teks bantuan berwarna oranye:

  `What next? Help Our System know what you made`

- Tombol `Next →` berada di bagian kiri bawah dan menjadi aksi utama.
- Link `Open pdf file` berada di sisi kanan bawah dengan warna biru muda.

### Interaksi

- Tombol `Next →` membuka popup Project Setup.
- Popup Welcome ditutup setelah berhasil berpindah ke popup berikutnya.
- Link `Open pdf file` membuka file dokumentasi PDF menggunakan aplikasi default sistem.

## Popup 2 — Project Setup

File implementasi: `ProjectSetupLintyPopup.cs`

### Tampilan

- Judul utama: `Whats you build?`
- Instruksi berwarna oranye:

  `Please Fill Correctly For Better Inspection Suggestion System`

- Form berisi tiga input:
  - `Name Project`
  - `Genre Game`
  - `Project Type (2d/3d)`
- Input menggunakan background hitam atau abu-abu sangat gelap.
- Placeholder menggunakan teks abu-abu.
- Dropdown memiliki ikon panah di sisi kanan.
- Tombol `← Previous` berwarna gelap.
- Tombol `Next →` berwarna hijau lime.

### Interaksi

- Nama project dapat diisi oleh pengguna.
- Genre game dipilih dari dropdown.
- Project type dipilih dari dropdown.
- Tombol `← Previous` kembali ke popup Welcome tanpa kehilangan data.
- Tombol `Next →` menyimpan nama project, genre, dan project type ke `EditorPrefs`.
- Setelah data tersimpan, popup berpindah ke Folder Confirmation.
- Nilai default seperti `Select Option here..` harus dianggap sebagai pilihan yang belum valid.

## Popup 3 — Folder Confirmation

File implementasi: `FolderStructureLintyPopup.cs`

### Tampilan

- Judul utama: `Thanks for filling project detail`.
- Instruksi berwarna oranye:

  `Now I suggest you to making structure standart folder`

- Daftar folder ditampilkan menggunakan checkbox.
- Checkbox aktif menggunakan tanda centang dengan tampilan terang.
- Folder standar yang ditampilkan meliputi:
  - Animations Folder
  - 2D Sprite Folder
  - UI Folder
  - Audio Folder
  - Scripts Folder
  - Post Processing Folder
- Tombol `Make it` berwarna hijau lime.
- Tombol `Tidak Sekarang` berwarna gelap.

### Interaksi

- Setiap checkbox dapat diaktifkan atau dinonaktifkan secara independen.
- Tombol `Make it` hanya membuat folder yang dipilih.
- Struktur tambahan berdasarkan genre dan project type ikut dibuat jika opsi genre-specific aktif.
- Tombol `Tidak Sekarang` melewati proses pembuatan folder dan menutup popup.
- Setelah folder berhasil dibuat, Asset Database di-refresh.
- Proses pembuatan folder tidak boleh membuat folder duplikat.

## Popup 4 — Folder Result

File implementasi: `FolderStructureLintyPopup.cs` atau popup hasil terpisah jika nantinya dipisahkan.

### Tampilan

- Judul utama: `Done`.
- Informasi ringkas menjelaskan jumlah folder yang siap digunakan.
- Daftar folder menggunakan ikon folder hijau.
- Setiap item folder memiliki ikon chevron di sisi kanan.
- Item folder pertama dapat dibuka untuk menampilkan deskripsi.
- Deskripsi folder berada dalam panel gelap yang sedikit lebih gelap dari background utama.
- Tombol `Next →` berada di bagian bawah kiri dengan warna hijau lime.
- Teks `You also can read it later` berada di bagian bawah kanan dengan warna abu-abu.

### Interaksi

- Item folder dapat di-expand dan di-collapse.
- Hanya detail folder yang dipilih yang ditampilkan.
- Tombol `Next →` melanjutkan ke tahap berikutnya atau menutup alur onboarding sesuai flow aplikasi.
- Teks dokumentasi di bagian bawah dapat digunakan sebagai opsi untuk membaca panduan di kemudian hari.

## Aturan Interaksi Umum

- Setiap tombol harus memiliki area klik yang jelas dan tidak tertutup elemen lain.
- State input harus dipertahankan ketika pengguna kembali ke popup sebelumnya.
- State checkbox harus tetap konsisten selama popup masih berada dalam alur onboarding.
- Tombol aksi utama menggunakan warna hijau lime secara konsisten.
- Tombol kembali, skip, atau aksi sekunder menggunakan warna gelap.
- Popup hanya berpindah setelah aksi berhasil diproses.
- Jika proses gagal, pengguna harus mendapatkan informasi error dan popup tidak berpindah secara diam-diam.
- Ukuran, warna, margin, dan alignment antar popup harus mengikuti pola visual yang sama.

## Status Implementasi Saat Ini

Implementasi kasar sudah tersedia pada:

- `Assets/SmartLinty/Editor/UI/WelcomeLintyPopup.cs`
- `Assets/SmartLinty/Editor/UI/ProjectSetupLintyPopup.cs`
- `Assets/SmartLinty/Editor/UI/FolderStructureLintyPopup.cs`

File-file tersebut masih perlu disesuaikan dengan desain referensi dan diperbaiki interaksinya, terutama pada penyimpanan input, perpindahan popup, checkbox folder, pembuatan folder, serta tampilan hasil folder.
