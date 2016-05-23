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

		Subscribe<CharacterModel>()
			.Format(CharacterName, "Der {0} ist ein kleiner wicht auf Level {1} und hat Attack {2}", "Name", "Level", "Stats.Attack")
			.Bind(CharacterLevel, "Level")
			.Init();
	}

	public void OnLevelUp()
	{
		_service.LevelUp();
	}
}
