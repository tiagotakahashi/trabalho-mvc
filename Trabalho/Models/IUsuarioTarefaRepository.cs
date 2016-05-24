using Trabalho.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trabalho.Data
{
    public interface IUsuarioTarefaRepository
    {
        IQueryable<Tarefa> GetTarefas();
        IQueryable<Usuario> GetUsuarios();
        IQueryable<Usuario> GetUsuariosTarefas();
        IQueryable<Tarefa> GetTarefasPorUsuario(int usuarioId);

        bool Salvar();

        bool AdicionarUsuario(Usuario novoUsuario);
        bool AdicionarTarefa(Tarefa novaTarefa);
    }
}
