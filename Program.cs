using System;
using System.Collections.Generic;
using System.Linq;

public class Program
{
    public static void Main()
    {
        List<Group> groupList = new List<Group>
        {
            new Group { GroupId = 1, Member1 = 1, Member2 = 2, Member3 = 3 },
            new Group { GroupId = 2, Member1 = 4, Member2 = 5, Member3 = 6 }
        };

        List<Member> memberList = new List<Member>
        {
            new Member { MemberId = 1, FirstName = "John", LastName = "Doe" },
            new Member { MemberId = 2, FirstName = "Jane", LastName = "Smith" },
            new Member { MemberId = 3, FirstName = "Jim", LastName = "Brown" },
            new Member { MemberId = 4, FirstName = "Jack", LastName = "Black" },
            new Member { MemberId = 5, FirstName = "Jill", LastName = "White" }
            // Note: Member with MemberId = 6 is missing to demonstrate the left join
        };

        var result = groupList
            .GroupJoin(memberList, group => group.Member1, member => member.MemberId, (group, members) => new { group, members })
            .SelectMany(g => g.members.DefaultIfEmpty(), (g, m1) => new { g.group, Member1 = m1 })

            .GroupJoin(memberList, gm1 => gm1.group.Member2, member => member.MemberId, (gm1, members) => new { gm1, members })
            .SelectMany(g => g.members.DefaultIfEmpty(), (g, m2) => new { g.gm1.group, g.gm1.Member1, Member2 = m2 })

            .GroupJoin(memberList, gm2 => gm2.group.Member3, member => member.MemberId, (gm2, members) => new { gm2, members })
            .SelectMany(g => g.members.DefaultIfEmpty(), (g, m3) => new
            {
                g.gm2.group.GroupId,
                Member1 = g.gm2.Member1 != null ? g.gm2.Member1.FirstName + " " + g.gm2.Member1.LastName : "N/A",
                Member2 = g.gm2.Member2 != null ? g.gm2.Member2.FirstName + " " + g.gm2.Member2.LastName : "N/A",
                Member3 = m3 != null ? m3.FirstName + " " + m3.LastName : "N/A"
            });

        foreach (var item in result)
        {
            Console.WriteLine($"Group ID: {item.GroupId}");
            Console.WriteLine($"Member 1: {item.Member1}");
            Console.WriteLine($"Member 2: {item.Member2}");
            Console.WriteLine($"Member 3: {item.Member3}");
            Console.WriteLine();
        }
    }
}

public class Group
{
    public long GroupId { get; set; }
    public long Member1 { get; set; }
    public long Member2 { get; set; }
    public long Member3 { get; set; }
}

public class Member
{
    public long MemberId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}
