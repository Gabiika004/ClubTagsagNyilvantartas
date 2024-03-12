using ClubTagsagNyilvantartas;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

class Program
{

    static async Task Main(string[] args)
    {
        var apiServive = new ApiService();

        // Feladat 1# 
        //Hány tag van

        var members = await apiServive.GetAllMembersAsync();
        Console.WriteLine($"Tagok száma: {members.Count()}");

        Console.WriteLine();

        //Feladat 2#
        //Ki lépett be legutoljára

        var latestEntryMember = GetLatestEntryMember(members);
        if (latestEntryMember != null)
        {
            Console.WriteLine($"A legutoljára belépett tag: {latestEntryMember.Fullname}, belépés ideje: {latestEntryMember.Entry}");
        }
        else
        {
            Console.WriteLine("Nem található belépési adat.");
        }

        Console.WriteLine();

        //Feladat 3#
        //Melyik érdeklődési körbe hányan tartoznak
        Dictionary<string,int> membersInterests = InterestsCountDictiony(members);

        foreach (var interest in membersInterests)
        {
            Console.WriteLine($"{interest.Key}: {interest.Value}");
        }


        Console.ReadKey();
    }

    private static Member GetLatestEntryMember(Member[] members)
    {
        string format = "MMM d, yyyy h:mm tt";
        CultureInfo provider = new CultureInfo("en-US"); 

        var latestEntryMember = members
            .Select(m => new
            {
                Member = m,
                EntryDate = DateTime.TryParseExact(m.Entry, format, provider, DateTimeStyles.None, out DateTime parsedDate) ? parsedDate : (DateTime?)null
            })
            .Where(x => x.EntryDate.HasValue)
            .OrderByDescending(x => x.EntryDate.Value)
            .FirstOrDefault()?.Member;

        return latestEntryMember;
    }


    private static Dictionary<string,int> InterestsCountDictiony(Member[] members)
    {
        Dictionary<string,int> result = new Dictionary<string,int>();
        foreach (var member in members)
        {
            if (result.ContainsKey(member.Interest))
            {
                result[member.Interest]++;
            }
            else
            {
                result.Add(member.Interest, 1);
            }

        }

        return result;
    }
}
