using System;
namespace OAuth2.Server.Model.DataModel
{
    public interface ILoginRequest
    {
        string[] errors { get; set; }
        string redirect { get; set; }
    }
}
