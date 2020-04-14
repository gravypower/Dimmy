using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using Dimmy.DeploymentHook;
using Sitecore.Configuration;
using Sitecore.UpdateCenter.Services.UpdateMode;

[assembly: PreApplicationStartMethod(typeof(Initialiser), nameof(Initialiser.Initialise))]

namespace Dimmy.DeploymentHook
{
    public static class Initialiser
    {
        private static readonly string HookBindMountBinPath;
        private static readonly IReadOnlyDictionary<AssemblyName, Assembly> AssemblyDictionary;
        private static readonly string HookName;

        static Initialiser()
        {
            HookName = Environment.GetEnvironmentVariable("DIMSIM_HOOK_NAME");
            
            if(HookName == null)
                return;
            
            HookBindMountBinPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, HookName, "bin");

            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            var assemblyDictionary = AppDomain.CurrentDomain.GetAssemblies()
                .ToDictionary(x => x.GetName(), x => x);

            AssemblyDictionary = new ReadOnlyDictionary<AssemblyName, Assembly>(assemblyDictionary);

        }

        public static void Initialise()
        {
            AddHookLayer();
            LoadDeploymentHookAssemblies();
        }

        private static void AddHookLayer()
        {
            var providerHelper = new ConfigurationProviderHelper();

            var layeredConfiguration = new LayeredConfigurationFiles();

            var configurationLayerProvider = layeredConfiguration.ConfigurationLayerProviders.FirstOrDefault();

            if (configurationLayerProvider != null && configurationLayerProvider.GetLayers().Any(l => l.Name == HookName))
                return;

            if (!(configurationLayerProvider is DefaultConfigurationLayerProvider defaultConfigurationLayerProvider)) return;

            var ddApplicationLayer = new DefaultConfigurationLayer(HookName, $"{HookName}/App_Config/Include/");

            AddLayer("Foundation", ddApplicationLayer);
            AddLayer("Feature", ddApplicationLayer);
            AddLayer("Project", ddApplicationLayer);

            defaultConfigurationLayerProvider.AddLayer(ddApplicationLayer);
            providerHelper.SaveConfigurationProvider(configurationLayerProvider);
        }

        private static void AddLayer(string layerName, DefaultConfigurationLayer ddApplicationLayer)
        {
            var configEntryInfo = new DefaultConfigurationLayer.ConfigEntryInfo
            {
                Path = layerName,
                Type = DefaultConfigurationLayer.ConfigEntryType.Folder,
                Enabled = true
            };
            ddApplicationLayer.LoadOrder.Add(configEntryInfo);
        }


        private static void LoadDeploymentHookAssemblies()
        {
            if(HookBindMountBinPath == null)
                return;

            var hookBindMountBinDirectory = new DirectoryInfo(HookBindMountBinPath);
            var hookBindMountAssemblies = hookBindMountBinDirectory.GetFiles("*.dll", SearchOption.TopDirectoryOnly);

            foreach (var file in hookBindMountAssemblies)
            {
                var assemblyName = AssemblyName.GetAssemblyName(file.FullName);

                if (!AssemblyDictionary.ContainsKey(assemblyName))
                    //equivalent to adding the assembly name to compilation/assemblies in web.config
                    AppDomain.CurrentDomain.Load(assemblyName);
            }
        }
        
        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var assemblyPath = Path.Combine(HookBindMountBinPath, new AssemblyName(args.Name).Name + ".dll");

            if (!File.Exists(assemblyPath)) return null;

            var assembly = Assembly.LoadFrom(assemblyPath);
            return assembly;
        }
    }
}