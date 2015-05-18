using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using Glshj.Abstract;

namespace Glshj.Util
{
    public class OneNormalNormalizer:Normalizer
    {
        private Vector3 normal;
        private CountParam count;

        public OneNormalNormalizer(Vector3 normal, CountParam count, bool gen = false)
        {
            this.normal = normal;
            this.count = count;
            if (gen)
                genBuffer();
        }

        public OneNormalNormalizer(Vector3 normal, int count, bool gen = false) :
            this(normal, new CountPram_normal(count), gen)
        {
        }

        public override Vector3[] getData()
        {
            Vector3[] res = new Vector3[count.Count];
            Fill(res, normal);
            return res;
        }

        private static void Fill(Vector3[] outarray, Vector3 value)
        {
            for (int i = 0; i < outarray.Length; i++)
                outarray[i] = value;
        }
    }

    class CountPram_normal:CountParam
    {
        int count;
        public CountPram_normal(int count){
            this.count = count;
        }

        public int  Count
        {
	        get { return count; }
        }
    }
}
