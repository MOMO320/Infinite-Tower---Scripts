using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static GameManager getInstance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType(typeof(GameManager)) as GameManager;
            }
            if(instance == null)
            {
                GameObject obj = new GameObject("obj");
                instance = obj.AddComponent(typeof(GameManager)) as GameManager;
            }
            return instance;
        }
    }
    public int Money { get; set; }
    public int UnitCount { get; set; }

    public bool isUnitOut { get; set; } // 유닛 리스폰타임 다됬는지 체크

    private Text unitCountText;
    private Text moneyCountText;

    public List<GameObject> Lv1_FirstUnit = new List<GameObject>();   // 1번 슬럿 유닛
    public List<GameObject> Lv1_SecondUnit = new List<GameObject>();  // 2번 슬럿 유닛
    public List<GameObject> Lv1_ThirdUnit = new List<GameObject>();   // 3번 슬럿 유닛
    public List<GameObject> Lv1_FourUnit = new List<GameObject>();    // 4번 슬럿 유닛

    // 슬럿들 담은 리스트의 리스트
    public List<List<GameObject>> selectSlotUnit = new List<List<GameObject>>();

    public List<GameObject> Lv1_ChooseIcon = new List<GameObject>();  // 슬럿 아이콘

    public GameObject[] Lv1_Units;  // 유닛 오브젝트 담기
    public GameObject[] Lv1_Icons;  // 아이콘 오브젝트 담기

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        unitCountText = GameObject.FindWithTag("UnitCountText").GetComponent<Text>();
        moneyCountText = GameObject.FindWithTag("MoneyCountText").GetComponent<Text>();
        selectSlotUnit.Add(Lv1_FirstUnit);
        selectSlotUnit.Add(Lv1_SecondUnit);
        selectSlotUnit.Add(Lv1_ThirdUnit);
        selectSlotUnit.Add(Lv1_FourUnit);
    }

    private void Start()
    {
        Money = 50;
    }

    private void FixedUpdate()
    {
        if (instance == null)
        {
            instance = this;
        }
        moneyCountText.text = Money.ToString();
        unitCountText.text = UnitCount.ToString();
    }

    public void SpwanUnit(List<GameObject> _unit, List<GameObject> _icon)
    {
        GameObject gameObject = null;

        for (int i = 0; i < _unit.Count; i++)
        {
            if (isUnitOut == true)
            {
                if(_unit[0].GetComponentInChildren<UnitBase>().name == _unit[i].GetComponentInChildren<UnitBase>().name)
                {
                    Instantiate(_unit[i], new Vector3(transform.position.x ,
                            transform.position.y, transform.position.z + 2f * i), Quaternion.identity);

                    _unit[i].name = _unit[i].GetComponentInChildren<UnitBase>().name + i;
                }
            }
            if(i == _unit.Count - 1)
            {
                isUnitOut = false;
                UnitCount += _unit[i].GetComponentInChildren<UnitBase>().Unit_Count;
               
                _unit.Clear();
                _icon.Remove(_icon[0]);
            }
        }


    }

}
