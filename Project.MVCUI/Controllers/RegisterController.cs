using Project.BLL.Repositories.ConcRep;
using Project.COMMON.Tools;
using Project.ENTITIES.Models;
using Project.VM.PureVMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;

namespace Project.MVCUI.Controllers
{
    public class RegisterController : Controller
    {
        AppUserProfileRepository _proRep;
        AppUserRepository _apRep;

        public RegisterController()
        {
            _proRep = new AppUserProfileRepository();
            _apRep = new AppUserRepository();
        }

        public ActionResult RegisterNow()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterNow(AppUserVM appUser,AppUserProfileVM profile)
        {
            if (_apRep.Any(x => x.UserName == appUser.UserName))
            {
                ViewBag.ZatenVar = "Kullanıcı ismi daha önce alınmıştır";
                return View();
            }
            else if (_apRep.Any(x => x.Email == appUser.Email))
            {
                ViewBag.ZatenVar = "Email kayıtlı bir kullanıcıya ait";
                return View();
            }

            appUser.Password = DantexCrypt.Crypt(appUser.Password);  // Şifreyi Kriptoladık

            AppUser domainUser = new AppUser
			{
                UserName = appUser.UserName,
                Email = appUser.Email,
                Password = appUser.Password,
            };
            _apRep.Add(domainUser); // Kullanıcı yanında profil eklemek istersek öncelıkle Repository'nin bu metodunu AppUser için calistirimasınız Cunku AppUser'in ID'si ilk basta olusmalı..Cünkübizim kurdugumuz birebir ilişkide AppUser zorunlu olan alan,Profile ise opsiyonel alandır...Dolayısyla Profile'in ID'si identity degildir.. o yüzden Profile eklenecekken ID belirlemek zorundayiz(manuel)...Birebir ilişki oldugundan dolayi Profile'in ID'si ile AppUser'in ID'si tutmak zorundadır...İlk başta AppUse'in ID'si SaveChanges ile olusurz (Repository sayesinde) ki sonra Profile'i rahatca ekleyebilelim 

            string gonderilecekMail = "Tebrikler... Hesabınız oluşturulmuştur... Hesabınızı ative etmek için http://localhost:52006/Register/Activation/" + domainUser.ActivationCode + " linke tıklayabilirsiniz";

            MailService.Send(appUser.Email, body: gonderilecekMail, subject: "Hesap Aktivasyonu! ! !");

            if (!string.IsNullOrEmpty(profile.FirstName.Trim()) || !string.IsNullOrEmpty(profile.LastName.Trim()))
            {
                AppUserProfile domainProfile = new AppUserProfile
                {
                    ID = domainUser.ID,
                    FirstName = profile.FirstName,
                    LastName = profile.LastName,
                };
            }

            return View("RegisterOK");		
            
        }

        public ActionResult RegisterOK()
        {
            return View();
        }

        public ActionResult Activation(Guid id)
        {
            AppUser aktifEdilecek = _apRep.FirstOrDefault(x => x.ActivationCode == id);
            if (aktifEdilecek != null)
            {
                aktifEdilecek.Active = true;
                _apRep.Update(aktifEdilecek);
                TempData["HesapAktifMi"] = "Hesap Aktif Hale Getirildi";
                return RedirectToAction("Login","Home");

            }
            // Şüpheli Aktiviteler için
            TempData["HesapAktifMi"] = "Hesabınız Bulunamadı";
            return RedirectToAction("Login", "Home");
                
        }
    }
}