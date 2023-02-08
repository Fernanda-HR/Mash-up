using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pt2pt
{
    public class Scene
    {
        public List<Figure> Figures;
        public List<Figure> RepFigures;

        public Scene()
        {
            Figures = new List<Figure>();
            RepFigures = new List<Figure>();
        }
    
    }
}
