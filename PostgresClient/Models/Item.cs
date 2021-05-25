using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PostgresClient.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string LongDescription { get; set; }
        public byte[] Image { get; set; }
    }
}
