﻿using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dimmy.Engine.Pipelines.GenerateEnvironmentFile;
using Dimmy.Engine.Services;

namespace Dimmy.Engine.Pipelines.StartProject.Nodes
{
    public class GenerateEnvironmentFile:Node<IStartProjectContext>
    {
        public override int Order => -2;
        private readonly Pipeline<Node<IGenerateEnvironmentFileContext>, IGenerateEnvironmentFileContext> _generateEnvironmentFilePipeline;
        
        public GenerateEnvironmentFile(
            Pipeline<Node<IGenerateEnvironmentFileContext>, IGenerateEnvironmentFileContext> generateEnvironmentFilePipeline)
        {
            _generateEnvironmentFilePipeline = generateEnvironmentFilePipeline;
        }
        public override async Task DoExecute(IStartProjectContext input)
        {
            var workingEnvironmentFile = Path.Combine(input.WorkingPath, ".env");
            if (File.Exists(workingEnvironmentFile)) File.Delete(workingEnvironmentFile);

            _generateEnvironmentFilePipeline.Execute(new GenerateEnvironmentFileContext
            {
                Project = input.Project,
                ProjectInstance = input.ProjectInstance,
                WorkingPath = input.WorkingPath
            });
            
            if (!File.Exists(workingEnvironmentFile)) throw new EnvironmentFileNotFound();
            
            var environmentalVariables = await File.ReadAllLinesAsync(workingEnvironmentFile);
            input.EnvironmentalVariables = environmentalVariables
                .Select(l => l.Split("="))
                .ToDictionary(l => l[0], l => l[1]);
        }
    }
}