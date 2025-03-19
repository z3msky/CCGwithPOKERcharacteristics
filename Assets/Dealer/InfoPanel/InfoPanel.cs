using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;


[RequireComponent(typeof(PlayerInput))]
public class InfoPanel : MonoBehaviour
{

	public Image ShadowImage;
	private TextMeshProUGUI m_text;


	// Start is called before the first frame update
	void Start()
    {
		m_text = GetComponentInChildren<TextMeshProUGUI>();
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	public void OnPoint(InputAction.CallbackContext context)
	{
		EventSystem evSys = EventSystem.current;
		PointerEventData eventData = new PointerEventData(evSys);
		eventData.position = context.ReadValue<Vector2>();


		List<RaycastResult> results = new List<RaycastResult>();
		foreach (GraphicRaycaster raycaster in FindObjectsByType<GraphicRaycaster>(FindObjectsSortMode.None))
		{
			raycaster.Raycast(eventData, results);
			//Debug.Log(results.Count + "Results");

			foreach (RaycastResult result in results)
			{
				InfoPanelItem item = result.gameObject.GetComponent<InfoPanelItem>();
				if (item != null)
				{
					m_text.text = item.InfoPanelMessage;
					ShadowImage.enabled = true;
					ShadowImage.sprite = item.InfoPanelSprite;
					return;
				}
			}
			results.Clear();
		}


		m_text.text = "Information";
		ShadowImage.enabled = false;
	}
}
