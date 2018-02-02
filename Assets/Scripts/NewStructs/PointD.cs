
public struct PointD
{
	private double x, y;

	public PointD (double p1, double p2)
	{
		this.x = p1;
		this.y = p2;
	}

	public static bool operator != (PointD left, PointD right)
	{
		if (left.x != right.x || left.y != right.y) {
			return true;
		} else {
			return false;
		}
	}

	public static bool operator == (PointD left, PointD right)
	{
		if (left.x == right.x && left.y == right.y) {
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
		PointD p = (PointD)obj;
		if (p.x == this.x && p.y == this.y) {
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

	public static PointD operator + (PointD left, PointD right)
	{
		return new PointD (left.x + right.x, left.y + right.y);
	}

	public static PointD operator - (PointD left, PointD right)
	{
		return new PointD (left.x - right.x, left.y - right.y);
	}

	public static PointD operator * (PointD left, double right)
	{
		return new PointD (left.x * right, left.y * right);
	}

	public static PointD operator * (double left, PointD right)
	{
		return new PointD (right.x * left, right.y * left);
	}

	public static PointD operator / (PointD left, double right)
	{
		return new PointD (left.x / right, left.y / right);
	}

	public double X { get { return x; } set { this.x = value; } }

	public double Y{ get { return y; } set { this.y = value; } }


}
