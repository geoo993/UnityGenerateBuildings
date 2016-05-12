using UnityEngine;
using System.Collections;

public class ColliderTests : MonoBehaviour {


	void OnTriggerEnter (Collider other)
	{
		Debug.Log (" You Just Hit The Building. ");


	}
	void OnTriggerExit (Collider other)
	{
		Debug.Log (" You Stopped Hitting The Building. ");
	}

//	void OnCollisionEnter (Collision col)
//	{
//		Debug.Log (" You Just Hit The Building. ");
//
//	}
}
