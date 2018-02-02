using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Simulation
{
	public class Controls : MonoBehaviour
	{
		public int Speed;
		public float Friction;
		public float LerpSpeed;
		private static float xDeg = 0;
		private static float yDeg = 0;
		private Quaternion fromRotation;
		private Quaternion toRotation;

		void Update ()
		{
			if (TraceButton.TraceBool == false) {
				xDeg += Input.GetAxis ("Horizontal") * Speed * Friction;
				yDeg += Input.GetAxis ("Vertical") * Speed * Friction;

				fromRotation = transform.rotation;
				toRotation = Quaternion.Euler (yDeg, xDeg, 0);

				transform.rotation = Quaternion.Lerp (fromRotation, toRotation, Time.deltaTime * LerpSpeed);
			}
		}

		public static float X { 
			get { return xDeg; } 
			set { xDeg = value;}
		}

		public static float Y { 
			get { return yDeg; } 
			set { yDeg = value;}
		}
	}
}