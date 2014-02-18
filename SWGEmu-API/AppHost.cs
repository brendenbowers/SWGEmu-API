using System.Net;
using Funq;
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

            container.Register<swgemurpcserver.rpc.SWGEmuAccountService.Stub>(c => swgemurpcserver.rpc.SWGEmuAccountService.CreateStub(c.Resolve<DeltaVSoft.RCFProto.RcfProtoChannel>())).ReusedWithin(ReuseScope.Request);
            container.Register<swgemurpcserver.rpc.SWGEmuCharacterDetailsService.Stub>(c => swgemurpcserver.rpc.SWGEmuCharacterDetailsService.Stub.CreateStub(c.Resolve<DeltaVSoft.RCFProto.RcfProtoChannel>())).ReusedWithin(ReuseScope.Request);
            container.Register<swgemurpcserver.rpc.SWGEmuStructureItemDetailsService.Stub>(c => swgemurpcserver.rpc.SWGEmuStructureItemDetailsService.CreateStub(c.Resolve<RcfProtoChannel>())).ReusedWithin(ReuseScope.Request);
            container.RegisterAutoWired<Model.AccountModel>().ReusedWithin(ReuseScope.Request);
            container.RegisterAutoWired<Model.CharacterModel>().ReusedWithin(ReuseScope.Request);
            container.RegisterAutoWired<Model.StructureModel>().ReusedWithin(ReuseScope.Request);

            container.Register<Model.StringDetailsModel>(c =>
                {

                    string path = SWGEmuAPI.Properties.Settings.Default.DetailsFilePath;
                    System.IO.DirectoryInfo pathInfo = new System.IO.DirectoryInfo(path);

                    //throw new Exception(ServiceStack.Text.JsonSerializer.SerializeToString<List<string>>(realpaths));
                    if (!pathInfo.Exists)
                    {
                        throw new System.IO.DirectoryNotFoundException("Direcotry for string details not found: " + path);
                    }

                    return new Model.StringDetailsModel(new FileSystemVirtualDirectory(VirtualPathProvider, null, pathInfo));
                }).ReusedWithin(ReuseScope.Hierarchy);

            try
            {
                SWGEmuAPI.Models.Inventory.CharacterInventoryItemExtensions.StringDetailsModel = container.Resolve<Model.StringDetailsModel>();
                SWGEmuAPI.Models.Structure.StructureItemDetailsExtension.StringDetailsModel = container.Resolve<Model.StringDetailsModel>();
            }
            catch (Exception diex)
            {

            }

            SetConfig(new EndpointHostConfig
            {
                DebugMode = true //Show StackTraces for easier debugging (default auto inferred by Debug/Release builds)
            });

            Plugins.Add(new CorsFeature());
        }


        public static RcfProtoChannel AccquireChannel(Container container)
        {

            return new RcfProtoChannel(new TcpEndpoint(44471));
        }

    }
}