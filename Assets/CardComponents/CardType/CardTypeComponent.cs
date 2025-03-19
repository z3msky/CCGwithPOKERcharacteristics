using UnityEngine;

[RequireComponent(typeof(Card))]
public class CardTypeComponent : MonoBehaviour
{
	public CardType CardTypeOfComponent { get; protected set; }
	public Card Card
	{
		get
		{
			return GetComponent<Card>();
		}
	}

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	protected void AnnounceAttachToCard()
	{
		Debug.Log("Attached [" + CardTypeOfComponent.ToString() + "] to card object [" + gameObject.name + "].");
	}

	virtual public void ActivateDesignElements(CardVisual card)
	{

	}

}
