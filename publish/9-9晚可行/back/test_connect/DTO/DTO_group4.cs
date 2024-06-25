namespace web.DTO_group4
{
    // register
    public struct RequestData
    {
        public string ID_number { get; set; }
        public string birthday { get; set; }
        public string email { get; set; }
        public string gender { get; set; }
        public string nation { get; set; }
        public string phone_number { get; set; }
        public string police_name { get; set; }
        public string police_number { get; set; }
        public string position { get; set; }
        public string status { get; set; }
    }
    // register:check if content is legal
    public struct LegalJudgeMessage
    {
        public string queryType { get; set; }
        public string queryContent { get; set; }
    }
    // FamilybgCheck
    public struct Info
    {
        public string name { get; set; }
        public string gender { get; set; }
        public string ID { get; set; }
        public List<string> crimeType { get; set; }
        public Info()
        {
            name = "";
            gender = "";
            ID = "";
            crimeType = new List<string>();
        }
    }
    public struct result
    {
        public List<Info> people { get; set; } // 每个人
        public List<string> relationship { get; set; } // 每个人和中心人的关系
        public result()
        {
            people = new List<Info>();
            relationship = new List<string>();
        }
    }
    public struct MyRequestData
    {
        public string InputText { get; set; }
    }
    // Investigation
    public class CaseRequestData
    {
        public string inputTextCase { get; set; }
    }

    public class PoliceRequestData
    {
        public string inputTextPolice { get; set; }
    }

    public class ModifyRequestData
    {
        public string inputCase { get; set; }
    }
}
