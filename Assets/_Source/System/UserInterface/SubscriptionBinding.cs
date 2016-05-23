using System;
using System.CodeDom;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using UnityEngine.UI;

public class SubscriptionBinding<T>
{
	private interface IBinding
	{
		object Target { get; }
		object GetValue(object model);
	}

	private class SingleBinding : IBinding
	{
		public object Target { get; private set; }
		private readonly PropertyReader<T> _source;

		public SingleBinding(object target, PropertyReader<T> source)
		{
			Target = target;
			_source = source;
		}

		public object GetValue(object model)
		{
			return _source.GetValue(model);
		}
	}

	private class FormattedBinding : IBinding
	{
		public object Target { get; private set; }
		private readonly string _format;
		private readonly PropertyReader<T>[] _sources;

		public FormattedBinding(object target, PropertyReader<T>[] source, string format)
		{
			Target = target;
			_sources = source;
			_format = format;
		}

		public object GetValue(object model)
		{
			string[] values = new string[_sources.Length];

			for (int i = 0; i < _sources.Length; i++)
				values[i] = _sources[i].GetValue(model).ToString();

			return String.Format(_format, values);
		}
	}

	private string _prefix = "";
	private readonly List<IBinding> _bindings;

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

	public SubscriptionBinding(MessageBroker broker)
	{
		_bindings = new List<IBinding>();
		broker.Sub<T>(Update);
	}

	public SubscriptionBinding<T> Prefix(string prefix)
	{
		_prefix = prefix;
		return this;
	}

	public SubscriptionBinding<T> Bind(object element, string property)
	{
		if (element == null)
			return this;

		_bindings.Add(new SingleBinding(element, new PropertyReader<T>(_prefix + property)));

		return this;
	}

	public SubscriptionBinding<T> Format(object element, string format, params string[] properties)
	{
		if (element == null)
			return this;

		PropertyReader<T>[] sources = new PropertyReader<T>[properties.Length];

		for (int i = 0; i < properties.Length; i++)
			sources[i] = new PropertyReader<T>(_prefix + properties[i]);

		_bindings.Add(new FormattedBinding(element, sources, format));

		return this;
	}

	public void Init()
	{
	}
}
