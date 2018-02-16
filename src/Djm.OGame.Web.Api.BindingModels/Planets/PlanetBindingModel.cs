namespace Djm.OGame.Web.Api.BindingModels.Planets
{
    public class PlanetBindingModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Coords { get; set; }
        public MoonBindingModel Moon { get; set; }
    }
}