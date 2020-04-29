using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PotatoBot.Bot;
using PotatoBot.Middleware;
using PotatoBot.Models;
using PotatoBot.Utils;

namespace PotatoBot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuildController : ControllerBase
    {
        private readonly GuildContext _context;

        public GuildController(GuildContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GuildResult>> GetGuildData(ulong id)
        {
            if (!await Authentication.CheckAuth(HttpContext, id)) return Unauthorized();

            var guildData = await _context.Guilds.FindAsync(id);

            if (guildData == null)
            {
                if(BotService.instance.IsOnGuild(id))
                {
                    var newGuild = await _context.InsertGuild(id);
                } else
                {
                    return NotFound();
                }
            }

            var discordGuild = await BotService.instance.discord.GetGuildAsync(id);

            return Ok(new GuildResult
            {
                Id = guildData.Id,
                Settings = guildData,
                Info = new GuildInfo
                {
                    Roles = discordGuild.Roles.Values
                        .Where(i => !i.IsManaged && i.Name != "@everyone")
                        .Select(i => new DiscordRole
                        {
                            Id = i.Id,
                            Name = i.Name,
                            Color = i.Color.ToString(),
                            Position = i.Position
                        }).OrderBy(i=>i.Position).ToList(),
                    Channels = discordGuild.Channels.Values
                        .Select(i => new DiscordChannel
                        {
                            Id = i.Id,
                            Name = i.Name,
                            Type = i.Type.ToString(),
                            Parent = i.Parent?.Name
                        }).ToList()
                }
            });
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGuildData(ulong id, GuildData guildData)
        {
            if (!await Authentication.CheckAuth(HttpContext, id)) return Unauthorized();
            if (id != guildData.Id)
            {
                return BadRequest();
            }

            _context.Entry(guildData).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GuildDataExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(guildData);
        }

        // POST: api/Guild
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        /*
        [HttpPost]
        public async Task<ActionResult<GuildData>> PostGuildData(GuildData guildData)
        {
            _context.Guilds.Add(guildData);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGuildData", new { id = guildData.Id }, guildData);
        }
        */

        private bool GuildDataExists(ulong id)
        {
            return _context.Guilds.Any(e => e.Id == id);
        }
    }
}
