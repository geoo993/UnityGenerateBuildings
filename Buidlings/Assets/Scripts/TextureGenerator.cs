using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextureGenerator : MonoBehaviour {


	private int resolution = 256;

	private MeshRenderer renderer;
	private Texture2D texture;


	private List<int> windowsIndexes = new List<int>();

	private List<Vector2[]> pixelsPoints = new List<Vector2[]>();
	private List <Color> interpolateColorsA = new List<Color>();
	private List <Color> interpolateColorsB = new List<Color>();
	private List <Color> interpolateComplete = new List<Color>();

	Color[] colors = new Color[]
	{
		Color.red, 
		Color.yellow, 
		Color.green, 
		Color.blue,
		Color.cyan,
		//Color.gray,
		//Color.magenta,
		Color.black,
		Color.white
	};
		
	List <float> id = new List<float>();
	List <float> times = new List<float>();

	private bool stripesFrequency = true;
	private float stripeFrequency = 16;//32
	private int stripeLoop = 16;
	private int stripeResolution = 256;


	private void Awake () {

		FillTexture ();
		
	}
	private void Update () {


		UpdateStripesColor ();


	}


	public void FillTexture () {

		renderer = GetComponent<MeshRenderer> ();

		if (texture == null) {


			// new Texture2D (width , heigth, TextureFormat: format, bool : mipmap)
			texture = new Texture2D (resolution, resolution, TextureFormat.RGB24, false);
			//texture = new Texture2D(resolution, resolution, TextureFormat.ARGB32, false);
			texture.name = "Procedural Texture";

			texture.wrapMode = TextureWrapMode.Clamp;
			texture.filterMode = FilterMode.Trilinear;//FilterMode.Bilinear; //FilterMode.Point;
			texture.anisoLevel = 9;

			renderer.material.mainTexture = texture;
		}



		if (texture.width != resolution) {
			texture.Resize(resolution, resolution);
		}

				
		pixelsPoints.Clear ();
		interpolateColorsA.Clear ();
		interpolateColorsB.Clear ();
		interpolateComplete.Clear ();
		id.Clear ();
		times.Clear ();
		renderer.material.mainTexture = null;

		texture = new Texture2D (256, 256, TextureFormat.RGB24, false);
		texture.name = "Stripes Texture";

		texture.wrapMode = TextureWrapMode.Clamp;
		texture.filterMode = FilterMode.Point;
		texture.anisoLevel = 9;


		stripeLoop = stripesFrequency ? 16 : 8;
		stripeFrequency = stripesFrequency ? 16 : 32;
		stripeResolution = (int)Mathf.Pow (stripeLoop, 2);

		
		// random offset windows indexes
		int a = 0;
		int rand = 0;
		while (a < stripeResolution)
		{

			rand = a + Random.Range(0,3);
			a = rand;

			//print(" a:  "+a); 
			windowsIndexes.Add (a);

			a ++;
		}
			
//		foreach (int inn in windowsIndexes) {
//			print (inn);
//		}
		//print ("windowsIndexes lenght:  " + windowsIndexes.Count);


		int v = 0;
		for (int x = 0; x < stripeLoop; x++) {

			List<Vector2> innerArray = new List<Vector2> ();

			for (int y = 0; y < stripeLoop; y++) {


				Color c = ExtensionMethods.RandomColor ();
				int yOffset1 = (int)stripeFrequency * x;
				int yOffset2 = (resolution - (resolution - ((int)stripeFrequency * x))) + (int)stripeFrequency; 

				for (int yOff = yOffset1; yOff < yOffset2; yOff++) {

					int xOffset1 = (int)stripeFrequency * y;
					int xOffset2 = (resolution - (resolution - ((int)stripeFrequency * y))) + (int)stripeFrequency; 
					for (int xOff = xOffset1; xOff < xOffset2; xOff++) {

						//texture.SetPixel (xOff, yOff, c);
						innerArray.Add (new Vector2 (xOff, yOff));
					}

				}

				pixelsPoints.Insert (v, innerArray.ToArray ());

				interpolateColorsA.Add (Color.red);
				interpolateColorsB.Add (Color.green);
				interpolateComplete.Add (Color.black);

				id.Add (0.0f);
				times.Add (Random.Range (10f, 20f));

				v++;
				//print (v);
			}

		}

//		print ("pixals Areas: "+pixelsPoints.Count);
//		print ("color A:  "+interpolateColorsA.Count);
//		print ("color B:  "+interpolateColorsB.Count);
//		print ("color Complete:  "+interpolateComplete.Count);
//		print ("ID:  "+id.Count);
//		print ("times:  "+times.Count);
//		print ("power multiplication of stripeLoop:  " + stripeResolution);
	}


	//private void UpdateStripesColor( int cases)
	private void UpdateStripesColor( )
	{
		//print (stripeResolution);


		int introLoop = 0;
		int midLoop = 0;
		int index = 0; 

		midLoop = stripeResolution;//windowsIndexes.Count - 1;

		for (int i = introLoop; i < midLoop; i++) {

			index = i;//windowsIndexes [i];

			if (id [index] < 1.0f) {
				
				id [index] += Time.deltaTime * (1.0f / times [index]);
			} else {
				id [index] = 0;
				times [index] = Random.Range (5, 15f);

				interpolateColorsA [index] = interpolateComplete [index];
				interpolateColorsB [index] = colors [Random.Range (0, colors.Length - 1)];
			}

			interpolateComplete [index] = Color.Lerp (interpolateColorsA [index], interpolateColorsB [index], id [index]);
		
		
			int xMin = ((int)pixelsPoints [index] [pixelsPoints [index].Length - 1].x - (int)stripeFrequency);
			int xMax = (int)pixelsPoints [index] [pixelsPoints [index].Length - 1].x;

			int yMin = ((int)pixelsPoints [index] [pixelsPoints [index].Length - 1].y - (int)stripeFrequency);  
			int yMax = (int)pixelsPoints [index] [pixelsPoints [index].Length - 1].y;

			for (int u = yMin; u < yMax; u++) {

				for (int h = xMin; h < xMax; h++) {

						//print (texture.GetPixel (h , u ));
					texture.SetPixel (h, u, interpolateComplete [index]);
				}
			}
				
		}

		//print (interpolateColorsA.Count +"   "+pixelsPoints.Count);

		//print ("idd  "+id [20]+"   times: "+times[20]);
		//Apply all SetPixel calls
		texture.Apply ();
		renderer.material.mainTexture = texture;
	}

		
}

