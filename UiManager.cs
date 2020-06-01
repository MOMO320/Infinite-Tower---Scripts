using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager instance;

    public static UiManager getInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(UiManager)) as UiManager;
            }
            if (instance == null)
            {
                GameObject obj = new GameObject("obj");
                instance = obj.AddComponent(typeof(UiManager)) as UiManager;
            }
            return instance;
        }
    }

    private GameObject uiBuyInfo;
    private GameObject uiUnitCard;
    private GameObject what;

    public Transform[] iconTrans;
    public GameObject iconParent;
    public int iconTransNum = 0;

    private bool isOpenBuyShop;
    private bool isOpenDescript;
    public bool isSelected;
    private TowerController towerController;
    private Image[] SpawnImages;

    private Animation animation;
    private GameObject DescriptionOBj;
    private GameObject UiCardCanvas;

    private CameraMove Camera;

    public Text unitName;
    public Text unitCount;
    public Text unitHp;
    public Text unitAttack;
    public Text unitSpeed;
    public Text unitAbility1;
    public Text unitAbility2;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        uiBuyInfo = GameObject.FindWithTag("CastleShopUi").transform.Find("Canvas").gameObject;
        what = GameObject.FindWithTag("UiBuy");
        towerController = GameObject.FindWithTag("PlayerTower").GetComponent<TowerController>();
        DescriptionOBj = what.transform.Find("UnitDescrition").gameObject;
       
        SpawnImages = what.transform.Find("UnitView").gameObject.GetComponentsInChildren<Image>();
         isSelected = false;
        isOpenBuyShop = false;
        isOpenDescript = false;
        animation = GetComponent<Animation>();
    }

    private void Start() 
    {
        //foreach(Image img in SpawnImages)
        //{
        //    img.enabled = false;
        //}
        Camera = GameObject.FindWithTag("MainCamera").GetComponent<CameraMove>();
    }

    private void Update()
    {
        SpawnImages = what.transform.Find("UnitView").gameObject.GetComponentsInChildren<Image>();
        if (instance == null)
        {
            instance = this;
        }
        if(Camera.isMoveCamera == true && isOpenBuyShop == true && isOpenDescript == false)
        {
            towerController.outLine.enabled = false;
            isSelected = false;
            UiBuyActive(isSelected);
            BuyButtonCLick(isSelected);
        }
        else if(Camera.isMoveCamera == true && isOpenBuyShop == true && isOpenDescript == true)
        {
            ExitButtonClick();
        }
        
    }

    // Tower UI - 1단계
    public void UiBuyActive(bool _isActive)
    { 
        uiBuyInfo.SetActive(_isActive);
        isOpenBuyShop = _isActive;
        what.transform.Find("ExitButton").gameObject.SetActive(_isActive);
    }

    // Tower UI - 2단계
    public void BuyButtonCLick(bool _isAtctive)
    {
        //if(isOpenBuyShop == true)
        //{
        isOpenDescript = true;
        what.transform.Find("UnitCard").gameObject.SetActive(_isAtctive);
        //foreach (Image img in SpawnImages)
        //{
        //    img.enabled = _isAtctive;
        //}
        //}
    }

    // canvas Text 
    public void DescriptText(string _name , int _unitCount ,int _hp , int _attack , float _speed, string _descript1 , string _descript2)
    {
        unitName = GameObject.FindWithTag("NameText").GetComponent<Text>();
        unitCount = GameObject.FindWithTag("UnitText").GetComponent<Text>();
        unitHp = GameObject.FindWithTag("HpText").GetComponent<Text>();
        unitAttack = GameObject.FindWithTag("AttackText").GetComponent<Text>();
        unitSpeed = GameObject.FindWithTag("SpeedText").GetComponent<Text>();
        unitAbility1 = GameObject.FindWithTag("AbilityText").GetComponent<Text>();
        unitAbility2 = GameObject.FindWithTag("AbilityText2").GetComponent<Text>();

        unitName.text = "NAME : " + _name;
        unitCount.text = "UNIT : " + _unitCount + " 유닛";
        unitHp.text = "HP : " + _hp;
        unitAttack.text = "ATTACK : " + _attack;
        unitSpeed.text = "SPEED : " + _speed;
        unitAbility1.text = _descript1;
        unitAbility2.text = _descript2; 
    }

    public void ExitButtonClick()
    {
        towerController.outLine.enabled = false;
        isSelected = false;
        UiBuyActive(isSelected);
        BuyButtonCLick(isSelected);
        animation = DescriptionOBj.GetComponent<Animation>();
        animation.CrossFade("PaperDownAni");
        isOpenDescript = false;
    }

    public void UnitCardButton(GameObject _chooseUnit , GameObject _chooseIcon, int _money , int _unitCount)
    {
        for (int i = 0; i < _unitCount; i++)
        {
            GameManager.instance.selectSlotUnit[GameManager.instance.Lv1_ChooseIcon.Count].Add(_chooseUnit);
        }
        GameManager.instance.Lv1_ChooseIcon.Add(_chooseIcon);
        GameManager.instance.Money -= _money;
    }
    
    public void UnitIconPosition(Transform _iconTrans , GameObject _chooseIcon)
    {
        _chooseIcon.transform.position = new Vector3(0,0,0);
        GameObject ChooseIcon = Instantiate<GameObject>(_chooseIcon, iconTrans[GameManager.instance.Lv1_ChooseIcon.Count - 1].transform.transform);
    }

    // 유닛의 스폰 타임 , 1초당 빼야 하는 수
    public void UnitSpawnTime(Slider _slide , float _second) 
    {
       _slide.value -= (_second * Time.deltaTime) / 100;
    }

}
