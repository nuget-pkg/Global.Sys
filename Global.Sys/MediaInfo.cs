namespace Global
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.RegularExpressions;
    using static Global.EasyObject;

#if GLOBAL_SYS
    public
#else
    internal
#endif
    static partial class MediaInfo
    {
        public static EasyObject? ParseMediaUrl(string url)
        {
            if (url.Contains("http://") || url.Contains("https://"))
            {
                var m = Sys.FindFirstMatch(url,
                    "http[s]?://.+$");
                if (m != null)
                {
                    url = m[0];
                }
            }
            if (url.StartsWith("http:") || url.StartsWith("https:"))
            {
                var info = NewObject("type", "web", "url", url, "site", "?");
                var dict = Sys.QueryParameterDictionary(url);
                List<string>? m;
                m = Sys.FindFirstMatch(url, "^https://(?:[^.]*[.])?youtube[.]com/watch[?]");
                if (m != null)
                {
                    info["site"] = "youtube";
                    info["videoId"] = dict["v"];
                    if (dict.ContainsKey("list"))
                    {
                        info["playlistId"] = dict["list"];
                    }
                    return info;
                }
                m = Sys.FindFirstMatch(url, "^https://(?:[^.]*[.])?xvideos[.]com/video[.]([^/]+)/");
                if (m != null)
                {
                    info["site"] = "xvideos";
                    info["videoId"] = m[1];
                    return info;
                }
                m = Sys.FindFirstMatch(url, "^https://(?:[^.]*[.])?xhamster[.]com/videos/[^/]*?[-]([^-/]+)$");
                if (m != null)
                {
                    info["site"] = "xhamster";
                    info["videoId"] = m[1];
                    return info;
                }
                m = Sys.FindFirstMatch(url, "^https://(?:[^.]*[.])?pornhub.com/view_video.php[?]");
                if (m != null)
                {
                    info["site"] = "pornhub";
                    info["videoId"] = dict["viewkey"];
                    return info;
                }
                m = Sys.FindFirstMatch(url, "^https://(?:[^.]*[.])?redtube.com/([^/]+)");
                if (m != null)
                {
                    info["site"] = "redtube";
                    info["videoId"] = m[1];
                    return info;
                }
                return info;
            }
            else
            {
                string fileName = "?";
                try
                {
                    fileName = Path.GetFileName(url);
                }
                catch (Exception)
                {
                    ;
                }
                var info = NewObject("type", "file", "fullName", url, "name", fileName, "site", "?");
                fileName = Regex.Replace(fileName, "^[+]+", "");
                List<string>? m;
                m = Sys.FindFirstMatch(
                    fileName,
                    @"^【([^【】]+)】"
                );
                if (m != null)
                {
                    info["site"] = m[1];
                }
                m = Sys.FindFirstMatch(
                    url,
                    @"\[([^\[\]]+)\][.][^.]+$",
                    @"【ID[=＝：]([^【】]+)】[.][^.]+$"
                );
                if (m != null)
                {
                    info["videoId"] = m[1];
                }
                return info;
            }
        }
    }
}
