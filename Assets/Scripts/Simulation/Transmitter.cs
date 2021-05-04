using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Simulation
{
	public class Transmitter : MonoBehaviour
	{
		private static Vector3 vector;
		private GetHeightTx gh;
		private GameObject t1;

		// Use this for initialization
		void Start ()
		{
			t1 = GameObject.Find ("T1");
			gh = t1.GetComponent<GetHeightTx> ();
		}
    
		// Update is called once per frame
		void Update ()
		{
			transform.position = new Vector3 (gh.Vector.x, gh.Vector.y + gh.Y, gh.Vector.z);
			vector = transform.position;
		}

		public static Vector3 Vector{ get { return vector; } }
	}
}
