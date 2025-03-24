using System.Text.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

public static class SetsAndMaps
{
    /// <summary>
    /// The words parameter contains a list of two-character 
    /// words (lower case, no duplicates). Using sets, find an O(n) 
    /// solution for returning all symmetric pairs of words.  
    ///
    /// For example, if words was: [am, at, ma, if, fi], we would return :
    ///
    /// ["am & ma", "if & fi"]
    ///
    /// The order of the array does not matter, nor does the order of the specific words in each string in the array.
    /// "at" would not be returned because "ta" is not in the list of words.
    ///
    /// As a special case, if the letters are the same (example: 'aa') then
    /// it would not match anything else (remember the assumption above
    /// that there were no duplicates) and therefore should not be returned.
    /// </summary>
    public static string[] FindPairs(string[] words)
    {
        var wordSet = new HashSet<string>(words);
        var pairs = new List<string>();
        var seen = new HashSet<string>();

        foreach (string word in words)
        {
            string reversed = new string(new char[] { word[1], word[0] });

            if (wordSet.Contains(reversed) && word != reversed && !seen.Contains(word) && !seen.Contains(reversed))
            {
                pairs.Add($"{word} & {reversed}");
                seen.Add(word);
                seen.Add(reversed);
            }
        }

        return pairs.ToArray();
    }

    /// <summary>
    /// Reads a census file and summarizes the degrees (education)
    /// earned by those contained in the file. The summary
    /// should be stored in a dictionary where the key is the
    /// degree earned and the value is the number of people that 
    /// have earned that degree.
    /// </summary>
    public static Dictionary<string, int> SummarizeDegrees(string filename)
    {
        var degrees = new Dictionary<string, int>();

        foreach (var line in File.ReadLines(filename))
        {
            var fields = line.Split(',');

            if (fields.Length < 4) continue;

            string degree = fields[3].Trim();

            if (!string.IsNullOrEmpty(degree))
            {
                degrees[degree] = degrees.GetValueOrDefault(degree, 0) + 1;
            }
        }

        return degrees;
    }

    /// <summary>
    /// Determines if 'word1' and 'word2' are anagrams.
    /// </summary>
    public static bool IsAnagram(string word1, string word2)
    {
        word1 = word1.Replace(" ", "").ToLower();
        word2 = word2.Replace(" ", "").ToLower();

        if (word1.Length != word2.Length) return false;

        var letterCount = new Dictionary<char, int>();

        foreach (char c in word1)
        {
            if (letterCount.TryGetValue(c, out int count))
                letterCount[c] = count + 1;
            else
                letterCount[c] = 1;
        }

        foreach (char c in word2)
        {
            if (!letterCount.ContainsKey(c) || letterCount[c] == 0)
                return false;

            letterCount[c]--;
        }

        return true;
    }
}

/// <summary>
/// Program class that fetches earthquake data from USGS.
/// </summary>
public class Program
{
    public static string[] EarthquakeDailySummary()
    {
        const string uri = "https://earthquake.usgs.gov/earthquakes/feed/v1.0/summary/all_day.geojson";

        using var client = new HttpClient();
        var json = client.GetStringAsync(uri).Result;
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        var featureCollection = JsonSerializer.Deserialize<FeatureCollection>(json, options);

        if (featureCollection?.Features == null) return Array.Empty<string>();

        var summaries = new List<string>();

        foreach (var feature in featureCollection.Features)
        {
            summaries.Add($"Magnitude: {feature.Properties.Mag}, Location: {feature.Properties.Place}, Time: {UnixTimeToDate(feature.Properties.Time)}");
        }

        return summaries.ToArray();
    }

    private static string UnixTimeToDate(long unixTime)
    {
        return DateTimeOffset.FromUnixTimeMilliseconds(unixTime).ToString("yyyy-MM-dd HH:mm:ss");
    }

    public static void Main()
    {
        var summaries = EarthquakeDailySummary();
        foreach (var summary in summaries)
        {
            Console.WriteLine(summary);
        }
    }
}

public class FeatureCollection
{
    public List<Feature> Features { get; set; }
}

public class Feature
{
    public FeatureProperties Properties { get; set; }
}

public class FeatureProperties
{
    [JsonPropertyName("mag")]
    public double Mag { get; set; }

    [JsonPropertyName("place")]
    public string Place { get; set; }

    [JsonPropertyName("time")]
    public long Time { get; set; }
}
