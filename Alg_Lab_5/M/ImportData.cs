﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;

namespace Alg_Lab_5.M
{
    public static class ImportData
    {
        public static int SizeNodeGraph = 40;
        public static SolidColorBrush ColorFillNodeGraph = (SolidColorBrush)new BrushConverter().ConvertFrom("#B0E0E6");
        public static SolidColorBrush ColorStrokeNodeGraph = (SolidColorBrush)new BrushConverter().ConvertFrom("#5F9EA0");
        public static SolidColorBrush ColorStrokeSelectedNodeGraph = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFFF00");
        public static SolidColorBrush ColorForeGroundTextGraph = (SolidColorBrush)new BrushConverter().ConvertFrom("#000000");
        public static SolidColorBrush ColorBackground = (SolidColorBrush)new BrushConverter().ConvertFrom("#f0f8ff");
        public static SolidColorBrush ColorStrokeRectangleOnEdgeGraph = (SolidColorBrush)new BrushConverter().ConvertFrom("#AFEEEE");
        public static SolidColorBrush ColorFillRectangleOnEndeGraph = (SolidColorBrush)new BrushConverter().ConvertFrom("#E0FFFF");
        public static Dictionary<string, string> Moods = new Dictionary<string, string>() { { "Standart", "Стандартный" }, {"Nodes", "Редактирование узлов" }, { "Edges", "Редактирование ребер" } };
    }
}
