using System;
using System.Collections;
using System.Collections.Generic;
using Custom.Interface;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MenuLinty : EditorWindow
{
    private int selectedTab = 0;

    // Dictionary untuk menyimpan state masing-masing properti berdasarkan namanya
    private Dictionary<string, PropertyState> guideStates = new Dictionary<string, PropertyState>();

    private float lastRepaintTime;
    private float repaintInterval = 2f; // Atur ke 3 detik sekali (bisa diubah jadi 1f kalau mau 1 detik)

    private GameObject selectedObj;
    private Texture2D activeTabTex;
    private Texture2D inactiveTabTex;
    private Texture2D normal_text,
        hover_text,
        active_text;
    private readonly string[] tabNames =
    {
        "Diagnosa",
        "Guide",
        "Structure",
        "File Relocator",
        "Setting"
    };

    // Status state untuk Tab Guide
    private bool showBoxCollider2DGuide = false;
    private bool showRigidbody2DGuide = false;
    private bool showTransformGuide = false;
    private bool showCanvasGuide = false;
    private bool showCanvasGroupGuide = false;
    private bool showAudioSourceGuide = false;
    private bool showCameraGuide = false;
    private bool showAnimatorGuide = false;
    private bool showSpriteRendererGuide = false;
    private bool showLight2DGuide = false;
    private bool showTextMeshProGuide = false;

    private bool isTriggerEffectVisible = false;
    private bool sizeEffectVisible = false;
    private bool bodyTypeEffectVisible = false;

    private Vector2 guideScrollPos;
    private Vector2 diagScrollPos;
    private Vector2 structureScrollPos;
    private Vector2 structureFolderListScrollPos;
    private int expandedStructureFolderIndex = 0;
    private FileRelocatorTabRenderer fileRelocatorRenderer;

    private List<IDiagnosticRule> diagnosticRules = new List<IDiagnosticRule>();

    [MenuItem("Tools/Linty Assistant Menu")]
    public static void ShowWindow()
    {
        MenuLinty window = GetWindow<MenuLinty>("Linty Asisten");
        window.minSize = new Vector2(380, 500);
    }

    private void OnInspectorUpdate()
    {
        // Hanya jalan jika sedang di Tab Guide dan ada objek terpilih
        if (selectedObj != null)
        {
            // Cek apakah waktu sekarang dikurangi waktu terakhir repaint sudah mencapai interval
            if (EditorApplication.timeSinceStartup - lastRepaintTime >= repaintInterval)
            {
                Repaint();
                lastRepaintTime = (float)EditorApplication.timeSinceStartup;
            }
        }
    }

    private void InitTabTextures()
    {
        if (activeTabTex == null)
        {
            activeTabTex = new Texture2D(1, 1);
            activeTabTex.SetPixel(0, 0, GenerateColorByHexa255(197, 104, 47));
            activeTabTex.Apply();
        }

        if (inactiveTabTex == null)
        {
            inactiveTabTex = new Texture2D(1, 1);
            inactiveTabTex.SetPixel(0, 0, GenerateColorByHexa255(18, 18, 18));
            inactiveTabTex.Apply();
        }

        if (normal_text == null)
        {
            normal_text = new Texture2D(1, 1);
            normal_text.SetPixel(0, 0, GenerateColorByHexa255(39, 174, 96)); // Warna Non-Aktif (Abu-abu gelap rapi)
            normal_text.Apply();
        }
        if (hover_text == null)
        {
            hover_text = new Texture2D(1, 1);
            hover_text.SetPixel(0, 0, GenerateColorByHexa255(39, 184, 106)); // Warna Non-Aktif (Abu-abu gelap rapi)
            hover_text.Apply();
        }
        if (active_text == null)
        {
            active_text = new Texture2D(1, 1);
            active_text.SetPixel(0, 0, GenerateColorByHexa255(20, 154, 86)); // Warna Non-Aktif (Abu-abu gelap rapi)
            active_text.Apply();
        }
    }

    Color GenerateColorByHexa255(float r, float g, float b)
    {
        return new Color(r / 255, g / 255, b / 255);
    }

    private void OnEnable()
    {
        InitTabTextures();
        fileRelocatorRenderer = new FileRelocatorTabRenderer();
        isTriggerEffectVisible = false;
        sizeEffectVisible = false;
        bodyTypeEffectVisible = false;
        Selection.selectionChanged += OnSelectionChanged;
    }

    private void OnDisable()
    {
        Selection.selectionChanged -= OnSelectionChanged;
    }

    private void OnSelectionChanged()
    {
        Repaint();
    }

    private void OnGUI()
    {
        // Debug.Log("position.width : " + position.width);
        // 1. Latar Belakang Gelap Kustom
        Color darkBackground = new Color(0.18f, 0.18f, 0.18f);
        EditorGUI.DrawRect(new Rect(0, 0, position.width, position.height), darkBackground);

        GUILayout.Space(12);

        GUIStyle titleStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            normal = { textColor = Color.white },
            fontSize = 12,
        };

        GUILayout.BeginHorizontal();
        GUILayout.Space(12);
        GUILayout.Label("Tab Navigasi :", titleStyle);
        GUILayout.EndHorizontal();

        GUILayout.Space(6);

        DrawTabNavigation();

        GUILayout.Space(12);

        switch (selectedTab)
        {
            case 0:
                DrawDiagnosaTab();
                break;
            case 1:
                DrawGuideTab();
                break;
            case 2:
                DrawStructureTab();
                break;
            case 3:
                DrawFileRelocatorTab();
                break;
            case 4:
                DrawSettingTab();
                break;
        }
    }

    private void DrawTabNavigation()
    {
        InitTabTextures();

        GUILayout.BeginHorizontal();
        GUILayout.Space(10);

        for (int i = 0; i < tabNames.Length; i++)
        {
            bool isActive = (i == selectedTab);

            GUIStyle btnStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = 12,
                fontStyle = FontStyle.Bold,
                fixedHeight = 28,
                fixedWidth = (position.width / tabNames.Length) - 7,
                alignment = TextAnchor.MiddleCenter,
                normal =
                {
                    background = isActive ? activeTabTex : inactiveTabTex,
                    textColor = Color.white,
                },
                hover = { background = activeTabTex, textColor = Color.white },
                active = { background = activeTabTex, textColor = new Color(0.9f, 0.9f, 0.9f) },
            };

            if (GUILayout.Button(tabNames[i], btnStyle))
            {
                selectedTab = i;
            }
        }

        GUILayout.EndHorizontal();
    }

    #region Tab Content Renderers
    private void DrawDiagnosaTab()
    {
        diagScrollPos = EditorGUILayout.BeginScrollView(diagScrollPos);
        selectedObj = Selection.activeGameObject;

        if (selectedObj == null)
        {
            DrawWarningBanner("Tidak ada Component ditemukan.");

            GUILayout.Space(8);
            GUILayout.BeginHorizontal();
            GUILayout.Space(12);

            GUIStyle infoStyle = new GUIStyle(EditorStyles.label)
            {
                normal = { textColor = Color.white },
                fontSize = 11,
                fontStyle = FontStyle.Bold,
            };
            GUILayout.Label("Mulai Pilih Object Di dalam Hierarchy Panelmu.", infoStyle);
            GUILayout.EndHorizontal();

            EditorGUILayout.EndScrollView();
            return;
        }

        // 1. Bagian Informasi Dasar
        DrawBasicInfoSection(selectedObj);

        // Horizontal divider di bawah Basic Info
        // GUILayout.Space(4);
        GUILayout.BeginHorizontal();
        GUILayout.Space(12);
        Rect dividerRect = new Rect(
            12,
            GUILayoutUtility.GetLastRect().yMax,
            position.width - 24,
            1
        );
        EditorGUI.DrawRect(dividerRect, new Color(0.5f, 0.5f, 0.5f));
        GUILayout.Space(12);
        GUILayout.EndHorizontal();
        GUILayout.Space(6);

        // 2. Bagian Diagnosa Komponen
        // DrawComponentDiagnosisSection(selectedObj);

        // 3. Tombol Aksi Massal di paling bawah
        GUIStyle fixAllBtnStyle = new GUIStyle(GUI.skin.button)
        {
            fontSize = 12,
            fontStyle = FontStyle.Bold,
            fixedHeight = 36,
            fixedWidth = 0,
            alignment = TextAnchor.MiddleCenter,
            normal =
            {
                background = normal_text, // Warna Hijau Linty solid
                textColor = Color.white,
            },
            hover =
            {
                background = hover_text, // hover state
                textColor = Color.white,
            },
            active =
            {
                background = active_text, // active state
                textColor = new Color(0.8f, 0.9f, 0.8f),
            },
        };

        GUILayout.BeginHorizontal();
        GUILayout.Space(12);
        if (GUILayout.Button("Fix All Suggestion", fixAllBtnStyle, GUILayout.ExpandWidth(true)))
        {
            // Execute all fixes logic
        }
        GUILayout.EndHorizontal();

        if (selectedObj != null)
            EditorGUILayout.EndScrollView();
    }

    private bool showDesc = true;
    private bool enable = true;

    private void DrawBasicInfoSection(GameObject obj)
    {
        // Draw manual background rect (sharp corners, no skin box rounding)
        // Rect infoRect = GUILayoutUtility.GetLastRect();
        // infoRect = new Rect(12, infoRect.yMin, position.width - 24, 80);
        // EditorGUI.DrawRect(infoRect, new Color(0.2f, 0.2f, 0.2f));

        GUILayout.BeginHorizontal();
        GUILayout.Space(13);
        GUILayout.Label($"Nama Object: {obj.name}", EditorStyles.boldLabel);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Space(12);
        GUILayout.Label($"Tags: {obj.tag}", EditorStyles.label);
        GUILayout.EndHorizontal();
        // GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();
        GUILayout.Space(12);
        GUILayout.Label($"Layer: {LayerMask.LayerToName(obj.layer)}", EditorStyles.label);
        GUILayout.Space(12);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Space(12);
        GUILayout.Label($"Object Gerak: {(!obj.isStatic ? "Ya, Bisa Bergerak" : "Tidak, Static")} ", EditorStyles.label);
        GUILayout.Space(12);
        GUILayout.EndHorizontal();

        // Fallback Diagnosa langsung jika diagnosticRules belum diisi
        bool evaluatedAny = false;
        foreach (var rule in diagnosticRules)
        {
            if (rule.Evaluate(selectedObj))
            {
                rule.DrawDiagnosticUI(selectedObj);
                evaluatedAny = true;
                GUILayout.Space(6);
            }
        }

        // Jika list rule kosong, berikan diagnosa standar untuk Rigidbody2D & BoxCollider2D
        // if (!evaluatedAny)
        // {
        //     Rigidbody2D rb2d = selectedObj.GetComponent<Rigidbody2D>();
        //     BoxCollider2D box2d = selectedObj.GetComponent<BoxCollider2D>();

        //     if (rb2d == null && box2d == null)
        //     {
        //         EditorGUILayout.HelpBox(
        //             "Objek ini tidak memiliki komponen Rigidbody 2D atau Box Collider 2D untuk didiagnosa.",
        //             MessageType.Info
        //         );
        //     }

        //     if (rb2d != null)
        //     {
        //         EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        //         EditorGUILayout.LabelField("🟢 Rigidbody 2D Terdeteksi", EditorStyles.boldLabel);
        //         EditorGUILayout.LabelField($"Body Type: {rb2d.bodyType}", EditorStyles.label);
        //         EditorGUILayout.EndVertical();
        //         GUILayout.Space(5);
        //     }

        //     if (box2d != null)
        //     {
        //         EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        //         EditorGUILayout.LabelField("📦 Box Collider 2D Terdeteksi", EditorStyles.boldLabel);
        //         EditorGUILayout.LabelField($"Is Trigger: {box2d.isTrigger}", EditorStyles.label);
        //         EditorGUILayout.LabelField($"Size: {box2d.size}", EditorStyles.label);
        //         EditorGUILayout.EndVertical();
        //     }
        // }

        // EditorGUILayout.EndScrollView();
    }

    private void DrawGuideTab()
    {
        selectedObj = Selection.activeGameObject;
        if (selectedObj == null)
        {
            DrawWarningBanner("Tidak ada Gameobject dipilih.");
            GUILayout.Space(8);
            GUILayout.Label("Mulai Pilih Object Di dalam Hierarchy Panelmu. untuk melihat\nInformasi Object", new GUIStyle(EditorStyles.label)
            {
                normal = { textColor = Color.white }, fontSize = 11, fontStyle = FontStyle.Bold
            });
            return;
        }

        DrawBasicInfoSection(selectedObj);
        guideScrollPos = EditorGUILayout.BeginScrollView(guideScrollPos);

        if (selectedObj.TryGetComponent(out BoxCollider2D boxCollider2D))
        {
            DrawGuideSection(boxCollider2D, "Box Collider 2D", ref showBoxCollider2DGuide,
                new List<IGuideable> { new CustomBoxCollider2D_Material(), new CustomBoxCollider2D_Istrigger(), new CustomBoxCollider2D_UsedByEfector(), new CustomBoxCollider2D_UsedByComposite(), new CustomBoxCollider2D_AutoTiling(), new CustomBoxCollider2D_Offset(), new CustomBoxCollider2D_EdgeRadius() },
                (handler, component) => handler.GetData(component));
        }

        if (selectedObj.TryGetComponent(out Rigidbody2D rigidbody2D))
        {
            DrawGuideSection(rigidbody2D, "Rigidbody 2D", ref showRigidbody2DGuide,
                new List<IRigidbody2DGuideable> { new Rigidbody2DBodyTypeGuide(), new Rigidbody2DMaterialGuide(), new Rigidbody2DSimulatedGuide(), new Rigidbody2DAutoMassGuide(), new Rigidbody2DMassGuide(), new Rigidbody2DLinearDampingGuide(), new Rigidbody2DAngularDampingGuide(), new Rigidbody2DGravityScaleGuide(), new Rigidbody2DCollisionDetectionGuide(), new Rigidbody2DSleepingModeGuide(), new Rigidbody2DInterpolateGuide(), new Rigidbody2DConstraintsGuide() },
                (handler, component) => handler.GetData(component));
        }

        DrawGuideSection(selectedObj.transform, "Transform", ref showTransformGuide,
            new List<ITransformGuideable> { new TransformPositionGuide(), new TransformRotationGuide(), new TransformScaleGuide() },
            (handler, component) => handler.GetData(component));

        if (selectedObj.TryGetComponent(out Canvas canvas))
        {
            DrawGuideSection(canvas, "Canvas", ref showCanvasGuide,
                new List<ICanvasGuideable> { new CanvasRenderModeGuide(), new CanvasPixelPerfectGuide(), new CanvasRenderCameraGuide(), new CanvasPlaneDistanceGuide(), new CanvasSortingLayerGuide(), new CanvasOrderInLayerGuide(), new CanvasTargetDisplayGuide(), new CanvasShaderChannelsGuide() },
                (handler, component) => handler.GetData(component));
        }

        if (selectedObj.TryGetComponent(out CanvasGroup canvasGroup))
        {
            DrawGuideSection(canvasGroup, "Canvas Group", ref showCanvasGroupGuide,
                new List<ICanvasGroupGuideable> { new CanvasGroupAlphaGuide(), new CanvasGroupInteractableGuide(), new CanvasGroupBlocksRaycastsGuide(), new CanvasGroupIgnoreParentGuide() },
                (handler, component) => handler.GetData(component));
        }

        if (selectedObj.TryGetComponent(out AudioSource audioSource))
        {
            DrawGuideSection(audioSource, "AudioSource", ref showAudioSourceGuide,
                new List<IAudioSourceGuideable> { new AudioSourceClipGuide(), new AudioSourceOutputGuide(), new AudioSourceMuteGuide(), new AudioSourcePlayOnAwakeGuide(), new AudioSourceLoopGuide(), new AudioSourceVolumeGuide(), new AudioSourcePitchGuide(), new AudioSourceSpatialBlendGuide(), new AudioSourcePriorityGuide(), new AudioSourceBypassGuide(), new AudioSource3DSettingsGuide() },
                (handler, component) => handler.GetData(component));
        }

        if (selectedObj.TryGetComponent(out Camera camera))
        {
            DrawGuideSection(camera, "Camera", ref showCameraGuide,
                new List<ICameraGuideable> { new CameraClearFlagsGuide(), new CameraProjectionGuide(), new CameraFieldOfViewGuide(), new CameraCullingMaskGuide(), new CameraClippingPlanesGuide(), new CameraViewportGuide(), new CameraDepthGuide(), new CameraTargetTextureGuide(), new CameraRenderingGuide() },
                (handler, component) => handler.GetData(component));
        }

        if (selectedObj.TryGetComponent(out Animator animator))
        {
            DrawGuideSection(animator, "Animator", ref showAnimatorGuide,
                new List<IAnimatorGuideable> { new AnimatorControllerGuide(), new AnimatorAvatarGuide(), new AnimatorRootMotionGuide(), new AnimatorUpdateModeGuide(), new AnimatorCullingModeGuide(), new AnimatorParametersGuide(), new AnimatorLayersGuide() },
                (handler, component) => handler.GetData(component));
        }

        if (selectedObj.TryGetComponent(out SpriteRenderer spriteRenderer))
        {
            DrawGuideSection(spriteRenderer, "Sprite Renderer", ref showSpriteRendererGuide,
                new List<ISpriteRendererGuideable> { new SpriteRendererSpriteGuide(), new SpriteRendererColorGuide(), new SpriteRendererFlipGuide(), new SpriteRendererMaterialGuide(), new SpriteRendererDrawModeGuide(), new SpriteRendererSizeGuide(), new SpriteRendererMaskGuide(), new SpriteRendererSortingLayerGuide(), new SpriteRendererOrderGuide(), new SpriteRendererSortPointGuide() },
                (handler, component) => handler.GetData(component));
        }

        if (selectedObj.TryGetComponent(out Light2D light2D))
        {
            DrawGuideSection(light2D, "2D Lighting", ref showLight2DGuide,
                new List<ILight2DGuideable> { new Light2DTypeGuide(), new Light2DBlendStyleGuide(), new Light2DColorGuide(), new Light2DIntensityGuide(), new Light2DSortingLayersGuide(), new Light2DFalloffGuide(), new Light2DPointRadiusGuide(), new Light2DShadowGuide() },
                (handler, component) => handler.GetData(component));
        }

        if (selectedObj.TryGetComponent(out TMP_Text textMeshPro))
        {
            DrawGuideSection(textMeshPro, "TextMeshPro", ref showTextMeshProGuide,
                new List<ITextMeshProGuideable> { new TextMeshProTextGuide(), new TextMeshProFontGuide(), new TextMeshProMaterialGuide(), new TextMeshProFontSizeGuide(), new TextMeshProColorGuide(), new TextMeshProAlignmentGuide(), new TextMeshProAutoSizeGuide(), new TextMeshProWrappingGuide(), new TextMeshProOverflowGuide(), new TextMeshProRichTextGuide(), new TextMeshProRaycastGuide(), new TextMeshProExtraSettingsGuide() },
                (handler, component) => handler.GetData(component));
        }

        EditorGUILayout.EndScrollView();
    }

    private void DrawGuideSection<TComponent, TGuide>(TComponent component, string title, ref bool foldoutStatus, List<TGuide> handlers, Func<TGuide, TComponent, GuideItem> getData)
    {
        if (component == null) return;
        GUILayout.Space(5);
        DrawComponentHeaderBox(title, ref foldoutStatus);
        if (!foldoutStatus) return;
        GUILayout.Space(5);
        foreach (TGuide handler in handlers) DrawGuideItem(getData(handler, component));
    }

    private void DrawGuideTabLegacy()
    {

        // diagScrollPos = EditorGUILayout.BeginScrollView(diagScrollPos);
        selectedObj = Selection.activeGameObject;

        if (selectedObj == null)
        {
            DrawWarningBanner("Tidak ada Gameobject dipilih.");

            GUILayout.Space(8);
            GUILayout.BeginHorizontal();
            GUILayout.Space(12);

            GUIStyle infoStyle = new GUIStyle(EditorStyles.label)
            {
                normal = { textColor = Color.white },
                fontSize = 11,
                fontStyle = FontStyle.Bold,
            };
            GUILayout.Label("Mulai Pilih Object Di dalam Hierarchy Panelmu. untuk melihat\n Informasi Object", infoStyle);
            GUILayout.EndHorizontal();

            // EditorGUILayout.EndScrollView();
            return;
        }

        // 1. Bagian Informasi Dasar
        DrawBasicInfoSection(selectedObj);

        guideScrollPos = EditorGUILayout.BeginScrollView(guideScrollPos);
        BoxCollider2D liveBox2D =
            selectedObj != null ? selectedObj.GetComponent<BoxCollider2D>() : null;
        Rigidbody2D rigidbody2D =
            selectedObj != null ? selectedObj.GetComponent<Rigidbody2D>() : null;

        if (liveBox2D != null)
        {
            GUILayout.Space(5);

            // 1. KELOMPOK BOX COLLIDER 2D
            DrawComponentHeaderBox("📦  Box Collider 2D", ref showBoxCollider2DGuide);
            // Ambil komponen BoxCollider2D dari objek aktif secara live
            if (showBoxCollider2DGuide)
            {
                GUILayout.Space(5);
                // --- DAFTARKAN SEMUA CHILD HANDLER KE DALAM LIST ---
                List<IGuideable> boxColliderHandlers = new List<IGuideable>()
                {
                    new CustomBoxCollider2D_Material(),
                    new CustomBoxCollider2D_Istrigger(),
                    new CustomBoxCollider2D_UsedByEfector(),
                    new CustomBoxCollider2D_UsedByComposite(),
                    new CustomBoxCollider2D_AutoTiling(),
                    new CustomBoxCollider2D_Offset(),
                    new CustomBoxCollider2D_EdgeRadius(),
                };

                // --- CUKUP 1 LOOPING UNTUK MERENDER SEMUA KARTU GAYA CLEAN ARCHITECTURE ---
                foreach (var handler in boxColliderHandlers)
                {
                    // Eksekusi method GetData dari masing-masing child class secara polimorfisme
                    GuideItem guide = handler.GetData(liveBox2D);
                    string propName = guide.propertyName ?? "Unknown";

                    // Pastikan setiap properti punya state sendiri di dictionary
                    if (!guideStates.ContainsKey(propName))
                    {
                        guideStates[propName] = new PropertyState();
                    }

                    // Ambil state spesifik milik properti ini
                    PropertyState state = guideStates[propName];

                    // Tentukan teks efek aktif (jika objek null, berikan pesan panduan)
                    string activeText = guide.currentEffect;
                    // Render kartu guide ke UI
                    DrawGuideCard(
                        guide.propertyName ?? "Property",
                        guide.oneLineDesc ?? "Deskripsi panduan komponen.",
                        ref state.isEffectVisible, // (Catatan: idealnya state visibility dipisah per item jika ingin independen)
                        activeText
                    );

                    // GUILayout.Space(8);
                }

                GUILayout.Space(5);
            }
        }

        if (rigidbody2D != null)
        {
            GUILayout.Space(5);

            // 1. KELOMPOK BOX COLLIDER 2D
            // DrawComponentHeaderBox("📦  Box Collider 2D", ref showBoxCollider2DGuide);
            DrawComponentHeaderBox("🟢  Rigidbody 2D", ref showRigidbody2DGuide);
            // Ambil komponen BoxCollider2D dari objek aktif secara live
            // 2. KELOMPOK RIGIDBODY 2D
            GUILayout.Space(5);
        }
        List<IRigidbody2DGuideable> rigidbodyHandlers = new List<IRigidbody2DGuideable>
        {
            new Rigidbody2DBodyTypeGuide(),
            new Rigidbody2DMaterialGuide(),
            new Rigidbody2DSimulatedGuide(),
            new Rigidbody2DAutoMassGuide(),
            new Rigidbody2DMassGuide(),
            new Rigidbody2DLinearDampingGuide(),
            new Rigidbody2DAngularDampingGuide(),
            new Rigidbody2DGravityScaleGuide(),
            new Rigidbody2DCollisionDetectionGuide(),
            new Rigidbody2DSleepingModeGuide(),
            new Rigidbody2DInterpolateGuide(),
            new Rigidbody2DConstraintsGuide(),
        };

        if (rigidbody2D != null && showRigidbody2DGuide)
        {
            foreach (IRigidbody2DGuideable handler in rigidbodyHandlers)
            {
                DrawGuideItem(handler.GetData(rigidbody2D));
            }
        }

        DrawComponentHeaderBox("Transform", ref showTransformGuide);
        if (showTransformGuide)
        {
            List<ITransformGuideable> transformHandlers = new List<ITransformGuideable>
            {
                new TransformPositionGuide(),
                new TransformRotationGuide(),
                new TransformScaleGuide(),
            };

            foreach (ITransformGuideable handler in transformHandlers)
            {
                DrawGuideItem(handler.GetData(selectedObj.transform));
            }
        }

        EditorGUILayout.EndScrollView();
    }

    private void DrawGuideItem(GuideItem guide)
    {
        string propName = guide.propertyName ?? "Property";
        if (!guideStates.ContainsKey(propName))
        {
            guideStates[propName] = new PropertyState();
        }

        PropertyState state = guideStates[propName];
        DrawGuideCard(
            propName,
            guide.oneLineDesc ?? "Deskripsi panduan komponen.",
            ref state.isEffectVisible,
            guide.currentEffect ?? "Belum ada data efek saat ini."
        );
    }

    private void DrawStructureTab()
    {
        List<string> createdFolders = LoadCreatedFolders();
        GUIStyle titleStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 18,
            normal = { textColor = new Color(0.87f, 0.87f, 0.87f) }
        };
        GUIStyle orangeStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 11,
            wordWrap = true,
            normal = { textColor = new Color(0.95f, 0.47f, 0.16f) }
        };
        GUIStyle mutedStyle = new GUIStyle(EditorStyles.label)
        {
            fontSize = 11,
            wordWrap = true,
            normal = { textColor = new Color(0.68f, 0.68f, 0.68f) }
        };
        GUIStyle itemStyle = new GUIStyle(EditorStyles.label)
        {
            fontSize = 12,
            normal = { textColor = new Color(0.90f, 0.90f, 0.90f) }
        };

        structureScrollPos = EditorGUILayout.BeginScrollView(structureScrollPos);
        GUILayout.Space(8);
        GUILayout.BeginHorizontal();
        GUILayout.Space(12);
        GUILayout.BeginVertical(GUILayout.Width(Mathf.Max(220, position.width - 36)));

        GUILayout.Label("Project Structure", titleStyle);
        GUILayout.Space(5);
        GUILayout.Label(
            "Informasi folder yang berhasil dibuat dari preset project sebelumnya.",
            orangeStyle
        );

        string preset = EditorPrefs.GetString("SmartLinty_LastFolderPreset", "");
        if (!string.IsNullOrEmpty(preset))
        {
            GUILayout.Space(7);
            GUILayout.Label($"Preset: {preset}", mutedStyle);
        }

        GUILayout.Space(10);
        if (createdFolders.Count == 0)
        {
            EditorGUILayout.HelpBox(
                "Belum ada hasil Folder Maker yang tersimpan. Jalankan Folder Setup terlebih dahulu.",
                MessageType.Info
            );

            if (GUILayout.Button("Open Folder Setup", GUILayout.Height(30)))
                FolderStructureLintyPopup.OpenWindow();
        }
        else
        {
            GUILayout.Label(
                $"{createdFolders.Count} folder siap digunakan",
                mutedStyle
            );
            GUILayout.Space(5);
            DrawStructureFolderList(createdFolders, itemStyle, mutedStyle);
        }

        GUILayout.EndVertical();
        GUILayout.Space(12);
        GUILayout.EndHorizontal();
        EditorGUILayout.EndScrollView();
    }

    private List<string> LoadCreatedFolders()
    {
        string savedFolders = EditorPrefs.GetString("SmartLinty_CreatedFolders", "");
        if (!string.IsNullOrEmpty(savedFolders))
        {
            return new List<string>(
                savedFolders.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)
            );
        }

        string genre = EditorPrefs.GetString("SmartLinty_Genre", "");
        string projectType = EditorPrefs.GetString("SmartLinty_ProjectType", "");
        if (string.IsNullOrEmpty(genre) || string.IsNullOrEmpty(projectType))
            return new List<string>();

        List<string> existingFolders = new List<string>();
        List<string> presetFolders = FolderMakerDatabase.GetAllFolders(
            genre,
            projectType,
            true,
            true,
            true,
            true,
            true,
            true
        );

        foreach (string folder in presetFolders)
        {
            if (AssetDatabase.IsValidFolder(folder))
                existingFolders.Add(folder);
        }

        return existingFolders;
    }

    private void DrawStructureFolderList(
        List<string> folders,
        GUIStyle itemStyle,
        GUIStyle descriptionStyle
    )
    {
        structureFolderListScrollPos = EditorGUILayout.BeginScrollView(
            structureFolderListScrollPos,
            false,
            true,
            GUILayout.Height(200)
        );

        for (int i = 0; i < folders.Count; i++)
        {
            string folder = folders[i];
            Rect rowRect = GUILayoutUtility.GetRect(
                Mathf.Max(220, position.width - 50),
                30,
                GUILayout.Height(30),
                GUILayout.ExpandWidth(true)
            );
            EditorGUI.DrawRect(rowRect, new Color(0.14f, 0.14f, 0.14f));

            GUIContent folderIcon = EditorGUIUtility.IconContent("Folder Icon");
            if (folderIcon.image != null)
            {
                GUI.DrawTexture(
                    new Rect(rowRect.x + 7, rowRect.y + 6, 18, 17),
                    folderIcon.image,
                    ScaleMode.ScaleToFit
                );
            }

            GUI.Label(
                new Rect(rowRect.x + 33, rowRect.y + 4, rowRect.width - 70, 22),
                folder,
                itemStyle
            );

            GUI.Label(
                new Rect(rowRect.x + rowRect.width - 32, rowRect.y + 2, 25, 25),
                expandedStructureFolderIndex == i ? "v" : ">",
                new GUIStyle(EditorStyles.boldLabel)
                {
                    fontSize = 16,
                    alignment = TextAnchor.MiddleCenter,
                    normal = { textColor = new Color(0.72f, 0.72f, 0.72f) }
                }
            );

            if (GUI.Button(rowRect, GUIContent.none, GUIStyle.none))
            {
                expandedStructureFolderIndex =
                    expandedStructureFolderIndex == i ? -1 : i;
                Repaint();
            }

            if (expandedStructureFolderIndex == i)
            {
                Rect detailRect = GUILayoutUtility.GetRect(
                    Mathf.Max(200, position.width - 70),
                    45,
                    GUILayout.Height(45),
                    GUILayout.ExpandWidth(true)
                );
                EditorGUI.DrawRect(detailRect, new Color(0.12f, 0.12f, 0.12f));
                GUI.Label(
                    new Rect(detailRect.x + 9, detailRect.y + 6, detailRect.width - 18, 34),
                    DescribeStructureFolder(folder),
                    descriptionStyle
                );
            }
        }

        EditorGUILayout.EndScrollView();
    }

    private string DescribeStructureFolder(string path)
    {
        if (path.Contains("/Scripts"))
            return "Folder untuk script dan logic sistem project.";
        if (path.Contains("/Prefabs"))
            return "Folder untuk prefab yang dapat digunakan kembali.";
        if (path.Contains("/Scenes") || path.Contains("/Levels"))
            return "Folder untuk scene, level, map, atau area permainan.";
        if (path.Contains("/Audio"))
            return "Folder untuk musik, suara, voice, dan ambience.";
        if (path.Contains("/UI"))
            return "Folder untuk interface, HUD, menu, dan elemen tampilan.";
        if (path.Contains("/Models") || path.Contains("/Sprites") || path.Contains("/Textures"))
            return "Folder untuk asset visual project.";
        if (path.Contains("/Data"))
            return "Folder untuk data konfigurasi dan gameplay.";
        return "Folder bagian dari struktur standar project.";
    }

    private void DrawSettingTab()
    {
        EditorGUILayout.HelpBox("Halaman Setting: Pengaturan SmartLinty.", MessageType.Info);
    }
    #endregion

    #region Component Diagnostics & UI Helpers
    private void DrawWarningBanner(string message)
    {
        Rect bannerRect = EditorGUILayout.GetControlRect(false, 38);
        bannerRect.x += 10;
        bannerRect.width -= 20;

        EditorGUI.DrawRect(bannerRect, new Color(0.85f, 0.45f, 0.15f));
        GUI.Label(
            bannerRect,
            $" ⚠️  {message}",
            new GUIStyle(EditorStyles.boldLabel)
            {
                normal = { textColor = Color.white },
                fontSize = 12,
                alignment = TextAnchor.MiddleLeft,
            }
        );
    }

    private void DrawComponentHeaderBox(string title, ref bool foldoutStatus)
    {
        GUIStyle headerStyle = new GUIStyle(EditorStyles.toolbarButton)
        {
            alignment = TextAnchor.MiddleLeft,
            fontStyle = FontStyle.Bold,
            fontSize = 11,
            normal = { textColor = Color.white },
        };

        Color oldBg = GUI.backgroundColor;
        GUI.backgroundColor = new Color(0.28f, 0.28f, 0.28f);

        if (
            GUILayout.Button(
                $" {(foldoutStatus ? "▼" : "▶")}  {title}",
                headerStyle,
                GUILayout.Height(24)
            )
        )
        {
            foldoutStatus = !foldoutStatus;
        }
        GUI.backgroundColor = oldBg;
    }

    private void DrawGuideCard(
        string propertyName,
        string oneLineDesc,
        ref bool isEffectShown,
        string effectDetailText
    )
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

        // Baris Atas: Nama Property & Tombol "Tell Me More"
        GUILayout.BeginHorizontal();
        GUILayout.Space(3);
        GUIStyle propNameStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            normal = { textColor = Color.white },
        };
        GUILayout.Label(propertyName, propNameStyle);

        GUILayout.FlexibleSpace();

        GUIStyle linkStyle = new GUIStyle(EditorStyles.label)
        {
            // PERBAIKAN: Menggunakan 'new Color(...)' yang benar
            normal = { textColor = new Color(0.4f, 0.8f, 0.4f) },
            // fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleRight,
            fontSize = 12,
            padding = new RectOffset(0, 12, 0, 0),
        };
        if (GUILayout.Button("Tell Me More", linkStyle, GUILayout.Width(position.width - 230))) { }
        GUILayout.EndHorizontal();

        GUILayout.Space(3);

        // Deskripsi Singkat
        GUIStyle descStyle = new GUIStyle(EditorStyles.wordWrappedLabel)
        {
            normal = { textColor = new Color(0.8f, 0.8f, 0.8f) },
            fontSize = 10,
        };

        GUILayout.Label(oneLineDesc, descStyle);

        // GUILayout.Space(3);

        // Styling untuk teks di dalam kotak hijau
        GUIStyle greenTextStyle = new GUIStyle(EditorStyles.label)
        {
            normal = { textColor = Color.black },
            fontSize = 9,
            fontStyle = FontStyle.Bold,
            wordWrap = true,
        };

        string displayText = isEffectShown
            ? $"Current Effect :\n{effectDetailText}"
            : "Current Effect : ...";

        // PERBAIKAN: Tinggikan minimal box menjadi 95f agar teks panjang muat dengan sempurna tanpa terpotong
        float calculatedHeight = isEffectShown ? 70f : 26f;

        // Ambil rect untuk kartu utama
        Rect cardRect = EditorGUILayout.GetControlRect(false, calculatedHeight);

        // Background gelap kartu
        // EditorGUI.DrawRect(cardRect, new Color(0.15f, 0.15f, 0.15f));

        // Kotak Hijau Muda
        Rect greenBoxRect = new Rect(
            cardRect.x + 3,
            cardRect.y + 4,
            cardRect.width - 40,
            cardRect.height - 8
        );
        EditorGUI.DrawRect(greenBoxRect, new Color(0.72f, 0.95f, 0.35f));

        // Render Teks di dalam kotak hijau dengan margin aman
        Rect textRect = new Rect(
            greenBoxRect.x + 6,
            greenBoxRect.y + 6,
            greenBoxRect.width - 12,
            greenBoxRect.height - 12
        );
        GUI.Label(textRect, displayText, greenTextStyle);

        // Tombol Ikon Mata (Eye Toggle) di sebelah kanan kotak hijau
        Rect eyeBtnRect = new Rect(
            cardRect.x + cardRect.width - 32,
            cardRect.y + (cardRect.height / 2) - 10,
            26,
            20
        );
        GUIStyle eyeStyle = new GUIStyle(GUI.skin.button) { fontSize = 11 };
        if (GUI.Button(eyeBtnRect, "👁️", eyeStyle))
        {
            isEffectShown = !isEffectShown;
        }

        EditorGUILayout.EndVertical();
    }
    #endregion
    private void DrawFileRelocatorTab()
    {
        if (fileRelocatorRenderer == null)
            fileRelocatorRenderer = new FileRelocatorTabRenderer();

        fileRelocatorRenderer.Draw(
            new Rect(0, 0, position.width, position.height),
            normal_text,
            hover_text,
            active_text
        );
    }


}
