using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System.Diagnostics;
using System.Net;
using TodoAPI_JwtAuth.MVC.Filters;
using TodoAPI_JwtAuth.MVC.Managers;
using TodoAPI_JwtAuth.MVC.Models;

namespace TodoAPI_JwtAuth.MVC.Controllers
{
    public class HomeController : Controller
    {
        private ITodoService _todoService;

        public HomeController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        [AuthFilter]
        public IActionResult Index()
        {
            RestResponse<List<Models.Todo>> response = _todoService.List();

            if (response.IsSuccessful)
            {
                return View(response.Data);
            }

            HttpContext.Session.Clear();

            return RedirectToAction(nameof(Login));
        }
        [AuthFilter]
        public IActionResult Create()
        {
            return View();
        }

        [AuthFilter]
        [HttpPost]
        public IActionResult Create(TodoCreate model)
        {
            if (ModelState.IsValid)
            {
            RestResponse<Models.Todo> response=_todoService.Create(model);
                if (response.StatusCode != HttpStatusCode.Created)
                {
                    ModelState.AddModelError("", "Servis Erişim Hatası.");

                    return View(model); 
                }

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }


        [AuthFilter]
        public IActionResult Edit(int id)
        {
            RestResponse<Models.Todo> response=_todoService.GetById(id);
            if (response.IsSuccessful == false)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(response.Data);
        }

        [AuthFilter]
        [HttpPost]
        public IActionResult Edit(int id,Models.Todo model)
        {
            if (ModelState.IsValid)
            {
                RestResponse<Models.Todo> response = _todoService.Update(id, model);
                if (response.IsSuccessful)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Güncelleme Yapılamadı ! (Servis Hatası).");
                }
            }
            return View(model);
        }

        [AuthFilter]
        public IActionResult Delete(int id)
        {
            RestResponse response = _todoService.Delete(id);
            if (response.StatusCode==HttpStatusCode.NotFound)
            {
                TempData["result"] = "Kayıt bulunmadı.";
            }
            else
            {
                TempData["result"] = "Kayıt silindi.";
            }
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Login(SignInModel model) 
        {
            if (ModelState.IsValid)
            {
                //gelen modeldeki bilgiler ile api ye istek yapıyoruz
                //token almaya çalışıyoruz.

                RestResponse<string> response=_todoService.Authenticate(model);
                if (response.StatusCode==HttpStatusCode.BadRequest)
                {
                    ModelState.AddModelError("", "Kullanıcı adı ya da şifre hatalı !");
                }
                else
                {
                    if (response.IsSuccessful)
                    {
                        string token = response.Data;
                        HttpContext.Session.SetString("token", token);

                        return RedirectToAction(nameof(Index)); 
                    }
                    else
                    {
                        ModelState.AddModelError("", "API Servisi hatası.");
                    }
                }
            }

            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Login));
        }


        public IActionResult Privacy()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}