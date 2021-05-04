using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Assets.Scripts.Simulation
{
    public class MainClass : MonoBehaviour
    {
        public const int RAYCACHE = 5;
        public GameObject Ray0;
        public GameObject Text;
        public GameObject Receiver;
        private RayTrace rt;
        private static RadioProperty rp;
        private FieldIntensity fi;
        private Receiver rx;
        private Transmitter tx;
		private System.TimeSpan tracetime;

        //
        private static Ray[] rayScript;
        private static GameObject[] texts;
        private static GameObject[] rays;

        // Label Style 
        private GUIStyle labelStyle;
        private GUIStyleState labelStyleState;

        void Awake()
        {
            rayScript = new Ray[RAYCACHE];
            texts = new GameObject[RAYCACHE];
            rays = new GameObject[RAYCACHE];

            for (int i = 0; i < RAYCACHE; i++)
            {

                rays[i] = Object.Instantiate(GameObject.Find("Ray")) as GameObject;
                texts[i] = Object.Instantiate(GameObject.Find("Distance")) as GameObject;

                if (i == 0)
                {
                    rays[i].SafeSetParent(this);
                    texts[i].SafeSetParent(rays[i]);
                }
                else {
                    rays[i].SafeSetParent(rays[i - 1]);
                    texts[i].SafeSetParent(rays[i]);
                }

                rays[i].AddComponent<Ray>();
                rays[i].name = "Ray" + i;

                texts[i].AddComponent<TextTransform>();
                texts[i].name = "Distance" + i;
                if (i != 0)
                {
                    rays[i].SetActive(false);
                    texts[i].SetActive(false);
                }
            }
        }

        private GameObject[] trans = new GameObject[1800];
        // Use this for initialization
        void Start()
        {
            labelStyle = new GUIStyle();
            labelStyle.fontSize = 14;
            labelStyle.fixedWidth = 100;
            labelStyle.alignment = TextAnchor.MiddleLeft;

            labelStyleState = new GUIStyleState();
			labelStyleState.textColor = Color.black;   // 文字色の変更.
            labelStyle.normal = labelStyleState;

            for (int i = 0; i < RAYCACHE; i++)
            {
                rayScript[i] = rays[i].GetComponent<Ray>();
                rayScript[i].SetLr();
            }

            rt = new RayTrace();
            rp = GetComponent<RadioProperty>();

            Ray0.SetActive(false);
            Text.SetActive(false);
	
        }

        // Update is called once per frame
        void Update()
        {
            rt.Trace(RAYCACHE, rays, rayScript, texts);
        }

        public void RaySet()
        {
            GetComponent<Renderer>().enabled = false;
            RayReset();
            findReceiver();
            GetComponent<Renderer>().enabled = true;
        }

        public void RayReset()
        {
            GameObject trace = GameObject.Find("trace");
            if (trace != null)
            {
                foreach (Transform child in trace.transform)
                {
                    GameObject.Destroy(child.gameObject);
                }
            }
            rt.ListClear();
            rp.ListClear();
            Controls.X = 0;
            Controls.Y = 0;
            rt.HitRx = false;
            texts[0].SetActive(true);
            rays[0].SetActive(true);
        }

        private float x = 0.0F, y = 0.0F, z = 0.0F;
        float divDeg;

        private void findReceiver()
        {
            float div = divDeg;
            float scale = 1 / div;

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            Quaternion eulerTest;
            for (x = -20; x <= 200; x += div)
            {
                for (y = 0; y <= 180; y += div)
                {
                    eulerTest = Quaternion.Euler(x, y, z);
                    transform.rotation = eulerTest;

                    rt.Trace(RAYCACHE, rays, rayScript, texts);
                }
            }
            sw.Stop();
			tracetime = sw.Elapsed;

            if (rt.HitRx)
            {
                rp.repetValueRemove(rt.DataList);

                if (RadioProperty.DataList.Count == 1)
                {
                    if (2 <= RadioProperty.DataList[0].HitNum)
                    {
                        double d = (double)Mathf.Sqrt(
                                                    Mathf.Pow(Transmitter.Vector.x - Assets.Scripts.Simulation.Receiver.Vector.x, 2)
                                                    + Mathf.Pow(Transmitter.Vector.y - Assets.Scripts.Simulation.Receiver.Vector.y, 2));

                        RadioProperty.DataList.Add(new TraceData
                        {
                            LookVector = new List<Vector3> { Assets.Scripts.Simulation.Receiver.Vector - Transmitter.Vector },
                            LookPoint = new List<Vector3> { Transmitter.Vector },
                            HitObject = new List<string> { "Receiver" },
                            Distance = new List<double> { d },

                            SumDistance = d,
                            HitPoint = new List<Vector3> { Assets.Scripts.Simulation.Receiver.Vector },
                            Angle = new List<double> { 0 },
                            HitNum = 1,
                            HitRx = true
                        }
                          );
                    }
                }
                if (RadioMap.Button == false)
                {
                    showResult(RadioProperty.DataList);
                }
                FieldIntensity fi = new FieldIntensity(RadioProperty.DataList, FICalculation.Gain, FICalculation.Frequency, FICalculation.Power);
                rp.SetFI(FieldIntensity.TotalEdb);
            }
            else {
                rp.SetFI(0);
            }
        }

        private Quaternion[] getEulerArray(float min, float max, float div, float scale)
        {
            Quaternion[] euler = new Quaternion[int.MaxValue - 1];
            int n = 0;

            for (x = min; x < max; x += div)
            {
                for (y = min; y < max; y += div)
                {
                    euler[n] = Quaternion.Euler(x, y, z);
                    n++;
                }
            }

            return euler;
        }

        private void showResult(List<TraceData> dataList)
        {

            for (int i = 0; i < RAYCACHE; i++)
            {
                texts[i].SetActive(false);
                rays[i].SetActive(false);
            }

            Ray0.SetActive(true);
            GameObject parent = GameObject.Find("trace");
            foreach (TraceData d in dataList)
            {
                for (int i = 0; i < d.HitObject.Count; i++)
                {
                    GameObject r = Object.Instantiate(GameObject.Find("Ray")) as GameObject;
                    r.SafeSetParent(parent);
                    r.AddComponent<Ray>();
                    Ray ray = r.GetComponent<Ray>();
                    ray.SetLr(d.Distance[i], d.LookPoint[i], d.LookVector[i]);
                }
            }
            Ray0.SetActive(false);
        }

        string dText = "1";
        //Show Raycast Params
        void OnGUI()
        {
            GUILayout.Label("", labelStyle);
            GUILayout.Label("", labelStyle);
            ShowLabelAndValue(" HitObject: ", rt.HitObject, labelStyle);
            ShowLabelAndValue(" HitNum: ", rt.HitNum.ToString(), labelStyle);
            ShowLabelAndValue(" FI: ", FieldIntensity.TotalEdb.ToString(), labelStyle);
			ShowLabelAndValue (" Trace Time: ", tracetime.ToString(), labelStyle);
            GUILayout.Label("", labelStyle);
            GUI.Label(new Rect(52, 380, 22, 20), " [deg]", labelStyle);

            dText = GUI.TextField(new Rect(2, 380, 50, 20), dText, 5);
            if (Validation.IsNumeric(dText))
            {
                divDeg = float.Parse(dText);
                dText = divDeg.ToString();
            }
            else {
                dText = "0";
            }
        }

        public static void ShowLabelAndValue(string label, string value, GUIStyle style)
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(label, style);
                GUILayout.Label(value, style);
            }
            GUILayout.EndHorizontal();
        }
    }

}