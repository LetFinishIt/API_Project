using FinalProjectLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FinalProjectClient.Controllers
{
    public class BookingSlotsController : Controller
    {
        // GET: BookingSlotsController
        public async Task<ActionResult> Index()
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                var response = await HttpClientHelper.client.GetAsync(HttpClientHelper.baseUrl + "/api/BookingSlots");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var bookingSlots = JsonConvert.DeserializeObject<IEnumerable<BookingSlot>>(json);
                    return View(bookingSlots);
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

        // GET: BookingSlotsController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                var response = await HttpClientHelper.client.GetAsync(HttpClientHelper.baseUrl + "/api/BookingSlots/" + id);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    var bookingSlot = JsonConvert.DeserializeObject<BookingSlot>(json);
                    return View(bookingSlot);
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

        // GET: BookingSlotsController/Create
        public async Task<ActionResult> Create()
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                var response = await HttpClientHelper.client.GetAsync(HttpClientHelper.baseUrl + "/api/Restaurants");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var restaurants = JsonConvert.DeserializeObject<IEnumerable<Restaurant>>(json);

                    BookingSlot emptyBookingSlot = new BookingSlot
                    {
                        RestaurantsDropdown = restaurants
                        .Where(r => r.OwnerUserId == HttpContext.Session.GetInt32("UserId"))
                    };

                    return View(emptyBookingSlot);
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

        // POST: BookingSlotsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(BookingSlot model)
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                if (ModelState.IsValid)
                {
                    var json = JsonConvert.SerializeObject(model);
                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await HttpClientHelper.client.PostAsync(HttpClientHelper.baseUrl + "/api/BookingSlots", content);

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"added resource at {response.Headers.Location}");
                        json = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("booking slot has been inserted " + json);

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

        // GET: BookingSlotsController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                var response = await HttpClientHelper.client.GetAsync(HttpClientHelper.baseUrl + "/api/BookingSlots/" + id);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    var bookingSlot = JsonConvert.DeserializeObject<BookingSlot>(json);

                    if (HttpContext.Session.GetInt32("UserId") == bookingSlot.Restaurant.OwnerUserId)
                    {
                        response = await HttpClientHelper.client.GetAsync(HttpClientHelper.baseUrl + "/api/Restaurants");

                        if (response.IsSuccessStatusCode)
                        {
                            json = await response.Content.ReadAsStringAsync();
                            var restaurants = JsonConvert.DeserializeObject<IEnumerable<Restaurant>>(json);


                            bookingSlot.RestaurantsDropdown = restaurants
                                .Where(r => r.OwnerUserId == HttpContext.Session.GetInt32("UserId"));
                            

                            return View(bookingSlot);
                        }
                        else
                        {
                            Console.WriteLine("Internal Server error");
                            return NotFound();
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

        // POST: BookingSlotsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(int id, BookingSlot model)
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                var response = await HttpClientHelper.client.GetAsync(HttpClientHelper.baseUrl + "/api/BookingSlots/" + id);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    var bookingSlot = JsonConvert.DeserializeObject<BookingSlot>(json);

                    if (HttpContext.Session.GetInt32("UserId") == bookingSlot.Restaurant.OwnerUserId)
                    {
                        if (ModelState.IsValid)
                        {
                            json = JsonConvert.SerializeObject(model);
                            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                            response = await HttpClientHelper.client.PutAsync(HttpClientHelper.baseUrl + "/api/BookingSlots/" + id, content);

                            if (response.IsSuccessStatusCode)
                            {
                                Console.WriteLine($"edited resource at {response.Headers.Location}");
                                json = await response.Content.ReadAsStringAsync();
                                Console.WriteLine("booking slot has been edited " + json);

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

        // POST: BookingSlotsController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                var response = await HttpClientHelper.client.GetAsync(HttpClientHelper.baseUrl + "/api/BookingSlots/" + id);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    var bookingSlot = JsonConvert.DeserializeObject<BookingSlot>(json);

                    if (HttpContext.Session.GetInt32("UserId") == bookingSlot.Restaurant.OwnerUserId)
                    {
                        response = await HttpClientHelper.client.DeleteAsync(HttpClientHelper.baseUrl + "/api/BookingSlots/" + id);
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
