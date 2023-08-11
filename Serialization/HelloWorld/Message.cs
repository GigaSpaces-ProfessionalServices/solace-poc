using System;
using GigaSpaces.Core.Metadata;

namespace GigaSpaces.Examples.HelloWorld
{
	/// <summary>
	/// A simple message object that is written to the space.
	/// Note that the class is not required to inherit from any base class, or 
	/// implement an interface.
	/// </summary>
	public class Message
	{
		/// <summary>
		/// Each object in the space needs an id property (which can be auto generated if needed), 
		/// in this case the text property is the id property.
		/// </summary>
		[SpaceID]
		public string Text { get; set; }
	}
}
