using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;

[ApiController]
[Route("api/crimeDataStatistics/caseCityStatistics")]
public class cityStatisticsControllerZYH : ControllerBase
{
    [HttpGet]
    public IActionResult HandleEndpoint()
    {
        CaseStatisticsZYH statistics = new CaseStatisticsZYH();
        statistics.getCityStatistics();
        string jsonBackData = JsonSerializer.Serialize(statistics.cityCount);
        return Ok(jsonBackData);
    }
}

[ApiController]
[Route("api/crimeDataStatistics/cityDateStatistics")]
public class cityDateStatisticsControllerZYH : ControllerBase
{
    [HttpGet]
    public IActionResult HandleEndpoint([FromQuery] string city)
    {
        try
        {
            CaseStatisticsZYH statistics = new CaseStatisticsZYH();
            if (city == "全部") // 如果city为“默认”，则返回所有城市的时间统计
            {
                statistics.getYearMonthStatistics();
                string jsonBackData = JsonSerializer.Serialize(statistics.numYearMonth);
                return Ok(jsonBackData);
            }
            else // 否则就统计指定城市的
            {
                statistics.getCityYearMonthStatistics(city);
                string jsonBackData = JsonSerializer.Serialize(statistics.numCityYearMonth);
                return Ok(jsonBackData);
            }
        }
        catch (Exception ex)
        {
            // 处理异常
            Console.WriteLine("cityDateStatisticsControllerZYH类的HandleEndpoint函数发生异常：" + ex.Message);
            return StatusCode(500); // 返回服务器错误状态码
        }
    }
}


[ApiController]
[Route("api/crimeDataStatistics/statusCityDateStatistics")]
public class statusCityDateStatisticsControllerZYH : ControllerBase
{
    [HttpGet]
    public IActionResult HandleEndpoint([FromQuery] string city, [FromQuery] string year, [FromQuery] string month)
    {
        CaseStatisticsZYH statistics = new CaseStatisticsZYH();
        statistics.getStatusCityDateStatistics(city, year, month);

        Dictionary<string, int> backData = new Dictionary<string, int>();
        backData.Add("立案", statistics.numFiling);
        backData.Add("调查", statistics.numInvestigating);
        backData.Add("结案", statistics.numClose);
        string jsonBackData = JsonSerializer.Serialize(backData);
        return Ok(jsonBackData);
    }
}


[ApiController]
[Route("api/crimeDataStatistics/cityTypeStatistics")]
public class cityTypeStatisticsControllerZYH : ControllerBase
{
    [HttpGet]
    public IActionResult HandleEndpoint()
    {
        CaseStatisticsZYH statistics = new CaseStatisticsZYH();
        statistics.getCityTypeStatistics();
        return Ok(statistics.cityType);
    }
}


[ApiController]
[Route("api/crimeDataStatistics/typeCityDateStatistics")]
public class typeCityDateStatisticsControllerZYH : ControllerBase
{
    [HttpGet]
    public IActionResult HandleEndpoint([FromQuery] string city, [FromQuery] string year, [FromQuery] string month)
    {
        CaseStatisticsZYH statistics = new CaseStatisticsZYH();
        statistics.getTypeCityDateStatistics(city, year, month);

        Dictionary<string, int> backData = new Dictionary<string, int>();
        backData.Add("强奸", statistics.numRape);
        backData.Add("抢劫", statistics.numRobbery);
        backData.Add("故意伤害", statistics.numInjury);
        backData.Add("盗窃", statistics.numTheft);
        backData.Add("诈骗", statistics.numFraud);
        backData.Add("谋杀", statistics.numMurder);

        string jsonBackData = JsonSerializer.Serialize(backData);
        return Ok(jsonBackData);
    }
}