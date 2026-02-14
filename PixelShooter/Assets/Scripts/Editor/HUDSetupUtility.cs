using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

namespace PixelShooter.Editor
{
    /// <summary>
    /// Editor utility to create HUD prefabs and setup
    /// </summary>
    public class HUDSetupUtility : EditorWindow
    {
        [MenuItem("PixelShooter/Setup HUD Prefabs")]
        public static void ShowWindow()
        {
            GetWindow<HUDSetupUtility>("HUD Setup");
        }

        private void OnGUI()
        {
            GUILayout.Label("HUD Setup Utility", EditorStyles.boldLabel);
            GUILayout.Space(10);

            if (GUILayout.Button("Create Shooter UI Element Prefab"))
            {
                CreateShooterElementPrefab();
            }

            GUILayout.Space(5);

            if (GUILayout.Button("Setup HUD in Scene"))
            {
                SetupHUDInScene();
            }
        }

        private static void CreateShooterElementPrefab()
        {
            // Create root GameObject
            GameObject shooterElement = new GameObject("ShooterUIElement");
            RectTransform rootRect = shooterElement.AddComponent<RectTransform>();
            rootRect.sizeDelta = new Vector2(100, 120);

            // Add Button component
            Button button = shooterElement.AddComponent<Button>();
            Image buttonImage = shooterElement.AddComponent<Image>();
            buttonImage.color = new Color(0.2f, 0.2f, 0.2f, 0.8f);

            // Add ShooterUIElement script
            UI.ShooterUIElement elementScript = shooterElement.AddComponent<UI.ShooterUIElement>();

            // Create background panel
            GameObject background = new GameObject("Background");
            background.transform.SetParent(shooterElement.transform, false);
            RectTransform bgRect = background.AddComponent<RectTransform>();
            bgRect.anchorMin = Vector2.zero;
            bgRect.anchorMax = Vector2.one;
            bgRect.offsetMin = Vector2.zero;
            bgRect.offsetMax = Vector2.zero;
            Image bgImage = background.AddComponent<Image>();
            bgImage.color = new Color(0.2f, 0.2f, 0.2f, 0.8f);

            // Create color indicator
            GameObject colorIndicator = new GameObject("ColorIndicator");
            colorIndicator.transform.SetParent(shooterElement.transform, false);
            RectTransform colorRect = colorIndicator.AddComponent<RectTransform>();
            colorRect.anchorMin = new Vector2(0.5f, 0.6f);
            colorRect.anchorMax = new Vector2(0.5f, 0.6f);
            colorRect.anchoredPosition = Vector2.zero;
            colorRect.sizeDelta = new Vector2(60, 60);
            Image colorImage = colorIndicator.AddComponent<Image>();
            colorImage.color = Color.white;

            // Create ball count text
            GameObject ballText = new GameObject("BallCountText");
            ballText.transform.SetParent(shooterElement.transform, false);
            RectTransform textRect = ballText.AddComponent<RectTransform>();
            textRect.anchorMin = new Vector2(0, 0);
            textRect.anchorMax = new Vector2(1, 0.3f);
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;
            TextMeshProUGUI tmpText = ballText.AddComponent<TextMeshProUGUI>();
            tmpText.text = "0";
            tmpText.fontSize = 24;
            tmpText.alignment = TextAlignmentOptions.Center;
            tmpText.color = Color.white;

            // Create selection border
            GameObject selectionBorder = new GameObject("SelectionBorder");
            selectionBorder.transform.SetParent(shooterElement.transform, false);
            RectTransform borderRect = selectionBorder.AddComponent<RectTransform>();
            borderRect.anchorMin = Vector2.zero;
            borderRect.anchorMax = Vector2.one;
            borderRect.offsetMin = new Vector2(-4, -4);
            borderRect.offsetMax = new Vector2(4, 4);
            Image borderImage = selectionBorder.AddComponent<Image>();
            borderImage.color = new Color(1f, 0.9f, 0.3f, 1f);
            selectionBorder.SetActive(false);

            // Create used overlay
            GameObject usedOverlay = new GameObject("UsedOverlay");
            usedOverlay.transform.SetParent(shooterElement.transform, false);
            RectTransform overlayRect = usedOverlay.AddComponent<RectTransform>();
            overlayRect.anchorMin = Vector2.zero;
            overlayRect.anchorMax = Vector2.one;
            overlayRect.offsetMin = Vector2.zero;
            overlayRect.offsetMax = Vector2.zero;
            Image overlayImage = usedOverlay.AddComponent<Image>();
            overlayImage.color = new Color(0, 0, 0, 0.7f);
            usedOverlay.SetActive(false);

            // Set references using reflection or serialized fields
            SerializedObject so = new SerializedObject(elementScript);
            so.FindProperty("colorIndicator").objectReferenceValue = colorImage;
            so.FindProperty("ballCountText").objectReferenceValue = tmpText;
            so.FindProperty("selectionBorder").objectReferenceValue = borderImage;
            so.FindProperty("backgroundPanel").objectReferenceValue = bgImage;
            so.FindProperty("usedOverlay").objectReferenceValue = usedOverlay;
            so.ApplyModifiedProperties();

            // Save as prefab
            string path = "Assets/Prefabs";
            if (!AssetDatabase.IsValidFolder(path))
            {
                AssetDatabase.CreateFolder("Assets", "Prefabs");
            }

            string prefabPath = path + "/ShooterUIElement.prefab";
            PrefabUtility.SaveAsPrefabAsset(shooterElement, prefabPath);
            
            DestroyImmediate(shooterElement);
            
            Debug.Log($"Shooter UI Element prefab created at {prefabPath}");
            EditorUtility.DisplayDialog("Success", "Shooter UI Element prefab created successfully!", "OK");
        }

        private static void SetupHUDInScene()
        {
            // Find or create Canvas
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                GameObject canvasObj = new GameObject("HUDCanvas");
                canvas = canvasObj.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvasObj.AddComponent<CanvasScaler>();
                canvasObj.AddComponent<GraphicRaycaster>();
            }

            // Create HUD Manager
            GameObject hudManager = new GameObject("HUDManager");
            hudManager.transform.SetParent(canvas.transform, false);
            RectTransform hudRect = hudManager.AddComponent<RectTransform>();
            hudRect.anchorMin = Vector2.zero;
            hudRect.anchorMax = Vector2.one;
            hudRect.offsetMin = Vector2.zero;
            hudRect.offsetMax = Vector2.zero;

            UI.HUDManager hudScript = hudManager.AddComponent<UI.HUDManager>();

            // Create Safe Area Panel
            GameObject safeArea = new GameObject("SafeAreaPanel");
            safeArea.transform.SetParent(hudManager.transform, false);
            RectTransform safeRect = safeArea.AddComponent<RectTransform>();
            safeRect.anchorMin = Vector2.zero;
            safeRect.anchorMax = Vector2.one;
            safeRect.offsetMin = Vector2.zero;
            safeRect.offsetMax = Vector2.zero;

            // Create Top Panel for Progress Display
            GameObject topPanel = CreatePanel("TopPanel", safeArea.transform, 
                new Vector2(0, 1), new Vector2(1, 1), new Vector2(0, -200), new Vector2(0, 0));
            UI.ProgressDisplay progressDisplay = topPanel.AddComponent<UI.ProgressDisplay>();

            // Create Bottom Panel for Shooter Panel
            GameObject bottomPanel = CreatePanel("BottomPanel", safeArea.transform,
                new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 0), new Vector2(0, 300));
            UI.ShooterPanel shooterPanel = bottomPanel.AddComponent<UI.ShooterPanel>();
            CreateShooterPanelUI(bottomPanel.transform, shooterPanel);

            // Create Action Feedback Panel
            GameObject feedbackPanel = CreatePanel("ActionFeedbackPanel", safeArea.transform,
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(-150, -50), new Vector2(150, 50));
            UI.ShooterActionFeedback actionFeedback = feedbackPanel.AddComponent<UI.ShooterActionFeedback>();

            // Set references in HUD Manager
            SerializedObject so = new SerializedObject(hudScript);
            so.FindProperty("shooterPanelComponent").objectReferenceValue = shooterPanel;
            so.FindProperty("progressComponent").objectReferenceValue = progressDisplay;
            so.FindProperty("feedbackComponent").objectReferenceValue = actionFeedback;
            so.FindProperty("mainCanvas").objectReferenceValue = canvas;
            so.FindProperty("scaler").objectReferenceValue = canvas.GetComponent<CanvasScaler>();
            so.FindProperty("safeAreaRect").objectReferenceValue = safeRect;
            so.ApplyModifiedProperties();

            Debug.Log("HUD setup complete!");
            EditorUtility.DisplayDialog("Success", "HUD has been set up in the scene!", "OK");
        }

        private static GameObject CreatePanel(string name, Transform parent, Vector2 anchorMin, Vector2 anchorMax, Vector2 offsetMin, Vector2 offsetMax)
        {
            GameObject panel = new GameObject(name);
            panel.transform.SetParent(parent, false);
            RectTransform rect = panel.AddComponent<RectTransform>();
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.offsetMin = offsetMin;
            rect.offsetMax = offsetMax;
            return panel;
        }

        private static void CreateShooterPanelUI(Transform parent, UI.ShooterPanel shooterPanel)
        {
            GameObject container = new GameObject("ShooterContainer");
            container.transform.SetParent(parent, false);
            RectTransform rect = container.AddComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            GridLayoutGroup grid = container.AddComponent<GridLayoutGroup>();
            grid.cellSize = new Vector2(100, 120);
            grid.spacing = new Vector2(10, 10);
            grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            grid.constraintCount = 3;

            SerializedObject so = new SerializedObject(shooterPanel);
            so.FindProperty("shooterContainer").objectReferenceValue = container.transform;
            so.ApplyModifiedProperties();
        }
    }
}
