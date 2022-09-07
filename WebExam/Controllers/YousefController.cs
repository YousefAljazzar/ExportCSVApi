using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebExam.Models;
using CsvHelper;
namespace WebExam.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class YousefController : ControllerBase
    {

        private ExamDatabaseContext _examDatabaseContext;

        public YousefController(ExamDatabaseContext examDatabaseContext)
        {
            _examDatabaseContext = examDatabaseContext;

        }

        [HttpGet]
        public List<List<string>> Get()
        {
            var list = _examDatabaseContext.Items.Select(p =>
            new List<string>{
                p.Name,
                p.SubCategory.Name,
                p.SubCategory.Category.Name
            }).ToList();
            return list;
        }

        [HttpGet("~/getCSV")]
        public IActionResult ExportCSV()
        {
            var list = Get();
            var config = new CsvConfiguration(CultureInfo.InvariantCulture);

            using (var writer = new StreamWriter("f://Report.csv"))
            using (var csv = new CsvWriter(writer, config))
            {
                foreach (var s in list)
                {
                    csv.WriteField(s);
                    csv.NextRecord();
                }
                writer.Flush();
            }
            return Ok();
        }

    }
}
