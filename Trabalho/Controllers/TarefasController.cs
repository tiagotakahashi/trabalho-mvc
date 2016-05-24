using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Trabalho.Models;
using Microsoft.AspNet.SignalR;
using Trabalho.Hubs;
using Trabalho.Util;
using Trabalho.Data;

namespace Trabalho.Controllers
{
    [System.Web.Mvc.Authorize]
    public class TarefasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        IHubContext hubContext = GlobalHost
                                 .ConnectionManager
                                 .GetHubContext<NotificationHub>();

        private IUsuarioTarefaRepository _repo;
        public TarefasController(IUsuarioTarefaRepository repo)
        {
            _repo = repo;
        }

        public async Task<ActionResult> Index()
        {
            return View(await db.Tarefas.ToListAsync());
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tarefa tarefaModel = await db.Tarefas.FindAsync(id);
            if (tarefaModel == null)
            {
                return HttpNotFound();
            }
            return View(tarefaModel);
        }

        public ActionResult Create()
        {
            return View();
        }

        // POST: ToDoModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Nome,Descricao,Concluido,UsuarioId")] Tarefa tarefaModel)
        {
            if (ModelState.IsValid)
            {
                tarefaModel.DataInclusao = DateTime.UtcNow;
                db.Tarefas.Add(tarefaModel);
                await db.SaveChangesAsync();

                //hubContext.Clients.All.todoCreatedC($@"O usuário {User.Identity.Name} criou a tarefa {toDoModel.Name}");
                MyUserType user;
                NotificationHub.MyUsers.TryGetValue(User.Identity.Name, out user);
                hubContext.Clients.AllExcept(user.ConnectionId).todoCreated($@"O usuário {User.Identity.Name} criou a tarefa {tarefaModel.Nome}");

                return RedirectToAction("Index");
            }

            return View(tarefaModel);
        }

        // GET: ToDoModels/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tarefa tarefaModel = await db.Tarefas.FindAsync(id);
            if (tarefaModel == null)
            {
                return HttpNotFound();
            }
            tarefaModel.Usuarios = SelectListItemHelper.GetUsuariosList();
            return View(tarefaModel);
        }

        // POST: ToDoModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Nome,Descricao,Concluido,UsuarioId")] Tarefa tarefaModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tarefaModel).State = EntityState.Modified;
                await db.SaveChangesAsync();

                MyUserType user;
                NotificationHub.MyUsers.TryGetValue(User.Identity.Name, out user);
                hubContext.Clients.AllExcept(user.ConnectionId).todoModified($@"O usuário {User.Identity.Name} alterou a tarefa {tarefaModel.Nome}");

                return RedirectToAction("Index");
            }
            return View(tarefaModel);
        }

        // GET: ToDoModels/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tarefa tarefaModel = await db.Tarefas.FindAsync(id);
            if (tarefaModel == null)
            {
                return HttpNotFound();
            }
            return View(tarefaModel);
        }

        // POST: ToDoModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Tarefa tarefaModel = await db.Tarefas.FindAsync(id);
            db.Tarefas.Remove(tarefaModel);
            await db.SaveChangesAsync();

            MyUserType user;
            NotificationHub.MyUsers.TryGetValue(User.Identity.Name, out user);
            hubContext.Clients.AllExcept(user.ConnectionId).todoDeleted($@"O usuário {User.Identity.Name} deletou a tarefa {tarefaModel.Nome}");

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
