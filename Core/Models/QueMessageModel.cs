using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable disable
namespace Bazaro.Core.Models
{
    public class QueueMessageModel
    {
        private readonly DateTime _RegisterTime;
        public QueueMessageModel()
        {
            _RegisterTime = DateTime.Now;
        }

        /// <summary>
        /// expire after this time out value as minute
        /// </summary>
        public int TimeOut { get; set; }
        public string Method { get; set; }
        public object Data { get; set; }
        public bool IsValid
        {
            get
            {
                return (DateTime.Now - _RegisterTime).TotalMinutes <= TimeOut;
            }
        }
    }
}
