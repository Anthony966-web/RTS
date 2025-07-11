using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class UnitSelectionManager : MonoBehaviour
{
    public static UnitSelectionManager Instance { get; set; }

    public List<GameObject> allUnitsList = new List<GameObject>();
    public List<GameObject> unitsSelected = new List<GameObject>();

    public LayerMask clickable;
    public LayerMask ground;
    public GameObject groundMarker;
    private Camera cam;

    public LayerMask attackable;
    public bool attackCursorVisible;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    private void Start()
    {
        cam = Camera.main;
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            // If We Are Hitting A Clickable Object
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickable))
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    MultiSelect(hit.collider.gameObject);
                }
                else
                {
                    SelectByClicking(hit.collider.gameObject);
                }
            }
            else // If We Are Not Hitting A Clickable Object 
            {
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    DeselectAll();
                }
            }
        }


        if (Input.GetMouseButtonDown(1) && unitsSelected.Count > 0)
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            // If We Are Hitting A Clickable Object
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
            {
                groundMarker.transform.position = hit.point;

                groundMarker.SetActive(false);
                groundMarker.SetActive(true);
            }
        }

        // Attack Target

        if (unitsSelected.Count > 0 && AtleastOneOffensiveUnit(unitsSelected))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            // If We Are Hitting A Clickable Object
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, attackable))
            {
                Debug.Log("Enemy Hovered With Mouse");

                attackCursorVisible = true;

                if(Input.GetMouseButtonDown(1))
                {
                    Transform target = hit.transform;

                    foreach(GameObject unit in unitsSelected)
                    {
                        if(unit.GetComponent<AttackController>())
                        {
                            unit.GetComponent<AttackController>().targetToAttack = target;
                        }
                    }
                }
            }
        }
        else
        {
            attackCursorVisible = false;
        }

        CursorSelector();
    }

    void CursorSelector()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, Mathf.Infinity, clickable))
        {
            CursorManager.Instance.SetMarkerType(CursorManager.CursorType.Selectable);
        }
        else if (Physics.Raycast(ray, out hit, Mathf.Infinity, attackable)
            && unitsSelected.Count > 0 && AtleastOneOffensiveUnit(unitsSelected))
        {
            CursorManager.Instance.SetMarkerType(CursorManager.CursorType.Attackable);
        }
        else if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground) && unitsSelected.Count > 0)
        {
            print("Works1");
            CursorManager.Instance.SetMarkerType(CursorManager.CursorType.Walkable);
        }
        else
        {
            CursorManager.Instance.SetMarkerType(CursorManager.CursorType.None);
        }

    }



    private bool AtleastOneOffensiveUnit(List<GameObject> unitsSelected)
    {
        foreach (GameObject unit in unitsSelected)
        {
            if (unit.GetComponent<AttackController>())
            {
                return true;
            }
        }
        return false;
    }

    private void MultiSelect(GameObject unit)
    {
        if (!unitsSelected.Contains(unit))
        {
            unitsSelected.Add(unit);
            TriggerSelectionIndicator(unit, true);
            EnableUnitMovement(unit, true);
        }
        else
        {
            EnableUnitMovement(unit, false);
            TriggerSelectionIndicator(unit, false);
            unitsSelected.Remove(unit);
        }
    }

    public void DeselectAll()
    {
        foreach (GameObject unit in unitsSelected)
        {
            EnableUnitMovement(unit, false);
            TriggerSelectionIndicator(unit, false);
        }

        groundMarker.SetActive(false);

        unitsSelected.Clear();
    }

    private void SelectByClicking(GameObject unit)
    {
        DeselectAll();

        unitsSelected.Add(unit);

        TriggerSelectionIndicator(unit, true);
        EnableUnitMovement(unit, true);
    }

    private void EnableUnitMovement(GameObject unit, bool trigger)
    {
        unit.GetComponent<UnitMovement>().enabled = trigger;
    }

    private void TriggerSelectionIndicator(GameObject unit, bool trigger)
    {
        unit.transform.Find("Indicator").gameObject.SetActive(trigger);
    }

    internal void DragSelect(GameObject unit)
    {
       if(!unitsSelected.Contains(unit))
        {
            unitsSelected.Add(unit);
            TriggerSelectionIndicator(unit, true);
            EnableUnitMovement(unit, true);
        }
    }
}
