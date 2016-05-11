using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextureGenerator : MonoBehaviour {


	[Range(2, 512)] public int resolution = 256;

	MeshRenderer renderer;
	private Texture2D texture;
	private Color color = Color.blue;

	[Range(2, 36)] public float frequency = 16f;
	[Range(1, 8)] public int octaves = 1;
	[Range(1f, 4f)] public float lacunarity = 2f;
	[Range(0f, 1f)] public float persistence = 0.5f;
	[Range(1, 3)] public int dimensions = 3;
	public Gradient gradientColor = new Gradient();

	List<int> windowsIndexes = new List<int>();

	List<Vector2[]> pixelsPoints = new List<Vector2[]>();
	List <Color> interpolateColorsA = new List<Color>();
	List <Color> interpolateColorsB = new List<Color>();
	List <Color> interpolateComplete = new List<Color>();

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

	public bool stripesFrequency = false;
	private float stripeFrequency = 16;//32
	private int stripeLoop = 16;
	private int stripeResolution = 256;


	//private void Awake () {
	private void OnEnable () {

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
	
			FillTexture ();
		
	}
	private void Update () {

//		if (transform.hasChanged) {
//			transform.hasChanged = false;
//			FillTexture();
//		}


		UpdateStripesColor ();


	}

	private void addGradient (Gradient g)
	{

		GradientColorKey blue = new GradientColorKey(Color.blue, 0.0f);
		GradientColorKey white = new GradientColorKey(Color.white, 0.3f);
		GradientColorKey black = new GradientColorKey(Color.black, 0.45f);
		GradientColorKey yellow = new GradientColorKey(Color.yellow, 0.6f);
		GradientColorKey red = new GradientColorKey(Color.red, 1f);

		GradientAlphaKey blueAlpha = new GradientAlphaKey(1,0);
		GradientAlphaKey yellowAlpha = new GradientAlphaKey(1,1);


		GradientColorKey[] colorKeys = new GradientColorKey[]{blue, white, black, yellow, red};
		GradientAlphaKey[] alphaKeys = new GradientAlphaKey[]{blueAlpha,yellowAlpha};
		g.SetKeys(colorKeys, alphaKeys);


	}




	public void FillTexture () {


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



//		//int r = 235;
//		for (int r = 0; r < resolution; r++) {
//
//			Color startC = ExtensionMethods.RandomColor ();
//			Color endC = ExtensionMethods.RandomColor ();
//			Color c = Color.Lerp(startC, endC, timer/100);
//
//			//print ("i: "+r+"    "+pixelsPoints[0] [r]);
//			int xMin = ((int)pixelsPoints [r] [pixelsPoints [r].Length - 1].x -  (int)frequency);
//			int xMax = (int)pixelsPoints [r] [pixelsPoints [r].Length - 1].x;
//
//			int yMin = ((int)pixelsPoints [r] [pixelsPoints [r].Length - 1].y -  (int)frequency);  
//			int yMax = (int)pixelsPoints [r] [pixelsPoints [r].Length - 1].y;
//
//			for (int u = yMin; u < yMax; u++) {
//				
//				for (int h = xMin; h < xMax; h++) {
//
//					texture.SetPixel (h, u, c);
//
//				}
//			}
//		}
		
			//texture.Apply();

			//renderer.material.mainTexture = texture;


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


//		switch (cases) {
//
//		case 0:
//			introLoop = 0;
//			midLoop = (stripeResolution / 2);
//			//index   (a * 2) ///in twos from 0
//
//			break;
//		case 1: 
//			introLoop = 1;
//			midLoop = (stripeResolution / 2) + 1;
//			//index   (a * 2) - 1    ///in twos from 1
//
//			break;
//		case 2: 
//			introLoop = 0;
//			midLoop = ((stripeResolution + 1) / 3) + 1;
//
//			///index (i * 3)   //in threes from 0
//
//			break;
//		case 3: 
//			introLoop = 1;
//			midLoop = ((stripeResolution + 1) / 3) + 1;
//
//			//index (a * 3) ///in threes eliminating -1 at start
//			//index (a * 3) - 1  ///in threes from 2
//			//index (a * 3) - 2   /////in threes from 1
//
//			break;
//		case 4: 
//			introLoop = 1;
//			midLoop = ((stripeResolution + 1) / 3) + 1;
//			//index (a * 3) - 2   /////in threes from 1
//
//			break;
//		case 5: 
//			introLoop = 0;
//			midLoop = (stripeResolution );
//			/////loop trough all
//
//			break;
//		}
//
//		for (int i = introLoop; i < midLoop; i++) {
//			
//
//			switch (cases) {
//
//			case 0:
//				index = (i * 2) ;
//				break;
//			case 1:
//				index = (i * 2) - 1;
//				break;
//			case 2:
//				index = (i * 3) ;
//				break;
//			case 3:
//				index = (i * 3) - 1;
//				break;
//			case 4:
//				index = (i * 3) - 2;
//				break;
//			case 5:
//				index = i;
//				break;
//			}


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

