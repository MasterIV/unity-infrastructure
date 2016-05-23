using System;
using System.CodeDom;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.UI;

public class PropertyBinding<T>
{
	private interface IBindingSource
	{
		object GetValue(object model);
		Type GetValueType();
	}

	private class FieldBindingSource : IBindingSource
	{
		private readonly FieldInfo _source;

		public FieldBindingSource(FieldInfo source)
		{
			_source = source;
		}

		public object GetValue(object model)
		{
			return _source.GetValue(model);
		}

		public Type GetValueType()
		{
			return _source.FieldType;
		}
	}

	private class PropertyBindingSource : IBindingSource
	{
		private readonly PropertyInfo _source;

		public PropertyBindingSource(PropertyInfo source)
		{
			_source = source;
		}

		public object GetValue(object model)
		{
			return _source.GetValue(model, null);
		}

		public Type GetValueType()
		{
			return _source.PropertyType;
		}
	}

	private class MethodBindingSource : IBindingSource
	{
		private readonly MethodInfo _source;

		public MethodBindingSource(MethodInfo source)
		{
			_source = source;
		}

		public object GetValue(object model)
		{
			return _source.Invoke(model, null);
		}

		public Type GetValueType()
		{
			return _source.ReturnType;
		}
	}

	private readonly IBindingSource[] _source;

	public PropertyBinding(string source)
	{
		var path = source.Split('.');
		var type = typeof(T);
		_source = new IBindingSource[path.Length];

		for (int i = 0; i < path.Length; i++)
		{
			_source[i] = InitSource(type, path[i]);
			type = _source[i].GetValueType();
		}
	}

	private IBindingSource InitSource(Type type, string source)
	{
		var field = type.GetField(source);
		if (field != null)
			return new FieldBindingSource(field);

		var property = type.GetProperty(source);
		if (property != null)
			return new PropertyBindingSource(property);

		var method = type.GetMethod(source);
		if (method != null)
			return new MethodBindingSource(method);

		throw new Exception("Source not found: "+source);
	}

	public object GetValue(object model)
	{
		for (int i = 0; i < _source.Length; i++)
			model = _source[i].GetValue(model);

		return model;
	}
}
