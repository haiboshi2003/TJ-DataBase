using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;


[ApiController]
[Route("api/keyIndividualsStatistics/repeatOffenderInfoStatistics")]
public class repeatOffenderInfoStatisticsControllerZYH : ControllerBase
{
    [HttpGet]
    public IActionResult HandleEndpoint()
    {
        keyIndividualsZYH ki = new keyIndividualsZYH();
        ki.getRepeatOffenderInfoStatistics();
        return Ok(ki.repeatOffendersInfo);
    }
}

[ApiController]
[Route("api/keyIndividualsStatistics/repeatOffenderFilterStatistics")]
public class repeatOffenderFilterStatisticsControllerZYH : ControllerBase
{
    [HttpGet]
    public IActionResult HandleEndpoint([FromQuery] string? ID, [FromQuery] string? name, [FromQuery] string sex, [FromQuery] string? address)
    {
        keyIndividualsZYH ki = new keyIndividualsZYH();
        ki.getRepeatOffenderFilterStatistics(ID, name, sex, address);
        return Ok(ki.repeatOffendersInfo);
    }
}


[ApiController]
[Route("api/keyIndividualsStatistics/repeatOffenderCaseStatistics")]
public class repeatOffenderCaseStatisticsControllerZYH : ControllerBase
{
    [HttpGet]
    public IActionResult HandleEndpoint([FromQuery] string id)
    {
        keyIndividualsZYH ki = new keyIndividualsZYH();
        ki.getRepeatOffenderCaseStatistics(id);
        return Ok(ki.repeatOffendersCase);
    }
}