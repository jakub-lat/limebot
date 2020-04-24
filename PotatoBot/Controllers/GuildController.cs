using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PotatoBot.Bot;
using PotatoBot.Middleware;
using PotatoBot.Models;

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
        public async Task<ActionResult<GuildData>> GetGuildData(ulong id)
        {
            if (!await Authentication.CheckAuth(HttpContext, id.ToString())) return Unauthorized();

            var guildData = await _context.Guilds.FindAsync(id.ToString());

            if (guildData == null)
            {
                if(BotService.instance.IsOnGuild(id))
                {
                    var newGuild = new GuildData()
                    {
                        Id = id.ToString()
                    };
                    _context.Guilds.Add(newGuild);
                    await _context.SaveChangesAsync();
                    return Ok(newGuild);
                } else
                {
                    return NotFound();
                }
            }

            return Ok(guildData);
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGuildData(string id, GuildData guildData)
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

        private bool GuildDataExists(string id)
        {
            return _context.Guilds.Any(e => e.Id == id);
        }
    }
}
