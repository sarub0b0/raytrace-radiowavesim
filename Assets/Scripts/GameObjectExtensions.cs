using UnityEngine;

/// <summary>
/// GameObject 型の拡張メソッドを管理するクラス
/// </summary>
public static class GameObjectExtensions
{
	/// <summary>
	/// ローカル座標を維持して親オブジェクトを設定します
	/// </summary>
	public static void SafeSetParent(this GameObject self, GameObject parent)
	{
		var t           = self.transform;
		t.parent        = parent.transform;
		t.localPosition = Vector3.zero;
		t.localRotation = Quaternion.identity;
		t.localScale    = Vector3.one;
		self.layer      = parent.layer;
	}
	
	/// <summary>
	/// ローカル座標を維持して親オブジェクトを設定します
	/// </summary>
	public static void SafeSetParent(this GameObject self, Component parent)
	{
		SafeSetParent(self, parent.gameObject);
	}
}

/// <summary>
/// Component 型の拡張メソッドを管理するクラス
/// </summary>
public static class ComponentExtensions
{
	/// <summary>
	/// ローカル座標を維持して親オブジェクトを設定します
	/// </summary>
	public static void SafeSetParent(this Component self, GameObject parent)
	{
		var t                   = self.transform;
		t.parent                = parent.transform;
		t.localPosition         = Vector3.zero;
		t.localRotation         = Quaternion.identity;
		t.localScale            = Vector3.one;
		self.gameObject.layer   = parent.layer;
	}
	
	/// <summary>
	/// ローカル座標を維持して親オブジェクトを設定します
	/// </summary>
	public static void SafeSetParent(this Component self, Component parent)
	{
		SafeSetParent(self, parent.gameObject);
	}
}