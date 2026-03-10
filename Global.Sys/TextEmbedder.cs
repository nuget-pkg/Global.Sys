using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using static Global.EasyObject;
namespace Global;

public static class TextEmbedder
{
    const long MinimumCheckLength = 8192;
    //const long MinimumCheckLength = 256;

    static TextEmbedder()
    {
        //Log("TextEmbedder initialized");
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }
    internal class SearchResult
    {
        public long Length {
            get; set;
        }
        public long StartPos {
            get; set;
        }
        public long EndPos {
            get; set;
        }
    }
    private static SearchResult CheckTailBytes(long offset, byte[] bytes)
    {
        SearchResult result = new SearchResult() {
            Length = offset + bytes.Length, StartPos = -1, EndPos = -1
        };
        const string neutral = "IBM437";
        string part = Encoding.GetEncoding(neutral).GetString(bytes);
        string pattern = @"\[/embed(:[-0-9a-zA-Z]+)?\]\s*$";
        Match m = Regex.Match(part, pattern);
        if (m.Success)
        {
            string startTag = $"[embed{m.Groups[1].Value}]";
            string endTag = $"[/embed{m.Groups[1].Value}]";
            result.EndPos = part.LastIndexOf(endTag);
            if (result.EndPos >= 0)
            {
                int idx = part.LastIndexOf(startTag, (int)result.EndPos);
                if (idx >= 0)
                {
                    result.Length = offset + idx;
                    result.StartPos = idx + startTag.Length;
                    long len = result.EndPos - result.StartPos;
                    string s = part.Substring((int)result.StartPos, (int)len);
                }
            }
        }
        return result;
    }
    private static long GetLength(string path)
    {
        try
        {
            if (path.StartsWith("http:") || path.StartsWith("https:"))
            {
                using (var fs = new PartialHTTPStream(path))
                {
                    return fs.Length;
                }
            }
            using (var fs = File.OpenRead(path))
            {
                return fs.Length;
            }
        }
        catch (Exception /*e*/)
        {
            //Log(e.ToString());
            return 0;
        }
    }
    public static byte[] GetHeadBytes(string path, long size)
    {
        try
        {
            if (path.StartsWith("http:") || path.StartsWith("https:"))
            {
                using (var fs = new PartialHTTPStream(path))
                {
                    long fileLen = fs.Length;
                    if (size > fileLen) size = fileLen;
                    byte[] result = new byte[size];
                    fs.Read(result, 0, result.Length);
                    return result;
                }
            }
            using (var fs = File.OpenRead(path))
            {
                long fileLen = fs.Length;
                if (size > fileLen) size = fileLen;
                byte[] result = new byte[size];
                fs.Read(result, 0, result.Length);
                return result;
            }
        }
        catch (Exception /*e*/)
        {
            //Log(e.ToString());
            return [];
        }
    }
    public static byte[] GetTailBytes(string path, long size)
    {
        try
        {
            if (path.StartsWith("http:") || path.StartsWith("https:"))
            {
                using (var fs = new PartialHTTPStream(path))
                {
                    long fileLen = fs.Length;
                    if (size > fileLen) size = fileLen;
                    long pos = fileLen - size;
                    byte[] result = new byte[size];
                    fs.Seek(pos, SeekOrigin.Begin);
                    fs.Read(result, 0, result.Length);
                    return result;
                }
            }
            using (var fs = File.OpenRead(path))
            {
                long fileLen = fs.Length;
                if (size > fileLen) size = fileLen;
                long pos = fileLen - size;
                byte[] result = new byte[size];
                fs.Seek(pos, SeekOrigin.Begin);
                fs.Read(result, 0, result.Length);
                return result;
            }
        }
        catch (Exception /*e*/)
        {
            //Log(e.ToString());
            return [];
        }
    }
    public static bool HasEmbeddedText(string path)
    {
        try
        {
            long fileLen = GetLength(path);
            long checkLen = MinimumCheckLength;
            while (true)
            {
                if (checkLen > fileLen) checkLen = fileLen;
                byte[] check = GetTailBytes(path, checkLen);
                SearchResult checkResult = CheckTailBytes(fileLen - checkLen, check);
                if (checkResult.EndPos < 0) return false;
                if (checkResult.StartPos >= 0) return true;
                if (checkLen >= fileLen) return false;
                checkLen *= 2;
            }
        }
        catch (Exception e)
        {
            Log(e.ToString());
            return false;
        }
    }
    static int seed = Environment.TickCount;
    public static string GetRandomDigits(/*int length*/)
    {
        string guid = Sys.GuidString();
        //return guid.Replace("-", "");
        return guid;
        //Random rnd = new Random(seed++);
        //string randomDigits =
        //    Sys.RandomString(rnd,
        //    ["a", "b", "c", "d", "e", "f",
        //    "0", "1", "2", "3", "4", "5", "6", "7", "8", "9"],
        //    length);
        //return randomDigits;
    }
    public static void ClearEmbeddedText(string path)
    {
        try
        {
            long fileLen = GetLength(path);
            long contentSize = GetOriginalContentSize(path);
            if (fileLen == contentSize) return;
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Write))
            {
                fs.SetLength(contentSize);
            }
        }
        catch (Exception e)
        {
            Log(e.ToString());
        }
    }
    public static void SetEmbeddedText(string path, string text)
    {
        try
        {
            if (HasEmbeddedText(path))
            {
                ClearEmbeddedText(path);
            }
            string randomDigits = GetRandomDigits();
            string embedText = $"[embed:{randomDigits}]{text}[/embed:{randomDigits}]";
            byte[] embedBytes = Encoding.UTF8.GetBytes(embedText);
            using (var fs = new FileStream(path, FileMode.Append, FileAccess.Write))
            {
                fs.Write(embedBytes, 0, embedBytes.Length);
            }
        }
        catch (Exception e)
        {
            Log(e.ToString());
        }
    }
    public static string? GetEmbeddedText(string path)
    {
        try
        {
            long fileLen = GetLength(path);
            long checkLen = MinimumCheckLength;
            while (true)
            {
                if (checkLen > fileLen) checkLen = fileLen;
                {
                }
                byte[] check = GetTailBytes(path, checkLen);
                SearchResult checkResult = CheckTailBytes(fileLen - checkLen, check);
                if (checkResult.EndPos < 0) return null;
                if (checkResult.StartPos >= 0)
                {
                    long len = checkResult.EndPos - checkResult.StartPos;
                    byte[] result = new byte[len];
                    Array.Copy(check, checkResult.StartPos, result, 0, len);
                    return Encoding.UTF8.GetString(result).Trim();
                }
                if (checkLen >= fileLen)
                {
                    return null;
                }
                checkLen *= 2;
            }
        }
        catch (Exception e)
        {
            Log(e.ToString());
            return null;
        }
    }
    public static long GetOriginalContentSize(string path)
    {
        try
        {
            long fileLen = GetLength(path);
            long checkLen = MinimumCheckLength;
            while (true)
            {
                if (checkLen > fileLen) checkLen = fileLen;
                byte[] check = GetTailBytes(path, checkLen);
                SearchResult checkResult = CheckTailBytes(fileLen - checkLen, check);
                if (checkResult.EndPos < 0) return checkResult.Length;
                if (checkResult.StartPos >= 0)
                {
                    return checkResult.Length;
                }
                if (checkLen >= fileLen)
                {
                    return checkResult.Length;
                }
                checkLen *= 2;
            }
        }
        catch (Exception e)
        {
            Log(e.ToString());
            return 0;
        }
    }
    public static string? GetOriginalContentAsText(string path)
    {
        try
        {
            long size = GetOriginalContentSize(path);
            return Encoding.UTF8.GetString(GetHeadBytes(path, size));
        }
        catch (Exception e)
        {
            Log(e.ToString());
            return null;
        }
    }
    public static byte[]? GetOriginalContentAsBytes(string path)
    {
        try
        {
            long size = GetOriginalContentSize(path);
            return GetHeadBytes(path, size);
        }
        catch (Exception e)
        {
            Log(e.ToString());
            return null;
        }
    }
}
