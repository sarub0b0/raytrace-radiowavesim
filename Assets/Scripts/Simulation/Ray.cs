using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Simulation
{
    public class Ray : MonoBehaviour
    {
        private float distance;
        private LineRenderer lr;
        private RaycastHit hitData;

        // Use this for initialization
        void Start ()
        {
            lr = GetComponent<LineRenderer> ();
        }
    
        // Update is called once per frame
        void Update ()
        {
            RayCast ();

        }

        public void RayCast ()
        {
            if (Physics.Raycast (transform.position, transform.forward, out hitData)) {
                lr.SetPosition (1, new Vector3 (0, 0, hitData.distance));
                distance = hitData.distance;
            } else {
                lr.SetPosition (1, new Vector3 (0, 0, 1000));
                distance = 1000;
            }

        }

        public float Distance{ get { return this.distance; } }

        public void SetLr(double d,Vector3 p, Vector3 v){
            lr = GetComponent<LineRenderer> ();
            lr.SetPosition (1, new Vector3 (0, 0, (float)d));
            lr.transform.position = p;
            lr.transform.forward = v;
        } 

        public void SetLr(){
            lr = GetComponent<LineRenderer> ();
        }
        public RaycastHit HitData{ get { return this.hitData; } }
    }
}
