using Microsoft.AspNetCore.SignalR;
using SignalR.Services;

namespace SignalR.Hubs
{
    public class PizzaHub : Hub
    {
        private readonly PizzaManager _pizzaManager;

        public PizzaHub(PizzaManager pizzaManager) {
            _pizzaManager = pizzaManager;
        }

        public override async Task OnConnectedAsync()
        {
            // Ajoute un user et donne l'info du nb de user à tout le monde
            await base.OnConnectedAsync();
            _pizzaManager.AddUser();
            await Clients.All.SendAsync("UpdateNbUsers", _pizzaManager.NbConnectedUsers);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnConnectedAsync();
            _pizzaManager.RemoveUser();
            await Clients.All.SendAsync("UpdateNbUsers", _pizzaManager.NbConnectedUsers);
        }

        public async Task SelectChoice(PizzaChoice choice)
        {
            // On va chercher le nom du groupe et on l'ajoute au groupe qu'il vient de clicker
            string nomGroupe = _pizzaManager.GetGroupName(choice);
            await Groups.AddToGroupAsync(Context.ConnectionId, nomGroupe);

            // Seulement à celui qui vient de click on lui donne les infos sur le prix de la pizza, de nombre d'argent et le nombre de pizza
            await Clients.Caller.SendAsync("UpdatePizzaPrice", _pizzaManager.GetPizzaPrice(choice));
            var updateNbPizzaAndMoneyInfo = new
            {
                nbPizzas = _pizzaManager.GetNbPizzas(choice),
                money = _pizzaManager.GetMoney(choice)
            };
            await Clients.Caller.SendAsync("UpdateNbPizzaAndMoney", updateNbPizzaAndMoneyInfo);
        }

        public async Task UnselectChoice(PizzaChoice choice)
        {
            // On va chercher le nom du groupe et on le retire du groupe
            string nomGroupe = _pizzaManager.GetGroupName(choice);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, nomGroupe);
        }

        public async Task AddMoney(PizzaChoice choice)
        {
            // On va chercher le nom du groupe et donnne l'info sur le nouveau montant à tout le groupe
            string nomGroupe = _pizzaManager.GetGroupName(choice);
            _pizzaManager.IncreaseMoney(choice);
            await Clients.Group(nomGroupe).SendAsync("UpdateMoney", _pizzaManager.GetMoney(choice));
        }

        public async Task BuyPizza(PizzaChoice choice)
        {
            // On va chercher le nom du groupe, on achète la pizza et on notifie le groupe
            string nomGroupe = _pizzaManager.GetGroupName(choice);
            _pizzaManager.BuyPizza(choice);
            var updateNbPizzaAndMoneyInfo = new
            {
                nbPizzas = _pizzaManager.GetNbPizzas(choice),
                money = _pizzaManager.GetMoney(choice)
            };
            await Clients.Group(nomGroupe).SendAsync("UpdateNbPizzaAndMoney", updateNbPizzaAndMoneyInfo);
        }
    }
}
