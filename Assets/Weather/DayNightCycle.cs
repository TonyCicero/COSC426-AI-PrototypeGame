using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public float day = 0;
    public float hour = 0;
    public float min = 0;
    public float counter = 0;
    public float sec = 0;
    [SerializeField]
    Transform sun;

    
    public float timeScale = 1;
    const float rotSec = 0.0041666667f;
    float sunRot=-90;
    // Start is called before the first frame update
    void Start()
    {
        
    }

	public void chg_tScale(string x){
		timeScale = int.Parse(x);
	}

    // Update is called once per frame
    void Update()
    {
        //sunRot += rotSec * timeScale * Time.deltaTime;
        sun.Rotate(rotSec * timeScale * Time.deltaTime,0,0);
        counter += Time.deltaTime * timeScale;
        sec = counter % 60;
        min = (counter / 60) % 60;
        hour = (counter / 3600) % 24;
        day = counter / 86400;
    }
}
