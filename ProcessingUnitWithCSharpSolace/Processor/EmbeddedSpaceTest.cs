using CustomExternalDataSource.ExternalDataSource;
using GigaSpaces.Core;
using GigaSpaces.Core.Persistency;
using System.Collections.Generic;

namespace Piper.Processor.Test
{
    public class EmbeddedSpaceTest
    {

        public static void Main(string[] args)
        {
            IDictionary<string, string> spaceProps = new Dictionary<string, string>();
            spaceProps.Add("space-config.engine.cache_policy", "1");
            spaceProps.Add("cluster-config.cache-loader.external-data-source", "true");
            spaceProps.Add("cluster-config.cache-loader.central-data-source", "true");

            IDictionary<string, string> externalDataSourceProps = new Dictionary<string, string>();
            // for NHibernate
            // use full rooted path if testing, otherwise ProcessingUnitContainer.Current.WorkingDirectory is added to relative path
            // when not deployed on to service grid ProcessingUnitContainer.Current.WorkingDirectory is null
            externalDataSourceProps.Add("nhibernate-hbm-dir", @"C:\GigaSpaces\XAP.NET-16.3.0-patch-p-3-x64\NET v4.0\Deploy\DataProcessor\NHibernateCfg");
            externalDataSourceProps.Add("nhibernate-config-file", @"C:\GigaSpaces\XAP.NET-16.3.0-patch-p-3-x64\NET v4.0\Deploy\DataProcessor\NHibernateCfg\nHibernate.cfg.xml");
            externalDataSourceProps.Add("InitialLoadThreadPoolSize", "3");
            externalDataSourceProps.Add("InitiallLoadWithSpaceRouting", "true");

            // for testing
            externalDataSourceProps.Add("AssemblyFileName", @"C:\GigaSpaces\XAP.NET-16.3.0-patch-p-3-x64\NET v4.0\Examples\ProcessingUnitWithCSharpSolace\Release\Piper.Common.dll");
            // for Solace
            externalDataSourceProps.Add("SpaceName", "demo");
            externalDataSourceProps.Add("Solace.Host", "localhost");
            externalDataSourceProps.Add("MaxAttempts", "3");
            externalDataSourceProps.Add("Solace.UserName", "default");
            externalDataSourceProps.Add("Solace.Password", "");
            externalDataSourceProps.Add("Solace.VpnName", "default");
            externalDataSourceProps.Add("Solace.ConnectRetries", "-1");


            //AbstractExternalDataSource externalDataSource = new NHibernateSpaceDataSource();

            AbstractExternalDataSource externalDataSource = new SolaceExternalDataSource();

            var factory = new EmbeddedSpaceFactory("demo")
            {
                ExternalDataSource = new ExternalDataSourceConfig { Instance = externalDataSource, CustomProperties = externalDataSourceProps },
                CustomProperties = spaceProps
            };

            
            //factory.LookupGroups = "xap-16.3.0";
            
            ISpaceProxy spaceProxy = factory.Create();
        }
    }
}
