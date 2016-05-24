using Trabalho.Models;
using System;
using System.Linq;

namespace Trabalho.Data
{
    public class UsuarioTarefaRepository : IUsuarioTarefaRepository
    {
        ApplicationDbContext _contexto;

        public UsuarioTarefaRepository(ApplicationDbContext contexto)
        {
            _contexto = contexto;
        }

        public IQueryable<Tarefa> GetTarefas()
        {
            return _contexto.Tarefas;
        }

        public IQueryable<Tarefa> GetTarefasPorUsuario(int usuarioId)
        {
            return _contexto.Tarefas.Where(x => x.UsuarioId == usuarioId);
        }

        public IQueryable<Usuario> GetUsuarios()
        {
            return _contexto.Usuarios;
        }

        public IQueryable<Usuario> GetUsuariosTarefas()
        {
            return _contexto.Usuarios.Include("Tarefas");
        }

        public bool Salvar()
        {
            try
            {
                return _contexto.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                //TODO log this error...
                return false;
            }
        }

        public bool AdicionarUsuario(Usuario novoUsuario)
        {
            try
            {
                _contexto.Usuarios.Add(novoUsuario);
                return true;
            }
            catch (Exception)
            {
                //TODO log this error...
                return false;
            }
        }

        public bool AdicionarTarefa(Tarefa novaTarefa)
        {
            try
            {
                _contexto.Tarefas.Add(novaTarefa);
                return true;
            }
            catch (Exception)
            {
                //TODO log this error...
                return false;
            }
        }
    }
}