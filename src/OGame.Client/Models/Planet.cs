using OGame.Client.Providers.Web;

namespace OGame.Client.Models
{
    public class Planet
    {
        internal IPlayerProvider PlayerProvider { get; }

        internal Planet(IPlayerProvider playerProvider)
        {
            PlayerProvider = playerProvider;
        }

        public int Id { get; internal set; }

        public string Name { get; internal set; }

        public string Coords { get; internal set; }

        public int? PlayerId { get; internal set; }
        public Player Player => PlayerId == null ? null : PlayerProvider.Get(PlayerId.Value);

        public Moon Moon { get; set; }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(Coords)}: {Coords}, {nameof(Moon)}: {Moon}";
        }
    }

    public class Moon
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Size { get; set; }

        public Moon(int id, string name, int size)
        {
            Id = id;
            Name = name;
            Size = size;
        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(Size)}: {Size}";
        }
    }
}
