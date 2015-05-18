using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using Glshj.Abstract;
using Util;

namespace Glshj.Regural2D
{
    public class Regular2DTextureer :Textureer
    {
        public Regular2DParam param;
        private int texbuffer;
        public float shift;
        public Regular2DTextureer(Regular2DParam param, int texbuffer, float shift=0)
        {
            this.param = param;
            this.texbuffer = texbuffer;
            this.shift = shift;
        }

        public Regular2DTextureer(Regular2DParam param, string texfile, float shift = 0) :
            this(param, TexUtil.CreateTextureFromFile(texfile), shift) { }

        public override Vector2[] getData()
        {
            Vector2[] data = new Vector2[param.count];
            int i = 0;
            for (double a = param.start + shift; a < MathHelper.TwoPi + param.start + shift; a += MathHelper.TwoPi / param.count)
            {
                float cos = (float)Math.Cos(a), sin = (float)Math.Sin(a);
                float sin90 = (float)Math.Sin(a + MathHelper.PiOver2),
                    cos90 = (float)Math.Cos(a + MathHelper.PiOver2);
                data[i++] = new Vector2(0.5f + (cos90 + sin90) / 2, 0.5f - (cos + sin) / 2);
            }
            return data;
        }

        public override int getTexture()
        {
            return texbuffer;
        }
    }
}
