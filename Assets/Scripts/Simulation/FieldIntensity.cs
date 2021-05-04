using UnityEngine;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Simulation
{
	public class FieldIntensity
	{
		Conversion con = new Conversion ();
		List<Complex> ray = new List<Complex> ();
		private Complex reflecE;    //反射を考慮した電界強度

		private double directEdb;
		private Complex totalE;
		private static double totalEdb;
		private double gt, gr;
		private double pt;
		private double ar;
		private double lambda;
		private double z0;
		private double h1, h2;
		private double l;
		private List<Complex> rayFI = new List<Complex> ();      //電界強度を格納

		private Complex ground;     //大地の複素誘電率
		private Complex concrete;   //建造物（コンクリート）の複素誘電率
		public FieldIntensity (List<TraceData> dataList, double gain, double freq, double power)
		{
			totalEdb = 0;
			//利得[dBi]　送受信電力[W]　距離[m]　波長[m]
			gr = gain;
			gt = gain;
			//-------------------------------
			pt = power * 1e-03;
			z0 = 120 * Math.PI;
			freq = freq * 1e+06;
			lambda = con.FreqToWave (freq);

			//自由伝搬損失
//            l = Math.Pow ((4 * Math.PI * d) / lambda, 2);

			//受信アンテナの実行面積[㎡]
			ar = (gr * lambda * lambda) / (4 * Math.PI);

			//対数→真数
			gr = con.LogToAntiG (gr);
			gt = con.LogToAntiG (gt);

			//物体の誘電率
			SetPermittivity (freq);
            
			//直接波による電界強度を計算
			//directE = directWaveFI (d);

			//大地を考慮した場合
			//groundE = groundReflectedFI (d);


			//各レイの電界強度
			for (int i = 0; i < dataList.Count; i++) {
				reflecE = reflectedwaveFI (dataList [i].SumDistance, dataList [i].Angle, dataList [i].HitNum, dataList [i].HitObject);
				rayFI.Add (reflecE);
			}

			//受信点の電界強度
			totalE = summationFI ();
			totalEdb = 20 * Math.Log10 (totalE.Magnitude * 1e+06);
			totalEdb = con.ToRoundDown (totalEdb, 3);
		}

		private Complex directWaveFI (double d)
		{
			Complex e;

			//電界強度[V/m]
			e = Math.Sqrt ((z0 * pt * gt) / (4 * Math.PI * d * d));
			directEdb = 20 * Math.Log10 (e.Magnitude * 1e+06);
			directEdb = con.ToRoundDown (directEdb, 3);

			return e;
		}

		private Complex reflectedwaveFI (double length, List<double> angle, int hitNum, List<string> objName)
		{
			double k;    //波数
			Complex r;    //反射係数
			Complex ei;  //EXP(-jφ)　　φ：位相差
			Complex e;
			Complex direct;


			k = 2 * Math.PI / lambda;
			Complex theta = new Complex (0, -k * length);

			ei = Complex.Exp (theta);
			direct = directWaveFI (length);

			e = direct * ei;
			if (hitNum > 1) {
				int i = 0;
				foreach (double a in angle) {
					r = TEWave (a, objName [i]);
					e = e * r;
					i++;
				}
			}
			return e;
		}

		private Complex summationFI ()
		{
			Complex sumE = new Complex (0, 0);
			foreach (Complex e in rayFI) {
				sumE = sumE + e;
			}

			return sumE;
		}

		private Complex TEWave (double theta, string objName)
		{
            Complex upsilon1 = new Complex(1, 0);
            Complex upsilon2 = refractiveIndex(objName); //屈折率 衝突した物体

            double mu1 = 1; // 空気 透磁率
            double mu2 = 1; // 金属以外はほぼ1

            Complex n = Math.Sqrt(mu2 / mu1) * Complex.Sqrt(upsilon2 / upsilon1);　// 相対屈折率
            Complex n2 = n * n;

            double sin2 = Math.Sin(theta) * Math.Sin(theta);

            Complex numerator = mu2 * Math.Cos(theta) - mu1 * Complex.Sqrt(n2 - sin2);
            Complex denominator = Math.Cos(theta) + Complex.Sqrt(n2 - sin2);

            Complex rte = numerator / denominator;

            return rte;
        }

		private Complex TMWave (double theta, string objName)
		{
            Complex upsilon = refractiveIndex (objName); //屈折率
			double sin2 = Math.Sin (theta) * Math.Sin (theta);

            Complex numerator = Complex.Sqrt (upsilon - sin2) - upsilon * Math.Cos (theta);
            Complex denominator = Complex.Sqrt (upsilon - sin2) + upsilon * Math.Cos (theta);

            Complex rtm = numerator / denominator;
			return rtm;

		}

		private Complex refractiveIndex (string objName)
		{

			Complex upsilon = GetPermittivity (objName);			

			return upsilon;
		}

		private void SetPermittivity (double freq)
		{

			//double upsilon0 = 1 / (4 * Math.PI * c * c * 1e-07);    //真空の誘電率ε₀
			
			double omega = 2 * Math.PI * freq;

			ground = new Complex (4, -10 * 1e-03 / omega);
			concrete = new Complex (6.76, -2.3 * 1e-03 / omega);
		}

		private Complex GetPermittivity (string objName)
		{

			switch (objName) {
			case "ground":
				return ground;
			case "obj":
				return concrete;
			default:
				return ground;
			}
		}

		private List<Complex> roundDown (HashSet<Complex> hs)
		{
			List<Complex> r = new List<Complex> ();
			foreach (Complex e in hs) {
				r.Add (new Complex (
                    con.ToRoundDown (e.Real, 15),
                    con.ToRoundDown (e.Imaginary, 15)));
			}
			return r;
		}

        #region 戻り値
		public double DirectEdb {
			get { return directEdb; }
		}

		public Complex RefE {
			get { return reflecE; }
		}

		public static double TotalEdb {
			get { return totalEdb; }
		}

		public List<Complex> Ray {
			get { return ray; }
		}
        #endregion
	}

	struct Conversion
	{

		public double WatToDbm (double wat)
		{
			wat = 10 * Math.Log10 (wat * 1000);
			return wat;
		}

		public double LogToAntiG (double g)
		{
			g = Math.Pow (10, g / 10);
			return g;
		}

		public double ToRoundDown (double dValue, int iDigits)
		{
			double dCoef = System.Math.Pow (10, iDigits);

			return dValue > 0 ? System.Math.Floor (dValue * dCoef) / dCoef :
                                System.Math.Ceiling (dValue * dCoef) / dCoef;
		}

		public double FreqToWave (double freq)
		{
			double c = 299792458;
			double wave;
			wave = c / freq;

			return wave;
		}

		public double WaveToFreq (double wave)
		{
			double c = 299792458;
			double freq;
			freq = c / wave;

			return freq;
		}
	}
}
