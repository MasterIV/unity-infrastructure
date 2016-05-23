using UnityEngine;
using System.Collections;

public abstract class BaseView : MonoBehaviour
{
	protected MessageBroker _broker;

	public void SetDependencies(MessageBroker broker)
	{
		_broker = broker;
	}

	protected SubscriptionBinding<T> Subscribe<T>()
	{
		return new SubscriptionBinding<T>(_broker);
	}

	protected ObjectBinding<T> With<T>(T subject)
	{
		return new ObjectBinding<T>(subject);
	}
}
