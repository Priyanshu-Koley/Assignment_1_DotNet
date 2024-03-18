using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace KeyValueService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KeysController : ControllerBase
    {
        private readonly KeyValueDbContext _context;

        public KeysController(KeyValueDbContext context)
        {
            _context = context;
        }

        [HttpGet("{key}")]
        public async Task<IActionResult> Get(string key)
        {
            var keyValue = await _context.KeyValuePairs.FirstOrDefaultAsync(kvp => kvp.Key == key);

            if (keyValue != null)
            {
                return Ok(new { key = keyValue.Key, value = keyValue.Value });
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add(KeyValuePairDto dto)
        {
            var existingKeyValue = await _context.KeyValuePairs.FirstOrDefaultAsync(kvp => kvp.Key == dto.Key);

            if (existingKeyValue != null)
            {
                return Conflict();
            }

            var newKeyValue = new KeyValuePair { Key = dto.Key, Value = dto.Value };
            _context.KeyValuePairs.Add(newKeyValue);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPatch("{key}/{value}")]
        public async Task<IActionResult> Update(string key, string value)
        {
            var keyValue = await _context.KeyValuePairs.FirstOrDefaultAsync(kvp => kvp.Key == key);

            if (keyValue != null)
            {
                keyValue.Value = value;
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete("{key}")]
        public async Task<IActionResult> Delete(string key)
        {
            var keyValue = await _context.KeyValuePairs.FirstOrDefaultAsync(kvp => kvp.Key == key);

            if (keyValue != null)
            {
                _context.KeyValuePairs.Remove(keyValue);
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }

    public class KeyValueDbContext : DbContext
    {
        public KeyValueDbContext(DbContextOptions<KeyValueDbContext> options) : base(options) { }

        public DbSet<KeyValuePair> KeyValuePairs { get; set; }
    }

    public class KeyValuePair
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class KeyValuePairDto
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
