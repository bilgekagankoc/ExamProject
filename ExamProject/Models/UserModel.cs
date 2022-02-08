using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamProject.Models
{

    public class UserModel : BaseModel
    {
        public string KullaniciAdi { get; set; }
        public string Sifre { get; set; }
        public int RolTipi { get; set; }
    }
}
