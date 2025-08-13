using System.Net.Http.Json;

// Models
public class Weather
{
    public string Type { get; set; }
    public bool Active { get; set; }
    public List<string> Effects { get; set; }
}

public class GearItem
{
    public string Name { get; set; }
    public int Quantity { get; set; }
}

public class Seed
{
    public string Name { get; set; }
    public int Quantity { get; set; }
}

public class Egg
{
    public string Name { get; set; }
    public int Quantity { get; set; }
}

public class EventItem
{
    public string Name { get; set; }
    public int Quantity { get; set; }
    public bool Available { get; set; }
}

public class ChrisPCraving
{
    public string Food { get; set; }
    public string Message { get; set; }
}

public class ApiResponse
{
    public Weather Weather { get; set; }
    public List<GearItem> Gear { get; set; }
    public List<Seed> Seeds { get; set; }
    public List<Egg> Eggs { get; set; }
    public List<EventItem> Events { get; set; }
    public ChrisPCraving ChrisPCraving { get; set; }
}

class Program
{
    static async Task Main()
    {
        string apiUrl = "https://gagapi.onrender.com/alldata";
        string discordWebhook = "https://discord.com/api/webhooks/1405205335035084810/4KeFzjsdgoD7GMv61t5vXU0RBA6uV91tVkU14_TWcL1otgFpPc8Gvmil8TGXX3fwsQEu"; // Replace with your webhook

        var http = new HttpClient();

        // Get data
        var data = await http.GetFromJsonAsync<ApiResponse>(apiUrl);

        if (data == null)
        {
            Console.WriteLine("No data received.");
            return;
        }
        var Gear = new List<string> { "Basic Sprinkler", "Advanced Sprinkler", "Godly Sprinkler", "Master Sprinkler", "Grandmaster Sprinkler", "Medium Toy", "Levelup Lollipop" };
        var Seeds = new List<string> { "Burning Bud", "Elder Strawberry", "Giant Pinecone", "Sugar Apple", "Ember Lily", "Beanstalk" };
        var Eggs = new List<string> { "Mythical Egg", "Paradise Egg", "Bug Egg" };

        // Build Discord-friendly message
        var message = $"@everyone Hello! \n **🌦 Weather:** {data.Weather?.Type}\n";
        if (data.Weather?.Effects != null && data.Weather.Effects.Count > 0)
        {
            message += "Effects:\n" + string.Join("\n", data.Weather.Effects.Select(e => $"- {e}")) + "\n";
        }

        if (data.Gear != null && data.Gear.Count > 0)
        {
            message += "\n**🛠 Gear:**\n" + string.Join("\n", data.Gear.Where(i=>Gear.Contains(i.Name)).Select(g => $"• {g.Name} x{g.Quantity}"));
        }

        if (data.Seeds != null && data.Seeds.Count > 0)
        {
            message += "\n\n**🌱 Seeds:**\n" + string.Join("\n", data.Seeds.Where(i => Seeds.Contains(i.Name)).Select(s => $"• {s.Name} x{s.Quantity}"));
        }

        if (data.Eggs != null && data.Eggs.Count > 0)
        {
            message += "\n\n**🥚 Eggs:**\n" + string.Join("\n", data.Eggs.Where(i => Eggs.Contains(i.Name)).Select(e => $"• {e.Name} x{e.Quantity}"));
        }

        if (data.ChrisPCraving != null)
        {
            message += $"\n\n**🍩 Chris P. is craving {data.ChrisPCraving.Food}!**\n{data.ChrisPCraving.Message}";
        }

        // Send to Discord
        var payload = new { content = message };
        var response = await http.PostAsJsonAsync(discordWebhook, payload);

        Console.WriteLine($"Discord status: {response.StatusCode}");
    }
}
