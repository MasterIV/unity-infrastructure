using System;
using System.CodeDom;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.UI;

public class PropertyReader<T>
{
	private interface IReaderSource
	{
		object GetValue(object model);
		Type GetValueType();
	}

	private class FieldReaderSource : IReaderSource
	{
		private readonly FieldInfo _source;

		public FieldReaderSource(FieldInfo source)
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

	private class PropertyReaderSource : IReaderSource
	{
		private readonly PropertyInfo _source;

		public PropertyReaderSource(PropertyInfo source)
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

	private class MethodReaderSource : IReaderSource
	{
		private readonly MethodInfo _source;

		public MethodReaderSource(MethodInfo source)
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

	private readonly IReaderSource[] _source;

	public PropertyReader(string source)
	{
		var path = source.Split('.');
		var type = typeof(T);
		_source = new IReaderSource[path.Length];

		for (int i = 0; i < path.Length; i++)
		{
			_source[i] = InitSource(type, path[i]);
			type = _source[i].GetValueType();
		}
	}

	private IReaderSource InitSource(Type type, string source)
	{
		var field = type.GetField(source);
		if (field != null)
			return new FieldReaderSource(field);

		var property = type.GetProperty(source);
		if (property != null)
			return new PropertyReaderSource(property);

		var method = type.GetMethod(source);
		if (method != null)
			return new MethodReaderSource(method);

		throw new Exception("Source not found: "+source);
	}

	public object GetValue(object model)
	{
		for (int i = 0; i < _source.Length; i++)
			model = _source[i].GetValue(model);

		return model;
	}
}
