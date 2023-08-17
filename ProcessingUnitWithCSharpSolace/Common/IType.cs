using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piper.Common
{
    public interface IType
    {
        string serializeToJsonString();
        IType deSerializeFromJsonString(string json);
    }
}
