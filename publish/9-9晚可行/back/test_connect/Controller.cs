using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;

public class MyRequestData
{
    public string InputText { get; set; }
}

[Route("api/query")]
[ApiController]
public class MyController : ControllerBase
{
    [HttpPost]
    public IActionResult HandleEndpoint(MyRequestData requestData)
    {
        string inputText = requestData.InputText; // 从请求的JSON数据中获取输入的字符串
        // 在这里处理接收到的字符串

        return Ok(inputText);
    }
}

