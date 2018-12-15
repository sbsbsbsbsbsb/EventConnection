using System.Net.Http;

namespace Model.Runtime
{
    public class ServiceMethodModel
    {
        public string Name { get; set; }
        public string PathMask { get; set; }
        public HttpMethod Protocol { get; set; }
    }
}
