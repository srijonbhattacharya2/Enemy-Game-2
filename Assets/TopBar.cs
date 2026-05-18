using UnityEngine;

public class TopBar : MonoBehaviour
{
    public void quit()
    {
    #if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
	#else
		Application.Quit();
	#endif
    }
}
