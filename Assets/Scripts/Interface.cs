using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Interface : MonoBehaviour
{

    public CarPhysics cs;
    public Text coinsCount;
    private SmoothCamera sc;
    public int coinsTarget3 = 5; 
    public int coinsTarget2 = 3; 
    public int coinsTarget1 = 1; 
    public Image[] Stars;
    public GameObject[] cars;
    private bool isPaused = false;
    public GameObject PausePanel;
    private bool coinsAdded = false;

    void Start()
    {
        sc = GetComponent<SmoothCamera>();
        cs = cars[data.car].GetComponent<CarPhysics>();
        cars[data.car].SetActive(true);
        sc.target = cars[data.car].transform;
        sc.sc = cars[data.car].GetComponent<WheelJoint2D>();
    }

   
    void Update()
    {
        if (cs.fp.activeSelf)
        {
            for (int b = 0; b < cs.ControlCar.Length; b++)
            {
                cs.ControlCar[b].clicked = false;
                cs.ControlCar[b].gameObject.SetActive(false);
            }
     
            coinsCount.text = "Coins: " + cs.coinsInt.ToString();

            if (Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadScene(0);
            }
            if (cs.coinsInt == coinsTarget3)
            {
                for(int i = 0; i < Stars.Length; i++)
                {
                    Stars[i].color = new Color(Stars[i].color.r, Stars[i].color.g, Stars[i].color.b, 255);
                }
                data.lvls[SceneManager.GetActiveScene().buildIndex - 1] = 3;
            }
            
            else if (cs.coinsInt >= coinsTarget2 && cs.coinsInt != coinsTarget3)
            {
                for (int i = 0; i < 2; i++)
                {
                    Stars[i].color = new Color(Stars[i].color.r, Stars[i].color.g, Stars[i].color.b, 255);
                }

                if (data.lvls[SceneManager.GetActiveScene().buildIndex - 1] != 3)
                {
                    data.lvls[SceneManager.GetActiveScene().buildIndex - 1] = 2;
                }
            }

            else if (cs.coinsInt >= coinsTarget1 && cs.coinsInt != coinsTarget2)
            {
                Stars[0].color = new Color(Stars[0].color.r, Stars[0].color.g, Stars[0].color.b, 255);
                
                if (data.lvls[SceneManager.GetActiveScene().buildIndex - 1] != 3 && data.lvls[SceneManager.GetActiveScene().buildIndex - 1] != 2)
                {
                    data.lvls[SceneManager.GetActiveScene().buildIndex - 1] = 1;
                }
            }

            if (!coinsAdded)
            {
                data.coins += cs.coinsInt;
                coinsAdded = true;
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape) && !isPaused && !cs.fp.activeSelf)
        {
            PauseOn();
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && isPaused)
        {
            Continue();
        }
    }

    public void PauseOn()
    {
        if (!cs.fp.activeSelf)
        {
            PausePanel.SetActive(true);
            Time.timeScale = 0;
            isPaused = true;
        }
    }

    public void Continue()
    {
        PausePanel.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
    }

    public void Restart()
    {
        Time.timeScale = 1;
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void goToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }
}
