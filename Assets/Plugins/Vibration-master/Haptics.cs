using UnityEngine;

public static class Haptics
{
	public static void Low()
	{
		Vibration.VibratePop();
	}

	public static void Medium()
	{
		Vibration.VibratePeek();
	}

	public static void High()
	{
		Vibration.VibrateNope();
	}
}