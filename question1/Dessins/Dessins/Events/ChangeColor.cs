namespace Dessins.Events
{
    public class ChangeColor : DrawingEvent
    {
        public override string Type { get { return "Color"; } }

        public string Couleur { get; set; }

        public ChangeColor(string couleur)
        {
            Couleur = couleur;
        }
    }
}
