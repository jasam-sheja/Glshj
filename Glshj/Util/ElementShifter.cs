using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Glshj.Abstract;

namespace Glshj.Util
{
    class ElementShifter:Elementer
    {
        private Elementer base_;
        public short shift;
        public ElementShifter(Elementer base_, short shift)
        {
            this.base_ = base_;
            this.shift = shift;
        }

        public override short[] getData()
        {
            return Shift(base_.getData(), shift);
        }

        public static short[] Shift(short[] data, short shift)
        {
            for (int i = 0; i < data.Length; i++)
                data[i] += shift;
            return data;
        }
    }
}
