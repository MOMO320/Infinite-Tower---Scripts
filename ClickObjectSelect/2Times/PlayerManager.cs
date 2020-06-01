using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public static PlayerManager getInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(PlayerManager)) as PlayerManager;
            }

            if(instance == null)
            {
                GameObject obj = new GameObject("obj");
                instance = obj.AddComponent(typeof(PlayerManager)) as PlayerManager;
            }
            return instance;
        }
    }


    RaycastHit hit;
    public List<UnitController> selectedUnits = new List<UnitController>();
    TowerController selectedTower;
    bool isDragging = false;
    Vector3 mousePosition;
    public ParticleSystem mouseClickParticle;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        selectedTower = GameObject.FindWithTag("PlayerTower").GetComponent<TowerController>();
    }


    private void OnGUI()
    {
        if (isDragging)
        {
            var rect = ScreenHelper.GetScreenRect(mousePosition, Input.mousePosition);
            ScreenHelper.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            ScreenHelper.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePosition = Input.mousePosition;
            // create a ray from the camera to our space
            var camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            // shoot that ray and get the hit data
            if(Physics.Raycast(camRay , out hit))
            {
                //Do something with that data(playerUnit선택)
                if(hit.transform.parent.CompareTag("PlayerUnit"))
                {
                    SelectUnit(hit.transform.GetComponent<UnitController>(), Input.GetKey(KeyCode.LeftShift));
                }
                else if(hit.transform.tag == "PlayerTower")
                {
                    SelectTower(selectedTower);
                }
                else
                {
                    DeselectTower(selectedTower);
                    isDragging = true;
                }
            }
        }


        if(Input.GetMouseButtonUp(0))
        {
            if(isDragging)
            {
                DeselectUnits();
                foreach(var selectableObject in FindObjectsOfType<PlayerUnitController>())
                {
                    if(IsWithinSelectionBounds(selectableObject.transform))
                    {
                        SelectUnit(selectableObject.gameObject.GetComponent<UnitController>(), true);
                    }
                }

                isDragging = false;
            }
        }

        if(Input.GetMouseButtonDown(1) && selectedUnits.Count > 0)
        {
            var camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(camRay, out hit))
            {
                if (hit.transform.CompareTag("Ground"))
                {
                    foreach(var selectableObj in selectedUnits)
                    {
                        selectableObj.MoveUnit(hit.point);
                        //mouseClickParticle.gameObject.transform.localPosition = Input.mousePosition;
                       // mouseClickParticle.Play(true);
                    }
                }
                else if(hit.transform.CompareTag("EnemyUnit"))
                {
                    foreach(var selectableObj in selectedUnits)
                    {
                        selectableObj.SetNewTarget(hit.transform);
                    }
                }
            }
        }

        DieUnitDeselected();
    }

    private void SelectUnit(UnitController unit , bool isMultiSelect = false)
    {
        if(!isMultiSelect)
        {
            DeselectUnits();
        }
        selectedUnits.Add(unit);
        unit.SetSelected(true);
    }

    private void SelectTower(TowerController tower)
    {
        tower.SetSelected(true);
    }

    private void DeselectTower(TowerController tower)
    {
        tower.SetSelected(false);
    }

    private void DeselectUnits()
    {
        for(int i = 0; i < selectedUnits.Count; i++)
        {
            selectedUnits[i].SetSelected(false);
        }
        selectedUnits.Clear();
    }

    private bool IsWithinSelectionBounds(Transform transform)
    {
        if(!isDragging)
        {
            return false;
        }

        var camera = Camera.main;
        var viewportBounds = Utils.GetViewportBounds(camera, mousePosition, Input.mousePosition);
        return viewportBounds.Contains(camera.WorldToViewportPoint(transform.position));
    }

    private void DieUnitDeselected()
    {
        for(int i = 0; i < selectedUnits.Count; i++)
        {
            if(selectedUnits[i].GetComponentInChildren<UnitBase>().Unit_Hp <= 0 )
            {
                selectedUnits.RemoveAt(i);
            }
        }
    }
}
