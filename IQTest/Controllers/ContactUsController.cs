using IQTest.DataAccess.Data;
using IQTest.Models;
using IQTest.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace IQTest.Controllers
{
    public class ContactUsController : Controller
    {
        

        public IActionResult Index()
        {
            return View();
        }



    }
}
