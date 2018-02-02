using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Threading;

namespace Assets.Scripts.Simulation
{
	public class RadioProperty:MonoBehaviour
	{
		private static double frequency;
		private static double txPower;
		private static double gain;
		private static double between;
		private static List<TraceData> dataList = new List<TraceData> ();
		private List<TraceData> dataListBuff = new List<TraceData> ();
		private static List<double> fi = new List<double> ();

		public void ResetParam ()
		{
			dataList.Clear ();
		}
		public void SetFI(double e){
			fi.Add(e);
		}

		//重複または値が近いものをまとめる
		public void repetValueRemove (List<TraceData> list)
		{
			dataList = new List<TraceData> (list);
			List<List<Vector3>> lookVecList = new List<List<Vector3>> ();
			List<List<Vector3>> lookPointList = new List<List<Vector3>> ();
			List<List<double>> distanceList = new List<List<double>> ();
			List<List<double>> angleList = new List<List<double>> ();
			List<string> objName0 = new List<string> ();
			int hitCount0;

			List<List<Vector3>> hPoint = new List<List<Vector3>> ();
			List<List<Vector3>> point = new List<List<Vector3>> ();
			List<List<Vector3>> vector = new List<List<Vector3>> ();
			List<List<double>> distance = new List<List<double>> ();
			List<List<double>> angle = new List<List<double>> ();

			string[] objBuff;
			int hitBuff;
			List<Vector3> hitPointBuff = new List<Vector3> ();

			int count;
			int index = 0;

			int indexObjNum = 0, indexRef = 0, indexHit = 0;

			removeDirectRay ();
			// distance sumaition

			removeHitRxAngle ();

			Stack stack1 = new Stack ();

			//同経路のレイ探索
			while (0<dataList.Count) {
				count = dataList.Count;
                            
				point.Clear ();
				vector.Clear ();
				distance.Clear ();
				angle.Clear ();
				hPoint.Clear();

				//*************************************************************
				//平均化用
				point.Add (dataList [0].LookPoint);
				vector.Add (dataList [0].LookVector);
				distance.Add (dataList [0].Distance);
				angle.Add (dataList [0].Angle);
				hPoint.Add(dataList[0].HitPoint);

				//比較
				hitBuff = dataList [0].HitNum;
				objBuff = dataList [0].HitObject.ToArray ();
				hitPointBuff = dataList [0].HitPoint;
				//*************************************************************

				//0番目の要素取得
				objName0 = dataList [0].HitObject;
				hitCount0 = dataList [0].HitNum;

				// 同経路のレイ検索
				for (int i=1; i<count; i++) {
					// 反射数
					if (dataList [i].HitNum == hitBuff) {
						indexRef = i;
					} else {
						indexRef = -1;
					}
					// 反射物
					if (objBuff.SequenceEqual (dataList [i].HitObject.ToArray ())) {
						indexObjNum = i;
                        
					} else {
						indexObjNum = -1;
					}
					// 交点
					if (nearPoint (hitPointBuff, dataList [i].HitPoint)) {
						indexHit = i;
					} else {
						indexHit = -1;
					}
                            
					if (indexRef != -1  
						&& indexObjNum != -1  
						&& indexHit != -1
						&& (indexObjNum == indexHit)
					    && (indexObjNum == indexRef)) {
						index = indexHit;
						point.Add (dataList [index].LookPoint);
						vector.Add (dataList [index].LookVector);
						distance.Add (dataList [index].Distance);
						angle.Add (dataList [index].Angle);
						hPoint.Add(dataList[index].HitPoint);
                                    
						stack1.Push (index);
					}
				}


				// 同一経路のレイ削除
				while (stack1.Count>0) {
					index = (int)stack1.Pop ();
					dataList.RemoveAt (index);
				}
				dataList.RemoveAt (0);

				// 
				dataListBuff.Add (new TraceData (){
                        LookVector=averageVec3(vector),
                        LookPoint=averageVec3(point),
                        Angle=averageDouble(angle),
                        Distance=averageDouble(distance),
						HitPoint=averageVec3(hPoint),
                        SumDistance=sumDistance(averageDouble(distance)),
                        HitNum=hitCount0,
                        HitObject=objName0                            
				});
    
			}
			dataList.Clear ();
			dataList = new List<TraceData> (dataListBuff);
		}

		private double range = 5;
		private bool nearPoint (List<Vector3> v1, List<Vector3> v2)
		{
			int n = v1.Count;
			bool near = false;
			double diff;
			double flag = 0;

			// v1とv2の差の平均
			if (v1.Count == v2.Count) {
				for (int i=0; i<n; i++) {
					diff = (Mathf.Abs (v1 [i].x - v2 [i].x) 
						+ Mathf.Abs (v1 [i].y - v2 [i].y) 
						+ Mathf.Abs (v1 [i].z - v2 [i].z)
                        ) / 3;

					if (diff < range) {
						flag += 0;
					} else {
						flag += 1;
					}
				}
			} else {
				flag = 1;
			}
			if (flag == 0)
				near = true;

			return near;
		}

		//重複するレイの座標を平均化
		private List<Vector3> averageVec3 (List<List<Vector3>> point)
		{
			List<Vector3> avrg = new List<Vector3> ();
			List<Vector3> buff = new List<Vector3> ();
			List<float> x = new List<float> ();
			List<float> y = new List<float> ();
			List<float> z = new List<float> ();

			bool flag = true;
			int i = 0;
			int j = 0;

			while (flag) {
				j = 0;

				// 行と列の入れ替え
				if (i < point [j].Count) {
					for (j = 0; j < point.Count; j++) {
						buff.Add (point [j] [i]);
					}
					i++;
 
					// 平均
					foreach (Vector3 p in buff) {
						x.Add (p.x);
						y.Add (p.y);
						z.Add (p.z);
					}
					avrg.Add (new Vector3 (x.Average (), y.Average (), z.Average ()));
					x.Clear ();
					y.Clear ();
					z.Clear ();
					buff.Clear ();
				} else {
					flag = false;
				}
			}

			return avrg;
		}

		//重複するレイの入射角の平均化
		private List<double> averageDouble (List<List<double>> angle)
		{
			List<double> avrg = new List<double> ();
			List<double> buff = new List<double> ();

			bool flag = true;
			int i = 0;

			while (flag) {                
				int j = 0;

				if (i < angle [j].Count) {
					for (j = 0; j < angle.Count; j++) {
						buff.Add (angle [j] [i]);
					}
					i++;
					avrg.Add (buff.Average ());

					buff.Clear ();
				} else {
					flag = false;
				}
			}
			return avrg;
		}


		//重複する直接波を１つにまとめる
		private void removeDirectRay ()
		{
			List<Vector3> tmpLookVector = new List<Vector3> (); //発射ベクトル
			List<Vector3> tmpLookPoint = new List<Vector3> ();  //発射点 
			List<string> tmpHitObject = new List<string> ();    //衝突した物体
			List<double> tmpDistance = new List<double> ();       //レイの長さ
			List<Vector3> tmpHitPoint = new List<Vector3> ();   //交点
			List<double> tmpAngle = new List<double> ();          //反射角
			float d = Vector3.Distance (Transmitter.Vector, Receiver.Vector);
			Stack stack = new Stack ();

			for (int i=0; i<dataList.Count; i++) {
				if (dataList [i].HitNum == 1) {
					tmpLookVector.Add (dataList [i].LookVector [0]);
					tmpDistance.Add (dataList [i].Distance [0]);

					stack.Push (i);
				}
			}
			if (0 < stack.Count) {
				int index;
				while (stack.Count>0) {
					index = (int)stack.Pop ();
					dataList.RemoveAt (index);
				}
      

				Vector3 aveVec = averageVec (tmpLookVector);

				tmpLookVector.Clear ();
				tmpLookVector.Add (aveVec);

            
				tmpDistance.Clear ();
				tmpDistance.Add (d);

				tmpAngle.Add (0);

				tmpLookPoint.Add (Transmitter.Vector);
				tmpHitObject.Add ("Receiver");
				tmpHitPoint.Add (Receiver.Vector);

				dataListBuff.Add (new TraceData{
                LookVector=tmpLookVector,
                LookPoint=tmpLookPoint,
                HitObject=tmpHitObject,
                Distance=tmpDistance,
				SumDistance=d,
                HitPoint=tmpHitPoint,
                Angle=tmpAngle,
                HitNum=1,
                HitRx=true}
				);
			}
		}
		private void removeHitRxAngle ()
		{
			foreach (TraceData td in dataList) {
				td.Angle.RemoveAt(td.Angle.Count-1);
			}
		}

		private Vector3 averageVec (List<Vector3> vList)
		{
			int count = vList.Count;
			float sumX = 0, sumY = 0, sumZ = 0;
			foreach (Vector3 v in vList) {
				sumX += v.x;
				sumY += v.y;
				sumZ += v.z;
			}

			return new Vector3 (sumX / count, sumY / count, sumZ / count);
		}

		private double sumDistance (List<double> list)
		{

			double sum = 0;

			foreach (double d in list) {
				sum += d;
			}
			return sum;
		}

		public static List<TraceData> DataList{ get { return dataList; } }

		public void ListClear ()
		{
			dataListBuff.Clear ();
			dataList.Clear ();
		}

		public void FIClear(){
			fi.Clear ();
			dataListBuff.Clear ();
			dataList.Clear ();
		}

		public static List<double> FI{ get { return fi; }}
	
		private GUIStyle labelStyle, labelStyleBox;
		private GUIStyleState labelStyleState;
		private string rText="10";
		void Start(){
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
		void OnGUI(){
			GUI.Label (new Rect (52, 400, 22, 20), " [m]", labelStyle);
			rText = GUI.TextField (new Rect (2, 400, 50, 20), rText, 5);
			if (Validation.IsNumeric (rText)) {
				range = float.Parse (rText);
				rText = range.ToString ();
			} else {
				rText = "0";
			}
		}
	}
}


