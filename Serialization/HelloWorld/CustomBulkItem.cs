using System;
using ProtoBuf;

namespace GigaSpaces.Examples.HelloWorld
{
    [ProtoContract(SkipConstructor = true)]
    public class CustomBulkItem
	{
        [ProtoMember(1)]
        public object items { get; set; }
        [ProtoMember(2)]
        public string operation { get; set; }
        [ProtoMember(3)] 
        public string spaceId { get; set; }
        [ProtoMember(4)]
        public string typeName { get; set; }
        //  public IDictionary property { get; set; }

      /*  public override string ToString()
        {
            return String.Concat(items, operation, spaceId, typeName);
        }*/

    }
}
