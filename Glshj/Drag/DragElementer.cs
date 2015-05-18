using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using Glshj.Abstract;

namespace Glshj.Drag
{
    public class DragElementer :Elementer
    {
        public Elementer source;
        public Elementer destinaion;
        public CountParam sourceCount;

        public DragElementer(Elementer source, Elementer destinaion, CountParam sourceCount)
        {
            this.source = source;
            this.destinaion = destinaion;
            this.sourceCount = sourceCount;
        }

        public override short[] getData()
        {
            short[] firstdata = source.getData();
            short[] seconddata = destinaion.getData();
            short[] data = new short[firstdata.Length + seconddata.Length];

            int i = 0, j = 0, idx = 0;
            //data[idx++] = firstdata[i++];
            //data[idx++] = (short)(seconddata[j++] + sourceCount.Count);
            
            for (; i < firstdata.Length && j < seconddata.Length; i++, j++)
            {
                data[idx++] = (short)(seconddata[j] + sourceCount.Count);
                data[idx++] = firstdata[i];
                
            }
            for (; i < firstdata.Length; i++)
                data[idx++] = firstdata[i];
            for (; j < seconddata.Length; j++)
                data[idx++] = (short)(seconddata[j] + sourceCount.Count);
            return data;
        }
    }
}
