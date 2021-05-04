using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.Simulation
{
    public class RayTrace
    {
        private bool hitRx = false;
        private string hitObj;
        private float distance;
        private Vector3 hitPoint;
        private Vector3 startPoint;
        private Vector3 startVec;
        private Vector3 normalVec;
        private Vector3 reflecVec;
        private int hitNum;
        private double sumDistance;
        private List<TraceData> dataList = new List<TraceData>();

        public RayTrace() { }

        public void Trace(int raycache, GameObject[] rays, Ray[] rayScript, GameObject[] texts)
        {
            TraceData data = new TraceData();
            hitNum = 0;
            float distance;
            startPoint = rays[0].transform.position;
            startVec = rays[0].transform.forward;

            for (int i = 0; i < raycache - 1; i++)
            {
                rayScript[i].RayCast();
                if (rayScript[i].HitData.collider)
                {
                    hitNum = i + 1;
                    rays[hitNum].SetActive(true);
                    texts[hitNum].SetActive(true);

                    hitObj = rayScript[i].HitData.collider.tag;
                    hitPoint = rayScript[i].HitData.point;
                    distance = rayScript[i].HitData.distance;

                    //Set Data
                    data.SetLookVec(startVec);
                    data.SetLookPoint(startPoint);

                    if (hitObj == "Receiver")
                    {
                        distance = Mathf.Sqrt(
                            Mathf.Pow(startPoint.x - Receiver.Vector.x, 2)
                            + Mathf.Pow(startPoint.y - Receiver.Vector.y, 2));

						data.SetDistance(distance);						
						data.SetHitPoint(Receiver.Vector);
						data.SetHitObj(hitObj);
						data.HitNum = hitNum;
						data.SetAngle(0);

						hitRx = true;
						data.HitRx = true;
						for (int j = hitNum; j < raycache; j++)
						{
							rays[j].SetActive(false);
							texts[j].SetActive(false);
						}
						
						dataList.Add(data);
						break;
                    }

                    data.SetDistance(distance);

                    data.SetHitPoint(rayScript[i].HitData.point);
                    data.SetHitObj(rayScript[i].HitData.collider.tag);
                    data.HitNum = hitNum;

                    normalVec = rayScript[i].HitData.normal.normalized;
                    reflecVec = Vector3.Reflect(startVec.normalized, normalVec);

                    data.SetAngle(startVec, reflecVec);

                    startVec = reflecVec;
                    startPoint = hitPoint;

                    rays[hitNum].transform.position = hitPoint;
                    rays[hitNum].transform.forward = startVec;                   
                }
                else {
                    for (int j = hitNum + 1; j < raycache; j++)
                    {
                        rays[j].SetActive(false);
                        texts[j].SetActive(false);
                    }
                    break;
                }
            }

        }

        public bool HitRx
        {
            get { return this.hitRx; }
            set { this.hitRx = value; }
        }

        public int HitNum { get { return this.hitNum; } }

        public string HitObject { get { return this.hitObj; } }

        public List<TraceData> DataList { get { return this.dataList; } }

        public void ListClear()
        {

            dataList.Clear();
        }
    }
}