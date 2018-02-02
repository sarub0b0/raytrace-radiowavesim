using UnityEngine;
using System.Collections;
using System;

namespace Assets.Scripts.Simulation
{
	public class CreateGround : MonoBehaviour
	{
		private GameObject[] children;
		private GameObject tile;
		private GameObject terrain;

		// Use this for initialization
		void Start ()
		{
			//StartCoroutine(Download(this.Url, tex => addSplatPrototype(tex)));
		}
    
		// Update is called once per frame
		void Update ()
		{

			tile = GameObject.Find ("tile");
			if (tile != null) {
				GameObject.Destroy (tile);
			}
		}
	}
}