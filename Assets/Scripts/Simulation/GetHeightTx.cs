using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Simulation
{
	public class GetHeightTx: MonoBehaviour
	{
		private Vector3 vector;
		private Vector3 forward;
		private RaycastHit hitData;
		private GUIStyle labelStyle, labelStyleBox;
		private GUIStyleState labelStyleState;
		private  float x, y, z;
		private static float height;
		private string xText = "0", yText = "10", zText = "0";
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
			forward = new Vector3 (0, -1, 0);
		}
		
		// Update is called once per frame
		void Update ()
		{			
			if (Physics.Raycast (transform.position, forward, out hitData)) {
				if (hitData.transform.tag != "Receiver") {
					height = hitData.point.y;
				}
			} else {
				height = 50;
			}
			vector = new Vector3 (x, height, z);	
		}
		
		void OnGUI ()
		{
			GUI.Label (new Rect (2, 200, 22, 20), " TxPoint", labelStyle);
			GUI.Label (new Rect (52, 220, 22, 20), " x", labelStyle);
			GUI.Label (new Rect (52, 240, 22, 20), " y", labelStyle);
			GUI.Label (new Rect (52, 260, 22, 20), " z", labelStyle);
			xText = GUI.TextField (new Rect (2, 220, 50, 20), xText, 5);
			yText = GUI.TextField (new Rect (2, 240, 50, 20), yText, 5);
			zText = GUI.TextField (new Rect (2, 260, 50, 20), zText, 5);

			if (Validation.IsNumeric (xText) 
				&& Validation.IsNumeric (yText)
				&& Validation.IsNumeric (zText)) {
				x = float.Parse (xText);
				y = float.Parse (yText);
				z = float.Parse (zText);
				xText = x.ToString ();
				yText = y.ToString ();
				zText = z.ToString ();
			} else {
				xText = "0";
				yText = "0";
				zText = "0";
			}
		}
		
		public Vector3 Vector { get { return vector; } }

		public float Y{ get { return y; } }

		public static float Height{ get { return height; } }
	}
}