using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using Glshj.Abstract;
using Glshj.Curve;
using Glshj.Regural2D;
using Glshj.Complex2D;
using Glshj.Combine;
using Glshj.Util;

namespace TestGljsh
{
    public class ClothTest : GameWindow
    {
        Vector3 pos = Vector3.Zero;
        Vector3 rot = Vector3.Zero;
        Vector4 lightpos = new Vector4(0, 0, 10, 1);
        Vector4 matspec = new Vector4(0.1f, 0.1f, 0.1f, 1);
        Vector4 matemission = new Vector4(0.3f, 0.2f, 0.2f, 0.0f);
        float matshin = 3;

        #region shapes
        List<Destroyable> destroy = new List<Destroyable>();
        private void addToDestroy(Shape shape)
        {
            destroy.Add(shape.getVertexer());
            for (int i = 0; i < shape.getControlsCount(); i++)
            {
                Controller controller = shape.getController(i);
                destroy.Add(controller);
                if (controller.normalizer != null)
                    destroy.Add(controller.normalizer);
                if (controller.elementer != null)
                    destroy.Add(controller.elementer);
                if (controller.textrureer != null)
                    destroy.Add(controller.textrureer);
                if (controller.colorer != null)
                    destroy.Add(controller.colorer);
            }
        }

        Shape cloth_wire;
        Shape cloth;
        Cloth cloth_param;

        #endregion

        public ClothTest() : base(800, 600) { }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            KeyDown += Keyboard_KeyDown;
            MouseMove += Mouse_Move;
            MouseDown += Mouse_ButtonDown;
            Closing += new EventHandler<System.ComponentModel.CancelEventArgs>(Test1_Closing);

            cloth_param = new Cloth();
            cloth_wire = new Shape(cloth_param.getwireframeVertexer(), new Controller(null, cloth_param.getWireFrameElementer(), new RandColorColorer(Color.Black, Color.White, cloth_param.getwireframeVertexer()), null), PrimitiveType.Lines);
            addToDestroy(cloth_wire);

            cloth = new Shape(cloth_param, new Controller(cloth_param.getframeNormalizer(), cloth_param.getFrameElementer(), new OneColorColorer(Color.Blue, cloth_param), null), PrimitiveType.Triangles);
            addToDestroy(cloth);

            GL.ClearColor(System.Drawing.Color.MidnightBlue);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);
            GL.Enable(EnableCap.ColorMaterial);
            GL.ColorMaterial(MaterialFace.FrontAndBack, ColorMaterialParameter.AmbientAndDiffuse);
            GL.CullFace(CullFaceMode.Front);
            GL.ShadeModel(ShadingModel.Smooth);
        }

        #region control
        void Test1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            foreach (Destroyable des in destroy)
                des.onDestroy();
        }

        void Mouse_ButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Left)
            {

            }
        }

        void Mouse_Move(object sender, MouseMoveEventArgs e)
        {
            if (e.Mouse[MouseButton.Left])
            {

            }
        }
        void Keyboard_KeyDown(object sender, KeyboardKeyEventArgs e)
        {
            int extra = 1;
            if (e.Keyboard[Key.ShiftLeft] || e.Keyboard[Key.ShiftRight])
                extra = 3;

            if (e.Key == Key.Escape)
            {
                this.Exit();
            }
            else if (e.Key == Key.Up)
                pos.Y += 1 * extra;
            else if (e.Key == Key.Down)
                pos.Y -= 1 * extra;
            else if (e.Key == Key.Right)
                pos.X += 1 * extra;
            else if (e.Key == Key.Left)
                pos.X -= 1 * extra;
            else if (e.Key == Key.Keypad1)
                pos.Z += 1 * extra;
            else if (e.Key == Key.Keypad0)
                pos.Z -= 1 * extra;
            else if (e.Key == Key.Keypad4)
                rot.X += 5 * extra;
            else if (e.Key == Key.Keypad5)
                rot.Y += 5 * extra;
            else if (e.Key == Key.Keypad6)
                rot.Z += 5 * extra;
            else if (e.Key == Key.Keypad7)
                rot.X -= 5 * extra;
            else if (e.Key == Key.Keypad8)
                rot.Y -= 5 * extra;
            else if (e.Key == Key.Keypad9)
                rot.Z -= 5 * extra;
            else if (e.Key == Key.W)
                cloth_param.move(Vector3.UnitZ / 10f * extra);
            else if (e.Key == Key.S)
                cloth_param.move(-Vector3.UnitZ / 10f * extra);
            else if (e.Key == Key.A)
                cloth_param.move(Vector3.UnitX / 10f * extra);
            else if (e.Key == Key.D)
                cloth_param.move(-Vector3.UnitX / 10f * extra);
            else if (e.Key == Key.T)
            {
                //cloth_param.onUpdateFrame(0.016f/5);
                //cloth_wire.reBufferVertex();
            }
            //System.Console.WriteLine(e.Key.ToString());
        }
        #endregion
        
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Width, Height);

            float aspect_ratio = Width / (float)Height;
            Matrix4 perpective = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver3, aspect_ratio, 1, 1000);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref perpective);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            this.Title = " FPS: " + string.Format("{0:F}", 1.0 / e.Time);
            cloth_param.onUpdateFrame((float)e.Time);
            //cloth_wire.reBufferVertex();
            cloth.reBuffer();

        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {

            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 lookat = Matrix4.LookAt(0, 0, 35, 0, 0, 0, 0, 1, 0);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref lookat);

            GL.Translate(pos);
            GL.Rotate(rot.X, Vector3.UnitX);
            GL.Rotate(rot.Y, Vector3.UnitY);
            GL.Rotate(rot.Z, Vector3.UnitZ);

            GL.Light(LightName.Light0, LightParameter.Position, lightpos);
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Specular, matspec);
            GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Shininess, matshin);
            GL.PushMatrix();
            GL.Begin(PrimitiveType.Lines);
            GL.Color4(255, 0, 0, 255);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(1, 0, 0);
            GL.Color4(0, 255, 0, 255);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 1, 0);
            GL.Color4(0, 0, 255, 255);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 0, 1);
            GL.End();
            GL.Translate(-lightpos.Xyz);
            GL.Begin(PrimitiveType.Lines);
            GL.Color4(255, 0, 0, 255);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(1, 0, 0);
            GL.Color4(0, 255, 0, 255);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 1, 0);
            GL.Color4(0, 0, 255, 255);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 0, 1);
            GL.End();
            GL.PopMatrix();

            GL.Translate(-cloth_param.Width / 2.0, -cloth_param.Height / 2.0, 0);
            //cloth_wire.Draw();
            cloth.Draw();

            this.SwapBuffers();
        }
    }

    public class Cloth:Vertexer,CountParam
    {
        Mass[,] mass = new Mass[30, 30];
        List<Mass> mass_list = new List<Mass>();
        List<Spring> spring = new List<Spring>();
        float width,height;
        static Vector3 gravity = -9.8f * Vector3.UnitY;
        static Vector3 airforce = 0.01f * Vector3.UnitZ;
        static float spring_constant = 5f;
        public Cloth(float width=10f, float height=10f, float weight=1f)
        {
            this.width = width;
            this.height = height;
            float mass_weight = weight / mass.GetLength(0) / mass.GetLength(1);
            float mass_width = width / mass.GetLength(1);
            float mass_height = width / mass.GetLength(0);
            for (int i = 0; i < mass.GetLength(0); i++)
                for (int j = 0; j < mass.GetLength(1); j++)
                {
                    mass[i, j] = new Mass(mass_weight, new Vector3(j * mass_width, i * mass_height, 0));
                    mass_list.Add(mass[i, j]);
                }
            mass[mass.GetLength(0) - 1, 0].pinned = true;
            mass[mass.GetLength(0) - 1, mass.GetLength(1) - 1].pinned = true;
            //structural springs
            for (int i = 0; i < mass.GetLength(0); i++)
                for (int j = 0; j < mass.GetLength(1) - 1; j++)
                    spring.Add(new Spring(mass[i, j], mass[i, j + 1], spring_constant, (mass[i, j].location - mass[i, j + 1].location).Length));
            for (int i = 0; i < mass.GetLength(0) - 1; i++)
                for (int j = 0; j < mass.GetLength(1); j++)
                    spring.Add(new Spring(mass[i, j], mass[i + 1, j], spring_constant, (mass[i, j].location - mass[i + 1, j].location).Length));
            //shear springs
            for (int i = 0; i < mass.GetLength(0) - 1; i++)
                for (int j = 0; j < mass.GetLength(1) - 1; j++)
                    spring.Add(new Spring(mass[i, j], mass[i + 1, j + 1], spring_constant, (mass[i, j].location - mass[i + 1, j + 1].location).Length));
            for (int i = 0; i < mass.GetLength(0) - 1; i++)
                for (int j = 1; j < mass.GetLength(1) ; j++)
                    spring.Add(new Spring(mass[i, j], mass[i + 1, j - 1], spring_constant, (mass[i, j].location - mass[i + 1, j - 1].location).Length));
            //bend springs
            for (int i = 0; i < mass.GetLength(0); i++)
                for (int j = 0; j < mass.GetLength(1); j++)
                {
                    if (i + 2 < mass.GetLength(0))
                        spring.Add(new Spring(mass[i, j], mass[i + 2, j], spring_constant, (mass[i, j].location - mass[i + 2, j].location).Length));
                    if (j + 2 < mass.GetLength(1))
                        spring.Add(new Spring(mass[i, j], mass[i, j + 2], spring_constant, (mass[i, j].location - mass[i, j + 2].location).Length));
                }
            Console.WriteLine(spring.Count);
        }

        public int rows { get { return mass.GetLength(0); } }
        public int cols { get { return mass.GetLength(1); } }
        public float Width { get { return width; } }
        public float Height { get { return height; } }

        int print_count_modulo = 0;
        public void onUpdateFrame(float dt)
        {
            float step = 0.001f;
            int count = (int)Math.Ceiling(dt / step);
            step = dt / count;
            //if (++print_count_modulo % 30 == 0)
               // Console.WriteLine("count " + count + ", step " + step);
            float b = 0.01f;
            for (int i_step = 0; i_step < count; i_step++)
            {
                //foreach (Mass ma in mass)
                //    ma.addForce(gravity * ma.m - b * ma.velocity);
                mass_list.AsParallel().ForAll(ma => ma.addForce(gravity * ma.m - b * ma.velocity));
                //foreach (Mass ma in mass)
                //    ma.simulate(step);
                //foreach (Spring sp in spring)
                //    sp.solve();
                spring.AsParallel().ForAll(sp => sp.solve());
                
                //foreach (Mass ma in mass)
                //    ma.simulate(step);
                mass_list.AsParallel().ForAll(ma => ma.simulate(step));
                dt -= step;
            }
        }

        public override Vector4[] getData()
        {
            Vector4[] data = new Vector4[Count];
            int idx=0;
            for (int i = 0; i < mass.GetLength(0); i++)
                for (int j = 0; j < mass.GetLength(1); j++, idx++)
                    data[idx] = new Vector4(mass[i, j].location, 1);                    
            return data;
        }

        public override int Count
        {
            get { return mass.GetLength(0) * mass.GetLength(1); }
        }

        public Normalizer getframeNormalizer()
        {
            if (normalizer == null)
                normalizer = new frameNormalizer(this);
            return normalizer;
        }
        private class frameNormalizer : Normalizer
        {
            Cloth father;
            public frameNormalizer(Cloth father)
            {
                this.father = father;
            }

            public override Vector3[] getData()
            {
                Vector3[] data = new Vector3[father.Count];
                Fill(data, Vector3.Zero);
                for (int i = 0; i < father.rows - 1; i++)
                    for (int j = 0; j < father.cols - 1; j++)
                    {
                        Vector3 norm = Vector3.Cross((father.mass[i, j].location - father.mass[i, j + 1].location),
                                                    (father.mass[i, j].location - father.mass[i + 1, j + 1].location));
                        data[i * father.cols + j] += norm;
                        data[i * father.cols + j + 1] += norm;
                        data[(i + 1) * father.cols + j + 1] += norm;

                        norm = Vector3.Cross((father.mass[i, j].location - father.mass[i + 1, j + 1].location),
                                             (father.mass[i, j].location - father.mass[i + 1, j].location));
                        data[i * father.cols + j] += norm;
                        data[(i + 1) * father.cols + j + 1] += norm;
                        data[(i + 1) * father.cols + j] += norm;
                    }
                data.AsParallel().ForAll(norm => norm.Normalize());
                return data;
            }

            private static void Fill(Vector3[] outarray, Vector3 value)
            {
                for (int i = 0; i < outarray.Length; i++)
                    outarray[i] = value;
            }
        }
        frameNormalizer normalizer;

        public Elementer getFrameElementer()
        {
            if (elementer == null)
                elementer = new frameElementer(this);
            return elementer;
        }
        private class frameElementer : Elementer
        {
            Cloth father;
            short[] data;
            public frameElementer(Cloth father)
            {
                this.father = father;
            }

            public override short[] getData()
            {
                if (data != null)
                    return data;
                data = new short[father.rows * father.cols * 6];
                int idx = 0;
                for (int i = 0; i < father.rows-1; i++)
                    for (int j = 0; j < father.cols-1; j++)
                    {
                        data[idx++] = (short)(i * father.cols + j);
                        data[idx++] = (short)((i + 1) * father.cols + j + 1);
                        data[idx++] = (short)(i * father.cols + j + 1);

                        data[idx++] = (short)(i * father.cols + j);
                        data[idx++] = (short)((i + 1) * father.cols + j);
                        data[idx++] = (short)((i + 1) * father.cols + j + 1);
                    }
                return data;
            }
        }
        frameElementer elementer;

        public Vertexer getwireframeVertexer()
        {
            if (wireVertexer == null)
                wireVertexer = new wireframeVertexer(this);
            return wireVertexer;
        }
        private class wireframeVertexer : Vertexer
        {
            Cloth father;
            public wireframeVertexer(Cloth father)
            {
                this.father = father;
            }

            public override Vector4[] getData()
            {
                Vector4[] data = new Vector4[Count];
                int idx = 0;
                for (int i = 0; i < father.mass.GetLength(0); i++)
                    for (int j = 0; j < father.mass.GetLength(1); j++)
                        data[idx++] = new Vector4(father.mass[i, j].location, 1);
                return data;
            }

            public override int Count
            {
                get { return father.mass.GetLength(0) * father.mass.GetLength(1); }
            }
        }
        wireframeVertexer wireVertexer;
       

        public Elementer getWireFrameElementer()
        {
            if (wireElementer == null)
                wireElementer = new wireframeElementer(this);
            return wireElementer;
        }
        private class wireframeElementer : Elementer
        {
            Cloth father;
            public wireframeElementer(Cloth father)
            {
                this.father = father;
            }

            public override short[] getData()
            {
                int len = father.mass.GetLength(1);
                short[] data = new short[father.spring.Count*2];
                int idx = 0;
                for (int i = 0; i < father.mass.GetLength(0); i++)
                    for (int j = 0; j < father.mass.GetLength(1) - 1; j++)
                    {
                        data[idx++] = (short)(i * len + j);
                        data[idx++] = (short)(i * len + j + 1);
                    }
                for (int i = 0; i < father.mass.GetLength(0) - 1; i++)
                    for (int j = 0; j < father.mass.GetLength(1); j++)
                    {
                        data[idx++] = (short)(i * len + j);
                        data[idx++] = (short)((i + 1) * len + j);
                    }
                return data;     
            }
        }
        wireframeElementer wireElementer;

        public void move(Vector3 mv)
        {
            mass[mass.GetLength(0) - 1, 0].location += mv;
            mass[mass.GetLength(0) - 1, mass.GetLength(1) - 1].location += mv;
        }
    }

    class Mass
    {
        public float m;
        public Vector3 location;
        Vector3 force;
        public Vector3 velocity;
        public bool pinned { set; get; }
        public Mass(float m, Vector3 location, bool pinned=false)
        {
            this.m = m;
            this.location = location;
            this.pinned = pinned;
        }
        public void addForce(Vector3 force){
            this.force += force;
        }
        private readonly object syncLock = new object();
        public void applyforce(Vector3 force)
        {
            lock (syncLock)
            {
                this.force += force;
            }
        }
        public void simulate(float dt)
        {
            if (!pinned)
            {
                velocity += (force / m) * dt;
                location += velocity * dt;
            }
            force = Vector3.Zero;
        }
    }

    class Spring
    {
        public Mass p1,p2;
        public float k;
        public float r;
        public float friction;
        public Spring(Mass p1, Mass p2, float k, float r,float friction=0.1f)
        {
            this.p1 = p1;
            this.p2 = p2;
            this.k = k;
            this.r = r;
            this.friction = friction;
        }

        public void solve(){
            Vector3 vec = p1.location - p2.location;
            float len = vec.Length;
            Vector3 force = (p2.velocity - p1.velocity) * friction;
            if (len > 1e-3)
            {
                force += vec * ((r - len) * k / len);
                p1.applyforce(force);
                p2.applyforce(-force);
            }
        }
    }
}
