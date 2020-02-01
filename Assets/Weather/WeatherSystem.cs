using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherSystem : MonoBehaviour{

    [SerializeField] DayNightCycle WorldTime;

    public float temp = 30; // degrees in celsius
    public Weather weatherType;
    private int switchWeather;   //for the pickweather switch statement
    public float timer = 0f; //timer for the weather
    public float reset = 20f; //value to reset the weather timer - low value for testing
    public float resetMax = 100; // max time between weather change -- change could result in same weather
    public float resetMin = 20; // min time between weather change
	//Water Settings
	public GameObject Water;
	[SerializeField]
	float WaterLevel = 1;
	[SerializeField]
	float WaveHeight = .5f;
	[SerializeField]
	bool Wup = true;
	[SerializeField]
	float WStart;
	[SerializeField]
	float WaveSpeed = 1;


    //particles for the weathers 

    [SerializeField] ParticleSystem  sunCloudParticles;
    [SerializeField] ParticleSystem  thunderStormParticles;
    [SerializeField] ParticleSystem  overcastParticles;

    [SerializeField] ParticleSystem sunCloudE;  //for emission module
    [SerializeField] ParticleSystem thunderStormE;
    [SerializeField] ParticleSystem overcastE;
    //variables 

    //sound(not implemented yet)
	[SerializeField] AudioSource WSFX;
    [SerializeField] float audioFadeTime = 0.25f;
    [SerializeField] AudioClip sunnyAud;
    [SerializeField] AudioClip thunderAud;
    [SerializeField] AudioClip overcastAud;

    //light
    public float lightDimTime = 0.3f;  //for the light switching between weather states 
    public float minIntensity = .35f;    //for thunderstorm light intensity
    public float maxIntensity = 1f;    //for Sunny light intensity
    public float overcastIntensity = 0.5f; //for overcast ""
    public float targetIntensity = 1.0f;
    //fog colors 
    public Color sunFog; //set sunfog color
    public Color thunderFog;
    public Color overcastFog;
    public float fogChangeSpeed = 0.005f; // speed in which the fog changes

    //Player/Weather objects
    private Transform player;           //player game object
    private Transform weather;          //weather game object
    public float weatherHeight = 15f;   // how high weather is 

    public enum Weather { //Class to hold types of weathers we can have 
        PickWeather,
        Sunny,
        Thunderstorm,
        Overcast


    }
    // Start is called before the first frame update
    void Start()
    {
		WStart = WaterLevel;
        GameObject playerGameObject = GameObject.FindGameObjectWithTag("Player"); //finds player game object
        player = playerGameObject.transform; //players position

        GameObject weatherGameObject = GameObject.FindGameObjectWithTag("Weather"); //finds weather game object
        weather = weatherGameObject.transform;

        //RenderSettings.fog = true;  //enable the fog in render settings
        RenderSettings.fogMode = FogMode.ExponentialSquared; //set fog mode to exponential squared*
        RenderSettings.fogDensity = 0.005f; //set fog density

        sunCloudE.Stop(); // sets suncloudE to suncloud particle system 
        thunderStormE.Stop();
        overcastE.Stop();
        WeatherFSM();//starts final state machine - getting error here 
    }

    // Update is called once per frame
    void Update()
    {
     

        timer -= Time.deltaTime * WorldTime.timeScale;  //counts down the weather  


        if (timer <= 0)
        {
            weatherType = WeatherSystem.Weather.PickWeather;  //picks new weather when timer reaches 0
            WeatherFSM();
            reset = Random.Range(resetMin, resetMax);
            timer = reset;           //makes it equal to time to reset
        }

        if (GetComponent<Light>().intensity > targetIntensity)
        {  //get light intensity and if its greater than overcast
            GetComponent<Light>().intensity -= Time.deltaTime + lightDimTime; //Then reduce intensity by light dim time
        }
        if (GetComponent<Light>().intensity < targetIntensity)
        {
            GetComponent<Light>().intensity += Time.deltaTime + lightDimTime;
        }

        weather.transform.position = new Vector3(player.position.x, //keeps weather on player
                                                 player.position.y + weatherHeight, 
                                                 player.position.z);
		Water.transform.position = new Vector3(250,WaterLevel,250);

		if ( WorldTime.timeScale < 100){
			if (Wup){
				WaterLevel += WaveSpeed*(Time.deltaTime * WorldTime.timeScale);
				if (WaterLevel >= WaveHeight + WStart){
					Wup = false;
					WStart = WaterLevel;

				}
			}else{
				WaterLevel -= WaveSpeed*(Time.deltaTime * WorldTime.timeScale);
				if (WaterLevel <= WStart -  WaveHeight){
					Wup = true;
					WStart = WaterLevel;

				}
			}
		}
        switch (weatherType)
        {
            case Weather.Sunny:
                temp += .01f * Time.deltaTime * WorldTime.timeScale;
                break;
            case Weather.Overcast:
                temp -= .05f * Time.deltaTime * WorldTime.timeScale;
                break;
            case Weather.Thunderstorm:
                temp -= .015f * Time.deltaTime * WorldTime.timeScale;
                break;
        }
    }

 
    //finite state machine for weather switch
    void WeatherFSM() {
       // Debug.Log("in FSM");
                   // while theres a weather state machine
            switch (weatherType)      // switch statement to pick which type pf weather
            {
                case Weather.PickWeather:
                    PickWeather();
                    break;
                case Weather.Sunny:
                    Sunny();
                    break;
                case Weather.Thunderstorm:
                    Thunderstorm();
                    break;
                case Weather.Overcast:
                    Overcast();
                    break;
            }
          
    }


    void PickWeather() {
       // Debug.Log("PickWeather Function");    //For testing purposes 
       
        switchWeather = Random.Range(0, 2);  //Generates random numbers 0-2 for type of weather 

        sunCloudE.Stop();   //disables particle system at start 
        thunderStormE.Stop();
        overcastE.Stop();

        switch (switchWeather)    //switch statement will send the type of weather to WeatherFSM
        {
            case 0:
                weatherType = WeatherSystem.Weather.Sunny;
                Sunny();
                break;
            case 1:
                weatherType = WeatherSystem.Weather.Thunderstorm;
                Thunderstorm();
                break;

            case 2:
                weatherType = WeatherSystem.Weather.Overcast;
                Overcast();
                break;
          


        }
    }

    
        
    


    void Sunny() {
        sunCloudE.Play();//enable particles
		WSFX.clip = sunnyAud;//audio
		WSFX.Play();
        Debug.Log("Sunny Function");    //For testing purposes 

        targetIntensity = maxIntensity; //set target light intensity

        Color currentFogColor = RenderSettings.fogColor;  //set current fog color to render setting
        RenderSettings.fogColor = Color.Lerp(currentFogColor, sunFog, fogChangeSpeed * Time.deltaTime); //makes the switch between current fog to sun fog  
    }

    void Thunderstorm() {
        thunderStormE.Play(); //enable particles
		WSFX.clip = thunderAud; //audio
		WSFX.Play();

        Debug.Log("ThunderStorm Function"); //For testing purposes 

        targetIntensity = minIntensity; //set target light intensity

        Color currentFogColor = RenderSettings.fogColor;
        RenderSettings.fogColor = Color.Lerp(currentFogColor, thunderFog, fogChangeSpeed * Time.deltaTime);
    }

    void Overcast() {
        overcastE.Play();  //enable particles
		WSFX.clip = overcastAud;//audio
		WSFX.Play();
        Debug.Log("Overcast Function"); //For testing purposes 

        targetIntensity = overcastIntensity; //set target light intensity

        Color currentFogColor = RenderSettings.fogColor;
        RenderSettings.fogColor = Color.Lerp(currentFogColor, overcastFog, fogChangeSpeed * Time.deltaTime);
    }

}
