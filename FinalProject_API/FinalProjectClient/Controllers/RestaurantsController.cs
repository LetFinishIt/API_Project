using FinalProjectLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectClient.Controllers
{
    public class RestaurantsController : Controller
    {
        // GET: RestaurantsController
        public async Task<ActionResult> Index()
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                var response = await HttpClientHelper.client.GetAsync(HttpClientHelper.baseUrl + "/api/Restaurants");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var restaurants = JsonConvert.DeserializeObject<IEnumerable<Restaurant>>(json);

                    return View(restaurants);
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

        // GET: RestaurantsController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                var response = await HttpClientHelper.client.GetAsync(HttpClientHelper.baseUrl + "/api/Restaurants/" + id);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    var restaurant = JsonConvert.DeserializeObject<Restaurant>(json);
                    return View(restaurant);
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

        // GET: RestaurantsController/Create
        public ActionResult Create()
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                return View();
            }
            else
            {
                return Redirect("/");
            }
        }

        // POST: RestaurantsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Restaurant model)
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                if (ModelState.IsValid)
                {
                    model.OwnerUserId = (int)HttpContext.Session.GetInt32("UserId");

                    var json = JsonConvert.SerializeObject(model);
                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await HttpClientHelper.client.PostAsync(HttpClientHelper.baseUrl + "/api/Restaurants", content);

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"added resource at {response.Headers.Location}");
                        json = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("restaurant has been inserted " + json);

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
                return Redirect("/");
            }
        }

        // GET: RestaurantsController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                var response = await HttpClientHelper.client.GetAsync(HttpClientHelper.baseUrl + "/api/Restaurants/" + id);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    var restaurant = JsonConvert.DeserializeObject<Restaurant>(json);

                    if (HttpContext.Session.GetInt32("UserId") == restaurant.OwnerUserId)
                    {
                        return View(restaurant);
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

        // POST: RestaurantsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Restaurant model, IFormFile imageFile)
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                var response = await HttpClientHelper.client.GetAsync(HttpClientHelper.baseUrl + "/api/Restaurants/" + id);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    var restaurant = JsonConvert.DeserializeObject<Restaurant>(json);

                    if (HttpContext.Session.GetInt32("UserId") == restaurant.OwnerUserId)
                    {
                        if (ModelState.IsValid)
                        {
                            model.OwnerUserId = (int)HttpContext.Session.GetInt32("UserId");

                            json = JsonConvert.SerializeObject(model);
                            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                            response = await HttpClientHelper.client.PutAsync(HttpClientHelper.baseUrl + "/api/Restaurants/" + id, content);

                            if (response.IsSuccessStatusCode)
                            {
                                Console.WriteLine($"edited resource at {response.Headers.Location}");
                                json = await response.Content.ReadAsStringAsync();
                                Console.WriteLine("restaurant has been edited " + json);

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

        // POST: RestaurantsController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {

            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                var response = await HttpClientHelper.client.GetAsync(HttpClientHelper.baseUrl + "/api/Restaurants/" + id);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    var restaurant = JsonConvert.DeserializeObject<Restaurant>(json);

                    if (HttpContext.Session.GetInt32("UserId") == restaurant.OwnerUserId)
                    {
                        response = await HttpClientHelper.client.DeleteAsync(HttpClientHelper.baseUrl + "/api/Restaurants/" + id);
                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Index");
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
