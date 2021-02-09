using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Zoo.Data.Abstract
{
    public class Entity
    {
        [DisplayName("Created on")]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
