using System.Collections.Generic;

namespace NTS.WEB.ResultView
{
    public class LoginResult
    {
        public bool Success { get; set; }
        public string Msg { get; set; }
        public List<string> Menus { get; set; }
        public string RedirectUrl { get; set; }
    }
}
