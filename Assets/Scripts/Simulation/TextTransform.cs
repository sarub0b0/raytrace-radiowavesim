using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Simulation
{
    public class TextTransform : MonoBehaviour
    {
        private GameObject obj;
        private Transform textTransform;
        private Transform cameraTransform;
        private TextMesh mesh;
        private Ray ray;
        private string rayNum;
        private Vector3 textPosition;

        // Use this for initialization
        void Start ()
        {
            rayNum = this.name.Substring (this.name.Length - 1);
            obj = GameObject.Find ("Ray" + rayNum);
            mesh = GetComponent<TextMesh> ();
            ray = obj.GetComponent<Ray> ();
        }
        
        // Update is called once per frame
        void Update ()
        {
            transform.forward = Camera.main.transform.forward;
            if (ray.HitData.collider) {
                textPosition = new Vector3 (0, 0, ray.HitData.distance / 3);
                //textPosition = new Vector3 (0, 0, Mathf.Abs (ray.HitData.point.z - ray.transform.position.z) / 2);
            } else {
                textPosition = new Vector3 (0, 0, 30);
            }

            transform.localPosition = textPosition;
        
//      transform.position = new Vector3 (ray.transform.position.x,
//                                             ray.transform.position.y,
//                                             ray.transform.position.z
//                                             );
            mesh.text = ray.Distance.ToString ();
        }
    }
}
