using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces.Generic
{
    public  interface InterfaceGeneric<T> where T:class
    {
        void Post(T Object);

    }
}
