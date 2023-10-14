using Microsoft.AspNetCore.Mvc;

using System.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using JewelryStore.Models;
using Microsoft.Data.SqlClient;
using System.Text.RegularExpressions;

namespace JewelryStore.Controllers
{
    public class Access : Controller
    {
        public IActionResult Registration()
        {
            //ClaimsPrincipal claimUser = HttpContext.User;

            //if (claimUser.Identity.IsAuthenticated)
            //{
            //    return RedirectToAction("Login", "Access");
            //}

            return View("Login");
        }
        [HttpPost]
        public async Task<IActionResult> Registration(Users modelRegistration)
        {
            if (modelRegistration.login == null || modelRegistration.password == null || modelRegistration.surname == null ||
                modelRegistration.firstname == null || modelRegistration.email == null)
            {
                ViewData["ValidateMessage"] = "Не были введены все необходимые поля";
                return View("Login");
            }
            SqlConnection myConnection = new SqlConnection("Server=(localdb)\\mssqllocaldb;Database=JewelryStore.Data;Trusted_Connection=True;MultipleActiveResultSets=true");
            // Проверка на существование пользователя с введенной электронной почтой
            myConnection.Open();
            string selectquery = $"select * from users where email = '{modelRegistration.email}'";
            SqlDataAdapter adpt = new SqlDataAdapter(selectquery, myConnection);
            DataTable table = new DataTable();
            adpt.Fill(table);
            myConnection.Close();
            if (table.Rows.Count > 0)
            {
                ViewData["ValidateMessage"] = "Пользователь с данной электронной почтой уже существует";
                return View("Login");
            }
            // Проверка на существование пользователя с введенным логином
            myConnection.Open();
            selectquery = $"select * from users where login = '{modelRegistration.login}'";
            adpt = new SqlDataAdapter(selectquery, myConnection);
            table = new DataTable();
            adpt.Fill(table);
            myConnection.Close();
            if (table.Rows.Count > 0)
            {
                ViewData["ValidateMessage"] = "Пользователь с данным логином уже существует";
                return View("Login");
            }
            // Проверка на корректность логина
            Regex lpCheck = new Regex(@"^\w{5,15}$");
            MatchCollection matches = lpCheck.Matches(modelRegistration.login);
            if (matches.Count == 0)
            {
                ViewData["ValidateMessage"] = "Некоррктно введен логин";
                return View("Login");
            }
            // Проверка на корректность пароля
            matches = lpCheck.Matches(modelRegistration.password);
            if (matches.Count == 0)
            {
                ViewData["ValidateMessage"] = "Некорректно введен пароль";
                return View("Login");
            }
            // Проверка на корректность фамилии
            Regex nameCheck = new Regex(@"^[A-ЯЁ][а-яё]+$");
            matches = nameCheck.Matches(modelRegistration.surname);
            if (matches.Count == 0)
            {
                ViewData["ValidateMessage"] = "Неверно введена фамилия";
                return View("Login");
            }
            // Проверка на корректность имени
            matches = nameCheck.Matches(modelRegistration.firstname);
            if (matches.Count == 0)
            {
                ViewData["ValidateMessage"] = "Неверно введено имя";
                return View("Login");
            }
            // Проверка на наличие и корректность отчества
            if (modelRegistration.patronymic == null)
            {
                modelRegistration.patronymic = "NULL";
            }
            else
            {
                matches = nameCheck.Matches(modelRegistration.patronymic);
                if (matches.Count == 0)
                {
                    ViewData["ValidateMessage"] = "Неверно введено отчество";
                    return View("Login");
                }
                else
                {
                    modelRegistration.patronymic = $"'{modelRegistration.patronymic}'";
                }
            }
            // Проверка на заполнение и корректность номера телефона
            if (modelRegistration.phone == null)
            {
                modelRegistration.phone = "NULL";
            }
            else
            {
                Regex phoneCheck = new Regex(@"^\+7-\d{3}-\d{3}-\d{2}-\d{2}$");
                matches = phoneCheck.Matches(modelRegistration.phone);
                if (matches.Count == 0)
                {
                    ViewData["ValidateMessage"] = "Неверно введен номер телефона";
                    return View("Login");
                }
                else
                {
                    modelRegistration.phone = $"'{modelRegistration.phone}'";
                }
            }
            myConnection.Open();
            // Добавление пользователя в базу данных
            selectquery = $"insert into Users (login, password, RolesId, surname, firstname, patronymic, email, phone) " +
                $"values ('{modelRegistration.login}', '{modelRegistration.password}', 3, '{modelRegistration.surname}', '{modelRegistration.firstname}', {modelRegistration.patronymic}, '{modelRegistration.email}', {modelRegistration.phone})";
            adpt = new SqlDataAdapter(selectquery, myConnection);
            table = new DataTable();
            adpt.Fill(table);
            myConnection.Close();

            List<Claim> claims = new List<Claim>(){
            new Claim(ClaimTypes.NameIdentifier, modelRegistration.login),
            new Claim("OtherProperties", "Example Role")
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            AuthenticationProperties properties = new AuthenticationProperties()
            {
                AllowRefresh = true
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), properties);

            ViewData["ValidateMessage"] = "Пользователь был успешно создан";
            return View("Login");
            }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(Users modelLogin)
        {
            // Проверка на заполнение полей
            if (modelLogin.login == null && modelLogin.password == null)
            {
                ViewData["ValidateMessage"] = "Ошибка. Не были введены данные";
            }
            else
            {
                // Проверка на существование пользователя с введенным логином
                SqlConnection myConnection = new SqlConnection("Server=(localdb)\\mssqllocaldb;Database=JewelryStore.Data;Trusted_Connection=True;MultipleActiveResultSets=true");
                //SqlConnection myConnection = new SqlConnection("Server=.\\SQLEXPRESS;Database=JewelryStore.Data;Trusted_Connection=True;MultipleActiveResultSets=true; TrustServerCertificate=true");
                myConnection.Open();
                string selectquery = $"select login, password, RolesId from Users where login = '{modelLogin.login}'";
                SqlDataAdapter adpt = new SqlDataAdapter(selectquery, myConnection);
                DataTable table = new DataTable();
                adpt.Fill(table);
                myConnection.Close();
                if (table.Rows.Count > 0)
                {
                    // Проверка на совпадение введенных логина и пароля с данными из бд
                    // Вход для администатора и сотрудника
                    myConnection.Open();
                    selectquery = $"select login, password, RolesId from Users where login = '{modelLogin.login}' and password = '{modelLogin.password}' and RolesId between 1 and 2";
                    adpt = new SqlDataAdapter(selectquery, myConnection);
                    table = new DataTable();
                    adpt.Fill(table);
                    myConnection.Close();
                    if (table.Rows.Count > 0)
                    {
                        List<Claim> claims = new List<Claim>(){
                        new Claim(ClaimTypes.NameIdentifier, modelLogin.login),
                        new Claim("OtherProperties", "Example Role")
                        };

                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                            CookieAuthenticationDefaults.AuthenticationScheme);

                        AuthenticationProperties properties = new AuthenticationProperties()
                        {
                            AllowRefresh = true
                        };

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity), properties);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewData["ValidateMessage"] = "Неверный пароль";
                    }
                    // Вход для клиента
                    myConnection.Open();
                    selectquery = $"select login, password, RolesId from Users where login = '{modelLogin.login}' and password = '{modelLogin.password}' and RolesId = 3";
                    adpt = new SqlDataAdapter(selectquery, myConnection);
                    table = new DataTable();
                    adpt.Fill(table);
                    myConnection.Close();
                    if (table.Rows.Count > 0)
                    {
                        List<Claim> claims = new List<Claim>(){
                        new Claim(ClaimTypes.NameIdentifier, modelLogin.login),
                        new Claim("OtherProperties", "Example Role")
                        };

                        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                            CookieAuthenticationDefaults.AuthenticationScheme);

                        AuthenticationProperties properties = new AuthenticationProperties()
                        {
                            AllowRefresh = true
                        };

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity), properties);

                        return RedirectToAction("Index2", "Home");
                    }
                    else
                    {
                        ViewData["ValidateMessage"] = "Неверный пароль";
                    }
                }
                else
                {
                    ViewData["ValidateMessage"] = "Пользователя с таким логином не существует";
                }
            }
            return View();
        }
    }
}