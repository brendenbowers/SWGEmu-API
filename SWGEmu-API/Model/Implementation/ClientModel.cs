using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Data;
using Dapper;
using OAuth2.Server.Model.DataModel;
using OAuth2.DataModels;
using ServiceStack.OrmLite;

namespace OAuth2.Server.Model
{
    public class ClientModel : IClientModel
    {
        public ServiceStack.OrmLite.IDbConnectionFactory DBFactory { get; set; } //injected by IOC
        
        public Client GetClientByID(string ClientID)
        {
            using (IDbConnection db = DBFactory.Open())
            {
                return db.Query<Client>("SELECT * FROM Client WHERE id = @id", new { id = ClientID }).FirstOrDefault(); 
            }
        }
        
        public List<Client> GetClientsByOwner(string ResourceOwnerID)
        {
            using (IDbConnection db = DBFactory.Open())
            {
                return db.Query<Client>("SELECT * FROM Client WHERE owned_by = @owned_by", new { owned_by = ResourceOwnerID }).ToList(); 
            }  
        }

        public List<Client> GetOwnedClients(ResourceOwner ResourceOwner)
        {
            return GetClientsByOwner(ResourceOwner.id);
        }

        public List<Client> GetClients()
        {
            using (IDbConnection db = DBFactory.Open())
            {
                return db.Query<Client>("SELECT * FROM Client").ToList(); 
            }
        }

        public bool ClientExists(string ClientID)
        {
            using (IDbConnection db = DBFactory.Open())
            {
                return db.Query<long>("SELECT COUNT(*) FROM Client WHERE id = @id", new { id = ClientID }).Single() != 0; 
            }
        }
        
        public bool ClientExists(OAuth2.DataModels.Client Client)
        {
            return ClientExists(Client.id);
        }

        public bool CreateClient(Client ToCreate)
        {

            using (IDbConnection db = DBFactory.Open())
            {
                return db.Execute("INSERT INTO Client(`id`,`name`,`description`,`secret`,`redirect_uri`,`type`,`icon`,`allowed_scope`,`contact_email`,`owned_by`,`service_account`) VALUES(@id,@name,@description,@secret,@redirect_uri,@type,@icon,@allowed_scope,@contact_email,@owned_by,@service_account);",
                        ToCreate) != 0; 
            }
        }

        public Client UpdateClient(OAuth2.DataModels.Client Client, OAuth2.DataModels.ResourceOwner ResourceOwner)
        {
            return UpdateClient(Client, ResourceOwner.id);
        }

        public Client UpdateClient(OAuth2.DataModels.Client Client, string ResourceOwner)
        {
            const string sql = "UPDATE `Client` SET `name` = COALESCE(@name, `name`), `description` = COALESCE(@description, `description`), `secret` = COALESCE(@secret, `secret`), `redirect_uri` = COALESCE(@redirect_uri, `redirect_uri`), `type` = COALESCE(@type, `type`), `icon` = COALESCE(@icon, `icon`), `allowed_scope` = COALESCE(@allowed_scope, `allowed_scope`), `contact_email` = COALESCE(@contact_email, `contact_email`), `owned_by` = COALESCE(@owned_by, `owned_by`), `service_account` = COALESCE(@service_account, `service_account`) WHERE id = @id AND (COALESCE(`owned_by`,'NULL') = COALESCE(@current_owned_by, 'NULL'))";

            using (IDbConnection db = DBFactory.Open())
            {
                if (db.Execute(sql, new { name = Client.name, description = Client.description, secret = Client.secret, redirect_uri = Client.redirect_uri, type = Client.type, icon = Client.icon, allowed_scope = Client.allowed_scope, contact_email = Client.contact_email, owned_by = Client.owned_by, service_account = Client.service_account, id = Client.id, current_owned_by = ResourceOwner }) != 0)
                    return GetClientByID(Client.id);
                return null; 
            }

        }

        public Client UpdateClient(OAuth2.DataModels.Client Client)
        {
            const string sql = "UPDATE `Client` SET `name` = COALESCE(@name, `name`), `description` = COALESCE(@description, `description`), `secret` = COALESCE(@secret, `secret`), `redirect_uri` = COALESCE(@redirect_uri, `redirect_uri`), `type` = COALESCE(@type, `type`), `icon` = COALESCE(@icon, `icon`), `allowed_scope` = COALESCE(@allowed_scope, `allowed_scope`), `contact_email` = COALESCE(@contact_email, `contact_email`), `owned_by` = COALESCE(@owned_by, `owned_by`), `service_account` = COALESCE(@service_account, `service_account`) WHERE id = @id";

            using (IDbConnection db = DBFactory.Open())
            {
                if (db.Execute(sql, new { name = Client.name, description = Client.description, secret = Client.secret, redirect_uri = Client.redirect_uri, type = Client.type, icon = Client.icon, allowed_scope = Client.allowed_scope, contact_email = Client.contact_email, owned_by = Client.owned_by, service_account = Client.service_account, id = Client.id }) != 0)
                    return GetClientByID(Client.id);
                return null; 
            }
        }

        public Client SetClient(OAuth2.DataModels.Client Client, OAuth2.DataModels.ResourceOwner ResourceOwner)
        {
            return SetClient(Client, ResourceOwner.id);
        }

        public Client SetClient(OAuth2.DataModels.Client Client, string ResourceOwner)
        {
            const string sql = "UPDATE `Client` SET `name` = @name, `description` = @description, `secret` = @secret, `redirect_uri` = @redirect_uri, `type` = @type, `icon` = @icon, `allowed_scope` = @allowed_scope, `contact_email` = @contact_email, `owned_by` = @owned_by, `service_account` = @service_account WHERE id = @id AND (COALESCE(`owned_by`,'NULL') = COALESCE(@current_owned_by, 'NULL'));";

            using (IDbConnection db = DBFactory.Open())
            {
                if (db.Execute(sql, new { name = Client.name, description = Client.description, secret = Client.secret, redirect_uri = Client.redirect_uri, type = Client.type, icon = Client.icon, allowed_scope = Client.allowed_scope, contact_email = Client.contact_email, owned_by = Client.owned_by, service_account = Client.service_account, id = Client.id, current_owned_by = ResourceOwner }) != 0)
                    return GetClientByID(Client.id);
                return null; 
            }

        }

        public Client SetClient(OAuth2.DataModels.Client Client)
        {
            const string sql = "UPDATE `Client` SET `name` = @name, `description` = @description, `secret` = @secret, `redirect_uri` = @redirect_uri, `type` = @type, `icon` = @icon, `allowed_scope` = @allowed_scope, `contact_email` = @contact_email, `owned_by` = @owned_by, `service_account` = @service_account WHERE id = @id";

            using (IDbConnection db = DBFactory.Open())
            {
                if (db.Execute(sql, new { name = Client.name, description = Client.description, secret = Client.secret, redirect_uri = Client.redirect_uri, type = Client.type, icon = Client.icon, allowed_scope = Client.allowed_scope, contact_email = Client.contact_email, owned_by = Client.owned_by, service_account = Client.service_account, id = Client.id }) != 0)
                    return GetClientByID(Client.id);
                return null; 
            }
        }

        public bool DeleteClient(OAuth2.DataModels.Client Client, OAuth2.DataModels.ResourceOwner ResourceOwner)
        {
            return DeleteClient(Client.id, ResourceOwner.id);
        }

        public bool DeleteClient(string ClientID, string ResourceOwnerID)
        {
            using (IDbConnection db = DBFactory.Open())
            {
                return db.Execute("DELETE FROM Client WHERE id = @id AND (COALESCE(`owned_by`,'NULL') = COALESCE(@owned_by, 'NULL'));", new { id = ClientID, owned_by = ResourceOwnerID }) != 0; 
            }
        }

        public bool DeleteClient(OAuth2.DataModels.Client Client)
        {
            return DeleteClient(Client.id);
        }

        public bool DeleteClient(string ClientID)
        {
            using (IDbConnection db = DBFactory.Open())
            {
                return db.Execute("DELETE FROM Client WHERE id = @id;", new { id = ClientID }) != 0; 
            }
        }

        
        public List<Client> GetClients(IEnumerable<string> ClientIDs)
        {
            using (IDbConnection db = DBFactory.Open())
            {
                return db.Query<Client>("SELECT * FROM Client WHERE id IN @id", new { id = ClientIDs }).ToList(); 
            }
        }
    }
}