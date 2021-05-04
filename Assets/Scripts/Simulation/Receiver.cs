using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Simulation
{
	public class Receiver : MonoBehaviour
	{
		private static Vector3 vector;
		private GameObject r1;
		private GetHeightRx gh;

		// Use this for initialization
		void Start ()
		{
			r1 = GameObject.Find ("R1");
			gh = r1.GetComponent<GetHeightRx> ();
		}
		
		// Update is called once per frame
		void Update ()
		{
			transform.position = new Vector3 (gh.Vector.x, gh.Vector.y + gh.Y, gh.Vector.z);
			vector = transform.position;
		}

		public void SetPosition(float x,float z){
			transform.position = new Vector3(x,200,z);
			transform.position = new Vector3(x, gh.GetHeight() + gh.Y, z);
            vector = transform.position;
		}
		public static Vector3 Vector{ get { return vector; } }
	}
}
