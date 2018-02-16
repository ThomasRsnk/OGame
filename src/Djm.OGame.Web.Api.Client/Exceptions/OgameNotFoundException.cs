using System;

namespace Djm.OGame.Web.Api.Client.Exceptions
{
    public class OgameNotFoundException : Exception
    {
        private const string Error = "La ressource demandée n'existe pas : ";

        public OgameNotFoundException() { }

        public OgameNotFoundException(string msg) : base(Error + msg)
        {

        }
    }
}