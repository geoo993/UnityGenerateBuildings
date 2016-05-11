using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MathArray : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
		windowsArray ();

	}

	void Update()
	{


	}


	private void windowsArray (){

		int length1 = 64;
		int length2 = length1 + 1;


		////random offset in array
		List<int> arrayInt = new List<int>();

		int a = 0;
		int rand = 0;
		while (a < length1-2)
		{

			rand = a + Random.Range(0,3);
			a = rand;

			print(" a:  "+a); 
			arrayInt.Add (a);

			a ++;

		}

		print ("array lenght:  " + arrayInt.Count);

//		int[] arr = new int[length1];
//
//		for (int i = 0; i < length1; i++) {
//
//			arr[i] = i;
//		}

//		for (int a = 0; a < (length1/2) ; a++) {
//			print(a * 2);   ///in twos from 0
//		}
//
//		for (int a = 1; a < (length1/2) + 1; a++) {
//			print((a * 2) - 1);    ///in twos from 1
//		}

//		for (int a = 0; a < (length2/3) + 1; a++) {
//			print(a * 3);	//in threes from 0
//		}


//		for (int a = 1; a < (length2/3) + 1; a++) {
//			//print(a * 3); ///in threes eliminating -1 at start
//			//print((a * 3) - 1); ///in threes from 2
//			print((a * 3) - 2); /////in threes from 1
//		}



		///backward
//		for (int a = (length2/3)-1; a > 0; a--) {
//
//			print(a * 3);
//		}
//
	}


}
