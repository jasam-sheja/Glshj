﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestGljsh
{
    class MainTester
    {

        [STAThread]
        public static void Main()
        {
            //using (Cloth_3.ClothTest example = new Cloth_3.ClothTest())
            //{
            //    example.Run(60.0, 0.0);
            //}
            //using (Cloth_2.ClothTest example = new Cloth_2.ClothTest())
            //{
            //    example.Run(60.0, 0.0);
            //}
            //using (Test2 example = new Test2())
            //{
            //    example.Run(30.0, 0.0);
            //}
            using (ClothTest example = new ClothTest())
            {
                example.Run(60.0, 0.0);
            }
            //using (Test1 example = new Test1())
            //{
            //    example.Run(30.0, 0.0);
            //}
            //using (cubetest example = new cubetest())
            //{
            //    example.Run(30.0, 0.0);
            //}
        }

    }
}
