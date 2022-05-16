using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FontAwesome.Sharp;


namespace CapaPresentacion.Class
{
    public class CustomColorTable : ProfessionalColorTable
    {
        //Color del borde del menu principal
        public override Color MenuItemBorder => Color.LightGray;


        //Color cuando paso por encima del menu principal con el mouse
        public override Color MenuItemSelectedGradientBegin => Color.FromArgb(80, 80, 80);
        public override Color MenuItemSelectedGradientEnd => Color.FromArgb(80, 80, 80);


        //Color del menu principal cuando lo presiono con el mouse
        public override Color MenuItemPressedGradientBegin => Color.FromArgb(28, 28, 28);
        public override Color MenuItemPressedGradientEnd => Color.FromArgb(80, 80, 80);


        //Color que le pone al submenu cuando paso por encima el mouse
        public override Color MenuItemSelected => Color.FromArgb(80, 80, 80);



    }
}
