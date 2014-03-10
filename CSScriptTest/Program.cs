using System;
using System.Linq.Expressions;
using CSScriptLibrary;

namespace CSScriptTest
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {

               CSScript.Execute(null, new []{@"C:\Users\Sasha\Documents\Visual Studio 2013\Projects\Rose.VExtension\CSScriptTest\Class1.cs"});
                CSScript.Evaluator.CompileCode(
                    SourceCodeProvider.GetFileContent(
                        @"C:\Users\Sasha\Documents\Visual Studio 2013\Projects\Rose.VExtension\CSScriptTest\Class1.cs"));
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadKey();
            }
        }
    }
}
