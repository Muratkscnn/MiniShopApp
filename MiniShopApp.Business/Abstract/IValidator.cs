using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniShopApp.Business.Abstract
{
    public interface IValidator<T>
    {
        public string Error { get; set; }
        bool Validation(T entity);

    }
}
