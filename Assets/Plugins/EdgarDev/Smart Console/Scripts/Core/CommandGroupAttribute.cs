/*
  Package Name: Smart Console
  Version: 2.3.0
  Author: EdgarDev
  Unity Asset Profile: https://assetstore.unity.com/publishers/64126
  Date: 2023-06-14
  Script Name: CommandGroupAttribute.cs

  Description:
  This class implements the CommandGroupAttribute attribute which can be applied
  to a class to indicate that commands inside of it should be registered in a group.
*/

using System;

namespace ED.SC
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public class CommandGroupAttribute : Attribute
	{
		public string Name;

		public CommandGroupAttribute() : this("")
		{
		}

		public CommandGroupAttribute(string name)
		{
			Name = name;
		}

		public bool HasName() => !string.IsNullOrEmpty(Name);
	}
}