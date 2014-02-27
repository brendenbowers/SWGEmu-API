using System;
using System.Collections.Generic;

namespace OAuth2.Server.Model
{
    public interface IClientModel
    {
        OAuth2.DataModels.Client GetClientByID(string ClientID);
        bool ClientExists(string ClientID);
        bool ClientExists(OAuth2.DataModels.Client Client);
        bool CreateClient(OAuth2.DataModels.Client Client);
        OAuth2.DataModels.Client UpdateClient(OAuth2.DataModels.Client Client, OAuth2.DataModels.ResourceOwner ResourceOwner);
        OAuth2.DataModels.Client UpdateClient(OAuth2.DataModels.Client Client, string ResourceOwner);
        OAuth2.DataModels.Client UpdateClient(OAuth2.DataModels.Client Client);
        OAuth2.DataModels.Client SetClient(OAuth2.DataModels.Client Client, OAuth2.DataModels.ResourceOwner ResourceOwner);
        OAuth2.DataModels.Client SetClient(OAuth2.DataModels.Client Client, string ResourceOwner);
        OAuth2.DataModels.Client SetClient(OAuth2.DataModels.Client Client);
        System.Collections.Generic.List<OAuth2.DataModels.Client> GetClientsByOwner(string ResourceOwnerID);
        System.Collections.Generic.List<OAuth2.DataModels.Client> GetOwnedClients(OAuth2.DataModels.ResourceOwner ResourceOwner);
        System.Collections.Generic.List<OAuth2.DataModels.Client> GetClients();
        System.Collections.Generic.List<OAuth2.DataModels.Client> GetClients(IEnumerable<string> ClientIDs); 
        bool DeleteClient(OAuth2.DataModels.Client Client, OAuth2.DataModels.ResourceOwner ResourceOwner);
        bool DeleteClient(string ClientID, string ResourceOwnerID);
        bool DeleteClient(OAuth2.DataModels.Client Client);
        bool DeleteClient(string ClientID);
    }
}
