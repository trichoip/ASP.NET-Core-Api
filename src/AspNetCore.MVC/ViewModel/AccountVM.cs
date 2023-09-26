using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System;

namespace asp.net_core_empty_5._0.ViewModel
{
    public class AccountVM
    {
        public long Id { get; set; }
        public double? Balance { get; set; }
    }
}
