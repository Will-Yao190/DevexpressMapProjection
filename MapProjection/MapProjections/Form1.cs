using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.XtraMap;

namespace MapProjections {
    public partial class Form1 : Form {
        const string filepath = @"..\..\Data\Countries.shp";
        List<ProjectionBase> mapProjections = new List<ProjectionBase>() {
            new BraunStereographicProjection(),
            new EllipticalMercatorProjection(),
            new EqualAreaProjection(),
            new EquidistantProjection(),
            new EquirectangularProjection(),
            new KavrayskiyProjection(),
            new LambertCylindricalEqualAreaProjection(),
            new MillerProjection(),
            new SinusoidalProjection(),
            new SphericalMercatorProjection(),
            new NAFLambertAzimuthalEqualAreaProjection()
        };

        GeoMapCoordinateSystem CoordinateSystem { get { return mapControl.CoordinateSystem as GeoMapCoordinateSystem; } }

        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            lbProjection.DataSource = mapProjections;
            lbProjection.SetSelected(0, true);

            Uri baseUri = new Uri(System.Reflection.Assembly.GetEntryAssembly().Location);         
            Uri uri = new Uri(baseUri, filepath);
            mapControl.Layers.Add(new VectorItemsLayer() {
                Data = new ShapefileDataAdapter() {
                    FileUri = uri
                }
            });
        }

        private void lbProjection_SelectedIndexChanged(object sender, EventArgs e) {
            CoordinateSystem.Projection = lbProjection.SelectedValue as ProjectionBase;
        }
    }
}
