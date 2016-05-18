using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MessageBroker
{
	private Dictionary<Type, IList> _subscriptions;

	public MessageBroker()
	{
		_subscriptions = new Dictionary<Type, IList>();
	}

	public void Pub<T>(T obj)
	{
		var key = typeof(T);

		if (!_subscriptions.ContainsKey(key)) return;

		var list = (List<Action<T>>) _subscriptions[key];
		for (int i = 0; i < list.Count; i++)
			list[i](obj);
	}

	public void Sub<T>(Action<T> callback)
	{
		List<Action<T>> list;
		var key = typeof(T);

		if (_subscriptions.ContainsKey(key))
		{
			list = (List<Action<T>>) _subscriptions[key];
		}
		else
		{
			list = new List<Action<T>>();
			_subscriptions.Add(key, list);
		}

		list.Add(callback);
	}
}
