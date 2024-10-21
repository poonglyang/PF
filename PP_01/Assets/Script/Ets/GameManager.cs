using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using System.IO;


public class GameManager : MonoBehaviour
{
    // 맵 전체를 아우르며 정보 전달의 역활을 하는 클래스
    // 가지고 있는 재화, 총기의 기본 스팩 설정 정보
    // 저장 및 불러오기를 한다

    public static GameManager instance = null;

    bool isFirst = false;

    public static GameManager Instance
    {
        get
        {
            if (instance == null) // 이전에 인스턴스가 만들어진 적이 없으면
            {


            }
            return instance; // 있던 것이나 새로 만든 것을 리턴
        }
    }

    // --------------------------코인 부분---------------------------------

    /// <summary>
    /// 코인 UI를 지금 가지고 있는 코인개수로 바꿈
    /// </summary>
    public int coin = 0;

    public int Coin
    {
        get => coin;
        set
        {
            coin = value;
            if (coinUIText == null)
            {
                coinUIText = FindAnyObjectByType<CoinUI>();
            }
            coinUIText.Goods = coin;
        }
    }

    CoinUI coinUIText;

    // --------------------------------------------------------------------

    // -------------------------캐시 부분----------------------------------

    /// <summary>
    /// 캐쉬 UI를 지금 가지고 있는 캐쉬개수로 바꿈
    /// </summary>
    public int cash = 0;

    public int Cash
    {
        get => cash;
        set
        {
            cash = value;
            if (cashUIText == null)
                cashUIText = FindAnyObjectByType<CashUI>();
            cashUIText.Goods = cash;
        }
    }

    CashUI cashUIText;

    // --------------------------------------------------------------------

    // -------------------------상점 부분----------------------------------

    public bool ResetValue = false;

    public enum ShopObjcet
    {
        pistolRange,
        pistolBulletSpeed,
        pistolDamage,
        pistolFireRate,
        shotgunRange,
        shotgunBulletSpeed,
        shotgunDamage,
        shotgunFireRate,
        ARRange,
        ARBulletSpeed,
        ARDamage,
        ARFireRate,
        SRRange,
        SRBulletSpeed,
        SRDamage,
        SRFireRate
    }

    [Header("상점 총기 - 성능 별 구매 단계")]
    public int[] ShopObjectStep = new int[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

    /// <summary>
    /// 2차원은 느리니 1차로 바꿔서 쓰자
    /// </summary>
    //public int[,] shopobjectprice = new int[12, 3] {
    //    { 5, 10, 15 }, { 5, 10, 15 }, { 5, 10, 15 },
    //    { 5, 10, 15 }, { 5, 10, 15 }, { 5, 10, 15 },
    //    { 5, 10, 15 }, { 5, 10, 15 }, { 5, 10, 15 },
    //    { 5, 10, 15 }, { 5, 10, 15 }, { 5, 10, 15 }};

    [SerializeField]
    public int[] ShopObjectPrice2 = new int[48] {
        5, 10, 15,  // 상점 단계별 권총 사거리 구매 가격
        5, 10, 15,  // 상점 단계별 권총 탄속 구매 가격
        5, 10, 15,  // 상점 단계별 권총 데미지 구매 가격
        5, 10, 15,  // 상점 단계별 권총 연사속도 구매 가격
        5, 10, 15,  // 상점 단계별 샷건 사거리 구매 가격
        5, 10, 15,  // 상점 단계별 샷건 탄속 구매 가격
        5, 10, 15,  // 상점 단계별 샷건 데미지 구매 가격
        5, 10, 15,  // 상점 단계별 샷건 연사속도 구매 가격
        5, 10, 15,  // 상점 단계별 AR 사거리 구매 가격
        5, 10, 15,  // 상점 단계별 AR 탄속 구매 가격
        5, 10, 15,  // 상점 단계별 AR 데미지 구매 가격
        5, 10, 15,  // 상점 단계별 AR 연사속도 구매 가격
        5, 10, 15,  // 상점 단계별 SR 사거리 구매 가격
        5, 10, 15,  // 상점 단계별 SR 탄속 구매 가격
        5, 10, 15,  // 상점 단계별 SR 데미지 구매 가격
        5, 10, 15   // 상점 단계별 SR 연사속도 구매 가격
    };

    // --------------------------------------------------------------------

    // -------------------------총기 설정----------------------------------

    [SerializeField]
    public float[] BulletInfo = new float[48] {
        15, 10, 2, 2,       // 1단계 권총 사거리, 권총 탄속, 권총 데미지, 권총 연사속도
        17, 13, 4, 1.8f,    // 2단계 권총 사거리, 권총 탄속, 권총 데미지, 권총 연사속도
        20, 15, 7, 1.6f,    // 3단계 권총 사거리, 권총 탄속, 권총 데미지, 권총 연사속도
        8, 7, 1f, 2.5f,  // 1단계 샷건 사거리, 샷건 탄속, 샷건 데미지, 샷건 연사속도
        10, 9, 2f, 2.3f,  // 2단계 샷건 사거리, 샷건 탄속, 샷건 데미지, 샷건 연사속도
        13, 12, 4f, 2.0f,  // 3단계 샷건 사거리, 샷건 탄속, 샷건 데미지, 샷건 연사속도
        30, 15, 3, 1.5f,       // 1단계 AR 사거리, AR 탄속, AR 데미지, AR 연사속도
        35, 18, 6, 1.3f,       // 2단계 AR 사거리, AR 탄속, AR 데미지, AR 연사속도
        40, 24, 10, 1f,       // 3단계 AR 사거리, AR 탄속, AR 데미지, AR 연사속도
        60, 30, 20, 5.5f,     // 1단계 SR 사거리, SR 탄속, SR 데미지, SR 연사속도
        70, 40, 30, 5f,     // 2단계 SR 사거리, SR 탄속, SR 데미지, SR 연사속도
        80, 50, 40, 4f      // 3단계 SR 사거리, SR 탄속, SR 데미지, SR 연사속도
    };

    [SerializeField]
    float[] BulletInfo2 = new float[64] {
        12, 15, 17, 20,         // 단계별 권총 사거리
        8, 10, 13, 15,         // 단계별 권총 탄속
        1.8f,2, 4, 7,            // 단계별 권총 데미지
        1.5f, 1.3f, 1f, 0.7f,      // 단계별 권총 연사 속도
        6.5f, 7.5f, 9, 12,          // 단계별 샷건 사거리
        5.5f, 7, 9, 12,           // 단계별 샷건 탄속
        0.8f, 1, 2, 4,            // 단계별 샷건 데미지
        2.2f, 2f, 1.8f, 1.6f,   // 단계별 샷건 연사 속도
        25, 30, 35, 40,         // 단계별 AR 사거리
        10, 12, 15, 18,         // 단계별 AR 탄속
        2.5f, 3, 6, 10,           // 단계별 AR 데미지
        1.1f, 0.9f, 0.7f, 0.5f,     // 단계별 AR 연사속도
        50, 60, 70, 80,         // 단계별 SR 사거리
        25, 30, 38, 48,         // 단계별 SR 탄속
        15, 20, 30, 40,         // 단계별 SR 데미지
        5f, 4.5f, 4, 3.3f          // 단계별 SR 연사 속도
    };

    public float GetBulletInfo(int objectType)
    {
        float objectTypeValue = BulletInfo2[4 * objectType + ShopObjectStep[objectType]];

        var enumValues = Enum.GetValues(typeof(ShopObjcet));


        foreach (var objectEnum in enumValues)
        {
            if (objectType == (int)objectEnum)
            {
                break;
            }
        }

        return objectTypeValue;
    }
    // --------------------------------------------------------------------

    private void OnValidate()
    {
        if (ResetValue)
        {
            ResetValueAll();
            ResetValue = false;
        }

    }

    void ResetValueAll()
    {
        BulletInfo2 = new float[64]{
        12, 15, 17, 20,         // 단계별 권총 사거리
        8, 10, 13, 15,         // 단계별 권총 탄속
        1.8f,2, 4, 7,            // 단계별 권총 데미지
        2.2f, 2, 1.8f, 1.2f,      // 단계별 권총 연사 속도
        6.5f, 7.5f, 9, 12,          // 단계별 샷건 사거리
        5.5f, 7, 9, 12,           // 단계별 샷건 탄속
        0.8f, 1, 2, 4,            // 단계별 샷건 데미지
        2.5f, 2.3f, 2.1f, 1.8f,   // 단계별 샷건 연사 속도
        25, 30, 35, 40,         // 단계별 AR 사거리
        10, 12, 15, 18,         // 단계별 AR 탄속
        2.5f, 3, 6, 10,           // 단계별 AR 데미지
        1.65f, 1.5f, 1.3f, 0.8f,     // 단계별 AR 연사속도
        50, 60, 70, 80,         // 단계별 SR 사거리
        25, 30, 38, 48,         // 단계별 SR 탄속
        15, 20, 30, 40,         // 단계별 SR 데미지
        6.2f, 5.5f, 5, 4          // 단계별 SR 연사 속도
        };


    }

    /// <summary>
    /// 세이브파일 이름
    /// </summary>
    const string SaveFileName = "Save.json";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            isFirst = true;
            DontDestroyOnLoad(instance.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        coinUIText = FindAnyObjectByType<CoinUI>();

        LoadData();

        if (coinUIText != null)
        {
            coinUIText.Goods = coin;
        }

        cashUIText = FindAnyObjectByType<CashUI>();

        if (cashUIText != null)
        {
            cashUIText.Goods = cash;
        }

    }

    private void OnEnable()
    {
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    void OnDisable()
    {
        // 활성화된 씬 변경 이벤트에서 해제
        SceneManager.activeSceneChanged -= OnActiveSceneChanged;
    }

    private void OnActiveSceneChanged(Scene arg0, Scene arg1)
    {
        if (isFirst)
        {
            Debug.Log($"씬 로드됨 코인 {Coin} 캐시 {Cash}");

            coinUIText = FindAnyObjectByType<CoinUI>();

            if (coinUIText != null)
            {
                coinUIText.Goods = coin;
            }

            cashUIText = FindAnyObjectByType<CashUI>();

            if (cashUIText != null)
            {
                cashUIText.Goods = cash;
            }
        }


    }

    void LoadData()
    {
        bool isSuccess = false;

        string path = $"{Application.dataPath}/Save/";

        if (Directory.Exists(path))
        {
            string fullPath = $"{path}{SaveFileName}";
            if (File.Exists(fullPath))
            {
                string jsonText = File.ReadAllText(fullPath);
                SaveData loadedData = JsonUtility.FromJson<SaveData>(jsonText);
                Coin = loadedData.coinData;
                Cash = loadedData.cashData;
                ShopObjectStep = loadedData.shopObjectStepData;

                isSuccess = true;
            }
        }

        if (!isSuccess) //로딩 실패시
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory($"{Application.dataPath}/Save");      // 폴더 없음 만들고
            }

            SetDefaultData();                                                   // 기본 데이터 불러옴
        }
    }

    void SetDefaultData()
    {
        Coin = 0;
        Cash = 0;
        ShopObjectStep = new int[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    }

    public void SaveGunData()
    {
        // Assets/Save/Save.json 으로 저장

        SaveData data = new SaveData();                                             // 직렬화 하는 클래스 생성
        data.coinData = Coin;                                                     // 데이터 설정
        data.cashData = Cash;
        data.shopObjectStepData = ShopObjectStep;

        string jsonText = JsonUtility.ToJson(data);                                 // json 문자열로 변환

        string path = $"{Application.dataPath}/Save/";                               // 경로 지정

        if (!Directory.Exists(path))                                                // 경로에 폴더가 있는지 없는지
        {
            // 없다면 폴더 생성
            Debug.Log("폴더 생성");
            Directory.CreateDirectory($"{Application.dataPath}/Save");              // 없으면 폴더 생성
        }

        File.WriteAllText($"{path}{SaveFileName}", jsonText);
    }

}