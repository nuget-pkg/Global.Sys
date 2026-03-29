using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Global;
#if !TEST_MINI
using static Global.EasyObject;
#else
using EasyObject = Global.MiniEasyObject;
using static Global.MiniEasyObject;
#endif

// ReSharper disable HeuristicUnreachableCode
#pragma warning disable CS0162 // 到達できないコードが検出されました

try
{
    Sys.SetupConsoleUTF8();
    Console.Error.Write("\n");
    UseAnsiConsole = true;
    if (false)
    {
        ShowDetail = true;
        Log(args, "args");
        var stdout = Sys.GetProcessStdout(Encoding.UTF8, "bash", "-c", "ls -l");
        Log(stdout, "stdout");
        var match = Sys.FindFirstMatch("abc", "xyz", "[a-z]+");
        Echo(match);
        match = Sys.FindFirstMatch("abc", "xyz");
        Echo(match == null);
        var containsSurrogate = "🔥引火★★帝国🔥";
        var surrogateRemoved = Sys.RemoveSurrogatePair(containsSurrogate);
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
        Sys.DumpObjectAsJson(dumped, true);
        Sys.DumpObjectAsJson(dumped, keyAsSymbol: true);
        Sys.DumpObjectAsJson(dumped, keyAsSymbol: true, removeSurrogatePair: true);
        var shuffled = dumped.Shuffle();
        Sys.DumpObjectAsJson(shuffled, keyAsSymbol: true, removeSurrogatePair: true);
        Sys.DumpObjectAsJson(shuffled.Take(2), keyAsSymbol: true, removeSurrogatePair: true);
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

        var propDbFilePath = Sys.HomeFile("tmp", "abc.litedb");
        var props = new LiteDBProps(new FileInfo(propDbFilePath));
        Echo(props, "initial state");
        props.Put("abc", 123);
        props.Put("ary", NewArray("a", null, 123));
        Console.WriteLine(props.Keys.Count);
        Echo(props.Keys);
        Echo(props);
        Echo(props.ToString());
        Console.WriteLine(props);
        var list = props.Get("list");
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
        var dtStr1 = Sys.DateTimeString(now);
        var dtStr2 = Sys.DateTimeStringSafe(now);
        Echo(dtStr1, "dtStr1");
        Echo(dtStr2, "dtStr2");
        var dStr1 = Sys.DateString(now);
        var dStr2 = Sys.DateStringCompact(now);
        Echo(dStr1, "dStr1");
        Echo(dStr2, "dStr2");
        //string[] wc1 = Sys.ExpandWildcard("/p/@youtube-1080p/*");
        //Log(wc1, "wc1");
        //string[] wc2 = Sys.ExpandWildcardList(
        //    "/p/@youtube-1080p/*",
        //    "/p/@youtube-2160p/*"
        //    );
        //Log(wc2, "wc2");
        Log(Sys.CygpathWindows("/c/home13/cmd"));
        Log(Sys.CygpathWindows("/mnt/c/home13/cmd"));
        Log(Sys.CygpathWindows(@"C:\home13\cmd"));
        //Sys.SetCwd("/p/@youtube-m4a");
        ////Sys.RunCommand("dir.exe", "*.m4a");
        //Sys.SetCwd("/mnt/p/@youtube-1080p");
        ////Sys.RunCommand("dir.exe", "*.mp4");
        var homeFile = Sys.HomeFile("@sub", "nuget.org", "univlang",
            "tmp.https://www.youtube.com/watch?v=pTxCQjZooQ8&list=PLTvSv0jkjbk_EhZwZjDeNJIIGK25yNGt8");
        Log(homeFile);
        File.WriteAllText(homeFile, "ハロー©2");
        homeFile = Sys.HomeFile("tmp", "a|b.txt");
        Log(homeFile);
        File.WriteAllText(homeFile, "ハロー©3");
        //Sys.Exit(1);
#if !TEST_MINI
        EasyObject mediaInfo;
        mediaInfo = MediaInfo.ParseMediaUrl(@"https://www.youtube.com/watch?v=YYWwIyamQvw");
        Log(mediaInfo, "(0)");
        mediaInfo = MediaInfo.ParseMediaUrl(@"https://www.youtube.com/watch?v=YYWwIyamQvw&list=RDE0vW5mS0y3U&index=9");
        Log(mediaInfo, "(1)");
        mediaInfo = MediaInfo.ParseMediaUrl(@"https://youtube.com/watch?v=YYWwIyamQvw&list=RDE0vW5mS0y3U&index=9");
        Log(mediaInfo, "(2)");
        mediaInfo = MediaInfo.ParseMediaUrl(@"https://www.xvideos.com/video.okdpihde0a3/_ai_");
        Log(mediaInfo, "(3)");
        mediaInfo = MediaInfo.ParseMediaUrl(
            @"https://jp.xhamster.com/videos/i-found-out-my-best-friends-wife-was-doing-porn-xhXMwoP");
        Log(mediaInfo, "(4)");
        mediaInfo = MediaInfo.ParseMediaUrl(@"https://jp.pornhub.com/view_video.php?viewkey=ph634d54a540f4a");
        Log(mediaInfo, "(5)");
        mediaInfo = MediaInfo.ParseMediaUrl(@"https://www.redtube.com/103102541");
        Log(mediaInfo, "(6)");
        mediaInfo = MediaInfo.ParseMediaUrl(
            @"P:\@porn\【redtube】WOWGIRLS Gorgeous models Eva Elfie and Kate Rich getting fucked by their mutual friend【ID：103102541】.mp4");
        Log(mediaInfo, "(7.1)");
        mediaInfo = MediaInfo.ParseMediaUrl(
            @"P:\@porn\【redtube】WOWGIRLS Gorgeous models Eva Elfie and Kate Rich getting fucked by their mutual friend【ID＝103102541】.mp4");
        Log(mediaInfo, "(7.2)");
        mediaInfo = MediaInfo.ParseMediaUrl(
            @"P:\@porn\++++【redtube】WOWGIRLS Gorgeous models Eva Elfie and Kate Rich getting fucked by their mutual friend【ID=103102541】.mp4");
        Log(mediaInfo, "(7.3)");
        mediaInfo = MediaInfo.ParseMediaUrl(@"C:\テスト\フォルダ\ああああ [xhXMwoP].mp4");
        Log(mediaInfo, "(8)");
#endif
        //Sys.SetCwd(@"C:\abc\def\xyz");
        Log(Sys.LimitStringLength("9MUSES - Glue (Areia Remix) ", 15));
        var assetPath = "assets/text-embed-text-01.json";
        Log(TextEmbedder.ExtractEmbeddedText(assetPath));
        Log(TextEmbedder.GetOriginalContentAsText(assetPath));
        Log(TextEmbedder.GetOriginalContentSize(assetPath));
        var bytes = TextEmbedder.GetHeadBytes(
            assetPath,
            TextEmbedder.GetOriginalContentSize(assetPath)
        );
        Log(bytes);
        var stringFromBytes = Encoding.UTF8.GetString(bytes);
        Log(stringFromBytes);
        bytes = TextEmbedder.GetOriginalContentAsBytes(assetPath)!;
        stringFromBytes = Encoding.UTF8.GetString(bytes!);
        Log(stringFromBytes);
        Log(TextEmbedder.HasEmbeddedText(assetPath));
        Log(TextEmbedder.HasEmbeddedText("assets/list01.txt"));
        Log(TextEmbedder.HasEmbeddedText("assets/not-exists.txt"));
        Log(TextEmbedder.ExtractEmbeddedText("assets/not-exists.txt"));
        Log(TextEmbedder.GetOriginalContentSize("assets/not-exists.txt"));
        Log(TextEmbedder.GetOriginalContentAsText("assets/not-exists.txt"));
        Log(TextEmbedder.GetOriginalContentAsBytes("assets/not-exists.txt"));
        TextEmbedder.InjectEmbeddedText("assets/text-embed-text-02.json", "Hello World!");
        Log(TextEmbedder.ExtractEmbeddedText("assets/text-embed-text-02.json"));
        var jsonPath = Sys.HomeFile("+sub", "nuget.org", "Global.Sys", "Global.Sys.Demo", "assets",
            "text-embed-text-02.json");
        TextEmbedder.InjectEmbeddedText(jsonPath, "Hello World!");
        Log(TextEmbedder.ExtractEmbeddedText(jsonPath));
        var eo = FromFile(jsonPath);
        Log(eo, "eo");
        var remoteJsonPath =
            "https://github.com/nuget-pkg/Global.Sys/blob/2026.0311.1056.12/Global.Sys.Demo/assets/text-embed-text-02.json";
        var eo2 = FromUrl(remoteJsonPath);
        Log(eo2, "eo2");

        var json = Utf8StringFromUrl("https://jsonplaceholder.typicode.com/todos/1");
        Log(json, "json");
        var todo = FromJson(json);
        Log(todo, "todo");

        var todo2 = FromUrl("https://jsonplaceholder.typicode.com/todos/1");
        Log(todo2, "todo2");

        Environment.SetEnvironmentVariable("HOME", "");
        var homeFile2 = Sys.HomeFile("tmp", "test.txt");
        Log(homeFile2, "homeFile2 with empty HOME env");
        //Sys.Crash("demo crash", exitCode: 123);

        Log(Sys.CygpathWindows("/c/home16/cmd"), "cygpath1");
        Log(Sys.CygpathWindows("/mnt/c/home16/cmd"), "cygpath2");

        //Sys.Sleep(1000);
        //Sys.OpenUrl("https://github.com/nuget-pkg/Global.Sys");
    }

    ShowDetail = true;
    DebugOutput = false;
    var gitRoot = Sys.FindGitRoot(Sys.GetCwd());
    Log(gitRoot, "gitRoot");
    var creatdZipPath = Sys.ZipDirectory(Sys.GitProjectFolder(Sys.GetCwd(), "Global.Sys.Demo", "assets")!);
    Log(creatdZipPath, "creatdZipPath");
    var zipInfo = new FileInfo(creatdZipPath);
    Log(zipInfo.Length, "zipInfo.Length");

    var fname = """[1080p] <xml>aaa</xml> ; {Title}!? x=11+22-33; ,(🔥引火★★帝国🔥):"name1" 'name2'?.txt""";
    Log(Sys.AdjustFileName(fname), "adjusted file name");
    Log(Sys.AdjustFileName(fname, ""), "adjusted file name (keeping surrogate pairs)");
    Log(Sys.AdjustFileName(fname, "@"), "adjusted file name (spicifying surrogate pairs' replacement)");

    var embeddedJsonUrl =
        "https://github.com/nuget-pkg/Global.Sys/blob/2026.0321.1925.40/Global.Sys.Demo/assets/text-embed-text-02.json";
    var embeddedEo = FromUrl(embeddedJsonUrl);
    Log(embeddedEo, "embeddedEO(github)");
    var embeddedText = TextEmbedder.ExtractEmbeddedText(embeddedJsonUrl)!;
    Log(embeddedText, "embeddedText(github)");

    embeddedJsonUrl =
        "https://gitlab.com/nuget-tools/nuget-assets/-/blob/2026.0311.1156.53/text-embed-text-02.json?ref_type=tags";
    embeddedEo = FromUrl(embeddedJsonUrl);
    Log(embeddedEo, "embeddedEO(gitlab)");
    embeddedText = TextEmbedder.ExtractEmbeddedText(embeddedJsonUrl)!;
    Log(embeddedText, "embeddedText(gitlab)");

    ////EasyObject embedded1 = EasyObject.ExtractFromFile("https://gitlab.com/nuget-tools/nuget-assets/-/blob/2026.0311.1339.52/json-with-embedded-json.json?ref_type=tags");
    var embedded1 =
        ExtractFromFile(
            "https://gitlab.com/nuget-tools/nuget-assets/-/blob/2026.0321.1903.42/json-with-embedded-json.json?ref_type=tags");
    Log(embedded1, "embedded1(gitlab)");

    var embedded2 =
        ExtractFromFile("https://gitlab.com/nuget-tools/nuget-assets/-/blob/2026.0320.1027.27/my-ls.exe?ref_type=tags");
    Log(embedded2, "embedded2(gitlab:binary file)");

    void LinkTest(string title, string url)
    {
        //LogWebLink(title, url);
        EchoWebLink(title, url);
    }

    LinkTest(
        "⭕️⁅🌐⁆@⁅反転mirror⁆パイパイ仮面でどうかしらん？ / 宝鐘マリン FULL 踊ってみた【練習用】",
        "https://www.youtube.com/watch?v=sLpodTN4xhI&list=PLTvSv0jkjbk9-emLIV2vM-0p7CeMnTYG2"
    );
    LinkTest(
        "⭕️🈂️❝FG⁅ｼﾞﾝｷﾞｽｶﾝ⁆❞🈂️ファイターズガール「ジンギスカン」踊ってみた 歌詞付き",
        "https://www.youtube.com/watch?v=DHbIIBmqHsw&list=PLTvSv0jkjbk8wtAgpVJH1L21EgeMi_ULc"
    );
    LinkTest(
        "⭕️⁅🌐⁆@ラム:DANCING STAR 2026",
        "https://www.youtube.com/watch?v=wzcdhDyNmMM&list=PLTvSv0jkjbk8gtWLMLXLHYrWio5ciOi8c"
    );
    LinkTest(
        "⭕️⁅🌐⁆@エレクトロニック・ダンス・ミュージック",
        "https://www.youtube.com/watch?v=4B5IHILMWOM&list=PLTvSv0jkjbk_u4GZBJK74w7aWylX-8FSt"
    );
    LinkTest(
        "⭕️⁅🌐⁆@⁅CHANNEL：〘!!GREAT!!〙Blackpink Diaries⁆⭕️❝BLACKPINK➡️Ice Cream (2026 Official Music Video)❞",
        "https://www.youtube.com/watch?v=YwhhB8rKb6U&list=PLTvSv0jkjbk9vEyRq7pK_U8fbGrXirdAi"
    );
    LinkTest(
        "⭕️⁅🌐⁆@⁅CHANNEL：〘!!GREAT!!〙Alyssa' s Music Loop⁆⭕️❝🎙 More Than Is Good for Me ( Original ) ✨️ EDM - Electronic Dance Music ✨️ # 179❞",
        "https://www.youtube.com/watch?v=qrW3yK7AWjE&list=PLTvSv0jkjbk-8ABf2TXzCXWk7zn10Ute7"
    );
    LinkTest(
        "⭕️⁅🌐⁆ ▶ ◉ ⁅超美麗アニメーション⁆ ◉",
        "https://www.youtube.com/watch?v=pXXu1HZ2O_U&list=PLTvSv0jkjbk9omW2O3POQEDOu2YYPYpV6"
    );
    LinkTest(
        "⭕️⁅🌐⁆@可愛いBUTTERFLY",
        "https://www.youtube.com/watch?v=snMjVaSYrdY&list=PLTvSv0jkjbk-IF-j3VUnHRoqDL9lbgHpS"
        );

#if true
    //throw new NotImplementedException();
#else
    Sys.Crash(new
    {
        abc = 123,
        xyz = new
        {
            test1 = new[] { "A", "B", "C ハロー©" }
        }
    });
#endif
}
catch (Exception e)
{
    Sys.Crash(e);
}
