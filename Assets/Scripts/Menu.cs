using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject levelChanger;
    public GameObject ExitPanel;
    public Image[] cars;
    public GameObject garagePanel;
    public Button[] bttns;
    public Text[] carsText;
    public Text coinText;
    public string[] levels;
    private char del = 'f';
    public Button[] updateBttns;
    SaveToJson STJ = new SaveToJson();
    private int carForT;

    void Awake()
    {
        if (!PlayerPrefs.HasKey("sv"))
        {
            for (int i = 0; i < STJ.t.Length; i++) STJ.t[i] = "0f0f0";
            string json = JsonUtility.ToJson(STJ);
            PlayerPrefs.SetString("sv", json);
        }
        STJ = JsonUtility.FromJson<SaveToJson>(PlayerPrefs.GetString("sv"));
        if (data.gameStarted) data.dat(STJ.coins, STJ.car, STJ.t, STJ.lvls);
    }

    void Start()
    {
        cars[data.car].color = Color.white;
        for (int i = 0; i < data.lvls.Length - 1; i++)
        {
            if (data.lvls[i] == 3)
            {
                bttns[i].interactable = true;
                carsText[i].gameObject.SetActive(false);
            }
        }
    }

    void Update()
    {
        if (garagePanel.activeSelf == true && Input.GetKeyDown(KeyCode.Escape))
        {
            garagePanel.SetActive(false);
        }
        else if (levelChanger.activeSelf == true && Input.GetKeyDown(KeyCode.Escape))
        {
            levelChanger.SetActive(false);
        }

        else if (ExitPanel.activeSelf == false && Input.GetKeyDown(KeyCode.Escape))
        {
            ExitPanel.SetActive(true);
        }

        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitPanel.SetActive(false);
        }

        coinText.text = data.coins.ToString();
    }

    private void Save()
    {
        STJ.dat(data.coins, data.car, data.t, data.lvls);
        string json = JsonUtility.ToJson(STJ);
        PlayerPrefs.SetString("sv", json);
    }

    public void OnClickedStart()
    {
        levelChanger.SetActive(true);
    }

    public void OnClickedExit()
    {
        Application.Quit();
    }

    public void LevelButtons(int level)
    {
        data.gameStarted = false;
        SceneManager.LoadScene(level);
    }

    public void carChanger(int car)
    {
        data.car = car;
        for (int c = 0; c < cars.Length; c++)
        {
            cars[c].color = Color.black;
            cars[car].color = Color.white;
        }
        //switch (car)
        //{            
        //    case 0:
        //        cars[car].color = Color.white;
        //        cars[car+1].color = Color.black;
        //    break;

        //    case 1:
        //        cars[car].color = Color.white;
        //        cars[car - 1].color = Color.black;
        //    break;
        //}
    }

    public void carTuning(int carPart)
    {
        string[] carforT = data.t[carForT].Split(del);
        int updateState = int.Parse(carforT[carPart]) + 1;
        if (updateState <= 6 && 5 * updateState <= data.coins)
        {
            carforT[carPart] = updateState.ToString();
            data.t[carForT] = carforT[0] + "f" + carforT[1] + "f" + carforT[2];           
            data.coins -= 5 * updateState;            
        }

        if (int.Parse(carforT[carPart]) == 6 || STJ.coins < 5 * updateState)
        {
            updateBttns[carPart].interactable = false;
        }
        Save();
        print(data.t[carForT]);
    }

    public void carForTun(int car)
    {
        carForT = car;
    }

    void OnApplicationQuit()
    {
        Save();
    }
}

public class SaveToJson
{
    public int coins = 0, car = 0;
    public string[] t = new string[2];
    public int[] lvls = new int[2];

    public void dat(int coins, int car, string[] t, int[] lvls)
    {
        this.coins = coins;
        this.car = car;
        this.t = t;
        this.lvls = lvls;
    }
}

public static class data
{
    public static int coins = 0, car = 0;
    public static string[] t = new string[3];
    public static int[] lvls = new int[2];
    public static bool gameStarted = true;

    public static void dat(int coins, int car, string[] t, int[] lvls)
    {
        data.coins = coins;
        data.car = car;
        data.t = t;
        data.lvls = lvls;
    }
}

