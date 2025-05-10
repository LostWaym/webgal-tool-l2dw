using UnityEditor;
using UnityEngine;
using System.Collections;


public class CreateCanvas : MonoBehaviour
{

	[MenuItem("Live2D/Create Live2D Canvas")]
	static void Create()
	{
		GameObject goLive2DCanvas = new GameObject("Live2D_Canvas") ;
		
		MeshRenderer meshRenderer = goLive2DCanvas.AddComponent<MeshRenderer>() ;
		meshRenderer.sharedMaterial = new Material(Shader.Find("Transparent/Diffuse")) ;
		meshRenderer.sharedMaterial.SetColor("_Color", new Color(0,0,0,0));
		
		MeshFilter meshFilter = goLive2DCanvas.AddComponent<MeshFilter>() ;
		
		meshFilter.mesh = new Mesh() ;
		Mesh mesh = meshFilter.sharedMesh ;
		mesh.name = "Live2D_Canvas" ;
		
		mesh.vertices = new Vector3[]
		{
			new Vector3 (-10f, 0.0f,  10f),
			new Vector3 ( 10f, 0.0f,  10f),
			new Vector3 ( 10f, 0.0f, -10f),
			new Vector3 (-10f, 0.0f, -10f)
		} ;
		mesh.triangles = new int[]
		{
			0, 1, 2,
			2, 3, 0
		} ;
		mesh.uv = new Vector2[]
		{
			new Vector2 (0.0f, 1.0f),
			new Vector2 (1.0f, 1.0f),
			new Vector2 (1.0f, 0.0f),
			new Vector2 (0.0f, 0.0f)
		} ;
		
		mesh.RecalculateNormals() ;	
		mesh.RecalculateBounds() ;	
		 ;

		if (!System.IO.File.Exists("Assets/Resources/Live2D_Canvas/Live2D_Canvas.asset"))
		{
			System.IO.Directory.CreateDirectory("Assets/Resources/Live2D_Canvas");
			AssetDatabase.CreateAsset(mesh, "Assets/Resources/Live2D_Canvas/" + mesh.name + ".asset") ;
			AssetDatabase.SaveAssets() ;
		}

		//goLive2DCanvas.AddComponent<LAppModelProxy>(); 
		goLive2DCanvas.AddComponent<MeshCollider>() ; 
		goLive2DCanvas.GetComponent<Transform>().Rotate(-90,0,0) ; 
		goLive2DCanvas.AddComponent<AudioSource>();
	}
}