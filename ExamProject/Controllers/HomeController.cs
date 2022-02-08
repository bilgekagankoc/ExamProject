using ExamProject.Models;
using ExamProject.SQLite;
using ExamProject.Wired;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ExamProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        ExamDbCRUD ex = new ExamDbCRUD();
        GetWired gw = new GetWired();
        public IActionResult Index()
        {
            if (!(ExamDb.connection.State == ConnectionState.Open))
            {
                ExamDb.connection.Open();
            }
            ex.VeriTabanıKontrol();
            bool sonuc = ex.YaziEklendiMi();
            if (!sonuc)
            {
                gw.getWiredText();
            }

                return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public bool KullaniciKontrol(UserModel sqlUser , UserModel user)
        {
            if((sqlUser.KullaniciAdi == user.KullaniciAdi) && (sqlUser.Sifre == user.Sifre))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [HttpPost]
        public IActionResult UserControl(UserModel user)
        {
            
            if (!(ExamDb.connection.State==ConnectionState.Open))
            {
                ExamDb.connection.Open();
            }
            
            var sonuc = ex.KullaniciBilgileri(user);

            var kullaniciKontrol = KullaniciKontrol(sonuc, user);
            if (kullaniciKontrol)
            {
                TempData["girisBasarilimi"] = true;
                if (sonuc.RolTipi == 1)
                {
                    ExamDb.connection.Close();
                    return RedirectToAction("CreateExam");
                }
                else
                {
                    ExamDb.connection.Close();
                    return null;
                }
            }
            else
            {
                TempData["girisBasarilimi"] = false;
                ExamDb.connection.Close();
                return  RedirectToAction("Index");
            }
            
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult CreateExam()
        {
            SoruTextModels stms = new SoruTextModels();
            stms.soruTextModels = ex.GetTextDb();
            return View(stms);
        }

        [HttpPost]
        public IActionResult SaveExam(SoruTextModels model)
        {
            ex.SaveExam(model);
            return RedirectToAction("CreateExam");
        }

        public IActionResult ExamList()
        {
            var asd = ex.GetExamDb();
            return View();
        }
    }
}
