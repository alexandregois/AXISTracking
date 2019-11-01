using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace axistracking.Domain.Dto
{
    public class ServiceResult<TEntity>
    {
        public String MessageError { get; set; }
        public Boolean? IsValid { get; set; }
		public Int32? StatusCode { get; set; }
		public Token RefreshToken { get; set; }
        public TEntity Data { get; set; }
    }
}
