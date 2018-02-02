using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Simulation
{
	public class FICalculation : MonoBehaviour
	{

		private static bool fiButton;
		private FieldIntensity fi;
			
		private static float g,f,p;
		private GUIStyle labelStyle, labelStyleBox;
		private GUIStyleState labelStyleState;
		private string gText="2.14",fText="300",pText="10";
		// Use this for initialization
		void Start ()
		{

			labelStyle = new GUIStyle ();
			labelStyle.fontSize = 14;
			labelStyle.fixedWidth = 70;
			labelStyle.alignment = TextAnchor.MiddleLeft;
			
			labelStyleBox = new GUIStyle (labelStyle);
			labelStyleBox.alignment = TextAnchor.MiddleRight;
			
			labelStyleState = new GUIStyleState ();
			labelStyleState.textColor = Color.black;   // 文字色の変更.
            labelStyle.normal = labelStyleState;
            labelStyleBox.normal = labelStyleState;
		}
			
		// Update is called once per frame
		void Update ()
		{

		}
			
		void OnGUI ()
		{
			fiButton = GUI.Button (new Rect (155, 2, 100, 25), "Field Intencity");
			if (fiButton) {
				Debug.Log ("Find Field Intencity");

				fi = new FieldIntensity (RadioProperty.DataList, g, f, p);

			} 

			GUI.Label (new Rect (2, 300, 22, 20), " Radio", labelStyle);
			GUI.Label (new Rect (52, 320, 22, 20), " [dBi]", labelStyle);
			GUI.Label (new Rect (52, 340, 22, 20), " [MHz]", labelStyle);
			GUI.Label (new Rect (52, 360, 22, 20), " [mW]", labelStyle);
			gText = GUI.TextField (new Rect (2, 320, 50, 20), gText, 5);
			fText = GUI.TextField (new Rect (2, 340, 50, 20), fText, 5);
			pText = GUI.TextField (new Rect (2, 360, 50, 20), pText, 5);

			if (Validation.IsNumeric (gText)
			    && Validation.IsNumeric (fText)
			    && Validation.IsNumeric (pText)) 
			{
				g = float.Parse (gText);
				f= float.Parse (fText);
				p= float.Parse (pText);
				gText = g.ToString ();
				fText = f.ToString ();
				pText = p.ToString ();
			} else {
				gText = "0";
				fText = "0";
				pText = "0";
			}
		}
		public static float Gain{get{return g;}}
		public static float Frequency{get{return f;}}
		public static float Power{get{return p;}}

	}
}
