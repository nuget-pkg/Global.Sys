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
        public static void Main(string[] args)
        {
            try
            {
                ShowDetail = true;
                Log(args, "args");
                string stdout = Sys.GetProcessStdout(Encoding.UTF8, "bash", "-c", "ls -l");
                Log(stdout, "stdout");
                var props = new LiteDBProps("myApp");
                Echo(props, "initial state");
                props.Put("abc", 123);
                props.Put("ary", NewArray("a", null, 123));
                Console.WriteLine(props.Keys.Count);
                Echo(props.Keys);
                Echo(props);
                Echo(props.ToString());
                Console.WriteLine(props);
                EasyObject list = props.Get("list");
                list.Add(123);
                props.Put("list", list);
                Echo(props);
                Echo(props.Get("list3").AsList);
                //Echo(props.Get("count").Cast<int>());
                Echo(props.Get("count2").IsNull);
                Echo(props.Get("xyz", 0));
                Echo(props.Get("zzz", new List<string>()));
                Echo(props.Get("xxx", NewArray(1, 2, 3)));
                var match = Sys.FindFirstMatch("abc", "xyz", "[a-z]+");
                Echo(match);
                match = Sys.FindFirstMatch("abc", "xyz");
                Echo(match == null);
                string containsSurrogate = "🔥引火★★帝国🔥";
                string surrogateRemoved = Sys.RemoveSurrogatePair(containsSurrogate);
                Echo(surrogateRemoved);

                var dumped = FromJson("""
                {
                  name: "🔥引火★★帝国🔥",
                  job: "Leader",
                  id: "199",
                  createdAt: "2020-02-20T11:00:28.107Z",
                  contactdetails: {
                    phone: "8439743294793",
                    email: "test@abc.com"
                  }
                }
                """);
                Sys.DumpObjectAsJson(dumped);
                Sys.DumpObjectAsJson(dumped, compact: true);
                Sys.DumpObjectAsJson(dumped, keyAsSymbol: true);
                Sys.DumpObjectAsJson(dumped, keyAsSymbol: true, removeSurrogatePair: true);
                var shuffled = dumped.Shuffle();
                Sys.DumpObjectAsJson(shuffled, keyAsSymbol: true, removeSurrogatePair: true);
                Sys.DumpObjectAsJson(shuffled.Take(2), keyAsSymbol: true, removeSurrogatePair: true);
            }
            catch (Exception e)
            {
                Sys.Crash(e);
            }
        }
    }
}
