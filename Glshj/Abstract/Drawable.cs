using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Glshj.Abstract
{
    public interface Drawable
    {
        void Draw();
        void Draw(params PrimitiveType[] mode);
        void Draw(params short[] index);
        void Draw(short index, PrimitiveType mode);
        void Draw(short[] index, PrimitiveType[] mode);
        void Draw(short index, PrimitiveType mode, int count, int indeces);
        void Draw(short[] index, PrimitiveType[] mode, int[] count, int[] indeces);
    }
}
