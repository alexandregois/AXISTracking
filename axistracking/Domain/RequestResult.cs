using axistracking.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace axistracking.Domain
{
    public class RequestResult<TEntity>
    {
        public ServiceResult<TEntity> Result { get; set; }
        public object Id { get; set; }
        public object Status { get; set; }
        public object IsCanceled { get; set; }
        public object IsCompleted { get; set; }
        public object CreateOptions { get; set; }
        public object IsFaulted { get; set; }
    }

}
