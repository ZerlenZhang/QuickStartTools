using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.localScale = Random.insideUnitSphere * 10.0f;	
	}
}
