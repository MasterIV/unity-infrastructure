using UnityEngine;
using System.Collections;

public class SavegameService
{
	public SavegameService(MessageBroker broker)
	{
		broker.Sub<CharacterModel>(UpdateSaveGame);
	}

	public void UpdateSaveGame(CharacterModel model)
	{
		Debug.Log("update Savegame to " + model.Level);
	}
}
