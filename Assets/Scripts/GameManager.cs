using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public enum Mod { Normal, Production };

public class GameManager : MonoBehaviour
{
    [SerializeField] private Material mat_normal;
    [SerializeField] private Material mat_selected;
    [SerializeField] private GameObject informationPanel;
    [SerializeField] private TMP_Text buildingName;

    [SerializeField] private Image buildingImage;
    [SerializeField] private Button productionButton;
    [SerializeField] private TMP_Text productionImageName;
    [SerializeField] private Sprite[] buildingImages;
    [SerializeField] private GameObject selectedBuilding;
    [SerializeField] private GameObject[] buildings;
    [SerializeField] private Material unselectableMat;
    [SerializeField] private Material selectableMat;
    [SerializeField] private BuildingController buildingController;

    private GameObject currentSelected;
    private GameObject producedBuilding;

    private Collider2D selectedBuildingCol;
    private SpriteRenderer selectedBuildingRend;
    private Material selectedBuildingMat;
    private Material selectedBuildingMatOriginal;



    Mod currentMod = Mod.Normal;

    public static GameManager Instance;

    private void Awake() 
    {
        Instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void UpdateUI(bool isSelected)
    {
        if (isSelected)
        {
            LayerMask lm = LayerMask.GetMask("Building");
                if (lm == (lm | (1 << currentSelected.layer)))
                {
                    BuildingController b = currentSelected.GetComponentInParent<BuildingController>();
                    if (b != null)
                    {
                        buildingName.text = b.buildingName;
                        buildingImage.sprite = buildingImages[b.imageIndex];
                        if (b.imageIndex == 0)
                        {
                            productionButton.gameObject.SetActive(true);
                            productionImageName.gameObject.SetActive(true);
                        }
                        else
                        {
                            productionButton.gameObject.SetActive(false);
                            productionImageName.gameObject.SetActive(false);
                        }
                    }
                }
        informationPanel.SetActive(true);
        }
        else
        {
            informationPanel.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (currentMod == Mod.Normal)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero, 100f, LayerMask.GetMask("Building"));
                if (hit.collider != null)
                {
                    if (currentSelected != null)
                    {
                        SpriteRenderer srCurrent = currentSelected.GetComponent<SpriteRenderer>();
                        if (srCurrent != null)
                        {
                            srCurrent.material = mat_normal;
                        }
                    }

                    // we've selected something
                    currentSelected = hit.transform.gameObject;
                    SpriteRenderer sr = currentSelected.GetComponent<SpriteRenderer>();
                    if (sr != null)
                    {
                        sr.material = mat_selected;
                    }
                    UpdateUI(true);
                }
                else
                {
                    // clicked on the empty
                    if (currentSelected != null)
                    {
                        SpriteRenderer srCurrent = currentSelected.GetComponent<SpriteRenderer>();
                        if (srCurrent != null)
                        {
                            srCurrent.material = mat_normal;
                        }
                        srCurrent = null;

                        UpdateUI(false);
                    }   
                }
            }
            else if (Input.GetMouseButtonDown(1))
            {
                if (currentSelected != null)
                {
                    LayerMask lm = LayerMask.GetMask("Building");
                    if (lm == (lm | (1 << currentSelected.layer)))
                    {
                        Vector2 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero, 100f, LayerMask.GetMask("Ground"));
                        if (hit.collider != null)
                        {
                            BuildingController building = currentSelected.GetComponentInParent<BuildingController>();
                            if (building != null)
                            {
                                building.SetDestinationPoint(hit.point);
                            }
                        }
                    }
                }
            }
        }
        else if (currentMod == Mod.Production && selectedBuilding != null)
        {
            // if mouse is on the ground, update selectedBuilding's pos
            Vector2 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero, 100f, LayerMask.GetMask("Ground"));
            bool canPutBuilding = false;
            if (hit.collider != null)
            {
                selectedBuilding.transform.position = hit.point;
                selectedBuilding.SetActive(true);
                // update selectedBuilding's color on collision
                if (Physics2D.OverlapBox(selectedBuilding.transform.position, selectedBuildingCol.bounds.size / 2, 90, LayerMask.GetMask("Building", "Wall")))
                {
                    selectedBuildingRend.material = unselectableMat;
                }
                else
                {
                    selectedBuildingRend.material = selectableMat;
                    canPutBuilding = true;
                }
            }
            else
            {
                selectedBuilding.SetActive(false);
            }

            if (Input.GetMouseButtonDown(0))
            {
                if (canPutBuilding)
                {
                    currentMod = Mod.Normal;
                    selectedBuildingRend.material = selectedBuildingMatOriginal;
                    producedBuilding = Instantiate(selectedBuilding);
                    SetLayerRecursively(producedBuilding, 7);

                    selectedBuilding.SetActive(false);
                    selectedBuilding = null;
                }
            }
            else if (Input.GetMouseButtonDown(1))
            {
                // go back to normal mod on left click
                currentMod = Mod.Normal;
                selectedBuilding.SetActive(false);
                selectedBuildingRend.material = selectedBuildingMatOriginal;
                selectedBuilding = null;
            }
        }
        
    }

    public Vector2 SetDestinationPos(Vector2 destinationPos)
    {
        destinationPos = producedBuilding.transform.GetChild(2).position;
        return destinationPos;
    }

    public void ProduceBuilding()
    {
        currentMod = Mod.Production;
        selectedBuilding = buildings[0];
        selectedBuildingCol = selectedBuilding.GetComponentInChildren<Collider2D>();
        selectedBuildingRend = selectedBuilding.GetComponentInChildren<SpriteRenderer>();
        selectedBuildingMatOriginal = selectedBuildingRend.material;
    }

    private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (null == obj)
        {
            return;
        }

        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            if (null == child)
            {
                continue;
            }
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

}
