using LiquidVolumeFX;
using Obi;
using UnityEngine;
using WaterRippleForScreens;


public class RotateBucketController : MonoBehaviour
{
    public static RotateBucketController instance = null;


    [SerializeField] private Vector3 rot;
    [SerializeField] private Vector3 backRot;
    // [SerializeField] private ParticleSystem waterFall;
    [SerializeField] private int numOfTotalPress;
    [SerializeField] private AudioSource waterStream;
    [SerializeField] private GameObject unLookedImage;
    [SerializeField] private GameObject handGlassPoint1;
    [SerializeField] private GameObject handGlassPoint2;
    [SerializeField] private GameObject hand;
    [SerializeField] private Animator anim;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerPoint1;
    [SerializeField] private GameObject playerPoint2;

    [SerializeField] private GameObject opponent1;
    [SerializeField] private GameObject opponent2;
    [SerializeField] private GameObject opponentAnim1;
    [SerializeField] private GameObject opponentAnim2;
    [SerializeField] private Animator firstOpponentAnim;
    [SerializeField] private Animator secondOpponentAnim;


    public LiquidVolume lv;
    public float liquidLevel;
    public float emissionSpeed = 10;
    public ObiEmitter emitter;
    public ParticleSystem water;




    private bool isPressed;
    private int numOfPress = 0;
    private int percentage = 0;
    private float saveFillAmountData; 
    private float checkTime = 1f;
    public AudioSource waterDrop;


    
    [HideInInspector] public int checkTap;
    [HideInInspector] public bool oneTime;
    [HideInInspector] public bool checkLoosing;





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
        isPressed = true;
        oneTime = true;
        checkTap = 0;
        checkLoosing = true;
        UiManager.instance.numOfTotalPress.text = numOfTotalPress.ToString();
        UiManager.instance.fillBar.fillAmount = 0;
        saveFillAmountData = PlayerPrefs.GetFloat("SaveData", saveFillAmountData);
        
        if (saveFillAmountData >= 0.5f && saveFillAmountData <= 0.52f)
        {
            UiManager.instance.fillAmountOfProgressBar.fillAmount = 0.51f;
            UiManager.instance.fillAmountOfBeer.fillAmount = 0.51f;
        }

        if (saveFillAmountData > 0.9f)
        {
            saveFillAmountData = 0f;
            PlayerPrefs.SetFloat("SaveData", saveFillAmountData);

        }
    }

    void Update()
    {
        BucketControll();
        CheckNumOfPress();
    }


    public void BucketControll()
    {
        if (UiManager.instance.start && UiManager.instance.oneTimePress)
        {

            FirstGlassController.instance.checkLoosing = true;
            SecondGlassController.instance.checkLoosing = true;
            
            if (UiManager.instance.getMainFirstPos)
            {
                GetFirstGlassPos();
                Invoke("SetMainPos", 1f);
            }

            if (Input.GetMouseButton(0))
            {           
                water.enableEmission = true;
                water.Play();
                waterStream.Play();
                emitter.speed = emissionSpeed;
                Debug.Log("Play");


                if (transform.eulerAngles.z < 67.8f)
                {
                    transform.Rotate(rot * Time.deltaTime * 5);
                    isPressed = false;
                }
            }
            else
            {
                emitter.speed = 0;
            }
        
            
            if (Input.GetMouseButtonDown(0))
            {
                numOfPress++;
                UiManager.instance.numOfPress.text = numOfPress.ToString();
                waterDrop.Play();
                checkTap = 1;
            }


            if (Input.GetMouseButtonUp(0))
            {
                water.enableEmission = false;
                isPressed = true;
                waterStream.Stop();

                
                Debug.Log("Stop");
                if (checkTap == 1)
                {
                    UiManager.instance.oneTimePress = false;
                }
            }

            if (transform.eulerAngles.z > 65f && isPressed)
            {
                transform.Rotate(backRot * Time.deltaTime * 5);
            }

            if (lv.level >= liquidLevel)
            {
                UiManager.instance.start = false;
                checkLoosing = false;

            }
        }

        if (!UiManager.instance.oneTimePress && checkTap == 1)
        {
            Invoke("GetSecondGlassPos", 0.3f);
            Invoke("NextGlass", 1.5f);
        }

        if (UiManager.instance.winCameraMov)
        {
            SetPlayerSecondPos();
        }
        
    }


    public void SetMainPos()
    {
        UiManager.instance.getMainFirstPos = false;
        SecondGlassController.instance.time = SecondGlassController.instance.timer;

    }
    

    public void CheckNumOfPress()
    {
        if (lv.level >= liquidLevel && !checkLoosing)
        {
            UiManager.instance.start = false;
            UiManager.instance.winCameraMov = false;
            CameraSwitchController.instance.anim.SetTrigger("SecondPos");
            anim.SetBool("Fail", true);
            opponent1.SetActive(false);
            opponent2.SetActive(false);
            opponentAnim1.SetActive(true);
            opponentAnim2.SetActive(true);
            firstOpponentAnim.SetBool("Win", true);
            secondOpponentAnim.SetBool("Win", true);
            SetPlayerFirstPos();
        
            Invoke("ActivateLosePanel", 1f);
        }

        if (lv.level > liquidLevel && !FirstGlassController.instance.checkLoosing)
        {
            Invoke("LooseFirstOpponent", 0.5f);
            Invoke("ActivateWinPanel", 1f);

        }
        
        if (lv.level > liquidLevel && !SecondGlassController.instance.checkLoosing)
        {
            Invoke("LooseSecondOpponent", 0.5f);
            Invoke("ActivateWinPanel", 1f);
        }
    }



    
    public void GetFirstGlassPos()
    {
        hand.SetActive(true);
        // UiManager.instance.checkFirstGlass = false;
        UiManager.instance.checkSecondGlass = false;
                    
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, 
            handGlassPoint2.transform.position, Time.deltaTime * 3f);
            
        gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation,
            handGlassPoint2.transform.rotation , Time.deltaTime * 5f);
    }

    public void GetSecondGlassPos()
    {
        hand.SetActive(false);
        if (oneTime)
        {
            CameraSwitchController.instance.anim.SetTrigger("ThirdPos");
            SecondGlassController.instance.checkLoosing = true;
            oneTime = false;
        }
                    
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, 
            handGlassPoint1.transform.position, Time.deltaTime * 3f);
            
        gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation,
            handGlassPoint1.transform.rotation , Time.deltaTime * 5f);
    }

    public void NextGlass()
    { 
        UiManager.instance.checkFirstGlass = true;
        // FirstGlassController.instance.onetime = true;
        SecondGlassController.instance.oneTime = true;
    }
    
    
    
    public void ActivateWinPanel()
    {
        UiManager.instance.winPanel.SetActive(true);
        // waterFall.Play();
        // UIManager.instance.start = false;

        if (UiManager.instance.fillAmountOfProgressBar.fillAmount >= 0f && UiManager.instance.fillAmountOfProgressBar.fillAmount < 0.5f)
        {
            UiManager.instance.fillAmountOfBeer.fillAmount -= 0.01f;
            UiManager.instance.fillAmountOfProgressBar.fillAmount += 0.01f;
            percentage = (int) (UiManager.instance.fillAmountOfProgressBar.fillAmount * 100);
            UiManager.instance.percentage.text = (percentage).ToString();

            saveFillAmountData = UiManager.instance.fillAmountOfProgressBar.fillAmount;
            PlayerPrefs.SetFloat("SaveData", saveFillAmountData);

            if (UiManager.instance.fillAmountOfProgressBar.fillAmount >= 0.49f && UiManager.instance.fillAmountOfProgressBar.fillAmount < 0.5f)
            {
                UiManager.instance.nextButton.SetActive(true);
            }
        }
        
        if (UiManager.instance.fillAmountOfProgressBar.fillAmount >= 0.51f)
        {
            UiManager.instance.fillAmountOfBeer.fillAmount -= 0.01f;
            UiManager.instance.fillAmountOfProgressBar.fillAmount += 0.01f;
            percentage = (int) (UiManager.instance.fillAmountOfProgressBar.fillAmount * 100);
            UiManager.instance.percentage.text = (percentage).ToString();

            if (UiManager.instance.fillAmountOfProgressBar.fillAmount > 0.95f)
            {
                unLookedImage.SetActive(true);
                UiManager.instance.nextButton.SetActive(true);

            }
            
            saveFillAmountData = UiManager.instance.fillAmountOfProgressBar.fillAmount;
            PlayerPrefs.SetFloat("SaveData", saveFillAmountData);

        }


    }


    public void ActivateLosePanel()
    {
        UiManager.instance.losePanel.SetActive(true);
        UiManager.instance.start = false;
    }



    public void SetPlayerFirstPos()
    {
        player.transform.position = Vector3.MoveTowards(player.transform.position, 
            playerPoint1.transform.position, Time.deltaTime * 3f);
            
        player.transform.rotation = Quaternion.Slerp(player.transform.rotation,
            playerPoint1.transform.rotation , Time.deltaTime * 5f);
    }
    
    public void SetPlayerSecondPos()
    {
        player.transform.position = Vector3.MoveTowards(player.transform.position, 
            playerPoint2.transform.position, Time.deltaTime * 3f);
            
        player.transform.rotation = Quaternion.Slerp(player.transform.rotation,
            playerPoint2.transform.rotation , Time.deltaTime * 5f);
    }

    public void LooseFirstOpponent()
    {
        anim.SetBool("Win", true);
        opponent1.SetActive(false);
        opponent2.SetActive(false);
        opponentAnim1.SetActive(true);
        opponentAnim2.SetActive(true);
        firstOpponentAnim.SetBool("Fail", true);
        secondOpponentAnim.SetBool("Win", true);
    }
    
    public void LooseSecondOpponent()
    {
        anim.SetBool("Win", true);
        opponent1.SetActive(false);
        opponent2.SetActive(false);
        opponentAnim1.SetActive(true);
        opponentAnim2.SetActive(true);
        firstOpponentAnim.SetBool("Win", true);
        secondOpponentAnim.SetBool("Fail", true);
    }

}

