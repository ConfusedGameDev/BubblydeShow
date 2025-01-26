using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleMenuController : MonoBehaviour
{
    private int cursorIndex = 0;

    private int page = 0;

    private int currentLocaleIndex;

    public GameObject[] cursorTitle;
    public GameObject[] cursorOption;

    public GameObject OptionPanel;

    public TMP_Text currentLanguageText;

    public Slider bgmSlider;
    public Slider sfxSlider;

    public int targetSceneID = 1;

    void Start()
    {
        currentLocaleIndex = LocalizationSettings.AvailableLocales.Locales.IndexOf(LocalizationSettings.SelectedLocale);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            cursorIndex++;
            if (page == 0)
            {
                if (cursorIndex >= cursorTitle.Length)
                {
                    cursorIndex = 0;
                }
            }
            else
            {
                if (cursorIndex >= cursorOption.Length)
                {
                    cursorIndex = 0;
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (page == 0)
            {
                if (cursorIndex == 0) LoadScene(targetSceneID);
                if (cursorIndex == 1)
                {
                    page = 1;
                    cursorIndex = 0;
                    OpenOptionPanel();
                }
                if (cursorIndex == 2) QuitGame();
            }
            else
            {
                if (cursorIndex == 0) SelectNextLanguage();
                if (cursorIndex == 1) AddBGMVolume();
                if (cursorIndex == 2) AddSFXVolume();
                if (cursorIndex == 3)
                {
                    page = 0;
                    cursorIndex = 1;
                    CloseOptionPanel();
                }

            }
        }

        UpdateCursor();
    }

    void UpdateCursor()
    {
        if (page == 0)
        {
            for (int i = 0; i < cursorTitle.Length; i++)
            {
                cursorTitle[i].SetActive(cursorIndex == i);
            }
        }
        else
        {
            for (int i = 0; i < cursorOption.Length; i++)
            {
                cursorOption[i].SetActive(cursorIndex == i);
            }
        }
    }



    void OpenOptionPanel()
    {
        OptionPanel.SetActive(true);
    }
    void CloseOptionPanel()
    {
        OptionPanel.SetActive(false);
    }


    void LoadScene(int StageID)
    {
        SceneManager.LoadScene(StageID);
    }

    void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif 
    }

    void SelectNextLanguage()
    {
        currentLocaleIndex = (currentLocaleIndex + 1) % LocalizationSettings.AvailableLocales.Locales.Count;
        ApplyLanguage();
    }

    void ApplyLanguage()
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[currentLocaleIndex];
    }


    void AddBGMVolume()
    {
        if (bgmSlider.value < 0.99f)
        {
            bgmSlider.value += 0.25f;
        }
        else
        {
            bgmSlider.value = 0.0f;
        }
    }

    void AddSFXVolume()
    {
        if (sfxSlider.value < 0.99f)
        {
            sfxSlider.value += 0.25f;
        }
        else
        {
            sfxSlider.value = 0.0f;
        }
    }

}
