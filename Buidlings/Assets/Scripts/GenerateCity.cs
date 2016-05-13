using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;


public class GenerateCity : MonoBehaviour {

	public Camera mainCamera;
	private Ray ray;
	private RaycastHit hit;
	private GameObject hitObject = null;

	[Range(5,20)] public int minSize = 10;
	[Range(100,400)] public int mapWidth = 200;
	[Range(100,400)] public int mapHeight = 200;

	private List<GameObject> areas = new List<GameObject>();
	private List<GameObject> areasIndexDelete = new List<GameObject>();
	private List<Vector3> edgePoints = new List<Vector3>();
	private List<Vector3> mapEdgePoints = new List<Vector3>();


	public GameObject sphere;
	private List <GameObject> newSpheres = new List<GameObject>();

	private int xSize, ySize, zSize, roundness, gameObjectCount ;
	private bool roundTop,  roundFront, roundBack, roundSides = false;


	private void Awake () {

		this.transform.name = "city";
		StartCoroutine(GenerateCityBuildings ());


	}

	private IEnumerator GenerateCityBuildings () 
	{
		WaitForSeconds wait = new WaitForSeconds (0.05f);


		GameObject startCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		startCube.transform.localScale = new Vector3 (mapWidth,1,mapHeight);
		startCube.transform.position = new Vector3(transform.position.x + mapWidth/2, transform.position.y,transform.position.z + mapHeight/2);
		areas.Add (startCube);

		addMapEdges ();


		for (int i = 0; i < areas.Count; i++) {

			float choice = Random.Range(0.0f,1.0f);
			//Debug.Log (choice);

			if (choice <= 0.5f){
				splitX ( areas [i] );
			}else{

				splitZ ( areas [i] );
			}
			//yield return wait;
		}

		Debug.Log ("all Areas: "+areas.Count +"   toDeleted: "+areasIndexDelete.Count);


		for (int j = 0; j< areas.Count; j++) {

			for (int a = 0; a < areasIndexDelete.Count; a++) {

				areas.Remove (areasIndexDelete [a]);
			}
			if (areas [j] == null) {

				areas.RemoveAt(j);
			}
		}

		Debug.Log ("areas: "+areas.Count);

		yield return wait;


		for (int i = 0; i < areas.Count; i++) {

			GetVectors (areas [i]);
		}

		yield return wait;

		print("map edges: "+mapEdgePoints.Count);
		print ("areas edges: " + edgePoints.Count);

		for (int i = 0; i < areas.Count; i++) {

			xSize = (int)areas[i].GetComponent<MeshRenderer> ().bounds.size.x;
			zSize = (int)areas[i].GetComponent<MeshRenderer> ().bounds.size.z;

			//print ("bounds: " + areas[i].GetComponent<MeshRenderer> ().bounds);
			//print ("size:  " + areas[i].GetComponent<MeshRenderer> ().bounds.size);


			//move from center
			float xx = areas[i].transform.position.x - ((float)xSize/2.0f);
			float zz = areas[i].transform.position.z - ((float)zSize/2.0f);

			Vector3 pivotPoint = new Vector3 (xx,areas[i].transform.position.y, zz);
			createSphere (pivotPoint, newSpheres);

			roundTop = (Random.Range (0, 2) == 0);
			roundFront = (Random.Range (0, 2) == 0);
			roundBack = (Random.Range (0, 2) == 0);
			roundSides = (Random.Range (0, 2) == 0);

			int c = 0;
			for (int r = 0; r < 20; r++) {
			
				if (c < xSize && c < zSize && c < 5) {
					c++;
				}
			}
			roundness = Random.Range (0, c);

			print ("top: " + roundTop + "    front: " + roundFront + "   back: " + roundBack + "   sides: " + roundSides);
			print ("x: " + xSize + "    y: " + ySize + "   z: " + zSize + "   roundness: " + roundness);



			int splitSize = 25;

			if (xSize > splitSize || zSize > splitSize) {

				print ("over than split size");

				int xCount = 1;
				while (xSize / xCount > splitSize) {

					//print ("x res search "+xSize/xCount);
					xCount++;
				}
				float xOffset = xSize / xCount;
				print ("x Offset: " + xOffset + "   x count: " + xCount);


				int zCount = 1;
				while (zSize / zCount > splitSize) {
					//print (" z res search "+zSize/zCount);
					zCount++;
				}
				float zOffset = zSize / zCount;

				print ("z Offset: " + zOffset + "   z count: " + zCount);

				List<Vector3> pointsInArea = new List<Vector3> ();
				for (int s = 0; s < xCount; s++) {

					float xP = pivotPoint.x + (s * xOffset);

					for (int z = 0; z < zCount; z++) {

						float zP = pivotPoint.z + (z * zOffset);

						Vector3 finalP = new Vector3 (xP, pivotPoint.y, zP);
						pointsInArea.Add(finalP);
					}

				}
				//print ("all point: " + pointsInArea.Count);

				xSize = (int)xOffset - 6;
				zSize = (int)zOffset - 6;


		
				for (int o = 0; o < pointsInArea.Count ; o++) {

					ySize = Random.Range (40, 60);
					float distanceToCenter = Vector3.Distance (GetClosestEdge (areas [i].transform.position, mapEdgePoints), areas [i].transform.position);
					float scaleHeight1 = (distanceToCenter + ySize) / 2;

					print ("xSize:  "+xSize+"   height1: " + scaleHeight1+"   zSize  "+zSize);


					//Vector3 buildingScale1 = new Vector3 (xSize, scaleHeight, zSize);
					//					buildingScript.xSize = xSize;
					//					buildingScript.ySize = (int)scaleHeight1;
					//					buildingScript.zSize = zSize;

					//Vector3 buildingPos1 = new Vector3 (pointsInArea [o].x + 3, transform.localPosition.y, pointsInArea [o].z + 3);
					//getBuilding("building" + i,buildingPos1);
				}

				print ("point in area: " + pointsInArea.Count);

			} else {

				int removeFromX = Random.Range(2,5);
				int removeFromZ = Random.Range(2,5);
				xSize -= removeFromX;
				ySize = Random.Range (20, 40);
				zSize -= removeFromZ;

				float distanceToCenter = Vector3.Distance (GetClosestEdge (areas [i].transform.position, mapEdgePoints), areas [i].transform.position);
				float scaleHeight2 = (distanceToCenter + ySize) / 3;

				print ("xSize:  "+xSize+"   height2: " + scaleHeight2+"   zSize  "+zSize);

				//Vector3 buildingScale2 = new Vector3 (xSize, scaleHeight, zSize);
				//				buildingScript.xSize = xSize;
				//				buildingScript.ySize = (int)scaleHeight2;
				//				buildingScript.zSize = zSize;

				Vector3 buildingPos2 = new Vector3 (pivotPoint.x + (removeFromX/2), this.transform.position.y, pivotPoint.z + (removeFromZ/2));


				//getBuilding("building" + i,buildingPos2);
			}


		}


	}
	private void getBuilding(string name, Vector3 position){

		//		GameObject building = buildingScript.CreateBuilding (position) as GameObject;//CreateCube() as GameObject;
		//		building.transform.parent = this.transform;
		//		building.name = name;
		//
	}
	private GameObject createSphere(Vector3 pos , List <GameObject> objectArr){

		GameObject a = (GameObject) Instantiate(sphere, pos, Quaternion.identity);
		a.transform.localScale = new Vector3 (2.5f, 2.5f, 2.5f);
		a.GetComponent<Renderer> ().material.color = Color.white;
		a.transform.parent = this.transform;
		objectArr.Add (a);

		return a;
	}


	private void addMapEdges()
	{
		mapEdgePoints.Add( new Vector3(0, 0, 0));
		mapEdgePoints.Add( new Vector3(mapWidth/2, 0, 0)); //center
		mapEdgePoints.Add( new Vector3(mapWidth, 0, 0));
		mapEdgePoints.Add( new Vector3(mapWidth, 0, mapHeight/2));
		mapEdgePoints.Add( new Vector3(mapWidth, 0, mapHeight));
		mapEdgePoints.Add( new Vector3(mapWidth/2, 0, mapHeight));
		mapEdgePoints.Add( new Vector3(0, 0, mapHeight));
		mapEdgePoints.Add( new Vector3(0, 0, mapHeight/2));


		//mapEdgePoints.Add( new Vector3(mapWidth/2, 0, mapHeight/2)); //center
	}

	Vector3 GetClosestEdge(Vector3 currentPosition, List<Vector3> targets)
	{
		Vector3 bestTarget = new Vector3();
		float closestDistanceSqr = Mathf.Infinity;
		//Vector3 currentPosition = transform.position;

		foreach(Vector3 potentialTarget in targets)
		{
			Vector3 directionToTarget = potentialTarget - currentPosition;
			float dSqrToTarget = directionToTarget.sqrMagnitude;
			if(dSqrToTarget < closestDistanceSqr)
			{
				closestDistanceSqr = dSqrToTarget;
				bestTarget = potentialTarget;
			}
		}

		return bestTarget;
	}

	private void GetDistinctArrayList(List<Vector3> arr, int idx)
	{

		int count = 0;

		if (idx >= arr.Count) return;

		Vector3 val = arr[idx];
		foreach (Vector3 v in arr)
		{
			if (v.Equals(arr[idx]))
			{
				count++;
			}
		}

		if (count > 1)
		{
			arr.Remove(val);
			GetDistinctArrayList(arr, idx);
		}
		else
		{
			idx += 1;
			GetDistinctArrayList(arr, idx);
		}
	}
		
	void GetVectors ( GameObject cube) 
	{
		Vector3[] v = new Vector3[4];

		Vector3 bMin = cube.GetComponent<BoxCollider>().bounds.min;
		Vector3 bMax = cube.GetComponent<BoxCollider>().bounds.max;

		edgePoints.Add(new Vector3 (Mathf.Round (bMax.x), Mathf.Round (bMax.y), Mathf.Round (bMax.z)));
		edgePoints.Add(new Vector3 (Mathf.Round (bMin.x), Mathf.Round (bMax.y), Mathf.Round (bMin.z)));
		edgePoints.Add(new Vector3 (Mathf.Round (bMin.x), Mathf.Round (bMax.y), Mathf.Round (bMax.z)));
		edgePoints.Add(new Vector3 (Mathf.Round (bMax.x), Mathf.Round (bMax.y), Mathf.Round (bMin.z)));

		GetDistinctArrayList (edgePoints, 0);

	}

	void splitX(GameObject splitMe){

		float xSplit =  Random.Range(minSize,splitMe.transform.localScale.x - minSize);
		float split1 = splitMe.transform.localScale.x - xSplit;

		float x1 = splitMe.transform.position.x - ((xSplit - splitMe.transform.localScale.x) / 2);
		float x2 = splitMe.transform.position.x + ((split1 - splitMe.transform.localScale.x) / 2);

		if (xSplit > minSize){

			gameObjectCount += 1;
			GameObject c1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
			c1.transform.localScale = new Vector3 (xSplit, splitMe.transform.localScale.y,splitMe.transform.localScale.z);
			c1.transform.position = new Vector3(x1,splitMe.transform.position.y,splitMe.transform.position.z);
			c1.GetComponent<Renderer>().material.color = new Color(Random.Range(0.0f,1.0f),Random.Range(0.0f,1.0f), Random.Range(0.0f,1.0f));
			c1.transform.parent = this.transform;
			c1.name = "ground" + gameObjectCount;
			areas.Add (c1);


			gameObjectCount += 1;
			GameObject c2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
			c2.transform.localScale = new Vector3 (split1, splitMe.transform.localScale.y,splitMe.transform.localScale.z);
			c2.transform.position = new Vector3(x2,splitMe.transform.position.y,splitMe.transform.position.z);
			c2.GetComponent<Renderer>().material.color = new Color(Random.Range(0.0f,1.0f),Random.Range(0.0f,1.0f), Random.Range(0.0f,1.0f));
			c2.transform.parent = this.transform;

			c2.name = "ground" + gameObjectCount;
			areas.Add (c2);

			areasIndexDelete.Add(splitMe);
			GameObject.DestroyImmediate(splitMe);
		}		
	}

	void splitZ(GameObject splitMe){
		
		float zSplit = Random.Range(minSize, splitMe.transform.localScale.z - minSize);
		float zSplit1 = splitMe.transform.localScale.z - zSplit;

		float z1 = splitMe.transform.position.z - ((zSplit - splitMe.transform.localScale.z) / 2);
		float z2 = splitMe.transform.position.z+ ((zSplit1 - splitMe.transform.localScale.z) / 2);


		if (zSplit > minSize){
			
			gameObjectCount += 1;
			GameObject c1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
			c1.transform.localScale = new Vector3 (splitMe.transform.localScale.x, splitMe.transform.localScale.y,zSplit);
			c1.transform.position = new Vector3( splitMe.transform.position.x, splitMe.transform.position.y, z1);
			c1.GetComponent<Renderer>().material.color = new Color(Random.Range(0.0f,1.0f),Random.Range(0.0f,1.0f), Random.Range(0.0f,1.0f));
			c1.transform.parent = this.transform;
			c1.name = "ground" + gameObjectCount;
			areas.Add (c1);


			gameObjectCount += 1;
			GameObject c2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
			c2.transform.localScale = new Vector3 (splitMe.transform.localScale.x, splitMe.transform.localScale.y,zSplit1);
			c2.transform.position = new Vector3(splitMe.transform.position.x, splitMe.transform.position.y, z2);
			c2.GetComponent<Renderer>().material.color = new Color(Random.Range(0.0f,1.0f),Random.Range(0.0f,1.0f), Random.Range(0.0f,1.0f));
			c2.transform.parent = this.transform;

			c2.name = "ground" + gameObjectCount;
			areas.Add (c2);

			areasIndexDelete.Add(splitMe);
			GameObject.DestroyImmediate(splitMe);

		}
	}
		

	void Update()
	{
		if (Input.GetMouseButtonDown (0)) {

			ray = mainCamera.ScreenPointToRay (Input.mousePosition);

			if (Physics.Raycast (ray, out hit)) {

				for (int i = 0; i < areas.Count; i++) {

					if (hit.collider.gameObject == areas [i]) {
						hitObject = hit.collider.gameObject;

						//Debug.Log (areas.IndexOf(hit.collider.gameObject));
						hitObject.GetComponent<Renderer> ().material.color = new Color (Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f), Random.Range (0.0f, 1.0f));

						Debug.Log ("scale: "+hitObject.transform.localScale +"    bounds size: "+ hitObject.GetComponent<Renderer>().bounds.size);


					}
				}
					
			}
		}


	}


	
}
