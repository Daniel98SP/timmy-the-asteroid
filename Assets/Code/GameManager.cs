using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject canvas;
    public GameObject initialPanel;
    public bool introScene;
    public int limitFrames;
    public GameObject HUD;
    public GameObject effectBarInsideInv;
    public GameObject effectBarInv;
    public GameObject effectBarInsideJump;
    public GameObject effectBarJump;
    public GameObject godModeText;
    public AudioClip dieSound;
    public GameObject howToPlayPanel;
    public GameObject transicionWin;



    private bool paused;
    public int lifes;
    public GameObject[] hearts;
    private int currentLifes;
    private GameObject currentCanvas;
    private bool efecto = false;
    private bool efectoJ = false;

    private float step;
    private float stepTime;
    private float lastStep;
    private float finishSegundos;
    private float width;
    private float posX;
    private float segundos;
    private RectTransform rect;

    private float stepJ;
    private float stepTimeJ;
    private float lastStepJ;
    private float finishSegundosJ;
    private float widthJ;
    private float posXJ;
    private float segundosJ;
    private RectTransform rectJ;

    private bool godMode = false;


    private void Awake()
    {

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = limitFrames;
        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(canvas);


    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        currentLifes = lifes;
        rect = effectBarInsideInv.GetComponent<RectTransform>();
        width = rect.sizeDelta.x;
        posX = rect.localPosition.x;

        rectJ = effectBarInsideJump.GetComponent<RectTransform>();
        widthJ = rectJ.sizeDelta.x;
        posXJ = rectJ.localPosition.x;
        Debug.Log("WIDTH" + width);
        Debug.Log("POSX" + posX);

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape) && !introScene)
        {
            if (!paused)
            {
                Pause();
            }
            else
            {
                UnPause();

            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            introScene = false;
            initialPanel.SetActive(false);
            HUD.SetActive(true);
            UnPause();
            SceneManager.LoadScene("Earth");
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            godMode = !godMode;
            if (godMode)
                godModeText.SetActive(true);
            else
                godModeText.SetActive(false);

        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            introScene = false;
            initialPanel.SetActive(false);
            HUD.SetActive(true);
            UnPause();
            SceneManager.LoadScene("Space");
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            introScene = false;
            initialPanel.SetActive(false);
            HUD.SetActive(true);
            UnPause();
            SceneManager.LoadScene("Planet");
        }

        if (efecto && Time.time <= finishSegundos)
        {

            if (Time.time >= lastStep + stepTime)
            {

                lastStep = Time.time;
                Vector2 currentSize = rect.sizeDelta;
                rect.sizeDelta = new Vector2(currentSize.x - step, 100);
                rect.localPosition = (rect.localPosition + new Vector3(-step / 2f, 0f, 0f));




            }

        }
        else if (efecto)
        {
            ResetEfecto();

        }

        if (efectoJ && Time.time <= finishSegundosJ)
        {

            if (Time.time >= lastStepJ + stepTimeJ)
            {

                lastStepJ = Time.time;
                Vector2 currentSize = rectJ.sizeDelta;
                rectJ.sizeDelta = new Vector2(currentSize.x - stepJ, 100);
                rectJ.localPosition = (rectJ.localPosition + new Vector3(-stepJ / 2f, 0f, 0f));




            }

        }
        else if (efectoJ)
        {
            ResetEfectoJ();

        }

    }

    private void ResetEfecto()
    {
        efecto = false;
        effectBarInv.SetActive(false);
        rect.sizeDelta = new Vector2(width, 100);
        rect.localPosition = new Vector3(posX, rect.localPosition.y, 0f);
    }

    private void ResetEfectoJ()
    {
        efectoJ = false;
        effectBarJump.SetActive(false);
        rectJ.sizeDelta = new Vector2(widthJ, 100);
        rectJ.localPosition = new Vector3(posXJ, rectJ.localPosition.y, 0f);
    }


    public bool getGodMode()
    {
        return godMode;
    }

    public void UnPause()
    {
        paused = false;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public void Efecto(float segundos)
    {
        ResetEfecto();
        efecto = true;
        finishSegundos = Time.time + segundos;
        effectBarInv.SetActive(true);
        stepTime = segundos / 30f;
        step = width / 30f;
        Debug.Log(stepTime);
    }

    public void EfectoJ(float segundos)
    {
        ResetEfectoJ();
        efectoJ = true;
        finishSegundosJ = Time.time + segundos;
        effectBarJump.SetActive(true);
        stepTimeJ = segundos / 30f;
        stepJ = widthJ / 30f;
        Debug.Log(stepTime);
    }

    public void Pause()
    {
        paused = true;
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }


    public void ExitToMainMenu()
    {
        SceneManager.LoadScene(0);
        Destroy(canvas);
        Destroy(this.gameObject);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void NextScene()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        if (currentScene >= 0 && currentScene < 4)
        {
            introScene = false;
            
                initialPanel.SetActive(false);
                if(currentScene != 0)
                    HUD.SetActive(true);
                UnPause();
                SceneManager.LoadScene(currentScene + 1);



        }
        else
        {
            //GameOver();
            transicionWin.SetActive(true);
            StartCoroutine(LoadSceneIn(5, 5));
        }
    }



    IEnumerator LoadSceneIn(float n, int scene)
    {
        yield return new WaitForSeconds(n);
        SceneManager.LoadScene(scene);
    }

    public void Die()
    {
        AudioSource.PlayClipAtPoint(dieSound, Camera.main.transform.position);
        if (!godMode)
        {
            ResetEfecto();
            ResetEfectoJ();
            currentLifes--;
            hearts[currentLifes].SetActive(false);
            if (currentLifes > 0)
            {

                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

            }
            else
            {
                GameOver();
            }
        }
    }

    public string CurrentSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

    private void GameOver()
    {
        SceneManager.LoadScene(0);
        Destroy(canvas);
        Destroy(this.gameObject);

    }

    public void howToPlay()
    {
        howToPlayPanel.SetActive(true);
    }

    public void howToPlayBack()
    {
        howToPlayPanel.SetActive(false);


    }
}
