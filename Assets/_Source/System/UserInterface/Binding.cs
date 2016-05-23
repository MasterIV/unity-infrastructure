using System;

public interface IBinding
{
	object Target { get; }
	object GetValue(object model);
}

public class SingleBinding : IBinding
{
	public object Target { get; private set; }
	private readonly IPropertyReader _source;

	public SingleBinding(object target, IPropertyReader source)
	{
		Target = target;
		_source = source;
	}

	public object GetValue(object model)
	{
		return _source.GetValue(model);
	}
}

public class FormattedBinding : IBinding
{
	public object Target { get; private set; }
	private readonly string _format;
	private readonly IPropertyReader[] _sources;

	public FormattedBinding(object target, IPropertyReader[] source, string format)
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
