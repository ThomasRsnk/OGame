using System;

namespace Djm.OGame.Web.Api.Services.Pictures
{
    public class PictureException : Exception
    {
        public PictureException() { }

        public PictureException(string msg) : base(msg)
        {

        }
    }
}