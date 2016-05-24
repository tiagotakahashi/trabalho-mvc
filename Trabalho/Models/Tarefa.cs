using Trabalho.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Trabalho.Models
{
    public class Tarefa
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Display(Name = "Concluído")]
        public bool Concluido { get; set; }

        public int UsuarioId { get; set; }

        [Display(Name = "Usuário")]
        public virtual string NomeUsuario
        {
            get
            {
                return SelectListItemHelper.GetNomeUsuario(UsuarioId);
            }
        }
        public IEnumerable<SelectListItem> Usuarios { get; set; }

        public DateTime DataInclusao { get; set; }
        [Display(Name = "Data inclusão")]
        public virtual string DataString
        {
            get
            {
                return DataInclusao.ToString("dd/MM/yyyy");
            }
        }
    }
}