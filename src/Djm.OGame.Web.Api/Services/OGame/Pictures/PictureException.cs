using System;

namespace Djm.OGame.Web.Api.Services.OGame.Pictures
{
    public class PictureException : Exception
    {
        public PictureException() { }

        public PictureException(string msg) : base(msg)
        {

        }
    }
}