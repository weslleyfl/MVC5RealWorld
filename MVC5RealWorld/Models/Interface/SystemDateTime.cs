using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC5RealWorld.Models.Interface
{
    public class SystemDateTime : IDateTime
    {
        public DateTime Now
        {
            get => DateTime.Now;
        }

        //public DateTime MyProperty
        //{
        //    get { return DateTime.Now; }
        //}

        //public DateTime Now
        //{
        //    get => Now;
        //    set => this.Now = value; // ?? DateTime.MinValue;                

        //}


    }
}
