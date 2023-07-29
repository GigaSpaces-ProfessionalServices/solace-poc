using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Piper.Common
{
    public class RecordDto
    {
        string? Opr { get; set; }
        string? Type { get; set; }

        string? SpaceIdName { get; set; }

        List<Field>? payload { get; set; }
        public RecordDto()
        {
        }
    }

    public class Field
    {
        string? ColumnName { get; set; }
        string? ColumnType { get; set; }
        JValue? Value { get; set; }
        public Field() { }
    }
}