﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Ink.Runtime;
using UnityEngine.SceneManagement;

// This is a super bare bones example of how to play and display a ink story in Unity.
public class BasicInkExample : MonoBehaviour
{
	private int count;
	public AudioSource click;
	public Sprite dana;
	public Sprite kimmy;
	public Sprite danaMom;
	public Sprite kimmyMom;
	public Sprite dean;
	public Sprite donna;
	public Sprite janey;
	public Sprite jimmy;
	public Sprite harold;
	
	[SerializeField] private TextAsset _inkJsonAsset;
	[SerializeField] private Story story;
	
	// UI Prefabs
	[SerializeField] private Text textPrefab;
	[SerializeField] private Button buttonPrefab;
	
	
	[SerializeField] private Canvas canvas;
	[SerializeField] private Canvas canvas2;

	[SerializeField] private Image imagePrefab;

	// Remove the default 
	// Creates a new Story object with the compiled story which we can then play!

	private void Start()
	{
		story = new Story(_inkJsonAsset.text);
		RemoveChildren();
		var text = story.Continue();
		if (story.currentChoices.Count > 0)
		{
			for (var i = 0; i < story.currentChoices.Count; i++) {
				Choice choice = story.currentChoices [i];
				Button button = CreateChoiceView (choice.text.Trim ());
				// Tell the button what to do when we press it
				button.onClick.AddListener (delegate {
					OnClickChoiceButton (choice);
				});
			}
		}
		CreateContentView(text);
	}

	private void Update()
	{
		RefreshView();
	}

	void RefreshView(){
		
		if (Input.GetKeyDown(KeyCode.Z))
		{
			SceneManager.LoadScene("Menu");
		}
		
		if (!Input.GetKeyDown(KeyCode.Mouse0)) return;
		
		click.Play();
		
		if (story.currentChoices.Count > 0)
		{
			for (var i = 0; i < story.currentChoices.Count; i++)
			{
				if (Input.GetKeyDown((KeyCode) 49 + i))
				{
					story.ChooseChoiceIndex(i);
				}
			}
		}

		if (!story.canContinue) return;

		var text = story.Continue();

		RemoveChildren();

		if (story.currentChoices.Count > 0)
		{
			for (var i = 0; i < story.currentChoices.Count; i++) {
				Choice choice = story.currentChoices [i];
				Button button = CreateChoiceView (choice.text.Trim ());
				// Tell the button what to do when we press it
				button.onClick.AddListener (delegate {
					OnClickChoiceButton (choice);
				});
			}
		}

		CreateContentView(text);
	}
	
	void OnClickChoiceButton(Choice choice)
	{
		story.ChooseChoiceIndex(choice.index);
		RemoveChildren();
		var text = story.Continue();
		var choiceText = "";

		
		if (story.currentChoices.Count > 0)
		{
			for (var i = 0; i < story.currentChoices.Count; i++) {
				Choice choices = story.currentChoices [i];
				Button button = CreateChoiceView (choices.text.Trim ());
				button.onClick.AddListener (delegate {
					OnClickChoiceButton (choices);
				});
			}
		}
		CreateContentView(text);
	}
	
	void CreateContentView(string text)
	{
		var storyText = Instantiate(textPrefab);
		var storyImage = Instantiate(imagePrefab);
		storyText.text = text;
		storyText.transform.SetParent (canvas.transform, false);
		storyImage.transform.SetParent (canvas.transform, false);


		storyText.text = text;
		{
			if (text.Contains("Dana:"))
			{
				storyImage.sprite = dana;
			}
			if (text.Contains("Kimmy:"))
			{
				storyImage.sprite = kimmy;
			}
			if (text.Contains("Mom:"))
			{
				storyImage.sprite = danaMom;
			}
			if (text.Contains("Mrs. Munro:"))
			{
				storyImage.sprite = kimmyMom;
			}
			if (text.Contains("Dean:"))
			{
				storyImage.sprite = dean;
			}
			if (text.Contains("Donna:"))
			{
				storyImage.sprite = donna;
			}
			if (text.Contains("Janey:"))
			{
				storyImage.sprite = janey;
			}
			if (text.Contains("Jimmy:"))
			{
				storyImage.sprite = jimmy;
			}
			if (text.Contains("Harold:"))
			{
				storyImage.sprite = harold;
			}
		}
	}
	
	    // Creates a button showing the choice text
	Button CreateChoiceView (string text) {
		// Creates the button from a prefab
		Button choice = Instantiate (buttonPrefab) as Button;
		choice.transform.SetParent (canvas2.transform, true);
		
		// Gets the text from the button prefab
		Text choiceText = choice.GetComponentInChildren<Text> ();
		choiceText.text = text;

		// Make the button expand to fit the text
		HorizontalLayoutGroup layoutGroup = choice.GetComponent <HorizontalLayoutGroup> ();
		layoutGroup.childForceExpandHeight = false;
	
		return choice;
	}
	
	void RemoveChildren () {
		int childCount = canvas.transform.childCount;
		for (int i = childCount - 1; i >= 0; --i) {
			Destroy (canvas.transform.GetChild (i).gameObject);
		}
		int buttonChildCount = canvas2.transform.childCount;
		for (int i = buttonChildCount - 1; i >= 0; --i) {
			Destroy (canvas2.transform.GetChild (i).gameObject);
		}
	}
}