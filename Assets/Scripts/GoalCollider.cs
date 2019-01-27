using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GoalCollider : MonoBehaviour {
	// Update is called once per frame
	void OnTriggerEnter(Collider hitObj) {
		if (hitObj.gameObject.tag == "Player") {
			StartCoroutine(SceneTransition.LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
		}
	}

}
