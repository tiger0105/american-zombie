using UnityEngine;

public class ZoomManager : MonoBehaviour
{
	public static ZoomManager Instance;
	public GameObject zoomIn;
	public GameObject zoomPanel;

	// Use this for initialization
	void Awake()
	{
		Instance = this;
		Init();
	}

	public void Zoom()
	{
		zoomIn.SetActive(!zoomIn.activeSelf);
		zoomPanel.SetActive(!zoomPanel.activeSelf);
	}

	public void Init()
	{
		zoomIn.SetActive(false);
		zoomPanel.SetActive(false);
	}
}