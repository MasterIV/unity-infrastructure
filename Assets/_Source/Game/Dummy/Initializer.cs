using UnityEngine;
using System.Collections;

public class Initializer : MonoBehaviour
{
	public CharacterView CharacterView;

	// Use this for initialization
	void Start()
	{
		var character = new CharacterModel()
		{
			Name = "Dario",
			Level = 1,
			Stats = new CharacterModel.Attributes() {Attack = 10, Defence = 20}
		};

		var broker = new MessageBroker();
		var service = new CharacterService(character, broker);

		CharacterView.SetDependencies(broker);
		CharacterView.SetService(service);

		new SavegameService(broker);
	}
}
