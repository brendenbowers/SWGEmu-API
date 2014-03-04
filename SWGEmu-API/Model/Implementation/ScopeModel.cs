using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using OAuth2.DataModels;
using Dapper;
using ServiceStack.OrmLite;
using ServiceStack.ServiceHost;



namespace OAuth2.Server.Model
{
    public class ScopeModel : OAuth2.Server.Model.IScopeModel
    {
        public ServiceStack.OrmLite.IDbConnectionFactory DBFactory { get; set; } //injected by IOC
        public IHttpResponse Response { get; set; }
        

        public IEnumerable<Scope> GetScopeDetails(string Scopes)
        {
            return GetScopeDetails(Scopes == null ? new string[] {} : Scopes.Split(new char[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries));
        }

        public IEnumerable<Scope> GetScopeDetails(IEnumerable<string> Scopes)
        {
            if (Scopes == null || Scopes.Count() == 0)
                return new List<Scope>();

            const string sql = "SELECT `scope_name`,`description`,`owned_by` FROM `Scope` WHERE `scope_name` IN @scopes";

            using (IDbConnection db = DBFactory.Open())
            {
                return db.Query<Scope>(sql, new { scopes = Scopes }); 
            }
        }

        public IEnumerable<Scope> GetScopeDetails()
        {
            const string sql = "SELECT `scope_name`,`description`,`owned_by` FROM `Scope`;";
            using (IDbConnection db = DBFactory.Open())
            {
                return db.Query<Scope>(sql); 
            }
        }

        public IEnumerable<Scope> GetOwnedScopeDetails(ResourceOwner Owner)
        {
            return GetOwnedScopeDetails(Owner.id);
        }

        public IEnumerable<Scope> GetOwnedScopeDetails(string ResourceOwnerID)
        {
            const string sql = "SELECT `scope_name`,`description`,`owned_by` FROM `Scope` WHERE (COALESCE(`owned_by`,'NULL') = COALESCE(@owned_by, 'NULL'))";
            using (IDbConnection db = DBFactory.Open())
            {
                return db.Query<Scope>(sql, new { owned_by = ResourceOwnerID }); 
            }
        }
        
        public Scope CreateScope(Scope Scope)
        {
            const string sql = "INSERT INTO Scope(`scope_name`,`description`,`owned_by`) VALUES(@scope_name,@description,@owned_by);";

            using (IDbConnection db = DBFactory.Open())
            {
                if (db.Execute(sql, Scope) != 0)
                {
                    return Scope;
                }
                return null; 
            }
        }


        public Scope SetScope(Scope Scope, ResourceOwner Owner)
        {
            return SetScope(Scope, Owner.id);
        }

        public Scope SetScope(Scope Scope, string OwnerID)
        {
            const string sql = "UPDATE Scope SET `description` = @description, `owned_by` = @owned_by WHERE `scope_name` = @scope_name AND (COALESCE(`owned_by`,'NULL') = COALESCE(@resource_owner_id, 'NULL'))";
            using (IDbConnection db = DBFactory.Open())
            {
                if (db.Execute(sql, new { description = Scope.description, owned_by = Scope.owned_by, resource_owner_id = OwnerID }) != 0)
                {
                    return Scope;
                }

                return null; 
            }
        }

        public Scope SetScope(Scope Scope)
        {
            const string sql = "UPDATE Scope SET `description` = @description, `owned_by` = @owned_by WHERE `scope_name` = @scope_name";
            using (IDbConnection db = DBFactory.Open())
            {
                if (db.Execute(sql, Scope) != 0)
                {
                    return Scope;
                }

                return null; 
            }
        }

        public Scope UpdateScope(Scope Scope, ResourceOwner Owner)
        {
            return UpdateScope(Scope, Owner.id);
        }

        public Scope UpdateScope(Scope Scope, string OwnerID)
        {
            const string sql = "UPDATE Scope SET `description` =  COALESCE(`description`, @description), `owned_by` = COALESCE(`owned_by`, @owned_by) WHERE `scope_name` = @scope_name AND (COALESCE(`owned_by`,'NULL') = COALESCE(@resource_owner_id, 'NULL'))";
            using (IDbConnection db = DBFactory.Open())
            {
                if (db.Execute(sql, new { description = Scope.description, owned_by = Scope.owned_by, resource_owner_id = OwnerID }) != 0)
                {
                    return Scope;
                }

                return null; 
            }
        }

        public Scope UpdateScope(Scope Scope)
        {
            const string sql = "UPDATE Scope SET `description` =  COALESCE(`description`, @description), `owned_by` = COALESCE(`owned_by`, @owned_by) WHERE `scope_name` = @scope_name;";
            using (IDbConnection db = DBFactory.Open())
            {
                if (db.Execute(sql, Scope) != 0)
                {
                    return Scope;
                }

                return null; 
            }
        }

        public bool DeleteScope(Scope Scope, ResourceOwner Owner)
        {
            return DeleteScope(Scope.scope_name, Owner.id);
        }

        public bool DeleteScope(string ScopeName, string OwnerID)
        {
            const string sql = "DELETE FROM Scope WHERE `scope_name` = @scope_name AND (COALESCE(`owned_by`,'NULL') = COALESCE(@resource_owner_id, 'NULL'));";
            using (IDbConnection db = DBFactory.Open())
            {
                return db.Execute(sql, new { scope_name = ScopeName, owned_by = OwnerID }) != 0; 
            }
        }

        public bool DeleteScope(Scope Scope)
        {
            return DeleteScope(Scope.scope_name);
        }

        public bool DeleteScope(string ScopeName)
        {
            const string sql = "DELETE FROM Scope WHERE `scope_name` = @scope_name;";
            using (IDbConnection db = DBFactory.Open())
            {
                return db.Execute(sql, new { scope_name = ScopeName }) != 0; 
            }
        }


        public bool ScopeExists(string Scope)
        {
            const string sql = "SELECT COUNT(*) FROM Scope WHERE scope_name = @scope_name";
            using (IDbConnection db = DBFactory.Open())
            {
                return db.Query<long>(sql, new { scope_name = Scope }).Single() != 0; 
            }
        }

        public bool ScopeExists(Scope Scope)
        {
            return ScopeExists(Scope.scope_name);
        }
    }
}