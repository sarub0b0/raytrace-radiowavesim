using UnityEngine;
using System.Collections;
using System.Threading;

namespace Assets.Scripts.Simulation
{
    public class TraceButton : MonoBehaviour
    {
        public GameObject Transmitter;
        private static bool traceButton;
        private static bool resetButton;
        private MainClass mainClass;

        // Use this for initialization
        void Start ()
        {
            mainClass = Transmitter.GetComponent<MainClass> ();
        }
    
        // Update is called once per frame
        void Update ()
        {
    
        }

        void OnGUI ()
        {
            //Raytrace Button
            traceButton = GUI.Button (new Rect (2, 2, 75, 25), "RayTrace");
            if (traceButton) {
                Debug.Log ("RayTrace Start");

                mainClass.RaySet ();
            } 

            resetButton = GUI.Button (new Rect (78, 2, 75, 25), "Reset");
            if (resetButton) {
                Debug.Log ("RayTrace Reset");

                mainClass.RayReset ();
                
            } 
        }

        public static bool TraceBool{ get { return traceButton; } }
        public static bool ResetBool{ get { return resetButton; } }
    }
}
