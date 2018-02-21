using System;

namespace Djm.OGame.Web.Api.Services
{
    public class PictureException : Exception
    {
        public PictureException() { }

        public PictureException(string msg) : base(msg)
        {

        }
    }
}