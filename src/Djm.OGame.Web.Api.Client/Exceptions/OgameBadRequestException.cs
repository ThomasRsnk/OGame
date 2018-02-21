using System;

namespace Djm.OGame.Web.Api.Client.Exceptions
{
    public class OgameBadRequestException: Exception
    {
        public OgameBadRequestException() { }

        public OgameBadRequestException(string msg) : base( msg)
        {

        }
    }
}