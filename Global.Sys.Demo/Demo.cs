//css_inc Global.Sys.cs
//css_nuget EasyObject
//css_embed add2.dll
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Global;
using static Global.EasyObject;

namespace Global
{
    public class Program
    {
        delegate int proto_add2(int a, int b);
        delegate IntPtr proto_greeting(IntPtr name);
        public static void Main(string[] args)
        {
            Log(args, "args");
            string stdout = Sys.GetProcessStdout(Encoding.UTF8, "bash", "-c", "ls -l");
            Log(stdout, "stdout");
            var props = new LiteDBProps("myApp");
            props.Put("abc", 123);
            //Echo(props);
            Console.WriteLine(props.Keys.Count);
            Echo(new List<string>());
            Echo(props.Keys);
        }
    }
}
