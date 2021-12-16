using FinalProjectLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectClient.Controllers
{
    public class UserCredsController : Controller
    {
        public async Task<ActionResult> Index()
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                var response = await HttpClientHelper.client.GetAsync(HttpClientHelper.baseUrl + "/api/UserCreds");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var users = JsonConvert.DeserializeObject<IEnumerable<UserCred>>(json);
                    return View(users);
                }
                else
                {
                    Console.WriteLine("Internal Server error");
                    return NotFound();
                }
            }
            else
            {
                return Redirect("/");
            }
        }

        public async Task<ActionResult> Details(int id)
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                var response = await HttpClientHelper.client.GetAsync(HttpClientHelper.baseUrl + "/api/UserCreds/" + id);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    var user = JsonConvert.DeserializeObject<UserCred>(json);
                    return View(user);
                }
                else
                {
                    Console.WriteLine("Internal Server error");
                    return NotFound();
                }
            }
            else
            {
                return Redirect("/");
            }
        }

        public ActionResult Signin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Signin(UserCred model)
        {
            if (ModelState.IsValid)
            {
                var response = await HttpClientHelper.client.GetAsync(HttpClientHelper.baseUrl + "/api/UserCreds/" + model.Email+"/"+model.Password);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    var user = JsonConvert.DeserializeObject<UserCred>(json);

                    HttpContext.Session.SetInt32("UserId",user.UserId);
                    HttpContext.Session.SetString("Email", user.Email);

                    //Console.WriteLine("Id:"+HttpContext.Session.GetInt32("UserId"));
                    //Console.WriteLine("Email:" + HttpContext.Session.GetString("Email"));

                    return Redirect("/Home/Index");
                }
                else
                {
                    ModelState.AddModelError("","Cannot find user with this Email and Password");
                    return View(model);
                }
            }
            else
            {
                ModelState.AddModelError("", "Email and Password are required");
                return View(model);
            }
        }

        public ActionResult Signout()
        {
            if (HttpContext.Session.Get("UserId") != null)
            {
                HttpContext.Session.Remove("UserId");
                HttpContext.Session.Remove("Email");
            }

            return Redirect("/");
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserCred model)
        {
            if (ModelState.IsValid)
            {
                var json = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await HttpClientHelper.client.PostAsync(HttpClientHelper.baseUrl + "/api/UserCreds", content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"added resource at {response.Headers.Location}");
                    json = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("user has been inserted " + json);

                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Email must be unique");
                    return View(model);
                }
            }
            else
            {
                ModelState.AddModelError("", "Email and Password are required");
                return View(model);
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                var response = await HttpClientHelper.client.GetAsync(HttpClientHelper.baseUrl + "/api/UserCreds/" + id);
                if (response.IsSuccessStatusCode)
                {

                    var json = await response.Content.ReadAsStringAsync();
                    var user = JsonConvert.DeserializeObject<UserCred>(json);

                    if (HttpContext.Session.GetInt32("UserId") == user.UserId)
                    {
                        return View(user);
                    }
                    else
                    {
                        //prevent edit access if not owner
                        return NotFound();
                    }
                    
                }
                else
                {
                    Console.WriteLine("Internal Server error");
                    return NotFound();
                }
            }
            else
            {
                return Redirect("/");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id,UserCred model)
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                var response = await HttpClientHelper.client.GetAsync(HttpClientHelper.baseUrl + "/api/UserCreds/" + id);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    var user = JsonConvert.DeserializeObject<UserCred>(json);

                    if (HttpContext.Session.GetInt32("UserId") == user.UserId)
                    {
                        if (ModelState.IsValid)
                        {
                            json = JsonConvert.SerializeObject(model);
                            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                            response = await HttpClientHelper.client.PutAsync(HttpClientHelper.baseUrl + "/api/UserCreds/" + id, content);

                            if (response.IsSuccessStatusCode)
                            {
                                Console.WriteLine($"edited resource at {response.Headers.Location}");
                                json = await response.Content.ReadAsStringAsync();
                                Console.WriteLine("user has been edited " + json);

                                return RedirectToAction("Index");
                            }
                            else
                            {
                                ModelState.AddModelError("", "Internal Error");
                                return View(model);
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Please fill all required fields");
                            return View(model);
                        }
                    }
                    else
                    {
                        //prevent edit access if not owner
                        return NotFound();
                    }

                }
                else
                {
                    Console.WriteLine("Internal Server error");
                    return NotFound();
                }

            }
            else
            {
                return Redirect("/");
            }
        }

        public async Task<ActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                var response = await HttpClientHelper.client.GetAsync(HttpClientHelper.baseUrl + "/api/UserCreds/" + id);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    var user = JsonConvert.DeserializeObject<UserCred>(json);

                    if (HttpContext.Session.GetInt32("UserId") == user.UserId)
                    {
                        response = await HttpClientHelper.client.DeleteAsync(HttpClientHelper.baseUrl + "/api/UserCreds/" + id);
                        if (response.IsSuccessStatusCode)
                        {
                            return Signout();
                        }
                        else
                        {
                            Console.WriteLine("Internal Server error");
                            return NotFound();
                        }
                    }
                    else
                    {
                        //prevent delete access if not owner
                        return NotFound();
                    }
                }
                else
                {
                    Console.WriteLine("Internal Server error");
                    return NotFound();
                }
            }
            else
            {
                return Redirect("/");
            }
        }
    }
}
