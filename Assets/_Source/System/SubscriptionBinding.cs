using System.CodeDom;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.UI;

public class SubscriptionBinding<T>
{
	private class Binding
	{
		public ILayoutElement Target;
		public FieldInfo Source;
	}

	private List<Binding> _bindings;

	public void Update(T model)
	{
		for (int i = 0; i < _bindings.Count; i++)
		{
			if (_bindings[i].Target is Text)
				((Text) _bindings[i].Target).text = _bindings[i].Source.GetValue(model).ToString();
			else if (_bindings[i].Target is Image)
				((Image) _bindings[i].Target).sprite = (Sprite) _bindings[i].Source.GetValue(model);
		}
	}

	public SubscriptionBinding(MessageBroker broker)
	{
		_bindings = new List<Binding>();
		broker.Sub<T>(Update);
	}

	public SubscriptionBinding<T> Bind(ILayoutElement element, string property)
	{
		if (element == null)
			return this;

		_bindings.Add(new Binding()
		{
			Target = element,
			Source = typeof(T).GetField(property)
		});

		return this;
	}

	public SubscriptionBinding<T> Translate(Text element, string key)
	{
		return this;
	}

	public void Init()
	{
	}
}
