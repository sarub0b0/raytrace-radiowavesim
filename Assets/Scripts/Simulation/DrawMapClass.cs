using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Simulation
{
	//定数を一覧にするクラス
	public class ConstValue
	{
		public const int ImageSize = 1000;
		public static int Div = 50;
		public const int ColorMax = 255;    // HSVのHueは0～360で一周するが，このプログラムでは赤から青の0～255までを使う
		public const int RandomMax = 100;   // 入力データ生成に用いる乱数の最大値
		public const int RandomMin = 0;     // 入力データ生成に用いる乱数の最小値
	}
	
	// マップ描画クラス
	public class DrawMapClass:MonoBehaviour
	{
		private Texture2D texture;
		private List<Color> colorList = new List<Color> ();
		private int BrushListWhite;

		// この2次元配列にデータ（数字）が入る
		private int dataSize = ConstValue.Div;
		public Point[] rxPoints = new Point[(ConstValue.Div) * (ConstValue.Div)];
				
		// 入力データを生成（0～100のどれかがランダムで入るようになってます)
		public void CreateData (List<double> el)
		{		
			double[,] data = new double[dataSize, dataSize ];
			for (int y = 0; y < dataSize; y++) {
				for (int x = 0; x < dataSize; x++) {
					data [x, y] = el [x + y * dataSize];
//					if (y % 2 == 0) {
//						data [x, y] = (x % 2) * 100;
//					} else {
//						data [x, y] = ((x + 1) % 2) * 100;
//					}
//					data [x, y] = (int)Random.Range (0, 100);
//					data [x, y] = el [x + y * dataSize];
				}
			}
			CreateColorList ();

			CreateMap (data);
		
			texture.Apply ();

			GameObject.Find ("Plane").GetComponent<Renderer> ().material.mainTexture = texture;

			colorList.Clear ();

			// Database db = GetComponent<Database>();

            //db.InsertedRadiomap(data);

            System.IO.StreamWriter sw = new System.IO.StreamWriter(
                          @"C:\\Users\\kousei\\Desktop\\unityLog.csv",
                          false,
                          System.Text.Encoding.GetEncoding("shift_jis"));

            WriteData("Cell", data, ref sw);


            sw.Close();

        }

        private void WriteData(string name, double[,] data, ref System.IO.StreamWriter sw)
        {
            sw.WriteLine(name);
            sw.WriteLine();
            sw.Write(",");
            int n;
            for (int i = 0; i < ConstValue.Div; i++)
            {
                n = i + 1;
                sw.Write(n + ",");
            }
            sw.WriteLine();
            for (int y = 0; y < ConstValue.Div; y++)
            {
                n = y + 1;
                sw.Write(n + ",");
                for (int x = 0; x < ConstValue.Div; x++)
                {
                    sw.Write(data[x, y] + ",");
                }
                sw.WriteLine();
            }
            sw.WriteLine();
        }



        public void CreateColorList ()
		{
			//ブラシの色を変えるときはここ
			for (int i = 0; i <= ConstValue.ColorMax; i++) {
				colorList.Add (ColorHSV.FromHsv (ConstValue.ColorMax - i, 255, 255));
			}

			colorList.Add (Color.white);
			BrushListWhite = colorList.Count - 1;
		}

		public void CreateMap (double[,] d)
		{
			Mesh mesh = GetComponent<MeshFilter> ().mesh;
			texture = new Texture2D (dataSize, dataSize, TextureFormat.RGBA32, true);

			#region 階調変換
			double min, max;
			
			//min，maxの初期値はありえない値にしておく
			min = ConstValue.RandomMax + 1;
			max = ConstValue.RandomMin - 1;
			
			for (int y = 0; y < dataSize; y++) {
				for (int x = 0; x < dataSize; x++) {
					if (d [x, y] != 0) {
						if (d [x, y] > max) {
							max = d [x, y];
						}
						if (d [x, y] < min) {
							min = d [x, y];
						}
					}
				}
			}
			
			double a;
			a = ConstValue.ColorMax / (max - min);

			int[] tempColor = new int[d.Length];
			int n = 0;
			for (int y = 0; y < dataSize; y++) {
				for (int x = 0; x < dataSize; x++) {
					if (d [x, y] == 0) {
						tempColor [n++] = BrushListWhite;
						DrawPoint (BrushListWhite, x, y);
					} else {
						tempColor [n++] = (int)(a * (d [x, y] - min));
						DrawPoint ((int)(a * (d [x, y] - min)), x, y);
					}	
				}
			}

			#endregion

			List<Color> colors = new List<Color> ();
			int index = (int)Mathf.Sqrt (mesh.colors.Length);
			int[,] cip = new int[index, index];

			n = 0;
			//int half = Mathf.CeilToInt ((float)size/2f);

			int divSize = index / ConstValue.Div;
		
			int dx = 0, dy = 0;
			for (int y=0; y<index; y++) {

				dx = 0;
				if (y % (divSize) == 0 && y != 0 && y != 100) {
					dy++;
				}

				for (int x=0; x<index; x++) {

					if (x % (divSize) == 0 && x != 0 && x != 100) {
						dx++;
					}
					if (d [dx, dy] == 0) {
						cip [x, y] = BrushListWhite;
					} else {
						cip [x, y] = (int)(a * (d [dx, dy] - min));
					}	
				}
			}

			for (int y=0; y<index; y++) {
				for (int x=0; x<index; x++) {
					colors.Add (colorList [cip [x, y]]);
				}
			}
			mesh.colors = colors.ToArray ();
			colors.Clear ();
		}

//		private void colorInterpolation (ref int[,] ps, Point p, int s, int c, int n)
//		{
//			int sx = 0;
//			int sz = 0;
//			int ex = 0;
//			int ez = 0;
//
//			int half = Mathf.CeilToInt ((float)s / 2f);
//			int end = n;
//			//x=0 || z= end
//			if (p.x == 0 || p.y == end) {
//				if (p.x == 0 && p.y == end) {
//					sx = 0;
//					sz = end;
//					ex = p.x + half;
//					ez = p.y - half;
//				} else if (p.x == 0) {
//					sx = 0;
//					sz = p.y;
//					ex = p.x + half;
//					ez = p.y - s;
//				} else if (p.y == end) {
//					sx = p.x;
//					sz = end;
//					ex = p.x + s;
//					ez = p.y - half;
//				}
//			} else {
//				sx = p.x;
//				sz = p.y;
//				ex = sx + s;
//				ez = sz - s;
//			}
//
//			if (end <= ex) {
//				ex = end;
//			} 
//			if (ez <= 0) {
//				ez = 0;
//			}
//
//			for (int z=sz; ez<=z; z--) {
//				for (int x = sx; x<= ex; x++) {
//					ps [x, z] = c;
//				}
//			}
//		}

		public void DrawPoint (int value, int x, int z)
		{
			texture.SetPixel (x, z, colorList [value]);
		}

		public Point[] RxPoints{ get { return rxPoints; } set { this.rxPoints = value; } }
	}
}