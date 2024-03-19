using System.ComponentModel.DataAnnotations;

namespace Assignment_1_DotNet.Entities
{
    public class KeyValue
    {
        [Key]
        public int Id { get; set; }
        public required string Key { get; set; }
        public required string Value { get; set; }
    }
}
