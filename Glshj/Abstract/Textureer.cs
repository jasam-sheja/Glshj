using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Glshj.Abstract
{
    public abstract class Textureer : Destroyable
    {
        protected int buff = -1;

        public abstract Vector2[] getData();
        public virtual int getTexture() { return 0; }
        public virtual int Buffer { get { return buff; } }
        public int genBuffer()
        {
            Vector2[] data = getData();
            if(buff<1)
                buff = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, buff);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(data.Length * Vector2.SizeInBytes), data, BufferUsageHint.StaticDraw);

            int size;
            GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);
            if (data.Length * Vector2.SizeInBytes != size)
                throw new ApplicationException("Texture data not uploaded correctly");

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            return buff;
        }

        public void onDestroy()
        {
            if (buff > 0)
                GL.DeleteBuffer(buff);
            buff = 0;
            int tbuff = getTexture();
            if (tbuff > 0)
                GL.DeleteTexture(tbuff);
        }
    }
}
