using UnityEngine;
using System.Collections;

public class CharacterService
{
	private CharacterModel _character;
	private MessageBroker _broker;

	public CharacterService(CharacterModel character, MessageBroker broker)
	{
		_character = character;
		_broker = broker;
	}

	public void LevelUp()
	{
		_character.Level++;
		_broker.Pub(_character);
	}
}
