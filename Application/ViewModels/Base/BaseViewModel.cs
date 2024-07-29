using System;

namespace Bazaro.Application.ViewModels.Base
{
    public class BaseViewModel<T>
    {
        public T Id { get; set; }
        //public DateTime Created { get; set; }
        //public DateTime Modified { get; set; }

        //public byte[]? RowVersion { get; set; }
    }
}
