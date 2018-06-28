using microCommerce.Dapper;
using microCommerce.Domain.Settings;
using microCommerce.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace microCommerce.SettingApi.Controllers
{
    public class SettingController : ServiceBaseController
    {
        private readonly IDataContext _dataContext;

        public SettingController(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet("/settings")]
        public virtual async Task<IActionResult> GetSetting()
        {
            var settings = await _dataContext.QueryAsync<Setting>("SELECT Id, Name, Value FROM Setting");

            return Json(settings);
        }

        [HttpGet("/settings/{Id:int}")]
        public virtual async Task<IActionResult> GetSettingById(int Id)
        {
            if (Id == 0)
                return NotFound();

            var setting = await _dataContext.FirstAsync<Setting>("SELECT * FROM Setting WHERE Id = @Id Limit 1", new { Id });
            if (setting == null)
                return NotFound();

            return Json(setting);
        }

        [HttpGet("/settings/find/")]
        public virtual async Task<IActionResult> GetSettingByName(string settingName)
        {
            if (string.IsNullOrEmpty(settingName))
                return BadRequest();

            var setting = await _dataContext.FirstAsync<Setting>("SELECT * FROM Setting WHERE Name = @settingName Limit 1", new { settingName });
            if (setting == null)
                return NotFound();

            return Json(setting);
        }

        [HttpPost("/settings")]
        public virtual async Task<IActionResult> InsertSetting(Setting setting)
        {
            if (setting == null)
                return BadRequest();

            await _dataContext.InsertAsync(setting);

            if (setting.Id == 0)
                return BadRequest();

            return Ok();
        }

        [HttpPut("/settings")]
        public virtual async Task<IActionResult> UpdateSetting(Setting setting)
        {
            if (setting == null)
                return BadRequest();

            setting = await _dataContext.FirstAsync<Setting>("SELECT * FROM Setting WHERE Id = @Id Limit 1", new { setting.Id });
            if (setting == null)
                return NotFound();

            await _dataContext.UpdateAsync(setting);

            return Ok();
        }

        [HttpDelete("/settings/{Id:int}")]
        public virtual async Task<IActionResult> DeleteSetting(int Id)
        {
            if (Id == 0)
                return BadRequest();

            var setting = await _dataContext.FirstAsync<Setting>("SELECT * FROM Setting WHERE Id = @Id Limit 1", new { Id });
            if (setting == null)
                return NotFound();

            await _dataContext.DeleteAsync(setting);

            return Ok();
        }
    }
}