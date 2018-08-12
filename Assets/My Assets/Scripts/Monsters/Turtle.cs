using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : Monster {
	
	public override void Start() {
		base.Start();
	}

	public override void Update() {
		base.Update();

        rigidbody2D.velocity = Vector3.zero;
        //rigidbody2D.AddForce(Vector2.right * speed, ForceMode2D.Impulse);
        gameObject.transform.Translate(new Vector3(1, 0, 0) * Time.deltaTime * speed);

	}
	

}
