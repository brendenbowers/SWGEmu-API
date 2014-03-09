using System.Net;
using Funq;
using ServiceStack;
using ServiceStack.DataAnnotations;
using ServiceStack.Logging;
using ServiceStack.Logging.Support.Logging;
using ServiceStack.OrmLite;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.Text;
using ServiceStack.WebHost.Endpoints;
using ServiceStack.ServiceInterface.Cors;
using DeltaVSoft.RCFProto;
using ServiceStack.VirtualPath;
using System;
using System.Collections.Generic;
using ServiceStack.IO;
using ServiceStack.Redis;
using ServiceStack.CacheAccess;
using ServiceStack.Razor;
using ServiceStack.ServiceInterface.Validation;
using OAuth2.DataModels;
using ServiceStack.CacheAccess.Providers;
using ServiceStack.Common.Web;
using SWGEmuAPI.Model;

namespace SWGEmuAPI
{
    public class AppHost : AppHostBase
    {

        public AppHost() : base("SWGEmu API", typeof(AppHost).Assembly)
        {

        }

        public override void Configure(Container container)
        {
            
            ServiceStack.Text.JsConfig.IncludeNullValues = true;
            ServiceStack.Text.JsConfig.IncludeTypeInfo = true;
            
            if (System.PlatformExtension.IsMono())
            {
                RCFProto.SetNativeDllPath(@"/var/mono-www/bin/libRCFProto_NET_impl.so");
            }
            else
            {
                RCFProto.SetNativeDllPath(@"C:/Users/Brenden/Documents/visual studio 2013/Projects/SwgEMU-API/SWGEmu-API/bin/RCFProto_NET_impl.dll");
            }
            RCFProto.Init();

            container.Register<RcfProtoChannel>(AccquireChannel).ReusedWithin(ReuseScope.Request);

            container.RegisterAutoWiredAs<InventoryItemTransformModel,IInventoryItemTransformModel>();
            container.RegisterAutoWiredAs<StructureTransformModel, IStructureTransformModel>();

            container.Register<swgemurpcserver.rpc.SWGEmuAccountService.Stub>(c => swgemurpcserver.rpc.SWGEmuAccountService.CreateStub(c.Resolve<DeltaVSoft.RCFProto.RcfProtoChannel>())).ReusedWithin(ReuseScope.Request);
            container.Register<swgemurpcserver.rpc.SWGEmuCharacterDetailsService.Stub>(c => swgemurpcserver.rpc.SWGEmuCharacterDetailsService.Stub.CreateStub(c.Resolve<DeltaVSoft.RCFProto.RcfProtoChannel>())).ReusedWithin(ReuseScope.Request);
            container.Register<swgemurpcserver.rpc.SWGEmuStructureItemDetailsService.Stub>(c => swgemurpcserver.rpc.SWGEmuStructureItemDetailsService.CreateStub(c.Resolve<RcfProtoChannel>())).ReusedWithin(ReuseScope.Request);
            container.RegisterAutoWiredAs<Model.AccountModel, Model.IAccountModel>().ReusedWithin(ReuseScope.Request);
            container.RegisterAutoWiredAs<Model.CharacterModel, Model.ICharacterModel>().ReusedWithin(ReuseScope.Request);
            container.RegisterAutoWiredAs<Model.StructureModel, IStructureModel>().ReusedWithin(ReuseScope.Request);

            container.Register<Model.IStringDetailsModel>(c =>
                {

                    string path = SWGEmuAPI.Properties.Settings.Default.DetailsFilePath;
                    System.IO.DirectoryInfo pathInfo = new System.IO.DirectoryInfo(path);

                    //throw new Exception(ServiceStack.Text.JsonSerializer.SerializeToString<List<string>>(realpaths));
                    if (!pathInfo.Exists)
                    {
                        //throw new System.IO.DirectoryNotFoundException("Direcotry for string details not found: " + path);
                    }

                    return new Model.StringDetailsModel(new FileSystemVirtualDirectory(VirtualPathProvider, null, pathInfo));
                }).ReusedWithin(ReuseScope.Hierarchy);


            SetConfig(new EndpointHostConfig
            {
                DebugMode = true //Show StackTraces for easier debugging (default auto inferred by Debug/Release builds)
            });

            //enable cors requests and enable the options method.
            Plugins.Add(new CorsFeature(allowedHeaders: "Content-Type, Authorization, Accept"));

            var emitGlobalHeadersHandler = new CustomActionHandler((httpReq, httpRes) => httpRes.EndRequest());

            this.Config.RawHttpHandlers.Add(httpReq =>
                httpReq.HttpMethod == HttpMethods.Options
                    ? emitGlobalHeadersHandler
                    : null); 

            //cahcing
            container.Register<IRedisClientsManager>(c => new PooledRedisClientManager("localhost:6379"));
            container.Register<ICacheClient>(c => (ICacheClient)c.Resolve<IRedisClientsManager>().GetCacheClient()).ReusedWithin(Funq.ReuseScope.None);
            container.Register<IRedisClient>(c => c.Resolve<IRedisClientsManager>().GetClient()).ReusedWithin(Funq.ReuseScope.Request);

            Plugins.Add(new ServiceStack.ServiceInterface.Admin.RequestLogsFeature());
            Plugins.Add(new RazorFormat());
            Plugins.Add(new SessionFeature());
            Plugins.Add(new ValidationFeature());


            //OAuth2 classes
            container.Register<IDbConnectionFactory>(c => new OrmLiteConnectionFactory(Properties.Settings.Default.ConnectionString, ServiceStack.OrmLite.MySql.MySqlDialectProvider.Instance));
            container.RegisterAutoWiredAs<OAuth2.Server.Model.ClientModel, OAuth2.Server.Model.IClientModel>();
            container.RegisterAutoWiredAs<OAuth2.Server.Model.ResourceOwnerModel, OAuth2.Server.Model.IResourceOwnerModel>();
            container.RegisterAutoWiredAs<OAuth2.Server.Model.TokenDBModel, OAuth2.Server.Model.IDBTokenModel>();
            container.RegisterAutoWiredAs<OAuth2.Server.Model.TokenCacheModel, OAuth2.Server.Model.ICacheTokenModel>();
            container.RegisterAutoWiredAs<OAuth2.Server.Model.CacheDBTokenModel, OAuth2.Server.Model.ITokenModel>();
            container.RegisterAutoWiredAs<OAuth2.Server.Model.ApprovalModel, OAuth2.Server.Model.IApprovalModel>();
            container.RegisterAutoWiredAs<OAuth2.Server.Model.ScopeModel, OAuth2.Server.Model.IScopeModel>();
            container.RegisterAutoWiredAs<OAuth2.Server.Model.AuthorizationCodeModel, OAuth2.Server.Model.IAuthorizationCodeModel>();
            container.RegisterAutoWired<OAuth2.Server.Model.ClientGrantModel>();
            container.RegisterAutoWired<OAuth2.Server.Model.CodeGrantModel>();
            container.RegisterAutoWired<OAuth2.Server.Model.PasswordGrantModel>();
            container.RegisterAutoWired<OAuth2.Server.Model.RefreshTokenGrantModel>();
            container.RegisterAutoWired<OAuth2.Server.Model.TokenGrantModel>();

            //add filter to inject client details from basic auth
            RequestFilters.Add(AddClientDetails);
        }


        public static RcfProtoChannel AccquireChannel(Container container)
        {

            return new RcfProtoChannel(new TcpEndpoint(44471));
        }


        private void AddClientDetails(IHttpRequest req, IHttpResponse res, object requestDto)
        {
            ITokenRequest request = requestDto as ITokenRequest;

            if (request == null)
            {
                return;
            }

            KeyValuePair<string, string>? clientdetails = req.GetBasicAuthUserAndPassword();

            if (clientdetails != null)
            {
                request.client_id = clientdetails.Value.Key;
                request.client_password = clientdetails.Value.Value;
            }
        }

    }
}