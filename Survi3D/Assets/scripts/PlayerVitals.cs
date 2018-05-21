using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.SceneManagement;

public class PlayerVitals : MonoBehaviour 
{
    #region Health Var
    [Space(10)]
    public Slider hpSlider;
	public int hpMax;
	public int hpFallRate;
    #endregion

    #region Thirst Var
    [Space(10)]
    public Slider thirstSlider;
	public int thirstMax;
	public int thirstFallRate;
    #endregion

    #region Hunger Var
    [Space(10)]
    public Slider hungerSlider;
	public int hungerMax;
	public int hungerFallRate;
    #endregion

    #region Stamina Var
    [Space(10)]
    public Slider staminaSlider;
    public int stamMaxNorm;
    public float stamMaxFat;
    private int stamFallRate;
    public int stamFallMult;
    private int stamRegainRate;
    public int stamRegainMult;
    #endregion

    #region Fatigue
    [Space(10)]
    public Slider fatigueSlider;
    public int fatigueMax;
    public int fatigueFallRate;

    public bool fatStage1 = true;
    public bool fatStage2 = true;
    public bool fatStage3 = true;
    #endregion

    #region Temperature Var
    [Header("TemperatureSettings")]
    public float freezingTemp;
    public float currTemp;
    public float NormTemp;
    public float heatTemp;
    public Text tempNumber;
    public Image tempBG;
    #endregion

    #region References
    private CharacterController charController;
    private FirstPersonController playerController;

    [SerializeField] private DisableManager disableManager;
    [SerializeField] private GameObject deathUI;
    #endregion



    void Start()
	{

        fatigueSlider.maxValue = fatigueMax;
        fatigueSlider.value = fatigueMax;

		hpSlider.maxValue = hpMax;
		hpSlider.value = hpMax;

		thirstSlider.maxValue = thirstMax;
		thirstSlider.value = thirstMax;

		hungerSlider.maxValue = hungerMax;
		hungerSlider.value = hungerMax;

        staminaSlider.maxValue = stamMaxNorm;
        staminaSlider.value = stamMaxNorm;

        stamFallRate = 1;
        stamRegainRate = 1;

        charController = GetComponent<CharacterController>();
        playerController = GetComponent<FirstPersonController>();
        disableManager = GameObject.FindGameObjectWithTag("DisableController").GetComponent<DisableManager>();

	}

    void UpdateTemp()
    {
        tempNumber.text = currTemp.ToString("00.0");
    }

	void Update()
	{
        #region Fatigue
        if (fatigueSlider.value <=60 && fatStage1)
        {
            stamMaxFat = 80;
            staminaSlider.value = stamMaxFat;
            fatStage1 = false;
        }
        else if (fatigueSlider.value <= 40 && fatStage2)
        {
            stamMaxFat = 60;
            staminaSlider.value = stamMaxFat;
            fatStage2 = false;
        }
        else if (fatigueSlider.value <= 20 && fatStage3)
        {
            stamMaxFat = 20;
            staminaSlider.value = stamMaxFat;
            fatStage3 = false;
        }

        if (fatigueSlider.value >=0)
        {
            fatigueSlider.value -= Time.deltaTime * fatigueFallRate;
        }
        else if (fatigueSlider.value <=0)
        {
            fatigueSlider.value = 0;
        }
        else if (fatigueSlider.value >= fatigueMax)
        {
            fatigueSlider.value = fatigueMax;
        }

        #endregion

        #region Temperature

        if (currTemp <= freezingTemp)
        {
            tempBG.color = Color.blue;
            UpdateTemp();
        }
        else if (currTemp >= heatTemp -0.1)
        {
            tempBG.color = Color.red;
            UpdateTemp();
        }
        else
        {
            tempBG.color = Color.green;
            UpdateTemp();
        }
        #endregion

        #region HP Controll
        if (hungerSlider.value <= 0 && (thirstSlider.value <= 0)) 
		{
			hpSlider.value -= Time.deltaTime * hpFallRate * 2;
		} 
		else if (hungerSlider.value <= 0 || thirstSlider.value <= 0 || currTemp <= freezingTemp || currTemp >= heatTemp) 
		{
			hpSlider.value -= Time.deltaTime * hpFallRate;
		}

		if (hpSlider.value <= 0) 
		{
			CharacterDeath ();
		}
        #endregion

        #region Hunger Controll
        if (hungerSlider.value >= 0) 
		{
			hungerSlider.value -= Time.deltaTime * hungerFallRate;
           
		} 

		else if (hungerSlider.value <= 0) 
		{
			hungerSlider.value = 0;
		} 

		else if (hungerSlider.value >= hungerMax) 
		{
			hungerSlider.value = hungerMax;
		}
        #endregion

        #region Thirst Controll
        if (thirstSlider.value >= 0) 
		{
			thirstSlider.value -= Time.deltaTime * thirstFallRate;
		} 

		else if (thirstSlider.value <= 0) 
		{
			thirstSlider.value = 0;
		} 

		else if (thirstSlider.value >= thirstMax) 
		{
			thirstSlider.value = thirstMax;
		}
        #endregion

        #region Stamina Controll
        if (charController.velocity.magnitude > 0 && Input.GetKey(KeyCode.LeftShift))
        {
            staminaSlider.value -= Time.deltaTime / stamFallRate * stamFallMult;

            if (staminaSlider.value>0)
            {
                currTemp += Time.deltaTime / 5;
            }
        }
        else
        {
            staminaSlider.value += Time.deltaTime / stamRegainRate * stamRegainMult;

            if (currTemp >= NormTemp)
            {
                currTemp -= Time.deltaTime / 10;
            }
        }

        if (staminaSlider.value >=stamMaxFat)
        {
            staminaSlider.value = stamMaxFat;
        }
        else if (staminaSlider.value<=0)
        {
            staminaSlider.value = 0;
            playerController.m_RunSpeed = playerController.m_WalkSpeed;
        }
        else if(staminaSlider.value >=0)
        {
            playerController.m_RunSpeed = playerController.m_RunSpeedNorm;
        }
        #endregion

    }
   public void CharacterDeath()
	{
        deathUI.SetActive(true);
        disableManager.DisablePlayer();
   
	}

    public void restartBtn()
    {
        deathUI.SetActive(false);

        //#region reset sliders
        //fatigueSlider.maxValue = fatigueMax;
        //fatigueSlider.value = fatigueMax;

        //hpSlider.maxValue = hpMax;
        //hpSlider.value = hpMax;

        //thirstSlider.maxValue = thirstMax;
        //thirstSlider.value = thirstMax;

        //hungerSlider.maxValue = hungerMax;
        //hungerSlider.value = hungerMax;

        //staminaSlider.maxValue = stamMaxNorm;
        //staminaSlider.value = stamMaxNorm;

        //stamFallRate = 1;
        //stamRegainRate = 1;
        //#endregion

        disableManager.EnablePlayer();
        
    }
    
}
