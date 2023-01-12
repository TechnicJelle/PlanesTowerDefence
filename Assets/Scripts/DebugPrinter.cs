using UnityEngine;

public static class DebugPrinter
{
	/// <summary>
	/// <b>Debug Print<br/></b>
	/// This only prints something in Debug Mode
	/// </summary>
	public static void DPrint(string msg)
	{
#if DEBUG
		Debug.Log(msg);
#endif
	}
}
