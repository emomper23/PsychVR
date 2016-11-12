using System.Collections.Generic;
using UnityEngine;

namespace ProceduralToolkit.Examples.UI
{
	public class LowPolyTerrainGeneratorUI : UIBase
	{
		public MeshFilter meshFilter;
		public GameObject terrain;
		public RectTransform leftPanel;
		private MeshCollider meshCollider;
		private MeshCollider meshCollider2;
		public List<Mesh> meshes = new List<Mesh>();
		public List<MeshDraft> drafts = new List<MeshDraft>();


		[Space]
		[Range(minXSize, maxXSize)]
		public int terrainSizeX = 20;
		[Range(minYSize, maxYSize)]
		public int terrainSizeY = 1;
		[Range(minZSize, maxZSize)]
		public int terrainSizeZ = 20;
		[Range(minCellSize, maxCellSize)]
		public float cellSize = 1;
		[Range(minNoiseScale, maxNoiseScale)]
		public int noiseScale = 5;
		private const int minXSize = 10;
		private const int maxXSize = 100;
		private const int minYSize = 1;
		private const int maxYSize = 5;
		private const int minZSize = 10;
		private const int maxZSize = 100;
		private const float minCellSize = 0.3f;
		private const float maxCellSize = 2;
		private const int minNoiseScale = 1;
		private const int maxNoiseScale = 20;
		private int vert_x;
		private int vert_z;


		private List<ColorHSV> targetPalette = new List<ColorHSV>();
		private List<ColorHSV> currentPalette = new List<ColorHSV>();

		private void Awake()
		{

			RenderSettings.skybox = new Material(RenderSettings.skybox);
			meshCollider = terrain.GetComponents<MeshCollider>()[0];
			meshCollider2 = terrain.GetComponents<MeshCollider>()[1];


			Generate();
			currentPalette.AddRange(targetPalette);
		}

		private void Update()
		{
			SkyBoxGenerator.LerpSkybox(RenderSettings.skybox, currentPalette, targetPalette, 0, 1, 4, Time.deltaTime);
		}

		public void Generate()
		{
			Vector3 terrainSize = new Vector3(terrainSizeX, terrainSizeY, terrainSizeZ);

			targetPalette = RandomE.TetradicPalette(0.25f, 0.75f);
			targetPalette.Add(ColorHSV.Lerp(targetPalette[0], targetPalette[1], 0.5f));

			var gradient = ColorE.Gradient(from: targetPalette[2].WithSV(0.8f, 0.8f),
				to: targetPalette[3].WithSV(0.8f, 0.8f));

			var draft = LowPolyTerrainGenerator.TerrainDraft(terrainSize, cellSize, noiseScale, gradient);
			vert_x = (int)(terrainSizeX / cellSize);
			vert_z = (int)(terrainSizeZ / cellSize);
			Debug.Log(vert_x + " " + vert_z);
			for (int i = 0; i < 9; i++)
			{
				MeshDraft temp = LowPolyTerrainGenerator.TerrainDraft(terrainSize, cellSize, noiseScale, gradient);
				temp.Move(Vector3.left * terrainSizeX / 2 + Vector3.back * terrainSizeZ / 2);
				drafts.Add(temp);

			}
			draft.Move(Vector3.left * terrainSizeX / 2 + Vector3.back * terrainSizeZ / 2);


			meshFilter.mesh = draft.ToMesh();
			meshCollider.sharedMesh = draft.ToMesh();
			meshCollider2.sharedMesh = draft.ToMesh();

			MorphFitRight(drafts[1], drafts[0]);
			//MorphFitDown(drafts[])

			MorphFitRight(drafts[2], drafts[1]);
			MorphFitRight(drafts[5], drafts[4]);
			MorphFitRight(drafts[7], drafts[6]);
			MorphFitRight(drafts[4], drafts[3]);
			MorphFitRight(drafts[8], drafts[7]);



			//ConnectMeshTwo(drafts[3], drafts[4]);
			//drafts[4].Rotate(Quaternion.Euler(0, -90, 0));
			//drafts[1].Rotate(Quaternion.Euler(0, -90, 0));
			//ConnectMesh(drafts[4], drafts[1]);
			//drafts[4].Rotate(Quaternion.Euler(0, 90, 0));
			//drafts[1].Rotate(Quaternion.Euler(0, 90, 0));

			for (int i = 0; i < 9; i++)
			{
				meshes.Add(drafts[i].ToMesh());
			}


		}

		public void MorphFitRight(MeshDraft fromMesh, MeshDraft toMesh)
		{

			Debug.Log(toMesh.triangles.ToArray().Length);
			Debug.Log(toMesh.vertices.ToArray().Length);
			Debug.Log(toMesh.normals.ToArray().Length);
			int length = toMesh.normals.ToArray().Length;
			Vector3[] vert = fromMesh.vertices.ToArray();
			Vector3[] norm = fromMesh.normals.ToArray();
			int[] tri = fromMesh.triangles.ToArray();
			// center.colors.RemoveRange(0, length);
			fromMesh.vertices.RemoveRange(0, length);
			fromMesh.normals.RemoveRange(0, length);
			fromMesh.triangles.RemoveRange(0, length);

			for (int i = 0; i < length; i++)
			{
				if (i > vert_x * 6 - 1)
				{
					//    center.colors.Add(new Color(0, 255, 0));
					fromMesh.vertices.Add(vert[i]);
					fromMesh.triangles.Add(tri[i]);
					fromMesh.normals.Add(norm[i]);
				}
				else
				{

					// center.colors.Add(new Color(255, 0, 0));
					// center.vertices.Add(draft.vertices[i]);
					fromMesh.triangles.Add(toMesh.triangles[i]);
					fromMesh.normals.Add(toMesh.normals[i]);
					if (i % 6 < 3)
					{   //0, 1, 2, 6=0

						if (i % 6 == 0)
						{   //0, 6=0
							//center.colors.Add(new Color(0, 0, 255));
							fromMesh.vertices.Add(new Vector3(vert[i].x, toMesh.vertices[(length - vert_x * 6) + i + 1].y, vert[i].z));
						}
						else
						{   //1, 2
							//        center.colors.Add(new Color(255, 0, 0));
							fromMesh.vertices.Add(vert[i]);
						}
					}
					else
					{   //3, 4, 5
						if (i % 6 == 4)
						{   //4
							//center.colors.Add(new Color(255, 0, 0));
							fromMesh.vertices.Add(vert[i]);
						}
						else if( i % 6 == 5)
						{   //5
							//center.colors.Add(new Color(0, 255, 0));
							fromMesh.vertices.Add(new Vector3(vert[i].x, toMesh.vertices[(length - vert_x * 6) + i - 1].y, vert[i].z));
						}
						else if( i % 6 == 3)
						{   //3
							fromMesh.vertices.Add(new Vector3(vert[i].x, toMesh.vertices[(length - vert_x * 6) + i - 2].y, vert[i].z));
						}

					}
					/* if (i %  == 0)
					{
						center.colors.Add(new Color(0, 255, 0));
						center.vertices.Add(draft.vertices[i]);
					}*/
					//center.vertices.Add(vert[i]);
					//center.triangles.Add(tri[i]);
					//center.normals.Add(norm[i]);
				}
			}
			//center.length.
			Debug.Log("connected");
		}

		public void ConnectMeshTwo(MeshDraft fromMesh, MeshDraft toMesh)
		{

			Debug.Log(toMesh.triangles.ToArray().Length);
			Debug.Log(toMesh.vertices.ToArray().Length);
			Debug.Log(toMesh.normals.ToArray().Length);
			int length = toMesh.normals.ToArray().Length;
			Vector3[] vert = fromMesh.vertices.ToArray();
			Vector3[] norm = fromMesh.normals.ToArray();
			int[] tri = fromMesh.triangles.ToArray();
			// center.colors.RemoveRange(0, length);
			fromMesh.vertices.RemoveRange(0, length);
			fromMesh.normals.RemoveRange(0, length);
			fromMesh.triangles.RemoveRange(0, length);

				for (int i = 0; i < length; i++)
			{
				if (i < vert_x * 6 * (vert_z - 1) )
				{
					//    center.colors.Add(new Color(0, 255, 0));
					fromMesh.vertices.Add(vert[i]);
					fromMesh.triangles.Add(tri[i]);
					fromMesh.normals.Add(norm[i]);
				}
				else
				{

					// center.colors.Add(new Color(255, 0, 0));
					// center.vertices.Add(draft.vertices[i]);
					fromMesh.triangles.Add(toMesh.triangles[i]);
					fromMesh.normals.Add(toMesh.normals[i]);
					if (i % 6 < 3)
					{   //0, 1, 2, 6=0

						if (i % 6 == 0)
						{   //0, 6=0
							//center.colors.Add(new Color(0, 0, 255));
							fromMesh.vertices.Add(new Vector3(vert[i].x, toMesh.vertices[(length - vert_x * 6 * (vert_z)) + i + 1].y, vert[i].z));
						}
						else
						{   //1, 2
							//        center.colors.Add(new Color(255, 0, 0));
							fromMesh.vertices.Add(vert[i]);
						}
					}
					else
					{   //3, 4, 5
						if (i % 6 == 4)
						{   //4
							//center.colors.Add(new Color(255, 0, 0));
							fromMesh.vertices.Add(vert[i]);
						}
						else if( i % 6 == 5)
						{   //5
							//center.colors.Add(new Color(0, 255, 0));
							fromMesh.vertices.Add(new Vector3(vert[i].x, toMesh.vertices[(length - vert_x * 6 * (vert_z)) + i - 1].y, vert[i].z));
						}
						else if( i % 6 == 3)
						{   //3
							fromMesh.vertices.Add(new Vector3(vert[i].x, toMesh.vertices[(length - vert_x * 6 * (vert_z)) + i - 2].y, vert[i].z));
						}

					}
					/* if (i %  == 0)
					{
						center.colors.Add(new Color(0, 255, 0));
						center.vertices.Add(draft.vertices[i]);
					}*/
					//center.vertices.Add(vert[i]);
					//center.triangles.Add(tri[i]);
					//center.normals.Add(norm[i]);
				}
			}
			//center.length.
			Debug.Log("connected");
		}

		public void MorphFitDown(MeshDraft fromMesh, MeshDraft toMesh)
		{

			Debug.Log(toMesh.triangles.ToArray().Length);
			Debug.Log(toMesh.vertices.ToArray().Length);
			Debug.Log(toMesh.normals.ToArray().Length);
			int length = toMesh.normals.ToArray().Length;
			Vector3[] vert = fromMesh.vertices.ToArray();
			Vector3[] norm = fromMesh.normals.ToArray();
			int[] tri = fromMesh.triangles.ToArray();
			// center.colors.RemoveRange(0, length);
			fromMesh.vertices.RemoveRange(0, length);
			fromMesh.normals.RemoveRange(0, length);
			fromMesh.triangles.RemoveRange(0, length);

			for (int i = 0; i < length; i++)
			{
				if (i % (vert_x - 6) == 0)
				{
					//    center.colors.Add(new Color(0, 255, 0));
					fromMesh.vertices.Add(vert[i]);
					fromMesh.triangles.Add(tri[i]);
					fromMesh.normals.Add(norm[i]);
				}
				else
				{

					// center.colors.Add(new Color(255, 0, 0));
					// center.vertices.Add(draft.vertices[i]);
					fromMesh.triangles.Add(toMesh.triangles[i]);
					fromMesh.normals.Add(toMesh.normals[i]);
					if (i % 6 < 3)
					{   //0, 1, 2, 6=0

						if (i % 6 == 0)
						{   //0, 6=0
							//center.colors.Add(new Color(0, 0, 255));
							fromMesh.vertices.Add(new Vector3(vert[i].x, toMesh.vertices[(length - vert_x * 6) + i + 1].y, vert[i].z));
						}
						else
						{   //1, 2
							//        center.colors.Add(new Color(255, 0, 0));
							fromMesh.vertices.Add(vert[i]);
						}
					}
					else
					{   //3, 4, 5
						if (i % 6 == 4)
						{   //4
							//center.colors.Add(new Color(255, 0, 0));
							fromMesh.vertices.Add(vert[i]);
						}
						else if (i % 6 == 5)
						{   //5
							//center.colors.Add(new Color(0, 255, 0));
							fromMesh.vertices.Add(new Vector3(vert[i].x, toMesh.vertices[(length - vert_x * 6) + i - 1].y, vert[i].z));
						}
						else if (i % 6 == 3)
						{   //3
							fromMesh.vertices.Add(new Vector3(vert[i].x, toMesh.vertices[(length - vert_x * 6) + i - 2].y, vert[i].z));
						}

					}
					//center.length.
					Debug.Log("connected");

				}
			}
		}

		public void UpdateVerticies(GameObject terrain, int idx)
		{
			terrain.GetComponent<MeshFilter>().mesh = meshes[idx];
			terrain.GetComponents<MeshCollider>()[0].sharedMesh = meshes[idx];
			terrain.GetComponents<MeshCollider>()[1].sharedMesh = meshes[idx];
			//Debug.Log("update" + idx);
		}
	}
		
}