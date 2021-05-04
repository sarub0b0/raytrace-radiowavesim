using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

//要するに今回は三角形を作っていく
namespace Assets.Scripts.Simulation
{
	public class CreateMesh : MonoBehaviour
	{
		//メッシュを作成
		private Mesh mesh;

		//頂点（作る図形は三角形だから３つ必要）
		private List<Vector3> heightList = new List<Vector3> ();
		private Vector3[] heights = new Vector3[]{};

		//頂点インデックス(作られるポリゴン数＊3)
		private List<int> triangleList = new List<int> ();
		private int[] triangles = new int[]{};

		void Start(){
			CreateMesh cm = GetComponent<CreateMesh> ();
			cm.CreateMeshData ();
		}
        // Use this for initialization
        // public void CreateMeshData (List<float> data)
		public void CreateMeshData()
        {

			System.IO.StreamReader elist = new System.IO.StreamReader (
				@"Assets/Scripts/Simulation/elevations.list", System.Text.Encoding.UTF8);

			string line = "";
			List<float> elev = new List<float> ();

			while ((line = elist.ReadLine ()) != null) {
				elev.Add (float.Parse(line));
			}

			mesh = new Mesh ();
//			terrain = GameObject.Find ("terrain");       
//			terrain.SetActive (false);

			float height;
			int w = Consts.Offset;
			int divWidth = 10;   
			int index = 0;

			//左上から右下にメッシュ作成
			//(-x , z) →　(x , -z) 
			for (int z=w; z>=-w; z-=divWidth) {
				for (int x=-w; x<=w; x+=divWidth) {
					height = elev[index];
                    heightList.Add(new Vector3(x, height, z));
                    //heightList.Add(new Vector3(x, 0, z));
                    index++;
				}
			}

			int parDiv = (int)Mathf.Sqrt (heightList.Count);
			int endCheck;

			for (int i =0; i<heightList.Count-parDiv-1; i++) {
				endCheck = (i + 1) % parDiv;
				if (endCheck != 0) {
					triangleList.Add (i);
					triangleList.Add (i + parDiv + 1);
					triangleList.Add (i + parDiv);
					triangleList.Add (i);
					triangleList.Add (i + 1);
					triangleList.Add (i + parDiv + 1);
				}
			}
			heights = heightList.ToArray ();
			triangles = triangleList.ToArray ();	

			//メッシュにわかりやすいよう名前をつける
			mesh.name = "MyMesh";

			//メッシュに頂点情報を設定
			mesh.vertices = heights;
        
			List<Color> colors = new List<Color> ();
			for (int i=0; i<heights.Length; i++) {
				colors.Add (Color.gray);
			}
			mesh.colors = colors.ToArray ();

			//メッシュに頂点インデックスを設定
			mesh.triangles = triangles;
        
			//法線、バウンディングボリュームを自動計算
			mesh.RecalculateBounds ();
			mesh.RecalculateNormals ();        

			//メッシュフィルター（描画）とメッシュコライダー（当たり判定）にメッシュを代入

			GameObject.Find ("Terrain").GetComponent<MeshFilter> ().mesh = mesh;

			GameObject.Find ("Terrain").GetComponent<MeshCollider> ().sharedMesh = mesh;
		}
    }
}                                                         
                                                                
