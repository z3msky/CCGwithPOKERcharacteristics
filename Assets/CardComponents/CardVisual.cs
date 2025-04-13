using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Card))]
public class CardVisual : MonoBehaviour
{
	[Header("Card Design References")]
	public Image FloatingCard;
	public Image RankImage;
	public Image SuitImage;
	public Image CardArtImage;
	public Image CardLightenerImage;
	public RawImage SummoningSickness;
	public GameObject Cardback;
	public TextMeshProUGUI PowerToughnessText;
	public TextMeshProUGUI CardNameText;
	public TextMeshProUGUI RulesText;
	public Image AbilityPanel;
	public GameObject AdvRetButtonPanel;

	[Header("Prefab")]
	public GameObject KeywordIconPrefab;

	public Card card {get; private set;}

	public void UpdateCardDisplay()
	{
		card = GetComponent<Card>();

		FloatingCard.gameObject.SetActive(!card.CurrentZone.CardsDisappear());
		SummoningSickness.gameObject.SetActive(
			card.EnteredThisTurn
			&& !card.IntentMode
			&& card.IsCardType(CardType.UNIT));


		RulesText.text = card.FullRulesText;
		SetupAbilityPanel();

		if (card.Revealed)
		{
			Cardback.gameObject.SetActive(false);

			CardArtImage.gameObject.SetActive((card.CardDataAsset.CardArt != null));
			RankImage.gameObject.SetActive(true);
			SuitImage.gameObject.SetActive(true);
			CardNameText.gameObject.SetActive(true);
			RulesText.gameObject.SetActive(true);
			AbilityPanel.gameObject.SetActive(true);
			AdvRetButtonPanel.gameObject.SetActive(false);
			PowerToughnessText.gameObject.SetActive(false);
			CardLightenerImage.gameObject.SetActive(true);

			FloatingCard.color = card.NormalCardColor;
			RankImage.color = Color.black;
			PowerToughnessText.color = Color.black;

			if (card.TraceMode)
			{
				FloatingCard.color = card.TraceCardColor;
			}
			else if (card.IntentMode)
			{
				CardLightenerImage.gameObject.SetActive(false);
				FloatingCard.color = card.IntentCardColor;
				RankImage.color = Color.gray;
				PowerToughnessText.color = Color.white;
			}
		}
		else
		{
			Cardback.gameObject.SetActive(true);

			CardArtImage.gameObject.SetActive(false);
			RankImage.gameObject.SetActive(false);
			SuitImage.gameObject.SetActive(false);
			CardNameText.gameObject.SetActive(false);
			PowerToughnessText.gameObject.SetActive(false);
			RulesText.gameObject.SetActive(false);
			AdvRetButtonPanel.gameObject.SetActive(false);
			AbilityPanel.gameObject.SetActive(false);

			return; // just leave if hidden
		}

		foreach (CardTypeComponent typeComponent in GetComponents<CardTypeComponent>())
		{
			typeComponent.ActivateDesignElements(this);
		}

		CardArtImage.sprite = card.CardDataAsset.CardArt;
		RankImage.sprite = card.RankSprites[card.Rank];
		switch (card.Suit)
		{
			case Suit.SPADES:
				SuitImage.sprite = card.SpadeSprite;
				break;
			case Suit.HEARTS:
				SuitImage.sprite = card.HeartSprite;
				break;
			case Suit.CLUBS:
				SuitImage.sprite = card.ClubSprite;
				break;
			case Suit.DIAMONDS:
				SuitImage.sprite = card.DiamondSprite;
				break;
		}

		CardNameText.text = card.ShortName.ToUpper();
		if (card.CardDataAsset.CenteredRules)
		{
			RulesText.horizontalAlignment = HorizontalAlignmentOptions.Center;
			RulesText.verticalAlignment = VerticalAlignmentOptions.Top;
		}
		else
		{
			RulesText.alignment = TextAlignmentOptions.TopLeft;
		}


		gameObject.name = card.CardName + " " + gameObject.GetInstanceID();
	}

	public void SetupAbilityPanel()
	{
		foreach (Transform child in AbilityPanel.transform)
			Destroy(child.gameObject);

		//Debug.Log("Reset Abilities: " + card.KeywordAbilities.Length);

		foreach (KeywordAbilityData keyword in card.KeywordAbilities)
		{
			Debug.Assert(KeywordIconPrefab.GetComponent<KeywordAbilityIcon>() != null);

			GameObject go = GameObject.Instantiate(KeywordIconPrefab, AbilityPanel.transform);
			KeywordAbilityIcon icon = go.GetComponent<KeywordAbilityIcon>();

			RulesText.text = keyword.Keyword.ToString() + ". " + RulesText.text;

			icon.Setup(keyword);
		}
	}

	public void UpdateCardDisplayHeight()
	{
		Rect old = GetComponent<Image>().rectTransform.rect;

		float hRatio = old.height / card.DisplayHeightTarget;
		float h = Mathf.Lerp(old.height, card.DisplayHeightTarget, card.LerpRatio * Time.deltaTime * 2);

		if (hRatio >= 0.95f || hRatio <= 1.05f)
		{
			GetComponent<Image>().rectTransform
			.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, card.DisplayHeightTarget);
		}
		else
		{
			GetComponent<Image>().rectTransform
				.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);
		}


		// kind of lazy but using opacity
		// because TypeComponent deals with enabling
		if (h >= 150)
		{
			RulesText.alpha = 1;
		}
		else
		{
			RulesText.alpha = 0;
		}
	}

	public static void UpdateAll()
	{
		foreach (CardVisual v in FindObjectsByType<CardVisual>(FindObjectsSortMode.None))
		{
			v.UpdateCardDisplay();
		}
	}
}
