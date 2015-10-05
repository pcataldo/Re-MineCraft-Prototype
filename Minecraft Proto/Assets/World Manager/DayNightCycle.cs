using UnityEngine;
using System.Collections;

public class DayNightCycle : MonoBehaviour 
{
	public GameObject sun;
	public float secondsInHour;
	
	public bool isNight = false;
	bool isTurning = false;

	void Awake ()
	{
		if(sun == null)
			sun = GameObject.Find("Sun)");
	}

	void Start () 
	{
	
	}
	

	void Update () 
	{
		if(!isTurning)
		{
			StartCoroutine(RotateSun());
		}

	}

	public IEnumerator RotateSun()
	{
		isTurning = true;

		//rotate Sun 15 degrees along the x
		sun.transform.Rotate(Vector3.right, 15);

		//if angle 180 or greater??? isNightTrue, turn off Light 
		if(sun.transform.rotation.eulerAngles.x > 180)
			isNight = true;
		else
			isNight = false;

		yield return new WaitForSeconds(secondsInHour);

		isTurning = false;
	}
}
