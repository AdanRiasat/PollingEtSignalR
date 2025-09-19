using labo.signalr.api.Data;
using labo.signalr.api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace labo.signalr.api.Hubs
{
    public class TaskHub: Hub
    {
        private readonly ApplicationDbContext _context;
        private int NbUsers = 0;

        public TaskHub (ApplicationDbContext context)
        {
            _context = context;
        }

        public override async Task OnConnectedAsync()
        {
            NbUsers++;
            var tasks = await _context.UselessTasks.ToListAsync();
            await Clients.All.SendAsync("UserCount", NbUsers);
            await Clients.Caller.SendAsync("TaskList", tasks);

            base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            NbUsers--;
            await Clients.All.SendAsync("UserCount", NbUsers);

            base.OnDisconnectedAsync(exception);
        }

        public async Task Add(string name)
        {
            UselessTask uselessTask = new UselessTask()
            {
                Completed = false,
                Text = name
            };
            _context.UselessTasks.Add(uselessTask);
            await _context.SaveChangesAsync();

            var tasks = await _context.UselessTasks.ToListAsync();
            await Clients.All.SendAsync("TaskList", tasks);
        }

        public async Task Complete(int id)
        {
            UselessTask? task = await _context.FindAsync<UselessTask>(id);
            if (task != null)
            {
                task.Completed = true;
                await _context.SaveChangesAsync();
            }

            var tasks = await _context.UselessTasks.ToListAsync();
            await Clients.All.SendAsync("TaskList", tasks);
        }
    }
}
