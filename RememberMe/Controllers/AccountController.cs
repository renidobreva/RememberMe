using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;

public class AccountController : Controller
{
    private const string CookieName = "LastUsername"; // 🔥 Име на бисквитката

    [HttpGet]
    public IActionResult Login()
    {
        Console.WriteLine("🔎 Зареждане на Login страница...");

        if (Request.Cookies.TryGetValue(CookieName, out string username))
        {
            Console.WriteLine($"✅ Намерена бисквитка: {username}");
            ViewData["Username"] = username;
        }
        else
        {
            Console.WriteLine("❌ Бисквитката НЕ е намерена!");
            ViewData["Username"] = "";
        }

        return View();
    }

    [HttpPost]
    public IActionResult Login(string Username, string RememberMe)
    {
        Console.WriteLine($"📥 Получено име: {Username}, RememberMe: {RememberMe}");

        bool remember = !string.IsNullOrEmpty(RememberMe) && RememberMe == "on"; // 🔥 Проверяваме правилно чекбокса

        if (!string.IsNullOrEmpty(Username))
        {
            if (remember)
            {
                Console.WriteLine("💾 Записваме бисквитка...");

                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.UtcNow.AddDays(30), // 🔥 Важно: Използвай UTC!
                    HttpOnly = false, // ❗ Временно изключено, за да видим в DevTools
                    Secure = false,
                    SameSite = SameSiteMode.Lax
                };

                Response.Cookies.Append(CookieName, Username, cookieOptions);
                Console.WriteLine("✅ Бисквитката е записана успешно!");
            }
            else
            {
                Console.WriteLine("🗑 Изтриваме бисквитката!");
                Response.Cookies.Delete(CookieName);
            }

            return RedirectToAction("Login"); // 🔥 Пренасочваме обратно към Login
        }

        ViewBag.Error = "Моля, въведете потребителско име.";
        return View();
    }
}
