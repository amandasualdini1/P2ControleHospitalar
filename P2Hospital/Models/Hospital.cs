using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace P2Hospital.Models
{
    public class Hospital
    {
        public int Id { get; set; }
        public string RazaoSocial { get; set; }
        public string CNPJ { get; set;
        }
    }
 
}
