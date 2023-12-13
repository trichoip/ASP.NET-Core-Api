using AspNetCore.Client.MVC.Models;
using AspNetCore.Client.MVC.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AspNetCore.Client.MVC.Controllers;


public class CarController : Controller
{
    private readonly CarService _carService;
    private readonly IConfiguration _configuration;

    public CarController(IConfiguration configuration)
    {
        _carService = new CarService();
        _configuration = configuration;
    }

    public IActionResult Detail(int id)
    {
        Car _car = _carService.Get(id);
        return View(_car);
    }

    public IActionResult List(int page)
    {
        page = page <= 0 ? 1 : page;

        int size = _configuration.GetValue<int>("AppSettings:PageSize");

        List<Car> _cars = _carService.GetAll(page, size).ToList();

        int total = _carService.GetAll(1, 100000).Count();
        int totalPage = (int)Math.Ceiling((double)total / size);

        ViewBag.TotalPage = totalPage;
        ViewBag.Page = page;

        return View(_cars);
    }

    public IActionResult Delete(int id)
    {
        _carService.Delete(id);

        return Redirect("/car/list");
    }

    public IActionResult Edit(Car Car)
    {
        _carService.Update(Car);
        return Redirect("/car/detail/" + Car.Id);
    }

    public IActionResult Create()
    {
        return Redirect("/car/list");
    }

}