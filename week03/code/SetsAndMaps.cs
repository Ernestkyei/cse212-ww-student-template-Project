using System.Text.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json.Serialization;
public static class SetsAndMaps
{
    /// <summary>
    /// The words parameter contains a list of two character 
    /// words (lower case, no duplicates). Using sets, find an O(n) 
    /// solution for returning all symmetric pairs of words.  
    ///
    /// For example, if words was: [am, at, ma, if, fi], we would return :
    ///
    /// ["am & ma", "if & fi"]
    ///
    /// The order of the array does not matter, nor does the order of the specific words in each string in the array.
    /// at would not be returned because ta is not in the list of words.
    ///
    /// As a special case, if the letters are the same (example: 'aa') then
    /// it would not match anything else (remember the assumption above
    /// that there were no duplicates) and therefore should not be returned.
    /// </summary>
    /// <param name="words">An array of 2-character words (lowercase, no duplicates)</param>
    public static string[] FindPairs(string[] words)
    {
        // TODO Problem 1 - ADD YOUR CODE HERE
        var wordSet = new HashSet<string>(words); // Store words for fast lookup
        var pairs = new List<string>();
        var seen = new HashSet<string>(); // To track already processed words

        foreach (string word in words)
        {
            string reversed = new string(new char[] { word[1], word[0] });

            // Check if the reversed word exists and hasn't been added already
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
    /// Read a census file and summarize the degrees (education)
    /// earned by those contained in the file.  The summary
    /// should be stored in a dictionary where the key is the
    /// degree earned and the value is the number of people that 
    /// have earned that degree.  The degree information is in
    /// the 4th column of the file.  There is no header row in the
    /// file.
    /// </summary>
    /// <param name="filename">The name of the file to read</param>
    /// <returns>fixed array of divisors</returns>
    public static Dictionary<string, int> SummarizeDegrees(string filename)
    {
        var degrees = new Dictionary<string, int>();

        foreach (var line in File.ReadLines(filename))
        {
            var fields = line.Split(',');

            if (fields.Length < 4) continue;

            string degree = fields[3].Trim(); // Extract and clean the degree field

            if (!string.IsNullOrEmpty(degree))
            {
                degrees[degree] = degrees.GetValueOrDefault(degree, 0) + 1;
            }
        }

        return degrees;
    }


    /// <summary>
    /// Determine if 'word1' and 'word2' are anagrams.  An anagram
    /// is when the same letters in a word are re-organized into a 
    /// new word.  A dictionary is used to solve the problem.
    /// 
    /// Examples:
    /// is_anagram("CAT","ACT") would return true
    /// is_anagram("DOG","GOOD") would return false because GOOD has 2 O's
    /// 
    /// Important Note: When determining if two words are anagrams, you
    /// should ignore any spaces.  You should also ignore cases.  For 
    /// example, 'Ab' and 'Ba' should be considered anagrams
    /// 
    /// Reminder: You can access a letter by index in a string by 
    /// using the [] notation.
    /// </summary>
    public static bool IsAnagram(string word1, string word2)
    {
        // TODO Problem 3 - ADD YOUR CODE HERE
        // Normalize words: Remove spaces and convert to lowercase
        word1 = word1.Replace(" ", "").ToLower();
        word2 = word2.Replace(" ", "").ToLower();

        if (word1.Length != word2.Length)
            return false; // If lengths differ, they cannot be anagrams

        var letterCount = new Dictionary<char, int>();

        // Count occurrences of each letter in word1
        foreach (char c in word1)
        {
            if (letterCount.TryGetValue(c, out int count))
                letterCount[c] = count + 1;
            else
                letterCount[c] = 1;
        }

        // Subtract occurrences for each letter in word2
        foreach (char c in word2)
        {
            if (!letterCount.ContainsKey(c) || letterCount[c] == 0)
                return false; // If a letter is missing or overused, not an anagram

            letterCount[c]--;
        }

            return true;
    }

    /// <summary>
    /// This function will read JSON (Javascript Object Notation) data from the 
    /// United States Geological Service (USGS) consisting of earthquake data.
    /// The data will include all earthquakes in the current day.
    /// 
    /// JSON data is organized into a dictionary. After reading the data using
    /// the built-in HTTP client library, this function will return a list of all
    /// earthquake locations ('place' attribute) and magnitudes ('mag' attribute).
    /// Additional information about the format of the JSON data can be found 
    /// at this website:  
    /// 
    /// https://earthquake.usgs.gov/earthquakes/feed/v1.0/geojson.php
    /// 
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

