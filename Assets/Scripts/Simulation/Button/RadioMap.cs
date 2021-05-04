using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.Simulation
{
	public class RadioMap : MonoBehaviour
	{
		private GameObject transmitter;
		private GameObject terrain;
		private GameObject r;
		private DrawMapClass dmc;
		private Receiver rx;
		private static bool button;
		private MainClass mc;
		private RadioProperty rp;

		// Use this for initialization
		void Start ()
		{
            terrain = GameObject.Find("Terrain");
            transmitter = GameObject.Find ("Transmitter");
			r = GameObject.Find ("Receiver");
			dmc = terrain.GetComponent<DrawMapClass> ();
			rx = r.GetComponent<Receiver> ();
			mc = transmitter.GetComponent<MainClass> ();
			rp = mc.GetComponent<RadioProperty> ();
            
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		void OnGUI ()
		{
			button = GUI.Button (new Rect (256, 2, 100, 25), "Radio Map");
			if (button) {
				Debug.Log ("Create Radio Map");
				createRadioMap ();
			} 

		}

		private void createRadioMap ()
		{
			rp.FIClear ();
			List<double> test = new List<double> ();
			float divSize = ConstValue.ImageSize / ConstValue.Div;
			int maxSize = Consts.Offset;

			System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch ();
			sw.Start ();

			for (float z=maxSize-divSize/2; z>=-maxSize; z-=divSize) {
				for (float x=-maxSize+divSize/2; x<= maxSize; x+=divSize) {

                    if (150 < z) {
						rp.SetFI (0);
					} else {
						rx.SetPosition (x, z);
						mc.RaySet ();
					}
                    
				}
			}

			sw.Stop ();
			Debug.Log (sw.Elapsed);

			dmc.CreateData (RadioProperty.FI);

		}



		public static bool Button{ get { return button; } }
	}
}
