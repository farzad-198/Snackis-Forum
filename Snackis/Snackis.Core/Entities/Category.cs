using System;
using System.Collections.Generic;
using System.Text;

namespace Snackis.Core.Entities
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
        public ICollection<Topic> Topics { get; set; }
            = new List<Topic>();
    }
}
