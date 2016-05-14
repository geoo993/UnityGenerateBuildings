using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;


public class GenerateCity : MonoBehaviour {

	[Range(10,40)] public int minSize = 10;
	[Range(100,1000)] public int mapWidth = 200;
	[Range(100,1000)] public int mapHeight = 200;

	private List<GameObject> areas = new List<GameObject>();
	private List<GameObject> areasIndexDelete = new List<GameObject>();
	private List<Vector3> edgePoints = new List<Vector3>();
	private List<Vector3> mapEdgePoints = new List<Vector3>();

	private int xSize, ySize, zSize, roundness, gameObjectCount ;
	private bool roundTop,  roundFront, roundBack, roundSides = false;
	private List <Vector3> verticesCopy = new List<Vector3> ();

	private List<int[]> controlPoints = new List<int[]>();
	private List<int> listOfVerticesIndexes = new List<int>();
	private List<int> pivotControlPoint= new List<int>();
	private List<int> topControlPointIndexes = new List<int>();
	private List<int> frontControlPointIndexes = new List<int>();
	private List<int> backControlPointIndexes = new List<int>();
	private List<int> sidesControlPointIndexes = new List<int>();
	private List<Vector3> vertices = new List<Vector3>();
	private List<Vector3> normals = new List<Vector3>();
	private List<Vector2> uv = new List<Vector2>();
	private int[] triangles; 

	private static int
	SetQuad (int[] triangles, int i, int v00, int v10, int v01, int v11) {

		triangles[i] = v00;
		triangles[i + 4] = v01;
		triangles[i + 1] = triangles[i + 4];
		triangles[i + 3] = v10;
		triangles[i + 2] = triangles[i + 3];
		triangles[i + 5] = v11;

		return i + 6;
	}


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

			xSize = (int)areas [i].GetComponent<MeshRenderer> ().bounds.size.x;
			zSize = (int)areas [i].GetComponent<MeshRenderer> ().bounds.size.z;

			//print ("bounds: " + areas[i].GetComponent<MeshRenderer> ().bounds);
			//print ("size:  " + areas[i].GetComponent<MeshRenderer> ().bounds.size);

			float distanceToCenter = Vector3.Distance (new Vector3 (mapWidth / 2, 0, mapHeight / 2), areas [i].transform.localPosition);

			float distanceToMapEdge = Vector3.Distance (GetClosestEdge (areas [i].transform.localPosition, mapEdgePoints), areas [i].transform.localPosition);


			//move from center
			float xx = areas [i].transform.position.x - ((float)xSize / 2.0f);
			float zz = areas [i].transform.position.z - ((float)zSize / 2.0f);

			Vector3 pivotPoint = new Vector3 (xx, areas [i].transform.position.y, zz);
		
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

			if (distanceToCenter < 150.0f) {

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
							pointsInArea.Add (finalP);
						}

					}
					//print ("all point: " + pointsInArea.Count);

					xSize = (int)xOffset - 6;
					zSize = (int)zOffset - 6;


		
					for (int o = 0; o < pointsInArea.Count; o++) {

						ySize = Random.Range (40, 60) + ((int)distanceToCenter / 2);
						print ("xSize:  " + xSize + "   ySize: " + ySize + "   zSize  " + zSize);

						Vector3 buildingPos1 = new Vector3 (pointsInArea [o].x + 3, transform.localPosition.y, pointsInArea [o].z + 3);
						getBuilding ("building" + i, buildingPos1);
					}

					print ("point in area: " + pointsInArea.Count);

				} else {

					int removeFromX = Random.Range (2, 5);
					int removeFromZ = Random.Range (2, 5);
					xSize -= removeFromX;
					ySize = Random.Range (20, 40) + ((int)distanceToCenter / 2);
					zSize -= removeFromZ;

					print ("xSize:  " + xSize + "   ySize: " + ySize + "   zSize  " + zSize);

					Vector3 buildingPos2 = new Vector3 (pivotPoint.x + (removeFromX / 2), this.transform.position.y, pivotPoint.z + (removeFromZ / 2));


					getBuilding ("building" + i, buildingPos2);
				}


			}
		}
		yield return wait;

		for (int i = 0; i < areas.Count; i++) {
			areas [i].GetComponent<MeshRenderer> ().material.color = Color.black;
		}

	}
	private void getBuilding(string name, Vector3 position){

		GameObject building = CreateBuilding (position) as GameObject;
		building.transform.parent = this.transform;
		building.name = name;
		
	}


////-------------------------------------------------------------------------------------------------------------- map area

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
////-------------------------------------------------------------------------------------------------------------- building mesh		

	public GameObject CreateBuilding(Vector3 position)
	{

		verticesCopy.Clear();
		controlPoints.Clear();
		listOfVerticesIndexes.Clear();

		pivotControlPoint.Clear();
		topControlPointIndexes.Clear (); 
		frontControlPointIndexes.Clear();
		backControlPointIndexes.Clear (); 
		sidesControlPointIndexes.Clear();

		vertices.Clear();
		normals.Clear();
		uv.Clear();

		triangles = null;


		GameObject build = new GameObject ();

		MeshFilter meshF = build.AddComponent< MeshFilter >();
		Mesh m = meshF.mesh;
		if (meshF == null){
			Debug.LogError("MeshFilter not found!");
			return null;
		}

		m = meshF.sharedMesh;
		if (m == null){
			meshF.mesh = new Mesh();
			m = meshF.sharedMesh;
		}
		m.name = "building mesh";

		m.Clear();


		CreateVertices();
		GetIndexes ();
		RandomOutlinesGeneration ();
		CreateTriangles();

		m.vertices = vertices.ToArray();
		m.triangles = triangles;

		m.normals = normals.ToArray();
		m.uv = uv.ToArray();

		m.RecalculateNormals();
		m.RecalculateBounds();
		m.Optimize();

		MeshCollider meshC = build.AddComponent(typeof(MeshCollider)) as MeshCollider;
		meshC.sharedMesh = m; // Give it your mesh here.

		MeshRenderer meshR = build.AddComponent<MeshRenderer> ();

		CreateColorAndtexture (meshR);
		//build.AddComponent<TextureGenerator> ();

		build.transform.position = position;

		return build;
	}

	private void GetIndexes (){

		int xlength = xSize + 1;
		int ylength = ySize + 1;
		int zlength = zSize + 1;

		int zExtra = zSize - 2;
		int offset = ((xlength * 2 ) + (zSize - 1 + zExtra)) ;

		//Debug.Log (" offset " + offset);

		for (int x = 0; x < offset + 1; x++) {

			for (int z = 0; z < zlength; z++)
			{
				List<int> innerArray = new List<int>();

				for (int y = 0; y < ylength; y++)
				{
					int myPos = (((offset * y) + x) + y);
					innerArray.Add (myPos);
					//print(innerArray[y]);
				}
				controlPoints.Insert(x, innerArray.ToArray());
			}

		}



		int p = 0;
		for (int v = 0; v < vertices.Count; v++) {

			verticesCopy.Add( vertices [v] );

			listOfVerticesIndexes.Add (p);
			p++;

			//top vertices
			if (vertices [v].y == ySize) {
				topControlPointIndexes.Add (v);
			}

		}
		//Debug.Log ("vertices length: "+vertices.Count +"   list of indexes length: "+ listOfVerticesIndexes.Count+"  vertex points copied: "+verticesCopy.Count);

		int midY = (int)(Mathf.Round (ySize / 2));
		int pivot = 0;

		for (int vi = 0; vi < listOfVerticesIndexes.Count; vi++) {

			//print (listOfIndexes.ToArray());
			for (int o = 0; o < offset + 1; o++) {

				if (  listOfVerticesIndexes[vi].Equals(controlPoints [o] [midY])   ) {

					pivotControlPoint.Add (pivot);
					//print (listOfIndexes[s]+"   "+newSpheres.Count);
					pivot++;
				} 
			}
		}

		////front
		for (int f = 0; f < xlength; f++) {

			frontControlPointIndexes.Add (f);
		}

		////back
		for (int b = xSize + zSize; b < pivotControlPoint.Count - (zSize - 1); b++) {

			backControlPointIndexes.Add (b);
		}


		//first side
		for (int sd = 0; sd < zSize - 1; sd++) {

			sidesControlPointIndexes.Add (sd + xlength);
		}

		//second side
		for (int sd2 = (xlength * 2) + (zSize - 1); sd2 < pivotControlPoint.Count; sd2++) {

			sidesControlPointIndexes.Add (sd2);
		}


	}

	private void RandomOutlinesGeneration()
	{
		float range = 3.5f;

		////front calcutations
		int fFromLoop = Random.Range (0, frontControlPointIndexes.Count - 1);
		int fToLoop = Mathf.Clamp (Random.Range (fFromLoop, frontControlPointIndexes.Count - 1), 0, frontControlPointIndexes.Count - 1);
		float fRandOffset = Random.Range (-range, range);

		//print ("front from: " + fFromLoop + "   front too: " + fToLoop + "   all front:" + frontControlPointIndexes.Count);

		if (fToLoop > fFromLoop + 1) {

			for (int i = fFromLoop; i < fToLoop; i++) {

				for (int z = 0; z < controlPoints [frontControlPointIndexes [i]].Length; z++) {

					vertices [controlPoints [frontControlPointIndexes [i]] [z]] = new Vector3 (
						vertices [controlPoints [frontControlPointIndexes [i]] [z]].x,
						vertices [controlPoints [frontControlPointIndexes [i]] [z]].y,
						vertices [controlPoints [frontControlPointIndexes [i]] [z]].z + fRandOffset);
				}

			}

		}

		////back calcutations
		int bFrom = Random.Range (0, backControlPointIndexes.Count - 1);
		int bTo = Mathf.Clamp (Random.Range (bFrom, backControlPointIndexes.Count - 1), 0, backControlPointIndexes.Count - 1);
		float bRandOffset = Random.Range (-range, range);

		//print ("back from: " + bFrom + "   back too: " + bTo + "   all backs:" + backControlPointIndexes.Count);

		if (bTo > bFrom + 1) {
			for (int i = bFrom; i < bTo; i++) {

				for (int z = 0; z < controlPoints [backControlPointIndexes [i]].Length; z++) {

					vertices [controlPoints [backControlPointIndexes [i]] [z]] = new Vector3 (
						vertices [controlPoints [backControlPointIndexes [i]] [z]].x,
						vertices [controlPoints [backControlPointIndexes [i]] [z]].y,
						vertices [controlPoints [backControlPointIndexes [i]] [z]].z + fRandOffset);
				}

			}
		}


		////sides calcutations
		int sType = Random.Range (0, 3);
		int sFrom = 0;
		int sTo = 0;
		int sFrom2 = 0;
		int sTo2 = 0;
		float sRandOffset = Random.Range (-range, range);

		switch (sType) {

		case 0:
			sFrom = 1;
			sTo = (sidesControlPointIndexes.Count / 2) - 1;
			break;
		case 1:
			sFrom = (sidesControlPointIndexes.Count / 2) + 1;
			sTo = (sidesControlPointIndexes.Count) - 1;
			break;
		case 2:
			sFrom = 1;
			sTo = (sidesControlPointIndexes.Count/2) - 1;
			sFrom2 = sTo + 2;
			sTo2 = sidesControlPointIndexes.Count - 1;
			break;

		}

		//print (" Sides Type" + sType + "   sides from: " + sFrom + "   sides too: " + bTo + "   all sides:" + sidesControlPointIndexes.Count);

		if (sType != 2) {

			for (int i = sFrom; i < sTo; i++) {

				//if (sType != 2 || sType == 2 && i != (sidesControlPointIndexes.Count / 2) && i != ((sidesControlPointIndexes.Count / 2) - 1)) {

				for (int z = 0; z < controlPoints [sidesControlPointIndexes [i]].Length; z++) {

					vertices [controlPoints [sidesControlPointIndexes [i]] [z]] = new Vector3 (
						vertices [controlPoints [sidesControlPointIndexes [i]] [z]].x + sRandOffset,
						vertices [controlPoints [sidesControlPointIndexes [i]] [z]].y,
						vertices [controlPoints [sidesControlPointIndexes [i]] [z]].z);
				}
				//				changeColor (newSpheres [sidesControlPointIndexes [i]], Color.white);
			}
		}else{

			for (int a = sFrom; a < sTo; a++) {

				for (int z = 0; z < controlPoints [sidesControlPointIndexes [a]].Length; z++) {

					vertices [controlPoints [sidesControlPointIndexes [a]] [z]] = new Vector3 (
						vertices [controlPoints [sidesControlPointIndexes [a]] [z]].x + sRandOffset,
						vertices [controlPoints [sidesControlPointIndexes [a]] [z]].y,
						vertices [controlPoints [sidesControlPointIndexes [a]] [z]].z);
				}

				//print (sidesControlPointIndexes[a]);
			}
			for (int aa = sFrom2; aa < sTo2; aa++) {

				for (int z = 0; z < controlPoints [sidesControlPointIndexes [aa]].Length; z++) {

					vertices [controlPoints [sidesControlPointIndexes [aa]] [z]] = new Vector3 (
						vertices [controlPoints [sidesControlPointIndexes [aa]] [z]].x - sRandOffset,
						vertices [controlPoints [sidesControlPointIndexes [aa]] [z]].y,
						vertices [controlPoints [sidesControlPointIndexes [aa]] [z]].z);
				}
				//print (sidesControlPointIndexes[aa]);
			}
		}


		////top calcutations
		float tRandOffset = Random.Range (-range, range);

		for (int y = 0; y < topControlPointIndexes.Count; y++) {

			vertices [topControlPointIndexes [y]] = new Vector3 (
				vertices [topControlPointIndexes [y]].x,
				vertices [topControlPointIndexes [y]].y +tRandOffset,
				vertices [topControlPointIndexes [y]].z);

		}

		//print (" top offset:  " + tRandOffset);

	}

	private void CreateVertices() {


		int cornerVertices = 8;
		int edgeVertices = (xSize + ySize + zSize - 3) * 4;

		int faceVertices = (
			(xSize - 1) * (ySize - 1) +
			(xSize - 1) * (zSize - 1) +
			(ySize - 1) * (zSize - 1)) * 2;

		int verticesLength = cornerVertices + edgeVertices + faceVertices;
		//vertices = new Vector3[verticesLength];
		//normals = new Vector3[verticesLength];
		//uv = new Vector2[verticesLength];

		//print ("vertices Length: "+verticesLength);

		int v = 0;
		// sides
		for (int y = 0; y <= ySize; y++) {
			for (int x = 0; x <= xSize; x++) {
				SetVertex(v++, x, y, 0);
			}
			for (int z = 1; z <= zSize; z++) {
				SetVertex(v++, xSize, y, z);
			}
			for (int x = xSize - 1; x >= 0; x--) {
				SetVertex(v++, x, y, zSize);
			}

			for (int z = zSize - 1; z > 0; z--) {
				SetVertex(v++, 0, y, z);
			}
		}


		// top 
		for (int z = 1; z < zSize; z++) {
			for (int x = 1; x < xSize; x++) {
				SetVertex(v++, x, ySize, z);
			}
		}
		//bottom
		for (int z = 1; z < zSize; z++) {
			for (int x = 1; x < xSize; x++) {
				SetVertex(v++, x, 0, z);
			}
		}



	}
	private void SetVertex (int i, int x, int y, int z) {

		Vector3 vect = new Vector3 (x, y, z);
		//vertices[i] = new Vector3(x, y, z);
		Vector3 inner = vect;


		////sides
		if (x < roundness) {
			if (roundSides) {
				inner.x = roundness;
			} else {
				inner.x = 0;
			}
		}
		else if (x > xSize - roundness) {

			if (roundSides) {
				inner.x = xSize - roundness;
			} else {
				inner.x = xSize; 
			}
		}

		////top and bottom
		if (y < roundness) {
			//bottom rounder
			//inner.y = roundness;
		}
		else if (y > ySize - roundness) {
			// top rounder
			//inner.y = 0;
			if (roundTop) {
				inner.y = ySize - roundness;
			} else {
				inner.y = ySize; 
			}
		}

		////front and back
		if (z < roundness) {
			// add or disable front rounder
			if (roundFront) {
				inner.z = roundness;
			} else {
				inner.z = 0;
			}
		}
		else if (z > zSize - roundness) {
			//add or disable back rounder
			if (roundBack) {
				inner.z = zSize - roundness;
			} else {
				inner.z = zSize;
			}
		}

		//		normals[i] = (vect - inner).normalized;
		//		vertices[i] = inner + normals[i] * roundness;
		//		uv[i] = new Vector2((float)x / ( xSize), (float)y / (ySize ));

		normals.Add((vect - inner).normalized);
		vertices.Add(inner + normals[i] * roundness);
		uv.Add(new Vector2((float)x / ( xSize), (float)y / (ySize) ));


	}

	private void CreateTriangles () {

		int quads = (xSize * ySize + xSize * zSize + ySize * zSize) * 2;
		int triLength = quads * 6;
		triangles = new int[triLength];
		int ring = (xSize + zSize) * 2;
		int t = 0, v = 0;

		for (int y = 0; y < ySize; y++, v++) {
			for (int q = 0; q < ring - 1; q++, v++) {
				t = SetQuad(triangles, t, v, v + 1, v + ring, v + ring + 1);
			}
			t = SetQuad(triangles, t, v, v - ring + 1, v + ring, v + 1);
		}

		t = CreateTopFace(triangles, t, ring);
		t = CreateBottomFace(triangles, t, ring);


	}

	private int CreateTopFace (int[] triangles, int t, int ring) {
		int v = ring * ySize;
		for (int x = 0; x < xSize - 1; x++, v++) {
			t = SetQuad(triangles, t, v, v + 1, v + ring - 1, v + ring);
		}
		t = SetQuad(triangles, t, v, v + 1, v + ring - 1, v + 2);

		int vMin = ring * (ySize + 1) - 1;
		int vMid = vMin + 1;
		int vMax = v + 2;

		for (int z = 1; z < zSize - 1; z++, vMin--, vMid++, vMax++) {
			t = SetQuad(triangles, t, vMin, vMid, vMin - 1, vMid + xSize - 1);
			for (int x = 1; x < xSize - 1; x++, vMid++) {
				t = SetQuad(
					triangles, t,
					vMid, vMid + 1, vMid + xSize - 1, vMid + xSize);
			}
			t = SetQuad(triangles, t, vMid, vMax, vMid + xSize - 1, vMax + 1);
		}

		int vTop = vMin - 2;
		t = SetQuad(triangles, t, vMin, vMid, vTop + 1, vTop);
		for (int x = 1; x < xSize - 1; x++, vTop--, vMid++) {
			t = SetQuad(triangles, t, vMid, vMid + 1, vTop, vTop - 1);
		}
		t = SetQuad(triangles, t, vMid, vTop - 2, vTop, vTop - 1);

		return t;
	}

	private int CreateBottomFace (int[] triangles, int t, int ring) {
		int v = 1;
		int vMid = vertices.Count - (xSize - 1) * (zSize - 1);

		t = SetQuad(triangles, t, ring - 1, vMid, 0, 1);
		for (int x = 1; x < xSize - 1; x++, v++, vMid++) {
			t = SetQuad(triangles, t, vMid, vMid + 1, v, v + 1);
		}
		t = SetQuad(triangles, t, vMid, v + 2, v, v + 1);

		int vMin = ring - 2;
		vMid -= xSize - 2;
		int vMax = v + 2;

		for (int z = 1; z < zSize - 1; z++, vMin--, vMid++, vMax++) {
			t = SetQuad(triangles, t, vMin, vMid + xSize - 1, vMin + 1, vMid);
			for (int x = 1; x < xSize - 1; x++, vMid++) {
				t = SetQuad(
					triangles, t,
					vMid + xSize - 1, vMid + xSize, vMid, vMid + 1);
			}
			t = SetQuad(triangles, t, vMid + xSize - 1, vMax + 1, vMid, vMax);
		}

		int vTop = vMin - 1;
		t = SetQuad(triangles, t, vTop + 1, vTop, vTop + 2, vMid);
		for (int x = 1; x < xSize - 1; x++, vTop--, vMid++) {
			t = SetQuad(triangles, t, vTop, vTop - 1, vMid, vMid + 1);
		}
		t = SetQuad(triangles, t, vTop, vTop - 1, vMid, vTop - 2);

		return t;
	}


	private void CreateColorAndtexture(MeshRenderer mR) {


		Material material = new Material (Shader.Find (".ShaderExample/TextureSplatting"));
		//Material material = new Material (Shader.Find ("Self-Illumin/Bumped Diffuse"));
		//Material material = new Material (Shader.Find ("Standard"));
		//material.color = Color.Lerp(Color.white, new Color((48.0f/255.0f),(48.0f/255.0f),(80.0f/255.0f)), Random.Range(0.0f, 1.0f));


		Texture[] smallStripes = new Texture[] {
			Resources.Load ("TextureStripe2") as Texture,
			Resources.Load ("TextureStripe7") as Texture,
			Resources.Load ("TextureStripe8") as Texture,
			Resources.Load ("TextureStripe9") as Texture
		};
		int pickSmallStripes = (int)Mathf.Floor (Random.value * smallStripes.Length);


		Texture[] bigStripes = new Texture[] {

			Resources.Load ("TextureStripe1") as Texture,
			Resources.Load ("TextureStripe3") as Texture,
			Resources.Load ("TextureStripe4") as Texture,
			Resources.Load ("TextureStripe5") as Texture,
			Resources.Load ("TextureStripe6") as Texture,
			Resources.Load ("TextureStripe10") as Texture
		};
		int pickBigStripes = (int)Mathf.Floor (Random.value * bigStripes.Length);

		Texture[] bigStripesInverted = new Texture[] {

			Resources.Load ("TextureStripe11") as Texture,
			Resources.Load ("TextureStripe33") as Texture,
			Resources.Load ("TextureStripe44") as Texture,
			Resources.Load ("TextureStripe55") as Texture,
			Resources.Load ("TextureStripe66") as Texture
		};
		int pickbigStripesInverted = (int)Mathf.Floor (Random.value * bigStripesInverted.Length);

		Texture small = smallStripes [pickSmallStripes] as Texture;
		Texture big = bigStripes [pickBigStripes] as Texture;
		Texture inverted = bigStripesInverted [pickbigStripesInverted] as Texture;


		material.SetTexture ("_MainTex", small);
		material.SetTextureScale ("_MainTex", new Vector2(2,Random.Range(2.0f,4.0f))); 

		material.SetFloat ("_White", Random.Range(0.0f , 1.0f));
		material.SetFloat ("_Transition1", Random.Range (0.25f, 1.0f));

		material.SetTexture ("_Texture1", inverted);
		material.SetTexture ("_Texture2", big);

		mR.material = material;

	}


	
}
