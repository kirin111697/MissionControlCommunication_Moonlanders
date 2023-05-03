using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get => _instance; }
    static GameManager _instance;

    public List<GameObject> RadarSets;
    GameObject earth;
    [SerializeField]
    int playerCount = 2;
    int totalRadarCnt;
    List<GameObject>[] playerRadarLists;
    public List<Color32> colorPalette;

    public enum RadarID { one, two, three, four, NUM_STAT };
    
    bool[] radarDetected;
    int detectedAmt = 0;
    bool allDetected = false;
    public float targetToSuccess = 10f;
    float accumTime = 0f;
    bool taskComplete = false;

    public GameObject GoodZoneEffect;

    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            //DontDestroyOnLoad(this.gameObject);
        }
        //SetupPlayerRadars();
        //SetupColorPalette();
        //radarDetected = new bool[playerCount];
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (allDetected && !taskComplete)
        {
            accumTime += Time.deltaTime;
            UIControls.S.SetSuccessBar(accumTime);
            CheckTaskStatus();
        }

        if (taskComplete)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                ResetGame();
            }            
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            ReloadGame();
        }
    }

    public void ChangePlayerAmount(int value)
    {
        Debug.Log(value);
        playerCount = value + 2;
    }

    public void SetupGame()
    {
        SetupPlayerRadars();
        SetupColorPalette();
        radarDetected = new bool[playerCount];
        UIControls.S.HideSetupPanel();
        UIControls.S.ConfigureSuccessBar(targetToSuccess);
        UIControls.S.ActivateSuccessBar();
    }

    void ClearEarth()
    {
        foreach (GameObject obj in RadarSets)
        {
            obj.SetActive(false);
        }
    }

    void SetupPlayerRadars()
    {
        playerRadarLists = new List<GameObject>[playerCount];
        for (int i = 0; i < playerCount; i++)
        {
            playerRadarLists[i] = new List<GameObject>();
        }
        ClearEarth();
        if (playerCount == 3)
        {
            earth = RadarSets[1];
            earth.SetActive(true);
            totalRadarCnt = 6;
        }
        else
        {
            earth = RadarSets[0];
            earth.SetActive(true);
            totalRadarCnt = 8;
        }

        int radarCntEach = totalRadarCnt / playerCount;
        for (int i = 0; i < radarCntEach; i++)
        {
            for (int j = 0; j < playerCount; j++)
            {
                GameObject radar = earth.transform.GetChild(i * playerCount + j).gameObject;
                radar.GetComponent<RadarControls>().radarID = (RadarID)j;
                playerRadarLists[j].Add(radar);
                //Debug.Log("player " + j + " radar count: " + playerRadarLists[j].Count);
            }
        }
    }

    void SetupColorPalette()
    {
        for (int i = 0; i < playerCount; i++)
        {
            int colorIndex = i % colorPalette.Count;
            Color32 pColor = colorPalette[colorIndex];
            foreach (GameObject obj in playerRadarLists[i])
            {
                obj.GetComponent<RadarControls>().SetRadarColor(pColor);
            }
        }
    }

    public void UpdateRadarStatus(RadarID id, bool detected)
    {
        if (radarDetected[(int)id] != detected)
        {
            if (detected)
            {
                detectedAmt++;
            }
            else
            {
                detectedAmt--;
            }
            radarDetected[(int)id] = detected;
        }
        CheckDetectedStatus();
        //Debug.Log(id + " detected: " + radarDetected[(int)id]);
    }

    void CheckDetectedStatus()
    {
        int threshold = Mathf.CeilToInt(playerCount / 2f);
        if (threshold <= 1)
            threshold = 2;
        if (detectedAmt >= threshold)
        {
            allDetected = true;
            GoodZoneEffect.SetActive(true);
        }
        else
        {
            allDetected = false;
            GoodZoneEffect.SetActive(false);
        }
    }

    void CheckTaskStatus()
    {
        if(accumTime >= targetToSuccess)
        {
            if (!taskComplete)
            {
                taskComplete = true;
                UIControls.S.ActivateSuccessText();
            }
        }
    }

    void ResetGame()
    {
        UIControls.S.HideSuccessText();
        UIControls.S.SetSuccessBar(0);
        accumTime = 0;
        taskComplete = false;
    }

    void ReloadGame()
    {
        Scene _scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(_scene.name);
    }
}
