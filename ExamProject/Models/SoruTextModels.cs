using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamProject.Models
{
    public class SoruTextModels
    {
        public SoruTextModels()
        {
            soruTextModels = new List<SoruTextModel>();
        }
        public List<SoruTextModel> soruTextModels { get; set; }
        public SoruCevapModel? SoruCevapModel { get; set; }
        public int SoruTextId { get; set; }

    }
}
