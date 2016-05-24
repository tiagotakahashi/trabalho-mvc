using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Trabalho.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        public string Sobrenome { get; set; }

        public string Cidade { get; set; }

        public ICollection<Tarefa> Tarefas { get; set; }

        public DateTime DataInclusao { get; set; }
        //public virtual ApplicationUser User { get; set; }
    }
}