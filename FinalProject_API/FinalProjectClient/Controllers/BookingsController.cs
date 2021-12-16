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
    public class BookingsController : Controller
    {
        // GET: BookingsController
        public async Task<ActionResult> Index()
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                var response = await HttpClientHelper.client.GetAsync(HttpClientHelper.baseUrl + "/api/Bookings");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var bookings = JsonConvert.DeserializeObject<IEnumerable<Booking>>(json);
                    return View(bookings);
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

        // GET: BookingsController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                var response = await HttpClientHelper.client.GetAsync(HttpClientHelper.baseUrl + "/api/Bookings/" + id);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    var booking = JsonConvert.DeserializeObject<Booking>(json);
                    return View(booking);
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

        // GET: BookingsController/Create
        public async Task<ActionResult> Create()
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                var response = await HttpClientHelper.client.GetAsync(HttpClientHelper.baseUrl + "/api/BookingSlots");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var bookingSlots = JsonConvert.DeserializeObject<IEnumerable<BookingSlot>>(json);

                    Booking emptyBooking = new Booking
                    {
                        BookingSlotsDropdown = bookingSlots
                    };

                    return View(emptyBooking);
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

        // POST: BookingsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Booking model)
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                if (ModelState.IsValid)
                {
                    model.UserId = (int)HttpContext.Session.GetInt32("UserId");

                    var json = JsonConvert.SerializeObject(model);
                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await HttpClientHelper.client.PostAsync(HttpClientHelper.baseUrl + "/api/Bookings", content);

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"added resource at {response.Headers.Location}");
                        json = await response.Content.ReadAsStringAsync();
                        Console.WriteLine("booking has been inserted " + json);

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

        // GET: BookingsController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                var response = await HttpClientHelper.client.GetAsync(HttpClientHelper.baseUrl + "/api/Bookings/" + id);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    var booking = JsonConvert.DeserializeObject<Booking>(json);

                    if (HttpContext.Session.GetInt32("UserId") == booking.UserId)
                    {
                        response = await HttpClientHelper.client.GetAsync(HttpClientHelper.baseUrl + "/api/BookingSlots");

                        if (response.IsSuccessStatusCode)
                        {
                            json = await response.Content.ReadAsStringAsync();
                            var bookingSlots = JsonConvert.DeserializeObject<IEnumerable<BookingSlot>>(json);


                            booking.BookingSlotsDropdown = bookingSlots;


                            return View(booking);
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

        // POST: BookingsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, Booking model)
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                var response = await HttpClientHelper.client.GetAsync(HttpClientHelper.baseUrl + "/api/Bookings/" + id);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    var bookingSlot = JsonConvert.DeserializeObject<Booking>(json);

                    if (HttpContext.Session.GetInt32("UserId") == bookingSlot.UserId)
                    {
                        if (ModelState.IsValid)
                        {
                            model.UserId = (int)HttpContext.Session.GetInt32("UserId");

                            json = JsonConvert.SerializeObject(model);
                            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                            response = await HttpClientHelper.client.PutAsync(HttpClientHelper.baseUrl + "/api/Bookings/" + id, content);

                            if (response.IsSuccessStatusCode)
                            {
                                Console.WriteLine($"edited resource at {response.Headers.Location}");
                                json = await response.Content.ReadAsStringAsync();
                                Console.WriteLine("booking has been edited " + json);

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

        // POST: BookingsController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                var response = await HttpClientHelper.client.GetAsync(HttpClientHelper.baseUrl + "/api/Bookings/" + id);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();

                    var booking = JsonConvert.DeserializeObject<Booking>(json);

                    if (HttpContext.Session.GetInt32("UserId") == booking.UserId)
                    {
                        response = await HttpClientHelper.client.DeleteAsync(HttpClientHelper.baseUrl + "/api/Bookings/" + id);
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
