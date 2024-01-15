using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwareTest.Model.Model
{
    public class ResultModel
    {
        public bool Status { get; set; } = true;
        public string Code { get; set; } = "0000";
        public string Message { get; set; } = "successfully";
        public List<ErrorModel> Errors { get; set; }
    }

    public class ResultModel<T> : ResultModel
    {
        public T Value { get; set; }
    }

    public class ErrorModel
    {
        public string Code { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }

}
