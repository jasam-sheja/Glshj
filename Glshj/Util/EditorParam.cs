using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;

namespace Glshj.Util
{
    public class EditorParam
    {
        public Matrix4 matrix;

        public EditorParam()
        {
            this.matrix = Matrix4.Identity;
        }

        public EditorParam(Matrix4 matrix)
        {
            this.matrix = matrix;
        }

        public Matrix4 Matrix { get { return matrix; } }
    }
}
