using System;

namespace Djm.OGame.Web.Api.Services.OGame
{
    public class OGameException : Exception
    {
        public OGameException() { }

        public OGameException(string msg) : base(msg)
        {

        }
    }
}