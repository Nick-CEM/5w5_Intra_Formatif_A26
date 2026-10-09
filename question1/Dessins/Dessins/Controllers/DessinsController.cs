using Dessins.Events;
using Microsoft.AspNetCore.Mvc;

namespace Dessins.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class DessinsController : ControllerBase
    {
        [HttpGet]
        // Rien à modifier ici, juste un exemple de dessin très simple
        public ActionResult GetDrawing1()
        {
            var drawSquare = new DrawSquare(2, 2);
            
            return Ok(drawSquare);
        }

        // TODO: Il faut ajouter une nouvelle action pour dessiner la séquence mentionnée dans l'énoncé
        [HttpGet]
        public ActionResult GetDrawing2()
        {
            // Event de départ
            DrawCircle EventDepart = new DrawCircle(1, 1);

            // Crée l'event enfant et l'ajoute
            Wait attendre1 = new Wait(3);
            EventDepart.DrawingEvents.Add(attendre1);

            // Crée le changement de couleur, Crée les deux squares et le wait et les ajoutent
            ChangeColor changeRouge = new ChangeColor("red");
            DrawSquare square1 = new DrawSquare(0, 2);
            DrawSquare square2 = new DrawSquare(2, 2);
            Wait attendre2 = new Wait(1);
            attendre1.DrawingEvents.Add(changeRouge);
            attendre1.DrawingEvents.Add(square1);
            attendre1.DrawingEvents.Add(square2);
            attendre1.DrawingEvents.Add(attendre2);

            // Crée le changement de couleur, Crée l'étoile et l'ajoute au wait
            ChangeColor changeJaune = new ChangeColor("yellow");
            DrawStar star = new DrawStar(1, 3, 20);
            attendre2.DrawingEvents.Add(changeJaune);
            attendre2.DrawingEvents.Add(star);

            return Ok(EventDepart);
        }
    }
}
