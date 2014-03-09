using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Dapper;
using OAuth2.DataModels;
using OAuth2.Server.Extension;
using ServiceStack.OrmLite;


namespace OAuth2.Server.Model
{
    public class ApprovalModel : OAuth2.Server.Model.IApprovalModel
    {
        public ServiceStack.OrmLite.IDbConnectionFactory DBFactory { get; set; } //injected by IOC

        public List<Approval> GetApprovalByResourceOwner(ResourceOwner ResourceOwner)
        {
            if(ResourceOwner == null)
                throw  new ArgumentNullException("ResourceOwner");
            return GetApprovalByResourceOwner(ResourceOwner.id);
        }

        public List<Approval> GetApprovalByResourceOwner(string ResourceOwnerID)
        {
            if(string.IsNullOrWhiteSpace(ResourceOwnerID))
                throw new ArgumentException("ResourceOwnerID is required", "ResourceOwnerID");

            const string sql = "SELECT `Approval`.`client_id`,`Approval`.`resource_owner_id`,`Approval`.`refresh_token`,`Approval`.`type`, COALESCE(GROUP_CONCAT(`Approval_Scope`.`scope_name` SEPARATOR  ' '), '') AS scope  FROM `Approval` LEFT JOIN `Approval_Scope` ON `Approval_Scope`.client_id = `Approval`.`client_id`  AND `Approval_Scope`.resource_owner_id = `Approval`.`resource_owner_id`  WHERE `Approval`.`resource_owner_id` = @resourceownerid GROUP BY `Approval`.`client_id`,`Approval`.`resource_owner_id`,`Approval`.`refresh_token`,`Approval`.`type`;";

            using (IDbConnection db = DBFactory.Open())
            {
                return db.Query<Approval>(sql, new { resourceownerid = ResourceOwnerID }).ToList(); 
            }
        }

        public List<Approval> GetApprovalByClientID(Client Client)
        {
            if(Client == null)
                throw new ArgumentNullException("Client");
            return GetApprovalByClientID(Client.id);
        }

        public List<Approval> GetApprovalByClientID(string ClientID)
        {
            if (string.IsNullOrWhiteSpace(ClientID))
                throw new ArgumentException("ClientID is required", "ClientID");

            const string sql = "SELECT `Approval`.`client_id`,`Approval`.`resource_owner_id`,`Approval`.`refresh_token`,`Approval`.`type`, COALESCE(GROUP_CONCAT(`Approval_Scope`.`scope_name` SEPARATOR  ' '), '') AS scope  FROM `Approval` LEFT JOIN `Approval_Scope` ON `Approval_Scope`.client_id = `Approval`.`client_id`  AND `Approval_Scope`.resource_owner_id = `Approval`.`resource_owner_id`  WHERE `Approval`.`client_id` = @clientid GROUP BY `Approval`.`client_id`,`Approval`.`resource_owner_id`,`Approval`.`refresh_token`,`Approval`.`type`;";

            using (IDbConnection db = DBFactory.Open())
            {
                return db.Query<Approval>(sql, new { clientid = ClientID }).ToList(); 
            }
        } 

        public Approval GetApproval(Client Client, ResourceOwner ResourceOwner)
        {
            if (Client == null)
                throw new ArgumentNullException("Client");
            if (ResourceOwner == null)
                throw new ArgumentNullException("ResourceOwner");

            using (IDbConnection db = DBFactory.Open())
            {
                return GetApproval(Client.id, ResourceOwner.id); 
            }
        }

        public Approval GetApproval(string ClientID, string ResourceOwnerID)
        {
            if (string.IsNullOrWhiteSpace(ClientID))
                throw new ArgumentException("Invalid Client ID");
            if (string.IsNullOrWhiteSpace(ResourceOwnerID))
                throw new ArgumentException("Invalid Resource Owner ID");

            const string sql = "SELECT `Approval`.`client_id`,`Approval`.`resource_owner_id`,`Approval`.`refresh_token`,`Approval`.`type`, COALESCE(GROUP_CONCAT(`Approval_Scope`.`scope_name` SEPARATOR  ' '), '') AS scope  FROM `Approval` LEFT JOIN `Approval_Scope` ON `Approval_Scope`.client_id = `Approval`.`client_id`  AND `Approval_Scope`.resource_owner_id = `Approval`.`resource_owner_id`  WHERE `Approval`.`client_id` = @clientid AND `Approval`.`resource_owner_id` = @resourceownerid GROUP BY `Approval`.`client_id`,`Approval`.`resource_owner_id`,`Approval`.`refresh_token`,`Approval`.`type`;";

            using (IDbConnection db = DBFactory.Open())
            {
                return db.Query<Approval>(sql, new { clientid = ClientID, resourceownerid = ResourceOwnerID }).FirstOrDefault(); 
            }
        }

        public Approval GetApprovalByRefreshToken(DataModels.Client Client, string RefreshToken)
        {
            if (Client == null)
                throw new ArgumentNullException("Client");
            return GetApprovalByRefreshToken(Client.id, RefreshToken);
        }

        public Approval GetApprovalByRefreshToken(string ClientID, string RefreshToken)
        {
            if (string.IsNullOrWhiteSpace(ClientID))
                throw new ArgumentException("Invalid Client ID");
            if (string.IsNullOrWhiteSpace(RefreshToken))
                throw new ArgumentException("Invalid Refresh Token");

            const string sql = "SELECT `Approval`.`client_id`,`Approval`.`resource_owner_id`,`Approval`.`refresh_token`,`Approval`.`type`, COALESCE(GROUP_CONCAT(`Approval_Scope`.`scope_name` SEPARATOR  ' '), '') AS scope FROM `Approval` LEFT JOIN `Approval_Scope` ON `Approval_Scope`.client_id = `Approval`.`client_id` AND `Approval_Scope`.resource_owner_id = `Approval`.`resource_owner_id` WHERE `Approval`.`client_id` = @clientid AND `Approval`.`refresh_token` = @refreshtoken GROUP BY `Approval`.`client_id`,`Approval`.`resource_owner_id`,`Approval`.`refresh_token`,`Approval`.`type`;";
            using (IDbConnection db = DBFactory.Open())
            {
                return db.Query<Approval>(sql, new { clientid = ClientID, refreshtoken = RefreshToken }).FirstOrDefault(); 
            }
        }

        public bool AddOrUpdateApproval(Approval Approval)
        {
            using (IDbConnection db = DBFactory.Open())
            {
                using (IDbTransaction trans = db.BeginTransaction())
                {
                    var insertParms = new
                    {
                        client_id = Approval.client_id,
                        resource_owner_id = Approval.resource_owner_id,
                        scope = Approval.scope,
                        refresh_token = Approval.refresh_token,
                        type = Approval.type.ToString(),
                    };

                    int res = db.Execute("REPLACE INTO Approval(`client_id`,`resource_owner_id`,`scope`,`refresh_token`,`type`) VALUES(@client_id, @resource_owner_id, @scope, @refresh_token, @type)", insertParms, trans);

                    if (res <= 0)
                    {
                        trans.Rollback();
                        return false;
                    }

                    const string sql = "INSERT INTO Approval_Scope(client_id, resource_owner_id, scope_name) VALUES(@client_id, @resource_owner_id, @scope_name);";

                    foreach (string scope in Approval.scope.Split(new char[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (db.Execute(sql, new { client_id = Approval.client_id, resource_owner_id = Approval.resource_owner_id, scope_name = scope }, trans) != 1)
                        {
                            trans.Rollback();
                            return false;
                        }
                    }

                    trans.Commit();
                    return true;

                } 
            }

        }

        public bool DeleteApproval(string ClientID, string ResourceOwnerID)
        {
            using (IDbConnection db = DBFactory.Open())
            {
                using (IDbTransaction transaction = db.BeginTransaction())
                {
                    const string approvalSQL = "DELETE FROM `Approval` WHERE `Approval`.`client_id` = @clientid AND `Approval`.resource_owner_id = @resourceownerid";
                    const string tokenSQL = "DELETE FROM `AccessToken` WHERE `AccessToken`.`client_id` = @clientid AND `AccessToken`.`resource_owner_id` = @resourceownerid";
                    var parms = new { clientid = ClientID, resourceownerid = ResourceOwnerID };
                    bool res = db.Execute(approvalSQL, parms, transaction) > 0;
                    if (!res)
                    {
                        transaction.Rollback();
                        return false;
                    }

                    res = db.Execute(tokenSQL, parms, transaction) > 0;
                    if (!res)
                    {
                        transaction.Rollback();
                        return false;
                    }

                    transaction.Commit();
                    return true;
                } 
            }
            

        }

        public bool DeleteApproval(Approval ToDelete)
        {
            return DeleteApproval(ToDelete.client_id, ToDelete.resource_owner_id);
        }

        public bool DeleteApproval(Client Client, ResourceOwner ResourceOwner)
        {
            return DeleteApproval(Client.id, ResourceOwner.id);
        }
    }
}