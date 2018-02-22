using System;

namespace Djm.OGame.Web.Api.Client.Exceptions
{
    public class OgameException: Exception
    {
        public OgameException() { }

        public OgameException(string msg) : base( msg)
        {

        }
    }
}