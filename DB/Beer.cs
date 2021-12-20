using System;
using System.Collections.Generic;

#nullable disable

namespace DB
{
    public partial class Beer
    {
        public int BeerId { get; set; }
        public string Name { get; set; }
        public int BrandId { get; set; }

        public virtual Brand Brand { get; set; }
    }
}
