**Dokumen Guide**

**Kata Pengantar**   
Selamat datang di MiniLinty-Dokumen Guide. Dokumen ini disusun sebagai buku pedoman utama (*technical handbook*) bagi seluruh anggota tim dalam merancang, mengembangkan, dan menggunakan ekstensi MiniLinty Assistant di dalam Unity Editor. 

**Tentang Buku Panduan Ini**   
Pengembangan game yang efisien dan berkelanjutan menuntut adanya keselarasan visi, standarisasi alur kerja, serta pemahaman teknis yang mendalam antar anggota tim. Buku pedoman ini dirancang bukan sekadar sebagai catatan kode atau dokumentasi singkat, melainkan sebagai *single source of truth* serta rujukan utama bagi setiap developer yang ingin memahami fondasi arsitektur project ini secara komprehensif. 

**Cakupan Materi**   
Dokumen ini berfokus pada Dokumentasi Component dan Line Pda tiap tiap Component. 

**Note**   
Dokumentasi akan mengikuti penulisan format dibawah :

Name Property : \<name of property\>  
One line Guide : \<very short desc\>  
Threeline Guide : \<Three Paragraf for detail explanation\>  
Is Have Feature Eyes on Current Effect : \<bool\>

1. When ‘true’ : …\<detail explanation\>  
2. When ‘false’ : … \<detail explanation\>  
3. When 0.5 :  … \<detail explanation\>  
4. When 1 :  … \<detail explanation\>  
5. When 0 :  … \<detail explanation\>

Visual   
![][image1]  
![][image2]![][image3]

tampilan berbeda antara “langsung di inspector component” atau “window baru \+ tab Guide” 

Navigasi 

Rigidbody2D   
BoxCollider2D   
Transform   
Canvas,   
Canvas Group,   
AudioSource 

**Rigidbody2D**   
Rigidbody 2D adalah komponen di Unity yang memberikan sifat fisik dunia nyata pada objek 2 dimensi, seperti berat dan massa. Dengan komponen ini, objek Anda bisa terpengaruh oleh gravitasi, saling bertabrakan, dan bergerak secara realistis. 

### **1\. Body Type**

* **Name Property** : Body Type  
* **One line Guide** : Menentukan jenis perilaku fisika dasar objek apakah bergerak bebas, statis, atau kinematik.  
* **Threeline Guide** :  
  Properti Body Type adalah pengatur utama bagaimana Rigidbody2D berinteraksi dengan mesin fisika, dengan tiga pilihan utama yaitu Dynamic, Kinematic, dan Static. Dynamic membuat objek sepenuhnya terpengaruh oleh gaya fisika seperti gravitasi dan dorongan, sangat cocok untuk karakter pemain atau bola.  
  Static dirancang untuk objek yang tidak pernah bergerak sama sekali seperti lantai, dinding, atau rintangan permanen, yang membuat mesin fisika mengoptimalkan performa karena posisi objek ini tidak perlu dihitung ulang setiap frame.  
  Kinematic berada di tengah-tengah, di mana objek digerakkan melalui skrip atau animasi tanpa terpengaruh oleh gaya eksternal, tetapi tetap dapat mendorong objek Dynamic lain di sekitarnya. Pemilihan tipe ini sangat menentukan efisiensi dan kebenaran logika mekanik game Anda.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Dynamic** : Objek sepenuhnya aktif dan terpengaruh oleh gravitasi serta gaya fisika luar, ideal untuk benda yang bisa bergerak bebas.  
* **2\. When Kinematic** : Objek digerakkan secara manual lewat skrip/animasi dan mengabaikan gaya luar, namun tetap dapat menabrak objek Dynamic.  
* **3\. When Static** : Objek dibekukan permanen sebagai penghalang/lantai untuk menghemat performa kalkulasi fisika secara maksimal.

### **2\. Material**

* **Name Property** : Material  
* **One line Guide** : Menentukan Physics Material 2D untuk mengatur tingkat gesekan dan pantulan permukaan objek.  
* **Threeline Guide** :  
  Properti Material memungkinkan Anda memasang aset Physics Material 2D eksternal ke dalam Rigidbody2D guna menentukan karakteristik fisik permukaan objek tersebut saat bersentuhan dengan kolider lain. Tanpa material ini, objek akan menggunakan pengaturan gesekan default dari project setting Unity.  
  Dengan mengatur nilai gesekan (*friction*) dan elastisitas pantulan (*bounciness*), Anda dapat menciptakan berbagai jenis permukaan yang unik seperti es yang licin, bola karet yang memantul tinggi, atau permukaan kasar yang menahan laju objek.  
  Penggunaan material fisik ini menghemat waktu pembuatan skrip karena interaksi pantulan dan gesekan ditangani secara otomatis dan akurat oleh kalkulasi matematis dari mesin fisika bawaan Unity.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Assigned (Ada Aset)** : Objek mengikuti parameter gesekan dan elastisitas pantulan sesuai dengan aturan kustom pada file Physics Material 2D yang dipilih.  
* **2\. When None (Kosong)** : Objek menggunakan nilai default bawaan dari Global Physics 2D Settings yang ada di dalam pengaturan project Unity.

### **3\. Simulated**

* **Name Property** : Simulated  
* **One line Guide** : Mengaktifkan atau menonaktifkan partisipasi objek dalam simulasi fisika secara keseluruhan.  
* **Threeline Guide** :  
  Properti Simulated bertindak sebagai saklar utama untuk menentukan apakah perhitungan gaya, gravitasi, dan tabrakan diproses oleh sistem untuk objek ini. Ketika diaktifkan, objek merespons seluruh hukum fisika yang berlaku di dalam dunia game secara aktif.  
  Menonaktifkan fitur ini sangat berguna untuk menghentikan aktivitas fisik objek secara instan tanpa harus mematikan GameObject-nya, sehingga posisi dan data komponen tetap aman tersimpan di dalam memori scene.  
  Hal ini sering dimanfaatkan untuk optimasi performa pada objek musuh atau item yang berada jauh dari area pandang pemain, serta saat game berada dalam kondisi jeda (*pause*).  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When True** : Objek aktif berpartisipasi penuh dalam simulasi fisika, mendeteksi tabrakan, dan merespons gaya gravitasi.  
* **2\. When False** : Objek dibekukan total dari perhitungan fisika, mengabaikan gravitasi dan tabrakan, namun posisi GameObject tetap dipertahankan.

### **4\. Use Auto Mass**

* **Name Property** : Use Auto Mass  
* **One line Guide** : Menghitung massa objek secara otomatis berdasarkan ukuran dan area dari Collider 2D yang terpasang.  
* **Threeline Guide** :  
  Properti Use Auto Mass membebaskan pengembang dari keharusan menebak-nebak atau memasukkan nilai massa secara manual dengan cara mengkalkulasikannya langsung dari luas area bentuk kolider. Semakin besar ukuran kolider, semakin besar pula massa otomatis yang diberikan pada objek tersebut.  
  Fitur ini membantu menjaga konsistensi fisik antar objek di dalam game, memastikan bahwa objek yang secara visual besar dan berat benar-benar memiliki massa yang lebih besar dibanding objek kecil.  
  Meskipun demikian, Anda tetap dapat menonaktifkannya jika ingin menentukan bobot khusus yang tidak sesuai dengan ukuran visual kolider demi kebutuhan desain gameplay tertentu.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When True** : Nilai massa dihitung otomatis oleh Unity berdasarkan luas area dari komponen Collider 2D yang melekat.  
* **2\. When False** : Nilai massa dilepaskan dari kalkulasi otomatis dan diatur secara manual melalui kolom properti Mass.

### **5\. Mass**

* **Name Property** : Mass  
* **One line Guide** : Menentukan bobot atau berat massa objek yang mempengaruhi kekuatan dorongan saat bertabrakan.  
* **Threeline Guide** :  
  Properti Mass menentukan seberapa berat objek tersebut dalam perhitungan momentum dan hukum tumbukan fisik. Objek dengan massa yang lebih besar akan lebih sulit didorong dan akan memberikan dampak tumbukan yang lebih kuat ke objek bermassa lebih kecil.  
  Nilai massa ini sangat penting dalam mendesain interaksi fisik yang realistis, seperti tabrakan antara kendaraan berat dengan karakter kecil atau interaksi antar objek puzzle.  
  Perlu dicatat bahwa massa tidak mempengaruhi kecepatan jatuh akibat gravitasi secara langsung di Unity, melainkan hanya mempengaruhi cara objek merespons gaya dorong dan tabrakan dengan benda lain.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When High Value (Contoh: 10+)** : Objek menjadi sangat berat, sulit didorong, dan akan memantulkan objek lain dengan tenaga benturan yang besar.  
* **2\. When Low Value (Contoh: 0.1)** : Objek sangat ringan dan mudah terpental jauh saat bersentuhan atau ditabrak oleh benda lain.

### **6\. Linear Damping**

* **Name Property** : Linear Damping  
* **One line Guide** : Mengatur hambatan pergerakan linier untuk memperlambat laju geseran objek seiring waktu.  
* **Threeline Guide** :  
  Properti Linear Damping (sebelumnya Linear Drag) berfungsi sebagai hambatan udara yang melawan pergerakan translasi objek saat meluncur. Semakin besar nilai damping, semakin cepat objek kehilangan momentum dan berhenti bergerak ketika dorongan dihentikan.  
  Fitur ini sangat berguna untuk mencegah objek meluncur tanpa henti di ruang terbuka, memberikan kontrol redaman kecepatan yang natural dan stabil.  
  Pengembang sering memodifikasinya untuk mensimulasikan lingkungan bertekanan seperti dalam air atau cairan kental, sehingga gerakan objek terasa lebih berat dan terkontrol.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When 0** : Objek meluncur bebas tanpa hambatan udara sama sekali, kecepatan akan terus bertahan jika tidak ada gaya luar.  
* **2\. When High Value** : Objek akan langsung mengerem dan berhenti dengan cepat begitu gaya dorong pada objek dihentikan.

### **7\. Angular Damping**

* **Name Property** : Angular Damping  
* **One line Guide** : Mengatur hambatan rotasi untuk memperlambat kecepatan putaran objek seiring waktu.  
* **Threeline Guide** :  
  Properti Angular Damping (sebelumnya Angular Drag) menentukan seberapa cepat objek meredam putaran rotasinya saat berputar di porosnya. Nilai yang tinggi akan membuat objek cepat berhenti berputar setelah mendapat gaya putar, sementara nilai nol membuat objek berputar terus menerus.  
  Hal ini sangat penting untuk mencegah objek berputar secara liar atau tidak wajar setelah mengalami benturan sudut yang keras di dalam game.  
  Dengan menyesuaikan nilai ini, Anda dapat menciptakan efek material yang berbeda, seperti benda yang berputar di atas permukaan yang sangat kesat versus permukaan yang sangat licin.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When 0** : Objek yang berputar akan terus berputar di porosnya tanpa henti selamanya karena tidak ada hambatan rotasi.  
* **2\. When High Value** : Putaran objek akan langsung teredam dan berhenti berputar dalam waktu singkat setelah menerima gaya putar.

### **8\. Gravity Scale**

* **Name Property** : Gravity Scale  
* **One line Guide** : Mengatur pengali kekuatan gravitasi dunia yang mempengaruhi kecepatan jatuh objek 2D.  
* **Threeline Guide** :  
  Properti Gravity Scale menentukan seberapa besar tarikan gravitasi dirasakan oleh Rigidbody2D ini secara spesifik tanpa mengubah gravitasi global proyek. Hal ini memungkinkan objek tertentu jatuh lebih cepat atau melayang lebih lambat dibanding objek lainnya.  
  Pengaturan ini sangat krusial dalam pembuatan game platformer untuk memberikan rasa kontrol lompatan (*jump feel*) yang pas dan responsif bagi pemain.  
  Nilai negatif bahkan dapat digunakan jika Anda ingin membuat objek tersebut jatuh ke atas alih-alih ke bawah, menciptakan mekanik gravitasi terbalik yang unik.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Positive (Contoh: 1\)** : Objek jatuh normal mengikuti tarikan gravitasi dunia Unity secara penuh.  
* **2\. When Zero (0)** : Objek melayang bebas dan tidak terpengaruh oleh gravitasi sama sekali.  
* **3\. When Negative (Contoh: \-1)** : Objek akan jatuh ke arah atas (terbalik melawan gravitasi normal dunia game).

### **9\. Collision Detection**

* **Name Property** : Collision Detection  
* **One line Guide** : Menentukan mode deteksi tabrakan untuk mencegah objek cepat menembus objek lain (*tunneling*).  
* **Threeline Guide** :  
  Properti Collision Detection mengatur seberapa teliti mesin fisika melacak tabrakan objek, dengan pilihan mode seperti Discrete dan Continuous. Mode Discrete mendeteksi tabrakan pada posisi akhir frame, yang efisien tetapi rentan meloloskan objek cepat.  
  Mode Continuous melakukan prediksi lintasan secara lebih detail di antara frame untuk mencegah objek dengan kecepatan sangat tinggi menembus dinding tipis atau objek statis di depannya.  
  Pemilihan mode ini harus disesuaikan dengan kebutuhan performa dan kecepatan gerak objek, di mana mode Continuous memakan lebih banyak sumber daya CPU.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Discrete** : Mode standar dan ringan, mengecek tabrakan setiap akhir frame (cocok untuk objek berkecepatan normal).  
* **2\. When Continuous** : Mode akurat tinggi yang memprediksi lintasan di antara frame, mencegah objek cepat menembus tembok tipis (*tunneling*).

### **10\. Sleeping Mode**

* **Name Property** : Sleeping Mode  
* **One line Guide** : Mengatur perilaku mode tidur (*sleep*) objek untuk menghemat kinerja CPU saat diam.  
* **Threeline Guide** :  
  Properti Sleeping Mode menentukan bagaimana Unity menghentikan perhitungan fisika untuk objek yang sedang diam tidak bergerak guna menghemat sumber daya sistem. Ketika objek tidak menerima gaya apapun dalam waktu tertentu, objek akan masuk ke status tidur (*sleep*).  
  Pilihan seperti Start Awake memastikan objek langsung aktif saat game dimulai, sementara mode lainnya memungkinkan pengoptimalan otomatis untuk objek statis atau pasif di dalam scene.  
  Fitur ini sangat penting untuk menjaga performa game tetap tinggi terutama ketika terdapat ratusan objek diam di dalam layar secara bersamaan.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Start Awake** : Objek langsung aktif dan siap memproses fisika sejak pertama kali scene dimuat.  
* **2\. When Start Asleep** : Objek dimulai dalam keadaan tertidur (tidak memakan proses CPU) sampai ada gaya atau benturan yang membangunkannya.

### **11\. Interpolate**

* **Name Property** : Interpolate  
* **One line Guide** : Menghaluskan pergerakan visual objek antar frame untuk menghilangkan efek patah-patah (*jitter*).  
* **Threeline Guide** :  
  Properti Interpolate membantu mengatasi masalah getaran visual yang sering terjadi pada objek yang bergerak cepat di dalam simulasi fisika. Dengan menggunakan interpolasi (seperti Interpolate atau Extrapolate), posisi objek dihaluskan berdasarkan pergerakan frame sebelumnya.  
  Hal ini membuat kamera yang mengikuti objek atau pergerakan objek itu sendiri terlihat jauh lebih mulus di mata pemain, terutama pada layar dengan *refresh rate* tinggi.  
  Meskipun sedikit menambah beban kalkulasi kecil, fitur ini sangat disarankan untuk objek utama seperti karakter pemain agar tampilannya profesional dan nyaman dilihat.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When None** : Tidak ada penghalusan visual, posisi objek dirender murni berdasarkan hasil kalkulasi frame fisika saat itu.  
* **2\. When Interpolate** : Posisi visual dihaluskan dengan melakukan estimasi berdasarkan pergerakan dari frame sebelumnya.  
* **3\. When Extrapolate** : Posisi visual dihaluskan dengan melakukan estimasi ke depan berdasarkan pergerakan pada frame berikutnya.

### **12\. Constraints**

* **Name Property** : Constraints  
* **One line Guide** : Membatasi pergerakan posisi (X/Y) atau rotasi (Z) objek dari pengaruh fisika eksternal.  
* **Threeline Guide** :  
  Properti Constraints berfungsi sebagai pengunci kaku untuk mencegah Rigidbody2D bergerak di sumbu tertentu atau berputar secara tidak diinginkan. Pada game 2D platformer, misalnya, pembatasan rotasi sumbu Z sangat wajib diaktifkan agar karakter tidak terjungkal atau miring saat menabrak dinding.  
  Anda juga dapat mengunci posisi sumbu X atau Y jika ingin membatasi pergerakan objek hanya pada bidang datar tertentu tanpa harus menuliskan kode pembatas posisi secara manual.  
  Fitur ini memberikan kontrol desainer yang sangat kuat terhadap batasan perilaku fisik objek langsung dari Inspector tanpa mengorbankan sistem fisika dinamis secara keseluruhan.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Freeze Position (X / Y di-centang)** : Objek dikunci mati pada sumbu posisi tersebut sehingga tidak bisa terdorong bergeser ke arah sumbu terkait.  
* **2\. When Freeze Rotation (Z di-centang)** : Objek dikunci rotasinya agar tidak bisa berputar/miring ketika menabrak objek lain (sangat wajib untuk kontrol karakter platformer).

**BoxCollider2D**   
Box Collider 2D adalah *komponen berbentuk kotak tak terlihat yang memberikan batasan fisik pada objek 2 dimensi*. Komponen ini berfungsi seperti dinding padat agar objek tidak saling menembus saat bertabrakan satu sama lain. 

### **1\. Material**

* **Name Property** : Material  
* **One line Guide** : Menentukan Physics Material 2D untuk mengatur tingkat gesekan dan elastisitas pantulan kolider.  
* **Threeline Guide** : Properti Material memungkinkan Anda menyematkan aset Physics Material 2D khusus ke dalam Box Collider 2D untuk mendefinisikan karakteristik permukaan objek saat bersentuhan atau bergesekan dengan objek lain di dalam scene. Tanpa material khusus ini, kotak kolider akan menggunakan pengaturan gesekan default global dari sistem fisika Unity.  
  Dengan mengatur nilai gesekan (*friction*) dan kelentingan (*bounciness*), Anda dapat menciptakan berbagai perilaku fisik yang realistis seperti permukaan es yang sangat licin, dinding kayu biasa, atau kotak karet yang memantul saat dilempar. Hal ini sangat membantu dalam mendesain variasi level permainan yang dinamis.  
  Penggunaan aset material terpisah ini juga sangat efisien karena satu file material yang sama dapat digunakan secara bersamaan oleh banyak kolider berbeda, sehingga memudahkan proses penyesuaian parameter fisika secara serentak tanpa harus mengubahnya satu per satu.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Assigned (Ada Aset)** : Kolider mengikuti parameter gesekan dan elastisitas pantulan sesuai dengan aturan pada file Physics Material 2D yang dipilih.  
* **2\. When None (Kosong)** : Kolider menggunakan nilai default bawaan dari Global Physics 2D Settings yang diatur pada project Unity.

### **2\. Is Trigger**

* **Name Property** : Is Trigger  
* **One line Guide** : Menentukan apakah kotak kolider berfungsi sebagai pemicu kejadian (*trigger*) alih-alih penghalang fisik.  
* **Threeline Guide** : Properti Is Trigger mengubah fungsi kotak kolider dari benda padat yang menahan objek lain menjadi area sensor tak kasat mata. Ketika opsi ini diaktifkan, objek fisik lain dapat menembus atau melewati kotak ini tanpa mengalami benturan fisik atau dorongan apa pun.  
  Meskipun diabaikan oleh mesin benturan fisik, kotak kolider yang bertindak sebagai trigger akan tetap mendeteksi masuk atau keluarnya objek lain melalui fungsi skrip khusus seperti `OnTriggerEnter2D` atau `OnTriggerExit2D`.  
  Fitur ini sangat esensial untuk mendesain berbagai mekanik gameplay penting seperti zona checkpoint, area portal teleportasi, kotak dialog interaktif, pengambilan koin, atau jebakan tersembunyi yang memicu efek saat pemain menginjaknya.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When True** : Kolider menjadi zona pemicu tembus pandang/tembus benda, tidak menahan objek lain, namun memicu fungsi callback skrip saat ada objek yang masuk ke dalamnya.  
* **2\. When False** : Kolider berfungsi sebagai benda padat fisik murni yang menahan dan memantulkan objek lain saat terjadi tabrakan.

### **3\. Used By Effector**

* **Name Property** : Used By Effector  
* **One line Guide** : Menghubungkan kotak kolider agar dapat berinteraksi khusus dengan komponen Effector 2D.  
* **Threeline Guide** : Properti Used By Effector bertindak sebagai jembatan komunikasi antara Box Collider 2D dengan komponen Effector 2D (seperti Area Effector, Buoyancy Effector, Surface Effector, atau Platform Effector) yang melekat pada GameObject yang sama atau induknya.  
  Ketika diaktifkan, kolider tidak hanya mendeteksi tabrakan biasa, tetapi juga tunduk pada gaya dorong, tarikan, arus angin, atau perilaku satu arah (*one-way platform*) yang dihasilkan oleh komponen effector tersebut.  
  Pengaturan ini sangat populer digunakan untuk membuat lantai platform lompat tembus dari bawah, ban berjalan (*conveyor belt*), area pusaran air, atau zona gaya dorong angin tanpa perlu menuliskan baris kode skrip fisika yang rumit secara manual.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When True** : Kolider memproses dan merespons efek gaya khusus yang dipancarkan oleh komponen Effector 2D yang terpasang.  
* **2\. When False** : Kolider mengabaikan keberadaan komponen Effector 2D dan hanya bertindak sebagai kolider atau pemicu standar biasa.

### **4\. Used By Composite**

* **Name Property** : Used By Composite  
* **One line Guide** : Menggabungkan kotak kolider ini ke dalam sistem Composite Collider 2D untuk optimasi geometri.  
* **Threeline Guide** : Properti Used By Composite digunakan untuk menyatukan bentuk kotak kolider ini dengan kolider-kolider tetangga lainnya menggunakan komponen pusat berupa Composite Collider 2D. Hal ini berfungsi untuk mereduksi jumlah total titik singgung benturan di dalam scene secara masif.  
  Penggabungan ini sangat penting untuk mencegah masalah celah garis sambung (*seam issues*) yang sering kali membuat karakter pemain tersangkut secara tidak sengaja di antara sambungan dua kotak lantai yang berdempetan.  
  Dengan mengaktifkannya, mesin fisika Unity memperlakukan kumpulan kotak kolider yang terhubung sebagai satu kesatuan objek geometri yang mulus, sehingga performa game menjadi jauh lebih optimal dan stabil.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When True** : Geometri kotak kolider digabungkan secara otomatis ke dalam komponen Composite Collider 2D untuk membentuk satu kesatuan bidang tabrakan yang efisien.  
* **2\. When False** : Kolider berdiri sendiri sebagai entitas terpisah dan tidak digabungkan dengan komponen komposit apa pun di sekitarnya.

### **5\. Auto Tiling**

* **Name Property** : Auto Tiling  
* **One line Guide** : Menyesuaikan ukuran kotak kolider secara otomatis ketika ukuran Sprite Renderer diubah (*Tiled Mode*).  
* **Threeline Guide** : Properti Auto Tiling sangat berguna ketika Anda menggunakan sprite dengan mode *Draw Mode: Tiled* yang ukurannya sering diubah-ubah di dalam scene. Opsi ini memastikan bahwa kotak kolider ikut meregang atau merapat secara otomatis mengikuti perubahan dimensi visual sprite tersebut.  
  Fitur ini menyelamatkan pengembang dari keharusan memperbarui ukuran kolider secara manual lewat Inspector setiap kali aset gambar latar belakang atau platform diubah skalanya.  
  Hal ini sangat mempercepat proses *level design*, terutama dalam menyusun dinding, ubin lantai, atau pembatas koridor yang berulang-ulang dengan panjang yang bervariasi.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When True** : Ukuran dan bentuk Box Collider 2D akan otomatis menyesuaikan diri secara real-time mengikuti perubahan dimensi sprite yang berulang (*tiled*).  
* **2\. When False** : Ukuran kotak kolider bersifat statis independen dan tidak akan berubah secara otomatis meskipun ukuran visual sprite diubah.

### **6\. Offset**

* **Name Property** : Offset  
* **One line Guide** : Mengatur pergeseran titik pusat posisi kotak kolider relatif terhadap titik pusat GameObject.  
* **Threeline Guide** : Properti Offset memungkinkan Anda memindahkan posisi pusat kotak kolider ke arah sumbu X atau Y lokal tanpa harus menggeser posisi GameObject utamanya. Hal ini sangat berguna jika bentuk grafis sprite tidak berada tepat di tengah titik poros asalnya.  
  Dengan menyesuaikan nilai offset, Anda dapat menyelaraskan letak kotak kolider agar pas menutupi bagian tubuh karakter atau objek tertentu yang bentuk visualnya tidak simetris terhadap titik tengah.  
  Pengaturan ini memberikan fleksibilitas tinggi dalam penataan tata letak fisik tanpa perlu merusak struktur hierarki atau posisi pivot asli dari aset gambar sprite yang digunakan.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Zero (0, 0\)** : Titik pusat kotak kolider berada tepat di tengah-tengah titik poros (*pivot*) GameObject.  
* **2\. When Offset Value Set** : Titik pusat kotak kolider bergeser sejauh nilai koordinat X dan Y yang dimasukkan dari titik pusat aslinya.

### **7\. Size**

* **Name Property** : Size  
* **One line Guide** : Menentukan lebar dan tinggi dari kotak kolider dalam satuan ruang lokal (*local space*).  
* **Threeline Guide** : Properti Size adalah parameter utama yang menentukan dimensi ukuran fisik dari kotak kolider (lebar sumbu X dan tinggi sumbu Y). Nilai ini diatur dalam satuan unit Unity dan menentukan seberapa besar area bidang kotak yang akan mendeteksi benturan.  
  Anda dapat menyesuaikan ukuran ini agar pas menutupi area sprite atau objek game tertentu, baik secara manual melalui kolom angka di Inspector maupun secara visual langsung menggunakan alat *Edit Collider* di Scene View.  
  Ketepatan ukuran size sangat krusial agar interaksi gameplay terasa adil bagi pemain, menghindari celah di mana objek terlihat bertabrakan secara visual padahal kolidernya terlalu kecil atau sebaliknya.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Adjusted to Sprite** : Kotak kolider menutupi seluruh area visual objek dengan pas, memberikan deteksi benturan yang akurat dan natural.  
* **2\. When Too Large/Small** : Objek dapat mengalami masalah aneh seperti melayang di udara sebelum menyentuh lantai atau menembus batas visual karena ukuran kolider tidak sesuai dengan gambarnya.

### **8\. Edge Radius**

* **Name Property** : Edge Radius  
* **One line Guide** : Memberikan radius lengkungan pada sudut-sudut kotak kolider untuk menghindari benturan tajam.  
* **Threeline Guide** : Properti Edge Radius menambahkan selimut radius melingkar di sekeliling tepi dan sudut siku-siku kotak kolider, mengubah bentuk kotaknya menjadi sedikit membulat di bagian ujung-ujungnya. Nilai default-nya adalah 0 untuk sudut siku-siku tajam sempurna.  
  Penambahan radius sudut ini sangat efektif untuk mencegah karakter pemain tersangkut atau 'nyangkut' secara tiba-tiba pada celah siku-siku lantai atau dinding saat berjalan menyusuri permukaan datar.  
  Dengan sudut yang membulat, gerakan geseran (*sliding*) karakter di sepanjang dinding atau lantai akan terasa jauh lebih mulus tanpa hambatan sudut mati yang mengganggu alur permainan.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When 0** : Sudut kotak kolider berbentuk siku-siku tajam sempurna tanpa ada lengkungan radius tambahan di bagian tepinya.  
* **2\. When Positive Value (Contoh: 0.1+)** : Sudut dan tepi kotak kolider menjadi tumpul melingkar, membuat objek lebih mulus meluncur melewati celah atau sudut tajam.

**Transform** 

### **1\. Position**

* **Name Property** : Position  
* **One line Guide** : Menentukan koordinat letak GameObject di dalam ruang koordinat dunia (*world space*) atau lokal (*local space*).  
* **Threeline Guide** : Properti Position mengatur letak koordinat suatu GameObject pada sumbu X (horizontal), Y (vertikal), dan Z (kedalaman) di dalam scene Unity. Setiap objek yang ada di dalam game wajib memiliki komponen Transform, menjadikannya titik acuan paling fundamental untuk menentukan di mana objek tersebut berada.  
  Nilai posisi ini dapat diubah langsung melalui Inspector, digeser secara visual menggunakan *Move Tool* di Scene View, atau dimanipulasi secara dinamis menggunakan skrip C\# untuk menciptakan pergerakan karakter, perpindahan kamera, maupun proses *spawn* objek.  
  Pemahaman tentang perbedaan antara posisi dunia (*World Position*) dan posisi lokal (*Local Position* terhadap parent-nya) sangat penting terutama ketika Anda bekerja dengan objek bertingkat (*hierarchy*), agar kalkulasi pergerakan bagian anak tetap akurat dan tidak melenceng.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Positive Value (X/Y/Z naik)** : Objek bergeser ke arah kanan, atas, atau maju menjauhi titik pusat koordinat acuan.  
* **2\. When Negative Value (X/Y/Z turun)** : Objek bergeser ke arah kiri, bawah, atau mundur mendekati titik pusat koordinat acuan.  
* **3\. When Zero (0, 0, 0\)** : Objek berada tepat di titik pusat absolut (*Origin*) dari koordinat referensinya.

### **2\. Rotation**

* **Name Property** : Rotation  
* **One line Guide** : Menentukan sudut kemiringan atau arah hadap GameObject pada sumbu X, Y, dan Z dalam satuan derajat.  
* **Threeline Guide** : Properti Rotation mengatur orientasi sudut putar dari GameObject menggunakan sistem sudut Euler (*Euler Angles*) pada sumbu X (Pitch), Y (Yaw), dan Z (Roll). Pengaturan ini menentukan ke arah mana objek tersebut menghadap atau bagaimana ia dimiringkan di dalam ruang game.  
  Di balik layar, Unity menyimpan nilai rotasi ini dalam bentuk *Quaternion* untuk menghindari masalah matematis kompleks seperti *gimbal lock*, namun panel Inspector menyajikannya dalam format sudut derajat biasa agar lebih mudah dibaca dan diatur oleh pengembang.  
  Rotasi sangat krusial dalam mengatur arah pandang kamera, arah tembakan peluru, kemiringan kendaraan, atau animasi elemen UI agar tampil selaras dengan narasi dan mekanik gameplay yang diinginkan.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Zero (0, 0, 0\)** : Objek berada dalam orientasi awal normal tanpa ada kemiringan atau putaran sudut sama sekali.  
* **2\. When Changed on Z-Axis** : Objek berputar secara dua dimensi (memutar bidang layar), sangat umum digunakan untuk game 2D seperti rotasi karakter atau arah tembakan.  
* **3\. When Changed on X/Y-Axis** : Objek berputar secara tiga dimensi, mengubah arah hadap ke samping, atas, atau bawah di ruang 3D.

### **3\. Scale**

* **Name Property** : Scale  
* **One line Guide** : Menentukan ukuran besar atau kecilnya proporsi GameObject pada sumbu X, Y, dan Z.  
* **Threeline Guide** : Properti Scale mengontrol pembesaran atau pengecilan ukuran visual dan fisik dari suatu GameObject secara relatif terhadap ukuran aslinya. Nilai default untuk setiap sumbunya adalah 1, yang berarti objek ditampilkan dalam ukuran normal penuh tanpa modifikasi skala.  
  Anda dapat memperbesar objek dengan memasukkan nilai lebih dari 1, atau mengecilkannya menggunakan nilai desimal di antara 0 dan 1\. Selain itu, memberikan nilai negatif (-1) dapat digunakan untuk membalikkan (*flip*) sprite atau model secara instan.  
  Perubahan skala pada objek induk (*parent*) akan secara otomatis mewariskan efek ukuran tersebut ke semua objek anak (*child*) di bawahnya, sehingga sangat memudahkan dalam mengatur ukuran kelompok aset secara serentak dalam satu hirarki.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Value is 1** : Objek dirender menggunakan ukuran proporsi standar aslinya tanpa pembesaran atau pengecilan.  
* **2\. When Value \> 1 (Contoh: 2\)** : Ukuran objek diperbesar dua kali lipat dari ukuran standarnya pada sumbu terkait.  
* **3\. When Value \< 1 (Contoh: 0.5)** : Ukuran objek diperkecil menjadi setengah dari ukuran standarnya.  
* **4\. When Negative Value (Contoh: \-1)** : Ukuran objek dibalik arahnya (*flipped*), sering digunakan untuk membalik arah hadap sprite karakter ke kiri atau kanan secara instan.

**Canvas**

Canvas adalah komponen utama yang digunakan untuk menampilkan antarmuka pengguna (*User Interface* atau UI) di dalam sebuah Scene. Canvas menjadi area referensi bagi elemen UI seperti Button, Text, Image, dan Panel agar dapat dirender dengan posisi serta urutan tampilan yang sesuai.

### **1\. Property**

* **Name Property** : Render Mode  
* **One line Guide** : Menentukan cara Canvas dirender terhadap kamera dan ruang dunia.  
* **Threeline Guide** : Render Mode menentukan hubungan Canvas dengan kamera serta posisi elemen UI saat dirender. Mode Screen Space - Overlay menampilkan UI langsung di atas seluruh tampilan tanpa memerlukan Camera. Mode Screen Space - Camera merender UI melalui Camera tertentu, sedangkan World Space menempatkan Canvas sebagai objek biasa di dalam ruang 3D.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Screen Space \\- Overlay** : UI selalu ditampilkan di atas objek lain dan mengikuti ukuran layar.  
* **2\. When Screen Space \\- Camera** : UI dirender melalui Camera yang dipilih dan dapat dipengaruhi oleh pengaturan kamera.  
* **3\. When World Space** : Canvas berada di dalam ruang dunia dan dapat diposisikan, diputar, atau diskalakan seperti GameObject biasa.

* **Name Property** : Pixel Perfect  
* **One line Guide** : Membuat elemen UI dirender lebih tajam dengan menyesuaikan posisi piksel.  
* **Threeline Guide** : Pixel Perfect digunakan untuk menjaga agar elemen UI tetap terlihat tajam, terutama pada tampilan 2D atau UI yang menggunakan sprite berukuran kecil. Saat diaktifkan, posisi elemen UI disesuaikan agar lebih tepat berada pada grid piksel layar.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When True** : Elemen UI dirender dengan penyesuaian piksel sehingga garis dan sprite terlihat lebih tajam.  
* **2\. When False** : Elemen UI mengikuti perhitungan posisi normal dan dapat terlihat lebih halus pada beberapa resolusi.

* **Name Property** : Render Camera  
* **One line Guide** : Menentukan Camera yang digunakan Canvas pada mode Screen Space - Camera.  
* **Threeline Guide** : Render Camera menjadi acuan untuk merender Canvas ketika Render Mode diatur ke Screen Space - Camera. Camera ini memengaruhi posisi, perspektif, dan jarak tampilan UI terhadap layar. Tetapkan Camera secara eksplisit agar hasil rendering UI tetap konsisten.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Camera Assigned** : Canvas dirender melalui Camera yang dipilih.  
* **2\. When Camera is Empty** : Canvas tidak memiliki Camera khusus dan dapat menggunakan Camera utama sesuai kondisi Scene.

* **Name Property** : Plane Distance  
* **One line Guide** : Mengatur jarak Canvas dari Render Camera pada mode Screen Space - Camera.  
* **Threeline Guide** : Plane Distance menentukan jarak bidang Canvas dari Camera yang digunakan untuk merender UI. Nilai yang lebih kecil menempatkan Canvas lebih dekat ke Camera, sedangkan nilai yang lebih besar menempatkannya lebih jauh. Pengaturan ini dapat memengaruhi urutan kedalaman UI terhadap objek lain.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Low Value** : Canvas berada lebih dekat dengan Camera.  
* **2\. When High Value** : Canvas berada lebih jauh dari Camera.

* **Name Property** : Sorting Layer  
* **One line Guide** : Menentukan Sorting Layer untuk mengatur prioritas tampilan Canvas.  
* **Threeline Guide** : Sorting Layer digunakan untuk mengelompokkan Canvas berdasarkan lapisan render. Canvas pada Sorting Layer yang lebih tinggi akan dirender di atas Canvas pada layer yang lebih rendah. Fitur ini berguna untuk memisahkan UI latar belakang, UI utama, dan UI notifikasi.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Higher Sorting Layer** : Canvas memiliki prioritas tampilan yang lebih tinggi.  
* **2\. When Lower Sorting Layer** : Canvas dirender di bawah Canvas pada layer yang lebih tinggi.

* **Name Property** : Order in Layer  
* **One line Guide** : Mengatur urutan Canvas di dalam Sorting Layer yang sama.  
* **Threeline Guide** : Order in Layer menentukan prioritas render ketika beberapa Canvas menggunakan Sorting Layer yang sama. Nilai yang lebih besar membuat Canvas dirender di atas Canvas dengan nilai yang lebih kecil. Gunakan properti ini untuk mengatur urutan panel, dialog, tooltip, atau notifikasi.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Higher Value** : Canvas ditampilkan di atas Canvas dengan nilai yang lebih rendah.  
* **2\. When Lower Value** : Canvas ditampilkan di bawah Canvas dengan nilai yang lebih tinggi.

* **Name Property** : Target Display  
* **One line Guide** : Menentukan layar tujuan untuk menampilkan Canvas.  
* **Threeline Guide** : Target Display digunakan ketika aplikasi menampilkan permainan pada lebih dari satu monitor atau display. Canvas dirender ke display yang dipilih berdasarkan nomor display yang tersedia. Pada penggunaan umum dengan satu monitor, gunakan Display 1 agar UI tampil pada layar utama.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Display 1** : Canvas ditampilkan pada display utama.  
* **2\. When Other Display** : Canvas ditampilkan pada display lain sesuai nomor yang dipilih.

* **Name Property** : Additional Shader Channels  
* **One line Guide** : Menentukan data tambahan yang dikirim Canvas ke shader saat merender elemen UI.  
* **Threeline Guide** : Additional Shader Channels menyediakan data vertex tambahan seperti Normal, Tangent, atau TexCoord untuk kebutuhan shader tertentu. Aktifkan hanya channel yang benar-benar digunakan oleh material atau shader pada elemen UI agar penggunaan data tetap efisien.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Channel Enabled** : Data channel dikirim ke shader sehingga efek visual yang membutuhkannya dapat bekerja.  
* **2\. When Channel Disabled** : Data channel tidak dikirim dan shader yang bergantung pada data tersebut dapat menampilkan hasil yang berbeda.

**Canvas Group**

Canvas Group digunakan untuk mengatur tampilan, interaksi, dan penerimaan raycast dari sekumpulan elemen UI secara bersamaan. Komponen ini dapat dipasang pada parent sehingga pengaturannya ikut memengaruhi seluruh child UI di dalam hierarkinya.

### **1\. Property**

* **Name Property** : Alpha  
* **One line Guide** : Mengatur tingkat transparansi Canvas Group dan seluruh elemen UI di dalamnya.  
* **Threeline Guide** : Alpha memiliki nilai dari 0 sampai 1 dan digunakan untuk mengatur seberapa terlihat elemen UI. Nilai ini diterapkan secara berantai kepada child, sehingga seluruh kelompok UI dapat dibuat transparan tanpa mengubah warna setiap elemen. Nilai Alpha tidak menonaktifkan interaksi secara otomatis, sehingga gunakan Interactable atau Blocks Raycasts jika UI juga perlu dibuat tidak dapat digunakan.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Value 1** : Elemen UI ditampilkan dengan tingkat transparansi penuh.  
* **2\. When Value 0** : Elemen UI menjadi sepenuhnya transparan.  
* **3\. When Value Between 0 and 1** : Elemen UI ditampilkan secara transparan sesuai nilai Alpha.

* **Name Property** : Interactable  
* **One line Guide** : Menentukan apakah elemen UI di dalam Canvas Group dapat menerima interaksi pengguna.  
* **Threeline Guide** : Interactable mengatur kemampuan komponen UI seperti Button, Toggle, atau Slider untuk merespons input. Ketika dinonaktifkan, elemen UI tetap dapat terlihat tetapi tidak dapat digunakan oleh pengguna. Pengaturan ini juga memengaruhi child Canvas Group selama Ignore Parent Groups tidak diaktifkan.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When True** : Elemen UI dapat merespons interaksi seperti klik, sentuhan, atau navigasi.  
* **2\. When False** : Elemen UI tidak dapat digunakan meskipun masih terlihat di layar.

* **Name Property** : Blocks Raycasts  
* **One line Guide** : Menentukan apakah Canvas Group menghalangi raycast dari pointer atau input pengguna.  
* **Threeline Guide** : Blocks Raycasts digunakan untuk mengatur apakah raycast berhenti pada area elemen UI di dalam Canvas Group. Fitur ini berguna untuk mencegah input menembus panel, modal dialog, atau overlay ke UI yang berada di belakangnya. Ketika dinonaktifkan, raycast dapat melewati Canvas Group dan menemukan elemen UI lain di bawahnya.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When True** : Canvas Group menghalangi raycast sehingga UI di belakangnya tidak menerima input.  
* **2\. When False** : Raycast dapat melewati Canvas Group menuju elemen UI lain.

* **Name Property** : Ignore Parent Groups  
* **One line Guide** : Menentukan apakah Canvas Group mengabaikan pengaturan Canvas Group dari parent.  
* **Threeline Guide** : Ignore Parent Groups membuat komponen ini tidak mengikuti pengaruh Interactable, Blocks Raycasts, dan Alpha dari Canvas Group parent. Pengaturan lokal Canvas Group tetap dapat diterapkan pada GameObject dan child-nya. Gunakan fitur ini ketika sebuah kelompok UI perlu memiliki perilaku interaksi yang berbeda dari parent-nya.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When True** : Pengaturan Canvas Group parent diabaikan oleh Canvas Group ini.  
* **2\. When False** : Pengaturan Canvas Group parent diteruskan dan digabungkan dengan pengaturan Canvas Group ini.

**AudioSource**

AudioSource adalah komponen yang digunakan untuk memutar Audio Clip di dalam Scene. AudioSource dapat digunakan untuk suara latar, efek suara, dialog, dan suara 3D yang berasal dari posisi GameObject tertentu.

### **1\. Property**

* **Name Property** : Audio Clip  
* **One line Guide** : Menentukan file audio yang akan diputar oleh AudioSource.  
* **Threeline Guide** : Audio Clip adalah sumber suara yang digunakan oleh AudioSource. Clip dapat berisi musik, efek suara, dialog, atau jenis audio lain yang didukung oleh Unity. Pastikan Audio Clip sudah ditambahkan ke project sebelum digunakan agar AudioSource dapat memutarnya.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Clip Assigned** : AudioSource memiliki sumber audio yang dapat diputar.  
* **2\. When Clip Empty** : AudioSource tidak menghasilkan suara karena tidak memiliki Audio Clip.

* **Name Property** : Output  
* **One line Guide** : Menentukan Audio Mixer Group tujuan untuk suara AudioSource.  
* **Threeline Guide** : Output digunakan untuk mengirim suara ke Audio Mixer Group tertentu. Dengan Audio Mixer, volume dan efek suara dapat diatur secara terpusat berdasarkan kelompok seperti Music, SFX, atau Voice. Jika tidak diisi, suara akan dikirim langsung ke output audio utama.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Audio Mixer Group Assigned** : Suara diproses oleh Audio Mixer Group yang dipilih.  
* **2\. When Output Empty** : Suara dikirim ke output audio utama.

* **Name Property** : Mute  
* **One line Guide** : Membisukan suara AudioSource tanpa menghapus Audio Clip.  
* **Threeline Guide** : Mute menghentikan suara agar tidak terdengar oleh pengguna, tetapi AudioSource tetap aktif dan status pemutarannya tetap berjalan. Fitur ini berguna untuk membuat opsi mute atau menonaktifkan suara sementara. Gunakan Mute ketika suara perlu dimatikan tanpa mengubah pengaturan Volume.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When True** : Suara tidak terdengar.  
* **2\. When False** : Suara dapat terdengar sesuai pengaturan Volume dan efek audio lainnya.

* **Name Property** : Play On Awake  
* **One line Guide** : Menentukan apakah AudioSource langsung diputar saat GameObject aktif.  
* **Threeline Guide** : Play On Awake membuat AudioSource mulai memutar Audio Clip ketika Scene dimuat atau GameObject diaktifkan. Nonaktifkan properti ini jika suara perlu dimulai melalui script atau event tertentu. Properti ini sering digunakan bersama Loop untuk membuat musik latar yang berjalan otomatis.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When True** : AudioSource mulai memutar suara secara otomatis.  
* **2\. When False** : AudioSource menunggu perintah Play dari script atau sistem lain.

* **Name Property** : Loop  
* **One line Guide** : Menentukan apakah Audio Clip diputar berulang kali setelah selesai.  
* **Threeline Guide** : Loop digunakan untuk suara yang harus terus berjalan, seperti musik latar, ambience, atau suara mesin. Ketika aktif, Audio Clip akan kembali ke awal setelah mencapai bagian akhir. Ketika nonaktif, AudioSource berhenti setelah satu kali pemutaran selesai.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When True** : Audio Clip diputar berulang kali sampai AudioSource dihentikan.  
* **2\. When False** : Audio Clip hanya diputar satu kali.

* **Name Property** : Volume  
* **One line Guide** : Mengatur tingkat keras suara AudioSource.  
* **Threeline Guide** : Volume menentukan intensitas suara yang dihasilkan AudioSource sebelum diproses oleh Audio Mixer. Nilai 0 membuat suara tidak terdengar, sedangkan nilai 1 menggunakan tingkat volume penuh dari AudioSource. Volume dapat diubah saat runtime untuk membuat efek fade in, fade out, atau pengaturan volume dinamis.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Value 0** : Suara AudioSource tidak terdengar.  
* **2\. When Value 1** : Suara diputar dengan tingkat volume penuh.  
* **3\. When Value Between 0 and 1** : Suara diputar dengan tingkat volume yang lebih rendah.

* **Name Property** : Pitch  
* **One line Guide** : Mengatur kecepatan pemutaran dan tinggi rendah nada Audio Clip.  
* **Threeline Guide** : Pitch memengaruhi kecepatan suara sekaligus tinggi rendah nadanya. Nilai standar 1 memutar audio dengan kecepatan dan nada asli. Nilai lebih rendah membuat suara lebih lambat dan rendah, sedangkan nilai lebih tinggi membuat suara lebih cepat dan tinggi.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Value 1** : Audio Clip diputar dengan kecepatan dan nada asli.  
* **2\. When Value Less Than 1** : Audio Clip diputar lebih lambat dengan nada lebih rendah.  
* **3\. When Value Greater Than 1** : Audio Clip diputar lebih cepat dengan nada lebih tinggi.

* **Name Property** : Spatial Blend  
* **One line Guide** : Menentukan perpaduan suara 2D dan suara 3D berdasarkan posisi AudioSource.  
* **Threeline Guide** : Spatial Blend mengatur seberapa besar suara dipengaruhi oleh posisi AudioSource terhadap listener. Nilai 0 menghasilkan suara 2D yang tidak dipengaruhi jarak, sedangkan nilai 1 menghasilkan suara 3D yang dipengaruhi posisi dan jarak. Nilai di antara keduanya menghasilkan perpaduan suara 2D dan 3D.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Value 0** : Suara bersifat 2D dan terdengar relatif sama dari seluruh posisi.  
* **2\. When Value 1** : Suara bersifat 3D dan dipengaruhi posisi AudioSource serta AudioListener.  
* **3\. When Value Between 0 and 1** : Suara menggunakan perpaduan karakteristik 2D dan 3D.

* **Name Property** : Priority  
* **One line Guide** : Menentukan prioritas AudioSource ketika jumlah suara yang diputar melebihi batas sistem.  
* **Threeline Guide** : Priority digunakan oleh Unity untuk menentukan suara mana yang lebih penting ketika banyak AudioSource aktif secara bersamaan. Nilai yang lebih rendah memiliki prioritas lebih tinggi. Gunakan prioritas tinggi untuk dialog atau suara penting, dan prioritas lebih rendah untuk ambience yang dapat dihentikan lebih dahulu.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Lower Value** : AudioSource lebih diprioritaskan untuk tetap diputar.  
* **2\. When Higher Value** : AudioSource lebih mudah dihentikan ketika batas suara aktif tercapai.

* **Name Property** : Bypass Effects, Bypass Listener Effects, dan Bypass Reverb Zones  
* **One line Guide** : Menentukan apakah AudioSource melewati efek audio tertentu saat diproses.  
* **Threeline Guide** : Bypass Effects melewati efek yang dipasang pada AudioSource. Bypass Listener Effects melewati efek yang dipasang pada AudioListener, sedangkan Bypass Reverb Zones melewati pengaruh Reverb Zone. Gunakan properti ini untuk suara yang harus tetap bersih atau tidak boleh dipengaruhi efek ruang tertentu.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Enabled** : AudioSource melewati efek audio yang sesuai.  
* **2\. When Disabled** : AudioSource diproses oleh efek audio yang aktif.

* **Name Property** : 3D Sound Settings  
* **One line Guide** : Mengatur perilaku suara 3D berdasarkan jarak dan posisi AudioSource.  
* **Threeline Guide** : Pengaturan suara 3D mencakup Doppler Level, Spread, Volume Rolloff, Min Distance, dan Max Distance. Doppler Level mengatur perubahan nada akibat pergerakan, Spread mengatur sebaran suara, sedangkan Min Distance dan Max Distance mengatur area pengaruh suara. Pengaturan ini hanya memberikan efek penuh ketika Spatial Blend menggunakan suara 3D.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Near Min Distance** : Suara terdengar lebih kuat dan berada pada tingkat volume utama.  
* **2\. When Beyond Max Distance** : Suara melemah sesuai Volume Rolloff dan dapat menjadi sangat kecil atau tidak terdengar.  
* **3\. When Doppler Level Greater Than 0** : Pergerakan AudioSource dan AudioListener dapat memengaruhi perubahan nada suara.

**Camera**

Camera adalah komponen yang digunakan untuk melihat dan merender dunia game ke layar. Camera menentukan area Scene yang terlihat, objek yang dirender, perspektif, serta cara hasil render ditampilkan kepada pemain.

### **1\. Property**

* **Name Property** : Clear Flags  
* **One line Guide** : Menentukan cara Camera membersihkan tampilan sebelum merender frame baru.  
* **Threeline Guide** : Clear Flags mengatur isi area tampilan yang tidak tertutup oleh objek. Camera dapat menggunakan warna solid, Skybox, atau tampilan dari Camera lain sesuai kebutuhan Scene. Pengaturan ini membantu membuat latar belakang dunia, langit, atau efek layering Camera.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Skybox** : Area kosong diisi menggunakan Skybox yang aktif.  
* **2\. When Solid Color** : Area kosong diisi dengan warna yang dipilih.  
* **3\. When Depth Only** : Camera hanya merender objek tanpa membersihkan isi tampilan sebelumnya.

* **Name Property** : Projection  
* **One line Guide** : Menentukan apakah Camera menggunakan perspektif atau proyeksi ortografis.  
* **Threeline Guide** : Projection menentukan cara ukuran objek berubah berdasarkan jaraknya dari Camera. Perspective membuat objek yang lebih jauh terlihat lebih kecil seperti pandangan nyata. Orthographic tidak menggunakan perspektif sehingga ukuran objek tetap terlihat sama meskipun jaraknya berubah.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Perspective** : Objek yang jauh terlihat lebih kecil dan bidang pandang menggunakan perspektif.  
* **2\. When Orthographic** : Objek tidak mengecil berdasarkan jarak dan cocok untuk game 2D atau tampilan strategis.

* **Name Property** : Field of View  
* **One line Guide** : Mengatur lebar area pandang Camera pada mode Perspective.  
* **Threeline Guide** : Field of View atau FOV menentukan seberapa luas area dunia yang terlihat oleh Camera. Nilai yang lebih besar menampilkan area yang lebih luas, sedangkan nilai yang lebih kecil membuat tampilan lebih sempit dan terasa lebih dekat. Properti ini tidak digunakan sebagai pengatur perspektif saat Projection menggunakan Orthographic.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Low Value** : Area pandang lebih sempit dan objek terlihat lebih dekat.  
* **2\. When High Value** : Area pandang lebih luas dan lebih banyak objek dapat terlihat.

* **Name Property** : Culling Mask  
* **One line Guide** : Menentukan Layer mana yang dirender oleh Camera.  
* **Threeline Guide** : Culling Mask digunakan untuk memilih kelompok Layer yang boleh dirender oleh Camera. Objek pada Layer yang tidak dipilih tidak akan terlihat melalui Camera tersebut. Fitur ini berguna untuk membuat Camera khusus seperti minimap, UI, efek visual, atau sistem split screen.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Layer Selected** : Objek pada Layer tersebut dapat dirender oleh Camera.  
* **2\. When Layer Deselected** : Objek pada Layer tersebut tidak dirender oleh Camera.

* **Name Property** : Clipping Planes  
* **One line Guide** : Menentukan jarak terdekat dan terjauh objek yang dapat dirender Camera.  
* **Threeline Guide** : Clipping Planes terdiri dari Near dan Far. Near menentukan batas minimum jarak objek dari Camera, sedangkan Far menentukan batas maksimum jarak objek yang masih dapat dirender. Pengaturan yang tepat dapat membantu menghindari objek yang tidak terlihat dan mengurangi beban rendering.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Before Near Distance** : Objek berada terlalu dekat dan tidak dirender.  
* **2\. When Between Near and Far** : Objek berada dalam rentang pandang dan dapat dirender.  
* **3\. When Beyond Far Distance** : Objek berada terlalu jauh dan tidak dirender.

* **Name Property** : Viewport Rect  
* **One line Guide** : Menentukan bagian layar yang digunakan Camera untuk menampilkan hasil render.  
* **Threeline Guide** : Viewport Rect menggunakan nilai relatif untuk menentukan posisi dan ukuran area Camera pada layar. Nilai ini dapat digunakan untuk membuat split screen, minimap, atau tampilan Camera kecil di sudut layar. Area viewport ditentukan oleh X, Y, W, dan H.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Full Screen** : Camera menggunakan seluruh area layar.  
* **2\. When Partial Screen** : Camera hanya menggunakan bagian layar sesuai ukuran dan posisi viewport.

* **Name Property** : Depth  
* **One line Guide** : Menentukan urutan Camera saat beberapa Camera merender ke layar yang sama.  
* **Threeline Guide** : Depth digunakan untuk mengatur Camera mana yang dirender lebih dahulu atau lebih akhir. Camera dengan nilai Depth lebih tinggi biasanya dirender setelah Camera dengan nilai yang lebih rendah. Kombinasi Depth dan Clear Flags dapat digunakan untuk menumpuk beberapa hasil render Camera.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Higher Depth** : Camera dirender dengan urutan lebih akhir dan dapat tampil di atas hasil Camera lain.  
* **2\. When Lower Depth** : Camera dirender dengan urutan lebih awal.

* **Name Property** : Target Texture  
* **One line Guide** : Menentukan Render Texture sebagai tujuan hasil render Camera.  
* **Threeline Guide** : Target Texture membuat Camera merender ke sebuah Render Texture, bukan langsung ke layar. Hasilnya dapat digunakan untuk monitor keamanan, minimap, portal, refleksi, atau material pada objek lain. Jika tidak diisi, Camera merender ke layar sesuai pengaturan viewport.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Render Texture Assigned** : Hasil render Camera disimpan ke Render Texture.  
* **2\. When Target Texture Empty** : Hasil render Camera ditampilkan ke layar.

* **Name Property** : HDR dan Occlusion Culling  
* **One line Guide** : Mengatur dukungan rentang warna tinggi dan penghilangan objek yang tertutup dari proses rendering.  
* **Threeline Guide** : HDR memungkinkan Camera mempertahankan rentang intensitas cahaya yang lebih luas untuk efek pencahayaan dan post-processing. Occlusion Culling membantu mengurangi proses rendering objek yang tertutup oleh objek lain. Penggunaan kedua fitur ini harus disesuaikan dengan kebutuhan visual dan performa project.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When HDR Enabled** : Camera dapat menggunakan rentang intensitas warna yang lebih luas.  
* **2\. When Occlusion Culling Enabled** : Objek yang tertutup dan tidak terlihat dapat dilewati dari proses rendering.

**Animator**

Animator adalah komponen yang mengatur dan menjalankan animasi pada GameObject. Animator menggunakan Animator Controller untuk berpindah antar state animasi berdasarkan parameter, kondisi, dan transisi yang telah dibuat.

### **1\. Property**

* **Name Property** : Controller  
* **One line Guide** : Menentukan Animator Controller yang digunakan untuk mengatur animasi GameObject.  
* **Threeline Guide** : Animator Controller berisi state, animation clip, parameter, dan transition yang menentukan perilaku animasi. Controller dapat mengatur perpindahan animasi seperti Idle, Walk, Run, dan Attack. Jika Controller tidak diisi, Animator tidak memiliki state machine untuk menjalankan animasi.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Controller Assigned** : Animator dapat menjalankan state dan animation clip dari Controller.  
* **2\. When Controller Empty** : Animator tidak memiliki Controller untuk mengatur animasi.

* **Name Property** : Avatar  
* **One line Guide** : Menentukan Avatar yang memetakan tulang model humanoid ke sistem animasi.  
* **Threeline Guide** : Avatar digunakan terutama pada model humanoid agar Animator dapat memahami hubungan tulang seperti tubuh, tangan, dan kaki. Avatar memungkinkan animation clip humanoid digunakan pada model lain melalui proses retargeting. Untuk model non-humanoid, Avatar dapat memiliki pengaturan yang berbeda sesuai struktur tulangnya.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Avatar Valid** : Animator dapat memetakan dan menjalankan animasi sesuai struktur model.  
* **2\. When Avatar Invalid** : Animasi humanoid dapat mengalami masalah pemetaan atau tidak berjalan dengan benar.

* **Name Property** : Apply Root Motion  
* **One line Guide** : Menentukan apakah pergerakan posisi dari animasi diterapkan ke Transform GameObject.  
* **Threeline Guide** : Root Motion memungkinkan posisi dan rotasi yang dibuat oleh animation clip menggerakkan GameObject secara langsung. Jika dinonaktifkan, pergerakan karakter biasanya dikendalikan oleh script, Rigidbody, atau sistem movement lain. Gunakan pengaturan ini sesuai cara project mengatur perpindahan karakter.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When True** : Pergerakan dari animasi dapat diterapkan ke Transform GameObject.  
* **2\. When False** : Pergerakan posisi dari animasi tidak diterapkan secara langsung dan dapat dikendalikan oleh sistem lain.

* **Name Property** : Update Mode  
* **One line Guide** : Menentukan waktu dan sinkronisasi pembaruan Animator terhadap frame permainan.  
* **Threeline Guide** : Update Mode mengatur apakah Animator diperbarui berdasarkan waktu normal, waktu tidak terpengaruh skala waktu, atau waktu fisika. Normal mengikuti waktu permainan biasa. Unscaled Time tetap memperbarui animasi ketika Time.timeScale diubah, sedangkan Animate Physics menyesuaikan pembaruan animasi dengan siklus fisika.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Normal** : Animasi mengikuti waktu permainan normal.  
* **2\. When Unscaled Time** : Animasi tetap berjalan meskipun Time.timeScale diperlambat atau dihentikan.  
* **3\. When Animate Physics** : Animasi diperbarui bersamaan dengan proses fisika.

* **Name Property** : Culling Mode  
* **One line Guide** : Menentukan perilaku Animator ketika GameObject berada di luar tampilan Camera.  
* **Threeline Guide** : Culling Mode dapat menghentikan pembaruan animasi ketika objek tidak terlihat untuk menghemat performa. Always Animate tetap memperbarui animasi meskipun objek tidak terlihat. Cull Update Transforms menghentikan pembaruan animasi yang tidak terlihat tetapi tetap memperbarui Transform ketika objek kembali terlihat.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Always Animate** : Animator tetap memperbarui animasi meskipun GameObject berada di luar Camera.  
* **2\. When Cull Update Transforms** : Animator dapat menghentikan pembaruan saat tidak terlihat dan melanjutkannya ketika terlihat kembali.

* **Name Property** : Parameters  
* **One line Guide** : Menyediakan nilai yang digunakan untuk mengontrol kondisi dan transisi animasi.  
* **Threeline Guide** : Parameter dapat berupa Float, Int, Bool, atau Trigger. Nilai parameter dibaca oleh transition untuk menentukan kapan Animator berpindah dari satu state ke state lain. Parameter biasanya diubah melalui script berdasarkan input pengguna, kecepatan karakter, kondisi permainan, atau event tertentu.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Bool True** : Transition dengan kondisi aktif dapat dijalankan.  
* **2\. When Trigger Set** : Transition yang menunggu trigger dapat dijalankan satu kali.  
* **3\. When Float or Int Changed** : Transition dapat berpindah berdasarkan nilai numerik yang ditentukan.

* **Name Property** : Layers dan Transitions  
* **One line Guide** : Mengatur pembagian state machine dan perpindahan antar animation state.  
* **Threeline Guide** : Layers memungkinkan beberapa kelompok animasi berjalan secara terpisah, misalnya animasi tubuh bagian bawah dan animasi serangan pada tubuh bagian atas. Transition menentukan hubungan perpindahan antar state, termasuk kondisi, durasi, dan blending animasi. Pengaturan yang tepat membuat perpindahan animasi terlihat lebih halus dan sesuai kondisi permainan.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Condition Met** : Animator berpindah ke state tujuan melalui transition.  
* **2\. When Transition Duration Higher** : Perpindahan antar animasi berlangsung lebih lama dan terlihat lebih halus.  
* **3\. When Layer Weight 0** : Animasi pada layer tersebut tidak memberikan pengaruh pada hasil akhir.

**Sprite Renderer**

Sprite Renderer adalah komponen yang digunakan untuk menampilkan Sprite sebagai gambar 2D di dalam Scene. Komponen ini mengatur tampilan Sprite, warna, material, ukuran, serta urutan render terhadap objek 2D lainnya.

### **1\. Property**

* **Name Property** : Sprite  
* **One line Guide** : Menentukan gambar Sprite yang ditampilkan oleh Sprite Renderer.  
* **Threeline Guide** : Sprite adalah aset gambar 2D yang dirender oleh komponen ini. Sprite dapat berasal dari gambar tunggal atau bagian tertentu dari Sprite Sheet. Jika Sprite tidak diisi, Sprite Renderer tidak menampilkan gambar.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Sprite Assigned** : Gambar Sprite ditampilkan pada GameObject.  
* **2\. When Sprite Empty** : Sprite Renderer tidak menampilkan gambar.

* **Name Property** : Color  
* **One line Guide** : Mengatur warna dan tingkat transparansi Sprite yang ditampilkan.  
* **Threeline Guide** : Color digunakan untuk memberi warna pada Sprite tanpa mengubah aset gambar aslinya. Nilai Alpha pada Color mengatur tingkat transparansi Sprite. Fitur ini dapat digunakan untuk efek tint, fade, damage flash, atau perubahan warna karakter saat runtime.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Alpha 1** : Sprite ditampilkan dengan tingkat transparansi penuh.  
* **2\. When Alpha 0** : Sprite menjadi sepenuhnya transparan.  
* **3\. When Color Changed** : Warna tampilan Sprite berubah sesuai nilai Color.

* **Name Property** : Flip X dan Flip Y  
* **One line Guide** : Membalik arah tampilan Sprite pada sumbu horizontal atau vertikal.  
* **Threeline Guide** : Flip X membalik Sprite dari kiri ke kanan, sedangkan Flip Y membalik Sprite dari atas ke bawah. Pembalikan ini hanya memengaruhi tampilan Sprite dan tidak mengubah aset gambar sumber. Fitur ini sering digunakan untuk membalik arah hadap karakter tanpa membuat Sprite baru.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Flip X Enabled** : Sprite dibalik secara horizontal.  
* **2\. When Flip Y Enabled** : Sprite dibalik secara vertikal.  
* **3\. When Both Enabled** : Sprite dibalik pada sumbu horizontal dan vertikal.

* **Name Property** : Material  
* **One line Guide** : Menentukan material dan shader yang digunakan untuk merender Sprite.  
* **Threeline Guide** : Material mengatur cara Sprite dirender, termasuk warna, transparansi, pencahayaan, dan efek visual lainnya. Material default biasanya sudah cukup untuk Sprite biasa. Gunakan material khusus jika Sprite membutuhkan efek seperti outline, dissolve, atau shader 2D tertentu.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Material Assigned** : Sprite dirender menggunakan shader dan pengaturan dari material tersebut.  
* **2\. When Material Empty** : Sprite menggunakan material default dari sistem Sprite Renderer.

* **Name Property** : Draw Mode  
* **One line Guide** : Menentukan cara Sprite dirender ketika ukurannya diubah.  
* **Threeline Guide** : Simple menampilkan Sprite dengan ukuran normal dan tidak menggunakan pengaturan ukuran khusus. Sliced memungkinkan bagian tengah Sprite diperluas tanpa merusak sudutnya, sedangkan Tiled mengulang tekstur Sprite untuk mengisi ukuran yang lebih besar. Draw Mode Sliced dan Tiled membutuhkan Sprite yang memiliki pengaturan border.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Simple** : Sprite ditampilkan sebagai gambar biasa.  
* **2\. When Sliced** : Bagian tengah Sprite dapat diperbesar tanpa mengubah bentuk sudut.  
* **3\. When Tiled** : Tekstur Sprite diulang untuk mengisi area yang lebih besar.

* **Name Property** : Size  
* **One line Guide** : Menentukan ukuran area Sprite ketika Draw Mode menggunakan Sliced atau Tiled.  
* **Threeline Guide** : Size mengatur lebar dan tinggi area yang dirender oleh Sprite Renderer. Properti ini tidak mengubah ukuran aset Sprite asli. Saat Draw Mode menggunakan Simple, ukuran Sprite biasanya mengikuti ukuran sumber dan pengaturan Transform.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Width or Height Increased** : Area Sprite menjadi lebih besar sesuai nilai yang diberikan.  
* **2\. When Width or Height Decreased** : Area Sprite menjadi lebih kecil.

* **Name Property** : Mask Interaction  
* **One line Guide** : Menentukan hubungan Sprite Renderer dengan Sprite Mask.  
* **Threeline Guide** : Mask Interaction mengatur apakah Sprite dirender tanpa memperhatikan Sprite Mask atau hanya ditampilkan pada area yang dipengaruhi Mask. Fitur ini berguna untuk membuat efek area terlihat, karakter yang muncul di balik objek, atau pembatas tampilan 2D. Pengaturan ini membutuhkan Sprite Mask yang tersedia di dalam Scene.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When None** : Sprite dirender tanpa pengaruh Sprite Mask.  
* **2\. When Visible Inside Mask** : Sprite hanya terlihat di dalam area Sprite Mask.  
* **3\. When Visible Outside Mask** : Sprite hanya terlihat di luar area Sprite Mask.

* **Name Property** : Sorting Layer  
* **One line Guide** : Menentukan lapisan render Sprite terhadap Sprite Renderer lainnya.  
* **Threeline Guide** : Sorting Layer mengelompokkan Sprite berdasarkan urutan lapisan. Sprite pada layer yang lebih tinggi dapat dirender di atas Sprite pada layer yang lebih rendah. Gunakan layer yang berbeda untuk mengatur latar belakang, objek dunia, karakter, dan efek foreground.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Higher Layer** : Sprite memiliki prioritas render yang lebih tinggi.  
* **2\. When Lower Layer** : Sprite dirender di bawah Sprite pada layer yang lebih tinggi.

* **Name Property** : Order in Layer  
* **One line Guide** : Mengatur urutan Sprite di dalam Sorting Layer yang sama.  
* **Threeline Guide** : Order in Layer menentukan Sprite mana yang tampil di depan ketika beberapa Sprite berada pada Sorting Layer yang sama. Nilai yang lebih besar biasanya dirender di atas nilai yang lebih kecil. Properti ini membantu mengatur urutan objek 2D tanpa harus mengubah posisi Z secara manual.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Higher Value** : Sprite ditampilkan di atas Sprite dengan nilai lebih rendah.  
* **2\. When Lower Value** : Sprite ditampilkan di bawah Sprite dengan nilai lebih tinggi.

* **Name Property** : Sprite Sort Point  
* **One line Guide** : Menentukan titik acuan yang digunakan untuk pengurutan Sprite.  
* **Threeline Guide** : Sprite Sort Point dapat menggunakan pusat Sprite atau pivot Sprite sebagai titik acuan. Titik ini digunakan ketika project memakai pengaturan sorting berdasarkan jarak atau posisi Sprite. Pilih pivot jika urutan tampilan perlu mengikuti titik kaki karakter atau titik dasar objek.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Center** : Pusat area Sprite digunakan sebagai titik pengurutan.  
* **2\. When Pivot** : Pivot yang ditentukan pada Sprite digunakan sebagai titik pengurutan.

**2D Lighting**

2D Lighting adalah komponen pencahayaan untuk memberikan efek cahaya dan bayangan pada objek 2D. Komponen ini bekerja pada project yang menggunakan Universal Render Pipeline dengan dukungan 2D Renderer dan dapat memengaruhi Sprite yang berada pada Sorting Layer tertentu.

### **1\. Property**

* **Name Property** : Light Type  
* **One line Guide** : Menentukan bentuk dan cara penyebaran cahaya 2D.  
* **Threeline Guide** : Light Type menentukan area yang dipengaruhi oleh cahaya. Freeform menggunakan bentuk yang dapat dibuat sesuai kebutuhan, Sprite menggunakan bentuk dari Sprite tertentu, Point memancarkan cahaya dari satu titik, dan Global memberikan pencahayaan pada area Scene secara luas. Pilih tipe berdasarkan bentuk efek cahaya yang ingin dibuat.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Point** : Cahaya menyebar dari satu titik dengan jarak pengaruh tertentu.  
* **2\. When Freeform** : Cahaya mengikuti bentuk area yang dibuat secara manual.  
* **3\. When Sprite** : Cahaya mengikuti bentuk Sprite yang digunakan.  
* **4\. When Global** : Cahaya memengaruhi area Scene secara global.

* **Name Property** : Blend Style  
* **One line Guide** : Menentukan cara warna cahaya digabungkan dengan warna Sprite.  
* **Threeline Guide** : Blend Style menggunakan pengaturan pada 2D Renderer untuk menentukan bagaimana cahaya memengaruhi objek yang diterangi. Setiap style dapat memiliki mode blending, warna, dan parameter intensitas yang berbeda. Gunakan style yang sesuai untuk cahaya normal, efek additive, atau efek khusus lainnya.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Blend Style Selected** : Cahaya menggunakan aturan blending dari style yang dipilih.  
* **2\. When Blend Style Changed** : Hasil perpaduan warna cahaya dan Sprite dapat berubah.

* **Name Property** : Color  
* **One line Guide** : Menentukan warna cahaya yang diberikan kepada objek 2D.  
* **Threeline Guide** : Color mengatur warna dasar cahaya yang dirender oleh 2D Lighting. Warna hangat dapat digunakan untuk lampu api atau matahari terbenam, sedangkan warna dingin dapat digunakan untuk malam atau cahaya bulan. Warna akhir yang terlihat juga dipengaruhi oleh Blend Style dan warna Sprite.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Warm Color** : Cahaya memberikan nuansa warna hangat pada Sprite.  
* **2\. When Cool Color** : Cahaya memberikan nuansa warna dingin pada Sprite.  
* **3\. When Alpha 0** : Cahaya menjadi transparan dan tidak memberikan pengaruh visual.

* **Name Property** : Intensity  
* **One line Guide** : Mengatur kekuatan cahaya yang diberikan kepada objek 2D.  
* **Threeline Guide** : Intensity menentukan seberapa kuat pengaruh warna cahaya terhadap Sprite. Nilai yang lebih tinggi menghasilkan cahaya yang lebih terang, sedangkan nilai yang lebih rendah menghasilkan cahaya yang lebih redup. Intensity dapat diubah saat runtime untuk membuat efek lampu berkedip atau perubahan cahaya secara bertahap.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Low Value** : Cahaya terlihat lebih redup.  
* **2\. When High Value** : Cahaya terlihat lebih kuat dan terang.

* **Name Property** : Target Sorting Layers  
* **One line Guide** : Menentukan Sorting Layer yang dapat dipengaruhi oleh 2D Lighting.  
* **Threeline Guide** : Target Sorting Layers membatasi objek 2D yang menerima pengaruh cahaya. Sprite pada layer yang dipilih dapat menerima pencahayaan, sedangkan Sprite pada layer lain tidak dipengaruhi oleh Light 2D tersebut. Fitur ini berguna untuk memisahkan objek latar belakang, karakter, foreground, atau efek tertentu.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Layer Selected** : Sprite pada Sorting Layer tersebut dapat menerima cahaya.  
* **2\. When Layer Deselected** : Sprite pada Sorting Layer tersebut tidak menerima cahaya.

* **Name Property** : Falloff Intensity  
* **One line Guide** : Mengatur perubahan intensitas cahaya berdasarkan jarak dari sumber cahaya.  
* **Threeline Guide** : Falloff Intensity menentukan seberapa cepat cahaya melemah dari pusat atau area sumbernya. Nilai falloff yang lebih tinggi dapat membuat batas cahaya lebih cepat redup, sedangkan nilai yang lebih rendah membuat cahaya menyebar lebih jauh dan halus. Pengaturan ini membantu menciptakan kesan jarak dan kedalaman pada Scene 2D.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When High Falloff** : Cahaya melemah lebih cepat dari sumbernya.  
* **2\. When Low Falloff** : Cahaya menyebar lebih jauh dengan perubahan intensitas yang lebih halus.

* **Name Property** : Point Light Radius  
* **One line Guide** : Menentukan radius area pengaruh Point Light.  
* **Threeline Guide** : Point Light Radius menentukan jarak maksimum penyebaran cahaya dari titik sumber. Radius yang lebih besar membuat area yang menerima cahaya menjadi lebih luas. Properti ini terutama digunakan ketika Light Type menggunakan Point.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Small Radius** : Cahaya hanya memengaruhi area yang dekat dengan sumber.  
* **2\. When Large Radius** : Cahaya memengaruhi area yang lebih luas.

* **Name Property** : Shadow Strength  
* **One line Guide** : Mengatur tingkat gelap bayangan yang dihasilkan oleh objek 2D.  
* **Threeline Guide** : Shadow Strength menentukan seberapa kuat bayangan terlihat ketika objek menerima cahaya. Nilai yang lebih tinggi menghasilkan bayangan yang lebih gelap dan jelas. Bayangan membutuhkan pengaturan renderer serta dukungan objek 2D yang sesuai agar dapat tampil.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When High Value** : Bayangan terlihat lebih gelap dan kuat.  
* **2\. When Low Value** : Bayangan terlihat lebih transparan dan lembut.

**TextMeshPro**

TextMeshPro adalah komponen untuk menampilkan teks dengan kualitas tinggi pada UI maupun objek 3D. TextMeshPro mendukung font asset khusus, pengaturan layout yang lebih lengkap, serta fitur seperti rich text, wrapping, alignment, dan efek visual pada teks.

### **1\. Property**

* **Name Property** : Text Input  
* **One line Guide** : Menentukan isi teks yang ditampilkan oleh TextMeshPro.  
* **Threeline Guide** : Text Input berisi karakter atau kalimat yang akan dirender pada Scene. Isi teks dapat diubah melalui Inspector, script, atau sistem localization. TextMeshPro juga mendukung tag rich text untuk mengubah gaya sebagian teks tanpa membuat komponen tambahan.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Text Assigned** : Teks ditampilkan sesuai isi Text Input.  
* **2\. When Text Empty** : Tidak ada karakter yang ditampilkan.

* **Name Property** : Font Asset  
* **One line Guide** : Menentukan font asset yang digunakan untuk merender teks.  
* **Threeline Guide** : Font Asset adalah aset font yang telah diproses agar dapat digunakan oleh TextMeshPro. Font asset menyimpan data karakter, glyph, dan informasi atlas yang dibutuhkan untuk menampilkan teks dengan tajam. Gunakan font asset yang sesuai dengan gaya visual dan karakter bahasa yang digunakan dalam project.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Font Assigned** : Teks dirender menggunakan font asset yang dipilih.  
* **2\. When Font Empty** : TextMeshPro tidak dapat merender karakter dengan benar.

* **Name Property** : Material Preset  
* **One line Guide** : Menentukan material dan shader yang digunakan oleh teks.  
* **Threeline Guide** : Material Preset mengatur tampilan visual teks seperti warna, outline, underlay, dan efek shader lainnya. Beberapa TextMeshPro dapat menggunakan font asset yang sama dengan material preset berbeda. Fitur ini memungkinkan variasi tampilan teks tanpa menggandakan font asset.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Material Preset Assigned** : Teks menggunakan pengaturan visual dari material tersebut.  
* **2\. When Material Preset Changed** : Tampilan teks berubah sesuai preset yang dipilih.

* **Name Property** : Font Size  
* **One line Guide** : Mengatur ukuran karakter teks.  
* **Threeline Guide** : Font Size menentukan besar kecilnya karakter secara langsung. Ukuran dapat digunakan bersama Auto Size untuk menyesuaikan teks terhadap area yang tersedia. Gunakan ukuran yang sesuai dengan skala Canvas atau ukuran objek agar teks tetap terbaca.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Larger Value** : Karakter teks terlihat lebih besar.  
* **2\. When Smaller Value** : Karakter teks terlihat lebih kecil.

* **Name Property** : Color  
* **One line Guide** : Menentukan warna dan transparansi teks.  
* **Threeline Guide** : Color mengatur warna utama karakter yang dirender. Nilai Alpha dapat digunakan untuk membuat teks transparan atau menjalankan efek fade. Warna akhir juga dapat dipengaruhi oleh Material Preset dan tag warna pada rich text.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Alpha 1** : Teks ditampilkan dengan transparansi penuh.  
* **2\. When Alpha 0** : Teks menjadi sepenuhnya transparan.  
* **3\. When Color Changed** : Teks ditampilkan dengan warna baru.

* **Name Property** : Alignment  
* **One line Guide** : Mengatur posisi perataan teks di dalam area TextMeshPro.  
* **Threeline Guide** : Alignment menentukan posisi teks secara horizontal dan vertikal. Teks dapat diratakan ke kiri, tengah, kanan, atas, atau bawah, termasuk kombinasi di antaranya. Pengaturan ini membantu menjaga posisi label, judul, dialog, dan informasi UI tetap konsisten.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Left or Top** : Teks diratakan ke sisi kiri atau atas area.  
* **2\. When Center** : Teks ditempatkan di tengah area.  
* **3\. When Right or Bottom** : Teks diratakan ke sisi kanan atau bawah area.

* **Name Property** : Auto Size  
* **One line Guide** : Menentukan apakah ukuran font menyesuaikan area teks secara otomatis.  
* **Threeline Guide** : Auto Size dapat mengecilkan atau membesarkan teks agar tetap berada di dalam area yang tersedia. Properti ini menggunakan batas ukuran minimum dan maksimum untuk mengontrol hasil penyesuaian. Gunakan Auto Size untuk label dinamis, dialog, atau teks yang panjangnya dapat berubah.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Enabled** : Ukuran teks menyesuaikan ruang yang tersedia.  
* **2\. When Disabled** : Ukuran teks mengikuti nilai Font Size secara langsung.

* **Name Property** : Word Wrapping  
* **One line Guide** : Menentukan apakah teks dapat berpindah ke baris berikutnya saat mencapai batas area.  
* **Threeline Guide** : Word Wrapping membantu menempatkan teks panjang ke beberapa baris. Ketika aktif, kata akan dipindahkan ke baris berikutnya sesuai lebar area TextMeshPro. Ketika nonaktif, teks dapat melewati batas area atau menggunakan perilaku overflow yang dipilih.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Enabled** : Teks dapat dibagi ke beberapa baris.  
* **2\. When Disabled** : Teks tetap berusaha berada pada baris yang sama.

* **Name Property** : Overflow  
* **One line Guide** : Menentukan perilaku teks ketika isi melebihi area yang tersedia.  
* **Threeline Guide** : Overflow dapat membuat teks terlihat di luar area, terpotong, diperkecil, atau diisi dengan tanda elipsis. Pengaturan ini membantu menentukan cara teks panjang ditampilkan pada UI yang memiliki ukuran terbatas. Pilih perilaku yang sesuai dengan kebutuhan informasi.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Overflow Visible** : Teks tetap terlihat meskipun melewati batas area.  
* **2\. When Overflow Truncated** : Bagian teks yang melewati area dipotong.  
* **3\. When Overflow Ellipsis** : Bagian teks yang terpotong digantikan dengan tanda elipsis.

* **Name Property** : Rich Text  
* **One line Guide** : Mengaktifkan dukungan tag untuk memberi format pada bagian tertentu dari teks.  
* **Threeline Guide** : Rich Text memungkinkan penggunaan tag seperti bold, italic, color, size, dan style lain yang didukung TextMeshPro. Fitur ini berguna untuk membuat variasi format dalam satu komponen teks. Pastikan isi teks berasal dari sumber yang aman jika tag rich text dapat diubah oleh pengguna.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Enabled** : Tag rich text diproses dan memengaruhi tampilan teks.  
* **2\. When Disabled** : Tag ditampilkan sebagai teks biasa atau tidak diproses sebagai format.

* **Name Property** : Raycast Target  
* **One line Guide** : Menentukan apakah teks dapat menerima raycast dari pointer atau input UI.  
* **Threeline Guide** : Raycast Target menentukan apakah TextMeshPro dianggap sebagai target interaksi oleh sistem UI. Aktifkan jika teks perlu menerima event pointer atau digunakan dalam sistem interaksi tertentu. Nonaktifkan jika teks hanya berfungsi sebagai tampilan dan tidak boleh menghalangi raycast ke elemen UI lain.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Enabled** : Teks dapat menerima atau menghalangi raycast UI.  
* **2\. When Disabled** : Raycast dapat melewati teks menuju elemen UI lainnya.

* **Name Property** : Extra Settings  
* **One line Guide** : Mengatur pengaturan tambahan seperti spacing, wrapping, kerning, dan style teks.  
* **Threeline Guide** : Extra Settings menyediakan pengaturan detail untuk mengontrol jarak antar karakter, kata, dan baris. Pengaturan ini dapat digunakan untuk menyesuaikan tampilan judul, label, atau teks dekoratif. Perubahan kecil pada spacing dan line spacing dapat memengaruhi keterbacaan serta kepadatan layout.  
* **Is Have Feature Eyes on Current Effect** : true  
* **1\. When Character Spacing Increased** : Jarak antar karakter menjadi lebih lebar.  
* **2\. When Line Spacing Increased** : Jarak antar baris menjadi lebih besar.  
* **3\. When Kerning Enabled** : Jarak pasangan karakter disesuaikan agar tampilan teks lebih rapi.

[image1]: <data:image/png;base64,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>

[image2]: <data:image/png;base64,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>

[image3]: <data:image/png;base64,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>
