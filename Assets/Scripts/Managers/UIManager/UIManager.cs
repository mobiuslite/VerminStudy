using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum UIType
{
    Dialogue,
    FastTravel,
    Inventory,
    Equipment,
    Battle,
    Pause,
    Options
}
public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public static bool GameIsPaused = false;

    List<UIChild> children;
    UIChild lastActiveUI;


    // Start is called before the first frame update
    void Awake()
    {
        children = new List<UIChild>();

        //Enables all children so we can get their UIChild component
        foreach(Transform gameobject in transform)
        {
            gameobject.gameObject.SetActive(true);
        }

        children.AddRange(GetComponentsInChildren<UIChild>());

        lastActiveUI = null;

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        HideAllUI();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        HideUI();
        Time.timeScale = 1f;
        GameIsPaused = false;

        if (BattleMediator.Instance.IsInBattle())
        {
            ShowUI(UIType.Battle);
        }
    }

    public void Pause()
    {
        ShowUI(UIType.Pause);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void OptionsMenu()
    {
        ShowUI(UIType.Options);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShowUI(UIType type)
    {
        HideUI();

        foreach(UIChild ui in children)
        {
            if(ui.GetUIType() == type)
            {
                ui.Show();
                lastActiveUI = ui;
            }
        }
    }

    public void ToggleUI(UIType type)
    {
        foreach (UIChild ui in children)
        {
            if (ui.GetUIType() == type)
            {
                if (ui.UIActive)
                {
                    ui.Hide();
                }
                else
                {
                    ui.Show();
                }       
                //lastActiveUI = ui;
            }
        }
    }

    public bool? CheckUIActive(UIType type)
    {
        foreach (UIChild ui in children)
        {
            if (ui.GetUIType() == type)
            {
                return ui.UIActive;
            }
        }

        return null;
    }

    public void HideUI()
    {
        if(lastActiveUI != null)
            lastActiveUI.Hide();
    }

    public void HideAllUI()
    {
        foreach (UIChild ui in children)
        {
            ui.Hide();
        }
    }
}
