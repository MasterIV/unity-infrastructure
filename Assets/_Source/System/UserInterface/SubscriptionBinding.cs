using System;
using System.CodeDom;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using UnityEngine.UI;

public abstract class InterfaceBinding<T>
{
	private string _prefix = "";
	private readonly List<IBinding> _bindings;

	protected InterfaceBinding()
	{
		_bindings = new List<IBinding>();
	}

	public void Update(T model)
	{
		for (int i = 0; i < _bindings.Count; i++)
		{
			if (_bindings[i].Target is Text)
				((Text) _bindings[i].Target).text = _bindings[i].GetValue(model).ToString();
			else if (_bindings[i].Target is Image)
				((Image) _bindings[i].Target).sprite = (Sprite) _bindings[i].GetValue(model);
			else if (_bindings[i].Target is Slider)
				((Slider) _bindings[i].Target).value = (float) _bindings[i].GetValue(model);
			else if (_bindings[i].Target is Toggle)
				((Toggle) _bindings[i].Target).isOn = (bool) _bindings[i].GetValue(model);
		}
	}

	public InterfaceBinding<T> Prefix(string prefix)
	{
		_prefix = prefix;
		return this;
	}

	public InterfaceBinding<T> Bind(object element, string property)
	{
		if (element == null)
			return this;

		_bindings.Add(new SingleBinding(element, new PropertyReader<T>(_prefix + property)));

		return this;
	}

	public InterfaceBinding<T> Format(object element, string format, params string[] properties)
	{
		if (element == null)
			return this;

		var sources = new PropertyReader<T>[properties.Length];

		for (int i = 0; i < properties.Length; i++)
			sources[i] = new PropertyReader<T>(_prefix + properties[i]);

		_bindings.Add(new FormattedBinding(element, sources, format));

		return this;
	}

	public abstract void Init();
}


public class SubscriptionBinding<T> : InterfaceBinding<T>
{

	public SubscriptionBinding(MessageBroker broker)
	{
		broker.Sub<T>(Update);
	}

	public override void Init()
	{

	}
}

public class ObjectBinding<T>: InterfaceBinding<T>
{
	private T _subject;

	public ObjectBinding(T subject)
	{
		_subject = subject;
	}

	public override void Init()
	{
		Update(_subject);
	}
}
