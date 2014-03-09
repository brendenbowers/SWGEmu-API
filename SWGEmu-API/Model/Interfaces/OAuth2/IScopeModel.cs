using System;
namespace OAuth2.Server.Model
{
    public interface IScopeModel
    {
        System.Collections.Generic.IEnumerable<OAuth2.DataModels.Scope> GetScopeDetails(System.Collections.Generic.IEnumerable<string> Scopes);
        System.Collections.Generic.IEnumerable<OAuth2.DataModels.Scope> GetScopeDetails(string Scopes);
        System.Collections.Generic.IEnumerable<OAuth2.DataModels.Scope> GetScopeDetails();
        System.Collections.Generic.IEnumerable<OAuth2.DataModels.Scope> GetOwnedScopeDetails(DataModels.ResourceOwner Owner);
        System.Collections.Generic.IEnumerable<OAuth2.DataModels.Scope> GetOwnedScopeDetails(string ResourceOwnerID);
        bool ScopeExists(string Scope);
        bool ScopeExists(OAuth2.DataModels.Scope Scope);
        OAuth2.DataModels.Scope CreateScope(OAuth2.DataModels.Scope Scope);
        OAuth2.DataModels.Scope SetScope(OAuth2.DataModels.Scope Scope, DataModels.ResourceOwner Owner);
        OAuth2.DataModels.Scope SetScope(OAuth2.DataModels.Scope Scope, string OwnerID);
        OAuth2.DataModels.Scope SetScope(OAuth2.DataModels.Scope Scope);
        OAuth2.DataModels.Scope UpdateScope(OAuth2.DataModels.Scope Scope, DataModels.ResourceOwner Owner);
        OAuth2.DataModels.Scope UpdateScope(OAuth2.DataModels.Scope Scope, string OwnerID);
        OAuth2.DataModels.Scope UpdateScope(OAuth2.DataModels.Scope Scope);
        bool DeleteScope(OAuth2.DataModels.Scope Scope, DataModels.ResourceOwner Owner);
        bool DeleteScope(string ScopeName, string OwnerID);
        bool DeleteScope(OAuth2.DataModels.Scope Scope);
        bool DeleteScope(string ScopeName);
    }
}
