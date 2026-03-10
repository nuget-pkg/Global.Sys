using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
                Sys.SetupConsoleUTF8();
                if (false)
                {
                    ShowDetail = true;
                    Log(args, "args");
                    string stdout = Sys.GetProcessStdout(Encoding.UTF8, "bash", "-c", "ls -l");
                    Log(stdout, "stdout");
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
                    ShowDetail = false;
                    EasyObject? mediaInfo;
                    mediaInfo = MediaInfo.ParseMediaUrl(@"https://www.youtube.com/watch?v=YYWwIyamQvw");
                    Log(mediaInfo, "(0)");
                    mediaInfo = MediaInfo.ParseMediaUrl(@"https://www.youtube.com/watch?v=YYWwIyamQvw&list=RDE0vW5mS0y3U&index=9");
                    Log(mediaInfo, "(1)");
                    mediaInfo = MediaInfo.ParseMediaUrl(@"https://youtube.com/watch?v=YYWwIyamQvw&list=RDE0vW5mS0y3U&index=9");
                    Log(mediaInfo, "(2)");
                    mediaInfo = MediaInfo.ParseMediaUrl(@"https://www.xvideos.com/video.okdpihde0a3/_ai_");
                    Log(mediaInfo, "(3)");
                    mediaInfo = MediaInfo.ParseMediaUrl(@"https://jp.xhamster.com/videos/i-found-out-my-best-friends-wife-was-doing-porn-xhXMwoP");
                    Log(mediaInfo, "(4)");
                    mediaInfo = MediaInfo.ParseMediaUrl(@"https://jp.pornhub.com/view_video.php?viewkey=ph634d54a540f4a");
                    Log(mediaInfo, "(5)");
                    mediaInfo = MediaInfo.ParseMediaUrl(@"https://www.redtube.com/103102541");
                    Log(mediaInfo, "(6)");
                    mediaInfo = MediaInfo.ParseMediaUrl(@"P:\@porn\【redtube】WOWGIRLS Gorgeous models Eva Elfie and Kate Rich getting fucked by their mutual friend【ID：103102541】.mp4");
                    Log(mediaInfo, "(7.1)");
                    mediaInfo = MediaInfo.ParseMediaUrl(@"P:\@porn\【redtube】WOWGIRLS Gorgeous models Eva Elfie and Kate Rich getting fucked by their mutual friend【ID＝103102541】.mp4");
                    Log(mediaInfo, "(7.2)");
                    mediaInfo = MediaInfo.ParseMediaUrl(@"P:\@porn\++++【redtube】WOWGIRLS Gorgeous models Eva Elfie and Kate Rich getting fucked by their mutual friend【ID=103102541】.mp4");
                    Log(mediaInfo, "(7.3)");
                    mediaInfo = MediaInfo.ParseMediaUrl(@"C:\テスト\フォルダ\ああああ [xhXMwoP].mp4");
                    Log(mediaInfo, "(8)");
                    var db = new LiteDBProps("myDb1");
                    db.DeleteAll();
                    db.Put("abc", 123);
                    db.Put("xyz", "hello ハロー©");
                    Log(db, "db");
                    var exp = db.ExportToPlainObject();
                    Log(exp, "exp");
                    var db2 = new LiteDBProps("myDb2");
                    db2.ImportFromPlainObject(exp);
                    Log(db2, "db2");
                    var json2 = db2.ExportToCommonJson();
                    Log(json2, "json2");
                    var db3 = new LiteDBProps("myDb3");
                    db3.ImportFromCommonJson(json2);
                    Log(db3, "db3");
                    var playlistText = File.ReadAllText("assets/【ダウンロード候補】");
                    var playlistLines = Sys.TextToLines(playlistText);
                    foreach (var line in playlistLines)
                    {
                        Log(line);
                        var mediaInfo2 = MediaInfo.ParseMediaUrl(line);
                        Log(mediaInfo2);
                    }
                    string propDbFilePath = Sys.HomeFile("tmp", "abc.litedb");
                    var props = new LiteDBProps(new FileInfo(propDbFilePath));
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
                    var now = DateTime.Now;
                    string dtStr1 = Sys.DateTimeString(now);
                    string dtStr2 = Sys.DateTimeStringSafe(now);
                    Echo(dtStr1, "dtStr1");
                    Echo(dtStr2, "dtStr2");
                    string dStr1 = Sys.DateString(now);
                    string dStr2 = Sys.DateStringCompact(now);
                    Echo(dStr1, "dStr1");
                    Echo(dStr2, "dStr2");
                    var wc1 = Sys.ExpandWildcard("/p/@youtube-1080p/*");
                    Log(wc1, "wc1");
                    var wc2 = Sys.ExpandWildcardList(
                        "/p/@youtube-1080p/*",
                        "/p/@youtube-2160p/*"
                        );
                    Log(wc2, "wc2");
                    Log(Sys.CygpathWindows("/c/home13/cmd"));
                    Log(Sys.CygpathWindows("/mnt/c/home13/cmd"));
                    Log(Sys.CygpathWindows(@"C:\home13\cmd"));
                    Sys.SetCwd("/p/@youtube-m4a");
                    //Sys.RunCommand("dir.exe", "*.m4a");
                    Sys.SetCwd("/mnt/p/@youtube-1080p");
                    //Sys.RunCommand("dir.exe", "*.mp4");
                }
                string homeFile = Sys.HomeFile("@sub", "nuget.org", "univlang", "tmp.https://www.youtube.com/watch?v=pTxCQjZooQ8&list=PLTvSv0jkjbk_EhZwZjDeNJIIGK25yNGt8");
                Log(homeFile);
                File.WriteAllText(homeFile, "ハロー©2");
                homeFile = Sys.HomeFile("tmp", "a|b.txt");
                Log(homeFile);
                File.WriteAllText(homeFile, "ハロー©3");
                Sys.Exit(1);
            }
            catch (Exception e)
            {
                Sys.Crash(e);
            }
        }
    }
}
