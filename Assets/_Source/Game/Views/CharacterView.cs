using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterView : BaseView
{
	public Text CharacterName;
	public Text CharacterLevel;
	public Text Headline;

	private CharacterService _service;

	public void SetService(CharacterService service)
	{
		_service = service;
	}

	// Use this for initialization
	void Start () {

	 Debug.Log("Bar");

		Subscribe<CharacterModel>()
			.Bind(CharacterName, "Name")
			.Bind(CharacterLevel, "Level")
			.Translate(Headline, "Character Profile")
			.Init();
	}

	public void OnLevelUp()
	{
		_service.LevelUp();
	}
}
