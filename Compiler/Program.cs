using Lex;
using Generate;
using Parse;
using Tree;

namespace App
{
    class Program
    {
        static void Main(string[] args)
        {
            Test("int foo() {return 2;}");
            //Compile(args[0]);
        }

        private static void Test(string testCode)
        {
            var lex = new Lexer(testCode);
            List<Token> tokens = lex.Tokenize();

            var parser = new Parser(tokens);
            CProgram program = parser.Parse();

            if (parser.hadError)
            {
                Console.WriteLine("Could not successfully parse");
                return;
            }
            
            Generator generator = new Generator(program);
            string assembly = generator.Generate();
            Console.WriteLine(assembly);

            PrettyPrinter pprinter = new PrettyPrinter(program);
            pprinter.Display();
            
            return;    
        }

        // private static void Compile(string filepath)
        // {
        //     Console.WriteLine($"Compiling {filepath}...");

        //     string sourceCode = File.ReadAllText(filepath);
        //     Console.WriteLine("Finished reading file...");

        //     var lex = new Lexer(sourceCode);
        //     List<Token> tokens =  lex.Tokenize();
        //     Console.WriteLine("Finished lexing...");

        //     var AST = new AST(tokens);
        //     Console.WriteLine("Finished parsing...");
            
        //     string assembly = AST.ConvertToAssembly();
        //     Console.WriteLine("\nWriting assembly to file...");

        //     string assemblyFile = "assembly.s";
        //     File.WriteAllText(assemblyFile, assembly);
        //     Console.WriteLine($"Assembly written to {assemblyFile}. Invoking GCC...");

        //     string executableFile = "out";
        //     var gccProcess = new Process();
        //     gccProcess.StartInfo.FileName = @"C:\MinGW\bin\gcc.exe";
        //     gccProcess.StartInfo.Arguments = $"-m64 {assemblyFile} -o {executableFile}";
        //     gccProcess.StartInfo.RedirectStandardOutput = true;
        //     gccProcess.StartInfo.RedirectStandardError = true;
        //     gccProcess.StartInfo.UseShellExecute = false;

        //     gccProcess.Start();
        //     string gccOutput = gccProcess.StandardOutput.ReadToEnd();
        //     string gccError = gccProcess.StandardError.ReadToEnd();
        //     gccProcess.WaitForExit();

        //     if (gccProcess.ExitCode == 0)
        //     {
        //         Console.WriteLine("Executable generated successfully...");
        //     }
        //     else
        //     {
        //         Console.WriteLine(gccOutput);
        //         Console.WriteLine($"GCC error: {gccError}");
        //     }

        //     //File.Delete(assemblyFile);
        //     Console.WriteLine("Assembly file deleted.");
        //     return;
        // }
    }
}
