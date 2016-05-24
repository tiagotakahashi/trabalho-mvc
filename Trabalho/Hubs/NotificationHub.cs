using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Trabalho.Models;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Trabalho.Hubs
{
    public class NotificationHub : Hub
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public static ConcurrentDictionary<string, MyUserType> MyUsers = new ConcurrentDictionary<string, MyUserType>();

        public override Task OnConnected()
        {
            MyUsers.TryAdd(Context.User.Identity.Name, new MyUserType() { ConnectionId = Context.ConnectionId });
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            MyUserType garbage;

            MyUsers.TryRemove(Context.User.Identity.Name, out garbage);

            return base.OnDisconnected(stopCalled);
        }

        //public void PushData()
        //{
        //    //Values is copy-on-read but Clients.Clients expects IList, hence ToList()
        //    Clients.Clients(MyUsers.Keys.ToList()).ClientBoundEvent(data);
        //}

        public void Refresh()
        {
            Clients.Caller.refresh(db.Tarefas.ToList());
        }

        public void Done(int id)
        {
            try
            {
                var model = db.Tarefas.Find(id);

                model.Concluido = true;

                db.SaveChanges();

                Clients.Others.todoDone($@"O usuário {Context.User.Identity.Name} concluiu a tarefa {model.Nome}");
                Clients.All.done(id);
            }
            catch (Exception ex)
            {
                Clients
                    .Caller
                    .error(
                        string.IsNullOrWhiteSpace(ex.Message)
                        ? ex.InnerException.Message
                        : ex.Message);
            }
        }
    }

    public class MyUserType
    {
        public string ConnectionId { get; set; }
        // Can have whatever you want here
    }
}