using System;
using System.Collections.Generic;
using System.Text;
using Zoo.Data.Models;

namespace Zoo.Service.DTO
{
    public class CreateAnimalDTO
    {
        public string Name { get; set; }

        public string Species { get; set; }
    }
}
