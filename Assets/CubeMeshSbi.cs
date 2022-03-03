using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class CubeMeshSbi : MonoBehaviour
{
	private Vector3 firstPoint, lastPoint;
	public Vector3[] normals = new Vector3[24];
	int layerMask;

	private void Start()
	{
		layerMask = LayerMask.GetMask("projectile");
	}
	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			var Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(Ray, out hit, layerMask))
			{
				if (hit.transform.CompareTag("plane"))
				{
					firstPoint = lastPoint = hit.point;
					transform.GetComponent<Collider>().enabled = true;
				}
			}
		}

		if (Input.GetMouseButton(0))
		{
			var Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(Ray, out hit,layerMask))
			{
				if (hit.transform.CompareTag("plane"))
				{
					lastPoint = hit.point;
					CreateCubeSbi2();
				}

				//lastPoint = hit.point;
				//CreateCubeSbi();

			}
		}
	}

	private void CreateCubeSbi()
	{

		Mesh mesh = GetComponent<MeshFilter>().mesh;
		mesh.Clear(); 

		Vector3[] vertices = {
			new Vector3 (.3f, -2, .5f) + new Vector3 (firstPoint.x,0,firstPoint.z),//s0
			new Vector3 (-.3f, -2, .5f)+ new Vector3 (firstPoint.x,0,firstPoint.z),//s1
			new Vector3 (-.3f, 2, .5f)+ new Vector3 (firstPoint.x,0,firstPoint.z),//s2
			new Vector3 (.3f, 2, .5f)+ new Vector3 (firstPoint.x,0,firstPoint.z),//s3
			new Vector3 (.3f, 2, -.5f)+ new Vector3(lastPoint.x, 0, lastPoint.z),//f4
			new Vector3 (-.3f, 2, -.5f)+ new Vector3(lastPoint.x, 0, lastPoint.z),//f5
			new Vector3 (-.3f, -2, -.5f)+ new Vector3(lastPoint.x, 0, lastPoint.z),//f6
			new Vector3 (.3f, -2, -.5f)+ new Vector3(lastPoint.x, 0, lastPoint.z),//f7
			};


		if (lastPoint.z > firstPoint.z)
		{
			vertices[1] = new Vector3(.3f, -2, .5f) + new Vector3(lastPoint.x, 0, lastPoint.z); ;//s0
			vertices[1] = new Vector3(-.3f, -2, .5f) + new Vector3(lastPoint.x, 0, lastPoint.z); ;//s1
			vertices[1] = new Vector3(-.3f, 2, .5f) + new Vector3(lastPoint.x, 0, lastPoint.z); ;//s2
			vertices[1] = new Vector3(.3f, 2, .5f) + new Vector3(lastPoint.x, 0, lastPoint.z);//s3
			vertices[1] = new Vector3(.3f, 2, -.5f) + new Vector3(firstPoint.x, 0, firstPoint.z);//f4
			vertices[1] = new Vector3(-.3f, 2, -.5f) + new Vector3(firstPoint.x, 0, firstPoint.z);//f5
			vertices[1] = new Vector3(-.3f, -2, -.5f) + new Vector3(firstPoint.x, 0, firstPoint.z);//f6
			vertices[1] = new Vector3(.3f, -2, -.5f) + new Vector3(firstPoint.x, 0, firstPoint.z);//f7
			
		}

		int[] triangles = {
			0, 2, 1, //face front
			0, 3, 2,
			2, 3, 4, //face top
			2, 4, 5,
			1, 2, 5, //face right
			1, 5, 6,
			0, 7, 4, //face left
			0, 4, 3,
			5, 4, 7, //face back
			5, 7, 6,
			0, 6, 7, //face bottom
			0, 1, 6
		};

		
		mesh.vertices = vertices;
		mesh.SetNormals(normals);
		mesh.triangles = triangles;
		mesh.Optimize();
		//mesh.RecalculateNormals();
		transform.GetComponent<MeshCollider>().sharedMesh = mesh;
	}

	private void CreateCubeSbi2()
	{
		if(lastPoint.z <= firstPoint.z)
		{
			Mesh mesh = GetComponent<MeshFilter>().mesh;
			mesh.Clear();

			Vector3[] vertices = {
			new Vector3 (.3f, -2, .5f) + firstPoint,//s0
			new Vector3 (-.3f, -2, .5f)+ firstPoint,//s1
			new Vector3 (-.3f, 2, .5f)+ firstPoint,//s2
			new Vector3 (.3f, 2, .5f)+ firstPoint,//s3
			new Vector3 (.3f, 2, -.5f)+ lastPoint,//f4
			new Vector3 (-.3f, 2, -.5f)+ lastPoint,//f5
			new Vector3 (-.3f, -2, -.5f)+ lastPoint,//f6
			new Vector3 (.3f, -2, -.5f)+ lastPoint,//f7
			};

			int[] triangles = {
			0, 2, 1, //face front
			0, 3, 2,
			2, 3, 4, //face top
			2, 4, 5,
			1, 2, 5, //face right
			1, 5, 6,
			0, 7, 4, //face left
			0, 4, 3,
			5, 4, 7, //face back
			5, 7, 6,
			0, 6, 7, //face bottom
			0, 1, 6
			};


			mesh.vertices = vertices;
			//mesh.SetNormals(normals);
			mesh.triangles = triangles;
			mesh.Optimize();
			mesh.RecalculateNormals();
			transform.GetComponent<MeshCollider>().sharedMesh = mesh;
		}
		else
		{
			Mesh mesh = GetComponent<MeshFilter>().mesh;
			mesh.Clear();

			Vector3[] vertices = {
			new Vector3 (.3f, -2, .5f) + firstPoint,//s0
			new Vector3 (-.3f, -2, .5f)+ firstPoint,//s1
			new Vector3 (-.3f, 2, .5f)+ firstPoint,//s2
			new Vector3 (.3f, 2, .5f)+ firstPoint,//s3
			new Vector3 (.3f, 2, -.5f)+ lastPoint,//f4
			new Vector3 (-.3f, 2, -.5f)+ lastPoint,//f5
			new Vector3 (-.3f, -2, -.5f)+ lastPoint,//f6
			new Vector3 (.3f, -2, -.5f)+ lastPoint,//f7
			};

			int[] triangles = {
			0, 1, 2, //face front
			0, 2, 3,
			2, 4, 3, //face top
			2, 5, 4,
			1, 5, 2, //face right
			1, 6, 5,
			0, 4, 7, //face left
			0, 3, 4,
			5, 7, 4, //face back
			5, 6, 7,
			0, 7, 6, //face bottom
			0, 6, 1
			};


			mesh.vertices = vertices;
			//mesh.SetNormals(normals);
			mesh.triangles = triangles;
			mesh.Optimize();
			mesh.RecalculateNormals();
			transform.GetComponent<MeshCollider>().sharedMesh = mesh;
		}		
	}
}
