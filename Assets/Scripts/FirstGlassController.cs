using System;
using UnityEngine;

public class FirstGlassController : MonoBehaviour
{
    public static FirstGlassController instance = null;

    
    [SerializeField] private GameObject point1;
    [SerializeField] private GameObject point2;
    [SerializeField] private ParticleSystem water;
    
    
    public float time = 1f;

    [HideInInspector] public float timer;
    [HideInInspector] public bool onetime;
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

    private void Start()
    {
        timer = time;
        onetime = true;
        checkLoosing = true;
    }

    void Update()
    {
        if (UiManager.instance.checkFirstGlass && UiManager.instance.start && time >= 0)
        {
            GetSecondPosition();
            
            Invoke("WaterFall", 0.4f);
        } 
        
        if (time <= 0)
        {
        
            water.enableEmission = false;
            RotateBucketController.instance.emitter.speed = 0;
            UiManager.instance.checkFirstGlass = false;
            RotateBucketController.instance.checkTap = 0;
            checkLoosing = false;
            RotateBucketController.instance.checkLoosing = true;
        
            Debug.Log("firstglass");
            Invoke("NextGlass", 1f);
            Invoke("GetFirstPosition", 0.7f);
        }
        
        if (RotateBucketController.instance.lv.level >= RotateBucketController.instance.liquidLevel && !checkLoosing)
        {
            // Invoke("Win", 1f);
        
            UiManager.instance.start = false;
            UiManager.instance.winCameraMov = false;
            CameraSwitchController.instance.anim.SetTrigger("SecondPos");
            RotateBucketController.instance.SetPlayerFirstPos();
            Debug.Log("win first");
        
        
        }
    }

    public void Win()
    {
        RotateBucketController.instance.ActivateWinPanel();
    }

    public void NextGlass()
    {
        UiManager.instance.checkSecondGlass = true;

        if (onetime)
        {
            CameraSwitchController.instance.anim.SetTrigger("FourthPos");
            onetime = false;
        }

        RotateBucketController.instance.oneTime = true;
    }

    public void WaterFall()
    {
        time -= Time.deltaTime; 
        RotateBucketController.instance.emitter.speed = RotateBucketController.instance.emissionSpeed;
        water.enableEmission = true;
        onetime = true;
        RotateBucketController.instance.waterDrop.Play();
    }
    
    
    public void GetFirstPosition()
    {
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, 
            point1.transform.position, Time.deltaTime * 3f);
            
        gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation,
            point1.transform.rotation , Time.deltaTime * 5f);

    }

    public void GetSecondPosition()
    {
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, 
            point2.transform.position, Time.deltaTime * 3f);
            
        gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation,
            point2.transform.rotation , Time.deltaTime * 5f);
        
    }
}
