using UnityEngine;
using System.Collections.Generic;

public class TraceData
{
    private List<Vector3> lookVector = new List<Vector3> (); //発射ベクトル
    private List<Vector3> lookPoint = new List<Vector3> ();  //発射点 
    private List<string> hitObject = new List<string> ();    //衝突した物体
    private List<double> distance = new List<double> ();       //レイの長さ
    private List<Vector3> hitPoint = new List<Vector3> ();   //交点
    private List<double> angle = new List<double> ();          //反射角

    private double sumDistance;
    private int hitNum ;          //反射数
    private bool hitRx=false;          //RXにあたったかどうか

    private Complex fi;           //電界強度

    public void SetLookVec (Vector3 look)
    {
        lookVector.Add (look);
    }

    public void SetLookPoint (Vector3 point)
    {
        lookPoint.Add (point);
    }

    public void SetHitObj (string name)
    {
        hitObject.Add (name);
    }

    public void SetDistance (double d)
    {
        distance.Add (d);
    }

    public void SetHitPoint (Vector3 point)
    {
        hitPoint.Add (point);
    }

    public void SetAngle (Vector3 look, Vector3 reflec)
    {
        angle.Add (Vector3.Angle (-look, reflec));
    }

	public void SetAngle (double a){
		angle.Add(a);
	}

    public double GetSumDistance ()
    {
        double s = 0;
        foreach (double d in distance) {
            s = s + d;
        }
        return s;
    }

    //Reset Data
    public void ResetData ()
    {
        lookPoint.Clear ();
        lookVector.Clear ();
        hitObject.Clear ();
        distance.Clear ();
        hitPoint.Clear ();
        angle.Clear ();
    }

    public List<Vector3> LookVector { 
        get { return lookVector; }
        set{ this.lookVector = value;}
    }

    public List<Vector3> LookPoint { 
        get { return lookPoint; } 
        set{ this.lookPoint = value;}
    }

    public List<string> HitObject { 
        get { return hitObject; }
        set{ this.hitObject = value;}
    }

    public List<double> Distance { 
        get { return distance; }
        set{ this.distance = value;}
    }

    public List<Vector3> HitPoint { 
        get { return hitPoint; } 
        set{ this.hitPoint = value;}
    }

    public List<double> Angle {
        get { return angle; }
        set{ this.angle = value;}
    }
    
    public double SumDistance { 
        get { return sumDistance; } 
        set{ this.sumDistance = value;}
    }

    public int HitNum { 
        get { return hitNum; } 
        set{ this.hitNum = value;}
    }

    public bool HitRx { 
        get { return hitRx; } 
        set{ this.hitRx = value;}
    }

    public Complex FI { 
        get { return fi; }
        set{ this.fi = value;}
    }
}