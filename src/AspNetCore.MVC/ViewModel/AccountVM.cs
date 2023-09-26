using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System;

namespace AspNetCore.MVC.ViewModel
{
    public class AccountVM
    {
        public long Id { get; set; }
        public double? Balance { get; set; }
    }
}
