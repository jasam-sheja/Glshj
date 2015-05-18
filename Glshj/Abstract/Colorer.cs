using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Glshj.Abstract
{
    public abstract class Colorer : Destroyable
    {
        protected int buff = -1;

        public abstract Vector4[] getData();
        public virtual int Buffer { get { return buff; } }
        public int genBuffer()
        {
            Vector4[] data = getData();
            if (buff < 1)
                buff = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, buff);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(data.Length * Vector4.SizeInBytes), data, BufferUsageHint.StaticDraw);

            int size;
            GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);
            if (data.Length * Vector4.SizeInBytes != size)
                throw new ApplicationException("Colors data not uploaded correctly");

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            return buff;
        }

        public void onDestroy()
        {
            if (buff > 0)
                GL.DeleteBuffer(buff);
            buff = 0;
        }
    }
}
