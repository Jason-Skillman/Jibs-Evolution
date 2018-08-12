using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	//Singleton
	public static CameraFollow main;

    public Transform target;
    [Range(0.01f, 1.0f)]
    public float speed = 0.1f;

    
    public void Awake() {
        //DontDestroyOnLoad(gameObject);
		//
		////Singleton
		//if(!main)
		//	main = this;
		//else
		//	Destroy(gameObject);
	}

    void Update() {

		//this.transform.position = Vector3.Lerp(this.transform.position, target.position, speed) + new Vector3(0, 0, -10);

		
		if(target.position.y >= 0) {
			//Smooth camera movement
			this.transform.position = Vector3.Lerp(this.transform.position, target.position, speed) + new Vector3(0, 0, -10);
        }
        else
        {
            this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(target.position.x,0,target.position.z), speed) + new Vector3(0, 0, -10);
        }
		
    }
}
