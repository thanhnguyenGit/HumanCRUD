using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BSS;
using ApiStudyFinal.Models;
using Newtonsoft.Json.Linq;
namespace ApiStudyFinal.Controllers
{
    public class HumanController : ControllerBase
    {
        [HttpGet]
        public Result GetOne(string strGuid)
        {
            Console.WriteLine("REQUEST TRIGGER!");
            string msg = DoValidGuid(strGuid, out Human outHuman);
            if (msg.Length > 0)
            {
                Log.WriteErrorLog(msg);
                return msg.ToResultError();
            }
            return outHuman.ToResultOk();
        }
        [HttpGet]
        public Result GetAll()
        {
            string msg = "";
            msg = Human.GetAll(out List<Human> ltHuman);
            if (msg.Length > 0)
            {
                Log.WriteErrorLog(msg);
                return msg.ToResultError();
            }
            return ltHuman.ToResultOk();
        }
        [HttpPost]
        public Result InsertOrUpdate([FromBody] JObject body)
        {
            Console.WriteLine("TRIGGER UPDATE");
            string msg = "";
            if (body == null) { Console.WriteLine($"{body}"); return "Invalid JSON body.".ToResultError(); }
            msg = DoValidate(body, out Human outHuman);
            if (msg.Length > 0)
            {
                Log.WriteErrorLog(msg);
                return msg.ToResultError();
            }
            msg = DoInsertOrUpdate(outHuman);
            if (msg.Length > 0)
            {
                Log.WriteErrorLog(msg);
                return msg.ToResultError();
            }
            return outHuman.ToResultOk();
        }
        [HttpGet]
        public Result TestConnection()
        {
            string msg = "";
            msg = Human.TestConnection();
            if (msg.Length > 0)
            {
                Log.WriteErrorLog(msg);
                return msg.ToResultError();
            }
            return msg.ToResultError();

        }
        [HttpPost]
        public Result Delete([FromBody] JObject body)
        {
            string msg = DoDelete(body);
            if (msg.Length > 0)
            {
                Log.WriteErrorLog(msg);
                return msg.ToResultError();
            }
            return "".ToResultOk();
        }

        public string DoValidGuid(string strGuid, out Human outHuman)
        {
            outHuman = null;

            if (string.IsNullOrEmpty(strGuid)) return "Guid khong ton tai".ToMessageForUser();

            if (!Guid.TryParse(strGuid, out Guid ObjectGuid)) return "Guid khong hop le".ToMessageForUser();

            string msg = Human.GetOne(ObjectGuid, out outHuman);

            if (msg.Length > 0) { return msg; }

            return "";
        }
        public string DoValidate([FromBody] JObject body, out Human outHuman)
        {
            outHuman = null;
            var temp = body["Human"];
            Console.WriteLine($"body: {body.ToString()}");
            string msg = BSS.Convertor.JsonToObject<Human>(temp, out outHuman);
            Console.WriteLine($"outHuman: {outHuman.ToString()}");
            if (msg.Length > 0) return msg;
            if (outHuman == null) return "Vui long kiem tra lai du lieu dau vao".ToMessageForUser();
            if (string.IsNullOrEmpty(outHuman.FullName)) return "Ten nguoi dung khong duoc de trong".ToMessageForUser();
            else if (outHuman.FullName.Length > 50) return "Ten nguoi khong duoc vuot qua 50 ky tu".ToMessageForUser();

            /*Log.WriteHistoryLog(outHuman.HumanID == 0 ? "Them moi nguoi dung" : "Cap nhat nguoi dung", Guid.Parse(outHuman.ObjectGuid));*/

            return msg;
        }
        public string DoInsertOrUpdate(Human human)
        {
            string msg = "";

            msg = human.InsertOrUpdate(new DBM());
            if (msg.Length > 0) return msg;
            return "";
        }
        public string DoDelete([FromBody] JObject body)
        {
            string msg = BSS.Convertor.JsonToObject<string>(body["strGuid"], out string ObjectGuid);
            if (msg.Length > 0) return msg;
            msg = Human.Delete(Guid.Parse(ObjectGuid));
            if (msg.Length > 0) return msg;
            Log.WriteHistoryLog("Xoa nguoi dung", Guid.Parse(ObjectGuid));

            return msg;
        }
    }
}
