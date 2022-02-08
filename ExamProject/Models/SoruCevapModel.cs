using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamProject.Models
{
    public class SoruCevapModel : BaseModel
    {
        public int SoruTextId { get; set; }
        public List<SoruModel> SoruModels { get; set; }

    }
}
