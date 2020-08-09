using Discord;
using Discord.Commands;
using JamyShell.Modules;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace JamyShell.Commands
{
    public class PoggitCommand : Command
    {
        static readonly HttpClient client = new HttpClient();
        string reply = null;
        PoggitPlugin replyJson = null;
        string error = null;


        [Command("poggit")]
        public async Task Poggit(string pluginName)
        {
            Logs.Alert("test");
            await Request(pluginName);
            if (error != null)
            {
                await ReplyAsync("Une erreur est survenue en accédant à l'API de Poggit! Réessayez Ultérieurement!");
            }
            else
            {
                if(replyJson == null)
                {
                    var embed = new EmbedBuilder()
                        .WithColor(Color.Red)
                        .WithAuthor(Context.User)
                        .WithTitle(":x: Poggit API: Plugin introuvable")
                        .WithThumbnailUrl("https://poggit.pmmp.io/res/defaultPluginIcon2.png")
                        .WithDescription(" > La commande **!poggit < nom du plugin > **vous permet d'obtenir des informations sur des plugins présent sur poggit. \n\n > **Plugin Recherché:** \n" + pluginName + "\nAucun plugin correspondant n'a été trouvé, l'api actuelle de poggit ne permet pas de trouver un plugin au nom correspondant. \n> Avez-vous essayez les plugin [**SeeDevice**](https://poggit.pmmp.io/p/SeeDevice) et [**LuckyBlock**](https://poggit.pmmp.io/p/LuckyBlock)")
                        .WithFooter("@Secure - Heberg x Poggit")
                        .WithCurrentTimestamp()
                        .Build();
                    await ReplyAsync(embed: embed);
                }
                else
                {
                    var field = new EmbedFieldBuilder()
                            .WithIsInline(false)
                            .WithName("Téléchargement")
                            .WithValue("Vous pouvez téléchargez la dernière version en production [ici](" + replyJson.Artifact_url + "/" + replyJson.Name + ".phar)\nVous pouvez aussi sélectionnez la version souhaitez [ici](" + replyJson.Html_url + ")");
                    var embed = new EmbedBuilder()
                        .WithColor(255, 0, 255)
                        .WithAuthor(Context.User)
                        .WithTitle("Poggit API: Plugin Trouvé!")
                        .WithThumbnailUrl((string)replyJson.Icon_url ?? "https://poggit.pmmp.io/res/defaultPluginIcon2.png")
                        .WithDescription("> La commande **!poggit <nom du plugin>** vous permet d'obtenir des informations sur des plugins présent sur poggit. \n \n> **Description du plugin (" + replyJson.Name + "):** \n" + replyJson.Tagline + "\n> **Version du plugin**\n" + replyJson.Version + "\n> **API:**\n[" + ((replyJson.Api is null || replyJson.Api.Count == 0) ? "Ce plugin ne déclare pas d'api" : replyJson.Api[0].From + "; " + replyJson.Api[0].To) + "]\n> **Catégorie: ** \n " + replyJson.Categories[0].Category_name + "\n> **Dépendance:**\n" + (!(replyJson.Deps is null && replyJson.Deps.Count > 0) ? "Ce plugin ne possède pas de dépendance" : replyJson.Deps[0].Name + " (**Voir Plus sur le site**)") + "\n> **License:** \n" + replyJson.License + "\n> **Téléchargements: ** \n" + replyJson.Downloads + " téléchargements")
                        .AddField(field)
                        .WithCurrentTimestamp()
                        .WithFooter("@Secure - Heberg x Poggit")
                        .Build();
                    await ReplyAsync(embed: embed);
                }
            }
        }

        public async Task Request(string plugin)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("http://poggit.pmmp.io/releases.json?name="+plugin);
                response.EnsureSuccessStatusCode();
                reply = await response.Content.ReadAsStringAsync();
                Logs.Debug(reply);
                if (!reply.Equals("[]"))
                {
                    var replyJsonArray = JsonConvert.DeserializeObject<List<PoggitPlugin>>(reply);
                    replyJson = replyJsonArray[0];
                }
            }
            catch (HttpRequestException e)
            {
                Logs.Alert("Exception Caught!");
                Logs.Alert("Message : "+e.Message);
                error = e.Message.ToString();
            }
        }
        public override string GetDescription()
        {
            return "Poggit Command!";
        }

        public override int GetCoolDown()
        {
            throw new System.NotImplementedException();
        }
    }

    public class Category
    {
        public bool Major { get; set; }
        public string Category_name { get; set; }
    }

    public class Api
    {
        public string From { get; set; }
        public string To { get; set; }
    }

    public class Dep
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public int DepRelId { get; set; }
        public bool IsHard { get; set; }
    }

    public class Producers
    {
        public IList<string> Collaborator { get; set; }
    }

    public class PoggitPlugin
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public string Html_url { get; set; }
        public string Tagline { get; set; }
        public string Artifact_url { get; set; }
        public int Downloads { get; set; }
        public int Repo_id { get; set; }
        public string Repo_name { get; set; }
        public int Project_id { get; set; }
        public string Project_name { get; set; }
        public int Build_id { get; set; }
        public int Build_number { get; set; }
        public string Build_commit { get; set; }
        public string Description_url { get; set; }
        public object Icon_url { get; set; }
        public string Changelog_url { get; set; }
        public string License { get; set; }
        public object License_url { get; set; }
        public bool Is_obsolete { get; set; }
        public bool Is_pre_release { get; set; }
        public bool Is_outdated { get; set; }
        public bool Is_official { get; set; }
        public int Submission_date { get; set; }
        public int State { get; set; }
        public int Last_state_change_date { get; set; }
        public IList<Category> Categories { get; set; }
        public IList<string> Keywords { get; set; }
        public IList<Api> Api { get; set; }
        public IList<Dep> Deps { get; set; }
        public Producers Producers { get; set; }
        public string State_name { get; set; }
    }
}
