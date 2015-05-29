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
        Vector4 lightpos = new Vector4(1, 1, -10, 0);
        Vector4 litspec = new Vector4(1, 1, 1, 1);
        Vector4 matemission = new Vector4(0.4f, 0.4f, 0.4f, 1.0f);
        float litshin = 90;
        bool update = false;
        bool wire = false;
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
        Shape nail;

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

            //cloth = new Shape(cloth_param, new Controller(cloth_param.getframeNormalizer(), cloth_param.getFrameElementer(), new OneColorColorer(Color.Blue, cloth_param), null), PrimitiveType.Triangles);
            cloth = new Shape(cloth_param, new Controller(cloth_param.getframeNormalizer(), cloth_param.getFrameElementer(), null, cloth_param.getFrameTextreer()), PrimitiveType.Triangles);
            addToDestroy(cloth);

            Regular2DParam nail_param = new Regular2DParam(cloth_param.Width/cloth_param.cols, 3);
            nail = new Shape(new Regular2DVertexer(nail_param), new Controller(new Regular2DNormalizer(nail_param), new Regular2DElementer(nail_param), new OneColorColorer(Color.Silver, nail_param), null), PrimitiveType.Polygon);
            addToDestroy(nail);

            GL.ClearColor(System.Drawing.Color.MidnightBlue);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);
            GL.Enable(EnableCap.ColorMaterial);
            GL.ColorMaterial(MaterialFace.Front, ColorMaterialParameter.Ambient);
            GL.CullFace(CullFaceMode.Front);
            GL.ShadeModel(ShadingModel.Smooth);
            GL.Enable(EnableCap.RescaleNormal);
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
                if(e.Keyboard[Key.L])
                    lightpos.Y += 1*extra;
                else
                    pos.Y += 1 * extra;
            else if (e.Key == Key.Down)
                if (e.Keyboard[Key.L])
                    lightpos.Y -= 1 * extra;
                else
                    pos.Y -= 1 * extra;
            else if (e.Key == Key.Right)
                if (e.Keyboard[Key.L])
                    lightpos.X += 1 * extra;
                else
                    pos.X += 1 * extra;
            else if (e.Key == Key.Left)
                if (e.Keyboard[Key.L])
                    lightpos.X -= 1 * extra;
                else
                    pos.X -= 1 * extra;
            else if (e.Key == Key.Keypad1)
                if (e.Keyboard[Key.L])
                    lightpos.Z += 1 * extra;
                else
                    pos.Z += 1 * extra;
            else if (e.Key == Key.Keypad0)
                if (e.Keyboard[Key.L])
                    lightpos.Z -= 1 * extra;
                else
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
                cloth_param.move(Vector3.UnitZ / 100f * extra);
            else if (e.Key == Key.S)
                cloth_param.move(-Vector3.UnitZ / 100f * extra);
            else if (e.Key == Key.A)
                cloth_param.move(Vector3.UnitX / 100f * extra);
            else if (e.Key == Key.D)
                cloth_param.move(-Vector3.UnitX / 100f * extra);
            else if (e.Key == Key.T)
            {
                cloth_param.onUpdateFrame(0.0015f);
                cloth_wire.reBufferVertex();
            }
            else if (e.Key == Key.P)
            {
                update = !update;
            }
            else if (e.Key == Key.E)
            {
                Console.WriteLine("kinect energy = " + cloth_param.VelocityEnergy()); 
            }
            else if (e.Key == Key.Number1)
            {
                wire = !wire;
            }
            else if (e.Key == Key.H)
            {
                Console.WriteLine("move object: W S A D");
                Console.WriteLine("move prespective: up down left right keypad0 keypad1");
                Console.WriteLine("move light pos: L + { up down left right keypad0 keypad1 }");
                Console.WriteLine("rotate prespective: keypad { 7 4 8 5 9 6 }");
                Console.WriteLine("play/pause: P");
                Console.WriteLine("step in time: T");
                Console.WriteLine("change draw mode: 1");
                Console.WriteLine("show kinect energy: E");
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
            if (update)
            {
                cloth_param.onUpdateFrame((float)e.Time);
                if(wire)
                    cloth_wire.reBufferVertex();
                else
                    cloth.reBuffer();
            }

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
            GL.Scale(10, 10, 10);

            GL.Light(LightName.Light0, LightParameter.Position, lightpos);
            GL.Light(LightName.Light0, LightParameter.Specular, new float[] { 1, 1, 1, 1.0f });
            GL.Light(LightName.Light0, LightParameter.Ambient, new float[]{ 1, 1, 1, 1.0f });
            GL.Light(LightName.Light0, LightParameter.Diffuse, new float[] { 1, 1, 1, 1.0f });
            //GL.Light(LightName.Light0, LightParameter.SpotDirection, new float[] { 0, 0, 1, 1.0f });
            GL.LightModel(LightModelParameter.LightModelAmbient, new float[] { 1, 1, 1, 1.0f });
            //GL.LightModel(LightModelParameter.LightModelTwoSide, 1);
            GL.Material(MaterialFace.Front, MaterialParameter.Diffuse, new float[] { 0.9f, 0.9f, 0.9f, 1.0f });
            GL.Material(MaterialFace.Front, MaterialParameter.Specular, new float[] { 1, 1, 1, 1.0f });
            GL.Material(MaterialFace.Front, MaterialParameter.Emission, matemission);
            GL.Material(MaterialFace.Front, MaterialParameter.Shininess, 90);
            GL.PushMatrix();
            GL.Begin(PrimitiveType.Lines);
            {
                GL.Color4(255, 0, 0, 255);
                GL.Vertex3(0, 0, 0);
                GL.Vertex3(1, 0, 0);
                GL.Color4(0, 255, 0, 255);
                GL.Vertex3(0, 0, 0);
                GL.Vertex3(0, 1, 0);
                GL.Color4(0, 0, 255, 255);
                GL.Vertex3(0, 0, 0);
                GL.Vertex3(0, 0, 1);
            }
            GL.End();
            GL.Translate(-lightpos.Xyz);
            GL.Begin(PrimitiveType.Lines);
            {
                GL.Color4(255, 0, 0, 255);
                GL.Vertex3(0, 0, 0);
                GL.Vertex3(1, 0, 0);
                GL.Color4(0, 255, 0, 255);
                GL.Vertex3(0, 0, 0);
                GL.Vertex3(0, 1, 0);
                GL.Color4(0, 0, 255, 255);
                GL.Vertex3(0, 0, 0);
                GL.Vertex3(0, 0, 1);
            }
            GL.End();
            GL.PopMatrix();

            GL.Translate(-cloth_param.Width / 2.0, -cloth_param.Height / 2.0, 0);
            if (wire)
                cloth_wire.Draw();
            else
                cloth.Draw();
            //GL.PushMatrix();
            //{
            //    GL.Translate(cloth_param.getLoc(cloth_param.rows - 1, 0));
            //    nail.Draw();
            //}
            //GL.PopMatrix();
            //GL.PushMatrix();
            //{
            //    GL.Translate(cloth_param.getLoc(cloth_param.rows - 1, cloth_param.cols - 1));
            //    nail.Draw();
            //}
            //GL.PopMatrix();

            //GL.Translate(Vector3.UnitZ);
            //GL.Begin(PrimitiveType.Quads);
            //{
            //    GL.Color3(Color.Blue);
            //    GL.Normal3(Vector3.UnitZ);
            //    GL.Vertex3(0, 0, 1);
            //    GL.Normal3(Vector3.UnitZ);
            //    GL.Vertex3(0, 1, 1);
            //    GL.Normal3(Vector3.UnitZ);
            //    GL.Vertex3(1, 1, 1);
            //    GL.Normal3(Vector3.UnitZ);
            //    GL.Vertex3(1, 0, 1);
            //}
            //GL.End();
            //GL.Begin(PrimitiveType.Lines);
            //{
            //    GL.Vertex3(0, 0, 0);
            //    GL.Vertex3(0, 0, 1);
            //    Vector3[] data = cloth_param.getframeNormalizer().getData();
            //    int idx = 0;
            //    for (int i = 0; i < cloth_param.rows; i++)
            //    {
            //        for (int j = 0; j < cloth_param.cols; j++)
            //        {
            //            GL.Vertex3(cloth_param.getLoc(i, j));
            //            GL.Vertex3(cloth_param.getLoc(i, j)+data[idx++]/20);
            //        }
            //    }
            //}
            //GL.End();

            this.SwapBuffers();
        }
    }

    public class Cloth:Vertexer,CountParam
    {
        Mass[,] mass = new Mass[17, 17];
        List<Mass> mass_list = new List<Mass>();
        List<Spring> spring = new List<Spring>();
        float width,height;
        static Vector3 gravity = -9.8f * Vector3.UnitY; 
        //static Vector3 airforce = 0.01f * Vector3.UnitZ;
        static float spring_constant_structural = 125;
        static float spring_constant_shear = 51;
        static float spring_constant_bend = 5;
        static float rho = 1.29f; //kilogram/qube(meter) kg/m3
        static float drag_cofficient = 0.95f;
        public Cloth(float width=1f, float height=2f, float weight=0.504f)
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
                    spring.Add(new Spring(mass[i, j], mass[i, j + 1], spring_constant_structural, (mass[i, j].location - mass[i, j + 1].location).Length));
            for (int i = 0; i < mass.GetLength(0) - 1; i++)
                for (int j = 0; j < mass.GetLength(1); j++)
                    spring.Add(new Spring(mass[i, j], mass[i + 1, j], spring_constant_structural, (mass[i, j].location - mass[i + 1, j].location).Length));
            //shear springs
            for (int i = 0; i < mass.GetLength(0) - 1; i++)
                for (int j = 0; j < mass.GetLength(1) - 1; j++)
                    spring.Add(new Spring(mass[i, j], mass[i + 1, j + 1], spring_constant_shear, (mass[i, j].location - mass[i + 1, j + 1].location).Length));
            for (int i = 0; i < mass.GetLength(0) - 1; i++)
                for (int j = 1; j < mass.GetLength(1); j++)
                    spring.Add(new Spring(mass[i, j], mass[i + 1, j - 1], spring_constant_shear, (mass[i, j].location - mass[i + 1, j - 1].location).Length));
            //bend springs
            //for (int i = 0; i < mass.GetLength(0); i++)
            //    for (int j = 0; j < mass.GetLength(1); j++)
            //    {
            //        if (i + 2 < mass.GetLength(0))
            //            spring.Add(new Spring(mass[i, j], mass[i + 2, j], spring_constant_bend, (mass[i, j].location - mass[i + 2, j].location).Length));
            //        if (j + 2 < mass.GetLength(1))
            //            spring.Add(new Spring(mass[i, j], mass[i, j + 2], spring_constant_bend, (mass[i, j].location - mass[i, j + 2].location).Length));
            //    }
            Console.WriteLine(spring.Count);
        }

        public int rows { get { return mass.GetLength(0); } }
        public int cols { get { return mass.GetLength(1); } }
        public float Width { get { return width; } }
        public float Height { get { return height; } }

        int print_count_modulo = 0;
        float step = 0.0015f;
        float dt = 0;

        public Vector3 getLoc(int i, int j)
        {
            return mass[i, j].location;
        }
        public void onUpdateFrame(float dti)
        {
            this.dt += dti;
            
            float b = 0.000f;
            
            int count = 0;
            while (dt > step)
            {
                float energy = VelocityEnergy();
                mass_list.AsParallel().ForAll(ma => ma.applyforce(gravity * ma.m - b * ma.velocity));
                spring.AsParallel().ForAll(sp => sp.solve());
                (getFrameElementer() as frameElementer).faces.AsParallel().ForAll(face => face.solveAir());

                mass_list.AsParallel().ForAll(ma => ma.simulate(step));

                dt -= step;
                count++;
                //float denergy = Math.Abs(VelocityEnergy() - energy);
                //if (!float.IsNaN(energy))
                //{
                //    if (denergy < 1e-7)
                //        step += 0.000011f;
                //    else
                //        step -= 0.00001f;
                //}
                //else
                //{
                //    Console.WriteLine("step " + step);
                //}
                //step = Math.Max(step, 0.001f);
            }
            if (print_count_modulo++ % 20 == 0)
                Console.WriteLine("count = " + count + ", step " + step);
            //energy = Math.Abs(VelocityEnergy() - energy);
            //if (energy >= 0)
            //    Console.WriteLine(" energy = " + energy);
            //else
            //    Console.WriteLine("f energy = " + energy);
        }

        private class Face
        {
            static float rho = Cloth.rho;
            static float drag_cofficient = Cloth.drag_cofficient;
            short[] i, j;
            Cloth father;
            public Face(Cloth father, short[] i, short[] j)
            {
                this.father = father;
                this.i = i;
                this.j = j;
            }

            public void solveAir()
            {
                
                float area = Vector3.Cross((father.mass[i[0], j[0]].location - father.mass[i[1], j[1]].location), (father.mass[i[0], j[0]].location - father.mass[i[2], j[2]].location)).Length / 2;
                Vector3 normal = Vector3.Cross((father.mass[i[0], j[0]].location - father.mass[i[1], j[1]].location), (father.mass[i[0], j[0]].location - father.mass[i[2], j[2]].location)).Normalized();
                Vector3 velocity = (father.mass[i[0], j[0]].velocity + father.mass[i[1], j[1]].velocity + father.mass[i[2], j[2]].velocity);
                Vector3 velocityn = Vector3.Dot(velocity, normal) * normal;
                Vector3 vnormal = velocityn.Normalized();
                if (!MathUtil.isZero(velocityn.Length))
                {
                    Vector3 fd = (-0.5f * rho * area * drag_cofficient * velocityn.LengthSquared) * vnormal;
                    father.mass[i[0], j[0]].applyforce(fd);
                    father.mass[i[1], j[1]].applyforce(fd);
                    father.mass[i[2], j[2]].applyforce(fd);
                    //if (float.IsNaN(fd.Length) || fd.Length > 0.002)
                    //{
                    //    Console.WriteLine("f " + fd.LengthFast + " " + fd.ToString());
                    //}
                }
                //if (fd.LengthFast > 0.01)
                //    Console.WriteLine("f " + fd.LengthFast + " " + fd.ToString());
                //if (velocity.LengthFast > 0.1)
                //    Console.WriteLine("v " + velocity.LengthFast + " " + velocity.ToString());
            }
        }

        #region draw
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
            Vector3[] data;
            public frameNormalizer(Cloth father)
            {
                this.father = father;
                data = new Vector3[father.Count];
            }

            public override Vector3[] getData()
            {
                Vector3[] data = new Vector3[father.Count];
                Fill(data, Vector3.Zero);
                for (int i = 0; i < father.rows - 1; i++)
                    for (int j = 0; j < father.cols - 1; j++)
                    {
                        Vector3 norm = Vector3.Cross((father.mass[i + 1, j + 1].location - father.mass[i, j].location),
                                                    (father.mass[i, j + 1].location - father.mass[i, j].location));
                        data[i * father.cols + j] += norm;
                        data[(i + 1) * father.cols + j + 1] += norm;
                        data[i * father.cols + j + 1] += norm;

                        norm = Vector3.Cross((father.mass[i + 1, j].location - father.mass[i, j].location),
                                             (father.mass[i + 1, j + 1].location - father.mass[i, j].location));
                        data[i * father.cols + j] += norm;
                        data[(i + 1) * father.cols + j] += norm;
                        data[(i + 1) * father.cols + j + 1] += norm;                        
                    }
                for (int i = 0; i < data.Length;i++ )
                {
                    data[i].Normalize();
                }
                //data.AsParallel().ForAll(norm => norm.Normalize());
                return data;
            }

            public Vector3[] getNormals() { return data; }

            private static void Fill(Vector3[] outarray, Vector3 value)
            {
                for (int i = 0; i < outarray.Length; i++)
                    outarray[i] = value;
            }
        }
        frameNormalizer normalizer;

        public Textureer getFrameTextreer()
        {
            if (textureer == null)
                textureer = new frameTextureer(this, Util.TexUtil.CreateTextureFromFile("tex/soft_fabric.jpg"));
            return textureer;
        }
        private class frameTextureer : Textureer
        {
            Cloth father;
            Vector2[] data;
            int texbuf;
            public frameTextureer(Cloth father, int texbuf)
            {
                this.father = father;
                this.texbuf = texbuf;
                data = new Vector2[father.Count];
                int idx=0;
                for (int i = 0; i < father.rows; i++)
                {
                    float v = (float)(i) / (father.rows-1);
                    for (int j = 0; j < father.cols; j++, idx++)
                    {
                        data[idx] = new Vector2((float)(j) / (father.cols-1), v);
                    }
                }
            }

            public override int getTexture()
            {
                return texbuf;
            }

            public override Vector2[] getData()
            {
                return data;
            }
        }
        frameTextureer textureer;

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
            public Face[] faces;
            public frameElementer(Cloth father)
            {
                this.father = father;
            }

            public override short[] getData()
            {
                if (data != null)
                    return data;
                data = new short[(father.rows - 1) * (father.cols - 1) * 6];
                faces = new Face[(father.rows - 1) * (father.cols - 1) * 2];
                int idx = 0;
                for (short i = 0; i < father.rows-1; i++)
                    for (short j = 0; j < father.cols-1; j++)
                    {
                        data[idx++] = (short)(i * father.cols + j);
                        data[idx++] = (short)((i + 1) * father.cols + j + 1);
                        data[idx++] = (short)(i * father.cols + j + 1);
                        faces[idx / 3 - 1] = new Face(this.father, new short[] { i, (short)(i + 1), i }, new short[] { j, (short)(j + 1), (short)(j + 1) });

                        data[idx++] = (short)(i * father.cols + j);
                        data[idx++] = (short)((i + 1) * father.cols + j);
                        data[idx++] = (short)((i + 1) * father.cols + j + 1);
                        faces[idx / 3 - 1] = new Face(this.father, new short[] { i, (short)(i + 1), (short)(i + 1) }, new short[] { j, j, (short)(j + 1) });
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

        
        #endregion
        
        public void move(Vector3 mv)
        {
            mass[mass.GetLength(0) - 1, 0].location += mv;
            mass[mass.GetLength(0) - 1, mass.GetLength(1) - 1].location += mv;
        }

        public float VelocityEnergy()
        {
            float energy = 0;
            foreach (Mass ma in mass)
            {
                energy += 0.5f * ma.m * ma.velocity.LengthSquared;
                //if (float.IsNaN(energy))
                //    Console.WriteLine("f energy = " + energy);
            }
            return energy;
        }
    }

    class Mass
    {
        public float m;
        public Vector3 location;
        Vector3 force;
        public Vector3 velocity;
        public bool pinned { set; get; }
        float lift = 0.99f;
        public Mass(float m, Vector3 location, bool pinned=false)
        {
            this.m = m;
            this.location = location;
            this.pinned = pinned;
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
                //Vector3 a = force * (lift / m);
                //if (float.IsNaN(a.X))
                //    Console.WriteLine("f a = " + a);
                velocity += force * (lift / m * dt);
                //if (velocity.LengthFast * dt > 0.01)
                //    Console.WriteLine("f: " + force.ToString() + " v: " + velocity + " s: " + velocity.LengthFast * dt);
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
        public Spring(Mass p1, Mass p2, float k, float r,float friction=1.0f)
        {
            this.p1 = p1;
            this.p2 = p2;
            this.k = k;
            this.r = r;
            this.friction = friction;
        }

        public void solve(){
            Vector3 vec = p1.location - p2.location;
            Vector3 nvec = vec.Normalized();
            float len = vec.Length;
            Vector3 force;
            if (!MathUtil.isZero(len))
            {
                Vector3 fvec = nvec * (r - len);
                force = fvec * k;
                //if (float.IsNaN(force.Length))
                //{
                //    Console.WriteLine("f " + force.LengthFast + " " + force.ToString());
                //}
                //if (float.IsNaN(nvec.Length))
                //{
                //    Console.WriteLine("f " + nvec.LengthFast + " " + nvec.ToString());
                //}
                p1.applyforce(force - Vector3.Dot(p1.velocity, nvec) * Math.Abs(r - len) * nvec);
                p2.applyforce(-force - Vector3.Dot(p2.velocity, nvec) * Math.Abs(r - len) * nvec);
            }
        }
    }

    class MathUtil
    {
        public static bool isZero(float x)
        {
            return Math.Abs(x) < 1e-10;
        }
    }
}
