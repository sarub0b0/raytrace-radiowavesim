
public struct Point
{
	private int mx, my;
	
	public Point (int p1, int p2)
	{
		this.mx = p1;
		this.my = p2;
	}
	
	public static bool operator != (Point left, Point right)
	{
		if (left.mx != right.mx || left.my != right.my) {
			return true;
		} else {
			return false;
		}
	}
	
	public static bool operator == (Point left, Point right)
	{
		if (left.mx == right.mx && left.my == right.my) {
			return true;
		} else {
			return false;
		}
	}
	
	public override bool Equals (System.Object obj)
	{
		// If parameter is null return false.
		if (this.GetType () != obj.GetType ()) {
			return false;
		}
		
		// If parameter cannot be cast to Point return false.
		Point p = (Point)obj;
		if (p.mx == this.mx && p.my == this.my) {
			// Return true if the fields match:
			return true;
		} else {
			return false;
		}
		
		// Return true if the fields match:
		
	}
	
	public override int GetHashCode ()
	{
		return base.GetHashCode ();
	}
	
	public static Point operator + (Point left, Point right)
	{
		return new Point (left.mx + right.mx, left.my + right.my);
	}
	
	public static Point operator - (Point left, Point right)
	{
		return new Point (left.mx - right.mx, left.my - right.my);
	}
	
	public static Point operator * (Point left, int right)
	{
		return new Point (left.mx * right, left.my * right);
	}
	
	public static Point operator * (int left, Point right)
	{
		return new Point (right.mx * left, right.my * left);
	}
	
	public static Point operator / (Point left, int right)
	{
		return new Point (left.mx / right, left.my / right);
	}
	public int x { get { return mx; } set { this.mx = value; } }
	
	public int y{ get { return my; } set { this.my = value; } }
	
	
}
