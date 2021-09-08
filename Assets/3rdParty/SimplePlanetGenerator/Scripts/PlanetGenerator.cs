using UnityEngine;
using System.Collections;

public class PlanetGenerator : MonoBehaviour {

	public bool isSimplex = true;

	public int multi = 8; //SCALE -- 0 - 10
	public float height = 1.3f; //Height Tweak 0-1
	public float cliff = 0.25f; //Cliff Tweak 0-1.5
	public float depth = 0.75f; //Water Tweak 0-1.5

	public float seed = 0.0f;

	public float hillStart = 0.8f;
	public float cliffPeak = 0.9f;
	public float groundArea = 0.45f;
	public float waterLine = 0.2f;

	public float hillDevider = 2.0f; //Cliff Multiplier Devided by Cliff Devider = height of hills
	public float groundMultiplier = 0.35f;

	public GameObject waterSphere;

	public float waterLevel = 0.3f;
	public Color32 waterColor;

	public Color32 underWaterColor;
	public Color32 aboveWaterColor; 
	public Color32 hillPeakColor;

	//SIMPLEX 
	Simplex simplex;

	int oct = 2;
	float lac = 0.4f; //NOISE 0.0 - 3.0
	float pers = 0.9f; //CLIFF FREQUENCY 0 - 4.0
	float amp = 3.5f; // AMPLIFICATION 0 -4.0

	Vector3 [] vertices;
	Vector3 [] normals;
	Vector3 [] origVertices;
	float [] heights;
	Color32 [] colors;

	Mesh mesh;

	float offset = 0.0f;

	void Awake(){
		GetMeshInfo ();
		GenerateHeights ();
		Visualize ();
		Colorize ();
		UpdateCollider ();
	}

	void Update(){
		Visualize ();
		Colorize ();
	}

	void GetMeshInfo(){
		mesh = GetComponent<MeshFilter> ().mesh;
		vertices = mesh.vertices;
		origVertices = mesh.vertices;
		normals = mesh.normals;
		heights = new float[vertices.Length];
		colors = new Color32[vertices.Length];
	}

	public void GenerateHeights(){
		if (isSimplex) {
			GenerateSimplex ();
		} else {
			GeneratePerlin ();
		}
	}

	void GenerateSimplex(){
		simplex = new Simplex (seed.ToString());
		for (int count = 0; count < vertices.Length; count++) {
			heights [count] = simplex.coherentNoise (vertices [count].x, vertices [count].y, vertices [count].z, oct, multi, amp, lac, pers);
		}
	}

	void GeneratePerlin(){
		for (int count = 0; count < vertices.Length; count++) {
			heights [count] = Mathf.PerlinNoise ((seed + (float) vertices[count].x), (seed + (float) vertices[count].z));
			count++;
		}
	}

	void Visualize(){
		VisualizeWater ();
		for (int count = 0; count < vertices.Length; count++) {
			if(heights[count] >= cliffPeak){
				vertices[count] = origVertices[count] + normals[count] * (heights[count] * (height * cliff));
			}
			else if(heights[count] > hillStart && heights[count] < cliffPeak){
				vertices[count] = origVertices[count] + normals[count] * (heights[count] * height * (cliff / hillDevider));
			}
			else if(heights[count] < groundArea && heights[count] > waterLine){
				vertices[count] = origVertices[count] + normals[count] * (heights[count] * height * groundMultiplier);
			}
			else if(heights[count] <= waterLine){
				vertices[count] = origVertices[count] + normals[count] * (heights[count] * (-height * -depth));
			}
			else{
				vertices[count] = origVertices[count] + normals[count] * (heights[count] * height);
			}
		}
		mesh.vertices = vertices;
		mesh.RecalculateBounds();
		mesh.RecalculateNormals();

	}

	void VisualizeWater (){
		if (waterSphere != null) {
			waterSphere.transform.localScale = new Vector3 (waterLevel + 10.0f, waterLevel + 10.0f, waterLevel + 10.0f);
			waterSphere.GetComponent<Renderer> ().materials [0].color = waterColor;

			offset = 0.2f * Time.time;
			waterSphere.GetComponent<Renderer> ().materials [0].SetTextureOffset ("_MainTex", new Vector2 (-offset, offset));
		}
	}

	public void UpdateCollider(){ //recalculates Collider
		if (GetComponent<MeshCollider>()) {
			Destroy (GetComponent<MeshCollider> ());
		}
		gameObject.AddComponent<MeshCollider> ();
	}

	void Colorize (){
		for (int count = 0; count < vertices.Length; count ++) {
			if(heights[count] > 0.03f){
				colors [count] = Color32.Lerp(aboveWaterColor, hillPeakColor, heights[count]);
			}
			else{
				colors[count] = Color32.Lerp (underWaterColor, aboveWaterColor, heights[count]);
			}


		}
		mesh.colors32 = colors;
	}


	public float getHeight(int i){
		return heights [i];
	}

	public Vector3 getOrig(int i){
		return origVertices [i];
	}

	public Vector3 getNormal(int i){
		return normals [i];
	}
}
