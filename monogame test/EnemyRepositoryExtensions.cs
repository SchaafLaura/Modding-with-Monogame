using Modding;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis;
using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;

namespace monogame_test
{
    internal static class EnemyRepositoryExtensions
    {
        public static void Init(this EnemyRepository repo)
        {
            if (repo.enemyPrototypes is null)
                repo.enemyPrototypes = new List<Enemy>[10];
            for (int i = 0; i < 10; i++)
                if (repo.enemyPrototypes[i] is null)
                    repo.enemyPrototypes[i] = new List<Enemy>();

            string sharedLibraryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Modding.dll");

            var references = new MetadataReference[]
            {
                /*MetadataReference.CreateFromFile(
                    typeof(object).Assembly.Location),         // mscorlib*/
                /*MetadataReference.CreateFromFile(
                    typeof(Console).Assembly.Location),        // System.Console*/
                MetadataReference.CreateFromFile(
                    typeof(System.Runtime.AssemblyTargetedPatchBandAttribute).Assembly.Location), // System.Runtime
                MetadataReference.CreateFromFile(
                    Assembly.Load("System.Runtime").Location), // Ensure System.Runtime is loaded
                MetadataReference.CreateFromFile(
                    sharedLibraryPath),                        // shared library for abstract Enemy class
                MetadataReference.CreateFromFile(
                    "Monogame.Framework.dll"),                 // access to Xna.Framework
            };

            string enemiesPath = "Enemies";
            if (!Directory.Exists(enemiesPath))
                throw new Exception("No such directory: " + enemiesPath);

            string[] files = Directory.GetFiles(enemiesPath);

            foreach (var path in files)
            {
                string scriptPath = path;
                string code = File.ReadAllText(scriptPath);

                SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);

                CSharpCompilation compilation = CSharpCompilation.Create(
                    assemblyName: "UserScriptAssembly",
                    syntaxTrees: [syntaxTree],
                    references: references,
                    options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

                using var ms = new MemoryStream();
                EmitResult result = compilation.Emit(ms);
                if (!result.Success)
                {
                    foreach (var diagnostic in result.Diagnostics)
                        Console.WriteLine(diagnostic.ToString());
                    return;
                }
                
                // Compilation succeeded, load the assembly
                ms.Seek(0, SeekOrigin.Begin);
                Assembly assembly = Assembly.Load(ms.ToArray());

                var typeName = "UserScripts.";
                typeName += path.Substring((enemiesPath.Length + 2) - 1); // get rid of "Enemies//"
                typeName = typeName.Remove(typeName.Length - 3); // get rid of ".cs"

                Type enemyType = assembly.GetType(typeName)!;
                object enemyInstance = Activator.CreateInstance(enemyType)!;
                var e = (EnemySpawner)enemyInstance;
                repo.AddEnemyPrototype(e.GetEnemyPrototype());
            }
        }
    }
}
