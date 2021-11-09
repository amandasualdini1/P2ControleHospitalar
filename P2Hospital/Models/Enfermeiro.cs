using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace P2Hospital.Models
{
    public class Enfermeiro
    {
   
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public int CodigoInternoEnfermeiro { get; set; }

        [DisplayName("Descrição")]
        public string Description { get; set; }

        [DisplayName("Nome da Imagem")]
        public string Image { get; set; }

        [NotMapped]
        [DisplayName("Imagem")]
        public IFormFile ImageFile { get; set; }
    }
}
