using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Simulation
{
	public class GetHeightRx: MonoBehaviour
	{
		private Vector3 vector;
		private RaycastHit hitData;
		private GUIStyle labelStyle, labelStyleBox;
		private GUIStyleState labelStyleState;
		private  float x, y, z;
		private static float height;
		private string xText = "10", yText = "5", zText = "10";
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
			if (Physics.Raycast (transform.position, transform.forward, out hitData)) {
				if (hitData.transform.tag != "Player") {
					height = hitData.point.y;
				}
			} else {
				height = 200;
			}

			vector = new Vector3 (x, height, z);
		}

        public float GetHeight()
        {
            if (Physics.Raycast(transform.position, transform.forward, out hitData)) {
                if (hitData.transform.tag != "Player") {
                    height = hitData.point.y;
                }
            } else {
                height = 200;
            }
            vector = transform.position;

            return height;
		}
		 
		void OnGUI ()
		{
			GUI.Label (new Rect (2, 100, 22, 20), " RxPoint", labelStyle);
			GUI.Label (new Rect (52, 120, 22, 20), " x", labelStyle);
			GUI.Label (new Rect (52, 140, 22, 20), " y", labelStyle);
			GUI.Label (new Rect (52, 160, 22, 20), " z", labelStyle);
			xText = GUI.TextField (new Rect (2, 120, 50, 20), xText, 5);
			yText = GUI.TextField (new Rect (2, 140, 50, 20), yText, 5);
			zText = GUI.TextField (new Rect (2, 160, 50, 20), zText, 5);

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

		public void SetPoint (int x1, int x2)
		{
			xText = x1.ToString ();
			zText = x2.ToString ();
		}

		public Vector3 Vector { get { return vector; } }

		public float Y{ get { return y; } }

		public static float Height{ get { return height; } }
	}
}