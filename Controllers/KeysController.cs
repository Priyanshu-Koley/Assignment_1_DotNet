// Ignore Spelling: dto

using Assignment_1_DotNet.Data;
using Assignment_1_DotNet.Entities;
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
        public async Task<IActionResult> GetKeyValuePairByKeyAsync([FromRoute] string key)
        {
            try
            {
                var keyValues = await _context.KeyValuePairs.Where(kvp => kvp.Key == key).ToListAsync();

                if (keyValues.Count != 0)
                {
                    //return Ok(new { key = keyValue.Key, value = keyValue.Value });
                    return Ok(keyValues);
                }
                else
                {
                    return NotFound();
                }

            }
            catch(Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddKeyValuePairAsync([FromBody]KeyValueDto dto)
        {
            try
            {
                var existingKeyValue = await _context.KeyValuePairs.FirstOrDefaultAsync(kvp => kvp.Key == dto.Key);

                if (existingKeyValue != null)
                {
                    return Conflict();
                }

                var newKeyValue = new KeyValue { Key = dto.Key, Value = dto.Value };
                _context.KeyValuePairs.Add(newKeyValue);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPatch("{key}/{value}")]
        public async Task<IActionResult> UpdateKeyValuePairByKeyAsync([FromRoute] string key, [FromRoute] string value)
        {
            try
            {
                var keyValues = await _context.KeyValuePairs.Where(kvp => kvp.Key == key).ToListAsync();

                if (keyValues.Count != 0)
                {
                    foreach (var keyValue in keyValues)
                    {
                        keyValue.Value = value;
                    }
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{key}")]
        public async Task<IActionResult> DeleteKeyValuePairByKeyAsync([FromRoute] string key)
        {
            try
            {
                var keyValues = await _context.KeyValuePairs.Where(kvp => kvp.Key == key).ToListAsync();

                if (keyValues.Count != 0)
                {
                    foreach (var keyValue in keyValues)
                    {
                        _context.KeyValuePairs.Remove(keyValue);
                    }
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
