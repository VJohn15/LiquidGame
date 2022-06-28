using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager instance = null;

    public Text numOfPress;
    public Text numOfTotalPress;
    public Text percentage;
    public GameObject winPanel;
    public GameObject losePanel;
    public Image fillBar;
    public Image fillAmountOfProgressBar;
    public Image fillAmountOfBeer;
    public float liquidCount = 0.3f;
    public GameObject nextButton;

    [SerializeField] private GameObject tapToStartPanel;


    [HideInInspector] public bool start;
    [HideInInspector] public bool winCameraMov;
    [HideInInspector] public bool loseCameraMov;
    [HideInInspector] public bool oneTimePress;
    [HideInInspector] public bool checkFirstGlass;
    [HideInInspector] public bool checkSecondGlass;
    [HideInInspector] public bool getMainFirstPos;

    
    
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance == this)
        {
            Destroy(gameObject);
        }

    }
    
    
    void Start()
    {
        start = false;
        winCameraMov = false;
        loseCameraMov = false;
        oneTimePress = false;
        checkFirstGlass = false;
        checkSecondGlass = false;
        getMainFirstPos = false;

    }
    

    public void TapToStart()
    {
        Invoke("StartControll", 1.5f);
        tapToStartPanel.SetActive(false);
        getMainFirstPos = true;
        winCameraMov = true;
        CameraSwitchController.instance.anim.SetTrigger("FirstPos");

    }

    public void NextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex == 10)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void StartControll()
    {
        start = true;
        oneTimePress = true;
    }
    

}
