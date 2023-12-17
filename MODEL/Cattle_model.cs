using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL
{
    public class Cattle_model
    {
        public int CattleId { get; set; }
        public int UserId { get; set; }
        public string Animal { get; set; }
        public string AnimalBreed { get; set; }
        public int Age { get; set; }
        public decimal Weight { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
    }
}
