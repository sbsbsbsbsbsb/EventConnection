using System.Collections.Generic;
using System.Linq;
using Model.Utils;
using Newtonsoft.Json;

namespace Model.DTO
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ResponseModel<T> where T: class
    {
        public ResponseModel()
        {
            Errors = new List<ErrorModel>();
            Response = new List<T>();
        }

        [JsonProperty("hasErrors")]
        public bool HasErrors { get; set; }

        [JsonProperty("errors")]
        public List<ErrorModel> Errors { get; set; }

        [JsonProperty("response")]
        public List<T> Response { get; set; }

        public T FirstResponce
        {
            get { return Response.FirstOrDefault(); }
            set
            {
                Response.Clear();
                Response.Add(value);
            }
        }

        public string ErrorTrace => Errors.GetTrace();
    }
}
