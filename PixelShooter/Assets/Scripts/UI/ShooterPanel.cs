using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

namespace PixelShooter.UI
{
    /// <summary>
    /// Manages the panel displaying available shooters
    /// </summary>
    public class ShooterPanel : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject shooterElementPrefab;
        [SerializeField] private Transform shooterContainer;
        [SerializeField] private TextMeshProUGUI panelTitle;

        [Header("Layout Settings")]
        [SerializeField] private float elementSpacing = 10f;
        [SerializeField] private bool useGridLayout = true;
        [SerializeField] private int maxColumns = 3;

        private List<ShooterUIElement> shooterElements = new List<ShooterUIElement>();
        private System.Action<Data.ShooterData> onShooterSelected;

        private void Awake()
        {
            if (shooterContainer == null)
            {
                shooterContainer = transform;
            }

            SetupLayout();
        }

        private void SetupLayout()
        {
            if (useGridLayout && shooterContainer != null)
            {
                GridLayoutGroup gridLayout = shooterContainer.GetComponent<GridLayoutGroup>();
                if (gridLayout == null)
                {
                    gridLayout = shooterContainer.gameObject.AddComponent<GridLayoutGroup>();
                }
                
                gridLayout.spacing = new Vector2(elementSpacing, elementSpacing);
                gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                gridLayout.constraintCount = maxColumns;
                gridLayout.cellSize = new Vector2(100f, 120f);
                gridLayout.childAlignment = TextAnchor.UpperCenter;
            }
        }

        public void InitializeShooters(List<Data.ShooterData> shooters, System.Action<Data.ShooterData> selectionCallback)
        {
            onShooterSelected = selectionCallback;
            ClearShooters();

            foreach (var shooterData in shooters)
            {
                CreateShooterElement(shooterData);
            }
        }

        private void CreateShooterElement(Data.ShooterData shooterData)
        {
            if (shooterElementPrefab == null || shooterContainer == null)
            {
                Debug.LogError("ShooterPanel: Missing prefab or container reference");
                return;
            }

            GameObject elementObj = Instantiate(shooterElementPrefab, shooterContainer);
            ShooterUIElement element = elementObj.GetComponent<ShooterUIElement>();
            
            if (element != null)
            {
                element.Initialize(shooterData, OnShooterElementSelected);
                shooterElements.Add(element);
            }
        }

        private void OnShooterElementSelected(Data.ShooterData selectedShooter)
        {
            // Update selection state for all shooters
            foreach (var element in shooterElements)
            {
                var data = element.GetShooterData();
                if (data != null)
                {
                    data.isSelected = (data == selectedShooter);
                    element.UpdateDisplay();
                }
            }

            onShooterSelected?.Invoke(selectedShooter);
        }

        public void UpdateShooterDisplays()
        {
            foreach (var element in shooterElements)
            {
                element.UpdateDisplay();
            }
        }

        private void ClearShooters()
        {
            foreach (var element in shooterElements)
            {
                if (element != null)
                {
                    Destroy(element.gameObject);
                }
            }
            shooterElements.Clear();
        }

        public void SetPanelTitle(string title)
        {
            if (panelTitle != null)
            {
                panelTitle.text = title;
            }
        }
    }
}
