using UnityEngine;
using System.Collections;
using JetBrains.Annotations;
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

	void Start()
	{
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
