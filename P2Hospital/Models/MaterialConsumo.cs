using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace P2Hospital.Models
{
    public class MaterialConsumo
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int QtdProduto { get; set; }
        public virtual Enfermeiro Enfermeiro { get; set; }  
        [NotMapped]
        public virtual List<SelectListItem> Enfermeiros { get; set; }
    }
}
