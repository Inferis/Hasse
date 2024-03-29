﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Hasse.Web.App_Start;
using Raven.Client.Document;
using Raven.Client.Indexes;

namespace Hasse.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        public static DocumentStore Store;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            BundleConfig.RegisterBundles(BundleTable.Bundles);

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            InitializeDocumentStore();
        }

        private void InitializeDocumentStore()
        {
            Store = new DocumentStore {
                ConnectionStringName = "RavenDB",
                Conventions = {
                    IdentityPartsSeparator = "-",
                    TransformTypeTagNameToDocumentKeyPrefix = RavenDBTransformTypeTagNameToDocumentKeyPrefix,
                }
            };
            Store.Initialize();

            IndexCreation.CreateIndexes(Assembly.GetCallingAssembly(), Store);
        }

        public static string RavenDBTransformTypeTagNameToDocumentKeyPrefix(string typeTagName)
        {
            return typeTagName.ToLowerInvariant();
        }
    }
}