using System;

namespace JamyShell
{
    public class Program
    {

        static void Main()
        {
            try
            {
                new JamyShellBot().StartAsync().GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
