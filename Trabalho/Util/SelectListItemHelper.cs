using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trabalho.Data;

namespace Trabalho.Util
{
    public class SelectListItemHelper
    {
        public static IEnumerable<SelectListItem> GetUsuariosList()
        {
            var _repo = new UsuarioTarefaRepository(new Models.ApplicationDbContext());
            return _repo.GetUsuarios()
                        .OrderByDescending(t => t.Nome)
                        .Take(50)
                        .Select(x => new SelectListItem()
                        {
                            Text = x.Nome,
                            Value = x.Id.ToString()
                        })
                        .ToList();
        }

        public static string GetNomeUsuario(int id)
        {
            var _repo = new UsuarioTarefaRepository(new Models.ApplicationDbContext());
            return _repo.GetUsuarios().Where(x => x.Id == id).FirstOrDefault().Nome;
        }
    }
}