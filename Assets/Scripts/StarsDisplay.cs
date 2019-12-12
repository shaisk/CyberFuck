using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class StarsDisplay : MonoBehaviour
{
    private Image[] stars;
    private Button[] bttns;
    private Menu ms;
    public int levelChanger = 0;
    private int unlockLevel;
    
    void Awake()
    {
        stars = GetComponentsInChildren<Image>();
        ms = Camera.main.GetComponent<Menu>();
        bttns = ms.levelChanger.GetComponentsInChildren<Button>();
    }

    void Start()
    {
        switch (data.lvls[levelChanger])
        {
            case 1:
                stars[1].color = new Color(stars[1].color.r, stars[1].color.g, stars[1].color.b, 255);
                break;
            case 2:
                unlockLevel = levelChanger + 1;
                bttns[unlockLevel].interactable = true;
                for (int i = 1; i < stars.Length - 1; i++) stars[i].color = new Color(stars[i].color.r, stars[i].color.g, stars[i].color.b, 255);
                break;
            case 3:
                unlockLevel = levelChanger + 1;
                bttns[unlockLevel].interactable = true;
                for (int i = 1; i < stars.Length; i++) stars[i].color = new Color(stars[i].color.r, stars[i].color.g, stars[i].color.b, 255);
                break;
        }
    }
}
